using HelperLibrary.WCF.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.WCF.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            // register all assemblies that container service types.
            var serviceTypeResolver = new ServiceTypeResolver();
            serviceTypeResolver.RegisterAssembly(typeof(Program).Assembly);

            // ServiceManager load all service from configuration file(App.config) automaticly.
            var serviceManager = new ServiceManager(serviceTypeResolver);
            serviceManager.ServiceOpened += (o, e) => { Console.WriteLine("Service {0} has been started.", e.ServiceName); };
            //foreach (var service in serviceManager.Services)
            //{
            //    Console.WriteLine("Service loaded:" + service.Name);
            //}
            using (serviceManager)
            {
                // start all service
                serviceManager.OpenAll();

                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
