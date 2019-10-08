(function () {
    'use strict';

    angular
        .module('account-module')
        .controller('ForgotCtrl', ForgotCtrl);

    ForgotCtrl.$inject = ['$scope', '$state', 'apiService', 'notificationService',
                         '$timeout', '$stateParams', 'ValidationService', 'triSettings'];

    /* @ngInject */
    function ForgotCtrl($scope, $state, apiService, notificationService, $timeout,
                        $stateParams, ValidationService, triSettings) {
        var vm = this;
        vm.triSettings = triSettings;
        vm.user = {
            email: ''
        };
        vm.resetClick = resetClick;

        ////////////////

        vm.login = login;

        function login() {
            $state.go("triangular.account.login");
        }

        function resetClick() {
            if (new ValidationService().checkFormValidity($scope.forgot)) {
                apiService.post('/api/account/forgot', vm.user, onUpdateCompleted)
            }
        }

        function onUpdateCompleted(result) {
            notificationService.displaySuccess('Tu nueva clave ha sido enviada por correo electrónico.');
            $timeout(function () {
                $state.go("triangular.account.login");
            }, 100);
        }
    }
})();