/* 
 * FileName:    StringItem.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/15/2015 1:44:36 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization
{
    using System.Xml.Serialization;

    /// <summary>
    /// a helper class for xml serialization.
    /// </summary>
    [XmlType("StringItem")]
    public class StringItem
    {
        /// <summary>
        /// the msgid element
        /// </summary>
        [XmlElementAttribute]
        public string MsgId { get; set; }

        /// <summary>
        /// the msgstr element
        /// </summary>
        [XmlElementAttribute]
        public string MsgStr { get; set; }
    }
}
