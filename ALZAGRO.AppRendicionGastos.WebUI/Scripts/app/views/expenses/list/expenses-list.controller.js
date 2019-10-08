(function () {
    'use strict';

    angular
        .module('expenses-module')
        .controller('ExpensesListCtrl', ExpensesListCtrl);


    ExpensesListCtrl.$inject = ['$scope', 'apiService', '$state', '$rootScope', '$q', 'sweetAlert', 'FileSaver', 'Blob', 'expenseService', 'notificationService', '$http', '$stateParams'];

    function ExpensesListCtrl($scope, apiService, $state, $rootScope, $q, sweetAlert, FileSaver, Blob, expenseService, notificationService, $http, $stateParams) {

        $scope.loggedUser = $rootScope.repository.loggedUser;
        var savedConfigsLS = localStorage.getItem('expense-list-config');
        if (savedConfigsLS) {
            var savedConfigs = JSON.parse(savedConfigsLS);
            if (savedConfigs.params.startDate) {
                savedConfigs.params.startDate = moment(savedConfigs.params.startDate).toDate();
                if (savedConfigs.params.endDate) {
                    savedConfigs.params.endDate = moment(savedConfigs.params.endDate).toDate();
                } else {
                    savedConfigs.params.endDate = moment().toDate();
                }
            } else {
                if (savedConfigs.params.endDate) {
                    savedConfigs.params.startDate = moment(savedConfigs.params.endDate).subtract(1, 'month').toDate();
                    savedConfigs.params.endDate = moment(savedConfigs.params.endDate).toDate();
                } else {
                    $scope.startDate = moment().subtract(1, 'month').toDate();
                    $scope.endDate = moment().toDate();
                }
            }
            savedConfigs.params.endDate = moment(savedConfigs.params.endDate).toDate();
            $scope.config = savedConfigs;
            $scope.config.params.companyId = Number($rootScope.currentSelectedCompany);
            $scope.startDate = savedConfigs.params.startDate;
            $scope.endDate = savedConfigs.params.endDate;
        } else {
            setDefaultsFilters();
            localStorage.setItem('expense-list-config', JSON.stringify($scope.config));
        }
        var firstSearchForCompany = true;

        $scope.payments = [
            {
                id: 0,
                description: 'Todos'
            }
        ];
        
        var promises_array = [];

        $scope.users = [{
            id: 0,
            fullName: 'Todos'
        }];
        

        $scope.tableLimitOptions = [10, 25, 50, 100, 200, 500];

        $scope.syncStatuses = [{
            id: 0,
            description: 'Todos'
        }];

        $scope.exportedOptionns = [
            {
                id: 0,
                description: 'Todos'
            },
            {
                id: 1,
                description: 'Exportados'
            },
            {
                id: 2,
                description: "Sin exportar"
            }
        ];

        $scope.$on('pageHasChanged', function (event) {
            firstSearchForCompany = false;
        });

        $scope.$on('subTotalChanged', function (event, newValue) {
            $scope.subTotal = newValue;
        });

        $scope.$on('ContextChanged', function (event, id) {
            firstSearchForCompany = true;
            $scope.users = _.remove($scope.users, function (userItem) {
                return userItem.id == 0;
            });
            setDefaultsFilters();
            localStorage.setItem('expense-list-config', null);
            //$scope.users.forEach(function (user) {
            //    if (user.id != 0) {
            //        $scope.users.splice($scope.users.indexOf(user), 1);
            //    }
            //});
            $scope.payments = _.remove($scope.payments, function (paymentItem) {
                return paymentItem.id == 0
            });
            if ($rootScope.repository.loggedUser.role != "Usuario") {
                getAllPayments();
                getAllUsers();
            } else {
                getUserPayments();
            }

         
            refresh(id);
        });

        $scope.endDateOptions = {
            maxDate: moment().toDate(),
            minDate: angular.copy($scope.startDate)
        }


        $scope.startDateOptions = {
            minDate: moment().subtract(20, 'years').toDate(),
            maxDate: angular.copy($scope.endDate)
        }


        $scope.startDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate();
                $scope.config.params.startDate = date;
                $scope.endDateOptions.minDate = date;
                refreshView();
            }
        }

        $scope.endDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate()
                $scope.config.params.endDate = date;
                $scope.startDateOptions.maxDate = date;
                refreshView();
            }
        }
        $scope.getParams = function () {
            return $scope.config.params;
        }



   

        $scope.cleanFilters = function () {
            setDefaultsFilters();
            refreshView();
        };

        function getDefaultsFilters() {
            return {
                params: {
                    companyId: Number($rootScope.currentSelectedCompany),
                    startDate: moment().subtract(1, 'month').startOf('day').add(1,'minute').toDate(),
                    endDate: moment().endOf('day').subtract(1,'minute').toDate(),
                    partialDescription: '',
                    page: 1,
                    size: 10,
                    paymentId: 0,
                    syncStatusId:0,
                    userId: 0,
                    categoryId: 0,
                    orderBy: '-Date',
                    getForBackEnd: true,
                    exported: 0
                }
            }
        }

        $scope.generateReport = function () {
            apiService.post('api/exportData/report/expenses', $scope.config.params, function (response) {
                $http.get(response.data.url, { responseType: 'arraybuffer' })
                    .then(function success(result) {
                        var file = new Blob([result.data], {
                            type: 'text/csv'
                        });
                        FileSaver.saveAs(file, response.data.url.substr(response.data.url.lastIndexOf('/') + 1));
                    });
            }, function (error) {
                notificationService.serverError(error, "No se pudo generar el archivo de reporte de gastos.");
            });
        }

        function setDefaultsFilters() {
            var defaults = getDefaultsFilters();
            
            $scope.startDate = defaults.params.startDate;
            $scope.endDate = defaults.params.endDate;
            $scope.config = defaults;
        }

        function currencyFilter(value) {
            var result = '';
            if (value) {
                result = expenseService.formatFloat(value)
            } else {
                result = 'N/A';
            }
            return result;
        }
            $scope.userIsAdminOrHigher = $scope.loggedUser && ($scope.loggedUser.role == "Administrativo" || $scope.loggedUser.role == "Administrador del sistema");

        var columns = [
            { headerName: "Estado", name: "syncStatus", orderBy: 'SyncStatus.Description', getClass: getClass, textCenter: true, sortable: true, alignCenter: true },
            { headerName: "Gasto ID", name: "id", sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Fecha", name: "date", isDate: true, sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Vendedor", parentRoot: 'user', name: "fullName", orderBy: 'User.FirstName', sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Grupo", name: "group", sortable: true, textCenter: true, alignCenter: true, visible: $scope.userIsAdminOrHigher },
            { headerName: $scope.userIsAdminOrHigher ? "Proveedor Desc" : "Proveedor", name: "provider", orderBy: 'Provider.LegalName', sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Tipo", name: "paymentType", orderBy: 'Aliquot.Description', sortable: true, textCenter: true, alignCenter: true, visible: !$scope.userIsAdminOrHigher },
            { headerName: "Pago", name: "payment", orderBy: 'Payment.Description', sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Categoría", name: "category", orderBy: 'Category.Description', sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Total", name: "total", applyFilter: currencyFilter, sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Exportado", name: "exported", applyFilter: boolNormalizer, sortable: true, textCenter: true, class: "text-center", visible: $scope.userIsAdminOrHigher }
        ];

        function nullNormalizer(value) {
            if (value) return value;
            return "N/A";
        }

        function boolNormalizer(value) {
            if (value) return "SI";
            return "NO";
        }

        $scope.columns = _.filter(columns, function (column) {
            return angular.isUndefined(column.visible) || column.visible;
        });


        $scope.actions = [];
        if ($rootScope.repository.loggedUser.role != 'Usuario') {
            if ($rootScope.repository.loggedUser.role == "Administrador del sistema") {
                $scope.actions.push({
                    isDisabled: function (item) {
                        return item.syncStatus != "Editando";
                    },
                    applyValidation: function (item) {
                        return true;
                    },
                    title: '',
                    tooltip: 'Desbloquear',
                    icon: 'zmdi zmdi-lock-open',
                    name: 'unlock',
                    classes: 'color-blue',
                });
            }
            $scope.actions.push({
                isDisabled: checkIfActionIsDisabled,
                applyValidation: function (item) {
                    return true;
                },
                title: '',
                tooltip: 'Aprobar',
                icon: 'alz-check-circle',
                name: 'approve',
                classes: 'color-green',
            },
            {
                isDisabled: checkIfActionIsDisabled,
                applyValidation: function (item) {
                    return true;
                },
                title: '',
                tooltip: 'Rechazar',
                icon: 'alz-cancel-circle',
                name: 'reject',
                classes: 'color-red',
                });




            $scope.customToolbar = [
                {
                    visible: true,
                    callback: approveAll,
                    label: 'Aprobar seleccionados',
                    icon: 'check-circle',
                    customIconClass: 'color-green font-size-2',
                    customClass: 'table-toolbar-label',
                    isDisabled: function (selected) {
                        return selected.length == 0 || _.filter(selected, function (item) { return item.exported; }).length > 0;
                    }
                },
                {
                    visible: true,
                    callback: rejectAll,
                    label: 'Rechazar seleccionados',
                    icon: 'cancel-circle',
                    customIconClass: 'color-red font-size-2',
                    customClass: 'table-toolbar-label',
                    isDisabled: function (selected) {
                        return selected.length == 0 || _.filter(selected, function (item) { return item.exported; }).length > 0;
                    }
                },
                {
                    visible: true,
                    callback: exportAll,
                    label: 'Exportar gastos',
                    icon: 'cloud-download',
                    customIconClass: 'color-blue font-size-2',
                    customClass: 'table-toolbar-label',
                    isDisabled: function (selected) {
                        return selected.length > 0;
                    }
                }
            ];
        }

        $scope.actions.push({
            isDisabled: function () { return false; },
            applyValidation: function (item) {
                return true;
            },
            title: '',
            tooltip: 'Ver detalle',
            icon: 'zmdi zmdi-assignment-o',
            name: 'view-detail',
            classes: 'color-blue',
        });

        function exportDateFilter(item) {
            if (item) return moment(item).format('DD/MM/YYYY');
            else {
                return 'N/A';
            }
        }

        function checkIfActionIsDisabled(item, action) {
            if (action) {
                if (action.name == 'approve') {
                    return (item.syncStatus == 'Aprobado' || item.syncStatus == 'Editando');
                }
                if (action.name == 'reject') {
                    return (item.syncStatus == 'Rechazado' || item.syncStatus == 'Editando');
                }
            }
            return false;
        }

        function refreshView() {
            localStorage.setItem('expense-list-config', JSON.stringify($scope.config));
            $rootScope.$broadcast('refresh');
            $scope.subTotal = 0;
        }

        $scope.actionCallback = function (actionName, item, event, tableData) {
            if (actionName == 'approve') {
                if (item.syncStatus === "Rechazado") {
                    var array = [];
                    array.push(item);
                    expenseService.openApproveModal(array).then(function (data) {
                        expenseService.approve(item,data).then(function () {
                            refreshView();
                        }).catch(function () { });
                    }).catch(function () { });;
                } else {
                    expenseService.approve(item).then(function () {
                        refreshView();
                    }).catch(function () { });
                }
            }
            if (actionName == 'reject') {
                expenseService.reject(item).then(function () {
                    refreshView();
                }).catch(function () { });
            }
            if (actionName == "view-detail") {
                localStorage.setItem('expense-list-config', JSON.stringify($scope.config));
                $state.go('triangular.expense-details', { id: item.id, data: item });
            }
            if (actionName == "unlock") {
                notificationService.displayInfo("Desbloqueando gasto...");
                var changeStatusDto = {
                    id: item.id,
                    syncStatus: _.head(_.filter($rootScope.syncStatuses, ["description", "Pendiente"])),
                    changeStatusReason: "Desbloquear gasto del estado de edición.",
                    changeStatusNote: "N/A"
                }
                apiService.post('api/expense/changeStatus', changeStatusDto, function (response) {
                    if (response.data.success) {
                        notificationService.displaySuccess("Gasto desbloqueado con éxito.");
                        refreshView();
                    }
                }, function (error) {
                    notificationService.serverError(error, "No se pudo desbloquear el gasto.");
                });
            }
        }

        $scope.filterBy = function (parameter) {
            if (parameter != null && parameter != undefined) {
                localStorage.setItem('expense-list-config', JSON.stringify($scope.config));
                refreshView();
            }
        }

        function getClass(item) {
            var customClass = 'color-white padding-10 width-table-status text-center '
            switch (item.syncStatus) {
                case 'Pendiente': customClass += 'background-orange'; break;
                case 'Rechazado': customClass += 'background-red'; break;
                case 'Aprobado': customClass += 'background-green'; break;
                case 'Duplicado': customClass += 'background-grey'; break;
                case 'Editando': customClass += 'background-lightblue'; break;
            }
            return customClass;
        }

        function approveAll(selected) {
            var defer_array = [];
            selected.forEach(function (item) {
                var defer = $q.defer();
                defer_array.push(defer.promise);
                expenseService.changeStatus(item, "Aprobado").then(function () {
                    defer.resolve();
                }).catch(function () {
                    defer.reject();
                });
            });
            return $q.all(defer_array);
        }        


        function rejectAll(selected) {
            var globalPromise = $q.defer();
            var defer_array = [];
            var deferredModal = $q.defer();
            defer_array.push(deferredModal.promise);
            if (selected && selected.length > 0) {
                expenseService.openRejectModal(selected).then(function (data) {
                    selected.forEach(function (item) {
                        var defer = $q.defer();
                        defer_array.push(defer.promise);
                        expenseService.changeStatus(item, "Rechazado", data).then(function () {
                            try {
                                deferredModal.resolve();
                            } catch (e){}
                            defer.resolve();
                        }).catch(function () {
                            defer.reject();
                        });
                    });
                }).catch(function (err) {
                    defer_array.push($q.reject());
                    deferredModal.reject();
                });
                
            } else {
                defer_array.push($q.reject());
                deferredModal.reject();
            }
            return $q.all(defer_array).then(function () {
                globalPromise.resolve();
            }).catch(function () {
                globalPromise.reject();
            });
            return globalPromise.promise;
        }

        function showExportModal() {
            var defer = $q.defer();
            sweetAlert.swal({
                title: "Exportar gastos",
                imageUrl: 'Content/images/popup_download.png',
                showCancelButton: true,
                confirmButtonText: "Exportar",
                confirmButtonColor: "",
                cancelButtonColor: "",
                confirmButtonClass: "color-green md-button md-alz-content-theme md-ink-ripple",
                cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                cancelButtonText: "CANCELAR",
                width: "400px",
                padding: "90px",
                html: '<div class="swal2-content" style="display: block;">Serán exportados los gastos presentes en la tabla que hayan sido aprobados y que no hayan sido exportados previamente.</div>',

            }).then(function (data) {
                defer.resolve();
            }).catch(function (err) {
                defer.reject();
            });
            return defer.promise;
        }

        function exportAll(selected) {
            var defer = $q.defer();
            showExportModal().then(function () {
                apiService.post('api/exportData/expenses', $scope.config.params, function (response) {
                    if (response.data.error_providers_path) {
                        $http.get(response.data.error_providers_path, { responseType: 'arraybuffer' })
                            .then(function success(result) {
                                var file = new Blob([result.data], {
                                    type: 'text/csv'
                                });
                                FileSaver.saveAs(file, response.data.error_providers_path.substr(response.data.error_providers_path.lastIndexOf('/') + 1));
                            });
                    }
                    sweetAlert.swal({
                        title: "Resultados",
                        type: 'info',
                        confirmButtonText: 'Aceptar',
                        showCancelButton: false,
                        width: "400px",
                        padding: "90px",
                        html: 
                        '<h5 class="text-left margin-bottom-0 padding-left-30">Registros exportados con éxito: ' + response.data.exported + '.</h5>' +
                        '<h5 class="text-left margin-bottom-0 padding-left-30">Registros que no pudieron ser exportados: ' + response.data.not_exported + '.</h5>' +
                        '<h5 class="text-left margin-bottom-0 padding-left-30">Proveedores no encontrados: ' + response.data.inexistent_providers + '.</h5>'

                    }).then(function () {
                        defer.resolve();
                    }).catch(function () {
                        defer.resolve();
                    });
                }, function (error) {
                    notificationService.serverError(error, "No se pudo generar el archivo de exportación de gastos.");
                    defer.reject();
                });
            }).catch(function (e) {
                defer.reject();
            });
            return defer.promise;
        }

        
        function exportedFilter(entity) {
            if (entity) {
                return 'SI'
            } else {
                return "NO"
            }
        }

        function refresh(companyId) {
            $scope.config.params.companyId = companyId;
            $scope.config.params.size = 10;
            $scope.config.params.page = 1;
            $scope.config.params.paymentId = 0;
            $scope.config.params.userId = 0;
            refreshView();
        }

        function getSyncStatuses() {
            var defer = $q.defer();
            $rootScope.syncStatuses = [];
            promises_array.push(defer.promise);
            apiService.get('api/syncStatus', null, function (response) {
                var syncStatuses = _.orderBy(response.data.filter(function (item) { return item.description != "Nuevo" }), ["description"], ["asc"]);
                $rootScope.syncStatuses = $scope.syncStatuses.concat(syncStatuses);
                $scope.syncStatuses = $scope.syncStatuses.concat(syncStatuses);
                defer.resolve();
            }, function (error) {
                defer.reject();
            });
        }

        function getRefusalReasons() {
            var defer = $q.defer();
            $rootScope.refusalReasons = [];
            apiService.get('api/changeReason/refusalReasons', null, function (response) {
                $rootScope.refusalReasons = response.data;
                $scope.refusalReasons = response.data;
                defer.resolve();
            }, function (error) {
                notificationService.displayError(error.data.message);
                defer.reject();
            });
            return defer.promise;
        }
        function getApprovalReasons() {
            var defer = $q.defer();
            $rootScope.refusalReasons = [];
            apiService.get('api/changeReason/approvalReasons', null, function (response) {
                $rootScope.approvalReasons = response.data;
                $scope.approvalReasons = response.data;
                defer.resolve();
            }, function (error) {
                notificationService.displayError(error.data.message);
                defer.reject();
            });
            return defer.promise;
        }

        function getUserPayments() {
            var defer = $q.defer();
            promises_array.push(defer.promise);
            apiService.get('api/payments/ofCurrentUser', null, function (result) {
                $scope.payments = $scope.payments.concat(result.data);
            }, function (error) {
                notificationService.displayError("Error de conexión al obtener el listado de formas de pago. Intente nuevamente en unos segundos.");
                defer.reject();
            });
        }

        function getAllPayments() {
            var config = {
                params: {
                    userName: '',
                    page: 1,
                    size: 10000,
                    partialDescription: '',
                    orderBy: 'Description',
                    userId: 0
                }
            }
            apiService.get('api/payments/search', config, function (result) {
                $scope.payments = $scope.payments.concat(result.data.results);
            }, function (error) {
                notificationService.displayError("Error de conexión al obtener el listado de formas de pago. Intente nuevamente en unos segundos.");
            });
        }

        function getAllUsers() {
            apiService.get('api/expense/getExpenseUsersByCompanyId/' + $rootScope.currentSelectedCompany, null, function (result) {
                $scope.users = $scope.users.concat(result.data);
            }, function (error) {
                notificationService.displayError("Error de conexión al obtener el listado de vendedores. Intente nuevamente en unos segundos.");
            });
        }

        function getCategories() {
            apiService.get('api/category/all', null, function (response) {
                $scope.categories = response.data;
                $scope.categories.unshift({
                    id: 0,
                    description: "Todas"
                });
            }, function (error) {
                notificationService.displayError("Error de conexión al obtener el listado de categorias. Intente nuevamente en unos segundos.");
            });
        }


        (function () {
            getSyncStatuses();
            getCategories();
            if ($rootScope.repository.loggedUser.role != "Usuario") {
                getRefusalReasons();
                getApprovalReasons();
                getAllPayments();
                getAllUsers();
            } else {
                getUserPayments();
            }
            return $q.all(promises_array)
        })();

        var refreshRatio = setInterval(function () {
            if ($rootScope.mvzTableData) {
                var hasSelectedItems = _.filter($rootScope.mvzTableData, ["selected", true]).length > 0;
                if (!hasSelectedItems) {
                    refreshView();
                }
            } else {
                refreshView();
            }
        }, ($rootScope.contextConfig.syncIntervalInSeconds ? $rootScope.contextConfig.syncIntervalInSeconds : 300) * 1000);
    }
})();