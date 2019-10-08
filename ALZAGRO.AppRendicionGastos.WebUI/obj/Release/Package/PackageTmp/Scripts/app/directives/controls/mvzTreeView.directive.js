(function (app) {
    'use strict';
    app.directive('mvzTreeView', mvzTreeView);
    function mvzTreeView() {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                data: '='
            },
            templateUrl: '/scripts/app/directives/controls/mvzTreeView.html',
            replace: false,
            link: function (scope, element, attr) {
            },
            controller: function ($scope, $element) {
            }
        }
    }
})(angular.module('common.ui'));

