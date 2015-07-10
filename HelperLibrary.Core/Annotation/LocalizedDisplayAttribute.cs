/* 
 * FileName:    LocalizedDisplayAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/13/2015 10:45:32 AM
 * Version:     v1.0
 * Description: This class is obsolete
 * */

namespace HelperLibrary.Core.Annotation
{
    using Localization;
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// A custom Attribute for localized
    /// </summary>
    [AttributeUsage(AttributeTargets.Method
                    | AttributeTargets.Property
                    | AttributeTargets.Field
                    | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LocalizedDisplayAttribute : Attribute
    {
        private readonly ILocalizedStringManager lclStrMng;

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
            : this(scope, LocalizedStringManager.Default, cultureName)
        {
        }

        /// <summary>
        /// Initialize LocalizedDisplayAttribute
        /// </summary>
        /// <param name="scope">the scope.Usaully a assembly name</param>
        /// <param name="lclStrMng">the localized string manager</param>
        /// <param name="cultureName">a culture for localization, for example "en-US".
        /// If not given, use current UI culture as default</param>
        /// <exception cref="ArgumentNullException">the scope is null or empty string,
        /// or lclStrMng is null.</exception>
        /// <exception cref="CultureNotFoundException">the cultureName is not right</exception>
        public LocalizedDisplayAttribute(string scope, ILocalizedStringManager lclStrMng, string cultureName = null)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            if (lclStrMng == null)
                throw new ArgumentNullException("lclStrMng");

            this.lclStrMng = lclStrMng;
            this.scope = scope;
            this.culture = cultureName != null
                ? CultureInfo.GetCultureInfo(cultureName)
                : CultureInfo.CurrentUICulture;
        }

        #region Properties

        public string Name { get; set; }

        #endregion

        #region Methods

        public string GetLocalizedName()
        {
            Contract.Assert(this.lclStrMng != null);

            string key = this.Name;
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return this.lclStrMng.GetLocalizedString(this.scope, key, this.culture.Name);
        }

        #endregion
    }
}
