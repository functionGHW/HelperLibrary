/* 
 * FileName:    ILocalizationDictionary.cs
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
    /// 本地化文本字典，抽象一种语言文化的本地化文本。
    /// </summary>
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// 字典对应的语言文化名称,格式应如"zh-CN"所示。
        /// </summary>
        string CultureName { get; }

        /// <summary>
        /// 根据参数获取key对应的本地化文本
        /// </summary>
        /// <param name="key">本地化文本对应的key</param>
        /// <param name="scope">用于区分词条的限定域参数</param>
        /// <returns>如果存在对应词条则返回其值，否则直接返回key</returns>
        string Get(string key, string scope);
    }
}
