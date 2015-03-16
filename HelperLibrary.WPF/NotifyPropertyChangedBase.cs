/* 
 * FileName:    NotifyPropertyChangedBase.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/12/2015 2:26:38 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Notify when a property was changed by property's name
        /// </summary>
        /// <param name="propertyName">name of property whose value was changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            InternalOnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notify when a property was changed by an expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression">an expression that simply return the property's value.</param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propMember = propertyExpression.Body as MemberExpression;
            if (propMember == null)
            {
                throw new ArgumentException();
            }
            string propertyName = propMember.Member.Name;
            InternalOnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notify when a property was changed
        /// </summary>
        /// <param name="propertyName">name of property</param>
        private void InternalOnPropertyChanged(string propertyName)
        {
            Contract.Ensures(propertyName != null && propertyName.Length > 0);

            var theEvent = this.PropertyChanged;
            if (theEvent != null)
            {
                theEvent(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
