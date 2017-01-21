/* 
 * FileName:    LocalizedDisplayAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/13/2015 10:45:32 AM
 * Version:     v1.0
 * Description: This class is obsolete
 * */

namespace HelperLibrary.Core.Localization.Annotations
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
                throw new ArgumentNullException(nameof(scope));
            
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
            string key = this.Name;
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return LocalizationUtility.GetString(this.scope, key, this.culture.Name);
        }

        #endregion
    }
}
