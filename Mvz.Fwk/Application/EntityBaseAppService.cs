
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ALZAGRO.AppRendicionGastos.Fwk.Application {
    public class EntityBaseAppService<E, D>
        where E : class, IEntityBase, new()
        where D : class, IDto, new() {

        protected readonly IEntityBaseRepository<Error> _errorsRepository;

        protected readonly IUnitOfWork unitOfWork;

        protected readonly IEntityBaseRepository<E> entityRepository;

        public EntityBaseAppService(IEntityBaseRepository<Error> errorsRepository,
                                  IUnitOfWork unitOfWork) {
            _errorsRepository = errorsRepository;
            this.unitOfWork = unitOfWork;
        }

        public Int64? CurrentUserId {
            get {

                var customPrincipal = HttpContext.Current.User as CustomPrincipal;
                if (customPrincipal != null) {
                    return customPrincipal.UserId;
                }
                else {
                    return null;
                }
            }
        }

        public EntityBaseAppService(IEntityBaseRepository<Error> errorsRepository,
                                    IUnitOfWork unitOfWork,
                                    IEntityBaseRepository<E> entityRepository) {
            _errorsRepository = errorsRepository;
            this.unitOfWork = unitOfWork;
            this.entityRepository = entityRepository;
        }

        public virtual IEnumerable<D> GetAll() {
            var entities = this.entityRepository.GetAll().ToList();
            var dtos = Mapper.Map<IEnumerable<E>,
                                   IEnumerable<D>>(entities);

            return dtos;
        }

        public virtual D GetById(Int64 id) {
            var area = this.entityRepository.GetSingle(id);
            var dtos = Mapper.Map<E, D>(area);
            return dtos;
        }

        public virtual void DeleteById(Int64 id) {
            var area = this.entityRepository.GetSingle(id);

            if (area != null) {
                this.entityRepository.Delete(area);
                this.unitOfWork.Commit();
            }
        }

        public virtual D Save(D dto) {
            E entity;
            if (dto.Id == 0) {
                entity = Mapper.Map<D, E>(dto);
                this.entityRepository.Add(entity);
            }
            else {
                entity = Mapper.Map<D, E>(dto);
                this.entityRepository.Edit(entity);
            }
            this.unitOfWork.Commit();

            if (entity != null) {
                dto.Id = entity.Id;
            }
            return dto;
        }

        public SearchResultViewModel<Dto> CreateResultWithoutOrder<Dto>(IEnumerable<Dto> items, ListViewCriteriaBase criteria) {
            var count = items.Count();

            if (criteria.Size > 0) {
                criteria.Page -= 1;
                items = items.Skip(criteria.Page * criteria.Size).Take(criteria.Size).ToList().AsEnumerable();
            }

            var searchResultViewModel = new SearchResultViewModel<Dto>();
            searchResultViewModel.Results = items;
            searchResultViewModel.TotalItems = count;

            return searchResultViewModel;
        }

        public SearchResultViewModel<Dto> CreateResultWithoutOrder<Entity, Dto>(IQueryable<Entity> query, ListViewCriteriaBase criteria) {

            var totalItems = query.Count();

            if (criteria.Size > 0) {
                criteria.Page -= 1;
                query = query.Skip(criteria.Page * criteria.Size).Take(criteria.Size).ToList().AsQueryable();
            }

            var list = query.ToList();
            var customerVm = Mapper.Map<IEnumerable<Entity>, IEnumerable<Dto>>(list);
            var searchResultViewModel = new SearchResultViewModel<Dto>();
            searchResultViewModel.Results = customerVm;
            searchResultViewModel.TotalItems = totalItems;
            return searchResultViewModel;
        }



        public SearchResultViewModel<Dto> CreateResult<Dto>(IEnumerable<Dto> items, ListViewCriteriaBase criteria,
                                                            String defaultOrderBy) {
            var count = items.Count();
            if (!String.IsNullOrEmpty(criteria.OrderBy)) {

                var sortDirection = "";

                //TODO: Esto esta realizado por que la directiva md-table le concatena un - al orderBy.
                if (criteria.OrderBy.IndexOf("-") != -1) {
                    sortDirection = " descending";
                    criteria.OrderBy = criteria.OrderBy.Remove(criteria.OrderBy.IndexOf("-"), 1);
                }

                var sortField = criteria.OrderBy;
                //Analiza si se trata de child objects
                var splitted_sortField = criteria.OrderBy.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted_sortField.Length > 1) {
                    sortField = GetSortFieldString(splitted_sortField, criteria);
                }
                else {
                    sortField = sortField.ToPascalCase();
                }


                items = items.OrderBy(sortField + sortDirection);


            }
            else {
                items = items.OrderBy(defaultOrderBy);
            }

            if (criteria.Size > 0) {
                criteria.Page -= 1;
                items = items.Skip(criteria.Page * criteria.Size).Take(criteria.Size).ToList().AsEnumerable();
            }

            var searchResultViewModel = new SearchResultViewModel<Dto>();
            searchResultViewModel.Results = items;
            searchResultViewModel.TotalItems = count;

            return searchResultViewModel;
        }
        public SearchResultViewModel<Dto> CreateResult<Dto>(IEnumerable<Dto> items) {

            var searchResultViewModel = new SearchResultViewModel<Dto>();
            searchResultViewModel.Results = items;
            searchResultViewModel.TotalItems = items.Count();

            return searchResultViewModel;
        }

        public SearchCompositeResultViewModel<T, R> CreateResult<T, R>(IEnumerable<T> items, R DtoData) {

            var searchResultViewModel = new SearchCompositeResultViewModel<T, R>();
            searchResultViewModel.Results = items;
            searchResultViewModel.TotalItems = items.Count();
            searchResultViewModel.Resume = DtoData;

            return searchResultViewModel;
        }

        public SearchResultViewModel<Dto> CreateResult<Entity, Dto>(IQueryable<Entity> query,
                                                                   ListViewCriteriaBase criteria,
                                                                   String defaultOrderBy) {

            var totalItems = query.Count();
            var customers = ApplyBaseQuery(query, criteria, defaultOrderBy).ToList();
            var customerVm = Mapper.Map<IEnumerable<Entity>, IEnumerable<Dto>>(customers);
            var searchResultViewModel = new SearchResultViewModel<Dto>();
            searchResultViewModel.Results = customerVm;
            searchResultViewModel.TotalItems = totalItems;
            return searchResultViewModel;
        }

        public IQueryable<T> ApplyBaseQuery<T>(IQueryable<T> query, ListViewCriteriaBase criteria, String defaultOrderBy) {
            if (!String.IsNullOrEmpty(criteria.OrderBy)) {

                var sortDirection = "";
                //TODO: Esto esta realizado por que la directiva md-table le concatena un - al orderBy.
                if (criteria.OrderBy.IndexOf("-") != -1) {
                    sortDirection = " descending";
                    criteria.OrderBy = criteria.OrderBy.Remove(criteria.OrderBy.IndexOf("-"), 1);
                }

                var sortField = criteria.OrderBy;
                //Analiza si se trata de child objects
                var splitted_sortField = criteria.OrderBy.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted_sortField.Length > 1) {
                    sortField = GetSortFieldString(splitted_sortField, criteria);

                }
                else {
                    sortField = sortField.ToPascalCase();
                }


                query = query.OrderBy(sortField + sortDirection, null);


            }
            else {
                query = query.OrderBy(defaultOrderBy, null);
            }

            if (criteria.Size > 0) {
                criteria.Page -= 1;
                if(criteria.Page < 0) {
                    criteria.Page = 0;
                }
                query = query.Skip(criteria.Page * criteria.Size).Take(criteria.Size).ToList().AsQueryable();
            }
            return query;
        }


        public IEnumerable<T> ApplyBaseQuery<T>(IEnumerable<T> query, ListViewCriteriaBase criteria, String defaultOrderBy) {
            if (!String.IsNullOrEmpty(criteria.OrderBy)) {

                var sortDirection = "";

                //TODO: Esto esta realizado por que la directiva md-table le concatena un - al orderBy.
                if (criteria.OrderBy.IndexOf("-") != -1) {
                    sortDirection = " descending";
                    criteria.OrderBy = criteria.OrderBy.Remove(criteria.OrderBy.IndexOf("-"), 1);
                }

                var sortField = criteria.OrderBy;
                //Analiza si se trata de child objects
                var splitted_sortField = criteria.OrderBy.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted_sortField.Length > 1) {
                    sortField = GetSortFieldString(splitted_sortField, criteria);
                }
                else {
                    sortField = sortField.ToPascalCase();
                }


                query = query.OrderBy(sortField + sortDirection, null);


            }
            else {
                query = query.OrderBy(defaultOrderBy, null);
            }

            if (criteria.Size > 0) {
                query = query.Skip(criteria.Page * criteria.Size).Take(criteria.Size).ToList().AsEnumerable();
            }
            return query;
        }

        public IQueryable<T> MatchInFields<T>(IQueryable<T> source, string partialDescription, bool all, Expression<Func<T, string[]>> fieldSelectors) {
            var searchKeys = partialDescription.ToLower().Split(' ', ',', ';', ':').ToList();
            if (source == null)
                throw new ArgumentNullException("source");
            if (fieldSelectors == null)
                throw new ArgumentNullException("fieldSelectors");
            NewArrayExpression newArray = fieldSelectors.Body as NewArrayExpression;
            if (newArray == null)
                throw new ArgumentOutOfRangeException("fieldSelectors", fieldSelectors, "You need to use fieldSelectors similar to 'x => new string [] { x.LastName, x.FirstName, x.NickName }'; other forms not handled.");
            if (newArray.Expressions.Count == 0)
                throw new ArgumentException("No field selected.");
            if (searchKeys == null || searchKeys.Count == 0)
                return source;

            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            Expression expression = null;

            foreach (var searchKeyPart in searchKeys) {
                Tuple<string> tmp = new Tuple<string>(searchKeyPart);
                Expression searchKeyExpression = Expression.Property(Expression.Constant(tmp), tmp.GetType().GetProperty("Item1"));
                // Expression accentExpression = Expression.Property(Expression.Constant("", typeof(string)), "comparer", StringComparison.InvariantCultureIgnoreCase); 
                Expression oneValueExpression = null;
                foreach (var fieldSelector in newArray.Expressions) {
                    Expression act = Expression.Call(fieldSelector, containsMethod, searchKeyExpression);
                    if (oneValueExpression == null)
                        oneValueExpression = act;
                    else
                        oneValueExpression = Expression.OrElse(oneValueExpression, act);
                }

                if (expression == null)
                    expression = oneValueExpression;
                else if (all)
                    expression = Expression.AndAlso(expression, oneValueExpression);
                else
                    expression = Expression.OrElse(expression, oneValueExpression);
            }
            return source.Where(Expression.Lambda<Func<T, bool>>(expression, fieldSelectors.Parameters));
        }

        private String GetSortFieldString(string[] splitted_sortField, ListViewCriteriaBase criteria) {
            var sortField = "";

            sortField = "iif((";

            for (int i = 0; i < splitted_sortField.Length - 1; i++) {
                var child = splitted_sortField.Take(i + 1);
                sortField = sortField + String.Join(".", child) + "==null";
                if (i < splitted_sortField.Length - 2) {
                    sortField = sortField + "||";
                }
            }

            var fieldName = splitted_sortField.LastOrDefault();

            if (fieldName == "Id" || fieldName.Contains("Rating") || fieldName.Contains("CurrentSessionId")) {
                sortField = sortField + "),0," + criteria.OrderBy + ")";
            }
            else if (fieldName.StartsWith("Is")) {
                sortField = sortField + "),true," + criteria.OrderBy + ")";
            }
            else if (fieldName.Contains("DateTime")) {
                sortField = sortField + "),null," + criteria.OrderBy + ")";
            }
            else {
                sortField = sortField + "),\" \"," + criteria.OrderBy + ")";
            }

            return sortField;
        }
    }
}
