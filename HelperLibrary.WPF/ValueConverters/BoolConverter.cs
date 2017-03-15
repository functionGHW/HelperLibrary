/* 
 * FileName:    BoolConverter.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2017/3/15 21:05:18
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HelperLibrary.WPF.ValueConverters
{
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return value;

            string paramStr = parameter as string;
            if (paramStr == null)
                return value;

            paramStr = paramStr.Trim();
            bool boolValue = (bool)value;

            if (paramStr.StartsWith(BoolConverterParameters.TextSwitchPrefix,
                StringComparison.InvariantCultureIgnoreCase))
            {
                char splitChar = '|';
                string[] textPairs = paramStr.Substring(BoolConverterParameters.TextSwitchPrefix.Length)
                    .Split(new[] { splitChar }, 2);
                if (textPairs.Length != 2)
                    throw new InvalidOperationException(string.Format("Parameter formater error! formatter:{0}text_for_ture{1}text_for_false",
                        BoolConverterParameters.TextSwitchPrefix, splitChar));
                
                return boolValue ? textPairs[0] : textPairs[1];
            }

            switch (paramStr.ToUpperInvariant())
            {
                case BoolConverterParameters.Reverse:
                    return !boolValue;
                case BoolConverterParameters.FalseAsHidden:
                    return boolValue ? Visibility.Visible : Visibility.Hidden;
                case BoolConverterParameters.TrueAsHidden:
                    return boolValue ? Visibility.Hidden : Visibility.Visible;
                case BoolConverterParameters.FalseAsCollapsed:
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
                case BoolConverterParameters.TrueAsCollapsed:
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                default:
                    throw new NotSupportedException("invalid parameter for BoolConverter, see class BoolConverter.BoolConverterParameters.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parameters for BooleanConverter
        /// </summary>
        public static class BoolConverterParameters
        {
            /// <summary>
            /// Text switcher，formatter："Switch:text_for_true|text_for_false"。
            /// </summary>
            public const string TextSwitchPrefix = "switch:";

            /// <summary>
            /// Reverse，True => False, False => True
            /// </summary>
            public const string Reverse = "REVERSE";

            #region Convert to Visibility

            /// <summary>
            /// True => Visibility.Hidden, False => Visibility.Visible
            /// </summary>
            public const string TrueAsHidden = "TRUEASHIDDEN";

            /// <summary>
            /// True => Visibility.Visible, False => Visibility.Hidden
            /// </summary>
            public const string FalseAsHidden = "FALSEASHIDDEN";

            /// <summary>
            /// True => Visibility.Collapsed, False => Visibility.Visible
            /// </summary>
            public const string TrueAsCollapsed = "TRUEASCOLLAPSED";

            /// <summary>
            /// True => Visibility.Visible, False => Visibility.Collapsed
            /// </summary>
            public const string FalseAsCollapsed = "FALSEASCOLLAPSED";

            #endregion
        }
    }
}
