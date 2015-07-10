/* 
 * FileName:    LocalizedExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 2:59:01 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF.MarkupExtensions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Markup;
    using System.Xaml;
    using Core.Localization;

    /// <summary>
    /// xaml extension for getting localized string
    /// This implementation use HelperLibrary.Core.Localization.LocalizedStringManager
    /// to implement localization.
    /// </summary>
    public class LocalizedExtension : MarkupExtension
    {
        private static readonly ILocalizedStringManager LclStrMng = LocalizedStringManager.Default;

        /// <summary>
        /// the scope
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// the culture of localized string to get
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// the key for searching localized string
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get localized string using the LocalizedStringManager implement.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>the localized string if successed, otherwise simply return the key string</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Contract.Assert(serviceProvider != null);
            Contract.Assert(LclStrMng != null);

            if (string.IsNullOrEmpty(this.Scope))
            {
                var rootObjProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
                if (rootObjProvider != null)
                {
                    this.Scope = rootObjProvider.RootObject.GetType().Assembly.GetName().Name;
                }
            }
            if (this.Culture == null)
            {
                this.Culture = CultureInfo.CurrentUICulture;
            }

            return LclStrMng.GetLocalizedString(this.Scope, this.Key, this.Culture.Name);
        }
    }
}
