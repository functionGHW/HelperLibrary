/* 
 * FileName:    IProxyBuilder.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:44:29 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WCF.Proxy
{
    public interface IProxyBuilder
    {
        TService GetProxy<TService>() where TService : class;
    }
}
