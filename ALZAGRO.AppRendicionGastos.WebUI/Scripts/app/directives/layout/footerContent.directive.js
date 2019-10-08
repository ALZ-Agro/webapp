(function(app) {
    'use strict';

    app.directive('footerContent', footerContent);

    function footerContent() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/scripts/app/directives/layout/footerContent.html'
        }
    }

})(angular.module('common.ui'));