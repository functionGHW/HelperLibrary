/* 
 * FileName:    ViewModelBase.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 2:25:06 PM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using System;
    using System.ComponentModel;

    public abstract class ViewModelBase : NotifyPropertyChangedBase, IDataErrorInfo
    {
        /// <summary>
        /// Initialize ViewModelBase
        /// </summary>
        protected ViewModelBase()
        {
            // this will always get the real type of this object
            this.thisType = this.GetType();
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <remarks>
        /// default value is an empty string ("").
        /// </remarks>
        public virtual string Error => string.Empty;

        private readonly Type thisType;

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
                return InternalUtility.ValidateDataHelper(this.thisType, propertyName, this);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Action<string, string> ShowMessgeDelegate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        protected void ShowMessage(string message, string title = "")
        {
            ShowMessgeDelegate?.Invoke(message, title);
        }
    }
}
