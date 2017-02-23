/* 
 * FileName:    SqlServerDbConnectionFactory.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/2/23 15:39:30
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Db
{
    public class SqlServerDbConnectionFactory : IDbConnectionFactory
    {
        private string connectionString;


        public SqlServerDbConnectionFactory(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            this.connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                connectionString = value;
            }
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
