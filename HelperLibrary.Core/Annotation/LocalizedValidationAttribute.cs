/* 
 * FileName:    LocalizedValidationAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 10:36:37 AM
 * Version:     v1.1
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
    /// Base class for implementing localization validation attribute.
    /// This implementation use HelperLibrary.Core.Localization.LocalizedStringManager
    /// to implement localization.
    /// </summary>
    public abstract class LocalizedValidationAttribute : ValidationAttribute
    {
        //private static readonly ILocalizedStringManager lclStrMng = LocalizedStringManager.Default;

        // fields for getting localized string
        private readonly string scope;
        private readonly CultureInfo culture;

        /// <summary>
        /// Initialize LocalizedValidationAttribute
        /// </summary>
        /// <param name="scope">the scope.Usaully a assembly name</param>
        /// <param name="cultureName">a culture for localization, for example "en-US".
        /// If not given, use current UI culture as default</param>
        /// <exception cref="ArgumentNullException">the scope is null or empty string.</exception>
        /// <exception cref="CultureNotFoundException">the cultureName is not right</exception>
        public LocalizedValidationAttribute(string scope, string cultureName = null)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException("scope");

            this.scope = scope;
            this.culture = cultureName != null ?
                CultureInfo.GetCultureInfo(cultureName) : CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// get the localized format message string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            //Contract.Assert(lclStrMng != null);
            Contract.Assert(this.culture != null);
            
            /* the ErrorMessageString usually equals ErrorMessage property 
             * which you can specify when using this Attribute.
             */

            return AnnotationHelper.GetLocalizedFormatErrorMessage(this.ErrorMessageString, name, this.scope, this.culture);
        }
    }
}
