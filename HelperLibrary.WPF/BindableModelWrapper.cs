/* 
 * FileName:    BindableModelWrapper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/16/2015 11:23:19 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.WPF
{
    using HelperLibrary.Core.Annotation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BindableModelWrapper<TModel> : IDataErrorInfo, INotifyPropertyChanged
        where TModel : class
    {
        private TModel model;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public BindableModelWrapper(TModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return model == null ? string.Empty : model.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator BindableModelWrapper<TModel>(TModel value)
        {
            return new BindableModelWrapper<TModel>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator TModel(BindableModelWrapper<TModel> value)
        {
            return value.Model;
        }

        /// <summary>
        /// 
        /// </summary>
        public TModel Model
        {
            get { return model; }
            set { this.model = value; }
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string this[string propertyName]
        {
            get
            {
                if (this.model == null
                    || string.IsNullOrEmpty(propertyName))
                {
                    return string.Empty;
                }

                Contract.Ensures(this.model != null);
                return InternalUtility.ValidateDataHelper(typeof(TModel), propertyName, this.model);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// 
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

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
