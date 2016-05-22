/* 
 * FileName:    ServiceTypeResolver.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 14:07:47
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelperLibrary.WCF.Server
{
    public sealed class ServiceTypeResolver : IServiceTypeResolver
    {
        private readonly List<Assembly> serviceAssemblys = new List<Assembly>();

        public void RegisterAssembly(Assembly assembly)
        {
            if (serviceAssemblys.Contains(assembly))
                return;

            serviceAssemblys.Add(assembly);
        }

        public Type GetServiceType(string typeName)
        {
            var t = Type.GetType(typeName, false);
            if (t != null)
                return t;

            foreach (var assembly in serviceAssemblys)
            {
                t = assembly.GetType(typeName, false);
                if (t != null)
                    return t;
            }
            throw new TypeLoadException("Loading service type failed, type name：" + typeName);
        }
    }
}
