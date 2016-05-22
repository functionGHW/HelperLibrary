/* 
 * FileName:    IService.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 12:22:35
 * Version:     v1.0
 * Description:
 * */

using System;

namespace HelperLibrary.WCF.Server
{
    /// <summary>
    /// abstract wcf service
    /// </summary>
    public interface IService : IDisposable
    {
        /// <summary>
        /// when service was opened
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        /// whenr service was closed
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// when service is closing
        /// </summary>
        event EventHandler Closing;

        /// <summary>
        /// when service was faulted
        /// </summary>
        event EventHandler Faulted;

        /// <summary>
        /// when service is opening
        /// </summary>
        event EventHandler Opening;

        /// <summary>
        /// get name of service
        /// </summary>
        string Name { get; }


        /// <summary>
        /// open the service
        /// </summary>
        void Open();

        /// <summary>
        /// close the service
        /// </summary>
        void Close();

    }
}
