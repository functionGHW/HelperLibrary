/* 
 * FileName:    DoNothingLocalizationManager.cs
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
    /// Null-Object模式，该类型用于避免null检查
    /// </summary>
    public sealed class DoNothingLocalizationManager : ILocalizationDictionaryManager
    {
        /// <summary>
        /// 获取DoNothingLocalizationManager的唯一实例
        /// </summary>
        public static readonly DoNothingLocalizationManager Instance = new DoNothingLocalizationManager();

        private DoNothingLocalizationManager()
        {
        }

        /// <summary>
        /// 什么都不做，直接返回key。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scope"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public string GetString(string key, string scope, string cultureName)
        {
            return key;
        }

        /// <summary>
        /// 此类型不支持此操作，未实现
        /// </summary>
        /// <param name="dictionary"></param>
        public void Add(ILocalizationDictionary dictionary)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 此类型不支持此操作，未实现
        /// </summary>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public bool Contains(string cultureName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 此类型不支持此操作，未实现
        /// </summary>
        /// <param name="cultureName"></param>
        public void Remove(string cultureName)
        {
            throw new NotSupportedException();
        }
    }
}
