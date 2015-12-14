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
    public interface IServiceCaller
    {
        void CallService<TService>(Action<TService> action) where TService : class;

        void CallService<TService>(Action<TService> action, Action callBack) where TService : class;

        TReturn CallService<TService, TReturn>(Func<TService, TReturn> action) where TService : class;

        TReturn CallService<TService, TReturn>(Func<TService, TReturn> action, Action<TReturn> callBack) where TService : class;
    }
}
