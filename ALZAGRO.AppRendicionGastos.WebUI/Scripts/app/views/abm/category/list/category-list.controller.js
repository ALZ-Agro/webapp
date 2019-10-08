(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('CategoryListCtrl', CategoryListCtrl);


    CategoryListCtrl.$inject = ['$scope', 'apiService', '$state', '$rootScope'];

    function CategoryListCtrl($scope, apiService, $state, $rootScope) {
        $scope.tableLimitOptions = [10, 25, 50, 100, 200, 500];
        $scope.config = {
                partialDescription: '',
                page: 1,
                size: 10,
                orderBy: 'Id'
        }

        $scope.getParams = function () {
            return $scope.config;
        }
        $scope.columns = [
            { headerName: "Id", name: "id", customClass: "first-column-padding", orderBy:"Id" },
            { headerName: "Descripción", name: "description", orderBy: "Description" },
            { headerName: "Mostrar con proveedor", name: "showTo", applyFilter: showToFilter, orderBy: 'ShowTo' },
            { headerName: 'Código', name: 'code', applyFilter: isNullFilter }
        ];

        function isNullFilter(item) {
            if (item) return item;
            return 'N/A';
        }

        function showToFilter(item) {
            switch (item) {
                case 0: return "Todos"; break;
                case 1: return "Genérico"; break;
                case 2: return "No genérico"; break;
                default: return 'Error'; break;
            }
        }

        

        $scope.actions = [];
      
        $scope.onLink = onLink;

        function onLink(actionName, entity) {}
        $scope.$on('ContextChanged', function (event, id) {
            $state.go('triangular.home-admin');
        });
        var hideCustomContextInterval = setInterval(function () {
            $rootScope.$broadcast('hideCustomContext', true);
        }, 10);

        $scope.$on('hideCustomContextCompleted', function () {
            clearInterval(hideCustomContextInterval);
        });

        $scope.add = function () {
            $state.go("triangular.category-edit");
        }
    }
})();