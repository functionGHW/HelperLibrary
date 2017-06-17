/* 
 * FileName:    EnumUtility.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  7/16/2015 5:55:15 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A helper class for Enum
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public static class EnumUtility<TEnum> where TEnum : struct, IConvertible
    {

        private static readonly Dictionary<TEnum, string> NameDict = new Dictionary<TEnum, string>();

        private static readonly Dictionary<string, TEnum> ValueDict = new Dictionary<string, TEnum>();

        private static readonly Type EnumType = typeof(TEnum);

        private static void EnsureIsEnumType()
        {
            if (!EnumType.IsEnum)
                throw new InvalidOperationException(EnumType.FullName + " is not Enum");
        }

        /// <summary>
        /// Get all value-name tuples of a Enum type.
        /// </summary>
        /// <returns>an array of Tuple</returns>
        public static Tuple<TEnum, string>[] GetEnumTuples()
        {
            EnsureIsEnumType();

            Array values = EnumType.GetEnumValues();

            var result = new Tuple<TEnum, string>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                object value = values.GetValue(i);
                string name = EnumType.GetEnumName(value);
                result[i] = Tuple.Create((TEnum)value, name);
            }
            return result;
        }

        /// <summary>
        /// Helper method to get enum value by its name.
        /// </summary>
        /// <param name="enumName">the name of value</param>
        /// <returns>the enum value</returns>
        /// <exception cref="ArgumentNullException">enumName is null or empty string.</exception>
        /// <exception cref="InvalidOperationException">enumName is invalid.</exception>
        public static TEnum GetEnumValue(string enumName)
        {
            EnsureIsEnumType();

            if (string.IsNullOrEmpty(enumName))
                throw new ArgumentNullException(nameof(enumName));

            string enumNameToUppper = enumName.ToUpperInvariant();
            TEnum value;
            if (ValueDict.TryGetValue(enumNameToUppper, out value))
            {
                return value;
            }
            if (Enum.TryParse(enumNameToUppper, true, out value))
            {
                ValueDict[enumNameToUppper] = value;
                return value;
            }

            throw new InvalidOperationException("Invalid name " + enumName);
        }
        
        /// <summary>
        /// Helper method to get name of a enum value.
        /// </summary>
        /// <param name="value">the enum value</param>
        /// <returns>the name of the enum value</returns>
        public static string GetEnumName(TEnum value)
        {
            EnsureIsEnumType();

            string name;
            if (NameDict.TryGetValue(value, out name))
            {
                return name;
            }
            name = Enum.GetName(EnumType, value);
            NameDict[value] = name;
            return name;
        }
    }
}
