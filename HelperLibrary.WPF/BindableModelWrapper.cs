/* 
 * FileName:    BindableModelWrapper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/16/2015 11:23:19 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public class BindableModelWrapper<TModel> : IDataErrorInfo, INotifyPropertyChanged
        where TModel : class
    {
        private TModel model;

        /// <summary>
        /// Initialize the instance by give object.
        /// </summary>
        /// <param name="model"></param>
        public BindableModelWrapper(TModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Returns a string that represents the model object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.model == null ? string.Empty : this.model.ToString();
        }

        /// <summary>
        /// implicit cast between BindableModelWrapper&lt;TModel&gt; and TModel
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator BindableModelWrapper<TModel>(TModel value)
        {
            return new BindableModelWrapper<TModel>(value);
        }

        /// <summary>
        /// explicit cast between BindableModelWrapper&lt;TModel&gt; and TModel
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator TModel(BindableModelWrapper<TModel> value)
        {
            return value.Model;
        }

        /// <summary>
        /// the wrapped model object.
        /// </summary>
        public TModel Model
        {
            get { return this.model; }
            set
            {
                this.model = value;
                this.OnPropertyChanged("Model");
            }
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">name of property</param>
        /// <returns>the error message if has errors, otherwise an empty string.</returns>
        public string this[string propertyName]
        {
            get
            {
                if (this.model == null
                    || string.IsNullOrEmpty(propertyName))
                {
                    return string.Empty;
                }

                Contract.Assert(this.model != null);
                return InternalUtility.ValidateDataHelper(typeof(TModel), propertyName, this.model);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Notify when a property was changed
        /// </summary>
        /// <param name="propertyName">name of property whose value was changed</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
