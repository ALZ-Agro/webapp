using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Application.UI;
using System;
using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface IExpenseAppService : IEntityBaseAppService<Expense, ExpenseDto>
    {

        SearchResultViewModel<ExpenseDto> Search(ExpenseListViewCriteria criteria);
        List<SearchUserDto> GetExpenseUsersByCompanyId(long companyId);

        AnalyticsDto GetAnalytics(ExpenseListViewCriteria criteria);

        ExpensesListSearchResultViewModel<ExpenseListDto> GetExpensesList(ExpenseListViewCriteria criteria);

        SearchResultViewModel<ExpenseStatusesLogDto> GetStatusLogList(ExpenseListViewCriteria criteria);


        ChangeExpenseStatusResult ChangeStatus(ChangeExpenseStatusDto entity);

        ExpenseDto GetSingle(long Id);

        ChangeExpenseStatusResult SetEditingState(long Id);

        SearchResultViewModel<ExpenseReportDto> GetReport(ExpenseListViewCriteria criteria);

        SearchResultViewModel<ExpenseStatusesLogDto> GetStatusesLogOfExpense(ExpenseListViewCriteria criteria);
    }
}