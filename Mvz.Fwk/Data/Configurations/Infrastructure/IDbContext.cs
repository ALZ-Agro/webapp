using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ALZAGRO.AppRendicionGastos.Fwk.Data.Infrastructure {

    public interface IDbContext {

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void Commit();

        Exception ProcessException(Exception ex);

        void ExecuteSP(String sp);
    }
}