(function () {
    'use strict';

    angular
        .module('expenses-module')
        .controller('ExpenseDetailsController', ExpenseDetailsController);


    ExpenseDetailsController.$inject = ['$scope', 'apiService', '$state', '$stateParams', '$q', '$window', 'expenseService', 'notificationService', '$rootScope', '$mdDialog'];

    function ExpenseDetailsController($scope, apiService, $state, $stateParams, $q, $window, expenseService, notificationService, $rootScope, $mdDialog) {

        $scope.hasPrivilege = $rootScope.repository &&
            $rootScope.repository.loggedUser &&
            $rootScope.repository.loggedUser.role != "Usuario";
        $scope.canEdit = false;
        $scope.receiptIsDisabled = false;
        var promises_array = [];
        $scope.forms = {};
        var errors_array = [];

        $scope.rotation = 0;

        $scope.rotateIcon = function (side) {
            if (side == "left") {
                $scope.rotation -= 90;
            } else {
                $scope.rotation += 90;
            }
            if ($scope.rotation < 0) {
                $scope.rotation = 360;
            }
            if ($scope.rotation > 360) {
                $scope.rotation = 0;
            }
        }

        //$scope.galleryIsInline = true;

        $scope.dateoptions = {
            maxDate: new Date(),
            minDate: moment().subtract(1, 'y').toDate()
        }

        $scope.payments = [];
        $scope.categories = [];
        $scope.aliquots = [];

        $scope.isEdit = $stateParams.isEdit;
        $scope.expense = {};
        $scope.images = [];
        $scope.previusExpense = null;
        $scope.nextExpense = null;

        ///// so left and right key work
        //var root = document.getElementById('expense-details');
        //root.click();

        //$scope.$on('onKeyDown', function (event, keyCode) {
        //    switch (keyCode) {
        //        case 39: $scope.goToNextExpense(); break;
        //        case 37: $scope.goToPreviusExpense(); break;
        //    }
        //});

        $scope.methods = {};
        $scope.methodsModal = {};

        $scope.$on('image-opened', function (event, id) {
            var options = {
                // optionName: 'option value'
                // for example:
                index: 0 // start at first slide
            };

            var pswpElement = document.querySelectorAll('.pswp')[0];
            var gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, $scope.images, options);
            gallery.init();
            //$rootScope.$broadcast('set-modal-index', id);
        });

        //$scope.onImageOpened = onImageOpened;

        //$scope.onModalClose = onModalClose;

        function format(mask, number) {
            var s = '' + number.replace(/-/g, ''), r = '';
            for (var im = 0, is = 0; im < mask.length && is < s.length; im++) {
                r += mask.charAt(im) == 'X' ? s.charAt(is++) : mask.charAt(im);
            }
            return r;
        }  

        function getFormattedReceipt() {
            if ($scope.expense.receipt) {
                return format('XXXX-XXXXXXXX', $scope.expense.receipt);
            } else {
                return 'N/A';
            }
        }

        $scope.getFormattedReceipt = getFormattedReceipt;

        function getValueOfFloat(floatToParse) {
            var result = expenseService.formatFloat(floatToParse);
            return result;
        }

        $scope.getValueOfFloat = getValueOfFloat;

        $scope.switchEdit = function () {
            if ($scope.isEdit) {
                /// guardar
                if ($scope.forms.expenseForm.$valid) {
                    notificationService.displayInfo("Guardando gasto...");
                    var pendingStatus = $rootScope.syncStatuses.filter(function (status) { return status.description == "Pendiente" })[0];
                    var dto = Object.assign({}, $scope.expense);
                    dto.syncStatus = pendingStatus;
                    apiService.post('api/expense', dto, function (response) {
                        replaceSyncStatusOfCurrentExpense(pendingStatus);
                        notificationService.displaySuccess("Gasto guardado con éxito.");
                        $scope.isEdit = false;
                        disableReceiptForm();
                    }, function (error) {
                        notificationService.serverError(error, "No se pudo guardar el gasto.");
                    });
                } else {
                    notificationService.displayError("El formulario se encuentra con errores.");
                }

            } else {
                /// habilitar editar.
                var changeStatusDto = {
                    id: $scope.expense.id,
                    syncStatus: _.head(_.filter($rootScope.syncStatuses, ["description", "Editando"])),
                    changeStatusReason: "Editar errores de carga.",
                    changeStatusNote: "N/A"
                }
                apiService.post('api/expense/changeStatus', changeStatusDto, function (response) {
                    if (response.data.success) {
                        $scope.expense.date = moment($scope.expense.date).toDate();
                        var editingStatus = $rootScope.syncStatuses.filter(function (status) { return status.description == "Editando" })[0];
                        replaceSyncStatusOfCurrentExpense(editingStatus);
                        disableReceiptForm();
                        $scope.isEdit = true;
                    }
                }, function (error) {
                    $scope.isEdit = false;
                    notificationService.serverError(error, "No se pudo tomar el gasto para editarlo.");
                });
               
            }
        }

        function disableReceiptForm() {
            $scope.expense.receipt = $scope.expense.receipt.length < 2 ? '' : $scope.expense.receipt;
            $scope.receiptIsDisabled = $scope.expense.category.description === 'Varios' && $scope.expense.aliquot.description == 'SIN IVA';
        }

        $scope.enteredViaNotifications = $stateParams.enteredViaNotifications;

        $scope.goToNextExpense = function () {
            if ($scope.nextExpense) {
                $state.go('triangular.expense-details', {
                    id: $scope.nextExpense.id,
                    data: $scope.nextExpense,
                    enteredViaNotifications: false
                });
                $scope.nextExpense = null;
            }

        }


        $scope.goToPreviusExpense = function () {
            if ($scope.previusExpense) {
                $state.go('triangular.expense-details', {
                    id: $scope.previusExpense.id,
                    data: $scope.previusExpense,
                    enteredViaNotifications: false
                });
                $scope.previusExpense = null;
            }
        }
       
  

        function getMappedObj() {
            var array = [];
            var expenseListObj = {
                id: $scope.expense.id,
                syncStatus: $scope.expense.syncStatus.description,
                date: $scope.expense.date,
                provider: $scope.expense.provider.legalName,
                paymentType: $scope.expense.aliquot.description,
                payment: $scope.expense.payment.description,
                category: $scope.expense.category.description,
                total: $scope.expense.total,
                exported: $scope.expense.exported,
                paymentId: $scope.expense.payment.id,
                user: $scope.expense.user
            }
            expenseListObj.user.fullName = expenseListObj.user.firstName + " " + expenseListObj.user.lastName;

            array.push(expenseListObj);

            return array;
        }

        function replaceSyncStatusOfCurrentExpense(newSyncStatus) {
            if ($rootScope.mvzTableData) {
                var indexOfCurrentExpense = _.indexOf($rootScope.mvzTableData, _.head(_.filter($rootScope.mvzTableData, ["id", Number($stateParams.id)])));
                if (indexOfCurrentExpense > -1) {
                    $rootScope.mvzTableData[indexOfCurrentExpense].syncStatus = newSyncStatus;
                }
            }
            $scope.expense.syncStatus = newSyncStatus;
        }

        $scope.reject = function () {
            var rejectedState = $rootScope.syncStatuses.filter(function (status) { return status.description == "Rechazado" })[0];
            var mapped = getMappedObj();
            expenseService.reject(mapped[0]).then(function () {
                if (!$scope.enteredViaNotifications) {
                    replaceSyncStatusOfCurrentExpense(rejectedState);
                } else {
                    $scope.goBack();
                }
            }).catch(function () {
                notificationService.serverError(error, "No se pudo rechazar el gasto.");
            });
        };

        $scope.approve = function () {
            var approvalState = $rootScope.syncStatuses.filter(function (status) { return status.description == "Aprobado" })[0];
            if ($scope.expense.syncStatus.description === "Rechazado") {
                var mapped = getMappedObj();
                expenseService.openApproveModal(mapped).then(function (data) {
                    expenseService.approve($scope.expense, data).then(function () {
                        if (!$scope.enteredViaNotifications) {
                            replaceSyncStatusOfCurrentExpense(approvalState);
                        } else {
                            $scope.goBack();
                        }
                    }).catch(function () {
                        notificationService.serverError(error, "No se pudo aprobar el gasto.");
                    });
                }).catch(function () { });
            } else {
                expenseService.approve($scope.expense).then(function () {
                    if (!$scope.enteredViaNotifications) {
                        replaceSyncStatusOfCurrentExpense(approvalState);
                    } else {
                        $scope.goBack();
                    }
                }).catch(function () {
                    notificationService.serverError(error, "No se pudo aprobar el gasto.");
                });
            }
            
        };


        $scope.goBack = function () {
            $state.go('triangular.expenses-list');
        }

        $scope.getFormattedDate = function () {
            if ($scope.expense) {
                return moment($scope.expense.date).format('DD/MM/YYYY');
            } else {
                return '';
            }
        }


        function calculatePreviusAndNextExpense() {
            if (!$scope.enteredViaNotifications) {
                var indexOfCurrentExpense = _.indexOf($rootScope.mvzTableData, _.head(_.filter($rootScope.mvzTableData, ["id", Number($stateParams.id)])));
                $scope.indexOfCurrentExpense = indexOfCurrentExpense;
                $scope.previusExpense = $rootScope.mvzTableData[indexOfCurrentExpense - 1] ? $rootScope.mvzTableData[indexOfCurrentExpense - 1] : null;
                $scope.nextExpense = $rootScope.mvzTableData[indexOfCurrentExpense + 1] ? $rootScope.mvzTableData[indexOfCurrentExpense + 1] : null;
            } else {
                $scope.previusExpense = null;
                $scope.nextExpense = null;
            }

        }

        function getExpense() {
            var defer = $q.defer();
            if (!$stateParams.id) {
                $state.go('triangular.expenses-list');
                defer.reject();
            } else {
                if ($stateParams.data) {
                    calculatePreviusAndNextExpense();
                }
                apiService.get('api/expense/' + $stateParams.id, null, function (response) {
                    if (response.data == null) {
                        notificationService.displayError("No se ha encontrado el gasto al que intentas acceder.");
                        $state.go('triangular.expenses-list');
                        defer.reject();
                    } else {
                        $scope.expense = response.data;
                        $scope.actionsEnabled = $scope.hasPrivilege && $scope.expense.syncStatus.description != 'Editando' && !$scope.expense.exported;
                        $scope.hasPrivilege = $scope.hasPrivilege && !$scope.expense.exported;
                        $scope.canEdit = $scope.hasPrivilege && (($scope.expense.updatedBy === $rootScope.repository.loggedUser.userId) || $scope.expense.syncStatus.description === "Pendiente");
                        $scope.$broadcast('ContextChanged', $scope.expense.companyId);
                        var host = $window.location.origin === "http://localhost:1487" ? "https://alzagroapp.mvzn.me" : $window.location.origin;
                        if (response.data.images.length > 0) {
                            response.data.images.forEach(function (image) {
                                /// TODO: Modify imagePath removing localUrl
                                var image = {
                                    src: host + '/' + image.path,
                                    w: 1024, // image width
                                    h: 768, // image height,
                                    id: image.id,
                                    url: host + '/' + image.path
                                }
                                $scope.images.push(image);
                            });
                            
                        }
                        if ($scope.canEdit && $scope.expense.syncStatus.description == 'Editando') {
                            $scope.expense.date = moment($scope.expense.date).toDate();
                            $scope.isEdit = true;
                            disableReceiptForm();
                        }
                        defer.resolve();
                    }
                }, function (error) {
                    defer.reject();
                    $state.go('triangular.expenses-list');
                });
            }
            return defer.promise;
        }


        //// viewLog

        $scope.viewExpenseStatusesLog = function (ev) {
            $mdDialog.show({
                controller: 'ExpenseStatusChangeLogCtrl',
                controllerAs: 'vm',
                templateUrl: '../Scripts/app/views/expenses/statuschangelog/statuschange-log.tmpl.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                locals: { expenseId: $scope.expense.id },
                clickOutsideToClose: true,
                fullscreen: true
            }).then(function (answer) {
                console.log('EXIT');
            }, function () {
                console.log('CANCEL EXIT');
            });
        };



        //// Edit

        function getPaymentOptions() {
            var defer = $q.defer();
            promises_array.push(defer.promise);

            apiService.get('api/payments/of/' + $scope.expense.userId, null, function (response) {
                $scope.payments = response.data;
                defer.resolve();
            }, function (error) {
                errors_array.push(error.data.message);
                defer.reject();
            });
        }

        function getAliquots() {
            var defer = $q.defer();
            promises_array.push(defer.promise);

            apiService.get('api/aliquot', null, function (response) {
                $scope.aliquots = response.data;
                defer.resolve();
            }, function (error) {
                errors_array.push(error.data.message);
                defer.reject();
             });
        }

        function getCategories() {
            var defer = $q.defer();
            promises_array.push(defer.promise);

            apiService.get('api/category/all', null, function (response) {
                $scope.categories = response.data;
                defer.resolve();
            }, function (error) {
                errors_array.push(error.data.message);
                defer.reject();
            });
        }

        function getSelectOptions() {
            getAliquots();
            getCategories();
            getPaymentOptions();
            return $q.all(promises_array);
        }

    

        //$scope.getMinDate = function () {
        //    return moment($scope.dateoptions.minDate).format('DD/MM/YYYY');
        //}

        //$scope.getMaxDate = function () {
        //    return moment($scope.dateoptions.maxDate).format('DD/MM/YYYY');
        //}

        $scope.onProviderSelected = function (provider) {
            if (provider) {
                $scope.expense.provider = provider;
            } 
        }

        $scope.onCategoryChanged = function (categoryId) {
            if (categoryId) {

            }
        }

        $scope.onFloatNumberChanged = function (floatNumber, control,decimals) {
            if (floatNumber) {
                if (floatNumber[0] === "$") {
                    floatNumber = floatNumber.substr(2);
                }
                var parsed = expenseService.formatFloat(floatNumber, false, decimals);
                //parsed = parsed.replace(/[.]/g, '');
                $scope.expense[control] = parsed;
            }
        }

        $scope.completeAllDigits = function (floatNumber, control) {
            $scope.onFloatNumberChanged(floatNumber, control, 2);
        }

        $scope.onDateChange = function (date) {
            var momentOfDate = null;
            if (date) {
                if (typeof date == 'object') {
                    momentOfDate = moment(date);
                }
                if (typeof date == "string") {
                    momentOfDate = moment(date, 'DD/MM/YYYY');
                }
                if (momentOfDate.isAfter(moment())) {
                    notificationService.displayError("No podés elegir una fecha posterior al día de hoy.");
                    $scope.expense.date = moment().toDate();
                } else {
                    $scope.expense.date = momentOfDate.toDate();
                }
            }
        };

        $scope.onAliquotSelected = function (aliquotID) {
            if (aliquotID) {

            }
        }

        $scope.onPaymentSelected = function (paymentId) {
            if (paymentId) {
                $scope.expense.payment = _.head(_.filter($scope.payments, ["id", paymentId]));
            }
        }

        $scope.onReceiptChanged = function (receiptText) {
            var mask = 'XXXX-XXXXXXXX';
            receiptText = receiptText.replace(/-/g, '').substr(0, mask.length-1);
            var formatted = format(mask, receiptText);
            var updateInterval = setInterval(function () {
                if ($scope.expense.receipt != formatted) {
                    $scope.expense.receipt = formatted;
                } else {
                    clearInterval(updateInterval);
                }
            }, 10);
        }

        getExpense().then(function () {
            getSelectOptions();
        }).catch(function () { });
        expenseService.getSyncStatuses();
    }
})();