(function (app) {
    'use strict';
    app.directive('mvzTitle', mvzTitle);
    function mvzTitle($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                title: "@"
            },
            templateUrl: '/scripts/app/directives/controls/mvzTitle.html',
            replace: true

        }
    }
})(angular.module('common.ui'));

