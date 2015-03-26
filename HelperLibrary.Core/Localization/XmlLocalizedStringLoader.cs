/* 
 * FileName:    XmlLocalizedStringLoader.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 3:37:04 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
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
    /// a xml version implement of ILocalizedStringLoader
    /// </summary>
    public class XmlLocalizedStringLoader : ILocalizedStringLoader
    {
        // the name of folder that contains the localization resources.
        // this folder should be in the same folder the program was in.
        private readonly string localizationFolderName = "Localization";


        // define name of xml elements to use
        private const string RootName = "LocalizedStrings";
        private const string ItemName = "StringItem";
        private const string KeyName = "Key";
        private const string ValueName = "Value";


        #region ILocalizedStringReader Members

        /// <summary>
        /// read localized strings from file
        /// </summary>
        /// <param name="scope">the scope</param>
        /// <param name="cultureName">the culture name</param>
        /// <returns>a dictionary contains the localized strings, 
        /// or null if no string was found.</returns>
        public IDictionary<string, string> GetLocalizedDictionary(string scope, string cultureName)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            if (string.IsNullOrEmpty(cultureName))
                throw new ArgumentNullException("cultureName");

            /* the format of resources file name is "{scope}.{cultureName}.xml"
             * for example: FunctionGHW.en-US.xml
             * 
             * and the final path of the file should be like this:
             *  "Localization\en-US\FunctionGHW.en-US.xml"
            */
            string fileName = scope + "." + cultureName + ".xml";
            string filePath = Path.Combine(localizationFolderName, cultureName, fileName);

            // parse xml document object
            XDocument doc = LoadDocument(filePath);

            if (doc == null)
            {
                return null;
            }
            // parse xml to dictionary
            return ReadStrings(doc);
        }

        #endregion

        private XDocument LoadDocument(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                return XDocument.Load(filePath);
            }
            catch (XmlException)
            {
                // something wrong in the xml file.
                return null;
            }
        }

        private IDictionary<string, string> ReadStrings(XDocument doc)
        {
            Contract.Ensures(doc != null);
            Contract.Ensures(doc.Root != null);

            // the root element, its name must be the same as RootName
            XElement root = doc.Root;
            if (root.Name != RootName)
            {
                return null;
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            var children = root.Elements(ItemName);
            foreach (XElement child in children)
            {
                string key = GetAttributeOrSubnodeValue(child, KeyName);
                // if key is null(means not found) or is empty string, skip this
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                string value = GetAttributeOrSubnodeValue(child, ValueName);
                // if value is not found, skip this
                if (value == null)
                {
                    continue;
                }
                dict.Add(key, value);
            }
            // if nothing valid was added, return null instead of an empty dictionary
            if (dict.Count > 0)
            {
                return dict;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// get first attribute or child element by name.
        /// if both attribute and child element exist, you will get the value of attribute
        /// </summary>
        /// <param name="element">the xml elemnt</param>
        /// <param name="name">name of attribute or element</param>
        /// <returns>the value of attribute or child element, or null if neither is not found.</returns>
        private string GetAttributeOrSubnodeValue(XElement element, string name)
        {
            Contract.Ensures(element != null);
            Contract.Ensures(name != null && name.Length > 0);

            // find attribute
            var attr = element.Attribute(name);
            if (attr != null)
            {
                return attr.Value;
            }

            // if no attribute found, find child element
            var child = element.Element(name);
            if (child != null)
            {
                string value = child.Value;
                if (value != null)
                {
                    value = value.Trim();
                }
                return value;
            }
            // nothing was found, return null
            return null;
        }
    }
}
