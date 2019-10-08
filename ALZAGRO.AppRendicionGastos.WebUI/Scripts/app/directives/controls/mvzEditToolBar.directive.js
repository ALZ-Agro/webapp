(function (app) {
    'use strict';
    app.directive('mvzEditToolBar', mvzEditToolBar);
    function mvzEditToolBar($compile, $state) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                cancelUrl: "@",
                saveCallback: "&",
                cancelCallback: "&",
                showCustomCancel: "@",
                showCustomSave: "@",
                customSaveLabel: "@"
            },
            templateUrl: '/scripts/app/directives/controls/mvzEditToolBar.html',
            replace: true,
            controller: function ($scope, $element) {

                if (angular.isUndefined($scope.showCustomCancel)) {
                    $scope.showCustomCancel = false;
                }
                if (angular.isUndefined($scope.showCustomSave)) {
                    $scope.showCustomSave = false;
                }

                $scope.cancel = cancel;

                function cancel() {
                    if ($scope.cancelUrl != null) {
                        $state.go($scope.cancelUrl);
                    }
                }
            }
        }
    }
})(angular.module('common.ui'));

