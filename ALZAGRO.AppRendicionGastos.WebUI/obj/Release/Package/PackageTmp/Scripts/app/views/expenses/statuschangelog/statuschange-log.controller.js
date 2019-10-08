(function () {
    'use strict';

    angular
        .module('expenses-module')
        .controller('ExpenseStatusChangeLogCtrl', ExpenseStatusChangeLogCtrl);


   /* @ngInject */
    function ExpenseStatusChangeLogCtrl($scope, $mdDialog, expenseId) {

        $scope.columns = [
            { headerName: "Fecha de modificación", name: "updatedDateTime", applyFilter: dateFilter, orderBy: 'UpdatedDateTime', textCenter: true, sortable: true, alignCenter:true },
            { headerName: "Modificado por", name: "userName", orderBy: 'User.FirstName', sortable: false, textCenter: true, alignCenter: true },
            { headerName: "Cambio", name: "change", orderBy: 'Change', sortable: false, textCenter: true, alignCenter: true },
            { headerName: "Motivo", name: "reasonOfChange", orderBy: 'ReasonOfChange', sortable: false, textCenter: true, alignCenter: true },
            { headerName: "Nota", name: "notes", orderBy: 'Notes', sortable: false, textCenter: true, alignCenter: true }
        ];

        function dateFilter(date) {
            if (date) {
                return moment(date).format('DD/MM/YYYY HH:mm');
            } else {
                return 'N/A';
            }
        }

        $scope.cancel = function () {
            $mdDialog.hide();
        }
        $scope.config = {
            expenseId: expenseId,
            orderBy: '-UpdatedDateTime',
            partialDescription: '',
            page: 1,
            size: 5
        }
        $scope.getParams = function() {
            return $scope.config;
        };
        

        $scope.tableLimitOptions = [5, 10, 25, {
            label: 'Todos',
            value: function () {
                return 0;
            }
        }];
    }
})();