using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using HelperLibrary.Core.Localization;

namespace HelperLibrary.WPF.LocalizationExtension
{
    /// <summary>
    /// provider localization text notification
    /// </summary>
    internal class LocalizationText : INotifyPropertyChanged
    {
        private readonly ILocalizationDictionaryManager dictManager;
        private string value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scope"></param>
        /// <param name="dictManager"></param>
        public LocalizationText(string key, string scope, ILocalizationDictionaryManager dictManager)
        {
            this.dictManager = dictManager ?? throw new ArgumentNullException(nameof(dictManager));
            Key = key;
            Scope = scope;
            Value = GetValue();

            LocalizationHelper.SubscribeCultureChanged(OnCultureChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get => value;
            private set
            {
                if (value == this.value)
                    return;

                this.value = value;
                OnPropertyChanged();
            }
        }

        private string GetValue()
        {
            return dictManager.GetString(Key, Scope, (Culture ?? CultureInfo.CurrentUICulture).Name);
        }

        private void OnCultureChanged(CultureInfo newCulture)
        {
            if (Equals(newCulture, Culture))
                return;

            Culture = newCulture;
            Value = GetValue();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
