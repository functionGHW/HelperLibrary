/* 
 * FileName:    ProxyBuilder.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:55:47 PM
 * Version:     v1.0
 * Description:
 * */

using System;

namespace HelperLibrary.WCF.Client
{
    /// <summary>
    /// a simply implementation of IProxyBuilder
    /// </summary>
    public class ProxyBuilder : IProxyBuilder
    {
        private readonly string endpointName;
        private readonly object callbackObject;

        public ProxyBuilder(string endpointName = null, object callbackObject = null)
        {
            this.endpointName = endpointName;
            this.callbackObject = callbackObject;
        }

        /// <summary>
        /// Get a service proxy from this builder
        /// </summary>
        /// <typeparam name="TService">The type of the service interface</typeparam>
        /// <returns>the proxy object to use</returns>
        public TService GetProxy<TService>() where TService : class
        {
            var factory = ChannelFactoryMananger.Instance.GetFactory<TService>(endpointName, callbackObject);
            return factory.CreateChannel();
        }
    }
}
