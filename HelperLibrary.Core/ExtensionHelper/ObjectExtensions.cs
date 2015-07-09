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
            Type t = typeof (T);
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

            InternalReverseArray(ary, 0, ary.Length);
        }

        /// <summary>
        /// Reverse the element of array between the startIndex and endIndex.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="ary"></param>
        /// <param name="startIndex">zero-based start index</param>
        /// <param name="length">length to reverse</param>
        /// <exception cref="ArgumentNullException">array is null</exception>
        /// <exception cref="IndexOutOfRangeException"> startIndex or length less than zero, 
        /// or startIndex plus length indicates a position out of range .</exception>
        public static void ReverseArray<TElement>(this TElement[] ary, int startIndex, int length)
        {
            if (ary == null)
                throw new ArgumentNullException("ary");
            if (startIndex < 0)
                throw new IndexOutOfRangeException("startIndex less than zero");
            if (length < 0)
                throw new IndexOutOfRangeException("length less than zero");
            if (startIndex + length > ary.Length)
                throw new IndexOutOfRangeException("position out of range.");

            InternalReverseArray(ary, startIndex, length);
        }

        internal static void InternalReverseArray<TElement>(TElement[] ary, int startIndex, int length)
        {
            Contract.Requires(ary != null);
            Contract.Requires(length >= 0);
            Contract.Requires(startIndex >= 0);
            Contract.Requires(startIndex + length <= ary.Length);

            int endIndex = startIndex + length - 1;
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
        /// convert and add many elements to the collection.
        /// </summary>
        /// <typeparam name="TElement">type of elements</typeparam>
        /// <typeparam name="TInput">type of input elements</typeparam>
        /// <param name="collection">the collection</param>
        /// <param name="converter">method that convert a Tinput object to Telement object.</param>
        /// <param name="listToAdd">list of elements to add</param>
        /// <exception cref="ArgumentNullException">cillection, listToAdd or converter is null.</exception>
        /// <exception cref="NotSupportedException">the collection is read-only.</exception>
        public static void AddRange<TElement, TInput>(this ICollection<TElement> collection,
            IEnumerable<TInput> listToAdd, Func<TInput, TElement> converter)
        {
            if (collection == null || listToAdd == null)
                throw new ArgumentNullException(collection == null ? "collection" : "listToAdd");

            if (converter == null)
                throw new ArgumentNullException("converter");

            if (collection.IsReadOnly)
                throw new NotSupportedException("Can not add, collection is read-only.");

            foreach (TInput item in listToAdd)
            {
                collection.Add(converter(item));
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

            foreach (TElement item in collection)
            {
                action.Invoke(item);
            }
        }
    }
}
