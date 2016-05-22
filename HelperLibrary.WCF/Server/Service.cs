/* 
 * FileName:    Service.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 12:22:35
 * Version:     v1.0
 * Description: 
 */

using System;
using System.ServiceModel;

namespace HelperLibrary.WCF.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class Service : ServiceHost, IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceImplement"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Service(Type serviceImplement)
            : base(serviceImplement)
        {
            this.Name = serviceImplement.Name;
        }

        public string Name { get; private set; }
    }
}