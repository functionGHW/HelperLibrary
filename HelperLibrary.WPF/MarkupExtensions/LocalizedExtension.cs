/* 
 * FileName:    LocalizedExtension.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 2:59:01 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF.MarkupExtensions
{
    using HelperLibrary.Core.Localization;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Markup;
    using System.Xaml;

    /// <summary>
    /// xaml extension for getting localized string
    /// </summary>
    public class LocalizedExtension : MarkupExtension
    {
        private static readonly ILocalizedStringManager lclStrMng = LocalizedStringManager.Default;

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
            Contract.Assert(lclStrMng != null);

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

            return lclStrMng.GetLocalizedString(Scope, Key, Culture.Name);
        }
    }
}
