/* 
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
    public class DbOperationInvoker
    {
        private IDbConnectionFactory connectionFactory;

        public DbOperationInvoker(IDbConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public IDbConnectionFactory ConnectionFactory
        {
            get { return connectionFactory; }
        }

        public void ChangeConnectionFactory(IDbConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            connectionFactory = factory;
        }

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
