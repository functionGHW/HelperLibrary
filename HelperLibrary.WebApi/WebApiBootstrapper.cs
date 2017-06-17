/* 
 * FileName:    WebApiBootstrapper.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2/29/2016 7:55:31 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace HelperLibrary.WebApi
{
    /// <summary>
    /// abstract class of Bootstrapper
    /// </summary>
    public abstract class WebApiBootstrapper : IDisposable
    {
        private HttpSelfHostServer host;
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        protected WebApiBootstrapper()
        {
        }

        /// <summary>
        /// On bootstrapper opening
        /// </summary>
        public event EventHandler Opening;
        
        /// <summary>
        /// On bootstrapper opened
        /// </summary>
        public event EventHandler Opened;
        
        /// <summary>
        /// On bootstrapper closing
        /// </summary>
        public event EventHandler Closing;
        
        /// <summary>
        /// On bootstrapper closed
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress"></param>
        public void Run(string baseAddress)
        {
            if (baseAddress == null)
                throw new ArgumentNullException(nameof(baseAddress));

            Run(new Uri(baseAddress, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress"></param>
        public void Run(Uri baseAddress)
        {
            var config = new HttpSelfHostConfiguration(baseAddress);
            ConfigHostConfiguration(config);
            host = new HttpSelfHostServer(config);
            ConfigHost(host);
            OnOpening();
            host.OpenAsync().Wait();
            OnOpened();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        protected virtual void ConfigHostConfiguration(HttpSelfHostConfiguration config)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpSelfHostServer"></param>
        protected virtual void ConfigHost(HttpSelfHostServer httpSelfHostServer)
        { }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (host == null)
                return;

            OnClosing();
            host.CloseAsync().Wait();
            OnClosed();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (host != null)
                {
                    host.Dispose();
                    host = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnOpening()
        {
            Opening?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnOpened()
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
