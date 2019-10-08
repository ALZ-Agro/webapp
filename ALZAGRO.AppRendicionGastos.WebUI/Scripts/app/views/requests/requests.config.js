(function () {
    'use strict';

    angular
        .module('requests-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.requests-list', {
                url: '/requests/list/:id',
                templateUrl: '../Scripts/app/views/requests/list/requests-list.tmpl.html',
                controller: 'RequestsListCtrl',
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
