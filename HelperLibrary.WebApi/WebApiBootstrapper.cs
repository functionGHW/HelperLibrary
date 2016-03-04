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
    public abstract class WebApiBootstrapper : IDisposable
    {
        private HttpSelfHostServer host;
        private bool disposed;

        protected WebApiBootstrapper()
        {
        }

        public event EventHandler Opening;
        public event EventHandler Opened;
        public event EventHandler Closing;
        public event EventHandler Closed;

        public void Run(string baseAddress)
        {
            if (baseAddress == null)
                throw new ArgumentNullException(nameof(baseAddress));

            Run(new Uri(baseAddress, UriKind.RelativeOrAbsolute));
        }

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

        protected virtual void ConfigHostConfiguration(HttpSelfHostConfiguration config)
        { }

        protected virtual void ConfigHost(HttpSelfHostServer httpSelfHostServer)
        { }

        public void Stop()
        {
            if (host == null)
                return;

            OnClosing();
            host.CloseAsync().Wait();
            OnClosed();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

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

        protected virtual void OnOpening()
        {
            Opening?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnOpened()
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
