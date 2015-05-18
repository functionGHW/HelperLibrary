/* 
 * FileName:    IFileSystem.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/18/2015 9:32:48 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.IOAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// an interface for isolating file system access.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// check if the file exists.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool FileExists(string filePath);

        /// <summary>
        /// open a file to read
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream OpenRead(string filePath);
    }
}
