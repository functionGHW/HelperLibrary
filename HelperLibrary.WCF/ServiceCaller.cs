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
    public class ServiceCaller : IServiceCaller
    {
        private readonly IProxyBuilder proxyBuilder;

        public ServiceCaller()
            : this(new ProxyBuilder())
        { }

        public ServiceCaller(IProxyBuilder proxyBuilder)
        {
            if (proxyBuilder == null)
                throw new ArgumentNullException(nameof(proxyBuilder));

            this.proxyBuilder = proxyBuilder;
        }

        public void CallService<TService>(Action<TService> action) where TService : class
        {
            this.CallService(action, null);
        }

        public void CallService<TService>(Action<TService> action, Action callBack) where TService : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var proxy = proxyBuilder.GetProxy<TService>();
            using (proxy as IDisposable)
            {
                action(proxy);
                callBack?.Invoke();
            }
        }

        public TReturn CallService<TService, TReturn>(Func<TService, TReturn> action) where TService : class
        {
            return this.CallService(action, null);
        }

        public TReturn CallService<TService, TReturn>(Func<TService, TReturn> action, Action<TReturn> callBack) where TService : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var proxy = proxyBuilder.GetProxy<TService>();
            using (proxy as IDisposable)
            {
                var result = action(proxy);
                callBack?.Invoke(result);
                return result;
            }
        }
    }
}