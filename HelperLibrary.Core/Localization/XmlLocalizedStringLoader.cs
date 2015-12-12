﻿/* 
 * FileName:    XmlLocalizedStringLoader.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 3:37:04 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using IOAbstractions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// a xml version implement of ILocalizedStringLoader
    /// </summary>
    public class XmlLocalizedStringLoader : ILocalizedStringLoader
    {
        // the name of folder that contains the localization resources.
        // this folder should be in the same folder the program was in.
        private readonly string localizationFolderName = "Localization";

        private readonly IFileSystem fileSystem;

        #region Constructors

        public XmlLocalizedStringLoader()
            : this(new FileSystemWrapper())
        {
        }

        public XmlLocalizedStringLoader(IFileSystem fileSystem)
        {
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            this.fileSystem = fileSystem;
        }

        #endregion

        #region ILocalizedStringReader Members

        /// <summary>
        /// read localized strings from file
        /// </summary>
        /// <param name="scope">the scope</param>
        /// <param name="cultureName">the culture name</param>
        /// <returns>a dictionary contains the localized strings, 
        /// or null if no string was found.</returns>
        /// <exception cref="ArgumentNullException">scope and/or cultureName are null or empty string</exception>
        public IDictionary<string, string> GetLocalizedDictionary(string scope, string cultureName)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException(nameof(scope));

            if (string.IsNullOrEmpty(cultureName))
                throw new ArgumentNullException(nameof(cultureName));

            /* the format of resources file name is "{scope}.{cultureName}.xml"
             * for example: FunctionGHW.en-US.xml
             * 
             * and the final path of the file should be like this:
             *  "Localization\en-US\FunctionGHW.en-US.xml"
            */
            string fileName = scope + "." + cultureName + ".xml";
            string filePath = Path.Combine(this.localizationFolderName, cultureName, fileName);

            // parse xml document object
            XDocument doc = this.LoadDocument(filePath);

            if (doc == null)
            {
                return null;
            }
            // parse xml to dictionary
            return this.DecodeXml(doc);
        }

        #endregion

        private XDocument LoadDocument(string filePath)
        {
            if (!this.fileSystem.FileExists(filePath))
                return null;

            try
            {
                using (var fileStream = this.fileSystem.OpenRead(filePath))
                {
                    return XDocument.Load(fileStream);
                }
            }
            catch (XmlException)
            {
                // something wrong in the xml file.
                return null;
            }
        }

        private IDictionary<string, string> DecodeXml(XDocument doc)
        {
            Contract.Assert(doc != null);

            IDictionary<string, string> result;

            /* use a XmlSerializer to deserialize the xml document.
             */
            var serializer = new XmlSerializer(typeof(LocalizationCollection));
            LocalizationCollection collection;
            using (XmlReader reader = doc.CreateReader())
            {
                /* if this file can not be deserialized, let collection be null.
                 * this may because this xml file has a wrong format.
                 */
                if (serializer.CanDeserialize(reader))
                {
                    collection = serializer.Deserialize(reader) as LocalizationCollection;
                }
                else
                {
                    collection = null;
                }
            }

            // null means fail to deserialize.
            if (collection != null && collection.Items != null)
            {
                result = new Dictionary<string, string>(collection.Items.Count);
                foreach (var item in collection.Items)
                {
                    result.Add(item.MsgId, item.MsgStr);
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
