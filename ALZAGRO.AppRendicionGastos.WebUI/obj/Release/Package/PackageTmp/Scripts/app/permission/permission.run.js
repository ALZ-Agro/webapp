(function () {
    'use strict';

    angular
        .module('app.permission')
        .run(permissionRun);

    /* @ngInject */
    function permissionRun($rootScope, $state, PermissionStore, RoleStore, membershipService, notificationService, triRoute, $window) {


        // create permissions and add check function verify all permissions
        var permissions = ['viewAdmin', 'viewUser', 'viewAnonymus', 'viewHidden', 'viewSystemAdmin', 'Semillas', 'Nutrientes', 'Mercados', 'Solidum'];


        PermissionStore.defineManyPermissions(permissions, function (permissionName) {
            return membershipService.hasPermission(permissionName);
        });


        // create roles for app
        RoleStore.defineManyRoles({
            'Administrador del sistema': ['viewSystemAdmin', 'viewAdmin', 'Semillas', 'Nutrientes', 'Mercados', 'Solidum'],
            'Administrativo': ['viewAdmin', 'Semillas', 'Nutrientes', 'Mercados', 'Solidum'],
            'Usuario': ['viewUser'],
            'Anonymus': ['viewAnonymus']
        });


        ///////////////////////

        // default redirect if access is denied
        function accessDenied(event, toState, toParams, options) {

            var currentUser = $rootScope.repository.loggedUser;

            if (toState.name != 'triangular.account.login') {
                notificationService.displayInfo("No tiene los permisos necesarios para ingresar a esta opción");
                membershipService.removeCredentials();
                $window.location.href = '/login';
            }
            if (currentUser.role) {
                if (currentUser.role === 'Administrador del sistema') {
                    $state.go('triangular.users-list');
                } else {
                    $state.go('triangular.home-admin');
                }

            } else {
                $state.go('401');
            }
            //switch (currentUser.role) {
            //    case "Administrador del sistema":
            //    case "Administrativo":
            //        $state.go('triangular.home-admin');
            //        break;
            //    case "Usuario":
            //        $state.go('triangular.home-admin');
            //        break;
            //    default:
            //        $state.go('401');
            //        break;
            //}
        }

        // watches

        // redirect all denied permissions to 401
        var deniedHandle = $rootScope.$on('$stateChangePermissionDenied', accessDenied);

        // remove watch on destroy
        $rootScope.$on('$destroy', function () {
            deniedHandle();
        });
    }

})();
