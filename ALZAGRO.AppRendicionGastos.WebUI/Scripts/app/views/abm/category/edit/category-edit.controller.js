(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('CategoryEditCtrl', CategoryEditCtrl);


    CategoryEditCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$stateParams', '$state', 'ValidationService', '$rootScope'];

    function CategoryEditCtrl($scope, apiService, notificationService, $stateParams, $state, ValidationService, $rootScope) {
 
        $rootScope.$broadcast('hideCustomContext', true);

        $scope.showToFieldOptions = [{
            id: 0,
            description: "Todos"
        },
        {
            id: 1,
            description: "Genérico"
        },
        {
            id: 2,
            description: "No genérico"
        }];

        /// Options:
        /// 0: Show when any provider is selected.
        /// 1: Show only when generic provider is selected
        /// 2: Show only when non generic provider is selected.

        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                var category = Object.assign({}, $scope.category);
                apiService.post('/api/category', category, onUpdateCompleted);
            }
        }

        function onUpdateCompleted() {
            notificationService.displaySuccess("Categoría actualizada con éxito.");
            $scope.cancel();
        }

        $scope.cancel = function () {
            $state.go("triangular.category-list");
        }

        function init() {
            if ($stateParams.id) {
                apiService.get('api/category/' + $stateParams.id, null, function (response) {
                    $scope.category = response.data;
                }, function (error) {
                    notificationService.serverError(error, "No se pudo obtener la categoría.");
                    $scope.cancel();
                });
            }

        }

        init();
    }
})();