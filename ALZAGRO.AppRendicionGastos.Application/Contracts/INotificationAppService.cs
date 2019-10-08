using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Contracts {
    public interface INotificationAppService : IEntityBaseAppService<Notification, NotificationDto> {
        //NotificationHubClient Hub { get; set; }
        SearchResultViewModel<NotificationDto> Search(NotificationListViewCriteria criteria);

        void UpdateSettings(NotificationUpdateCriteria criteria);

        void CreateNewForExpense(Expense entity, ExpenseStatusLog change);


        int GetUnreadNumber();




        //void SendPushNotification(Notification dto);
    }
}
