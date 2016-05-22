/* 
 * FileName:    IProxyBuilder.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:44:29 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WCF.Client
{
    /// <summary>
    /// an interface to abstract the creation of service proxy.
    /// </summary>
    public interface IProxyBuilder
    {
        /// <summary>
        /// Get a service proxy from this builder
        /// </summary>
        /// <typeparam name="TService">The type of the service interface</typeparam>
        /// <returns>the proxy object to use</returns>
        TService GetProxy<TService>() where TService : class;
    }
}
