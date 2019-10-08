(function () {
    'use strict';

    angular
        .module('home-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.home-admin', {
                url: '/home',
                templateUrl: '../Scripts/app/views/home/admin/admin-home.tmpl.html',
                controller: 'AdminHomeCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewAdmin', 'viewUser']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                }
            });
      
        triMenuProvider.addMenu({
            name: 'Resumen',
            icon: 'alz-chart-bar',
            type: 'link',
            priority: 1.0,
            state: 'triangular.home-admin',
            permission: ['viewAdmin', 'viewUser']
        });
    }
})();
