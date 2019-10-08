(function () {
    'use strict';

    angular
        .module('users-module')
        .controller('UsersListCtrl', UsersListCtrl);


    UsersListCtrl.$inject = ['$scope', 'apiService', '$state', '$rootScope', 'notificationService', '$mdDialog'];

    function UsersListCtrl($scope, apiService, $state, $rootScope, notificationService, $mdDialog) {
        $scope.tableLimitOptions = [10, 25, 50, 100, {
            label: 'Todos',
            value: function () {
                return 0;
            }
        }];
        $scope.config = {
            params: {
                partialDescription: '',
                page: 1,
                size: 10,
                orderBy: 'Id'
            }
        }

        $scope.getParams = function () {
            return $scope.config;
        }
        $scope.columns = [
            { headerName: "Usuario", name: "username", customClass: "first-column-padding" },
            { headerName: "Código", name: "id_Erp", applyFilter: notNullFilter }, 
            {
                headerName: "Empresa", name: 'userCompanyGroupsPlain', itemTitleName: 'company', isArray: true, sortable: false, noItemMessage: 'N/A'
            },
            {
                headerName: "Grupo de ventas", name: 'userCompanyGroupsPlain', itemTitleName: 'companyGroup', isArray: true, sortable: false, customClass: 'no-minimize', noItemMessage: 'N/A'
            }, 
            { headerName: "E-mail", name: "email" },
            { headerName: "Nombre", name: "firstName" },
            { headerName: "Apellido", name: "lastName" },
            { headerName: "Tipo de usuario", name: "roleId", applyFilter: roleFilter },
            { headerName: "Última fecha conexión", name: "lastActivityDateTime", applyFilter: dateFilter, customClass: "text-center" },
            { headerName: "Bloqueo de acceso", name: "isLocked", alignCenter: true, isBoolean: true, customCallback: lockUser }
        ];

        function roleFilter(role) {
            switch (role) {
                case 1: return "Administrativo"; break;
                case 2: return "Usuario"; break;
                case 3: return "Administrador del sistema"; break;
                default: return 'N/A'; break;
            }
        }

        function lockUser(user) {
            user.isLocked = !user.isLocked;
            apiService.post('api/users/updateProfile', user, function (response) {
                var changedToStatus = user.isLocked ? "desbloqueado" : "bloqueado";
                notificationService.displaySuccess(user.email + " " + changedToStatus + " con éxito.");
                $rootScope.$broadcast('refresh');
            }, function (error) {
                user.isLocked = !user.isLocked;
                notificationService.displayError("No se pudo bloquear a " + user.firstName + " " + user.lastName + "." + error.data.message ? " Error: " + error.data.message : "");
            });

        }

        function dateFilter(date) {
            if (date) {
                return moment(date).format('DD/MM/YYYY HH:mm');
            } else {
                return 'N/A';
            }
        }

        function notNullFilter(value) {
            if (!value || value === "0") {
                return 'N/A';
            }
            return value;
        }

        $scope.actions = [
            //{
            //    name: "watchUserRequests", icon: "fa fa-eye fa-2x",
            //    applyValidation: showAction, tooltip: "Ver listado de consultas", customStyle: "color:#757575"
            //},
            //{
            //    name: "watchUserPayments", icon: "fa fa fa-usd fa-2x",
            //    applyValidation: showAction, tooltip: "Ver listado de pagos", customStyle: "color:#757575"
            //}
        ];


        function showAction(item, action) {
            return true;
        }

        $scope.onLink = onLink;

        function onLink(actionName, entity) {
            //if (actionName == 'watchUserRequests') {
            //    $state.go("triangular.requests-list", { id: entity.id });
            //}
            //if (actionName == 'watchUserPayments') {
            //    $state.go("triangular.payments-list", { id: entity.id });
            //}

        }
        $scope.$on('ContextChanged', function (event, id) {
            $state.go('triangular.home-admin');
        });
        $rootScope.$broadcast('hideCustomContext', true);

        $scope.add = function () {
            $state.go("triangular.users-edit");
        }

        $scope.importCSV = function () {
            var parent = angular.element(document.body);
            $mdDialog.show({
                controller: 'ImportDialogController',
                templateUrl: '../Scripts/app/views/provider/import/import.tmpl.html',
                parent: parent,
                //targetEvent: event,
                locals: {
                    filesToRead:2
                },
                clickOutsideToClose: true
            }).then(function (response) {
                notificationService.displayInfo("Cargando archivo...");
                var importProviderList = [];
                var rejectedList = [];
                response.data.forEach(function (item) {
                    var itemLegalCondition = item["Condición ante AFIP"];
                    var providerDto = {
                        id: 0,
                        syncedWithERP: true,
                        cuit: item["CUIT"],
                        email: item["Correo electrónico"],
                        phoneNumber: item["Teléfono"],
                        contactFullName: item["Nombre de contacto"],
                        legalName: item["Nombre Legal"],
                        legalCondition: itemLegalCondition ? _.head(_.filter($scope.legalConditions, ["description", itemLegalCondition])) : null
                    }
                    if (itemLegalCondition && providerDto.legalCondition) {
                        importProviderList.push(providerDto);
                    } else {
                        rejectedList.push(item);
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
                    FileSaver.saveAs(blob, "Proveedores_rechazados_" + moment().format("DD_MM_YYYY HHmmss") + ".txt");
                }
                if (importProviderList.length > 0) {
                    notificationService.displayInfo("Guardando proveedores...");
                    apiService.post('api/provider/import', importProviderList, function (result) {
                        if (importProviderList.length == 1 && result.data.errors.length == importProviderList.length) {
                            notificationService.displayError("No se pudo importar el proveedor.");
                        } else {
                            notificationService.displaySuccess((importProviderList.length == 1 ? "Proveedor importado" : (((importProviderList.length - result.data.errors.length) > 1) ? "Proveedor importado" : " proveedores importados")) + " con éxito.");
                            if (result.data.errors.length) {
                                notificationService.displayError(result.data.errors.length == 1 ? "Un proveedor" : (result.data.errors.length + " proveedores no pudieron ser importados."));
                                var config = {
                                    header: true,
                                    delimiter: ",",
                                };

                                var unparsed = Papa.unparse(result.data.errors, config);
                                var blob = new Blob([unparsed], { type: 'text/csv;charset=utf-8;' });
                                FileSaver.saveAs(blob, "Proveedores_rechazados_por_servidor_" + moment().format("DD_MM_YYYY HHmmss") + ".txt");
                            }
                        }
                    }, function (error) {

                    });
                }
            }).catch(function (error) {
                if (error) {
                    notificationService.displayError(error);
                }
            });
        }
    }
})();