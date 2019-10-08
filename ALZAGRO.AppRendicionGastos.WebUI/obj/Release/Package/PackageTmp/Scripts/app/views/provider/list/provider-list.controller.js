(function () {
    'use strict';

    angular
        .module('provider-module')
        .controller('ProviderListCtrl', ProviderListCtrl);


    ProviderListCtrl.$inject = ['$scope', 'apiService', '$state', '$stateParams', '$rootScope', '$q', 'sweetAlert', 'listService', 'FileSaver', 'Blob', 'expenseService', 'notificationService', '$window', '$http', '$mdDialog'];

    function ProviderListCtrl($scope, apiService, $state, $stateParams, $rootScope, $q, sweetAlert, listService, FileSaver, Blob, expenseService, notificationService, $window, $http, $mdDialog) {

        $scope.legalConditions = [];
        
        $scope.tableLimitOptions = [10, 25, 50, 100, 200,500];
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
            { headerName: "Vendedor", name: "userFullName", orderBy: 'User.FirstName', sortable: true, textCenter: true, alignCenter: true, applyFilter: nullValidator  },
            { headerName: "Fecha", name: "updatedDateTime", orderBy: 'UpdatedDateTime', applyFilter: dateFilter, sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Nombre o Razón Social", name: "legalName", sortable: true, textCenter: true, alignCenter: true },
            { headerName: "CUIT", name: "cuit", sortable: true, textCenter: true, alignCenter: true },
            {
                headerName: "Condición ante AFIP", name: "description", parentRoot: 'legalCondition', orderBy: 'LegalCondition.Description', sortable: true, textCenter: true, alignCenter: true
            },
            { headerName: "Nombre del Contacto", name: "contactFullName", orderBy: 'ContactFullName', applyFilter:nullValidator, sortable: true, textCenter: true, alignCenter: true },
            { headerName: "Exportado", name: "syncedWithERP", applyFilter: exportedFilter, sortable: false, textCenter: true, alignCenter: true },
        ];

        function nullValidator(value) {
            if (value && value != "NULL") return value;
            return 'N/A';
        }


        function dateFilter(entity) {
            if (entity) {
                return moment(entity).format('DD/MM/YYYY HH:mm');
            } else {
                return 'N/A';
            }
        }

        function exportedFilter(entity) {
            if (entity) {
                return 'SI'
            } else {
                return "NO"
            }
        }

        $scope.actions = [
            {
                isDisabled: function () { return false; },
                applyValidation: function (item) {
                    return true;
                },
                title: '',
                tooltip: 'Ver detalle',
                icon: 'zmdi zmdi-account-box',
                name: 'view-detail',
                classes: 'color-blue font-size-2',
            }
        ];

        $scope.actionCallback = actionCallback;

       

        $scope.customButtons = [
            {
                visible: true,
                iconOnly:true,
                callback: exportAll,
                label: 'Exportar proveedores',
                icon: 'cloud-download',
                customIconClass: 'color-blue font-size-2',
                customClass: 'table-toolbar-label margin-0 padding-0 md-icon-button',
                isDisabled: function (selected) {
                    return selected.length > 0 || $rootScope.mvzTableData.length == 0;
                }
            }
        ];

        function actionCallback(actionName, item, event, tableData) {
            if (actionName == "view-detail") {
                $state.go('triangular.provider-details', { id: item.id, data: item });
            }
        }
        
        function showExportModal() {
            var defer = $q.defer();
            sweetAlert.swal({
                title: "Exportar proveedores",
                text: "Serán exportados todos los proveedores visualizados en la tabla. Éstos luego quedarán habilitados para todos los usuarios de la aplicación.",
                imageUrl: 'Content/images/popup_download.png',
                showCancelButton: true,
                confirmButtonText: "Exportar",
                confirmButtonColor: "",
                cancelButtonColor: "",
                confirmButtonClass: "color-green md-button md-alz-content-theme md-ink-ripple",
                cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                cancelButtonText: "CANCELAR",
                width: "400px",
                input: 'text',
                inputPlaceholder: 'Nombre del archivo',
                inputAttributes: {
                    maxlength: 30,
                    autocapitalize: 'off',
                    autocorrect: 'off'
                },
                padding: "90px",
            }).then(function (input) {
                defer.resolve(input);
            }).catch(function (err) {
                defer.reject();
            });
            return defer.promise;
        }


        function exportAll(selected) {
            var defer = $q.defer();
            showExportModal().then(function (inputText) {
                if (angular.isUndefined(inputText) || inputText == "") {
                    inputText = 'Listado_de_proveedores';
                }
                $scope.config.fileName = inputText;
                var config = Object.assign({}, $scope.config);
                apiService.post('api/exportData/providers', config, function (response) {
                    $http.get(response.data.url, { responseType: 'arraybuffer' })
                        .then(function success(result) {
                            var file = new Blob([result.data], {
                                type: 'text/csv'
                            });

                            FileSaver.saveAs(file, ($scope.config.fileName ? $scope.config.fileName + ".csv" : "Listado_de_proveedores" + "-" + moment().format('DD-MM-YYYY-HH:mm') + ".csv"));
                            $rootScope.$broadcast('refresh');
                            defer.resolve();
                        });
                }, function (error) {
                    notificationService.serverError(error, "No se pudo generar el archivo de exportación de proveedores.");
                    defer.reject();
                });
            }).catch(function () {
                defer.reject();
            });
            return defer.promise;
        }

        function getValue(item, posibilities) {
            try {
                return _.head(_.filter(_.at(item, posibilities), function (value) { return !!value }));
            } catch (e) {
                return null;
            }
        }
       

        $scope.importCSV = function () {
            var parent = angular.element(document.body);
            $mdDialog.show({
                controller: 'ImportDialogController',
                templateUrl: '../Scripts/app/views/provider/import/import.tmpl.html',
                parent: parent,
                //targetEvent: event,
                clickOutsideToClose: true
            }).then(function (response) {
                notificationService.displayInfo("Cargando archivo...");
                var importProviderList = [];
                var rejectedList = [];
                var regex = new RegExp(/[^\x20-\x7E]+/g);
                response.data.forEach(function (item) {
                    if (Object.keys(item).length > 1) {
                        Object.keys(item).forEach(function (key, value) {
                            if (regex.test(key)) {
                                item[key.replace(regex, '')] = item[key];
                                delete item[key];
                            }
                        });
                        var itemLegalCondition = item["Condición ante AFIP"] || item["Condicin ante AFIP"] || item["Condicion ante AFIP"] || item["condicion ante afip"];
                        var providerDto = {
                            id: 0,
                            syncedWithERP: true,
                            cuit: getValue(item, ["CUIT","cuit","Cuit"]),
                            email: getValue(item, ["Correo electrónico", "Correo electronico","Correo electrnico"]),
                            phoneNumber: getValue(item, ["Teléfono", "Telefono", "Telfono"]),
                            contactFullName: getValue(item, ["Nombre de contacto", "Nombre de Contacto", "Nombre De Contacto", "nombre de contacto"]),
                            legalName: getValue(item, ["Nombre Legal", "nombre legal", "Nombre legal"]),
                            legalCondition: itemLegalCondition ? _.head(_.filter($scope.legalConditions, ["description", itemLegalCondition])) : null
                        }
                        if (itemLegalCondition && providerDto.legalCondition && providerDto.legalName  && providerDto.legalName != '0') {
                            importProviderList.push(providerDto);
                        } else if (!providerDto.legalName) {
                            item.Error = "Debés ingresar un nombre legal para este registro.";
                            rejectedList.push(item);
                        } else {
                            item.Error = "No se pudo encontrar una condición ante el AFIP válida para este proveedor.";
                            rejectedList.push(item);
                        }
                    }
                    
                });
                if (rejectedList.length > 0) {
                    notificationService.displayError("No se " + (rejectedList.length == 1 ? "pudo" : "pudieron") + " importar " + (rejectedList.length == 1 ? "un proveedor" : (rejectedList.length + " proveedores")) + " de la lista.");
                    var config = {
                        header: true,
                        newline: "\r\n",
                        delimiter: ",",
                        quotes: false,
                    };

                    var unparsed = Papa.unparse(rejectedList, config);
                    var blob = new Blob([unparsed], { type: "text/csv" });
                    FileSaver.saveAs(blob, "Proveedores_rechazados_" + moment().format("DD_MM_YYYY HHmmss") + ".csv");
                }
                if (importProviderList.length > 0) {
                    notificationService.displayInfo("Guardando proveedores...");
                    var errorList = [];
                    var import_promises = [];
                    var chunks = _.chunk(importProviderList,100);
                    chunks.forEach(function (chunk) {
                        var defer = $q.defer();
                        import_promises.push(defer.promise);
                        apiService.post('api/provider/import', chunk, function (result) {
                            if (result.data.errors.length > 0) {
                                errorList = errorList.concat(result.data.errors);
                            }
                            defer.resolve();
                        }, function (error) {
                            chunk.forEach(function (item) {
                                item.Error = "Error al importar este proveedor. Por favor verifique sus datos cargados.";
                            });
                            errorList = errorList.concat(chunk);
                            defer.resolve();
                        });
                    });
                    return $q.all(import_promises).then(function () {
                        if (importProviderList.length == 1 && errorList.length == importProviderList.length) {
                            notificationService.displayError("No se pudo importar el proveedor.");
                        } else {
                            var importedSuccessfully = (importProviderList.length - errorList.length);
                            notificationService.displaySuccess((importProviderList.length == 1 ? "Proveedor importado" : ((importedSuccessfully === 1) ? "Proveedor importado" : (importedSuccessfully + " proveedores importados"))) + " con éxito.");
                            if (errorList.length > 0) {
                                notificationService.displayError(errorList.length == 1 ? "Un proveedor no pudo ser importado" : (errorList.length + " proveedores no pudieron ser importados."));
                            }
                        }
                        if (errorList.length > 0) {
                            var config = {
                                header: true,
                                delimiter: ",",
                            };

                            var mappedErrorList = _.map(errorList, function (errorItem) {
                                return {
                                    "Nombre de contacto": errorItem.contactFullName,
                                    "CUIT": errorItem.cuit,
                                    "Correo electrónico": errorItem.email,
                                    "Nombre Legal": errorItem.legalName,
                                    "Telefono": errorItem.phoneNumber,
                                    "Condición ante AFIP": errorItem.legalCondition,
                                    "Error": errorItem.error
                                }
                            });

                            var unparsed = Papa.unparse(mappedErrorList, config);
                            var blob = new Blob([unparsed], { type: 'text/csv;charset=utf-8;' });
                            FileSaver.saveAs(blob, "Proveedores_rechazados_por_servidor_" + moment().format("DD_MM_YYYY HHmmss") + ".csv");
                        }

                    }).catch(function () {
                    });
                }
            }).catch(function (error) {
                if (error) {
                    notificationService.displayError(error);
                }
            });
        }

        
        function getDefaultFilters() {
            return {
                getForBackOffice: true,
                partialDescription: '',
                page: 1,
                size: 10,
                orderBy: 'Id',
                OverrideExportCheck: true,
                startDate: $scope.startDate,
                endDate: $scope.endDate,
                userId: 0,
                exportStatus:0
            }
        }


        

        

        function refreshView() {
            $rootScope.$broadcast('mvztable-refresh-to-page', 1);
        }

 

        function generateReport() {
            apiService.post('api/exportData/report/providers', $scope.config, function (response) {
                $http.get(response.data.url, { responseType: 'arraybuffer' })
                    .then(function success(result) {
                        var file = new Blob([result.data], {
                            type: 'text/csv'
                        });
                        FileSaver.saveAs(file, response.data.url.substr(response.data.url.lastIndexOf('/') + 1));
                    });
            }, function (error) {
                notificationService.serverError(error, "No se pudo generar el archivo de reporte de nuevos proveedores.");
            });
        }

        


        function init() {
            apiService.get('api/legalCondition', null, function (response) {
                $scope.legalConditions = response.data;
            }, function (error) {
                notificationService.displayError("No se pudieron obtener las categorías de IVA.");
            });
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

        var refreshRatio = setInterval(function () {
            if ($scope.config.size < 100) {
                $rootScope.$broadcast('refresh');
            }
        }, ($rootScope.contextConfig.syncIntervalInSeconds ? $rootScope.contextConfig.syncIntervalInSeconds : 300) * 1000);

        $scope.$on('$destroy', function () {
            clearInterval(refreshRatio);
        });

        init();
    }
})();