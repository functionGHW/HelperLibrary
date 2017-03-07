/* 
 * FileName:    CollectionTypeExtensions.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  8/19/2015 3:43:28 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.ExtensionHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CollectionTypeExtensions
    {
        /// <summary>
        /// Check if a collection is empty or just a null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">the ref of collection</param>
        /// <returns>return true if the collection is null or empty, 
        /// otherwise return false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
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
                throw new ArgumentNullException(nameof(ary));

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
                throw new ArgumentNullException(nameof(ary));
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


        /// <summary>
        /// Add all elements of the collection into the source collection.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException">source or collection is null.</exception>
        public static void AddRange<TElement>(this ICollection<TElement> source, IEnumerable<TElement> collection)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// Map and add all elements of the collection into the source collection.
        /// Using the given delegate to transform elements.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TAnother"></typeparam>
        /// <param name="source"></param>
        /// <param name="collection"></param>
        /// <param name="transform"></param>
        /// <exception cref="ArgumentNullException">source, collection or transform is null.</exception>
        public static void AddRange<TSource, TAnother>(this ICollection<TSource> source,
            IEnumerable<TAnother> collection, Func<TAnother, TSource> transform)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            foreach (var item in collection)
            {
                source.Add(transform(item));
            }
        }
    }
}
