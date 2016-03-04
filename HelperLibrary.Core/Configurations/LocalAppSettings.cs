/* 
 * FileName:    LocalAppSettings.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/4/2016 5:42:28 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Configurations
{
    /// <summary>
    /// simply wrapper of ConfigurationManager.AppSettings
    /// </summary>
    public class LocalAppSettings : IAppSettings
    {
        public string this[string name] => Get(name);

        public virtual string Get(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }

        public virtual void Add(string name, string value)
        {
            ConfigurationManager.AppSettings.Add(name, value);
        }

        public virtual void Set(string name, string newValue)
        {
            ConfigurationManager.AppSettings.Set(name, newValue);
        }

        public virtual void Remove(string name)
        {
            ConfigurationManager.AppSettings.Remove(name);
        }
    }
}
