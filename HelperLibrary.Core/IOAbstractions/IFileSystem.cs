/* 
 * FileName:    IFileSystem.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/18/2015 9:32:48 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.IOAbstractions
{
    using System.IO;

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
        /// open an exist text file to read
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream OpenRead(string filePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <param name="share"></param>
        /// <returns></returns>
        Stream Open(string path, FileMode mode, FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.None);
    }
}
