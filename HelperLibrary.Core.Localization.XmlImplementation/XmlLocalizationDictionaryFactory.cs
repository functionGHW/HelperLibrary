/* 
 * FileName:    XmlLocalizationDictionaryFactory.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 10:10:48 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HelperLibrary.Core.Localization
{
    /*
     * 此类型实现基于这样的一种XML文档格式：
     * 
     * <!-- LocalizationResource必须是根节点 -->
     * <LocalizationResource>  
     *      <!-- LocalizationDictionary表示一种语言的本地化字典,CultureName属性指定语言 -->
     *      <LocalizationDictionary CultureName="zh-CN"> 
     *          <!-- Scope表示其中的一个限定域，Name属性指定Scope的名称 -->
     *          <Scope Name="Scope1"> 
     *              <!-- 每个Item代表一个本地化的文本键值对，其中Key和Value可以用属性也可以单独的标签表示。 -->
     *              <!-- Value值是本地化本文，在程序中使用Key来获取Value值 -->
     *              <Item Key="hello" > 
     *                  <Value>你好!</Value>
     *              </Item>
     *              <Item>
     *                  <Key>the time is {0}</Key>
     *                  <Value>现在时间是{0}。</Value>
     *              </Item>
     *              ......
     *          </Scope>
     *          <!-- 另一个Scope -->
     *          <Scope Name="Scope2">
     *              ......
     *          </Scope>
     *          ......
     *      </LocalizationDictionary>
     *      <!-- 另一种语言的本地化字典 -->
     *      <LocalizationDictionary CultureName="en-US"> 
     *          ......
     *      </LocalizationDictionary>
     * </LocalizationResource>
     * 
     */

    /// <summary>
    /// 基于XML格式的一种本地化数据字典工厂类实现
    /// </summary>
    public class XmlLocalizationDictionaryFactory : ILocalizationDictionaryFactory
    {
        // define name of xml elements to use
        private const string RootTagName = "LocalizationResource";
        private const string DictionaryTagName = "LocalizationDictionary";
        private const string ScopeTagName = "Scope";
        private const string ItemTagName = "Item";
        private const string KeyName = "Key";
        private const string ValueName = "Value";


        private readonly XDocument xmlDocument;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        public XmlLocalizationDictionaryFactory(XDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw new ArgumentNullException("xmlDocument");
            this.xmlDocument = xmlDocument;
        }

        /// <summary>
        /// 创建指定的语言文化的本地化字典对象。
        /// </summary>
        /// <param name="cultureName">指定的语言文化名称，例如zh-CN</param>
        /// <exception cref="ArgumentNullException">cultureName为null</exception>
        /// <exception cref="FormatException">XML文档格式不正确，详细出错信息见异常的提示信息</exception>
        /// <returns></returns>
        public ILocalizationDictionary CreateLocalizationDictionary(string cultureName)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");

            var list = LoadLocalizationData(cultureName);
            return new LocalizationDictionary(cultureName, list);
        }

        /// <summary>
        /// 加载资源数据
        /// </summary>
        /// <param name="cultrueName"></param>
        /// <exception cref="FormatException">XML文档格式不正确，详细出错信息见异常的提示信息</exception>
        /// <returns></returns>
        private IEnumerable<LocalizationItem> LoadLocalizationData(string cultrueName)
        {
            // the root element, its name must be the same as RootName
            XElement root = xmlDocument.Root;
            if (root == null || root.Name != RootTagName)
                throw new FormatException("XML文档格式错误，根节点必须是" + RootTagName);

            var locDictionary = root.Elements(DictionaryTagName).FirstOrDefault(dict => IsCultureNameMatched(dict, cultrueName));

            if (locDictionary == null)
                throw new FormatException("XML文档中未找到CultureName=\"" + cultrueName + "\"的LocalizationDictionary节点");

            var list = new List<LocalizationItem>();
            var children = locDictionary.Elements(ScopeTagName);
            foreach (XElement scopeNode in children)
            {
                var attr = scopeNode.Attribute("Name");
                string scopeName = attr == null ? "" : attr.Value;
                foreach (var child in scopeNode.Elements(ItemTagName))
                {

                    string key = GetAttributeOrSubnodeValue(child, KeyName);
                    // if key is null(means not found) or is an empty string, skipping this one
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    string value = GetAttributeOrSubnodeValue(child, ValueName);
                    // if value is not found, skipping this one
                    if (value == null)
                    {
                        continue;
                    }
                    var newItem = new LocalizationItem(scopeName, key, value);
                    list.Add(newItem);
                }
            }
            return list;
        }

        private bool IsCultureNameMatched(XElement dict, string cultureName)
        {
            var attr = dict.Attribute("CultureName");

            return attr != null && attr.Value == cultureName;
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
            Contract.Assert(element != null);
            Contract.Assert(!string.IsNullOrEmpty(name));

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
                value = value.Trim();
                return value;
            }
            // nothing was found, return null
            return null;
        }
    }
}
