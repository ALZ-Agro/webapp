using ALZAGRO.Reports.Data.Entities;
using System.Linq;

namespace ALZAGRO.Reports.Data.Interfaces {

    public interface IReportRepository<T> where T : class, new() {
        
        void Add(T entity);

        IQueryable<T> GetAll();

        void Remove(T entity);

    }
}