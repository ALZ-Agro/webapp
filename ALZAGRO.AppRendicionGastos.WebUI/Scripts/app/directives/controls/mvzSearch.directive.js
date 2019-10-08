(function (app) {
    'use strict';

    app.directive('mvzSearch', mvzSearch);

    function mvzSearch($compile, $q, $http, apiService) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: '=',
                onSelect: "=",
                display: '@',
                isDisabled: '=',
                updateParams: '&',                
                selectedDisplay: '@',
                label: "@",
                controllerName: '@',
                actionName: '@',
                required: "@",
                notFoundMessage: '@',
                name: "@",
                requireFilterParameters: '='
            },
            templateUrl: '/scripts/app/directives/controls/mvzSearch.html',
            replace: true,
            controller: function ($scope, $element) {

                var config = {
                    params: {
                        getForBackOffice:true
                    }
                };

                $scope.searchText = null;
                if (angular.isUndefined($scope.required)) {
                    $scope.required = "false";
                }
                if (angular.isUndefined($scope.isDisabled)) {
                    $scope.isDisabled = false;
                } 
                if (angular.isUndefined($scope.display)) {
                    $scope.display = "description";
                }
                if (angular.isUndefined($scope.label)) {
                    $scope.label = "Buscar...";
                }
                if (angular.isUndefined($scope.orderBy)) {
                    $scope.orderBy = "id";
                }
                if (angular.isUndefined($scope.notFoundMessage)) {
                    $scope.notFoundMessage = "No se encontraron resultados.";
                }

                if (angular.isUndefined($scope.actionName)) {
                    $scope.actionName = "search";
                }
                if (angular.isUndefined($scope.selectedDisplay)) {
                    $scope.selectedDisplay = $scope.display;
                }


                $scope.searchItems = function () {
                    var deferred = $q.defer();
                    if ($scope.searchText.length > 0) {
                        var customParams = $scope.updateParams();
                        if (customParams) {
                            angular.forEach(customParams, function (value, key) {
                                config.params[key] = value;
                            });
                        }

                        config.params.partialDescription = $scope.searchText;
                        if ($scope.requireFilterParameters == true && customParams == null) {
                            deferred.resolve("");
                            return deferred.promise;
                        }
                        else {
                            apiService.get('/api/' + $scope.controllerName + '/' + $scope.actionName, config, function (items) {
                                if (config.params.getForBackOffice) {
                                    deferred.resolve(items.data.results);
                                } else {
                                    deferred.resolve(items.data);
                                }
                            });
                            return deferred.promise;
                        }
                    }

                }
            }
        }
    }
})(angular.module('common.ui'));