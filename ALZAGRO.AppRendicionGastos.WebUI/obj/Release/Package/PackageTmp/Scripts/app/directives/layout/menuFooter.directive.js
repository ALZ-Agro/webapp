(function(app) {
    'use strict';

    app.directive('menuFooter', menuFooter);

    function menuFooter() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/scripts/app/directives/layout/menuFooter.html'
        }
    }

})(angular.module('common.ui'));