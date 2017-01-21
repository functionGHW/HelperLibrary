/* 
 * FileName:    WcfExampleService.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 4:47:19 PM
 * Description:
 * */

using Examples.WCF.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.WCF.Host.ServiceImplementations
{
    public class WcfExampleService : IWcfExampleService
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public void SayHello(string msg)
        {
            Console.WriteLine("[{0}] Received message:{1}", DateTime.Now.ToString("HH:mm:dd"), msg);
        }
    }
}
