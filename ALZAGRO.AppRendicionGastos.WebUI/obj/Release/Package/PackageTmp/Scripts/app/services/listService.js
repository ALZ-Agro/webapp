(function (app) {
    'use strict';

    app.factory('listService', listService);

    /* @ngInject */

    function listService(apiService, notificationService, sweetAlert) {

        var service = {
            getFilter: getFilter,
            getFilters: getFilters,
            getSelected: getSelected,
            getSelectedIndex: getSelectedIndex,
            remove: remove
        };

        return service;
        function remove(controllerName, entities, id, callback) {
            if (id) {
                sweetAlert.swal({
                    title: "Eliminar elemento",
                    text: "¿Desea eliminar este elemento?",
                    type: "error",
                    showCancelButton: true,
                    confirmButtonText: "CONFIRMAR",
                    confirmButtonColor: "",
                    cancelButtonColor: "",
                    confirmButtonClass: "md-primary md-button md-alz-content-theme md-ink-ripple",
                    cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                    cancelButtonText: "CANCELAR",
                    width: "400px",
                    padding: "90px"
                }).then(function (isConfirm) {
                    if (isConfirm) {
                        var r = true;
                    }
                    if (r == true) {
                        apiService.remove('/api/' + controllerName + '/' + id, function () {
                            notificationService.displaySuccess("El registro se ha eliminado con éxito.");
                            callback(getSelectedIndex(entities, id));
                        });
                    }

                }, function () {
                });

            }
            else {
                var entity = getSelected(entities);
                if (entity) {
                    sweetAlert.swal({
                        title: "Eliminar elemento",
                        text: "¿Desea eliminar este elemento?",
                        type: "error",
                        showCancelButton: true,
                        confirmButtonText: "ACEPTAR",
                        cancelButtonText: "CANCELAR",
                        confirmButtonColor: "",
                        cancelButtonColor: "",
                        confirmButtonClass: "md-primary md-button md-alz-content-theme md-ink-ripple",
                        cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                        width: "400px",
                        padding: "100px"
                    }).then(function (isConfirm) {
                        if (isConfirm) {
                            var r = true;
                        }
                        if (r == true) {
                            apiService.remove('/api/' + controllerName + '/' + entity.id, function () {
                                notificationService.displaySuccess("El registro se ha eliminado con éxito.");
                                callback(getSelectedIndex(entities, entity.id));
                            });
                        }
                    }, function () {
                    });

                }
            }
        };

        function getSelectedIndex(entities, id) {
            var index = 0;
            angular.forEach(entities, function (entity, key) {
                if (entity.id == id) {
                    index = key;
                }
            });
            return index;
        };

        function getSelected(entities) {
                var selectedItems = [];
                angular.forEach(entities, function (entity, key) {
                    if (entity.selected) {
                        selectedItems.push(entity);
                    }
                });
                //if (selectedItems.length == 1) {
                //    return selectedItems[0];
                //}
                //if (selectedItems.length >= 2) {
                //    //notificationService.displayError("No puede haber m&aacute;s de un elemento seleccionado.");
                    return selectedItems;
                //}
                //else {
                //    //notificationService.displayError("Debe seleccionar un elemento.");
                //    return null;
                //}
        };


        function getFilter(currentFilter, currentPage, pageSize) {
            var config = {
                params: {
                    page: currentPage,
                    pageSize: pageSize,
                    filter: currentFilter
                }
            };

            return config;
        };



        function getFilters(baseFilter, filter) {
            var config = {
                params: {
                    baseFilter: baseFilter,
                    filter: filter
                }
            };
            return config;
        };


    }

})(angular.module('common.core'));