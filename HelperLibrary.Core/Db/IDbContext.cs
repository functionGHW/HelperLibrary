/* 
 * FileName:    IDbContext.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  1/11/2016 10:49:46 PM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Linq;

namespace HelperLibrary.Core.Db
{
    public interface IDbContext : IDisposable
    {

        IQueryable<TEntity> QueryableSet<TEntity>() where TEntity : class;

        int SaveChanges();

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;
    }
}
