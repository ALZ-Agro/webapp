(function () {
    'use strict';

    angular
        .module('payments-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.payments-list', {
                url: '/payments/list/:id',
                templateUrl: '../Scripts/app/views/payments/list/payments-list.tmpl.html',
                controller: 'PaymentsListCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column',
                        sideMenuSize: 'full',
                    }
                }
            })
    }
})();
