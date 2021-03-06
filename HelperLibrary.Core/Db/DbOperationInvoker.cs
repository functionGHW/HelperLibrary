﻿/* 
 * FileName:    DbHelper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  4/27/2016 5:16:01 PM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

namespace HelperLibrary.Core.Db
{
    /// <summary>
    /// 数据库操作执行器，封装简化对数据库的访问代码。
    /// </summary>
    public class DbOperationInvoker
    {
        private IDbConnectionFactory connectionFactory;

        public DbOperationInvoker(IDbConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// 获取当前使用的数据库连接工厂对象
        /// </summary>
        public IDbConnectionFactory ConnectionFactory
        {
            get { return connectionFactory; }
        }

        /// <summary>
        /// 设置新的数据库连接工厂对象
        /// </summary>
        /// <param name="factory"></param>
        public void ChangeConnectionFactory(IDbConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            connectionFactory = factory;
        }

        /// <summary>
        /// 执行查询命令,参数化查询的参数通过parameters传递。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string sql, IDictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            using (var conn = connectionFactory.CreateConnection())
            {
                var command = conn.CreateCommand(sql, cmdType, parameters);
                var adapter = connectionFactory.CreateDataAdapter();
                conn.Open();
                DataSet result = command.ExecuteDataSet(adapter);
                return result;
            }
        }

        /// <summary>
        /// 执行标量查询命令,参数化查询的参数通过parameters传递。
        /// </summary>
        /// <typeparam name="T">返回结果值的类型，必须使用和查询结果匹配的类型。</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, IDictionary<string, object> parameters = null,
            CommandType cmdType = CommandType.Text)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            using (var conn = connectionFactory.CreateConnection())
            {
                var command = conn.CreateCommand(sql, cmdType, parameters);
                conn.Open();
                T result = (T)command.ExecuteScalar();
                return result;
            }
        }

        /// <summary>
        /// 执行非查询命令,参数化查询的参数通过parameters传递。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="useTransaction"></param>
        /// <param name="cmdType"></param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, IDictionary<string, object> parameters = null,
            bool useTransaction = true, CommandType cmdType = CommandType.Text)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            TransactionScope ts = null;
            if (useTransaction)
                ts = new TransactionScope();

            int result;
            using (ts)
            {
                using (var conn = connectionFactory.CreateConnection())
                {
                    var command = conn.CreateCommand(sql, cmdType, parameters);
                    conn.Open();
                    result = command.ExecuteNonQuery();
                }
                ts?.Complete();
            }
            return result;
        }
    }
}
