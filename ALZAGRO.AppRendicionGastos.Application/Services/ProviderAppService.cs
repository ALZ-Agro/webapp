using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Linq;
using System;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using System.Data.Entity;
using System.Collections.Generic;
using AutoMapper;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class ProviderAppService : EntityBaseAppService<Provider, ProviderDto>, IProviderAppService {

        #region Variables
        private readonly IEntityBaseRepository<Expense> expenseRepository;
        private readonly IEntityBaseRepository<ExpenseStatusLog> expenseStatusLogsRepository;
        private readonly IEntityBaseRepository<Image> imageRepository;
        #endregion

        public ProviderAppService(IEntityBaseRepository<Provider> providerRepository,
                              IEntityBaseRepository<Expense> expenseRepository,
                               IEntityBaseRepository<ExpenseStatusLog> expenseStatusLogsRepository,
                               IEntityBaseRepository<Image> imageRepository,
                              IEntityBaseRepository<Error> errorsRepository,
                              IUnitOfWork unitOfWork) :
            base(errorsRepository, unitOfWork, providerRepository) {
            this.expenseRepository = expenseRepository;
            this.expenseStatusLogsRepository = expenseStatusLogsRepository;
            this.imageRepository = imageRepository;
        }



        public object Search(ProviderListViewCriteria criteria) {

            var providers = this.entityRepository.GetAllIncluding(x => x.LegalCondition, 
                                                                  x => x.User);

            if(!criteria.OverrideExportCheck) {
                if (criteria.Exported) {
                    providers = providers.Where(x => x.SyncedWithERP.Equals(true));
                }
                else {
                    providers = providers.Where(x => x.SyncedWithERP.Equals(false));
                }
            } else {
                providers = providers.Where(x => x.Cuit > 9);
            }

            if (criteria.StartDate != null) {
                providers = providers.Where(x => DbFunctions.DiffDays(criteria.StartDate, x.UpdatedDateTime) >= 0);
            }

            if (criteria.EndDate != null) {
                providers = providers.Where(x => DbFunctions.DiffDays(criteria.EndDate, x.UpdatedDateTime) <= 0);
            }

            if(criteria.UserId != 0) {
                providers = providers.Where(x => x.UserId == criteria.UserId);
            }

            if(criteria.RoleId != 0) {
                providers = providers.Where(x => x.User.RoleId == criteria.RoleId);
            }

            if(criteria.ExportStatus != 0) {
                if(criteria.ExportStatus == 1) {
                    providers = providers.Where(x => x.SyncedWithERP.Equals(true));
                } else {
                    providers = providers.Where(x => x.SyncedWithERP.Equals(false));
                }
            }



            if (!string.IsNullOrEmpty(criteria.PartialDescription)) {
                providers = this.MatchInFields<Provider>(providers, criteria.PartialDescription, true, c => new[] { c.LegalName.ToLower() });
            }


            if (criteria.GetForBackOffice) {
                return this.CreateResult<Provider, ProviderDto>(providers, criteria, "LegalName");
            }
            return this.CreateResult<Provider, SearchProviderDto>(providers, criteria, "LegalName");
        }

        public override void DeleteById(long id) {
            var expensesWithThisProvider = this.expenseRepository.GetAll().Where(x => x.ProviderId == id).ToList();
            var expensesIdList = expensesWithThisProvider.Select(x => x.Id).ToList();
            var logs = this.expenseStatusLogsRepository.GetAll().
                                                        Where(x => expensesIdList.Contains(x.ExpenseId)).
                                                        ToList();
            var images = this.imageRepository.GetAll().
                                              Where(x => expensesIdList.Contains(x.ExpenseId)).
                                              ToList();
            foreach(var log in logs) {
                this.expenseStatusLogsRepository.Delete(log);
            }
            foreach (var image in images) {
                this.imageRepository.Delete(image);
            }
            foreach (var expense in expensesWithThisProvider) {
                this.expenseRepository.Delete(expense);
            }
            var provider = this.entityRepository.GetSingle(id);
            this.entityRepository.Delete(provider);
            this.unitOfWork.Commit();
        }

        public SearchResultViewModel<ProviderDto> ExportData(ProviderListViewCriteria criteria, bool? SyncedWithERP, List<long> providersToInclude) {
            var providers = this.entityRepository.GetAllIncluding(x => x.LegalCondition);
            if (SyncedWithERP != null) {
                providers = providers.Where(x => x.SyncedWithERP.Equals(SyncedWithERP));
            }
            if(providersToInclude.Count > 0) {
                providers = providers.Where(x => providersToInclude.Contains((long)x.Cuit));
            }
            return this.CreateResult<Provider, ProviderDto>(providers, criteria, "LegalName");
        }

        public List<ExportProviderDto> Import(List<ProviderDto> providers) {
            List<ExportProviderDto> errors = new List<ExportProviderDto>();
            var providersCUIT = providers.Select(x => x.Cuit).ToList();
            var existingProviders = this.entityRepository.GetAll().Where(x => providersCUIT.Contains((long)x.Cuit)).Select(x => new {
                x.Cuit,
                x.Id
            }).ToList();
            var existingProvidersCUIT = existingProviders.Select(x => x.Cuit).ToList();
            foreach (var provider in providers) {
                try {
                    provider.UpdatedDateTime = DateTime.Now;
                    if (!existingProvidersCUIT.Contains(provider.Cuit)) {
                        Save(provider);
                    }
                    else {
                        var existingProviderId = existingProviders.Where(x => x.Cuit == provider.Cuit).Select(x => x.Id).FirstOrDefault();
                        provider.Id = existingProviderId;
                        entityRepository.Edit(Mapper.Map<ProviderDto, Provider>(provider));
                    }
                }
                catch (Exception e) {
                    var errorProvider = Mapper.Map<ProviderDto, ExportProviderDto>(provider);
                    errorProvider.Error = "No se pudo guardar el proveedor. Por favor intente nuevamente. Error: " + e.Message;
                    errors.Add(errorProvider);
                }
            }
            return errors;
        }
    }
}