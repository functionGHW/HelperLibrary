/* 
 * FileName:    IServerCaller.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:38:12 PM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.WCF
{
    /// <summary>
    /// an interface for calling wcf services easily
    /// </summary>
    public interface IServiceCaller
    {
        /// <summary>
        /// Call a wcf service without return value.
        /// </summary>
        /// <typeparam name="TService">The type of contract interface</typeparam>
        /// <param name="action">action to do with the service</param>
        /// <param name="callback">callback method</param>
        void CallService<TService>(Action<TService> action, Action callback = null) where TService : class;
        
        /// <summary>
        /// Call a wcf service that has return value.
        /// </summary>
        /// <typeparam name="TService">The type of contract interface</typeparam>
        /// <typeparam name="TReturn">The tyoe of return value</typeparam>
        /// <param name="action">action to do with the service</param>
        /// <param name="callback">callback method</param>
        TReturn CallService<TService, TReturn>(Func<TService, TReturn> action, Action<TReturn> callback = null) where TService : class;
    }
}
