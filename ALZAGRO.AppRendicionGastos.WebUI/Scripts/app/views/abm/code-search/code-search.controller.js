(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('CodeSearchCtrl', CodeSearchCtrl);


    CodeSearchCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$rootScope', '$q', '$mdExpansionPanel'];

    function CodeSearchCtrl($scope, apiService, notificationService, $state, $rootScope, $q, $mdExpansionPanel) {

        $mdExpansionPanel().waitFor('panelConfigOne').then(function (instance) {
            instance.expand();
        });

        var hideCustomContextInterval = setInterval(function () {
            $rootScope.$broadcast('hideCustomContext', true);
        }, 10);
        

        $scope.$on('hideCustomContextCompleted', function () {
            clearInterval(hideCustomContextInterval);
        });

        $scope.$on('ContextChanged', function (event, id) {
            $state.go('triangular.home-admin');
        });
        

        $scope.cancel = function () {
            $state.go("triangular.home-admin");
        }

        $scope.search = {
            category2: null
    };
        $scope.categories = [];
        $scope.taxes = [];
        $scope.companies = [];

        $scope.getValue = function(model) {
            if (model) return model;
            return '-';
        }


        function getTaxes() {
            $scope.taxes = [
                {
                    id: 'G',
                    description: 'Gravado'
                }, {
                    id: 'N',
                    description: 'No Gravado'
                }
            ];
        }

        function getSectors() {
            $scope.userGroups = [];
            apiService.get('api/users/groups',
                null,
                function (response) {
                    
                    angular.forEach(response.data,
                        function(group) {
                            angular.forEach($scope.companies,
                                function(company) {
                                    $scope.userGroups.push({
                                        id: group.id + company.id,
                                        description: group.description + ' ' + company.name,
                                        code: group.code + company.id
                                    });
                                });
                        });
                },
                function() {
                    notificationService.displayError('No se pudieron obtener los sectores.');
                });
        }

        function getCompanies() {
            var defer = $q.defer();
            $scope.companies = [];
            apiService.get('api/company',
                null,
                function (response) {
                    $scope.companies = response.data;
                    defer.resolve();
                },
                function () {
                    notificationService.displayError('No se pudieron obtener las empresas ni los grupos de venta.');
                    defer.reject();
                });
            return defer.promise;
        }

        function getCategories() {
            $scope.categories = [];
            apiService.get('api/category/all',
                null,
                function (response) {
                    $scope.categories = response.data;
                },
                function () {
                    notificationService.displayError('No se pudieron obtener las categorías.');
                });
        }

        function getPayments() {
            $scope.payments = [
                {
                    id: 'E',
                    description: 'Efectivo'
                },
                {
                    id: 'T',
                    description: 'Tarjeta'
                }
            ];
        }

        function init() {
            getCompanies().then(function () {
                getSectors();
            }).catch(angular.noop);
            getTaxes();
            getCategories();
            getPayments();
        }

        init();
    }
})();