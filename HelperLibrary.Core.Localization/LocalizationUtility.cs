/* 
 * FileName:    LocalizationUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 8:50:31 PM
 * Version:     v1.1
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Localization
{
    /// <summary>
    /// 本地化 辅助工具类
    /// </summary>
    public static class LocalizationUtility
    {
        // 当前使用的本地化字典管理器
        private static ILocalizationDictionaryManager currentLocalMngr = EmptyLocalizationDictionaryManager.Instance;

        /// <summary>
        /// 设置新的本地化字典管理器以供后续使用。
        /// </summary>
        /// <param name="localizationManager">新的本地化字典管理器对象</param>
        /// <exception cref="System.ArgumentNullException">localizationManager是null</exception>
        public static void SetLocalizationDictionaryManager(ILocalizationDictionaryManager localizationManager)
        {
            if (localizationManager == null) 
                throw new ArgumentNullException("localizationManager");

            currentLocalMngr = localizationManager;
        }

        /// <summary>
        /// 根据指定的参数，获取key对应的本地化文本。
        /// </summary>
        /// <param name="key">本地化文本的key</param>
        /// <param name="scope">用于区分词条的限定域参数</param>
        /// <param name="cultureName">语言文化名称，例如zh-CN,en-US，若为指定则使用系统默认的UI culrture</param>
        /// <returns>如果存在对应的本地化文本则返回其值，其他情况返回key</returns>
        public static string GetString(string key, string scope, string cultureName = null)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = CultureInfo.CurrentUICulture.Name;
            }
            return currentLocalMngr.GetString(key, scope, cultureName);
        }
    }
}
