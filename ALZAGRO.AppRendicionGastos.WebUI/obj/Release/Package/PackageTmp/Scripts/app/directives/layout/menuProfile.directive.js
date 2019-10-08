(function(app) {
    'use strict';

    app.directive('menuProfile', menuProfile);

    function menuProfile() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/scripts/app/directives/layout/menuProfile.html'
        }
    }

})(angular.module('common.ui'));