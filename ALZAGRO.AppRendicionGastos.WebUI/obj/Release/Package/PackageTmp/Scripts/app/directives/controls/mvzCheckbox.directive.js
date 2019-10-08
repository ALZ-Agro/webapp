(function (app) {
    'use strict';
    app.directive('mvzCheckbox', mvzCheckbox);
    function mvzCheckbox($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: "=",
                label: "@",
                name: "@",
            },
            templateUrl: '/scripts/app/directives/controls/mvzCheckbox.html',
            replace: true

        }
    }
})(angular.module('common.ui'));

