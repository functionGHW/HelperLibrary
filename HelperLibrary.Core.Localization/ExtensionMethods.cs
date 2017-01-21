/* 
 * FileName:    ExtensionMethods.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 9:47:58 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 根据指定的参数，获取key对应的本地化文本。
        /// </summary>
        /// <param name="mngr"></param>
        /// <param name="key">本地化文本的key</param>
        /// <param name="scope">用于区分词条的限定域参数</param>
        /// <param name="cultureName">语言文化名称，例如zh-CN,en-US</param>
        /// <returns>如果存在对应的本地化文本则返回其值，其他情况返回key</returns>
        public static string GetString(this ILocalizationDictionaryManager mngr, string key, string scope, string cultureName)
        {
            if (mngr == null)
                throw new ArgumentNullException(nameof(mngr));

            var dict = mngr.Get(cultureName);
            return dict == null ? key : dict.Get(key, scope);
        }
    }
}
