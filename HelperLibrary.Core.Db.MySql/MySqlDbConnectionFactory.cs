using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace HelperLibrary.Core.Db.MySql
{
    public class MySqlDbConnectionFactory : IDbConnectionFactory
    {
        public MySqlDbConnectionFactory()
        {
        }

        public IDbConnection CreateConnection(string connString)
        {
            return new MySqlConnection(connString);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
