(function(app) {
    'use strict';

    app.directive('mvzPaginator', mvzPaginator);

    function mvzPaginator() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/scripts/app/directives/controls/mvzPaginator.html'
        }
    }

})(angular.module('common.ui'));