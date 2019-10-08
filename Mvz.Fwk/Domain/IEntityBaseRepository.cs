using System;
using System.Linq;
using System.Linq.Expressions;

namespace ALZAGRO.AppRendicionGastos.Fwk.Domain {

    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new() {
        
        IQueryable<T> GetAllIncluding(params Expression<Func<T, Object>>[] includeProperties);
        
        IQueryable<T> GetAll();

        T GetSingle(Int64 id);

        IQueryable<T> FindBy(Expression<Func<T, Boolean>> predicate);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void ExecuteSP(String sp);
        Int64 GetLastInsertId();
    }
}