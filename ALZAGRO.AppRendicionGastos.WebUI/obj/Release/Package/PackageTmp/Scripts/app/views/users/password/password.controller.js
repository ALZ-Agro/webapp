(function () {
    'use strict';

    angular
        .module('users-module')
        .controller('UsersPasswordCtrl', UsersPasswordCtrl);


    UsersPasswordCtrl.$inject = ['$scope', 'apiService', 'notificationService',
                                 '$timeout', '$stateParams', '$state', '$http', 'ValidationService', '$rootScope',
                                 'membershipService', '$location'];

    function UsersPasswordCtrl($scope, apiService, notificationService, $timeout, $stateParams, $state, $http, ValidationService, $rootScope,
                               membershipService, $location) {

        // var currentEntityId = $stateParams.id ? $stateParams.id : 0;
        $scope.user = $rootScope.repository.loggedUser;
        $scope.entity = {};
        $scope.entity.userId = $scope.user.userId;

        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                apiService.post("/api/users/changePassword", $scope.entity, onUpdateCompleted);
            }
        }


        function onUpdateCompleted(result) {
            membershipService.removeCredentials();
            $state.go("triangular.account.login");
            notificationService.displaySuccess("La contraseña se ha modificado con éxito.");
            //$timeout(function () {
            //    $location.path("/reports/ranking");
            //}, 100);
        }

        function init() {
            //if (currentEntityId != 0) {
            //    apiService.get('/api/users/get/' + currentEntityId, null, onLoadUserCompleted);
            //}
        }

        function onLoadUserCompleted(result) {
            //$scope.user = result.data;
        };

        init();
    }
})();