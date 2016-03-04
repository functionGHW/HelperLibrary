/* 
 * FileName:    IDbContextFactory.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/10/2016 3:05:33 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Db
{
    public interface IDbContextFactory
    {
        IDbContext CreateDbContext(string param);
    }
}
