(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('PaymentEditCtrl', PaymentEditCtrl);


    PaymentEditCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$stateParams', '$state', 'ValidationService', '$rootScope', 'sweetAlert'];

    function PaymentEditCtrl($scope, apiService, notificationService, $stateParams, $state, ValidationService, $rootScope, sweetAlert) {
 
        $rootScope.$broadcast('hideCustomContext', true);
        var currentEntityId = $stateParams.id ? $stateParams.id : 0;

        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                var payment = Object.assign({}, $scope.payment);
                if (payment.user == null) {
                    sweetAlert.swal({
                        title: "Guardar tipo de pago",
                        text: 'Al parecer no asignaste esta forma de pago a ningún usuario, por lo que quedará liberada para ser usada por todos. ¿Estás seguro de continuar?',
                        type: 'error',
                        showCancelButton: true,
                        confirmButtonText: "ACEPTAR",
                        confirmButtonColor: "",
                        cancelButtonColor: "",
                        confirmButtonClass: "color-green md-button md-alz-content-theme md-ink-ripple",
                        cancelButtonClass: "md-button md-alz-content-theme md-ink-ripple cancel-color-text",
                        cancelButtonText: "CANCELAR",
                        width: "400px",
                        padding: "90px",

                    }).then(function () {
                        payment.userId = null;
                        apiService.post('/api/payments', payment, onUpdateCompleted);
                    }).catch(function (err) {
                       
                    });
                } else {
                    payment.userId = payment.user.id;
                    apiService.post('/api/payments', payment, onUpdateCompleted);
                }
            }
        }

        function onUpdateCompleted() {
            notificationService.displaySuccess("Forma de pago actualizada con éxito.");
            $scope.cancel();
        }
        
        $scope.cancel = function () {
            $state.go("triangular.payment-list");
        }

        function init() {
            if ($stateParams.id) {
                apiService.get('api/payments/' + $stateParams.id, null, function (response) {
                    $scope.payment = response.data;
                }, function (error) {
                    notificationService.serverError(error, "No se pudo obtener el tipo de pago.");
                    $scope.cancel();
                });
            }
            
        }

        init();
    }
})();