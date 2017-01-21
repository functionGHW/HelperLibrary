/* 
 * FileName:    ILocalizationDictionaryFactory.cs
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
    /// 抽象创建本地化字典的工厂
    /// </summary>
    public interface ILocalizationDictionaryFactory
    {
        /// <summary>
        /// 创建指定的语言文化的本地化字典对象。
        /// </summary>
        /// <param name="cultureName">指定的语言文化名称，例如zh-CN</param>
        /// <returns></returns>
        ILocalizationDictionary CreateLocalizationDictionary(string cultureName);
    }
}
