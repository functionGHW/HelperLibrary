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
        private readonly Configuration cfgFile;

        public LocalAppSettings()
        {
            this.cfgFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string this[string name] => Get(name);

        public virtual string Get(string name)
        {
            var cfgItem = cfgFile.AppSettings.Settings[name];
            return cfgItem?.Value;
        }

        public virtual void Add(string name, string value)
        {
            cfgFile.AppSettings.Settings.Add(name, value);
            cfgFile.Save(ConfigurationSaveMode.Minimal);
        }

        public virtual void Set(string name, string newValue)
        {
            var cfgItem = cfgFile.AppSettings.Settings[name];
            if (cfgItem == null)
                throw new ConfigurationErrorsException(
                    $"update configuration failed: no configuration named {name} found");

            cfgItem.Value = newValue;
            cfgFile.Save(ConfigurationSaveMode.Minimal);
        }

        public virtual void Remove(string name)
        {
            cfgFile.AppSettings.Settings.Remove(name);
            cfgFile.Save(ConfigurationSaveMode.Minimal);
        }
    }
}
