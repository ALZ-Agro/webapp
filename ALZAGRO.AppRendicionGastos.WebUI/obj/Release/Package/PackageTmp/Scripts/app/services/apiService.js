(function (app) {
    'use strict';

    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'notificationService', '$rootScope'];

    function apiService($http, notificationService, $rootScope, $state) {
        var service = {
            get: get,
            post: post,
            remove: remove
        };

        function get(url, config, success, failure) {
            return $http.get(url, config)
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Debe autenticarse para ingresar a esta opci&oacute;n.');
                            $rootScope.previousState = $state.is();
                            $state.go("triangular.account-login");
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                        else {
                            if (error.data){
                                if (error.data.length > 0 && error.data[0]) {
                                    notificationService.displayError(error.data);
                                }
                                else {
                                    if (error.data.message) {
                                        notificationService.displayError(error.data.message);
                                    }
                                    else {
                                        notificationService.displayError("Se produjo un error inesperado.");
                                    }
                                }
                            }
                            else {
                                notificationService.displayError("Se produjo un error inesperado.");
                            }
                        }
                    });
        }

        function post(url, data, success, failure) {
            return $http.post(url, data)
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Debe autenticarse para ingresar a esta opci&oacute;n.');
                            $rootScope.previousState = $state.is();
                            $state.go("triangular.account-login");
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                        else {
                            if (error.data) {
                                if (error.data.length > 0 && error.data[0]) {
                                    notificationService.displayError(error.data);
                                }
                                else {
                                    if (error.data.message) {
                                        notificationService.displayError(error.data.message);
                                    }
                                    else {
                                        notificationService.displayError("Se produjo un error inesperado.");
                                    }
                                }
                            }
                            else {
                                notificationService.displayError("Se produjo un error inesperado.");
                            }
                        }
                    });
        }

        function remove(url, success, failure) {
            return $http.delete(url).
                then(function (result) {
                    success(result);
                }, function (error) {
                    if (error.status == '401') {
                        notificationService.displayError('Debe autenticarse para ingresar a esta opci&oacute;n.');
                        $rootScope.previousState = $state.is();
                        $state.go("triangular.account-login");;
                    }
                    else if (failure != null) {
                        failure(error);
                    }
                    else {
                        if (error.data) {
                            if (error.data.length > 0 && error.data[0]) {
                                notificationService.displayError(error.data);
                            }
                            else {
                                if (error.data.message) {
                                    notificationService.displayError(error.data.message);
                                }
                                else {
                                    notificationService.displayError("Se produjo un error inesperado.");
                                }
                            }
                        }
                        else {
                            notificationService.displayError("Se produjo un error inesperado.");
                        }
                    }
                });
        };

        return service;
    }

})(angular.module('common.core'));