using System.Data;

namespace HelperLibrary.Core.Db
{
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Gets or sets connection string to use
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// create a new connection instance
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// create a new DbDataAdapter instance
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter CreateDataAdapter();
    }
}
