(function() {
    angular
        .module('app')
        .controller('ErrorPageController', ErrorPageController);

    /* @ngInject */
    function ErrorPageController($state, triRoute) {
        var vm = this;

        vm.goHome = goHome;

        /////////

        function goHome() {
            $state.go('triangular.account.login');
        }
    }
})();
