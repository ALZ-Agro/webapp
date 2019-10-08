(function () {
    'use strict';

    angular
        .module('app')
        .config(translateConfig);

    /* @ngInject */
    function translateConfig(triSettingsProvider, triRouteProvider) {
        var now = new Date();
        // set app name & logo (used in loader, sidemenu, footer, login pages, etc)
        triSettingsProvider.setName('AlzAgro');
        triSettingsProvider.setCopyright('&copy;' + now.getFullYear() + "<a href=''>AlzAgro S.R.L</a>. Todos los derechos reservados.");
        triSettingsProvider.setLogo('../Content/images/header-menu.png');
        // set current version of app (shown in footer)
        triSettingsProvider.setVersion('1.0.0');
        // set the document title that appears on the browser tab
        triRouteProvider.setTitle('AlzAgro');
        triRouteProvider.setSeparator('|');
        //triRouteProvider.setHomeState('triangular.account.login');
    }
})();
