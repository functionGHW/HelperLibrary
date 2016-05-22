/* 
 * FileName:    ChannelFactoryMananger.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/22/2016 2:31:54 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace HelperLibrary.WCF.Client
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class ChannelFactoryMananger
    {
        /// <summary>
        /// 
        /// </summary>
        internal static ChannelFactoryMananger Instance { get; } = new ChannelFactoryMananger();

        private static readonly string[] DuplexBindingNames =
        {
            "WSDualHttpBinding",
            "NetTcpBinding"
        };

        private const string WcfConfigSectionName = "system.serviceModel/client";

        private readonly Lazy<List<ChannelEndpointElement>> endpointElements =
            new Lazy<List<ChannelEndpointElement>>(() =>
            {
                var clientSection =
                    ConfigurationManager.GetSection(WcfConfigSectionName) as ClientSection;
                if (clientSection == null)
                    throw new ConfigurationErrorsException("read configuration section failed: system.serviceModel/client");

                return clientSection.Endpoints.OfType<ChannelEndpointElement>().ToList();
            });


        private readonly Dictionary<string, ChannelFactory> factoryCache =
            new Dictionary<string, ChannelFactory>();

        private readonly object factoryCacheSyncObj = new object();


        private ChannelFactoryMananger()
        {
        }


        /// <summary>
        /// create channel factory
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="callbackObject">a callback object for duplex channel. For a simplex communication, it's value should be null.</param>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal ChannelFactory<TService> GetFactory<TService>(string endpointName = null,
            object callbackObject = null)
        {
            string contractName = typeof(TService).FullName;
            
            string key = GetCacheLKey(endpointName, contractName, callbackObject);

            ChannelFactory factory;
            if (!factoryCache.TryGetValue(key, out factory))
            {
                lock (factoryCacheSyncObj)
                {
                    if (!factoryCache.TryGetValue(key, out factory))
                    {
                        factory = CreateFactory<TService>(endpointName, callbackObject);
                        factoryCache.Add(key, factory);
                    }
                }
            }
            var result = (ChannelFactory<TService>)factory;
            return result;
        }

        private static string GetCacheLKey(string endpointName, string contractName, object callbackObject)
        {
            if (callbackObject == null)
                return $"{endpointName}|{contractName}";

            string callbackSegment = callbackObject.GetType().FullName + callbackObject.GetHashCode();
            return $"{endpointName}|{contractName}|{callbackSegment}";
        }

        private ChannelFactory<TService> CreateFactory<TService>(string endpointName,
            object callbackObject)
        {
            string contractName = typeof(TService).FullName;
            var ep = GetEndpointConfig(endpointName, contractName);
            ChannelFactory<TService> result;
            string binding = ep.Binding;
            if (IsDuplexBinding(binding) && callbackObject != null)
            {
                result = new DuplexChannelFactory<TService>(callbackObject, ep.Name);
            }
            else
            {
                result = new ChannelFactory<TService>(ep.Name);
            }

            return result;
        }

        private ChannelEndpointElement GetEndpointConfig(string endpointName, string contractName)
        {
            var list = endpointElements.Value;
            ChannelEndpointElement ep;

            if (list.All(item => item.Contract != contractName))
                throw new ConfigurationErrorsException(
                    $"No endpoint whose contract value equals {contractName}. Check your configuration file");

            /* When endpoint name is not given, it will search endpoint by contract name.
             * In this case, the endpoint with contract property that value equals endpointName should be unique, 
             * or there will be an exception.
             * 
             * If endpoint name was given, trying getting endpoint by name, and then ckeck the contract name.
             */
            ep = string.IsNullOrEmpty(endpointName)
                ? GetEndpointByContract(contractName)
                : GetEndpointByName(endpointName, contractName);
            return ep;
        }

        private ChannelEndpointElement GetEndpointByName(string endpointName, string contractName)
        {
            var list = endpointElements.Value;
            var ep = list.FirstOrDefault(item => item.Name == endpointName);
            if (ep == null)
                throw new ConfigurationErrorsException($"configuration of endpoint not found. Endpoint name: {endpointName}");

            if (ep.Contract != contractName)
            {
                string message = $"The contract of endpoint named {endpointName} is not matched: Expected type is {contractName}, but actual value is {ep.Contract}";
                throw new ConfigurationErrorsException(message);
            }
            return ep;
        }

        private ChannelEndpointElement GetEndpointByContract(string contractName)
        {
            var list = endpointElements.Value;
            if (list.Count(item => item.Contract == contractName) > 1)
                throw new ConfigurationErrorsException(
                    $"Multiple endpoint was found, which contract are {contractName}, pass a endpoint name for identifying or delete redundant endpoints");

            var ep = list.First(item => item.Contract == contractName);
            return ep;
        }

        private static bool IsDuplexBinding(string binding)
        {
            return DuplexBindingNames.Any(
                item => item.Equals(binding, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
