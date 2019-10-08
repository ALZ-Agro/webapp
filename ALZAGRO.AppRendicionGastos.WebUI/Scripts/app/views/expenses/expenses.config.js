(function () {
    'use strict';

    angular
        .module('expenses-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.expenses-list', {
                url: '/expenses/list',
                templateUrl: '../Scripts/app/views/expenses/list/expenses-list.tmpl.html',
                controller: 'ExpensesListCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewAdmin', 'viewUser']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                },
                params: {
                    isReport: null
                }
            })
            .state('triangular.expense-details', {
                url: '/expense/:id',
                templateUrl: '../Scripts/app/views/expenses/details/expense-details.tmpl.html',
                controller: 'ExpenseDetailsController',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewAdmin', 'viewUser']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                },
                params: {
                    id: null,
                    data: null,
                    enteredViaNotifications: null,
                },
            });
        
        triMenuProvider.addMenu({
            name: 'Gastos',
            icon: 'alz-coins',
            type: 'link',
            priority: 2,
            state: 'triangular.expenses-list',
            permission: ['viewAdmin', 'viewUser'],
        });
        triMenuProvider.addMenu({
            name: 'Gastos',
            icon: 'alz-coins',
            type: 'dropdown',
            permission: ['viewHidden'],
            priority: 2.1,
            children: [{
                name: 'Detalles',
                icon: 'zmdi zmdi-accounts-alt',
                type: 'link',
                state: 'triangular.expense-details',
            }],
        });
    }
})();
