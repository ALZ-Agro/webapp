(function (app) {
    'use strict';
    app.directive('mvzTextarea', mvzTextarea);
    function mvzTextarea($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: '=',
                label: "@",
                requiredMessage: "@",
                name: "@",
                type: "@",
                validationRules:'@',

            },
            templateUrl: '/scripts/app/directives/controls/mvzTextarea.html',
            replace: true,
            controller: function ($scope) {
                $scope.getLength = getLength;
                $scope.limit = 0;
                if ($scope.validationRules) {
                    $scope.limit = Number($scope.validationRules.split('max_length:')[1]);
                } else {
                    return 0;
                }
                function getLength() {
                    return String($scope.model).length;
                }
            }

        }
    }
})(angular.module('common.ui'));

