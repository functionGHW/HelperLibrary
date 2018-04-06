using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Db
{
    public class SqliteDbConnectionFactory : IDbConnectionFactory
    {
        private string connectionString;

        public SqliteDbConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
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
            return new SQLiteConnection(connectionString);
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter();
        }
    }
}
