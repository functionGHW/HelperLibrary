/* 
 * FileName:    XmlConfigurationFile.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/18/2015 10:07:48 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Xml xonfiguration file
    /// </summary>
    public class XmlConfigurationFile : IConfigurationFile
    {
        #region Static members

        /* define tag and attribute using in XML file.
         */
        public static readonly string RootElementName = "configurations";
        public static readonly string ItemElementName = "setting";
        public static readonly string NameAttributeName = "name";
        public static readonly string ValueAttributeName = "value";

        /// <summary>
        /// a helper method to create an XML configuration file.
        /// This file only contains a empty root node.
        /// </summary>
        /// <param name="fullPath">full path to save the file</param>
        /// <returns></returns>
        private static XDocument CreateXmlFile(string fullPath)
        {
            XDocument doc = new XDocument(new XElement(RootElementName));
            doc.Save(fullPath);
            return doc;
        }

        #endregion

        #region Fields

        private readonly IDictionary<string, string> m_configurationDictionary;

        private readonly object m_dictSyncObj;

        private readonly XDocument m_doc;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize the instance using a file path.
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="isCreateNew">indicate whether to create a new file.
        /// True for creating new file, and false for loading exists file.</param>
        public XmlConfigurationFile(string filePath, bool isCreateNew = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath");

            string fullPath = Path.GetFullPath(filePath);

            bool fileExists = File.Exists(fullPath);
            if (fileExists && isCreateNew)
                throw new IOException("file already exists. " + fullPath);
            
            this.m_configurationDictionary = new Dictionary<string, string>();
            this.m_dictSyncObj = new object();
            this.FullPath = fullPath;

            if (isCreateNew)
            {
                this.m_doc = CreateXmlFile(fullPath);
            }
            else
            {
                this.m_doc = XDocument.Load(fullPath);
            }

            LoadAllConfigurations();
        }

        /// <summary>
        /// try to load all configurations from file.
        /// </summary>
        private void LoadAllConfigurations()
        {
            Contract.Ensures(this.m_configurationDictionary != null);
            Contract.Ensures(this.m_doc != null);

            lock (m_dictSyncObj)
            {
                var root = this.m_doc.Root;
                if (root == null
                    || root.Name != RootElementName)
                {
                    throw new InvalidDataException("the format of configuration file is not right");
                }

                foreach (var item in root.Elements(ItemElementName))
                {
                    var nameAttr = item.Attribute(NameAttributeName);
                    var valueAttr = item.Attribute(ValueAttributeName);
                    if (valueAttr == null)
                    {
                        continue;
                    }
                    if (nameAttr == null || string.IsNullOrEmpty(nameAttr.Value))
                    {
                        continue;
                    }
                    this.m_configurationDictionary.Add(nameAttr.Value, valueAttr.Value);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets full path of the current xml file
        /// </summary>
        public string FullPath { get; private set; }

        #endregion

        #region IConfigurationFile Members

        /// <summary>
        /// Get, add or update value of configuration by name.
        /// When add or update value, if the configuration with specific name not exist, 
        /// do adding, otherwise do updating.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <returns>value of configuration if success, otherwise null</returns>
        public string this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException("name");

                return InternalGetConfiguration(name);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException("name");

                if (value == null)
                    throw new ArgumentNullException("value");

                InternalAddOrUpdateConfiguration(name, value, ConfigOpt.AddOrUpdate);
            }
        }

        /// <summary>
        /// Check if exists configuraion with specific name.
        /// </summary>
        /// <param name="name">name of configuration.</param>
        /// <returns>true if the configuration exists, otherwise return false.</returns>
        public bool ContainsConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Contract.Ensures(m_configurationDictionary != null);

            return this.m_configurationDictionary.ContainsKey(name);
        }

        /// <summary>
        /// Get value of configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <returns>value of configuration if success, otherwise null</returns>
        public string GetConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            return InternalGetConfiguration(name);
        }

        /// <summary>
        /// Add a new configuration of specific name and value.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <param name="value">value of configuration</param>
        public void AddConfiguration(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (value == null)
                throw new ArgumentNullException("value");

            InternalAddOrUpdateConfiguration(name, value, ConfigOpt.Add);
        }

        /// <summary>
        /// Update value of specific configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        /// <param name="newValue">new value of configuration</param>
        public void UpdateConfiguration(string name, string newValue)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (newValue == null)
                throw new ArgumentNullException("value");

            InternalAddOrUpdateConfiguration(name, newValue, ConfigOpt.Update);
        }

        /// <summary>
        /// Remove a configuration by name.
        /// </summary>
        /// <param name="name">name of configuration</param>
        public void RemoveConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            InternalRemoveConfiguration(name);
        }

        /// <summary>
        /// Save change to file
        /// </summary>
        public void SaveChange()
        {
            Contract.Ensures(this.m_doc != null);
            lock (m_dictSyncObj)
            {
                this.m_doc.Save(FullPath);
            }
        }

        /// <summary>
        /// Get all configurations as a dictionary
        /// </summary>
        /// <returns>a dictionary contains all configurations if success, 
        /// otherwise return null.</returns>
        public IDictionary<string, string> ToDictionary()
        {
            Contract.Ensures(this.m_configurationDictionary != null);

            lock (m_dictSyncObj)
            {
                return new Dictionary<string, string>(this.m_configurationDictionary);
            }
        }

        #endregion

        private string InternalGetConfiguration(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Ensures(m_configurationDictionary != null);

            string value = null;
            if (this.m_configurationDictionary.TryGetValue(name, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        [Flags]
        internal enum ConfigOpt : byte
        {
            Add = 1,
            Update = 2,
            AddOrUpdate = 3,
        }

        private void InternalAddOrUpdateConfiguration(string name, string value, ConfigOpt opt)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(value != null);

            lock (m_dictSyncObj)
            {
                bool existsConfig = this.m_configurationDictionary.ContainsKey(name);
                switch (opt)
                {
                    case ConfigOpt.Add:
                        if (existsConfig)
                            throw new InvalidOperationException("configuration already exists");

                        AddConfigurationToXml(name, value);
                        break;
                    case ConfigOpt.Update:
                        if (!existsConfig)
                            throw new InvalidOperationException("configuration not found");

                        UpdateConfigurationToXml(name, value);
                        break;
                    case ConfigOpt.AddOrUpdate:
                        if (existsConfig)
                        {
                            UpdateConfigurationToXml(name, value);
                        }
                        else
                        {
                            AddConfigurationToXml(name, value);
                        }
                        break;
                }
                this.m_configurationDictionary[name] = value;
            }
        }

        private bool AddConfigurationToXml(string name, string value)
        {
            XElement root = this.m_doc.Root;
            XElement configruation = new XElement(ItemElementName,
                new XAttribute(NameAttributeName, name),
                new XAttribute(ValueAttributeName, value));

            root.Add(configruation);

            return true;
        }

        private bool UpdateConfigurationToXml(string name, string value)
        {
            var xmlValue = (from setting in m_doc.Root.Elements(ItemElementName)
                            let nameAttr = setting.Attribute(NameAttributeName)
                            let valueAttr = setting.Attribute(ValueAttributeName)
                            where nameAttr != null
                               && valueAttr != null
                               && nameAttr.Value == name
                            select valueAttr)
                            .SingleOrDefault();

            if (xmlValue != null)
            {
                xmlValue.Value = value;
                return true;
            }

            return false;
        }

        private void InternalRemoveConfiguration(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            lock (m_dictSyncObj)
            {
                if (this.m_configurationDictionary.ContainsKey(name))
                {
                    var configuration = (from setting in m_doc.Root.Elements(ItemElementName)
                                         let nameAttr = setting.Attribute(NameAttributeName)
                                         where nameAttr != null
                                            && nameAttr.Value == name
                                         select setting)
                                        .SingleOrDefault();
                    configuration.Remove();

                    this.m_configurationDictionary.Remove(name);
                }
            }

        }

    }
}
