(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('StatusChangeReport', StatusChangeReport);


    StatusChangeReport.$inject = ['$scope', 'apiService', '$state', '$stateParams', '$rootScope', '$q', 'sweetAlert', 'listService', 'FileSaver', 'Blob', 'expenseService', 'notificationService', '$window', '$http', '$mdDialog'];

    function StatusChangeReport($scope, apiService, $state, $stateParams, $rootScope, $q, sweetAlert, listService, FileSaver, Blob, expenseService, notificationService, $window, $http, $mdDialog) {
        $scope.tableLimitOptions = [10, 25, 50, 100, 200, 500];
        $scope.startDate = moment().subtract(1, 'month').toDate();
        $scope.endDate = moment().toDate();

        $scope.config = getDefaultFilters();

        $scope.users = [{
            id: 0,
            fullName: 'Todos'
        }];

        $scope.endDateOptions = {
            maxDate: moment().toDate(),
            minDate: angular.copy($scope.startDate)
        }


        $scope.startDateOptions = {
            minDate: moment().subtract(20, 'years').toDate(),
            maxDate: angular.copy($scope.endDate)
        }



        $scope.$on('ContextChanged', function (event, id) {
            $state.go('triangular.home-admin');
        });

        $scope.getParams = function () {
            return $scope.config;
        }

        $scope.startDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate();
                $scope.config.startDate = date;
                $scope.endDateOptions.minDate = date;
                refreshView();
            }
        }

        $scope.endDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate()
                $scope.config.endDate = date;
                $scope.startDateOptions.maxDate = date;
                refreshView();
            }
        }

        $scope.cleanFilters = function () {
            $scope.config = getDefaultFilters();
            refreshView();
        }

        $scope.generateReport = generateReport;
        $scope.refreshView = refreshView;

        $scope.columns = [
            { headerName: "Vendedor", name: "expenseUploaderFullName", orderBy: 'User.FirstName', textCenter: true, sortable: true, alignCenter: true },
            { headerName: "Fecha de modificación", name: "updatedDateTime", applyFilter: dateFilter, orderBy: 'UpdatedDateTime', textCenter: true, sortable: true, alignCenter: true },
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
        function getDefaultFilters() {
            return {
                orderBy: '-UpdatedDateTime',
                partialDescription: '',
                page: 1,
                size: 10,
                userId: 0,
                startDate: $scope.startDate,
                endDate: $scope.endDate
            }
        }

        function refreshView() {
            $rootScope.$broadcast('mvztable-refresh-to-page', 1);
        }



        function generateReport() {
            apiService.post('api/exportData/report/statusChange', $scope.config, function (response) {
                $http.get(response.data.url, { responseType: 'arraybuffer' })
                    .then(function success(result) {
                        var file = new Blob([result.data], {
                            type: 'text/csv'
                        });
                        FileSaver.saveAs(file, response.data.url.substr(response.data.url.lastIndexOf('/') + 1));
                    });
            }, function (error) {
                notificationService.serverError(error, "No se pudo generar el archivo de reporte de cambios de estado.");
            });
        }

        function init() {
            var config = {
                params: {
                    page: 1,
                    size: -1,
                    partialDescription: '',
                    roleId: 2
                }
            }
            apiService.get('api/users/search', config, function (response) {
                $scope.users = _.concat($scope.users, _.orderBy(response.data.results, ['FirstName', 'ASC']));
            }, function (error) {
                notificationService.displayError("No se pudo obtener el listado de vendedores.");
            });

        }
        init();
    }
})();