/* 
 * FileName:    IConfigurationFile.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/18/2015 9:41:44 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// interface for configuration file
    /// </summary>
    public interface IConfigurationFile
    {
        /// <summary>
        /// Get or set value of configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <returns>value of configuration if success, otherwise null</returns>
        string this[string name] { get; set; }

        /// <summary>
        /// Check if exists configuraion with specific name.
        /// </summary>
        /// <param name="name">name of configuration.</param>
        /// <returns>true if the configuration exists, otherwise return false.</returns>
        bool ContainsConfiguration(string name);

        /// <summary>
        /// Get value of configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <returns>value of configuration if success, otherwise null</returns>
        string GetConfiguration(string name);

        /// <summary>
        /// Add a new configuration of specific name and value.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <param name="value">value of configuration</param>
        void AddConfiguration(string name, string value);

        /// <summary>
        /// Update value of specific configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <param name="newValue">new value of configuration</param>
        void UpdateConfiguration(string name, string newValue);

        /// <summary>
        /// Remove a configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        void RemoveConfiguration(string name);

        /// <summary>
        /// Save change to file
        /// </summary>
        void SaveChange();
        
        /// <summary>
        /// Get all configurations as a dictionary
        /// </summary>
        /// <returns>a dictionary contains all configurations if success, otherwise return null.</returns>
        IDictionary<string, string> ToDictionary();
    }
}
