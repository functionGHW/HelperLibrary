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
    /// <summary>
    /// Util for requesting WCF service
    /// </summary>
    public static class WcfClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="endpointName"></param>
        /// <param name="callbackObject"></param>
        /// <returns></returns>
        public static WcfClient<TService> Create<TService>(string endpointName = null, object callbackObject = null)
            where TService : class
        {
            var factory = ChannelFactoryMananger.Instance.GetFactory<TService>(callbackObject: callbackObject);
            return new WcfClient<TService>(factory, factory.Endpoint.Address);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="action"></param>
        /// <param name="callbackObject"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="callbackObject"></param>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class WcfClient<TService> where TService : class
    {
        private IChannelFactory<TService> factory;
        private EndpointAddress address;
        private Uri via;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="address"></param>
        /// <param name="via"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WcfClient(IChannelFactory<TService> factory, EndpointAddress address, Uri via = null)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            this.factory = factory;
            this.address = address;
            this.via = via;
        }

        /// <summary>
        /// 调用无返回值的WCF服务
        /// </summary>
        /// <param name="action"></param>
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
            return via == null ? factory.CreateChannel(address) : factory.CreateChannel(address, via);
        }
    }
}
