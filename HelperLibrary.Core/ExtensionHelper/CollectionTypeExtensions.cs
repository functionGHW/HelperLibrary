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
