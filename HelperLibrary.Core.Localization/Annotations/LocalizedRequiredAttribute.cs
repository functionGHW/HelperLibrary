/* 
 * FileName:    LocalizedRequiredAttribute.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 10:49:07 AM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.Core.Localization.Annotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Localization;

    /// <summary>
    /// A required attribute that implements localization.
    /// </summary>
    public class LocalizedRequiredAttribute : LocalizedValidationAttribute
    {

        /// <summary>
        /// Initialize LocalizedRequiredAttribute
        /// </summary>
        /// <param name="scope">the scope.Usaully a assembly name</param>
        /// <param name="cultureName">a culture for localization, for example "en-US".
        /// If not given, use current UI culture as default</param>
        /// <exception cref="ArgumentNullException">the scope is null or empty string.</exception>
        /// <exception cref="CultureNotFoundException">the cultureName is not right</exception>
        public LocalizedRequiredAttribute(string scope, string cultureName = null)
            : base(scope, cultureName)
        {
        }

        
        public bool AllowEmptyStrings { get; set; }

        /// <summary>
        /// Checks that the value of the required data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        /// <exception cref="ValidationException">The data field value was null.</exception>
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var stringValue = value as string;
            if (stringValue != null && !this.AllowEmptyStrings)
            {
                return stringValue.Trim().Length != 0;
            }
            return true;
        }
    }
}
