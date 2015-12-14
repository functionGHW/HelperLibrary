/* 
 * FileName:    ProxyBuilder.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 2:55:47 PM
 * Version:     v1.0
 * Description:
 * */

using System;

namespace HelperLibrary.WCF.Proxy
{
    class ProxyBuilder: IProxyBuilder
    {
        public TService GetProxy<TService>() where TService : class
        {
            throw new NotImplementedException();
        }
    }
}
