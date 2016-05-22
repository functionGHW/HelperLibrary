/* 
 * FileName:    ServiceEventArgs.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 12:22:35
 * Version:     v1.0
 * Description: 
 */

using System;

namespace HelperLibrary.WCF.Server
{
    public sealed class ServiceEventArgs : EventArgs
    {
        /// <summary>
        /// Get service name
        /// </summary>
        public string ServiceName { get; private set; }

        public ServiceEventArgs(string serviceName)
        {
            ServiceName = serviceName;
        }
    }
}