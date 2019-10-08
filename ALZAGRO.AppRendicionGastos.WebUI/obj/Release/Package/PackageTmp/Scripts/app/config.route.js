(function () {
    'use strict';

    angular
        .module('app')
        .config(routeConfig);

    /* @ngInject */
    function routeConfig($stateProvider, $urlRouterProvider, $locationProvider) {
        // Setup the apps routes

         //404 & 500 pages
        $stateProvider
        .state('404', {
            url: '/login',
            templateUrl: 'Scripts/app/views/account/login/login.tmpl.html',
            controller: 'LoginCtrl',
            controllerAs: 'vm',
            data: {
                layout: {
                    showToolbar: false,
                    toolbarShrink: true,
                    toolbarClass: 'none',
                    contentClass: 'layout-column',
                    sideMenuSize: 'off',
                    footer: true
                }
            }
        })

        .state('401', {
            url: '/login',
        })

        .state('500', {
            url: '/login',
            templateUrl: 'Scripts/app/views/account/login/login.tmpl.html',
            controller: 'LoginCtrl',
            controllerAs: 'vm',
            data: {
                layout: {
                    showToolbar: false,
                    toolbarShrink: true,
                    toolbarClass: 'none',
                    contentClass: 'layout-column',
                    sideMenuSize: 'off',
                    footer: true
                }
            }
        });

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

        $urlRouterProvider.otherwise(function ($injector, $location) {
            var repositorySS = localStorage.getItem('repository') || null;
            var repository = repositorySS ? JSON.parse(repositorySS) : null;
            if (repositorySS && (repository && Object.keys(repository.loggedUser).length > 0)) {
                return '/home';
            } else {
                return '/login';
            }
        });
    }
})();
