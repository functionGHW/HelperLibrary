﻿/* 
 * FileName:    LocalizedValidationAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 10:36:37 AM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.Core.Localization.Annotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Localization;

    /// <summary>
    /// Base class for implementing localization validation attribute.
    /// This implementation use HelperLibrary.Core.Localization.LocalizedStringManager
    /// to implement localization.
    /// </summary>
    public abstract class LocalizedValidationAttribute : ValidationAttribute
    {
        // fields for getting localized string
        private readonly string scope;
        private readonly string cultureName;

        /// <summary>
        /// Initialize LocalizedValidationAttribute
        /// </summary>
        /// <param name="scope">the scope.Usaully a assembly name</param>
        /// <param name="cultureName">a culture for localization, for example "en-US".
        ///     If not given, use current UI culture as default</param>
        /// <exception cref="ArgumentNullException">the scope is null or empty string.</exception>
        /// <exception cref="CultureNotFoundException">the cultureName is not right</exception>
        protected LocalizedValidationAttribute(string scope, string cultureName = null)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentNullException(nameof(scope));
            
            this.scope = scope;
            this.cultureName = cultureName;
        }
        
        /// <summary>
        /// get the localized format message string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            //Contract.Assert(lclStrMng != null);
            Contract.Assert(this.cultureName != null);

            /* the ErrorMessageString usually equals ErrorMessage property 
             * which you can specify when using this Attribute.
             */

            /* Both errorMessage and name will be localized. 
             * The errorMsaage usually use ValidationAttribute.ErrorMessageString property,
             * it usually equals ValidationAttribute.ErrorMessage property 
             * which you can specify when using this Attribute. The parameter name usually 
             * come from DisplayAttribute or the name of property to be validated by default.
             */
            string localizedMessage = LocalizationUtility.GetString(this.scope, this.ErrorMessage, this.cultureName);
            string localizedName = LocalizationUtility.GetString(this.scope, name, this.cultureName);

            return string.Format(this.cultureName, localizedMessage, localizedName);
        }
    }
}
