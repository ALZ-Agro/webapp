(function () {
    'use strict';

    angular
        .module('payments-module')
        .controller('PaymentsListCtrl', PaymentsListCtrl);


    PaymentsListCtrl.$inject = ['$scope', 'apiService', '$state', '$stateParams'];

    function PaymentsListCtrl($scope, apiService, $state, $stateParams) {

        $scope.tableLimitOptions = [10, 25, 50, 100, 200, 500];

        var currentEntityId = $stateParams.id ? $stateParams.id : 0;

        $scope.columns = [
            { headerName: "Usuario", name: "username", customClass: "first-column-padding" },
            { headerName: "Fecha de Pago", name: "updatedDateTime", isDate: true },
            { headerName: "Descripción", name: "description" },
            { headerName: "Monto", name: "amount" }
        ];


        var currentDate = new Date();
        $scope.advancedDateFilters = {
            dateFrom: {
                label: "Fecha desde",
                name: "dateFrom",
                model: new Date(currentDate.getFullYear(), currentDate.getMonth() - 2, currentDate.getDate()),
                param: "dateFrom"
            },
            dateUntil: {
                label: "Fecha hasta",
                name: "dateUntil",
                model: currentDate,
                param: "dateUntil"
            }
        };

        $scope.getParams = function () {
            return {
                userId: currentEntityId,
                dateFrom: $scope.advancedDateFilters.dateFrom.model,
                dateUntil: $scope.advancedDateFilters.dateUntil.model

            }
        };

        $scope.goBack = function () {
            $state.go("triangular.users-list");
        }

    }
})();