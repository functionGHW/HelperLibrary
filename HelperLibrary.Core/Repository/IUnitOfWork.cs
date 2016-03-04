/* 
 * FileName:    IUnitOfWork.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2/29/2016 8:00:25 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
