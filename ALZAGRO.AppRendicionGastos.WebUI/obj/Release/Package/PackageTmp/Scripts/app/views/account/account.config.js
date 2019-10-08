(function () {
    'use strict';

    angular
        .module('account-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
        .state('triangular.account', {
            abstract: true,
            templateUrl: '/Scripts/app/views/account/layouts/authentication.tmpl.html',
            data: {
                permissions: {
                    only: ['viewAnonymus']
                },
                layout: {
                    showToolbar: false,
                    toolbarShrink: true,
                    toolbarClass: 'none',
                    contentClass: 'layout-column',
                    sideMenuSize: 'off',
                    footer: false
                }
            }
        })

        .state('triangular.account.login', {
            url: '/login',
            templateUrl: '/Scripts/app/views/account/login/login.tmpl.html',
            controller: 'LoginCtrl',
            controllerAs: 'vm',
        })

        .state('triangular.account.forgot', {
            url: '/forgot',
            templateUrl: '/Scripts/app/views/account/forgot/forgot.tmpl.html',
            controller: 'ForgotCtrl',
            controllerAs: 'vm',
        })
    }
})();
