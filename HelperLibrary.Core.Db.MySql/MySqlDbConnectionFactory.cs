using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace HelperLibrary.Core.Db.MySql
{
    public class MySqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string connStr;

        public MySqlDbConnectionFactory(string connStr)
        {
            if (connStr == null)
                throw new ArgumentNullException(nameof(connStr));

            this.connStr = connStr;
        }

        public IDbConnection CreateConnection(object param)
        {
            return new MySqlConnection(connStr);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
