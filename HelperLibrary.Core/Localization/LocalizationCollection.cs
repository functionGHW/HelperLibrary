/* 
 * FileName:    LocalizationCollection.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/15/2015 1:43:29 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// a helper class for xml serialization.
    /// </summary>
    [XmlType("LocalizationCollection")]
    public class LocalizationCollection
    {
        public LocalizationCollection()
        {
            this.Items = new List<StringItem>();
        }

        /// <summary>
        /// the CultureName attribute
        /// </summary>
        [XmlAttributeAttribute]
        public string CultureName { get; set; }

        /// <summary>
        /// the 
        /// </summary>
        [XmlElement(ElementName = "StringItem")]
        public List<StringItem> Items { get; set; }

        /// <summary>
        /// a helper method to add StringItem instance to the Items.
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(StringItem item)
        {
            this.Items.Add(item);
        }
    }
}
