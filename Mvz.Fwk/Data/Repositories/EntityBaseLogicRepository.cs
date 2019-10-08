//using Mvz.Fwk.Domain.Entities;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
//using System.Linq;

//namespace Mvz.Fwk.Data {
//    public abstract class EntityBaseLogicRepository<T, I> : IRepository<T>
//        where T : EntityBase
//        where I : IContext {
//        protected readonly I context;
//        protected readonly IExceptionMapper exceptionMapper;

//        public RepositoryBase(I context, IExceptionMapper exceptionMapper) {
//            this.context = context;

//            this.exceptionMapper = exceptionMapper;
//        }

//        public virtual IQueryable<T> GetAll() {
//            DbContext ctx = this.context as DbContext;
//            ctx.DisableFilter("NotDeleted");
//            IQueryable<T> query = this.context.Set<T>();
//            return query;
//        }

//        public virtual IQueryable<T> GetAllAsNoTracking() {
//            IQueryable<T> query = this.context.Set<T>().AsNoTracking();
//            return query;
//        }

//        public virtual IQueryable<T> Get() {
//            IQueryable<T> query = this.context.Set<T>().Where(x => x.Status != 0);
//            return query;
//        }

//        public virtual IQueryable<T> GetAsNoTracking() {
//            IQueryable<T> query = this.context.Set<T>().AsNoTracking().Where(x => x.Status != 0);
//            return query;
//        }

//        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
//            IQueryable<T> query = this.context.Set<T>().Where(predicate);
//            return query;
//        }

//        public virtual void Save(T entity) {
//            try {
//                entity.UpdatedDate = Time.TimeManager.Now;
//                this.BeforeSave(entity);

//                if (this.EntityIsNew(entity)) {
//                    this.Insert(entity);
//                }
//                else {
//                    this.Update(entity);
//                }
//                this.context.SaveChanges();
//            }
//            catch (DbEntityValidationException e) {
//                foreach (var eve in e.EntityValidationErrors) {
//                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
//                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
//                    foreach (var ve in eve.ValidationErrors) {
//                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
//                            ve.PropertyName, ve.ErrorMessage);
//                    }
//                }
//                this.DataBaseException(e);
//            }
//            catch (Exception ex) {
//                this.DataBaseException(ex);
//            }
//        }

//        protected virtual void BeforeSave(T entity) {
//        }

//        protected virtual void Insert(T entity) {
//            this.context.Set<T>().Add(entity);
//        }

//        protected virtual void Update(T entity) {

//            foreach (var collection in ReflectionHelper.GetCollections<T>(entity)) {
//                foreach (var point in collection) {
//                    var childEntity = point as EntityBase;
//                    childEntity.UpdatedDate = Time.TimeManager.Now;
//                    if (childEntity.Id == 0) {
//                        this.context.Entry(childEntity).State = System.Data.Entity.EntityState.Added;
//                    }
//                    else {
//                        this.context.Entry(childEntity).State = System.Data.Entity.EntityState.Modified;
//                    }

//                }
//            }
//            this.context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
//        }

//        public virtual void Delete(List<Int64> keys) {
//            try {
//                foreach (var key in keys) {
//                    var entity = this.context.Set<T>().Find(key);
//                    this.context.Set<T>().Remove(entity);
//                }

//                this.context.SaveChanges();
//            }
//            catch (Exception ex) {
//                this.DataBaseException(ex);
//            }
//        }


//        public virtual void Delete(T entity) {
//            try {
//                this.context.Set<T>().Remove(entity);
//                this.context.SaveChanges();
//            }
//            catch (Exception ex) {
//                this.DataBaseException(ex);
//            }
//        }

//        public void MarkCollectionAsUnchanged(IEnumerable entities) {
//            if (entities != null) {
//                foreach (var item in entities) {
//                    context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;
//                }
//            }
//        }

//        protected void DataBaseException(Exception ex) {
//            var str = this.exceptionMapper.MapException(ex.ToString());
//            throw new Exception(str, ex);
//        }

//        #region ManyToManyColecctions

//        protected void UpdateManyToManyColllection<E>(ICollection<E> sourceCollection, ICollection<E> targetCollection) where E : EntityBase {
//            this.AddItemToManyToManyCollection(sourceCollection, targetCollection);
//            this.RemoveItemToManyToManyCollection(sourceCollection, targetCollection);
//        }

//        private void AddItemToManyToManyCollection<E>(ICollection<E> sourceCollection, ICollection<E> targetCollection) where E : EntityBase {

//            if (sourceCollection != null) {
//                foreach (E source in sourceCollection) {
//                    bool itemFounded = false;
//                    foreach (E target in targetCollection) {
//                        if (this.GetCollectionItemId(source) == this.GetCollectionItemId(target)) {
//                            itemFounded = true;
//                        }
//                    }

//                    if (!itemFounded) {
//                        targetCollection.Add(source);
//                        this.context.Entry(source).State = System.Data.Entity.EntityState.Unchanged;
//                    }
//                }
//            }
//        }
//        private void RemoveItemToManyToManyCollection<E>(ICollection<E> sourceCollection,
//                                             ICollection<E> targetCollection) where E : EntityBase {
//            List<E> itemsToDelete = new List<E>();

//            foreach (E target in targetCollection) {
//                bool itemFounded = false;

//                if (sourceCollection != null) {
//                    foreach (E source in sourceCollection) {
//                        if (this.GetCollectionItemId(source) == this.GetCollectionItemId(target)) {
//                            itemFounded = true;
//                        }
//                    }
//                }
//                if (!itemFounded) {
//                    itemsToDelete.Add(target);
//                }
//            }
//            // Remove un-existing items
//            foreach (var itemToDelete in itemsToDelete) {
//                targetCollection.Remove(itemToDelete);
//            }
//        }

//        #endregion

//        protected void UpdateEntityColllections<E>(ICollection<E> sourceCollection,
//                                                   ICollection<E> targetCollection) where E : EntityBase {
//            this.AddItemToCollection(sourceCollection, targetCollection);
//            this.RemoveItemToCollection(sourceCollection, targetCollection);
//        }

//        protected virtual Boolean EntityIsNew(T entity) {
//            return (this.GetEntityId(entity) == "0");
//        }

//        protected virtual Boolean CollectionItemIsNew<E>(E entity) where E : EntityBase {
//            return (this.GetCollectionItemId(entity) == "0");
//        }

//        private String GetCollectionItemId<E>(E entity) where E : EntityBase {
//            string keyName = GetPropertyNameKey(entity);
//            return entity.GetType().GetProperty(keyName).GetValue(entity, null).ToString();
//        }

//        private String GetEntityId(T entity) {
//            string keyName = GetPropertyNameKey(entity);
//            return entity.GetType().GetProperty(keyName).GetValue(entity, null).ToString();
//        }

//        private String GetPropertyNameKey<E>(E entity) where E : EntityBase {
//            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
//            ObjectSet<E> set = objectContext.CreateObjectSet<E>();
//            string keyNames = set.EntitySet.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
//            return keyNames;
//        }

//        private void AddItemToCollection<E>(ICollection<E> sourceCollection,
//                                              ICollection<E> targetCollection) where E : EntityBase {
//            if (sourceCollection != null) {
//                foreach (E item in sourceCollection) {
//                    if (CollectionItemIsNew(item)) {
//                        targetCollection.Add(item);
//                    }
//                }
//            }
//        }
//        private void RemoveItemToCollection<E>(ICollection<E> sourceCollection,
//                                             ICollection<E> targetCollection) where E : EntityBase {
//            List<E> itemsToDelete = new List<E>();

//            foreach (E target in targetCollection) {
//                bool itemFounded = false;

//                if (sourceCollection != null) {
//                    foreach (E source in sourceCollection) {
//                        if (this.GetCollectionItemId(source) == this.GetCollectionItemId(target)) {
//                            itemFounded = true;
//                        }
//                    }
//                }
//                if (!itemFounded) {
//                    itemsToDelete.Add(target);
//                }
//            }
//            // Remove un-existing items
//            foreach (var itemToDelete in itemsToDelete) {
//                this.context.Entry(itemToDelete).State = System.Data.Entity.EntityState.Deleted;
//                //targetCollection.Remove(itemToDelete);
//            }
//        }
//    }
//}