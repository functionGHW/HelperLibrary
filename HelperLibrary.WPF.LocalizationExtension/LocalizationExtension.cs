/* 
 * FileName:    LocalizationExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 10:33:40 PM
 * Description:
 * */

using HelperLibrary.Core.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xaml;

namespace HelperLibrary.WPF.LocalizationExtension
{
    /// <summary>
    /// 提供本地化支持的扩展标记，依赖于ET.CommonLibrary.Localization
    /// </summary>
    public class LocalizationExtension : MarkupExtension
    {
        private static ILocalizationDictionaryManager locDictMngr = EmptyLocalizationDictionaryManager.Instance;

        /// <summary>
        /// 设置使用的本地化字典管理器，初始默认的实例为DoNothingLocalizationManager.Instance
        /// </summary>
        /// <param name="localizationDictionaryManager">本地化管理实例</param>
        public static void SetLocalizationDictionaryManager(ILocalizationDictionaryManager localizationDictionaryManager)
        {
            if (localizationDictionaryManager == null)
                throw new ArgumentNullException("localizationDictionaryManager");
            locDictMngr = localizationDictionaryManager;
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public LocalizationExtension()
        {
        }

        /// <summary>
        /// 使用指定的key值初始化实例
        /// </summary>
        /// <param name="key"></param>
        public LocalizationExtension(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// 用于区分词条的限定域参数
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 语言文化名称，例如zh-CN,en-US
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 本地化文本的key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Scope))
            {
                var rootObjProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
                if (rootObjProvider != null)
                {
                    Scope = rootObjProvider.RootObject.GetType().Assembly.GetName().Name;
                }
            }
            if (Culture == null)
            {
                Culture = CultureInfo.CurrentUICulture;
            }

            return locDictMngr.GetString(Key, Scope, Culture.Name);
        }
    }
}
