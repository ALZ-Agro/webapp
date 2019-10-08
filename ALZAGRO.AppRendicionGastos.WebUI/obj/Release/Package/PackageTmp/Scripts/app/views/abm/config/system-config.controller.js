(function () {
    'use strict';

    angular
        .module('abm-module')
        .controller('ConfigCtrl', ConfigCtrl);


    ConfigCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'ValidationService', '$rootScope'];

    function ConfigCtrl($scope, apiService, notificationService, $state, ValidationService, $rootScope) {
 
        var hideCustomContextInterval = setInterval(function () {
            $rootScope.$broadcast('hideCustomContext', true);
        }, 10);

        $scope.$on('hideCustomContextCompleted', function () {
            clearInterval(hideCustomContextInterval);
        });

        $scope.$on('ContextChanged', function (event, id) {
            $state.go('triangular.home-admin');
        });
        

        
        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                var config = Object.assign({}, $scope.config);
                apiService.post('/api/config', config, onUpdateCompleted);
            }
        }

        function onUpdateCompleted() {
            notificationService.displaySuccess("Configuración actualizada con éxito.");
            $rootScope.configParams = $scope.config;
            sessionStorage.setItem('configParams', JSON.stringify($scope.config));
        }

        $scope.cancel = function () {
            $state.go("triangular.home-admin");
        }

        function init() {
                apiService.get('api/config/', null, function (response) {
                    $scope.config = response.data;
                }, function (error) {
                    notificationService.serverError(error, "No se pudieron obtener los parámetros de configuración.");
                    $scope.cancel();
                });
        }

        init();
    }
})();