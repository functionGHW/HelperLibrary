﻿/* 
 * FileName:    ILocalizationDictionaryManager.cs
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
    /// 抽象本地化字典管理器
    /// </summary>
    public interface ILocalizationDictionaryManager
    {
        /// <summary>
        /// 获取指定语言文化的本地化词典。
        /// </summary>
        /// <param name="cultureName">语言文化名称</param>
        /// <returns>如果对应的语言文化字典存在则返回该字典，否则返回null</returns>
        ILocalizationDictionary Get(string cultureName);


        /// <summary>
        /// 确定是否已存在指定语言文化的本地化字典，
        /// </summary>
        /// <param name="cultureName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>如果存在则返回true，否则返回false</returns>
        bool Contains(string cultureName);

        /// <summary>
        /// 添加新的本地化字典，若对应的语言文化字典已存在则抛出异常。
        /// </summary>
        /// <param name="dictionary">新的本地化字典</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Add(ILocalizationDictionary dictionary);

        /// <summary>
        /// 移除指定语言文化的本地化字典，如果不存在则直接返回。
        /// </summary>
        /// <param name="cultureName">指定的语言文化名称</param>
        /// <exception cref="ArgumentNullException">cultureName是null</exception>
        void Remove(string cultureName);
    }
}
