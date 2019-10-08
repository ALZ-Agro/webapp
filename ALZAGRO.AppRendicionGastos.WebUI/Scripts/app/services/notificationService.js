(function (app) {
    'use strict';

    app.factory('notificationService', notificationService);

    function notificationService() {

        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        var service = {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo,
            serverError: serverError
        };

        return service;

        function displaySuccess(message, timeout) {
            if (timeout) {
                toastr.options.timeOut = timeout;
                setTimeout(function () {
                    toastr.options.timeOut = 3000;
                }, 100);
            }
            toastr.success(message);
        }

        function displayError(error, timeout) {
            if (timeout) {
                toastr.options.timeOut = timeout;
                setTimeout(function () {
                    toastr.options.timeOut = 3000;
                }, 100);
            }
            if (Array.isArray(error)) {
                error.forEach(function (err) {
                    toastr.error(err);
                });
            } else {
                toastr.error(error);
            }
        }


        function serverError(error, errorMessage) {
            var message = error.data.message ? error.data.message : errorMessage;
            displayError(message);
        }

        function displayWarning(message, timeout) {
            if (timeout) {
                toastr.options.timeOut = timeout;
                setTimeout(function () {
                    toastr.options.timeOut = 3000;
                }, 100);
            }
            toastr.warning(message);
        }

        function displayInfo(message, timeout) {
            if (timeout) {
                toastr.options.timeOut = timeout;
                setTimeout(function () {
                    toastr.options.timeOut = 3000;
                }, 100);
            }
            toastr.info(message);
        }

    }

})(angular.module('common.core'));