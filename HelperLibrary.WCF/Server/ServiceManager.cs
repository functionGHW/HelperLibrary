/* 
 * FileName:    ServiceManager.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2016/5/18 14:07:47
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Configuration;

namespace HelperLibrary.WCF.Server
{
    /// <summary>
    /// implement IServiceManger, create service using app.config
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceTypeResolver typeResolver;
        private readonly List<IService> services;

        /// <summary>
        /// 
        /// </summary>
        public ServiceManager(IServiceTypeResolver typeResolver)
        {
            if (typeResolver == null)
                throw new ArgumentNullException(nameof(typeResolver));

            this.typeResolver = typeResolver;
            services = GetAllServices();

            foreach (var s in services)
            {
                s.Opened += this.AnyServiceOpened;
                s.Opening += this.AnyServiceOpening;
                s.Closed += this.AnyServiceClosed;
                s.Closing += this.AnyServiceClosing;
                s.Faulted += this.AnyServiceFaulted;
            }
        }

        private List<IService> GetAllServices()
        {
            Contract.Assert(typeResolver != null);

            var section = ConfigurationManager.GetSection(ServicesSectionName) as ServicesSection;

            if (section == null)
                throw new ConfigurationErrorsException("Configuration section not found£º " + ServicesSectionName);

            var allServices = section.Services;
            var list = new List<IService>();
            for (int i = 0; i < allServices.Count; i++)
            {
                var s = allServices[i];

                var t = typeResolver.GetServiceType(s.Name);
                var svc = new Service(t);
                list.Add(svc);
            }
            return list;
        }

        private const string ServicesSectionName = "system.serviceModel/services";

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceOpened;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceOpening;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceClosed;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceClosing;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceFaulted;

        public IEnumerable<IService> Services => services;

        public void OpenAll()
        {
            services.ForEach(item => item.Open());
        }

        public void OpenService(string serviceName)
        {
            if (serviceName == null)
                throw new ArgumentNullException(nameof(serviceName));

            var s = services.FirstOrDefault(item => item.Name == serviceName);
            s?.Open();
        }

        public void CloseAll()
        {
            services.ForEach(item => item.Close());
        }

        public void CloseService(string serviceName)
        {
            if (serviceName == null)
                throw new ArgumentNullException(nameof(serviceName));

            var s = services.FirstOrDefault(item => item.Name == serviceName);
            s?.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var service in services)
                {
                    IDisposable disposable = service;
                    disposable.Dispose();
                }
            }
        }

        private void AnyServiceOpened(object sender, EventArgs e)
        {
            var s = sender as IService;
            if (s == null)
                return;

            OnServiceOpened(new ServiceEventArgs(s.Name));
        }

        private void AnyServiceOpening(object sender, EventArgs e)
        {
            var s = sender as IService;
            if (s == null)
                return;

            OnServiceOpened(new ServiceEventArgs(s.Name));
        }

        private void AnyServiceClosed(object sender, EventArgs e)
        {
            var s = sender as IService;
            if (s == null)
                return;

            OnServiceClosed(new ServiceEventArgs(s.Name));
        }

        private void AnyServiceClosing(object sender, EventArgs e)
        {
            var s = sender as IService;
            if (s == null)
                return;

            OnServiceClosed(new ServiceEventArgs(s.Name));
        }

        private void AnyServiceFaulted(object sender, EventArgs e)
        {
            var s = sender as IService;
            if (s == null)
                return;

            OnServiceFaulted(new ServiceEventArgs(s.Name));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceOpened(ServiceEventArgs e)
        {
            var handler = ServiceOpened;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceClosed(ServiceEventArgs e)
        {
            var handler = ServiceClosed;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceFaulted(ServiceEventArgs e)
        {
            var handler = ServiceFaulted;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceOpening(ServiceEventArgs e)
        {
            var handler = ServiceOpening;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServiceClosing(ServiceEventArgs e)
        {
            var handler = ServiceClosing;
            handler?.Invoke(this, e);
        }
    }
}