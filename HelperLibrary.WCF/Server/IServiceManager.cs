/* 
 * FileName:    IServiceManager.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 12:25:11
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;

namespace HelperLibrary.WCF.Server
{
    /// <summary>
    /// service instance manager
    /// </summary>
    public interface IServiceManager : IDisposable
    {
        /// <summary>
        /// when a service was opened
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceOpened;

        /// <summary>
        /// when a service is opening
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceOpening;

        /// <summary>
        /// when a service was closed
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceClosed;

        /// <summary>
        /// when a service is closing
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceClosing;

        /// <summary>
        /// when a service was faulted
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceFaulted;

        /// <summary>
        /// get service list
        /// </summary>
        IEnumerable<IService> Services { get; }

        /// <summary>
        /// open all services
        /// </summary>
        void OpenAll();
        
        /// <summary>
        /// open a service by name
        /// </summary>
        /// <param name="serviceName"></param>
        void OpenService(string serviceName);

        /// <summary>
        /// close all services
        /// </summary>
        void CloseAll();

        /// <summary>
        /// close a service by name
        /// </summary>
        /// <param name="serviceName"></param>
        void CloseService(string serviceName);
    }
}
