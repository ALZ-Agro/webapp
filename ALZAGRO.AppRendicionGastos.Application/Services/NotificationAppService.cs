using ALZAGRO.AppRendicionGastos.Application.Contracts;
using ALZAGRO.AppRendicionGastos.Application.Criterias;
using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.Application;
using ALZAGRO.AppRendicionGastos.Fwk.Culture;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using ALZAGRO.AppRendicionGastos.Fwk.UI;
using System;
using System.Linq;
using System.Web;
using FcmSharp.Settings;
using FcmSharp;
using AutoMapper;
using System.Reflection;
using FcmSharp.Requests;
using System.Threading;
using Newtonsoft.Json;

namespace ALZAGRO.AppRendicionGastos.Application.Services {
    public class NotificationAppService : EntityBaseAppService<Domain.Entities.Notification,
                                                                NotificationDto>, INotificationAppService {

        private readonly IEntityBaseRepository<User> usersRepository;
        private readonly IEntityBaseRepository<Device> devicesRepository;


        private readonly ITimeService timeService;

        public NotificationAppService(IEntityBaseRepository<Error> errorsRepository,
                                      IUnitOfWork unitOfWork,
                                      IEntityBaseRepository<Domain.Entities.Notification> notificationsRepository,
                                      IEntityBaseRepository<User> usersRepository,
                                      IEntityBaseRepository<Device> devicesRepository,
                                      ITimeService timeService) :
            base(errorsRepository, unitOfWork, notificationsRepository) {
            this.usersRepository = usersRepository;
            this.devicesRepository = devicesRepository;
            this.timeService = timeService;
        }
        public SearchResultViewModel<NotificationDto> Search(NotificationListViewCriteria criteria) {
            var notifications = this.entityRepository.GetAll();

            if(criteria.UserId != 0) {
                notifications = notifications.Where(x => x.UserId == criteria.UserId);
            } else {
                notifications = notifications.Where(x => x.UserId == (int)this.CurrentUserId);
            }

            if (!string.IsNullOrEmpty(criteria.PartialDescription)) {
                notifications = this.MatchInFields<Domain.Entities.Notification>(notifications, criteria.PartialDescription, true, c => new[] {
                    c.Title.ToLower(),
                    c.Message.ToLower()
                });
            }

            if (criteria.Unread) {
                notifications = notifications.Where(x => x.Read == false);
            }

            var result = this.CreateResult<Domain.Entities.Notification, NotificationDto>(notifications, criteria, "Id desc");
            return result;

        }


        public void CreateNewForExpense(Expense entity, ExpenseStatusLog change) {
            var user = this.usersRepository.GetSingle(entity.UserId);

            var userDevices = this.devicesRepository.GetAll().Where(x => x.UserId == entity.UserId).ToList();

            var expenseNotificationDetail = Mapper.Map<Expense, ExpenseNotificationDetailDto>(entity);

            var clickParameter = JsonConvert.SerializeObject(expenseNotificationDetail);

            var notification = new Domain.Entities.Notification() {
                Id = 0,
                UserId = user.Id,
                Title = "Tu solicitud de gasto para el día " + entity.Date.Date.ToString("dd/MM/yyyy") + " fue rechazado por " + change.ReasonOfChange,
                Status = 1,
                Message = change.Notes,
                RoleId = user.RoleId,
                Type = "info",
                Read = false,
                ClickParameter = clickParameter,
                OnAppClick = "editExpensePage"
            };
            this.entityRepository.Add(notification);
            this.unitOfWork.Commit();

            foreach (var device in userDevices) {
                notification.DeviceId = device.Id;
                notification.Device = device;

                SendPushNotification(notification);
            }
        }


        public void UpdateSettings(NotificationUpdateCriteria criteria) {
            var user = this.usersRepository.GetSingle(criteria.UserId);
            user.ShowNotifications = criteria.ShowNotifications;
            this.usersRepository.Edit(user);
            this.unitOfWork.Commit();
        }

     

        public int GetUnreadNumber() {
            return this.entityRepository.GetAll().Where(x => x.Read == false && x.UserId == (int)this.CurrentUserId && (x.ExpireDateTime == null || this.timeService.LocalDateTimeNow.CompareTo(((DateTime)x.ExpireDateTime)) <= 0)).Count();
        }

     



        private void SendPushNotification(Domain.Entities.Notification dto) {
            try {
                var settings = FileBasedFcmClientSettings.CreateFromFile("alzagro-5bc7e", HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FirebaseJsonPath"]));

                // Construct the Client:
                using (var client = new FcmClient(settings)) {
                    // Construct the Data Payload to send:
                    var dtoData = Mapper.Map<Domain.Entities.Notification, NotificationDataDto>(dto);
                    var data = dtoData.GetType()
                                 .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                      .ToDictionary(prop => prop.Name, prop => prop.GetValue(dtoData, null) == null ? "" : prop.GetValue(dtoData, null).ToString());


                    // The Message should be sent to the News Topic:
                    var message = new FcmMessage() {
                        ValidateOnly = false,
                        Message = new Message {
                            Data = data,
                            Token = dto.Device.Token,
                            Notification = new FcmSharp.Requests.Notification() {
                                Title = dtoData.Title,
                                Body =  dtoData.Message
                            }
                        }
                    };
                    switch (dto.Device.DeviceType) {
                        case "ios":
                            // iOS

                            break;
                        case "android":
                            // Android
                            message.Message.AndroidConfig = new AndroidConfig {
                                Priority = AndroidMessagePriorityEnum.HIGH,
                                Notification = new AndroidNotification() {
                                    Title = "ALZAGRO",
                                    Body = dtoData.Title + ". " + dtoData.Message,
                                    Icon = "ic_icon",
                                    Color = "#7BA529",
                                    Sound = "default"
                                }
                            };

                            if (dto.ExpireDateTime != null) {
                                message.Message.AndroidConfig.TimeToLive = dto.ExpireDateTime - this.timeService.LocalDateTimeNow;
                            }
                            break;
                    }

                    // Finally send the Message and wait for the Result:
                    CancellationTokenSource cts = new CancellationTokenSource();

                    // Send the Message and wait synchronously:
                    var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                }
            }

            catch (Exception e) {
                string str = e.Message;
            }
        }
      
    }



    }