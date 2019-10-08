(function (app) {
    'use strict';

    app.factory('membershipService', membershipService);

    /* @ngInject */
    function membershipService(apiService, notificationService, $http, $base64, $rootScope, $q, $state, RoleStore) {

        var service = {
            login: login,
            register: register,
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn,
            hasPermission: hasPermission,
            getRememberedUser: getRememberedUser,
            getConfigParams: getConfigParams
        }
        var gettingConfigParams = false;
        var getConfigParamsInterval = setInterval(function () {
            if (angular.equals({}, $rootScope.contextConfig) && !gettingConfigParams) {
                gettingConfigParams = true;
                getConfigParams().then(function () {
                    clearInterval(getConfigParamsInterval);
                    gettingConfigParams = false;
                }).catch(function () {
                    gettingConfigParams = false;
                });
            } else {
                clearInterval(getConfigParamsInterval);
            }
        }, 10);

        function getConfigParams() {
            
            var defer = $q.defer();
            if (isUserLoggedIn()) {
                var configParams = sessionStorage.getItem('configParams');
                if (configParams) {
                    $rootScope.contextConfig = JSON.parse(configParams);
                    defer.resolve();
                } else {
                    apiService.get('api/config', null, function (response) {
                        $rootScope.contextConfig = response.data;
                        sessionStorage.setItem('configParams', JSON.stringify(response.data));
                        defer.resolve();
                    }, function (error) {
                        defer.reject();
                    });
                }
            } else {
                defer.reject();
            }
            return defer.promise;
        }

        function login(user, completed, failed) {
            if (!failed) {
                failed = loginFailed;
            }
            apiService.post('/api/account', user, completed, failed);
        }

        function getPermission(user, completed) {
            apiService.post('/api/userPermissions/search', user, completed, null);
        }

        function register(user, completed) {
            apiService.post('/api/account/register', user,
            completed,
            registrationFailed);
        }

        function hasPermission(permission) {
            var deferred = $q.defer();
            var hasPermission = false;
            var currentUser = $rootScope.repository.loggedUser;
            var role = currentUser ? currentUser.role : 'Anonymus';

            if (RoleStore.hasRoleDefinition(role)) {
                var roles = RoleStore.getStore();
                if (angular.isDefined(roles[role])) {
                    if (-1 != roles[role].validationFunction.indexOf(permission)) {
                        hasPermission = true;
                    }
                }
                if (currentUser && (permission == "Semillas" || permission == "Mercados" || permission == "Solidum" || permission == "Nutrientes")) {
                    if (currentUser.role == "Administrativo" || currentUser.role == "Administrador del sistema") {
                        hasPermission = true;
                    } else {
                        var mappedUserCompanies = _.map(currentUser.enabledCompanies, function (enabledCompanyGroup) {
                            return enabledCompanyGroup.company;
                        });
                        var userCompany = _.filter(mappedUserCompanies, ["name", permission]);
                        if (currentUser.enabledCompanies && currentUser.enabledCompanies.length > 0 && userCompany && userCompany.length > 0) {
                            hasPermission = true;
                        }
                    }
                }
            }

            // if we have permission resolve otherwise reject the promise
            if (hasPermission) {
                deferred.resolve();
            }
            else {
                deferred.reject();
            }

            // return promise
            return deferred.promise;
        }

        function saveCredentials(user) {
            if (user.email && user.password) {
                var membershipData = $base64.encode(user.email + ':' + user.password);

                $rootScope.repository = {
                    loggedUser: {
                        email: user.email,
                        userId: user.userId,
                        role: user.role.description,
                        authdata: membershipData,
                        fullName: user.fullName,
                        enabledCompanies: user.enabledCompanies,
                        showNotifications: user.showNotifications
                    }
                };

                if (user.rememberMe) {
                    var rememberedUser = {
                        email: user.email,
                        password: user.password,
                        rememberMe: user.rememberMe
                    }
                    localStorage.setItem('alzagro-rememberedUser', JSON.stringify(rememberedUser));
                }

                $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
                localStorage.setItem('alzagro-repository', JSON.stringify($rootScope.repository));
            }
        }

        function getRememberedUser() {
            var rememberedUser = localStorage.getItem('alzagro-rememberedUser');

            if (rememberedUser) {
                return JSON.parse(rememberedUser);
            }

            return null;
        }


        function removeCredentials() {
            $rootScope.repository = {};
            localStorage.removeItem('alzagro-repository');

            $http.defaults.headers.common.Authorization = undefined;
        };

        function loginFailed(response) {
            notificationService.displayError(response.data);
        }

        function registrationFailed(response) {
            notificationService.displayError('No se ha realizado el registro. Intente nuevamente por favor.');
        }

        function isUserLoggedIn() {
            return $rootScope.repository.loggedUser != null;
        }

        return service;
    }
})(angular.module('common.core'));