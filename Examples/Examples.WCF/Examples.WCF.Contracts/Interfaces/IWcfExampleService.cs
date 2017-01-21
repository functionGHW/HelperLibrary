/* 
 * FileName:    IWcfExampleService.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/21/2017 4:44:17 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Examples.WCF.Contracts.Interfaces
{
    [ServiceContract]
    public interface IWcfExampleService
    {
        [OperationContract]
        DateTime GetCurrentDateTime();

        [OperationContract]
        void SayHello(string msg);
    }
}
