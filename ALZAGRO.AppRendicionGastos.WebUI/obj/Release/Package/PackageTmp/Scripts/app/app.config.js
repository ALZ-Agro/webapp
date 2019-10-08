(function () {
    'use strict';

    angular
        .module('app')
        .config(configFunction);

    /* @ngInject */
    function configFunction($qProvider, $compileProvider, $translateProvider, $mdDateLocaleProvider, $mdAriaProvider, blockUIConfig, $provide) {

        //Remove the built in  ivhTreeviewCheckbox directive
        $provide.decorator('ivhTreeviewCheckboxDirective', function ($delegate) {
            $delegate.shift();
            return $delegate;
        });

        //BlockUI settings
        blockUIConfig.message = "Cargando ...";

        blockUIConfig.requestFilter = function (config) {
            if (config.url.match(/((api\/expense\/list)|updateLastActivityDateTime|fileUpload|.css|.html|.js|(api\/notifications\/search)|(api\/notifications\/getUnreadNumber))/)) {
                return false;
            }
            //if (config.url.match(/(attachments|search|annotations|appointments|.css|.html|.js)/) && window.location.href.indexOf("room") != -1) {
            //    return false;
            //}
        };

        //Translate
        $translateProvider.useStaticFilesLoader({
            prefix: 'Content/vendors/angular-directives/ghiscoding.angular-validation/locales/validation/',
            suffix: '.json'
        });

        $translateProvider.useSanitizeValueStrategy('sanitizeParameters');
        //define translation maps you want to use on startup
        $translateProvider.preferredLanguage('es');

        //Format spanish
        $mdDateLocaleProvider.formatDate = function (date) {
            if (date) {
                var tempDate = moment(date);
                if (tempDate.isValid()) {
                    return tempDate.format('DD/MM/YYYY');
                } else {
                    return '';
                }
            } else {
                return '';
            }
        };



        $mdDateLocaleProvider.parseDate = function (dateString) {
            toastr.clear();
            var m = moment(dateString, 'DD/MM/YYYY', true);
            if (m.isValid()) {
                return m.toDate();
            } else {
                toastr.options = {
                    "debug": false,
                    "positionClass": "toast-top-right",
                    "onclick": null,
                    "fadeIn": 300,
                    "fadeOut": 500,
                    "timeOut": 3000,
                    "extendedTimeOut": 5000,
                    "preventOpenDuplicates": true,
                    "preventDuplicates": true,
                    "maxOpened": 0
                };
                toastr.error("La fecha ingresada es inválida. El formato aceptado es DD/MM/AAAA.");
                return new Date(NaN);
            }
        };

        //Globally disables all ARIA warnings.
        $mdAriaProvider.disableWarnings();

        //Fix change in angular 1.6.3         
        $compileProvider.preAssignBindingsEnabled(true);

        //Fix transition superseded UI-Router
        $qProvider.errorOnUnhandledRejections(false);
    }
})();
