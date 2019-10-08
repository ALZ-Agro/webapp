(function () {
    'use strict';

    angular
        .module('account-module')
        .controller('LoginCtrl', LoginCtrl);


    LoginCtrl.$inject = ['$scope', 'membershipService', 'notificationService', '$rootScope',
        'triSettings', 'triRoute', '$state', 'ValidationService', 'apiService', '$q'];

    function LoginCtrl($scope, membershipService, notificationService, $rootScope, triSettings,
        triRoute, $state, ValidationService, apiService, $q) {

        var vm = this;
        var rememberedUser = membershipService.getRememberedUser();
        
        vm.user = {
            email: rememberedUser ? rememberedUser.email : '',
            password: rememberedUser ? rememberedUser.password : '',
            userId: '',
            rememberMe: rememberedUser ? rememberedUser.rememberMe : false
        };
        vm.loginClick = loginClick;
        vm.triSettings = triSettings;


        ////////////////
        function init() {

        }
        var entering = false;

        vm.login = function (event) {
            if ((event.keyCode == 13 || event.key == "Enter") && !entering) {
                vm.loginClick();
            }
        }

        function loginClick() {
            if (new ValidationService().checkFormValidity($scope.login) && !entering) {
                entering = true;
                membershipService.login(vm.user, loginCompleted, loginFailed);
                setTimeout(function () {
                    entering = false;
                }, 2 * 1000);
            }
        }

        function loginFailed(error) {
            notificationService.displayError(response.data);
            entering = false;
        }

        

        function loginCompleted(result) {
            entering = false;
            if (result.data.success) {
                vm.user.email = result.data.email;
                vm.user.userId = result.data.userId;
                vm.user.role = result.data.role;
                vm.user.fullName = result.data.firstName + " " + result.data.lastName;
                vm.user.enabledCompanies = result.data.enabledCompanies;
                vm.user.showNotifications = result.data.showNotifications;
                if (vm.user.enabledCompanies && vm.user.enabledCompanies.length > 0) {
                    var mappedUserCompanies = _.map(vm.user.enabledCompanies, function (userCompanyGroup) {
                        return userCompanyGroup.company
                    });
                    $rootScope.$broadcast('ContextChanged', mappedUserCompanies[0].id);
                }
                membershipService.saveCredentials(vm.user);
                apiService.post('api/users/updateLastActivityDateTime/' + result.data.userId, null, onUpdateActivityDateTime);

                membershipService.getConfigParams().then(function () {
                    if ($rootScope.previousState &&
                        $rootScope.previousState != '/' &&
                        $rootScope.previousState != '/login'
                    ) {
                        $location.path($rootScope.previousState);
                    }
                    else {
                        if (vm.user.role.description == "Usuario" || vm.user.role.description == "Administrativo") {
                            $state.go('triangular.home-admin');
                            notificationService.displaySuccess('Bienvenido ' + result.data.firstName + " " + result.data.lastName);
                        } else if (vm.user.role.description == "Administrador del sistema") {
                            $state.go('triangular.users-list');
                            notificationService.displaySuccess('Bienvenido ' + result.data.firstName + " " + result.data.lastName);
                        } else {
                            $state.go('401');
                        }
                    }
                });
            }
            else {
                notificationService.displayError(result.data.message);
            }
        }
        function onUpdateActivityDateTime() {
            //console.log("Fecha actualizada");
        };

       
        init();
    }
})();