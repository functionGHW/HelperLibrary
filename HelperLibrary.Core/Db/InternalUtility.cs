/* 
 * FileName:    InternalUtility.cs
 * Author:      gaohongwei<gaohongwei@bjdaxiang.cn>
 * CreateTime:  2016/5/22 22:16:54
 * Description:
 * */

using HelperLibrary.Core.Annotation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Db
{
    internal static class InternalUtility
    {
        private static Dictionary<Type, Dictionary<string, Action<object, object>>> mapperCaches =
            new Dictionary<Type, Dictionary<string, Action<object, object>>>();

        private static readonly object SyncObj = new object();

        // 获取 类型的属性到数据列名的映射字典，如果不存在则会先创建。
        internal static Dictionary<string, Action<object, object>> GetEntityMapper(Type entityType)
        {
            Contract.Assert(entityType != null);
            Dictionary<string, Action<object, object>> mapper;
            if (!mapperCaches.TryGetValue(entityType, out mapper))
            {
                lock (SyncObj)
                {
                    if (!mapperCaches.TryGetValue(entityType, out mapper))
                    {
                        mapper = CreateEntityMapper(entityType);
                        mapperCaches.Add(entityType, mapper);
                    }
                }
            }
            return mapper;
        }

        /* 创建所有的公共属性和数据列名的映射集合。
         * 1.仅支持public属性。
         * 2.如果使用了NotMappedAttribute标签标记属性，则跳过。
         * 3.如果是使用了ColumnAttribute标签指定了Name，则使用此Name作为列名。
         * 4.其他的属性默认使用属性名作为映射的数据列名。
         */
        private static Dictionary<string, Action<object, object>> CreateEntityMapper(Type entityType)
        {
            var propertyMapper = new Dictionary<string, Action<object, object>>();
            foreach (var property in entityType.GetProperties())
            {
                if (property.GetCustomAttributes(typeof(NotMappedAttribute), true).Any())
                    continue;

                var colName = property.Name;
                var columnAttr =
                    property.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as
                        ColumnAttribute;
                if (columnAttr != null && !string.IsNullOrEmpty(columnAttr.Name))
                    colName = columnAttr.Name;

                propertyMapper.Add(colName, CreateSetterAction(property));
            }
            return propertyMapper;
        }

        // 构造 属性Setter方法的委托，该委托用于动态的调用以设置对象属性值。
        // 目前利用Expression预编译实现比直接反射更快的速度。
        private static Action<object, object> CreateSetterAction(PropertyInfo prop)
        {
            var setter = prop.SetMethod;
            ParameterExpression instanceParameter =
                Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter =
                Expression.Parameter(typeof(object[]), "parameters");

            var parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = setter.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }
            Expression instanceCast = setter.IsStatic
                ? null
                : Expression.Convert(instanceParameter, setter.ReflectedType);

            MethodCallExpression methodCall = Expression.Call(
                instanceCast, setter, parameterExpressions);

            var lambda = Expression.Lambda<Action<object, object[]>>(
                methodCall, instanceParameter, parametersParameter);

            Action<object, object[]> execute = lambda.Compile();
            return (instance, value) => { execute(instance, new[] { value }); };
        }


        internal static string GetUpdateStatement(Type entityType, string[] columns)
        {
            string tableName = GetTableName(entityType);
            string updateSegment = CreateUpdateSegment(columns);
            string[] keyNames = GetKeyNames(entityType);
            string conditionSegment = string.Join(" AND ", keyNames.Select(key => key + "=" + "@key_" + key));
            string updateStatement = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, updateSegment, conditionSegment);

            return updateStatement;
        }

        internal static Dictionary<string, object> CreateUpdateParameters(object entity, string[] columnsToUpdate, string[] keyNames)
        {
            var dict = new Dictionary<string, object>();
            var props = entity.GetType().GetProperties();
            foreach (string columnName in columnsToUpdate)
            {
                var prop = GetPropertyByColumn(props, columnName);
                object value = prop.GetValue(entity);

                dict.Add(CreateParameterKey(columnName), value);
            }

            foreach (string key in keyNames)
            {
                var prop = GetPropertyByColumn(props, key);
                object value = prop.GetValue(entity);

                dict.Add("@key_" + key, value);
            }
            return dict;
        }

        internal static string[] GetKeyNames(Type entityType)
        {
            return entityType.GetProperties()
                .Where(prop => prop.GetCustomAttribute(typeof(KeyAttribute)) != null)
                .Select(GetColumnName)
                .ToArray();
        }

        private static string CreateUpdateSegment(string[] columns)
        {
            return string.Join(",", columns.Select(col => col + "=@" + col));
        }


        private static ConcurrentDictionary<Type, string> insertStatementMapper = new ConcurrentDictionary<Type, string>();

        internal static string GetInsertStatement(Type entityType)
        {
            string insertStatement;
            if (!insertStatementMapper.TryGetValue(entityType, out insertStatement))
            {
                string tableName = GetTableName(entityType);
                var columnNames = GetColumnNames(entityType);

                string columnStr = string.Join(",", columnNames);
                string valueStr = string.Join(",", columnNames.Select(CreateParameterKey));

                insertStatement = string.Format("INSERT INTO {0} ({1}) VALUES({2})", tableName, columnStr, valueStr);
                insertStatementMapper.TryAdd(entityType, insertStatement);
            }
            return insertStatement;
        }

        private static string GetTableName(Type entityType)
        {
            string name = entityType.Name;

            var attr = entityType.GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault() as TableAttribute;
            if (attr != null && !string.IsNullOrEmpty(attr.Name))
            {
                name = attr.Name;
            }
            return name;
        }

        private static ConcurrentDictionary<Type, List<string>> columnNamesForInsertMapper =
            new ConcurrentDictionary<Type, List<string>>();

        internal static string[] GetColumnNames(Type entityType)
        {
            List<string> list;
            if (!columnNamesForInsertMapper.TryGetValue(entityType, out list))
            {
                list = new List<string>();
                foreach (var property in entityType.GetProperties())
                {
                    if (property.GetCustomAttributes(typeof(NotMappedAttribute), true).Any())
                        continue;

                    if (property.GetCustomAttributes(typeof(AutoGeneratedColumnAttribute), true).Any())
                        continue;

                    var colName = GetColumnName(property);

                    list.Add(colName);
                }
                columnNamesForInsertMapper.TryAdd(entityType, list);
            }
            return list.ToArray();
        }

        private static string GetColumnName(PropertyInfo property)
        {
            var colName = property.Name;
            var columnAttr =
                property.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as
                    ColumnAttribute;
            if (columnAttr != null && !string.IsNullOrEmpty(columnAttr.Name))
                colName = columnAttr.Name;
            return colName;
        }

        internal static Dictionary<string, object> CreateParameters(IEnumerable<string> columnNames, object entity)
        {
            var dict = new Dictionary<string, object>();
            var props = entity.GetType().GetProperties();
            foreach (string columnName in columnNames)
            {
                var prop = GetPropertyByColumn(props, columnName);
                object value = prop.GetValue(entity);

                dict.Add(CreateParameterKey(columnName), value);
            }
            return dict;
        }

        private static PropertyInfo GetPropertyByColumn(PropertyInfo[] props, string columnName)
        {
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Name))
                {
                    if (attr.Name == columnName)
                        return prop;
                }
                if (prop.Name == columnName)
                    return prop;
            }
            return null;
        }

        private static string CreateParameterKey(string columnName)
        {
            return "@" + columnName;
        }



        private static ConcurrentDictionary<Type, string> deleteStatementMapper = new ConcurrentDictionary<Type, string>();

        internal static string GetDeleteStatement(Type entityType)
        {
            string statement;
            if (!deleteStatementMapper.TryGetValue(entityType, out statement))
            {
                string tableName = GetTableName(entityType);
                string[] keyNames = GetKeyNames(entityType);
                string conditionSegment = string.Join(" AND ", keyNames.Select(key => key + "=" + "@" + key));

                statement = string.Format("DELETE FROM {0} WHERE {1}", tableName, conditionSegment);
                deleteStatementMapper.TryAdd(entityType, statement);
            }
            return statement;
        }

        internal static Dictionary<string, object> CreateDeleteParameters(object entity, string[] keyNames)
        {
            var dict = new Dictionary<string, object>();
            var props = entity.GetType().GetProperties();
            foreach (string key in keyNames)
            {
                var prop = GetPropertyByColumn(props, key);
                object value = prop.GetValue(entity);

                dict.Add(CreateParameterKey(key), value);
            }
            return dict;
        }
    }
}
