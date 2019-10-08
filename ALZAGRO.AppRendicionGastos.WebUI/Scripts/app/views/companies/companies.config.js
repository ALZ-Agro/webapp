(function () {
    'use strict';

    angular
        .module('companies-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider, $rootScopeProvider) {

        triMenuProvider.addMenu({
            name: 'Semillas',
            icon: 'alz-seed',
            type: 'link',
            contextVariable: 1,
            priority: 0.1,
            state: 'triangular.home-admin',
            permission: ['Semillas'],
        });
        triMenuProvider.addMenu({
            name: 'Nutrientes',
            icon: 'alz-seeding',
            contextVariable: 2,
            type: 'link',
            priority: 0.2,
            state: 'triangular.home-admin',
            permission: ['Nutrientes'],
        });
        triMenuProvider.addMenu({
            name: 'Mercados',
            icon: 'fa fa-usd',
            type: 'link',
            contextVariable: 3,
            priority: 0.3,
            state: 'triangular.home-admin',
            permission: ['Mercados'],
        });
        triMenuProvider.addMenu({
            name: 'Solidum',
            icon: 'alz-solidum',
            type: 'link',
            contextVariable: 4,
            priority: 0.4,
            state: 'triangular.home-admin',
            permission: ['Solidum'],
        });

    }
})();
