(function () {
    'use strict';

    angular
        .module('requests-module')
        .controller('RequestsListCtrl', RequestsListCtrl);


    RequestsListCtrl.$inject = ['$scope', 'apiService', '$state', '$stateParams'];

    function RequestsListCtrl($scope, apiService, $state, $stateParams) {

        var currentEntityId = $stateParams.id ? $stateParams.id : 0;

        $scope.columns = [
            { headerName: "Usuario", name: "username", customClass: "first-column-padding" },
            { headerName: "Descripción", name: "description" },
            { headerName: "Fecha de Consulta", name: "updatedDateTime", isDate: true },
            { headerName: "Fecha de Accidente", name: "accidentDate", isDate: true },
            { headerName: "Fecha de Convenio", name: "agreementDate", isDate: true },
            { headerName: "Indemnización", name: "compensation", isDecimal: true },
            { headerName: "Apu", name: "apu", isDecimal: true },
            { headerName: "Adicional", name: "aditional", isDecimal: true }
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