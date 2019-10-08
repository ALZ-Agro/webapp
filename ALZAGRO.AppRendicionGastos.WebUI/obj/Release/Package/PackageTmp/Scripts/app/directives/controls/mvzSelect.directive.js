(function (app) {
    'use strict';

    app.directive('mvzSelect', mvzSelect);

    function mvzSelect($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: '=',
                isDisabled: '=?',
                onSelect: "&",
                customOptions: "=",
                value: '@',
                display: '@',
                options: '=',
                placeholder: "@",
                label: "@",
                help: "@",
                isRequired: "=?",
                preventClear:"=?",
                requiredMessage: "@",
                name: "@",
                validationRules: "@",
                friendly: '=?',
                customClass: '=',
                customClassDinamic: '@',
                applyCustomClassDinamicWhen:'=?'

            },
            templateUrl: '/scripts/app/directives/controls/mvzSelect.html',
            replace: true,
            controller: function ($scope, $element) {
                if (angular.isUndefined($scope.isRequired)) {
                    $scope.isRequired = false;
                }

                if (angular.isUndefined($scope.customClassDinamic)) {
                    $scope.customClassDinamic = '';
                }

                if (angular.isUndefined($scope.isDisabled)) {
                    $scope.isDisabled = false;
                }
                if (angular.isUndefined($scope.customClass)) {
                    $scope.customClassAssigned = '';
                }
                if (angular.isUndefined($scope.applyCustomClassDinamicWhen)) {
                    $scope.applyCustomClassDinamicWhen = false;
                }

                $scope.$watch('customClass', function (newValue, oldValue) {
                    if (newValue != undefined) {
                        $scope.customClassAssigned = newValue;
                    }
                });

                if (angular.isUndefined($scope.validationRules) && $scope.isRequired == true) {
                    $scope.validationRules = 'required';
                }
                $scope.clear = clear;
                $scope.onClose = onClose;
              
                function clear() {
                    $scope.model = null;
                    onClose(null);     
                }

                function onClose(itemSelected) {
                    if (!angular.isUndefined($scope.onSelect())) {
                        $scope.onSelect()(itemSelected);
                    }
                }
            }
        }
    }
})(angular.module('common.ui'));