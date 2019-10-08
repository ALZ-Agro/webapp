(function () {
    'use strict';

    angular
        .module('users-module')
        .controller('UsersEditCtrl', UsersEditCtrl);


    UsersEditCtrl.$inject = ['$scope', 'apiService', 'notificationService',
        '$timeout', '$stateParams', '$state', '$http', 'ValidationService', '$rootScope', '$q'];

    function UsersEditCtrl($scope, apiService, notificationService, $timeout, $stateParams, $state, $http, ValidationService, $rootScope, $q) {
 
        $scope.user = {
            userCompanyGroups: []
        };
        $scope.userRoleOptions = [];
        $scope.userGroups = [];
        $scope.companies = [];
        $rootScope.$broadcast('hideCustomContext', true);
        var currentEntityId = $stateParams.id ? $stateParams.id : 0;
        $scope.isEditing = currentEntityId != 0;
        $scope.save = function () {
            if (new ValidationService().checkFormValidity($scope.form)) {
                var user = Object.assign({}, $scope.user);
                //user.userCompanyGroups.forEach(function (group) {
                //    group.company = _.head(_.filter($scope.companies, ["id", group.company.id]));
                //    group.userGroup = _.head(_.filter($scope.userGroups, ["id", group.userGroup.id]));
                //});
                user.role = _.head(_.filter($scope.userRoleOptions, ["id", $scope.user.role]));
                user.roleId = user.role.id;
                apiService.post('/api/users/', user, onUpdateCompleted);
            }
        }

        $scope.addNewUserCompanyGroup = function () {
            var companyGroupDto = {
                id: 0,
                userId: $scope.user.id,
                company: {},
                userGroup: {}
            }
            $scope.user.userCompanyGroups.push(companyGroupDto);
        }

        $scope.removeUserCompany = function (companyGroup,index) {
            $scope.user.userCompanyGroups.splice(index, 1);
            $scope.addCompanyOption(companyGroup.company.id);
        }

        $scope.cancel = function () {
            $state.go("triangular.users-list");
        }

        $scope.resetPassword = function () {
            apiService.post('/api/users/resetPassword', $scope.user, onResetPasswordCompleted);
        }

        $scope.addCompanyOption = function (clearedOption) {
            var clearedObj = _.head(_.filter($scope.companyOptions, ["id", clearedOption]));
            $scope.companies.push(clearedObj);
            $scope.companies = _.orderBy($scope.companies, ["id"], ["asc"]);
        }

        $scope.removeCompanyOption = removeCompanyOption;

        $scope.onGroupSelected = onGroupSelected;

        function onGroupSelected(selectedOption) {
            var selectedOptObj = _.head(_.filter($scope.userGroups, ["id", selectedOption]));
            $scope.user.userCompanyGroups.filter(function (userCompanyGroup) {
                if (userCompanyGroup.userGroup.id === selectedOption) {
                    userCompanyGroup.userGroup = selectedOptObj;
                }
            });
        }

        function removeCompanyOption(selectedOption) {
            var selectedOptObj = _.head(_.filter($scope.companyOptions, ["id", selectedOption]));
            $scope.user.userCompanyGroups.filter(function (userCompanyGroup) {
                if (userCompanyGroup.company.id === selectedOption) {
                    userCompanyGroup.company = selectedOptObj;
                }
            });
            $scope.companies = _.remove($scope.companies, function (company) {
                return company.id != selectedOption;
            });
        }

        $scope.hasSelectedUserRole = function () {
            if (!$scope.userRoleOptions || $scope.userRoleOptions.length === 0 || !$scope.user.role) {
                return false;
            }

            var userRoleDescription = $scope.userRoleOptions.filter(function (option) {
                return option.id === $scope.user.role;
            })[0].description;

            return userRoleDescription === "Usuario";
        }
        
        function init() {
            var promises = [];
            var defer = $q.defer();
            promises.push($q(function (resolve, reject) {
                apiService.get('api/users/roles', null, function (response) {
                    //if (response.data) {
                    //    response.data.forEach(function (role) {
                    //        switch (role.description) {
                    //            case "Usuario": role.friendlyName = "Vendedor"; break;
                    //            case "Admin": role.friendlyName = "Administrativo"; break;
                    //            case "SystemAdmin": role.friendlyName = "Administrador del sistema"; break;
                    //        }
                    //    });
                    //}
                    $scope.userRoleOptions = response.data;
                    resolve();
                }, function (error) {
                    notificationService.serverError(error, "No se pudieron obtener los roles.");
                    reject();
                });
            }));

            promises.push($q(function (resolve, reject) {
                apiService.get('api/users/groups', null, function (response) {
                    $scope.userGroups = response.data;
                    resolve();
                }, function (error) {
                    notificationService.serverError(error, "No se pudieron obtener los grupos de usuario.");
                    reject();
                });
            }));
            promises.push($q(function (resolve, reject) {
                apiService.get('api/company', null, function (response) {
                    $scope.companies = response.data;
                    $scope.companyOptions = [];
                    angular.copy(response.data, $scope.companyOptions);
                    resolve();
                }, function (error) {
                    notificationService.serverError(error, "No se pudieron obtener los grupos de usuario.");
                    reject();
                });
            }));

            return $q.all(promises);
        }

        function getUserData() {
            if (currentEntityId) {
                apiService.get('/api/users/get/' + currentEntityId, null, function onLoadUserCompleted(result) {
                    result.data.password = null;
                    if (!result.data.id_Erp || result.data.id_Erp === "0") {
                        result.data.id_Erp = null;
                    }
                    $scope.user = result.data;
                    $scope.user.role = $scope.user.role.id;
                    $scope.user.userCompanyGroups.forEach(function (usercompanygroup) {
                        $scope.removeCompanyOption(usercompanygroup.company.id);
                    });
                });
            }
           
        }


        function onUpdateCompleted(response) {
            notificationService.displaySuccess("El usuario " + $scope.user.email + " se ha guardado con éxito.");
            $timeout(function () {
                $state.go("triangular.users-list");
            }, 100);
        };

        function onResetPasswordCompleted(result) {
            notificationService.displaySuccess("La contraseña del usuario " + $scope.user.email +
                                               " se ha reseteado con éxito.");
            $timeout(function () {
                $state.go("triangular.users-list");
            }, 100);
        };

        init().then(function () {
            getUserData();
        }).catch(function () {
            $state.go('triangular.users-list');
        });
    }
})();