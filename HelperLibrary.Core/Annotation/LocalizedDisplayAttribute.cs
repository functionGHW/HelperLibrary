/* 
 * FileName:    LocalizedDisplayAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/13/2015 10:45:32 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Annotation
{
    using HelperLibrary.Core.Localization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A custom Attribute for localized
    /// </summary>
    [AttributeUsage(AttributeTargets.Method 
                    | AttributeTargets.Property 
                    | AttributeTargets.Field 
                    | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LocalizedDisplayAttribute : Attribute
    {
        private static readonly ILocalizedStringManager lclStrMng = LocalizedStringManager.Default;

        // fields for getting localized string
        private readonly string scope;
        private readonly CultureInfo culture;

        /// <summary>
        /// Initialize LocalizedDisplayAttribute
        /// </summary>
        /// <param name="scope">the scope.Usaully a assembly name</param>
        /// <param name="cultureName">a culture for localization, for example "en-US".
        /// If not given, use current UI culture as default</param>
        /// <exception cref="ArgumentNullException">the scope is null or empty string.</exception>
        /// <exception cref="CultureNotFoundException">the cultureName is not right</exception>
        public LocalizedDisplayAttribute(string scope, string cultureName = null)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            this.scope = scope;
            this.culture = cultureName != null ?
                CultureInfo.GetCultureInfo(cultureName) : CultureInfo.CurrentUICulture;
        }

        #region Properties

        public string Name { get; set; }

        #endregion

        #region Methods

        public string GetLocalizedName()
        {
            Contract.Assert(lclStrMng != null);

            string key = this.Name;
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return lclStrMng.GetLocalizedString(scope, key, this.culture.Name);
        }

        #endregion
    }
}
