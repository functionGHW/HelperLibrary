/* 
 * FileName:    WcfClient.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/22/2016 2:31:54 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.WCF.Client
{
    public static class WcfClient
    {
        public static WcfClient<TService> Create<TService>(string endpointName, object callbackObject = null)
            where TService : class
        {
            var proxyBuilder = new ProxyBuilder(endpointName, callbackObject);
            return new WcfClient<TService>(proxyBuilder);
        }

        public static void RequestService<TService>(Action<TService> action, object callbackObject = null)
            where TService : class
        {
            var factory = ChannelFactoryMananger.Instance.GetFactory<TService>(callbackObject: callbackObject);
            TService proxy = factory.CreateChannel();
            using (proxy as IDisposable)
            {
                action.Invoke(proxy);
            }
        }

        public static TResult RequestService<TService, TResult>(Func<TService, TResult> action,
            object callbackObject = null)
            where TService : class
        {
            var factory = ChannelFactoryMananger.Instance.GetFactory<TService>(callbackObject: callbackObject);
            TService proxy = factory.CreateChannel();
            using (proxy as IDisposable)
            {
                TResult result = action.Invoke(proxy);
                return result;
            }
        }
    }

    public class WcfClient<TService> where TService : class
    {
        private readonly IProxyBuilder proxyBuilder;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyBuilder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WcfClient(IProxyBuilder proxyBuilder)
        {
            if (proxyBuilder == null)
                throw new ArgumentNullException(nameof(proxyBuilder));

            this.proxyBuilder = proxyBuilder;
        }

        /// <summary>
        /// 调用无返回值的WCF服务
        /// </summary>
        /// <typeparam name="TService">WCF契约的类型</typeparam>
        public void RequestService(Action<TService> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            TService service = GetProxy();

            using (service as IDisposable)
            {
                action(service);
            }
        }

        /// <summary>
        /// 调用有返回值的WCF服务
        /// </summary>
        /// <typeparam name="TService">WCF契约的类型</typeparam>
        /// <typeparam name="TResult">操作的返回值类型</typeparam>
        public TResult RequestService<TResult>(Func<TService, TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            TService service = GetProxy();

            using (service as IDisposable)
            {
                return action(service);
            }
        }


        private TService GetProxy()
        {
            return proxyBuilder.GetProxy<TService>();
        }
    }
}
