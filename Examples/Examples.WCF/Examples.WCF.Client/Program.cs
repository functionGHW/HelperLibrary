using Examples.WCF.Contracts.Interfaces;
using HelperLibrary.WCF.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.WCF.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            /* 
             * call service using static methods
             */
            // call service method with return value
            var currentTime = WcfClient.RequestService<IWcfExampleService, DateTime>(service => service.GetCurrentDateTime());
            Console.WriteLine("Current Time: {0}", currentTime);

            // call service method without return value
            WcfClient.RequestService<IWcfExampleService>(service => service.SayHello("Hello wcf!"));

            /*
             * call service using WcfClient object
             */

            //var client = new WcfClient<IWcfExampleService>(new ProxyBuilder("WcfExampleService"));
            var client = WcfClient.Create<IWcfExampleService>();
            
            // call service method with return value
            currentTime = client.RequestService<DateTime>(service => service.GetCurrentDateTime());
            Console.WriteLine("Current Time: {0}", currentTime);

            // call service method without return value
            client.RequestService(service => service.SayHello("Hello again!"));

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
