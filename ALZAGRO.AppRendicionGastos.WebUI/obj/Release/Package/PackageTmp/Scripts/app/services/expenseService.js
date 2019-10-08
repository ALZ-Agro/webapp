(function (app) {
    'use strict';

    app.factory('expenseService', expenseService);

    /* @ngInject */

    function expenseService(apiService, notificationService, sweetAlert, $q, $rootScope, $filter) {

        var service = {
            approve: approve,
            reject: reject,
            openRejectModal: openRejectModal,
            openApproveModal: openApproveModal,
            changeStatus: changeStatus,
            getSyncStatuses: getSyncStatuses,
            formatFloat: formatFloat,
            formatMoney: formatMoney
        };

        return service;

        function formatMoney(number, decimalCount, decimalSeparator, thousandSeparator) {
            var decimalCount = isNaN(decimalCount = Math.abs(decimalCount)) ? 2 : decimalCount,
                decimalSeparator = decimalSeparator == undefined ? "." : decimalSeparator,
                thousandSeparator = thousandSeparator == undefined ? "," : thousandSeparator,
                s = parseFloat(number) < 0 ? "-" : "",
                i = String(parseInt(number = Math.abs(Number(number) || 0).toFixed(decimalCount))),
                j = (j = i.length) > 3 ? j % 3 : 0;

            return s + (j ? i.substr(0, j) + thousandSeparator : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousandSeparator) + (decimalCount ? decimalSeparator + Math.abs(number - i).toFixed(decimalCount).slice(2) : "");
        };

        function formatFloat(total, addSign,decimals) {
            if (angular.isUndefined(addSign)) {
                addSign = true
            }
            if (angular.isUndefined(decimals)) {
                decimals = 0;
            }
            var string = total.toString();
            total = parseFloat(string);
            var stringifiedTotal = String(total);
            if (!decimals) {
                var indexOfSeparator = stringifiedTotal.indexOf('.');
                if (indexOfSeparator != -1) {
                    var decimalPart = stringifiedTotal.substr(indexOfSeparator + 1, stringifiedTotal.length);
                    if (decimalPart.length > 1) {
                        decimals = 2;
                    } else {
                        decimals = 1;
                    }

                }
            }
           
            var format = $filter('currency')(total, '$', decimals).replace(/[,.]/g, function(m) {
                return m === ',' ? '.' : '.';
            });
            if (addSign) {
                return format;
            }
            return format.substr(2);
        }

        function getSyncStatuses() {
            var defer = $q.defer();
            if (!$rootScope.syncStatuses) {
                $rootScope.syncStatuses = [];
                apiService.get('api/syncStatus', null, function (response) {
                    var syncStatuses = _.orderBy(response.data.filter(function (item) { return item.description != "Nuevo" }), ["description"], ["asc"]);
                    $rootScope.syncStatuses = syncStatuses;
                    defer.resolve();
                }, function (error) {
                    defer.reject();
                });
            } else {
                defer.resolve();
            }
            return defer.promise;
        }

        function reject(item) {
            var defer = $q.defer();
            var array = [];
                array.push(item);
            openRejectModal(array).then(function (data) {
                changeStatus(item, "Rechazado", data).then(function () {
                    defer.resolve();
                }).catch(function () {
                    defer.reject();
                });
            }).catch(function () {
                defer.reject();
            });

            return defer.promise;
        }

        function approve(item, data) {
            var defer = $q.defer();        
            changeStatus(item, "Aprobado", data).then(function () {
                defer.resolve();
            }).catch(function () {
                defer.reject();
            });
            return defer.promise;
        }


        function changeStatus(element, newStatus, changeStatusData) {
            var defer = $q.defer();
            var changeStatusDto = {
                id: element.id,
                syncStatus: _.head(_.filter($rootScope.syncStatuses, ["description", newStatus])),
                ChangeStatusReason: changeStatusData ? changeStatusData.reason : '',
                ChangeStatusNote: changeStatusData ? changeStatusData.note : ''
            }
            apiService.post('api/expense/changeStatus', changeStatusDto, function (response) {
                defer.resolve();
            }, function (error) {
                defer.reject();
            });
            return defer.promise;
        }

        function openRejectModal(expenses) {
            var defer = $q.defer();
            var selectOptions = '';
            $rootScope.refusalReasons.forEach(function (reason) {
                selectOptions += '<option value="' + reason.id + '">' + reason.description + '</option>';
            });
            var inputCheckInterval = setInterval(function () {
                var input = document.getElementById('notas').value;
                if (input != undefined && input != "") {
                    swal.enableConfirmButton();
                } else {
                    swal.disableConfirmButton();
                }
            }, 20);
            var html = '';
            if (expenses.length > 1) {
                html += '<p>Serán rechazados los ' + expenses.length + ' gastos seleccionados.</p><p>¿Está seguro?</p>'
            } else {
                var formattedTotal = formatFloat(expenses[0].total);
                html += "<h5>¿Está seguro de rechazar este gasto?</h5>";
                html += '<div class="swal-grid-container">'+
                    '<div class="swal-grid-item">Usuario</div>' +
                    '<div class="swal-grid-item">' + expenses[0].user.fullName + '</div > ' +
                    '<div class="swal-grid-item">Vendedor</div > ' +
                    '<div class="swal-grid-item">' + expenses[0].provider + '</div > ' +
                    '<div class="swal-grid-item">Categoría</div > ' +
                    '<div class="swal-grid-item">' + expenses[0].category + '</div > ' +
                    '<div class="swal-grid-item">Forma de pago</div > ' +
                    '<div class="swal-grid-item">' + expenses[0].payment + '</div > ' +
                    '<div class="swal-grid-item">Tipo de pago</div > ' +
                    '<div class="swal-grid-item">' + expenses[0].paymentType + '</div > ' +
                    '<div class="swal-grid-item">Monto</div > ' +
                    '<div class="swal-grid-item">' + formattedTotal + '</div > ' +
                    '<div class="swal-grid-item">Fecha</div > ' +
                    '<div class="swal-grid-item">' + moment(expenses[0].date).format('DD-MM-YYYY') + '</div > ' +
                    '</div>'
            }
            html += '<md-input-container class="md-alz-content-theme md-block md-input-has-placeholder sa-select-input margin-bottom-30 margin-top-40"><label for="reason" style="top:-24px;left:-40px;color: rgba(0,0,0,0.54);">Razón de rechazo</label>' +
                '<select class="swal2-select" id="reason" style="display: block;margin: 0 auto;border: none;color:black;padding-left: 0;border-bottom: 1px solid black;width: 70%;outline: none !important;">' + selectOptions + '</select></md-input-container>' +
                '<md-input-container class="md-alz-content-theme md-block md-input-has-placeholder" style="width: 70%;border: 0;margin: 0 auto;outline: 0;">' +
                '<label for="notes" style="top:-24px;left: -32%;color: rgba(0,0,0,0.54);">Notas</label><textarea  ng-change="onInput()" class="swal2-textarea" id="notas" placeholder="" style="display: block;box-shadow: none;margin: 0;outline: 0;border: 0;border-bottom: 1px solid black;"></textarea></md-input-container>'
            sweetAlert.swal({
                title: "Rechazar gasto" + (expenses.length > 1 ? 's' : ''),
                type: '',
                showCancelButton: true,
                confirmButtonText: "Rechazar",
                confirmButtonColor: "",
                cancelButtonColor: "",
                confirmButtonClass: "color-green md-button md-alz-content-theme md-ink-ripple",
                cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                cancelButtonText: "CANCELAR",
                width: "400px",
                padding: "90px",
                html: html

            }).then(function (data) {
                clearInterval(inputCheckInterval);
                var data = {
                    note: document.getElementById('notas').value,
                    reason: _.head(_.filter($rootScope.refusalReasons, ["id", Number(document.getElementById('reason').value)])).description
                }
                defer.resolve(data);
            }).catch(function (err) {
                defer.reject();
            });
            swal.disableConfirmButton();
            return defer.promise;
        }

        function openApproveModal(expenses) { 
                var defer = $q.defer();
                var selectOptions = '';
                $rootScope.approvalReasons.forEach(function (reason) {
                    selectOptions += '<option value="' + reason.id + '">' + reason.description + '</option>';
                });
                var inputCheckInterval = setInterval(function () {
                    var input = document.getElementById('notas').value;
                    if (input != undefined && input != "") {
                        swal.enableConfirmButton();
                    } else {
                        swal.disableConfirmButton();
                    }
                }, 20);
                var formattedTotal = formatFloat(expenses[0].total);
                var html = '<h5>Este gasto fue rechazado con anterioridad</h5>';
                    html += "<h5>¿Está seguro de aprobarlo?</h5>";
                    html += '<div class="swal-grid-container">' +
                        '<div class="swal-grid-item">Usuario</div>' +
                        '<div class="swal-grid-item">' + expenses[0].user.fullName + '</div > ' +
                        '<div class="swal-grid-item">Vendedor</div > ' +
                        '<div class="swal-grid-item">' + expenses[0].provider + '</div > ' +
                        '<div class="swal-grid-item">Categoría</div > ' +
                        '<div class="swal-grid-item">' + expenses[0].category + '</div > ' +
                        '<div class="swal-grid-item">Forma de pago</div > ' +
                        '<div class="swal-grid-item">' + expenses[0].payment + '</div > ' +
                        '<div class="swal-grid-item">Tipo de pago</div > ' +
                        '<div class="swal-grid-item">' + expenses[0].paymentType + '</div > ' +
                        '<div class="swal-grid-item">Monto</div > ' +
                        '<div class="swal-grid-item">' + formattedTotal + '</div > ' +
                        '<div class="swal-grid-item">Fecha</div > ' +
                        '<div class="swal-grid-item">' + moment(expenses[0].date).format('DD-MM-YYYY') + '</div > ' +
                        '</div>'
                html += '<md-input-container class="md-alz-content-theme md-block md-input-has-placeholder sa-select-input margin-bottom-30 margin-top-40"><label for="reason" style="top:-24px;left:-40px;color: rgba(0,0,0,0.54);">Razón de aprobación</label>' +
                    '<select class="swal2-select" id="reason" style="display: block;margin: 0 auto;border: none;color:black;padding-left: 0;border-bottom: 1px solid black;width: 70%;outline: none !important;">' + selectOptions + '</select></md-input-container>' +
                    '<md-input-container class="md-alz-content-theme md-block md-input-has-placeholder" style="width: 70%;border: 0;margin: 0 auto;outline: 0;">' +
                    '<label for="notes" style="top:-24px;left: -32%;color: rgba(0,0,0,0.54);">Notas</label><textarea  ng-change="onInput()" class="swal2-textarea" id="notas" placeholder="" style="display: block;box-shadow: none;margin: 0;outline: 0;border: 0;border-bottom: 1px solid black;"></textarea></md-input-container>'
                sweetAlert.swal({
                    title: "Aprobar gasto",
                    type: '',
                    showCancelButton: true,
                    confirmButtonText: "Aprobar",
                    confirmButtonColor: "",
                    cancelButtonColor: "",
                    confirmButtonClass: "color-green md-button md-alz-content-theme md-ink-ripple",
                    cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                    cancelButtonText: "CANCELAR",
                    width: "400px",
                    padding: "90px",
                    html: html

                }).then(function (data) {
                    clearInterval(inputCheckInterval);
                    var data = {
                        note: document.getElementById('notas').value,
                        reason: _.head(_.filter($rootScope.approvalReasons, ["id", Number(document.getElementById('reason').value)])).description
                    }
                    defer.resolve(data);
                }).catch(function (err) {
                    defer.reject();
                });
                swal.disableConfirmButton();
                return defer.promise;
            }
        }

})(angular.module('common.core'));