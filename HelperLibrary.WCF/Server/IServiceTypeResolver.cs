/* 
 * FileName:    IServiceTypeResolver.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 12:22:35
 * Version:     v1.0
 * Description: 
 */

using System;
using System.Reflection;

namespace HelperLibrary.WCF.Server
{
    public interface IServiceTypeResolver
    {
        /// <summary>
        /// add an assembly that contains service-implement types.
        /// </summary>
        /// <param name="assembly"></param>
        void RegisterAssembly(Assembly assembly);


        /// <summary>
        /// Get service-implement type by service name.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Type GetServiceType(string serviceName);
    }
}