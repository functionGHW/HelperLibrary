/* 
 * FileName:    AnnotationHelper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/23/2015 3:25:06 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Annotation
{
    using HelperLibrary.Core.Localization;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AnnotationHelper
    {
        private static readonly ILocalizedStringManager lclStrMng = LocalizedStringManager.Default;

        /// <summary>
        /// a helper method for localizing error message and formating it when override
        /// ValidationAttribute.FormatErrorMessage(string name) method.
        /// </summary>
        /// <param name="errorMessage">usually use ErrorMessage property.</param>
        /// <param name="name">parameter that passed to method FormatErrorMessage</param>
        /// <param name="scope">scope. In most case, using name of assembly</param>
        /// <param name="culture">culture for localization</param>
        /// <returns></returns>
        internal static string GetLocalizedFormatErrorMessage(string errorMessage, string name,
            string scope, CultureInfo culture)
        {
            Contract.Assert(lclStrMng != null);
            Contract.Assert(scope != null);
            Contract.Assert(culture != null);
            Contract.Assert(errorMessage != null);
            Contract.Assert(name != null);

            /* Both errorMessage and name will be localized. 
             * The errorMsaage usually use ValidationAttribute.ErrorMessageString property,
             * it usually equals ValidationAttribute.ErrorMessage property 
             * which you can specify when using this Attribute. The parameter name usually 
             * come from DisplayAttribute or the name of property to be validated by default.
             */
            string localizedMessage = lclStrMng.GetLocalizedString(scope, errorMessage, culture.Name);
            string localizedName = lclStrMng.GetLocalizedString(scope, name, culture.Name);

            return String.Format(culture, localizedMessage, localizedName);
        }
    }
}
