(function () {
    'use strict';

    angular
        .module('provider-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.provider-list', {
                url: '/providers',
                templateUrl: '../Scripts/app/views/provider/list/provider-list.tmpl.html',
                controller: 'ProviderListCtrl',
                controllerAs: 'vm',
                
                data: {
                    permissions: {
                        only: ['viewAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                }
            })
            .state('triangular.provider-details', {
                url: '/provider/:id',
                templateUrl: '../Scripts/app/views/provider/details/provider-details.tmpl.html',
                controller: 'ProviderDetailsCtrl',
                controllerAs: 'vm',
                
                data: {
                    permissions: {
                        only: ['viewAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                },
                params: {
                    id: null,
                    data: null
                },
            });

        triMenuProvider.addMenu({
            name: 'Proveedores',
            icon: 'alz-users',
            type: 'link',
            hideCustomContext: true,
            priority: 6.0,
            state: 'triangular.provider-list',
            permission: ['viewAdmin'],
        });
    }
})();
