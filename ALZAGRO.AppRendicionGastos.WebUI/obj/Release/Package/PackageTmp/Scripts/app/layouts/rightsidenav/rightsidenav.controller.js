(function () {
    'use strict';

    angular
        .module('triangular.components')
        .controller('RightSidenavController', RightSidenavController);

    /* @ngInject */
    function RightSidenavController($scope, $http, $mdSidenav, membershipService, $state, API_CONFIG, apiService, $rootScope, $timeout) {
        var vm = this;
        vm.currentTab = 1;
        $scope.currentUser = $rootScope.repository.loggedUser;

        vm.markAllAsReaded = function () {
            vm.notificationGroups.forEach(function (group) {
                var parentIndex = vm.notificationGroups.indexOf(group);
                group.notifications.forEach(function (notification) {
                    if (!notification.read) {
                        var dto = Object.assign({}, notification);
                        dto.read = true;
                        dto.clickParameter = JSON.stringify(dto.clickParameter);
                        apiService.post('api/notifications', dto, function () {
                            notification.read = true;
                            $rootScope.$broadcast('notificationsNumberChanged');
                        }, function (error) {
                            notificationService.displayError(error.data.message);
                        });
                    }
                })
            })
        }

        vm.onNotificationClick = function (notification, parentIndex, ev) {
            $mdSidenav('notifications').toggle();
            var index = vm.notificationGroups[parentIndex].notifications.indexOf(notification); 
            if (!notification.read) {
                $rootScope.$broadcast('notificationsNumberChanged');
                notification.read = true;
                var notificationDto = Object.assign({}, notification);
                notificationDto.clickParameter = JSON.stringify(notificationDto.clickParameter);
                apiService.post('api/notifications', notificationDto, function () {
                    vm.notificationGroups[parentIndex].notifications[index].read = true;
                    $rootScope.notifications[parentIndex].read = true;
                }, function (error) {
                    console.log(error);
                });
            }
            //var clickParameters = JSON.parse(notification.clickParameter);
            $state.go('triangular.expense-details', {
                id: notification.clickParameter.Id,
                data: notification,
                enteredViaNotifications:true
            });
        }
        function onLoadNotificationsCompleted(notificationsResult) {
                   
            vm.notificationGroups = [{
                name: 'Notificaciones',
                notifications: [],
                totalItems: notificationsResult.totalItems
            }];

            notificationsResult.results = _.orderBy(notificationsResult.results, ['updatedDateTime'], ['desc']);

            notificationsResult.results.forEach(function (notification) {
                notification.icon = "alz-cancel-circle";
                notification.iconColor = "#FF5151";
                notification.clickParameter = JSON.parse(notification.clickParameter);
                notification.date = notification.updatedDateTime;
                vm.notificationGroups[0].notifications.push(notification);
            });
        }

        vm.switchNotifications = function () {
            if ($rootScope.repository.loggedUser) {
                var data = {
                    params: {
                        userId: $rootScope.repository.loggedUser,
                        showNotifications: $scope.currentUser.showNotifications
                    }
                }
                membershipService.updateSettings(data);
                $rootScope.$broadcast('turnNotifications');
            }
        }

        
        $scope.$on('onNotificationRefresh', function ($event, results) {
            onLoadNotificationsCompleted(results);
        });

        $scope.$on('triSwitchNotificationTab', function ($event, tab) {
            vm.currentTab = tab;
            var config = {
                params: {
                    userId: 0,
                    size: 10,
                    page: 1
                }
            }
            if ($rootScope.repository && $rootScope.repository.loggedUser) {
                config.params.userId = $rootScope.repository.loggedUser.userId;
                apiService.get('api/notifications/search', config, onNotificationsCompleted, function (error) { });
            }
            
        });

        function onNotificationsCompleted(result) {
            onLoadNotificationsCompleted(result.data);
        }

        vm.nextPage = function () {
            $rootScope.$broadcast('changeSize');
        }
    }
})();
