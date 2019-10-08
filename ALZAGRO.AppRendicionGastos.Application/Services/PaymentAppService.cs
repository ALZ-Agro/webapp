using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using System;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class PaymentAppService : EntityBaseAppService<Payment, PaymentDto>, IPaymentAppService
    {

        #region Variables
        #endregion

        public PaymentAppService(IEntityBaseRepository<Payment> paymentRepository,
                              IEntityBaseRepository<Error> errorsRepository,
                              IUnitOfWork unitOfWork) :
            base(errorsRepository, unitOfWork, paymentRepository)
        {
        }

        public List<PaymentDto> GetUserPaymentModes() {
            var userPayments = this.entityRepository.GetAllIncluding(x => x.User, x => x.User.UserCompanyGroups).Where(x => x.UserId == (int)this.CurrentUserId || x.UserId == null).ToList();

            return Mapper.Map<List<Payment>, List<PaymentDto>>(userPayments);
        }

        public List<PaymentDto> GetUserPaymentModes(long Id) {
            var userPayments = this.entityRepository.GetAllIncluding(x => x.User, x => x.User.UserCompanyGroups).Where(x => x.UserId == Id || x.UserId == null).ToList();

            return Mapper.Map<List<Payment>, List<PaymentDto>>(userPayments);
        }

        public SearchResultViewModel<PaymentDto> Search(ListViewCriteriaBase criteria) {
            var query = this.entityRepository.GetAllIncluding(x => x.User, x => x.User.UserCompanyGroups);

            if(criteria.UserId != 0) {
                query = query.Where(x => x.UserId == criteria.UserId);
            }

            if(!string.IsNullOrEmpty(criteria.PartialDescription)) {
                query = this.MatchInFields<Payment>(query, criteria.PartialDescription, true, c => new[] { c.Description.ToLower(),
                                                                                                           c.User.FirstName.ToLower(),
                                                                                                           c.User.LastName.ToLower()
                });
            }

            return this.CreateResult<Payment, PaymentDto>(query, criteria, "Id");
        }

        public override PaymentDto GetById(long id) {
            var payment = this.entityRepository.GetAllIncluding(x => x.User, x => x.User.UserCompanyGroups).Where(x => x.Id == id).FirstOrDefault();
            return Mapper.Map<Payment,PaymentDto>(payment);
        }
    }
}