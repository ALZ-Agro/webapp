using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Fwk.Application {

    public interface IEntityBaseAppService<E>
       where E : class, IEntityBase, new() { 

    }
    
    public interface IEntityBaseAppService<E, D> 
        where E : class, IEntityBase, new()
        where D : class, IDto, new() {

        IEnumerable<D> GetAll();

        D GetById(Int64 id);

        D Save(D areaDto);

        void DeleteById(Int64 id);
    }
}       