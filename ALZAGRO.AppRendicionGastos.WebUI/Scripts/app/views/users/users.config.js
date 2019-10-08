(function () {
    'use strict';

    angular
        .module('users-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
            .state('triangular.users-list', {
                url: '/users/list',
                templateUrl: '../Scripts/app/views/users/list/users-list.tmpl.html',
                controller: 'UsersListCtrl',
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
            .state('triangular.users-edit', {
                url: '/users/edit/:id',
                templateUrl: '../Scripts/app/views/users/edit/users-edit.tmpl.html',
                controller: 'UsersEditCtrl',
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
            .state('triangular.users-password', {
                url: '/users/change-password',
                templateUrl: '../Scripts/app/views/users/password/users-password.tmpl.html',
                controller: 'UsersPasswordCtrl',
                controllerAs: 'vm',
                data: {
                    permissions: {
                        only: ['viewAdmin', 'viewUser', 'viewSystemAdmin']
                    },
                    layout: {
                        contentClass: 'layout-column',
                    }
                }
            })
            .state('triangular.users-profile', {
                url: '/users/edit-profile',
                templateUrl: '../Scripts/app/views/users/profile/users-profile.tmpl.html',
                controller: 'UsersProfileCtrl',
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
            name: 'Usuarios',
            icon: 'fa fa-user',
            type: 'link',
            priority: 0.6,
            hideCustomContext: true,
            state: 'triangular.users-list',
            permission: ['viewSystemAdmin'],
        });

        triMenuProvider.addMenu({
            name: 'Mi Perfil',
            icon: 'fa fa-user',
            type: 'link',
            priority: 0.0,
            hideCustomContext: true,
            state: 'triangular.users-profile',
            permission: ['viewHidden'],
        });

        triMenuProvider.addMenu({
            name: 'Cambiar contraseña',
            hideCustomContext: true,
            icon: 'fa fa-user',
            type: 'link',
            priority: 0.0,
            state: 'triangular.users-password',
            permission: ['viewHidden'],
        });
    }
})();
