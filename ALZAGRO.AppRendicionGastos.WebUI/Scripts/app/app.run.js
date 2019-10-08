(function () {
    'use strict';

    angular
        .module('app')
        .run(runFunction)
        .constant('ENV', {
            decimalQuantities: 1
        });

    /* @ngInject */
    function runFunction($rootScope, $state, $http, notificationService, triMenu, $window) {
        var repositoryLS = localStorage.getItem('alzagro-repository');
        var currentSelectedCompanyLS = localStorage.getItem('currentSelectedCompany');
        $rootScope.repository = repositoryLS ? JSON.parse(repositoryLS) : {};
        $rootScope.contextConfig = {};
        $rootScope.currentSelectedCompany = currentSelectedCompanyLS ? JSON.parse(currentSelectedCompanyLS) : 1;
        $rootScope.onKeyDown = function (event) {
            $rootScope.$broadcast('onKeyDown', event.keyCode);
        }

        $rootScope.companies = [
            { id: 1, description: 'Semillas' },
            { id: 2, description: 'Nutrientes' },
            { id: 3, description: 'Mercados' },
            { id: 4, description: 'Solidum' }
        ];

        changeAppColours();

        function changeAppColours() {
            $rootScope.currentSelectedCompanyName = _.head(_.filter($rootScope.companies, ["id", $rootScope.currentSelectedCompany])).description;
            var html = document.getElementsByTagName('html')[0];
            /// check if browser supports css variables
            if (window.CSS && window.CSS.supports && window.CSS.supports('--fake-var', 0)) {
                switch ($rootScope.currentSelectedCompanyName) {
                    case "Semillas": html.style.setProperty("--currentCompanyColor", "#7BA529"); break;
                    case "Nutrientes": html.style.setProperty("--currentCompanyColor", "#F3C330"); break;
                    case "Mercados": html.style.setProperty("--currentCompanyColor", "#AF4D42"); break;
                    case "Solidum": html.style.setProperty("--currentCompanyColor", "#AF9442"); break;
                }
            }
        }

        $rootScope.$on('ContextChanged', function (event, id) {
            localStorage.setItem('currentSelectedCompany', id);
            if ($state.current.controller === 'ExpenseDetailsController') {
                $state.go('triangular.expenses-list');
            }
            $rootScope.currentSelectedCompany = id;

            changeAppColours();
            var menues = angular.copy(triMenu.menu);

            triMenu.removeAllMenu();
            menues.forEach(function (menu) {
                if (menu.contextVariable != undefined) {
                    menu.active = menu.contextVariable == id;
                }
                triMenu.addMenu(menu);
            });
        })

        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        } else {
            $state.go('triangular.account.login');
        }

        // default redirect if access is denied
        function redirectError() {
            notificationService.displayError("No tiene los permisos para ingresar a esa opción.");
            $state.go('triangular.account.login');
        }

        // watches

        // redirect all errors to permissions to 500
        var errorHandle = $rootScope.$on('$stateChangeError', redirectError);

        // remove watch on destroy
        $rootScope.$on('$destroy', function () {
            errorHandle();
        });
    }
})();
