using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using System.Collections.Generic;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using System;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Fwk.Criteria;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface IPaymentAppService : IEntityBaseAppService<Payment, PaymentDto>
    {
        List<PaymentDto> GetUserPaymentModes();
        List<PaymentDto> GetUserPaymentModes(long Id);
        SearchResultViewModel<PaymentDto> Search(ListViewCriteriaBase criteria);
    }
}