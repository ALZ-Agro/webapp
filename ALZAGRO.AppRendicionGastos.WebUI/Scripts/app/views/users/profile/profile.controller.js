(function () {
    'use strict';

    angular
        .module('users-module')
        .controller('UsersProfileCtrl', UsersProfileCtrl);


    UsersProfileCtrl.$inject = ['$scope', 'apiService', 'notificationService',
                                '$timeout', '$state', '$rootScope', '$location',
                                'ValidationService', 'membershipService'];

    function UsersProfileCtrl($scope, apiService, notificationService, $timeout, $state, $rootScope,
                              $location, ValidationService, membershipService) {

        $scope.user = {};
        $scope.editions = [];
        $scope.editionsIds = [];
        $scope.selectedEditions = [];
        var currentUser = $rootScope.repository.loggedUser;

        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                apiService.post('/api/users/updateProfile', $scope.user, onUpdateCompleted);
            }
        }

        $scope.cancel = function () {
            $state.go("triangular.users-list");
        }

        function init() {

        }

        function onLoadUserCompleted(result) {
            $scope.user = result.data;
        };


        function onUpdateCompleted(response) {
            // $scope.user.password = currentUser.password;
            // membershipService.saveCredentials($scope.user);
            notificationService.displaySuccess("Perfil actualizado con éxito.");
            $timeout(function () {
                $location.path("/users/list");
            }, 100);
        };

        init();
    }
})();