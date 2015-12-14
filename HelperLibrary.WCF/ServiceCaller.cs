/* 
 * FileName:    ServiceCaller.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:46:27 PM
 * Version:     v1.0
 * Description:
 * */


using System;
using HelperLibrary.WCF.Proxy;

namespace HelperLibrary.WCF
{
    /// <summary>
    /// an simply implementation of IServiceCaller
    /// </summary>
    public class ServiceCaller : IServiceCaller
    {
        private readonly IProxyBuilder proxyBuilder;

        /// <summary>
        /// Initialize an instance of ServiceCaller.
        /// </summary>
        public ServiceCaller()
            : this(new ProxyBuilder())
        { }

        /// <summary>
        /// Initialize an instance of ServiceCaller with a special instance of IProxyBuilder.
        /// </summary>
        /// <exception cref="ArgumentNullException">proxyBuilder is null</exception>
        public ServiceCaller(IProxyBuilder proxyBuilder)
        {
            if (proxyBuilder == null)
                throw new ArgumentNullException(nameof(proxyBuilder));

            this.proxyBuilder = proxyBuilder;
        }

        /// <summary>
        /// Call a wcf service without return value.
        /// </summary>
        /// <typeparam name="TService">The type of contract interface</typeparam>
        /// <param name="action">action to do with the service</param>
        /// <param name="callback">callback method</param>
        /// <exception cref="ArgumentNullException">the argument action is null</exception>
        public void CallService<TService>(Action<TService> action, Action callback = null) where TService : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var proxy = proxyBuilder.GetProxy<TService>();
            using (proxy as IDisposable)
            {
                action(proxy);
                callback?.Invoke();
            }
        }


        /// <summary>
        /// Call a wcf service that has return value.
        /// </summary>
        /// <typeparam name="TService">The type of contract interface</typeparam>
        /// <typeparam name="TReturn">The tyoe of return value</typeparam>
        /// <param name="action">action to do with the service</param>
        /// <param name="callback">callback method</param>
        /// <exception cref="ArgumentNullException">the argument action is null</exception>
        public TReturn CallService<TService, TReturn>(Func<TService, TReturn> action, Action<TReturn> callback = null) where TService : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var proxy = proxyBuilder.GetProxy<TService>();
            using (proxy as IDisposable)
            {
                var result = action(proxy);
                callback?.Invoke(result);
                return result;
            }
        }
    }
}