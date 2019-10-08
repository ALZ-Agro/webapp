(function (app) {
    'use strict';
    app.directive('mvzEditor', mvzEditor);
    function mvzEditor($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: '=',
                label: "@",
                name: "@",
                type: "@",
                isDisabled: "=?",
                isReadonly:"=?",
                validationRules: "@",
                validationCallBack:"=?",
                maxLength: "@",
                min: '@',
                help: "@",
                step: '@',
                onChange: '&',
                onBlur: '&',
                onKeyUp: '&',
                onKeyDown: '&'
            },
            templateUrl: '/scripts/app/directives/controls/mvzEditor.html',
            replace: true,
            controller: function ($scope, $element) {
                if (angular.isUndefined($scope.isDisabled)) {
                    $scope.isDisabled = false;
                }

                $scope.onKeyDownCallback = function(event) {
                    return event.keyCode == 69 ? false : true
                }

                if (angular.isUndefined($scope.isReadonly)) {
                    $scope.isReadonly = false;
                }

                $scope.onChangeInput = function (data) {
                    if (!angular.isUndefined($scope.onChange())) {
                        $scope.onChange()(data,$scope.name);
                    }
                }

                $scope.onBlurInput = function (data) {
                    if (!angular.isUndefined($scope.onBlur())) {
                        $scope.onBlur()(data, $scope.name);
                    }
                }
            }
        }
    }
})(angular.module('common.ui'));

