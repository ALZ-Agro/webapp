(function () {
    'use strict';

    angular
        .module('abm-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.reports-status-change', {
                    url: '/reports/expense-status-change',
                    templateUrl: '../Scripts/app/views/abm/reports/status-change/status-change.tmpl.html',
                    controller: 'StatusChangeReport',
                    controllerAs: 'vm',
                    data: {
                        permissions: {
                            only: ['viewSystemAdmin', 'viewAdmin']
                        },
                        layout: {
                            contentClass: 'layout-column'
                        }
                    }
            })
            .state('triangular.code-search', {
                url: '/search-code',
                templateUrl: '../Scripts/app/views/abm/code-search/code-search.tmpl.html',
                controller: 'CodeSearchCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin', 'viewAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                }
            })
                .state('triangular.payment-list', {
                url: '/payment/list',
                templateUrl: '../Scripts/app/views/abm/payment/list/payment-list.tmpl.html',
                controller: 'PaymentListCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                }
            })
            .state('triangular.payment-edit', {
                url: '/payment/edit/:id',
                templateUrl: '../Scripts/app/views/abm/payment/edit/payment-edit.tmpl.html',
                controller: 'PaymentEditCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                }
            })
            .state('triangular.category-list', {
                url: '/category/list',
                templateUrl: '../Scripts/app/views/abm/category/list/category-list.tmpl.html',
                controller: 'CategoryListCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                }
            })
            .state('triangular.category-edit', {
                url: '/category/edit/:id',
                templateUrl: '../Scripts/app/views/abm/category/edit/category-edit.tmpl.html',
                controller: 'CategoryEditCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column'
                    }
                }
            })
            .state('triangular.configuration', {
                url: '/config',
                templateUrl: '../Scripts/app/views/abm/config/system-config.tmpl.html',
                controller: 'ConfigCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                }
            });

        triMenuProvider.addMenu({
            name: 'Forma de pago',
            icon: 'fa fa-credit-card-alt',
            type: 'link',
            priority: 8.0,
            hideCustomContext: true,
            state: 'triangular.payment-list',
            permission: ['viewSystemAdmin']
        });
        triMenuProvider.addMenu({
            name: 'Buscador de código',
            icon: 'fa fa-search',
            type: 'link',
            priority: 6.5,
            hideCustomContext: true,
            state: 'triangular.code-search',
            permission: ['viewSystemAdmin', 'viewAdmin']
        });
        triMenuProvider.addMenu({
            name: 'Categorías',
            icon: 'fa fa-list',
            type: 'link',
            priority: 7.0,
            hideCustomContext: true,
            state: 'triangular.category-list',
            permission: ['viewSystemAdmin'],
        });
        triMenuProvider.addMenu({
            name: 'Configuración',
            icon: 'fa fa-cog',
            type: 'link',
            priority: 9.0,
            hideCustomContext: true,
            state: 'triangular.configuration',
            permission: ['viewSystemAdmin']
        });
        triMenuProvider.addMenu({
            name: 'Cambios de estado',
            icon: 'fa fa-exchange',
            type: 'link',
            priority: 8.0,
            hideCustomContext: true,
            state: 'triangular.reports-status-change',
            permission: ['viewSystemAdmin', 'viewAdmin']
        });
    }
})();
