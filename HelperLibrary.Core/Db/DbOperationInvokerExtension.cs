/*----------------------------------------------------------------
// Copyright (C) 年份 北京大象科技有限公司
// 版权所有。 	
/* 
 * FileName:    DbOperationInvokerExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/27/2016 5:16:01 PM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Db
{
    /// <summary>
    /// 扩展DbOperationInvoker的扩展方法类，提供一些便利的ORM风格扩展方法。
    /// </summary>
    public static class DbOperationInvokerExtension
    {
        /// <summary>
        /// 查询全部结果并以实体集合的形式返回结果
        /// </summary>
        /// <param name="invoker"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEntity> QueryAll<TEntity>(this DbOperationInvoker invoker)
            where TEntity : class, new()
        {
            if (invoker == null)
                throw new ArgumentNullException(nameof(invoker));

            string sql = InternalUtility.GetSelectAllStatement(typeof(TEntity));
            return QueryMany<TEntity>(invoker, sql);
        }

        /// <summary>
        /// 执行sql查询并以实体集合的形式返回结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="invoker"></param>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="parameters">sql语句命名参数列表</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> QueryMany<TEntity>(this DbOperationInvoker invoker,
            string sql,
            IDictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text) where TEntity : class, new()
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");

            DataSet dataSet = invoker.ExecuteQuery(sql, parameters, cmdType);

            return dataSet.Tables[0].ToEntities<TEntity>();
        }

        /// <summary>
        /// 执行sql查询并按指定的形式把数据行映射到结果集
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invoker"></param>
        /// <param name="sql">sql语句</param>
        /// <param name="mapper">从数据行到结果对象的映射器委托</param>
        /// <param name="parameters">sql语句命名参数列表</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public static IEnumerable<TResult> QueryAndMap<TResult>(this DbOperationInvoker invoker,
            string sql,
            Func<DataRow, TResult> mapper,
            IDictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text)
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            DataSet dataSet = invoker.ExecuteQuery(sql, parameters, cmdType);
            var list = new List<TResult>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var item = mapper(row);
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 执行sql查询并返回结果中的第一个实体。如果没有任何结果，返回null。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="invoker"></param>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="parameters">sql语句命名参数列表</param>
        /// <returns>查询结果中的第一个实体对象，如果没有任何结果，返回null</returns>
        public static TEntity QueryOne<TEntity>(this DbOperationInvoker invoker,
            string sql,
            IDictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text) where TEntity : class, new()
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");

            DataSet dataSet = invoker.ExecuteQuery(sql, parameters, cmdType);

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.Tables[0].Rows[0].ToEntity<TEntity>();
            }
            return null;
        }

        /// <summary>
        /// 将实体对象写入数据库。该方法根据类型的定义生成sql并执行，可通过各种Attribute改变生成的sql。
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="entity">实体对象</param>
        /// <param name="useTransaction">是否启用事务</param>
        /// <typeparam name="TEntity">实体对象的类型</typeparam>
        /// <exception cref="ArgumentNullException">entity 为null</exception>
        /// <returns>受影响的数据行数</returns>        
        public static int InsertEntity<TEntity>(this DbOperationInvoker invoker, TEntity entity, bool useTransaction = true) where TEntity : class
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (entity == null)
                throw new ArgumentNullException("entity");

            Type entityType = typeof(TEntity);
            var insertStatement = InternalUtility.GetInsertStatement(entityType);
            string[] columnNames = InternalUtility.GetColumnNames(entityType);
            var parameters = InternalUtility.CreateParameters(columnNames, entity);

            return invoker.ExecuteNonQuery(insertStatement, parameters, useTransaction: useTransaction);
        }

        /// <summary>
        /// 更新数据实体的改动到数据库，注意该方法需要识别使用主键数据作为标识生成sql并执行操作。
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="entity">包含修改信息的实体对象</param>
        /// <param name="propsForUpdate">需要更新的对象属性名的数组，如果需要更新全部属性则可以传入null</param>
        /// <param name="useTransaction">是否启用事务</param>
        /// <typeparam name="TEntity">实体对象的类型</typeparam>
        /// <exception cref="ArgumentNullException">entity 为null</exception>
        /// <returns>受影响的数据行数</returns>
        public static int UpdateEntity<TEntity>(this DbOperationInvoker invoker, TEntity entity, string[] propsForUpdate = null, bool useTransaction = true) where TEntity : class
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (entity == null)
                throw new ArgumentNullException("entity");

            Type entityType = typeof(TEntity);
            string[] columnsToUpdate = propsForUpdate;
            if (propsForUpdate == null || propsForUpdate.Length == 0)
            {
                columnsToUpdate = InternalUtility.GetColumnNames(entityType);
            }
            string updateStatement = InternalUtility.GetUpdateStatement(entityType, propsForUpdate);
            var keyNames = InternalUtility.GetKeyNames(entityType);
            var parameters = InternalUtility.CreateUpdateParameters(entity, columnsToUpdate, keyNames);

            return invoker.ExecuteNonQuery(updateStatement, parameters, useTransaction: useTransaction);

        }

        /// <summary>
        /// 从数据库中删除指定实体对应的记录，注意该方法需要识别使用主键数据作为标识生成sql并执行操作
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="entity">包含要删除记录主键的实体对象</param>
        /// <param name="useTransaction">是否启用事务</param>
        /// <typeparam name="TEntity">实体对象的类型</typeparam>
        /// <exception cref="ArgumentNullException">entity 为null</exception>
        /// <returns>受影响的数据行数</returns>
        public static int DeleteEntity<TEntity>(this DbOperationInvoker invoker, TEntity entity, bool useTransaction = true) where TEntity : class
        {
            if (invoker == null)
                throw new ArgumentNullException("invoker");
            if (entity == null)
                throw new ArgumentNullException("entity");

            Type entityType = typeof(TEntity);
            string deleteStatement = InternalUtility.GetDeleteStatement(entityType);
            var keyNames = InternalUtility.GetKeyNames(entityType);
            var parameters = InternalUtility.CreateDeleteParameters(entity, keyNames);

            return invoker.ExecuteNonQuery(deleteStatement, parameters, useTransaction: useTransaction);
        }
    }
}
