(function () {
    'use strict';

    angular
        .module('provider-module')
        .controller('ProviderDetailsCtrl', ProviderDetailsCtrl);


    ProviderDetailsCtrl.$inject = ['$scope', 'apiService', '$state', '$stateParams', '$q', '$window', 'expenseService', 'notificationService', '$rootScope'];

    function ProviderDetailsCtrl($scope, apiService, $state, $stateParams, $q, $window, expenseService, notificationService, $rootScope) {


        $scope.provider = {};
        $scope.nextProvider = null;
        $scope.previusProvider = null;
       
        $scope.goBack = function () {
            $state.go('triangular.provider-list');
        }

        $scope.goToNextProvider = function () {
            if ($scope.nextProvider) {
                $state.go('triangular.provider-details', {
                    id: $scope.nextProvider.id,
                    data: $scope.nextProvider
                });
                $scope.nextProvider = null;
            }
        }

        $scope.goToPreviusProvider = function () {
            if ($scope.previusProvider) {
                $state.go('triangular.provider-details', {
                    id: $scope.previusProvider.id,
                    data: $scope.previusProvider
                });
                $scope.previusProvider = null;
            }
        }

        $scope.$on('onKeyDown', function (event, keyCode) {
            switch (keyCode) {
                case 39: $scope.goToNextProvider(); break;
                case 37: $scope.goToPreviusProvider(); break;
            }
        });

        $scope.getFormattedCUIT = function(){
            if ($scope.provider) {
                if ($scope.provider.cuit != null && $scope.provider.cuit != '') {
                    var cuit = String($scope.provider.cuit);
                    return cuit.substr(0, 2) + ' ' + cuit.substr(2, 10) + ' ' + cuit.substr(10, 11);
                } else { return '';}
            } else {
                return '';
            }
        }

        $scope.getFormattedPhone = function () {
            if ($scope.provider) {
                if ($scope.provider.phoneNumber != '' && $scope.provider.phoneNumber != null) {
                    var phone = String($scope.provider.phoneNumber);
                    return '(' + (phone.length > 10 ? phone.substr(0, 4) : phone.substr(0, 3)) + ') ' + (phone.length > 10 ? phone.substr(4, 8) : phone.substr(3, 7)) + ' ' + (phone.length > 10 ? phone.substr(8, phone.length) : phone.substr(7, phone.length));
                } else {
                    return '';
                }
            } else { return ''; }
        }


        function calculatePreviusAndNextProviders() {
            var indexOfCurrentProvider = _.indexOf($rootScope.mvzTableData, _.head(_.filter($rootScope.mvzTableData, ["id", Number($stateParams.id)])));
            if (indexOfCurrentProvider > 0) {
                $scope.previusProvider = $rootScope.mvzTableData[indexOfCurrentProvider - 1] ? $rootScope.mvzTableData[indexOfCurrentProvider - 1] : null;
                $scope.nextProvider = $rootScope.mvzTableData[indexOfCurrentProvider + 1] ? $rootScope.mvzTableData[indexOfCurrentProvider + 1] : null;
            } else {
                $scope.previusProvider = null;
                $scope.nextProvider = null;
            }
            
        }

        function checkIfProvider() {
            var defer = $q.defer();
            if (!$stateParams.id || !$stateParams.data) {
                $state.go('triangular.provider-list');
                defer.reject();
            } else {
                calculatePreviusAndNextProviders();
                $scope.provider = $stateParams.data;
                defer.resolve();
            }
            return defer.promise;
        }


        checkIfProvider();
    }
})();