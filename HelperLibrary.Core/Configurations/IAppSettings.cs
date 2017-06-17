/* 
 * FileName:    IAppSettings.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/4/2016 5:37:10 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Configurations
{
    /// <summary>
    /// manager configurations of a application
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Get value by name
        /// </summary>
        /// <param name="name">the config name</param>
        /// <returns></returns>
        string this[string name] { get;}

        /// <summary>
        /// Get value by name
        /// </summary>
        /// <param name="name">the config name</param>
        /// <returns></returns>
        string Get(string name);

        /// <summary>
        /// Add new config
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Add(string name, string value);

        /// <summary>
        /// update value of config, throw exception when the config is not exist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newValue"></param>
        void Set(string name, string newValue);

        /// <summary>
        /// Remove a config.
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
    }
}
