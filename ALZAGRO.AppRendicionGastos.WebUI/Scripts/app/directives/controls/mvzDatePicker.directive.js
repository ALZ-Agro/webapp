(function (app) {

    'use strict';

    app.directive('mvzDatePicker', mvzDatePicker);

    function mvzDatePicker($compile) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model: '=',
                options: "=",
                onChange: '&',
                label: "@",
                disabled: "=?",
                validationRules: "@",
                onBlur: '&',
                name: "@",
            },
            templateUrl: '/scripts/app/directives/controls/mvzDatePicker.html',
            replace: true,
            controller: function ($scope, $element) {
                $scope.id = '_' + Math.random().toString(36).substr(2, 9);
                if (angular.isUndefined($scope.disabled)) {
                    $scope.disabled = false;
                }

                $scope.onDateBlur = function (date) {
                    if (!angular.isUndefined($scope.onBlur())) {
                        var pickerElement = document.getElementById($scope.id)
                        if (pickerElement) {
                            var datePicker = pickerElement.childNodes[1];
                            if (datePicker) {
                                var input = datePicker.childNodes[0].value;
                                $scope.onBlur()(input);
                            }
                        }
                    }
                }

                $scope.onChangeSelected = function (date) {
                    if (!angular.isUndefined($scope.onChange())) {
                        $scope.onChange()(date);
                    }
                }
            }
        }
    }
})(angular.module('common.ui'));