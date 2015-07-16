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

    public static class EnumUtility
    {
        public static Tuple<TEnum, string>[] GetEnumTuples<TEnum>() where TEnum : struct, IConvertible
        {
            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new InvalidOperationException(enumType.FullName + " is not Enum");

            Array values = enumType.GetEnumValues();

            var result = new Tuple<TEnum, string>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                object value = values.GetValue(i);
                string name = enumType.GetEnumName(value);
                result[i] = Tuple.Create((TEnum)value, name);
            }
            return result;
        }
    }
}
