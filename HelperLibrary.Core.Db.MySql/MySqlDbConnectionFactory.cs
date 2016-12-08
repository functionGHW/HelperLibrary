using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace HelperLibrary.Core.Db
{
    public class MySqlDbConnectionFactory : IDbConnectionFactory
    {
        private string connectionString;

        public MySqlDbConnectionFactory(string connectionString)
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
            return new MySqlConnection(connectionString);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }
    }
}
