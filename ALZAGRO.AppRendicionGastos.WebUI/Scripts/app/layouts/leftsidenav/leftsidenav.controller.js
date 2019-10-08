(function () {
    'use strict';

    angular
        .module('triangular.components')
        .controller('LeftSidenavController', LeftSidenavController);

    /* @ngInject */
    function LeftSidenavController(triSettings, triLayout, triRoute, $state) {
        var vm = this;
        vm.layout = triLayout.layout;
        vm.sidebarInfo = {
            appName: triSettings.name,
            appLogo: triSettings.logo
        };


        var repository = localStorage.getItem('alzagro-repository');
        if (repository) {
            var parsed = JSON.parse(repository);
            vm.loggedUser = Object.keys(parsed.loggedUser).length > 0;
        } else {
            vm.loggedUser = false;
        }

        vm.toggleIconMenu = toggleIconMenu;
        vm.gohome = goHome;

        function goHome() {
            $state.go('triangular.home-admin');
        }

        function toggleIconMenu() {
            var menu = vm.layout.sideMenuSize == 'icon' ? 'full' : 'icon';
            triLayout.setOption('sideMenuSize', menu);

            //"../Content/images/logo-foas-small.png"
        }
    }
})();
