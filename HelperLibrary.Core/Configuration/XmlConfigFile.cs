/* 
 * FileName:    XmlConfigFile.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/17/2015 10:38:37 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class XmlConfigFile : IConfigFile
    {

        private string m_fullPath;

        private ConfigFileMode m_fileMode;

        private readonly object docSyncObj = new object();

        private XDocument m_doc;

        private IDictionary<string, string> m_configDict;
        private bool isInitialized = false;
        private readonly object m_ConfigDictSyncObj = new object();

        /// <summary>
        /// all access to the dict should accorss this method
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetConfigDict()
        {
            if (!isInitialized)
            {
                lock (m_ConfigDictSyncObj)
                {
                    if (!isInitialized)
                    {
                        InitializeDict();
                        isInitialized = true;
                    }
                }
            }
            return m_configDict;

        }

        private void InitializeDict()
        {
            switch (m_fileMode)
            {
                case ConfigFileMode.Open:
                    if (!File.Exists(m_fullPath))
                    {
                        throw new FileNotFoundException("can not find the config file", m_fullPath);
                    }
                    this.ReadConfigurantions();
                    break;
                case ConfigFileMode.Create:
                    if (File.Exists(m_fullPath))
                    {
                        throw new IOException("File already exists.");
                    }
                    break;
                case ConfigFileMode.CreateNew:
                    if (File.Exists(m_fullPath))
                    {
                        using (var fs = File.OpenWrite(m_fullPath))
                        {
                            fs.SetLength(0);
                        }
                    }
                    break;
                case ConfigFileMode.OpenOrCreate:
                    if (File.Exists(m_fullPath))
                    {
                        this.ReadConfigurantions();
                    }
                    break;
            }
        }


        #region Constructors

        public XmlConfigFile(string filePath, ConfigFileMode fileMode,
            string rootName = null,
            string elementName = null,
            string configNameAttributeName = null,
            string configValueTagName = null)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            this.m_fullPath = Path.GetFullPath(filePath);
            this.m_fileMode = fileMode;
            this.m_configDict = new Dictionary<string, string>();

            this.RootName = rootName ?? "configurations";
            this.ElementName = elementName ?? "setting";
            this.ConfigKeyName = configNameAttributeName ?? "name";
            this.ConfigValueName = configValueTagName ?? "value";
        }

        #endregion

        #region Properties

        public string RootName { get; set; }

        public string ElementName { get; set; }

        public string ConfigKeyName { get; set; }

        public string ConfigValueName { get; set; }

        #endregion

        #region IConfigFile Members

        /// <summary>
        /// Gets the fullpath of the config file
        /// </summary>
        public string FileName
        {
            get { return m_fullPath; }
        }

        public bool ContainsConfig(string configName)
        {
            if (string.IsNullOrEmpty(configName))
                throw new ArgumentNullException("configName");

            var dict = this.GetConfigDict();
            Contract.Assert(dict != null);
            return dict.ContainsKey(configName);
        }

        public string GetConfig(string configName)
        {
            if (string.IsNullOrEmpty(configName))
                throw new ArgumentNullException("configName");

            return InternalGetConfig(configName);
        }

        public void AddOrUpdateConfig(string configName, string value)
        {
            if (string.IsNullOrEmpty(configName))
                throw new ArgumentNullException("configName");

            Contract.Ensures(this.ConfigDict != null);
            this.ConfigDict.Add(configName, value);
        }

        public bool RemoveConfig(string configName)
        {
            if (string.IsNullOrEmpty(configName))
                throw new ArgumentNullException("configName");

            throw new NotImplementedException();
        }

        public string this[string configName]
        {
            get
            {
                if (string.IsNullOrEmpty(configName))
                    throw new ArgumentNullException("configName");

                return InternalGetConfig(configName);
            }
            set
            {
                if (string.IsNullOrEmpty(configName))
                    throw new ArgumentNullException("configName");

                Contract.Assert(this.ConfigDict != null);
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets all config item as a dict and return it.
        /// </summary>
        /// <returns> an dict contains all config item if success, otherwise null</returns>
        public IDictionary<string, string> GetAllConfigs()
        {
            var dict = this.GetConfigDict();
            Contract.Assert(dict != null);
            return new Dictionary<string, string>(dict);
        }

        #endregion

        private string InternalGetConfig(string configName)
        {
            Contract.Ensures(!string.IsNullOrEmpty(configName));

            var dict = this.GetConfigDict();
            Contract.Assert(dict != null);
            string value = null;
            if (dict.TryGetValue(configName, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        private void InternalSetConfig(string configName, string value)
        {
            Contract.Ensures(!string.IsNullOrEmpty(configName));
            Contract.Ensures(value != null);

            var dict = this.GetConfigDict();

            lock (m_ConfigDictSyncObj)
            {
                if (m_doc == null)
                {
                    CreateNewFile(m_fullPath);
                }
                var root = m_doc.Root;
                if (dict.ContainsKey(configName))
                {
                    XElement config = root.Element(configName);
                    //config.Value
                }
                else
                {
                    root.Add(new XElement(this.ElementName, new XAttribute("name", configName),
                                    new XAttribute(this.ConfigValueName, value)));
                }
            }
        }

        private void CreateNewFile(string filePath)
        {
            XDocument doc = new XDocument();
            doc.Root.Name = RootName;
            doc.Save(filePath);
            this.m_doc = doc;
        }

        private XDocument LoadConfigDoc(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("can not find the config file", path);
            }
            var cfgFile = XDocument.Load(path);

            Contract.Ensures(cfgFile != null);
            Contract.Ensures(cfgFile.Root != null);
            var root = cfgFile.Root;
            if (root.Name != RootName)
            {
                throw new XmlException("format error! the root of configuration file must be " + RootName);
            }
            return cfgFile;
        }

        private void ReadConfigurantions()
        {
            var cfgFile = LoadConfigDoc(m_fullPath);

            Contract.Ensures(cfgFile != null);
            Contract.Ensures(cfgFile.Root != null);

            var root = cfgFile.Root;
            var configDict = new Dictionary<string, string>();
            foreach (var item in root.Elements(ElementName))
            {
                var nameAttr = item.Attribute(ConfigKeyName);
                if (nameAttr == null || string.IsNullOrWhiteSpace(nameAttr.Value))
                {
                    continue;
                }
                var valueAttr = item.Attribute(ConfigValueName);
                if (valueAttr == null)
                {
                    continue;
                }
                configDict.Add(nameAttr.Value, valueAttr.Value);
            }

            this.m_doc = cfgFile;
            this.m_configDict = configDict;
        }
    }
}
