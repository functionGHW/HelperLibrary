/* 
 * FileName:    ViewModelBase.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 2:25:06 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ViewModelBase : NotifyPropertyChangedBase, IDataErrorInfo
    {
        /// <summary>
        /// Initialize ViewModelBase
        /// </summary>
        public ViewModelBase()
        {
            thisType = this.GetType();
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <remarks>
        /// default value is an empty string ("").
        /// </remarks>
        public virtual string Error
        {
            get { return string.Empty; }
        }

        private Type thisType;

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public virtual string this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return string.Empty;
                }
                var prop = thisType.GetProperty(propertyName);

                if (prop == null)
                {
                    // no such a property
                    return string.Empty;
                }
                object value = prop.GetValue(this, null);
                var attrs = prop.GetCustomAttributes(typeof(ValidationAttribute), true)
                    as ValidationAttribute[];
                // validate the value and get errormessage
                var errors = from attr in attrs
                             where !attr.IsValid(value)
                             select attr.ErrorMessage;

                if (errors.Any())
                {
                    return errors.First();
                }
                return string.Empty;
            }
        }

        #endregion
    }
}
