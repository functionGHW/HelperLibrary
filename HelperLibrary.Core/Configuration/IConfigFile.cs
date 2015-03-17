/* 
 * FileName:    IConfigFile.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/16/2015 5:06:01 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// an interface for read simply text config
    /// </summary>
    public interface IConfigFile
    {
        /// <summary>
        /// Gets the name of the config file
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// check if there is a config item with the configName exist
        /// </summary>
        /// <param name="configName"></param>
        /// <returns>true if exists, false if not exists</returns>
        bool ContainsConfig(string configName);

        /// <summary>
        /// Get a config item by name
        /// </summary>
        /// <param name="configName">name of config item</param>
        /// <returns>the value of the config item if success, otherwise null.</returns>
        string GetConfig(string configName);

        /// <summary>
        /// Add or update a config item by name.
        /// </summary>
        /// <param name="configName">name of config item</param>
        /// <param name="value">the new value</param>
        void AddOrUpdateConfig(string configName, string value);

        /// <summary>
        /// remove a config item from the file.
        /// </summary>
        /// <param name="configName">name of config item</param>
        /// <returns>true if success, otherwise false.</returns>
        bool RemoveConfig(string configName);

        /// <summary>
        /// Gets or sets the value of a config item by name.
        /// Note that, when the item of given name not exists,
        /// you get a null when try to get the value of that item 
        /// and you add a new config item of given name when you try to set the value of that item;
        /// You will update the value of the config item when you try to set value 
        /// and the item is already exists.
        /// </summary>
        /// <param name="configName">name of config item</param>
        /// <returns>the value of the config item if success, otherwise null.</returns>
        string this[string configName] { get; set; }

        /// <summary>
        /// Gets all config item as a dict and return it.
        /// </summary>
        /// <returns> an dict contains all config item if success, otherwise null</returns>
        IDictionary<string, string> GetAllConfigs();
    }
}
