/* 
 * FileName:    IRepositoryFactory.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  2/29/2016 8:01:12 PM
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary.Core.Repository
{
    public interface IRepositoryFactory
    {
        //TRepository CreateRepository<TRepository>(object param) where TRepository : class;

        object CreateRepository(Type repositoryType, object param);
    }
}
