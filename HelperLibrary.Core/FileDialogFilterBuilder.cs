using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core
{
    /// <summary>
    /// a class to build filter string for OpenFileDialog.Filter
    /// </summary>
    public class FileDialogFilterBuilder
    {
        private readonly Dictionary<string, HashSet<string>> extMap = new Dictionary<string, HashSet<string>>();


        private string HandleExtension(string ext)
        {
            if (string.IsNullOrWhiteSpace(ext))
                throw new FormatException("extension string can not be null or empty");

            ext = ext.Trim();
            if (!ext.StartsWith("."))
                throw new FormatException("extension string must start with \".\"");

            return ext;
        }

        /// <summary>
        /// get extensions with special name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>null if name not exist, otherwise an array of all extensions</returns>
        public string[] GetExtensions(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} can not be null or empty", nameof(name));
            extMap.TryGetValue(name, out var set);
            return set?.ToArray();
        }

        /// <summary>
        /// add exteions to list with the name, if name not exist, create new one.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public FileDialogFilterBuilder AddExtensions(string name, params string[] extensions)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} can not be null or empty", nameof(name));

            if (extensions.Length == 0)
                return this;

            bool isNew = !extMap.TryGetValue(name, out var set);
            if (isNew)
                set = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var ext in extensions)
                set.Add(HandleExtension(ext));

            if (isNew)
                extMap.Add(name, set);

            return this;
        }

        /// <summary>
        /// remove all extensions of the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileDialogFilterBuilder RemoveAllExtensionsOf(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} can not be null or empty", nameof(name));

            extMap.Remove(name);
            return this;
        }

        /// <summary>
        /// create item for *.*
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FileDialogFilterBuilder CreateExtensionForAllFile(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} can not be null or empty", nameof(name));

            if (extMap.TryGetValue(name, out var set))
                throw new InvalidOperationException($" \"{name}\" already exists");

            extMap.Add(name, new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".*" });

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public FileDialogFilterBuilder RemoveExtension(string name, string extension)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} can not be null or empty", nameof(name));

            extension = HandleExtension(extension);

            if (extMap.TryGetValue(name, out var set))
            {
                set.Remove(extension);
                if (set.Count == 0)
                    extMap.Remove(name);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var tmp = new List<string>(extMap.Count * 2);
            foreach (var item in extMap)
            {
                tmp.Add(item.Key);
                tmp.Add(string.Join(";", item.Value.Select(x => "*" + x)));
            }
            return string.Join("|", tmp);
        }
    }
}
