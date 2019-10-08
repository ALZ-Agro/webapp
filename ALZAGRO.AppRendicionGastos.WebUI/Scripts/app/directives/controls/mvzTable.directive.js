(function (app) {
    'use strict';

    app.directive('mvzTable', mvzTable);

    function mvzTable($compile, $http, $rootScope, apiService, listService, notificationService, $state,$mdDialog,Blob,FileSaver) {
        return {
            restrict: 'AE', //attribute or element
            scope: {
                actions: "=",
                fabActions:"=",
                columns: "=",
                extras: "=?",
                advancedFilters: "=",
                data: "=?",
                title: "@",
                keyItem: "@",
                sourceName: "@",
                actionsClass: "@",
                showActions: "=",
                groupBy:"=?", //{field:"",labels:[{value1:""},{value2:""},...]}
                editUrl: "@",
                allowAdd: "=",
                iconAdd:"@",
                allowEdit: "=",
                iconEdit: "@",
                allowDelete: "=",
                cleanTable: "@",
                controllerName: "@",
                actionName: "@",
                customDelete: "@",
                deleteUrl:"@",
                useRemoteSource: "=?",
                autoLoad: "@",
                customParams:"=",
                onActionCallback: '&',
                updateParams: '&',
                allowSelection: "=",
                disableSearchDescription: "=?",
                showFilter: "@",
                showFilters:"=?",
                showMultiSelect: "=",
                exportExcel: '=?',
                exportOnlyVerified: '=?',
                exportExcelUrl: '@',
                exportVariable: '@',
                exportDefaultName: '=?',
                customToolbar: '=',
                customFooter: '=',
                customButtons: '=',
                forceShowToolbar: '=?',
                tableLimitOptions:'=?'
            },
            templateUrl: '/scripts/app/directives/controls/mvzTable.html',
            replace: false,
            link: function (scope, rootScope, element, attr) {
                scope.edit = edit;
                scope.remove = remove;
                scope.getKey = getKey;
                scope.add = add;
                scope.removeSelected = removeSelected;
                scope.editSelected = editSelected;
                scope.select = select;
                scope.selectedItems = [];

                if (!attr.remoteSource) {
                    scope.remoteSource = null;
                }
               
                //ABM
                function edit(id,data) {
                    if (id != null) {
                        if (angular.isUndefined(scope.editUrl)) {
                            $state.go("triangular." + attr.controllerName + "-edit", { id: id,data:data });
                        } else {
                            $state.go(scope.editUrl, { id: id, data: data });
                        }
                    }
                }

                function editSelected() {
                    var selected = listService.getSelected(scope.data);

                    if (selected.length == 1) {
                        if (angular.isUndefined(scope.editUrl)) {
                            $state.go("triangular." + attr.controllerName + "-edit", { id: selected[0].id });
                        } else {
                            $state.go("triangular." + scope.editUrl + "-edit", { id: selected[0].id });
                        }
                    }
                    else {
                        notificationService.displayError("Debe seleccionar solo un registro");
                    }
                }

                function add() {
                    if (angular.isUndefined(scope.editUrl)) {
                            $state.go("triangular." + attr.controllerName + "-edit", { fromList :true});
                        } else {
                        $state.go(scope.editUrl, { fromList: true });
                        }
                }

                function select(item) {
                    var i = scope.data.indexOf(item);
                    scope.data[i].selected = !scope.data[i].selected;
                }

                function remove(itemId) {
                    if (angular.isUndefined(scope.deleteUrl)) {
                        listService.remove(scope.controllerName, scope.data, itemId, function (index) {
                            scope.data.splice(index, 1);
                            scope.getSource();
                        });
                    }
                    else {
                        listService.remove(scope.deleteUrl, undefined, scope.data, itemId, function (index) {
                            scope.data.splice(index, 1);
                            scope.getSource();
                        });
                    }
                };

                function removeSelected() {

                    var selected = listService.getSelected(scope.data);
                    if (selected.length > 1) {
                        if (angular.isUndefined(attr.deleteUrl)) {
                            listService.removeSelected(attr.controllerName, attr.customDelete, scope.data, function (response) {
                                if (response && response.length) {
                                    scope.data = response;
                                }
                                else {
                                    scope.getSource();
                                }
                            });
                        }
                        else {
                            listService.removeSelected(attr.deleteUrl, undefined, scope.data, function (response) {
                                if (response && response.length) {
                                    scope.data = response;
                                }
                                else {
                                    scope.getSource();
                                }
                            });
                        }
                        
                    }
                    else {
                        remove(selected[0].id);
                    }
                    scope.getSource();
                    
                };
            
                //Special Actions
                function getKey(item) {
                    return item[scope.keyItem];
                }

                

                
            },

            controller: function ($scope, $http, $element, $rootScope) {
                $scope.isEditingOrder = false;
                //Defaults
                if (angular.isUndefined($scope.name)) {
                    $scope.name = 'table-' + $scope.controllerName;
                }
                if (angular.isUndefined($scope.extras)) {
                    $scope.extras = [];
                }

                if (angular.isUndefined($scope.forceShowToolbar)) {
                    $scope.forceShowToolbar = false;
                }
                if (angular.isUndefined($scope.data)) {
                    $scope.data = [];
                }
                else {
                    $scope.totalItems = $scope.data.length;
                }

                if (angular.isUndefined($scope.exportDefaultName)) {
                    $scope.exportDefaultName = $scope.title;
                }
                if (angular.isUndefined($scope.exportExcel)) {
                    $scope.exportExcel = false;
                }
                if (angular.isUndefined($scope.exportOnlyVerified)) {
                    $scope.exportOnlyVerified = false;
                }
                if (angular.isUndefined($scope.exportExcelUrl)) {
                    $scope.exportExcelUrl = '/ExportData';
                }
                if (angular.isUndefined($scope.exportVariable)) {
                    $scope.exportVariable = 'control';
                }
                if (angular.isUndefined($scope.keyItem)) {
                    $scope.keyItem = "id";
                }
                if (angular.isUndefined($scope.useRemoteSource)) {
                    $scope.useRemoteSource = true;
                }         
                if (angular.isUndefined($scope.actionsClass)) {
                    $scope.actionsClass = null;
                }
                if (angular.isUndefined($scope.showActions)) {
                    $scope.showActions = true;
                }
                if (angular.isUndefined($scope.showFilter)) {
                    $scope.showFilter = false;
                }
                if (angular.isUndefined($scope.showFilters)) {
                    $scope.showFilters = true;
                }
                if (angular.isUndefined($scope.disableSearchDescription)) {
                    $scope.disableSearchDescription = false;
                }
                if (angular.isUndefined($scope.autoLoad)) {
                    $scope.autoLoad = true;
                }
                if (angular.isUndefined($scope.isArray)) {
                    $scope.isArray = false;
                }
                if (angular.isUndefined($scope.editUrl)) {
                    $scope.editUrl = $scope.controllerName;
                }
                if (angular.isUndefined($scope.actionName)) {
                    $scope.actionName = "search";
                }
                if (angular.isUndefined($scope.sourceName)) {
                    $scope.sourceName = 'results';
                }
                if (angular.isUndefined($scope.iconEdit)) {
                    $scope.iconEdit = 'zmdi-edit';
                }
                if (angular.isUndefined($scope.iconAdd)) {
                    $scope.iconAdd = 'zmdi-plus';
                }
               if(!angular.isUndefined($scope.customFooter)){
                    $scope.paginator = angular.element('md-table-pagination')[0].children;
                }

                //Scope Methods
                $scope.removeFilter = removeFilter;
                $scope.getSource = getSource;
                $scope.exportToExcel = exportToExcel;
                $scope.addAll = function (value) {
                    $scope.selectAll = !value;
                    angular.forEach($scope.data, function (item, i) {
                        item.selected = !value;
                    });
                }

                $scope.customToolbarCallback = function (callback) {
                    callback(listService.getSelected($scope.data)).then(function () {
                        $scope.getSource();
                    }).catch(function (e) { });
                }

                $scope.$on('setTableData', function (ev, params) {
                    var orderedData = _.orderBy(params.newData, ['order'], ['asc']);
                    $scope.data = orderedData;
                    $rootScope.mvzTableData = orderedData;
                });

                $scope.$on('updateOutputData', function () {
                    $rootScope.mvzTableData = $scope.data;
                })

                $scope.$on('updateInputData', function () {
                    $scope.data = $rootScope.mvzTableData;
                })

               $scope.validationFn = function (column,item) {
                    return { isValid: column.applyValidation(item), message: column.errorMessage };
                }

               $scope.pageHasChanged = function (page, limit) {
                   $rootScope.$broadcast('pageHasChanged');
                   $scope.getSource();
               }


                $scope.customButtonsCallback = function (button) {
                    if (button.isReOrder) {
                        var i = 0;
                        $scope.data.forEach(function (element) {
                            i++;
                            if (!element[$rootScope.orderingVariable].order || element[$rootScope.orderingVariable].order === 0) {
                                if (angular.isUndefined(element.order)) {
                                    element.order = i;
                                }
                            } else {
                                element.order = element[$rootScope.orderingVariable].order;
                            }     
                        });
                        $scope.isEditingOrder = !$scope.isEditingOrder;
                        $rootScope.$broadcast('editBackupSwitch');
                        // switch buttons
                        $scope.customButtons.forEach(function (customButton) {
                            customButton.hide = !customButton.hide;
                        });
                        if (button.isSave) {
                            button.callback().then(function () {
                                $scope.getSource();
                            });
                        }
                    } else {
                        button.callback().then(function () {
                            if (!button.dontRefresh) {
                                $scope.getSource();
                            }
                        });
                    }

                }

               
                $scope.capitalize = function (string) {
                    if (string) {
                    return string.charAt(0).toUpperCase() + string.slice(1);
                    }
                }

                $scope.isSorteable = function (column) {
                    return angular.isUndefined(column.sortable) || column.sortable;
                }

                $scope.customToolbarDisabled = function (disabled) {
                    return disabled(listService.getSelected($scope.data));
                }

                $scope.formatDate = function (item, param, formatOption) {
                    var formattedModel = item[param];
                    switch (formatOption) {
                        case 'asHour': var arrayOfHour = formattedModel.split(':');
                                           formattedModel = arrayOfHour[0] + ':' + arrayOfHour[1];
                                           break;
                        case 'asDate': formattedModel = moment(formattedModel).format('DD-MM-YYYY'); break;
                    }
                    return formattedModel;
                }


                var lastSearchValue;
                $scope.setSearchFilter = function (filter, setFilterFunction) {
                    if (filter.model && filter.model[filter.searchParam] && filter.model != lastSearchValue) {
                        setFilterFunction(filter.model[filter.searchParam]);
                    }
                    lastSearchValue = filter.model;
                }

                $scope.changeRatingFilter = function(filter, $event) {
                    $scope.showClear = true;
                    filter.model = $event.rating;
                    changeFilter(filter);
                }

                $scope.changeFilter = changeFilter;

                function changeFilter(filter, callback) {
                    if ($scope.filter && $scope.filter.form && $scope.filter.form.city){
                        $scope.filter.form.city.$setValidity('required', true);
                    }
                    $scope.showClear = true;
                    if (callback) {
                        callback(filter.model);
                    }
                    for (var filter in $scope.advancedFilters) {
                        
                        var advancedFilter = $scope.advancedFilters[filter];
                        if (advancedFilter.isSearch) {
                            if (advancedFilter.model) {
                                $scope.filterParams[advancedFilter.param] = advancedFilter.model[advancedFilter.searchParam];
                            } else {
                                $scope.filterParams[advancedFilter.param] = advancedFilter.model;
                            }
                        } else if (advancedFilter.isStatus) {
                            $scope.filterParams[advancedFilter.param] = advancedFilter.model;
                            switch (advancedFilter.model) {
                                case null: $scope.advancedFilters[filter].model = 0; break;
                                case true: $scope.advancedFilters[filter].model = 1; break;
                                case false: $scope.advancedFilters[filter].model = 2; break;
                            }

                        }

                        else {
                            $scope.filterParams[advancedFilter.param] = advancedFilter.model;
                        }
                        if (advancedFilter.disableDependencies) {
                            advancedFilter.disableDependencies();
                        }
                    }
                    $scope.getSource();
                }

                $scope.clear = function () {
                    if ($scope.filter && $scope.filter.form && $scope.filter.form.city) {
                        $scope.filter.form.city.$setValidity('required', true);
                    }
                    for (var filter in $scope.advancedFilters) {
                        $scope.advancedFilters[filter].model = null;
                        if ($scope.advancedFilters[filter].isRating) {
                            angular.element('#rating').data().$starRatingCompController.rating = 0;
                        }
                        if ($scope.advancedFilters[filter].default || $scope.advancedFilters[filter].default === 0 ) {
                            $scope.advancedFilters[filter].model = $scope.advancedFilters[filter].default;
                        }
                    }

                    $scope.changeFilter();
                    $scope.showClear = false;
                }
                
                //Scope Variables
                $scope.filterParams = {};
                var queryUndefined = (angular.isUndefined($scope.customParams) || angular.equals({}, $scope.customParams));
                $scope.query = queryUndefined ? { page: 1, partialDescription: "", size: '10', orderBy: null } : $scope.customParams;
                $scope.filter = {
                    options: {
                        debounce: 500
                    }
                };         
                $scope.toolbar = {
                    showAdd: true,
                    showEdit: false,
                    showDelete: false
                }              
                $scope.selectAll = false;
                $scope.selected = false;
                $scope.showClear = false;
                $scope.totalItems = $scope.data.length;

                $scope.$watch('data', function (newValue, oldValue) {
                    if (newValue && (newValue !== oldValue)) {
                       
                        var selected = _.filter(_.concat($scope.data), function (item) { return item.selected == true; });
                        $rootScope.selectedItems = selected;
                        if (selected.length < 1) {
                            $scope.toolbar.showAdd = true,
                            $scope.toolbar.showEdit = false,
                            $scope.toolbar.showDelete = false,
                            $scope.selected = false;
                        }
                        if (selected.length == 1) {
                            $scope.toolbar.showAdd = false,
                            $scope.toolbar.showEdit = true,
                            $scope.toolbar.showDelete = true,
                            $scope.selected = true;
                        }
                        if (selected.length > 1) {
                            $scope.toolbar.showAdd = false,
                            $scope.toolbar.showEdit = false,
                            $scope.toolbar.showDelete = true,
                            $scope.selected = true;
                        }
                    }
                    
                }, true);

                $scope.$on('refresh', function () {
                    $scope.getSource();
                });

                //Init
                function activate() {
                   
                    var md = new MobileDetect(window.navigator.userAgent);
                    if (md.phone()) {
                        $scope.isMobile = true;
                    }
                    else {
                        $scope.isMobile = false;
                    }
                    var bookmark;
                    $scope.$watch('query.partialDescription', function (newValue, oldValue) {
                        if (!oldValue) {
                            bookmark = $scope.query.page;
                        }

                        if (newValue !== oldValue) {
                            $scope.query.page = 1;
                        }

                        if (!newValue) {
                            $scope.query.page = bookmark;
                        }

                        if ($scope.useRemoteSource) {
                            getSource();
                        }
                       
                    });
                }

                //Functions
                $scope.getHalfStarVisible = function (number){
                    var absDiff = Math.abs(rating % 1);

                    if (absDiff == 0.1) {
                        return false;
                    }

                    return absDiff > 0;
                }

                $scope.getSelectedLength = function () {
                    return _.filter($scope.data, ["selected", true]).length;
                }


                $scope.onAction = function(actionName, item, ev) {
                    if ($scope.fabActions) {
                        var action = $scope.fabActions.filter(function (item) { return item.name == actionName; });
                        if (action.length > 0) {
                            var config = getConfig();
                            $scope.onActionCallback()(actionName, config.params, ev);
                        }
                        else {
                            $scope.onActionCallback()(actionName, item, ev);
                        }
                    }
                    else {
                        $scope.onActionCallback()(actionName, item, ev, $rootScope.mvzTableData);
                    }
                }

                function exportToExcel() {
                    var config = getConfig();
                    if ($scope.exportOnlyVerified) {
                        var confirm = $mdDialog.prompt()
                        .title('Exportar datos')
                        .textContent('Elija el nombre del archivo que se descargará:')
                        .placeholder('Nombre')
                        .ariaLabel('fileName')
                        .initialValue(config.params.fileName ? config.params.fileName : null)
                       // .targetEvent(ev)
                        .required(true)
                        .ok('DESCARGAR')
                        .cancel('CANCELAR');

                        $mdDialog.show(confirm).then(function (result) {
                            config.params.fileName = result;
                            apiService.post($scope.exportExcelUrl, config.params, function (result) {
                                $http.get(result.data.url, { responseType: 'arraybuffer' })
                                    .then(function success(response) {
                                        var file = new Blob([response.data], {
                                            type: 'text/csv'
                                        });

                                        FileSaver.saveAs(file,config.params.fileName?config.params.fileName + ".csv":$scope.exportDefaultName + "-" + moment().format('DD-MM-YYYY-HH:mm') + ".csv");
                                    });
                            }, function () {
                                notificationService.serverError(error, "No se pudo generar el reporte. Intente nuevamente en unos minutos.");

                            });
                        }, function () {
                            
                        });
                    } else {
                        apiService.exportData($scope.exportExcelUrl, config.params);
                    }
                }          

                

                function getConfig(){
                    var config = {
                        params: $scope.query
                    };
                    var customParams = $scope.updateParams();
                    customParams.orderBy = config.params.orderBy ? config.params.orderBy : customParams.orderBy;
                    customParams.partialDescription = config.params.partialDescription;
                    if (customParams) {
                        customParams.size = config.params.size;
                        customParams.page = config.params.page;
                        angular.forEach(customParams, function (value, key) {
                            config.params[key] = value;
                        });
                    }
                    if ($scope.filterParams) {
                        angular.forEach($scope.filterParams, function (value, key) {
                            config.params[key] = value;
                        });
                    }
                    else {
                        for (i = 0; i < $scope.advancedFilters.length; i++) {
                            if ($scope.advancedFilters[i].param.includes('Date') && $scope.advancedFilters[i].model != null) {
                                $scope.filterParams[$scope.advancedFilters[i].param] = $scope.advancedFilters[i].model.toISOString();
                            }
                            else if (typeof ($scope.advancedFilters[i].model) === "boolean") {
                                $scope.filterParams[$scope.advancedFilters[i].param] = $scope.advancedFilters[i].model ? "true" : "false";
                            }
                            else {
                                $scope.filterParams[$scope.advancedFilters[i].param] = $scope.advancedFilters[i].model;
                            }
                            
                        }
                        angular.forEach($scope.filterParams, function (value, key) {
                            config.params[key] = value;
                        });
                    }

                    return config;
                }

                $scope.$on('mvztable-update', function () {
                    if ($scope.useRemoteSource) {
                        getSource();
                    }
                   
                });

                $scope.$on('mvztable-refresh-to-page', function (ev, page) {
                    getSource(page);
                })

                $scope.isSelectAllChecked = function () {
                    return $scope.selectAll;
                }

                $scope.showFiltersToolbar = function () {
                    $scope.showFilter = true;
                }

                function getSource(page) {
                  //  $scope.data = [];
                    $rootScope.mvzTableData = [];
                    $rootScope.mvzTableLoaded = false;
                    if ($scope.cleanTable) {
                        $scope.data = [];
                    }
                    $scope.selectAll = false;
                    $scope.selected = false;

                    if ($scope.filter && $scope.filter.form && $scope.filter.form.city) {
                        $scope.filter.form.city.$setValidity('required', true);
                    }
                    var config = getConfig();

                    if (page && typeof page == 'number') {
                        config.params.page = page;
                    }

                    if (window.innerWidth <= 480 && window.innerHeight <= 800) {
                        $scope.showFilter = false;
                    }

                    if ($scope.autoLoad != "false" && $scope.useRemoteSource) {
                        $scope.promise = apiService.get('/api/' + $scope.controllerName + '/' +
                                                       $scope.actionName, config, onLoadCompleted, onLoadFailed);
                    } else {
                        $scope.autoLoad = true;
                    }
                    
                }

                function removeFilter() {
                    $scope.showFilter = false;
                    $scope.query.partialDescription = '';

                    if ($scope.filter.form.$dirty) {
                        $scope.filter.form.$setPristine();
                    }
                }

                function onLoadCompleted(result) {
                    
                    if (result.data["resume"]) {
                        $scope.resume = result.data["resume"];
                    }
                    else {
                        $scope.resume = {};
                    }

                    if (result.data[$scope.sourceName]) {
                        //Initialize
                        angular.forEach(result.data[$scope.sourceName], function (item, k) {
                            item.selected = false;
                        });
                        
                        angular.forEach(result.data[$scope.sourceName], function (item, k) {
                            var current = $scope.data.filter(function (obj) { return obj.id == item.id; });
                            if (current.length > 0 && current[0].selected) {
                                item.selected == true;
                            }
                        });
                        if (!$scope.groupBy) {
                            $scope.data = result.data[$scope.sourceName];
                        }
                        else {
                            $scope.data = _.groupBy(result.data[$scope.sourceName], function (r) {
                                return $scope.groupBy.parentRoot ? r[$scope.groupBy.parentRoot][$scope.groupBy.field] : r[$scope.groupBy.field];
                            });
                        }

                    }
                    else {
                        $scope.data = [];
                    }
                   
                    $scope.totalItems = result.data.totalItems;
                    if ($scope.totalItems > 0 && !angular.isUndefined($scope.tableLimitOptions)) {
                        var lastOption = $scope.tableLimitOptions[$scope.tableLimitOptions.length - 1],
                            lastOptionIndex = $scope.tableLimitOptions.indexOf(lastOption);
                        if (typeof lastOption === 'object') {
                            $scope.tableLimitOptions[lastOptionIndex].value = function () {
                                return $scope.totalItems;
                            };
                        }
                    }
                   
                    $rootScope.mvzTableData = $scope.data;
                    $rootScope.$broadcast('subTotalChanged', result.data.subTotal);
                    $rootScope.mvzTableLoaded = true;
                    $rootScope.$broadcast('mvzloadfinished', $scope.data);
                                           
                }

                function onLoadFailed(response) {
                    notificationService.displayError(response.data.message);
                }

                activate();
            }
        }
    }

})(angular.module('common.ui'));