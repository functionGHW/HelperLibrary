/* 
 * FileName:    ObjectExtensions.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  3/11/2015 11:12:01 AM
 * Version:     v1.1
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public static class ObjectExtensions
    {
        /// <summary>
        /// create a new instance of T and copy value of public properties from old instance to new instance.
        /// Note that only the properties that can read and write will be copied. 
        /// </summary>
        /// <typeparam name="T">reference type</typeparam>
        /// <param name="obj">the old instance</param>
        /// <returns>a new instance of T, or null if you passed a null</returns>
        public static T CloneInstance<T>(this T obj) where T : class, new()
        {
            if (obj == null)
            {
                return null;
            }
            Type t = typeof(T);
            T newObj = new T();
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;

            // copy the public instance properties
            foreach (var p in t.GetProperties(flag))
            {
                if (p.CanRead && p.CanWrite)
                {
                    p.SetValue(newObj, p.GetValue(obj, null));
                }
            }
            return newObj;
        }

        /// <summary>
        /// call IDisposable.Dispose method if the object implement IDisposable.
        /// </summary>
        /// <param name="obj"></param>
        public static void TryDispose(this object obj)
        {
            var disposable = obj as IDisposable;
            disposable?.Dispose();
        }
    }
}
