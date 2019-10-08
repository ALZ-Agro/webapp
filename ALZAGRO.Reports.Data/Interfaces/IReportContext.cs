

using ALZAGRO.Reports.Data.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ALZAGRO.Reports.Data.Interfaces {
    public interface IReportContext {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void Commit();
        
        
    }
}