//https://github.com/danialfarid/ng-file-upload

(function (app) {
    'use strict';
    app.directive('mvzFileUpload', mvzFileUpload);
    function mvzFileUpload($compile, $timeout, $mdToast, $rootScope, notificationService, Upload) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                model:'=',
                multipleFiles: '=',
                entity: '@',
                label: "@",
                name: "@",
                onActionCallback: '&',
                onRemoveCallback: '&'
            },
            templateUrl: '/scripts/app/directives/controls/mvzFileUpload.html',
            replace: false,
            link: function (scope, element, attr) {

                $rootScope.upload = [];
                scope.status = 'idle';  // idle | uploading | complete
                scope.upload = onUpload;
                scope.removeFile = removeFile;
                scope.files = [];

                function onUpload($files) {
                    if ($files.length != 0) {
                        scope.status = 'uploading';
                        for (var i = 0; i < $files.length; i++) {
                            var $file = $files[i];
                            (function (index) {
                                $rootScope.upload[index] = Upload.upload({
                                    url: "/api/fileUpload//upload?entity=" + scope.entity,
                                    method: "POST",
                                    headers: {
                                        'Accept': "application/json",
                                        'Content-Type': undefined
                                    },
                                    file: $file
                                }).progress(function (evt) {
                                }).success(function (data, status, headers, config) {
                                    notificationService.displaySuccess(data.fileName + ' se ha subido con éxito.');
                                    var file = {};
                                    file.path = "/Resources/" + data.fileName;
                                    file.fileName = data.fileName;
                                    if (scope.multipleFiles == true) {
                                        scope.files.push(file);
                                    }
                                    else {
                                        scope.files = [];
                                        scope.files.push(file);
                                    }

                                    scope.status = 'idle';
                                    onAction(data.fileName);
                                }).error(function (data, status, headers, config) {
                                    notificationService.displayError(data.Message);
                                });
                            })(i);
                        }
                    }

                }

                function removeFile(index) {
                    scope.files.splice(index, 1);
                    scope.onRemoveCallback()();
                }

                function onAction(item) {
                    scope.onActionCallback()(item);
                };
            },
            controller: function ($scope, $element) {
            }
        }
    }
})(angular.module('common.ui'));

