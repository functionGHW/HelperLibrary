/* 
 * FileName:    InternalUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/16/2015 11:41:33 AM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using HelperLibrary.Core.Annotation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// a hepler class for internal
    /// </summary>
    internal static class InternalUtility
    {
        /// <summary>
        /// a helper method to get error message of properties that has ValidationAttribute
        /// </summary>
        /// <param name="type">type that defines the property</param>
        /// <param name="propertyName">name of the property</param>
        /// <param name="instance">instance to validate</param>
        /// <returns>the error message string.</returns>
        internal static string ValidateDataHelper(Type type, string propertyName, object instance)
        {
            Contract.Requires(type != null);
            Contract.Requires(instance != null);
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            var prop = type.GetProperty(propertyName);
            if (prop == null)
            {
                // no such a property
                return string.Empty;
            }
            object value = prop.GetValue(instance, null);

            /* Here we set the propertyName as the context's MemberName so that 
             * we can support to format the error message.
             * 
             * If the property has a DisplayAttribute, the context's DisplayName property 
             * will find the DisplayAttribute by MemberName; otherwise the DisplayName 
             * is same as MemberName. If we don't give the MemverName, the name of 
             * the instance's type(the ViewModel) will be as the DisplayName.
             * 
             * For example: [LocalizedRequired(Scope, ErrorMessage="The {0} is required")]
             *          the argument "{0}" will be replace by the property's DisplayName
             */
            ValidationContext context = new ValidationContext(instance)
            {
                MemberName = propertyName,
            };

            var attrs = prop.GetCustomAttributes(typeof(ValidationAttribute), true)
                as ValidationAttribute[];

            /* validate the value and get all validation results that not valid
             * Note that instead of getting the ErrorMessage directly, 
             * we get the ValidationResult first, because some ValidationAttribute classes
             * may implement localization(for example HelperLibrary.Core.Annotation.*). 
             */
            var errors = from attr in attrs
                         where !attr.IsValid(value)
                         select attr.GetValidationResult(value, context);

            if (errors.Any())
            {
                return errors.First().ErrorMessage;
            }
            return string.Empty;
        }
    }
}
