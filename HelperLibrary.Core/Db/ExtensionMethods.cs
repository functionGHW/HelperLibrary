using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HelperLibrary.Core.Db
{
    /// <summary>
    /// Some extension methods for db operation.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Add many parameters for a DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        public static void AddParameters(this IDbCommand command,
            IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var param in parameters)
            {
                AddParameter(command, param.Key, param.Value);
            }
        }

        /// <summary>
        /// Add one parameter for a DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public static void AddParameter(this IDbCommand command, string paramName, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = value;
            command.Parameters.Add(param);
        }

        /// <summary>
        /// update many parameters of DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        public static void UpdateParameters(this IDbCommand command,
            IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var param in parameters)
            {
                UpdateParameter(command, param.Key, param.Value);
            }
        }

        /// <summary>
        /// Update one parameter of DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public static void UpdateParameter(this IDbCommand command, string paramName, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            command.Parameters[paramName] = value;
        }

        /// <summary>
        /// Execute a DbCommand with a DbDataAdapter,and return the data using DataSet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="adapter"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(this IDbCommand command, IDbDataAdapter adapter)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            var dataSet = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Create DbCommand
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string sql, CommandType cmdType = CommandType.Text,
            IDictionary<string, object> parameters = null)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = cmdType;

            if (parameters != null)
            {
                cmd.AddParameters((parameters));
            }
            return cmd;
        }

        /// <summary>
        /// 映射数据表中的所有数据行 到<typeparamref name="TEntity"/>集合中。仅支持public属性，
        /// 支持通过<see cref="ColumnAttribute"/>修改映射的列名和<see cref="NotMappedAttribute"/>跳过属性。
        /// </summary>
        /// <param name="dt"></param>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>包含全部实体对象的集合。如果数据表不包含任何行，该方法返回一个空集合而不是null</returns>
        public static IEnumerable<TEntity> ToEntities<TEntity>(this DataTable dt)
            where TEntity : class, new()
        {
            if (dt == null)
                throw new ArgumentNullException("dt");

            var propertyMapper = InternalUtility.GetEntityMapper(typeof(TEntity));
            var columnNames = dt.Columns.OfType<DataColumn>()
                .Select(item => item.ColumnName)
                .ToArray();

            columnNames = propertyMapper.Keys.Intersect(columnNames).ToArray();
            var list = new List<TEntity>();
            foreach (var row in dt.AsEnumerable())
            {
                TEntity entity = new TEntity();
                foreach (var name in columnNames)
                {
                    var setterDelegate = propertyMapper[name];
                    setterDelegate.Invoke(entity, row[name]);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 映射一个数据行 到 一个<typeparamref name="TEntity"/>对象中。仅支持public属性，
        /// 支持通过<see cref="ColumnAttribute"/>修改映射的列名和<see cref="NotMappedAttribute"/>跳过属性。
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="row"></param>
        /// <returns>映射成功后的实体对象</returns>
        public static TEntity ToEntity<TEntity>(this DataRow row) where TEntity : class, new()
        {
            if (row == null)
                throw new ArgumentNullException("row");

            var propertyMapper = InternalUtility.GetEntityMapper(typeof(TEntity));
            var columnNames = row.Table.Columns.OfType<DataColumn>()
                .Select(item => item.ColumnName)
                .ToArray();

            TEntity entity = new TEntity();
            foreach (var name in columnNames)
            {
                var setterDelegate = propertyMapper[name];
                setterDelegate.Invoke(entity, row[name]);
            }
            return entity;
        }
    }
}
