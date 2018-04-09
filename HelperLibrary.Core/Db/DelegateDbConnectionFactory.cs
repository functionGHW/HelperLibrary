using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Db
{
    /// <summary>
    /// implement with delegates
    /// </summary>
    public class DelegateDbConnectionFactory : IDbConnectionFactory
    {
        private readonly Func<string, IDbConnection> createConnection;
        private readonly Func<IDbDataAdapter> createDataAdapter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createConnection">delegate for creating connection object using connection string</param>
        /// <param name="createDataAdapter">delegate for creating data adapter object</param>
        public DelegateDbConnectionFactory(Func<string, IDbConnection> createConnection, Func<IDbDataAdapter> createDataAdapter)
        {
            this.createConnection = createConnection ?? throw new ArgumentNullException(nameof(createConnection));
            this.createDataAdapter = createDataAdapter ?? throw new ArgumentNullException(nameof(createDataAdapter));
        }

        /// <summary>
        /// Gets or sets connection string to use
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// call delegate to create a new connection instance
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return createConnection(ConnectionString);
        }

        /// <summary>
        /// call delegate create a new DbDataAdapter instance
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return createDataAdapter();
        }
    }
}
