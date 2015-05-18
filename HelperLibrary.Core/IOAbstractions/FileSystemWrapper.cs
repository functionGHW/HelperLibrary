/* 
 * FileName:    FileSystemWrapper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/18/2015 11:09:36 AM
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
    /// a class wrapper file system operations
    /// </summary>
    public class FileSystemWrapper : IFileSystem
    {
        #region IFileSystem Members

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }

        public Stream Open(string filePath, FileMode mode, 
            FileAccess access = FileAccess.ReadWrite, 
            FileShare share = FileShare.None)
        {
            return File.Open(filePath, mode, access, share);
        }

        #endregion
    }
}
