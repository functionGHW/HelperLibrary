using System;
using System.Windows;
using System.Windows.Data;

namespace HelperLibrary.WPF.LocalizationExtension
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dp"></param>
        /// <param name="key"></param>
        /// <param name="scope"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetLocalizationBinding(this DependencyObject obj, 
            DependencyProperty dp, string key, string scope)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (dp == null) throw new ArgumentNullException(nameof(dp));

            var binding = LocalizationHelper.CreateLocalizationBinding(key, scope);
            BindingOperations.SetBinding(obj, dp, binding);
        }
    }
}
