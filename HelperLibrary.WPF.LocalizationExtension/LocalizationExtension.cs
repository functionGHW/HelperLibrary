/* 
 * FileName:    LocalizationExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 10:33:40 PM
 * Description:
 * */

using System;
using System.Globalization;
using System.Windows.Markup;
using System.Xaml;

namespace HelperLibrary.WPF.LocalizationExtension
{
    /// <summary>
    /// 提供本地化支持的扩展标记，依赖于ET.CommonLibrary.Localization
    /// </summary>
    public class LocalizationExtension : MarkupExtension
    {
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

            var binding = LocalizationHelper.CreateLocalizationBinding(Key, Scope, Culture);
            return binding.ProvideValue(serviceProvider);
        }
    }
}
