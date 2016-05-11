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

namespace HelperLibrary.Core.Db
{
    public class DbOperationHelper
    {
        private IDbConnectionFactory connectionFactory;

        public DbOperationHelper(IDbConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            this.connectionFactory = connectionFactory;
        }

        public void ChangeConnectionFactory(IDbConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            connectionFactory = factory;
        }

        public DataSet ExecuteDataSet(string sql, IDictionary<string, object> parameters = null)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            using (var conn = connectionFactory.CreateConnection(null))
            {
                var command = conn.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.AddParameters(parameters);
                }
                
                DataSet result = command.ExecuteDataSet(connectionFactory.CreateDataAdapter());
                return result;
            }
        }
    }
}
