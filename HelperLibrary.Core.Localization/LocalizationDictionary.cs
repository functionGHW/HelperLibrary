/* 
 * FileName:    LocalizationDictionary.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 8:50:31 PM
 * Version:     v1.1
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    /// <summary>
    /// 通用化本地化字典实现类，该类接受一个字典项集合作为数据来源
    /// </summary>
    public sealed class LocalizationDictionary : ILocalizationDictionary
    {
        private readonly string cultureName;
        private readonly IEnumerable<LocalizationItem> items;
        private readonly object syncObj = new object();
        private IDictionary<string, string> localizationItemsCache = null;

        /// <summary>
        /// 使用指定语言的数据源集合创建本地化字典
        /// </summary>
        /// <param name="cultureName">语言名称</param>
        /// <param name="items">字典项集合</param>
        public LocalizationDictionary(string cultureName, IEnumerable<LocalizationItem> items)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");
            if (items == null)
                throw new ArgumentNullException("items");

            this.cultureName = cultureName;
            this.items = items;
        }


        /// <summary>
        /// 字典对应的语言文化名称,格式应如"zh-CN"所示。
        /// </summary>
        public string CultureName
        {
            get { return cultureName; }
        }

        /// <summary>
        /// 根据参数获取key对应的本地化文本
        /// </summary>
        /// <param name="key">本地化文本对应的key</param>
        /// <param name="scope">用于区分词条的限定域参数</param>
        /// <returns>如果存在对应词条则返回其值，否则直接返回key</returns>
        public string Get(string key, string scope)
        {
            var dict = GetLocalizationItems();
            string dictKey = GetDictionaryKey(key, scope);
            string value = null;
            return dict.TryGetValue(dictKey, out value) ? value : key;
        }

        private IDictionary<string, string> GetLocalizationItems()
        {
            if (localizationItemsCache == null)
            {
                lock (syncObj)
                {
                    if (localizationItemsCache == null)
                    {
                        localizationItemsCache = CreateLocalizationItems();
                    }
                }
            }
            return localizationItemsCache;
        }

        private IDictionary<string, string> CreateLocalizationItems()
        {
            var dict = items.ToDictionary(item => GetDictionaryKey(item.Key, item.Scope), item => item.Value);
            return dict;
        }

        private string GetDictionaryKey(string key, string scope)
        {
            return scope + "||" + key;
        }
    }
}
