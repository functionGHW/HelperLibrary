using System.Data;

namespace HelperLibrary.Core.Db
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string connString);

        IDbDataAdapter CreateDataAdapter();
    }
}
