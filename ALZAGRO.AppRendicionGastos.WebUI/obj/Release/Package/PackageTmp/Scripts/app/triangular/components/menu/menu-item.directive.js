(function() {
    'use strict';

    angular
        .module('triangular.components')
        .directive('triMenuItem', triMenuItemDirective);

    /* @ngInject */
    function triMenuItemDirective() {
        var directive = {
            restrict: 'E',
            require: '^triMenu',
            scope: {
                item: '='
            },
            // replace: true,
            template: '<div ng-include="::triMenuItem.item.template"></div>',
            controller: triMenuItemController,
            controllerAs: 'triMenuItem',
            bindToController: true
        };
        return directive;
    }

    /* @ngInject */
    function triMenuItemController($scope, $injector, $mdSidenav, $state, $filter, $window, triBreadcrumbsService, triMenu) {        
        var triMenuItem = this;
        var rScope = $injector.get('$rootScope');
        // load a template for this directive based on the type ( link | dropdown )
        triMenuItem.item.template = '../Scripts/app/triangular/components/menu/menu-item-' + triMenuItem.item.type + '.tmpl.html';

        switch(triMenuItem.item.type) {
            case 'dropdown':
                // if we have kids reorder them by priority
                triMenuItem.item.children = $filter('orderBy')(triMenuItem.item.children, 'priority');
                triMenuItem.toggleDropdownMenu = toggleDropdownMenu;
                // add a check for open event
                $scope.$on('toggleDropdownMenu', function(event, item, open) {
                    // if this is the item we are looking for
                    if(triMenuItem.item === item) {
                        triMenuItem.item.open = open;
                    }
                    else {
                        triMenuItem.item.open = false;
                    }
                });
                // this event is emitted up the tree to open parent menus
                $scope.$on('openParents', function() {
                    // openParents event so open the parent item
                    triMenuItem.item.open = true;
                    // also add this to the breadcrumbs
                    triBreadcrumbsService.addCrumb(triMenuItem.item);
                });
                break;
            case 'link':
                
                if (triMenuItem.item.contextVariable) {
                    triMenuItem.openLink = changeContext;
                    triMenuItem.item.active = rScope.currentSelectedCompany == triMenuItem.item.contextVariable;
                } else {
                    triMenuItem.openLink = openLink;
                    // on init check if this is current menu
                    checkItemActive($state.current.name, $state.params);

                    $scope.$on('$stateChangeSuccess', function (event, toState, toParams) {
                        checkItemActive(toState.name, toParams);
                    });
                }
                break;
                
        }

        function checkItemActive() {
            // first check if the state is the same
            triMenuItem.item.active = $state.includes(triMenuItem.item.state, triMenuItem.item.params);
            // if we are now the active item reset the breadcrumbs and open all parent dropdown items
            if(triMenuItem.item.active) {
                triBreadcrumbsService.reset();
                triBreadcrumbsService.addCrumb(triMenuItem.item);
                $scope.$emit('openParents');
            }
        }

        function toggleDropdownMenu() {
            $scope.$parent.$parent.$broadcast('toggleDropdownMenu', triMenuItem.item, !triMenuItem.item.open);
        }


        function changeContext() {
            rScope.$broadcast('ContextChanged', triMenuItem.item.contextVariable);

            if ($window.location.pathname.match(/\d+$/)) {
                $state.go(triMenuItem.item.state);
            }
            triMenuItem.item.active = rScope.currentSelectedCompany == triMenuItem.item.contextVariable;
        }

        function openLink() {
            try {
                clearInterval(refreshRatio);
            } catch (e) {

            }
            
            if(angular.isDefined(triMenuItem.item.click)) {
                $injector.invoke(triMenuItem.item.click);
            }
            else {
                var params = angular.isUndefined(triMenuItem.item.params) ? {} : triMenuItem.item.params;
                if(angular.isDefined(triMenuItem.item.openInNewTab) && triMenuItem.item.openInNewTab === true) {
                    var url = $state.href(triMenuItem.item.state, params);
                    $window.open(url, '_blank');
                }
                else {
                    $state.go(triMenuItem.item.state, params);
                }
                if (triMenuItem.item.hideCustomContext) {
                    rScope.$broadcast('hideCustomContext', true);
                    triMenu.menu.forEach(function (item) {
                        item.active = false;
                    });
                } else {
                    rScope.$broadcast('hideCustomContext', false);
                    triMenu.menu.forEach(function (item) {
                        item.active = rScope.currentSelectedCompany == item.contextVariable;;
                    });
                }
            }
            triMenuItem.item.active = true;
            $mdSidenav('left').close();
        }
    }
})();
