(function () {
    'use strict';

    angular
        .module('app', [
            'ui.router',
            'triangular',
            'ngAnimate',
            'ngCookies',
            'ngSanitize',
            'ngMessages',
            'ngMaterial',
            'googlechart',
            'chart.js',
            'linkify',
            'angularMoment',
            'textAngular',
            //'uiGmapgoogle-maps',
            'hljs',
            'papa-promise',
            'angular-js-xlsx',
            'ngFileSaver',
            'md.data.table',
            'material.components.expansionPanels',
            //angularDragula(angular),
            'ngFileUpload',
            'nvd3',
            'common.core',
            'app.permission',
            'common.ui',
            'app.views'
        ])

        // set a constant for the API we are connecting to
        .constant('API_CONFIG', {
            'url': 'http://www.movizen.com'
        })
})();
