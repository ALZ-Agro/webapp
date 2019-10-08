(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('PaymentListCtrl', PaymentListCtrl);


    PaymentListCtrl.$inject = ['$scope', '$state', '$rootScope'];

    function PaymentListCtrl($scope, $state, $rootScope) {
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
            { headerName: "Id", name: "id", customClass: "first-column-padding", orderBy: 'Id' },
            { headerName: "Descripción", name: "description", orderBy:'Description' },
            { headerName: "Usuario", name: "fullName", parentRoot: 'user', orderBy: 'User.FirstName', applyFilter:userFilter },
            { headerName: "Id del usuario", name: "userId", orderBy: 'User.Id', applyFilter:isEmptyFilter }
        ];


        $scope.actions = [];


        function isEmptyFilter(item) {
            if (item) return item;
            return '-';
        }

        function userFilter(item) {
            if (item) {
                return item;
            } else {
                return "Todos";
            }
        }

 

        $scope.onLink = onLink;

        function onLink(actionName, entity) {

        }
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
            $state.go("triangular.payment-edit");
        }
       
    }
})();