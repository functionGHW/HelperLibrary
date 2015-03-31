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
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

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
                    p.SetValue(newObj, p.GetValue(obj, null), null);
                }
            }
            return newObj;
        }

        /// <summary>
        /// Reverse the element of array
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ary"></param>
        /// <exception cref="ArgumentNullException">array is null</exception>
        public static void ReverseArray<TElement>(this TElement[] ary)
        {
            if (ary == null)
                throw new ArgumentNullException("ary");

            InternalReverseArray<TElement>(ary, 0, ary.Length - 1);
        }

        /// <summary>
        /// Reverse the element of array between the startIndex and endIndex.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ary"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <exception cref="ArgumentNullException">array is null</exception>
        /// <exception cref="IndexOutOfRangeException"> startIndex or endIndex out of range, 
        /// or startIndex greater than endIndex.</exception>
        public static void ReverseArray<TElement>(this TElement[] ary, int startIndex, int endIndex)
        {
            if (ary == null)
                throw new ArgumentNullException("ary");
            if (startIndex < 0 || startIndex >= ary.Length)
                throw new IndexOutOfRangeException("startIndex out of range");
            if (endIndex < 0 || endIndex >= ary.Length)
                throw new IndexOutOfRangeException("endIndex out of range");
            if (startIndex > endIndex)
                throw new ArgumentOutOfRangeException("endIndex must greater than startIndex.", (Exception)null);

            InternalReverseArray<TElement>(ary, startIndex, endIndex);
        }

        internal static void InternalReverseArray<TElement>(TElement[] ary, int startIndex, int endIndex)
        {
            Contract.Requires(ary != null);
            Contract.Requires(startIndex > 0 && startIndex < ary.Length);
            Contract.Requires(endIndex > 0 && endIndex < ary.Length);
            Contract.Requires(startIndex <= endIndex);

            if (ary.Length < 2 || endIndex == startIndex)
                return;

            while (startIndex < endIndex)
            {
                TElement tmp = ary[startIndex];
                ary[startIndex] = ary[endIndex];
                ary[endIndex] = tmp;

                startIndex++;
                endIndex--;
            }
        }

        /// <summary>
        /// add many elements to the collection.
        /// </summary>
        /// <typeparam name="TElement">type of elements</typeparam>
        /// <param name="collection">the collection</param>
        /// <param name="listToAdd">list of elements to add</param>
        /// <exception cref="ArgumentNullException">cillection or listToAdd is null.</exception>
        /// <exception cref="NotSupportedException">the collection is read-only.</exception>
        public static void AddRange<TElement>(this ICollection<TElement> collection,
            IEnumerable<TElement> listToAdd)
        {
            if (collection == null || listToAdd == null)
                throw new ArgumentNullException(collection == null ? "collection" : "listToAdd");

            if (collection.IsReadOnly)
                throw new NotSupportedException("Can not add, collection is read-only.");

            foreach (TElement item in listToAdd)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// traversing a collection with the action.
        /// </summary>
        /// <typeparam name="TElement">element type of collection</typeparam>
        /// <param name="collection">the collection</param>
        /// <param name="action">the action to invoke</param>
        public static void Foreach<TElement>(this IEnumerable<TElement> collection, 
            Action<TElement> action)
        {
            if (collection == null || action == null)
                throw new ArgumentNullException(collection == null ? "collection" : "action");

            foreach(TElement item in collection)
            {
                action.Invoke(item);
            }
        }
    }
}
