(function () {
    'use strict';

    angular
        .module('triangular.components')
        .controller('ToolbarController', DefaultToolbarController);

    /* @ngInject */
    function DefaultToolbarController($scope, $rootScope, $mdMedia, $element, $filter, $mdUtil, $mdSidenav,
        $mdToast, $state, $timeout, $document, triBreadcrumbsService, triSettings, triLayout, membershipService, apiService) {
        var vm = this;
        var repository = localStorage.getItem('alzagro-repository');
        if (repository) {
            var parsed = JSON.parse(repository);
            vm.loggedUser = Object.keys(parsed.loggedUser).length > 0; 
        } else {
            vm.loggedUser = false;
        }
        if ($state && $state.current.controller == 'ProviderListCtrl' || $state.current.controller == 'UsersListCtrl') {
            vm.hideContext = true;
        } else {
            vm.hideContext = false;
        }
        vm.breadcrumbs = triBreadcrumbsService.breadcrumbs;
        vm.emailNew = false;
        vm.openSideNav = openSideNav;
        vm.hideMenuButton = hideMenuButton;
        vm.toggleNotificationsTab = toggleNotificationsTab;
        vm.isFullScreen = false;
        vm.customContext = $rootScope.currentSelectedCompanyName;
        $scope.$on('ContextChanged', function (event, id) {
            $rootScope.currentSelectedCompanyName = _.head(_.filter($rootScope.companies, ["id", id])).description;
            vm.customContext = $rootScope.currentSelectedCompanyName;
            vm.hideContext = false;
        });
        $scope.$on('hideCustomContext', function (event, changeTo) {
            vm.hideContext = changeTo;
            if (vm.hideContext == changeTo) {
                $rootScope.$broadcast('hideCustomContextCompleted');
            }
        });
        vm.fullScreenIcon = 'zmdi zmdi-fullscreen';
        vm.toggleFullScreen = toggleFullScreen;
        //vm.redirectAulaVirtual = redirectAulaVirtual;
        vm.logOut = logOut;
        vm.currentUser = $rootScope.repository.loggedUser;
        vm.allowRefresh = true;

        var config = {
            params: {
                userId: 0,
                size: 10,
                page: 1
            }
        }

        if (vm.currentUser) {
            config.params.userId = vm.currentUser.userId;
        }


        $scope.$on('changeSize', function (event) {
            config.params.size += 10;
            apiService.get('api/notifications/search', config, function (results) {
                $rootScope.$broadcast('onNotificationRefresh', results.data);
            }, function (error) { });
        });
        ////////////////

        //Se recuperan las posiciones para obtener la cantidad

        function refreshNotifications() {
            if ($rootScope.repository && $rootScope.repository.loggedUser) {
                if (vm.allowRefresh) {
                    apiService.get('api/notifications/search', config, onLoadNotificationsCompleted, function (error) { });
                }
                if (config.params.userId === $rootScope.repository.loggedUser.userId) {
                    $timeout(refreshNotifications, 15 * 1000);
                }
            }
        }

        refreshNotifications();

        setInterval(function () {
            if ($rootScope.repository && $rootScope.repository.loggedUser) {
                apiService.get('api/notifications/getUnreadNumber', null, function (result) {
                    vm.numberOfNotifications = result.data;
                }, function (error) { });
            }
        }, 5000);


        $scope.$on('notificationsNumberChanged', function (ev, number) {
            apiService.get('api/notifications/getUnreadNumber', null, function (result) {
                vm.numberOfNotifications = result.data;
            }, function (error) { });
        });

        function onLoadNotificationsCompleted(results) {

            if ($rootScope.repository && $rootScope.repository.loggedUser && $rootScope.repository.loggedUser.email && vm.allowRefresh) {
                //vm.numberOfNotifications = results.data.totalItems;

                $rootScope.$broadcast('onNotificationRefresh', results.data);
            }
            
        }

        function openSideNav(navID) {
            $mdUtil.debounce(function () {
                $mdSidenav(navID).toggle();                
            }, 300)();
        }

        function hideMenuButton() {

            if ($mdSidenav('notifications').isOpen()) {
                vm.allowRefresh = false;
            }
            else {
                vm.allowRefresh = true;
            }
            switch (triLayout.layout.sideMenuSize) {
                case 'hidden':
                    return false;
                case 'off':
                    return true;
                default:
                    return $mdMedia('gt-sm');
            }
        }

        function toggleNotificationsTab(tab) {
            $rootScope.$broadcast('triSwitchNotificationTab', tab);
            vm.openSideNav('notifications');
        }

        function toggleFullScreen() {
            vm.isFullScreen = !vm.isFullScreen;
            vm.fullScreenIcon = vm.isFullScreen ? 'zmdi zmdi-fullscreen-exit' : 'zmdi zmdi-fullscreen';
            // more info here: https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
            var doc = $document[0];
            if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement) {
                if (doc.documentElement.requestFullscreen) {
                    doc.documentElement.requestFullscreen();
                } else if (doc.documentElement.msRequestFullscreen) {
                    doc.documentElement.msRequestFullscreen();
                } else if (doc.documentElement.mozRequestFullScreen) {
                    doc.documentElement.mozRequestFullScreen();
                } else if (doc.documentElement.webkitRequestFullscreen) {
                    doc.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (doc.exitFullscreen) {
                    doc.exitFullscreen();
                } else if (doc.msExitFullscreen) {
                    doc.msExitFullscreen();
                } else if (doc.mozCancelFullScreen) {
                    doc.mozCancelFullScreen();
                } else if (doc.webkitExitFullscreen) {
                    doc.webkitExitFullscreen();
                }
            }
        }

        function logOut() {
            membershipService.removeCredentials();
            $state.go("triangular.account.login");
        }
    }
})();
