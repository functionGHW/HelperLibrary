using System.Data;

namespace HelperLibrary.Core.Db
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(object param);

        IDbDataAdapter CreateDataAdapter();
    }
}
