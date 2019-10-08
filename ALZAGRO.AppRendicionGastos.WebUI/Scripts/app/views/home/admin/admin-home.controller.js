(function () {
    'use strict';

    angular
        .module('home-module')
        .controller('AdminHomeCtrl', AdminHomeCtrl);


    AdminHomeCtrl.$inject = ['$scope', 'apiService', '$rootScope', 'notificationService', '$q', '$filter'];

    function AdminHomeCtrl($scope, apiService, $rootScope, notificationService, $q, $filter) {

        $scope.byVendorIsReady = false;
        $scope.totalChartReady = false;

        $scope.loggedUser = $rootScope.repository.loggedUser;

        $scope.totals = {
            fuel: 0,
            food: 0,
            hotels: 0,
            other:0
        };

        $scope.startDate = moment().subtract(1, 'month').toDate();
        $scope.endDate = moment().toDate();

        $scope.$on('ContextChanged', function (event, id) {
            refresh(id);
        });
        $scope.endDateOptions = {
            maxDate: moment().toDate(),
            minDate: angular.copy($scope.startDate)
        }


        $scope.startDateOptions = {
            minDate: moment().subtract(20, 'years').toDate(),
            maxDate: angular.copy($scope.endDate)
        }
        

        $scope.startDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate();
                $scope.config.params.startDate = date;
                $scope.endDateOptions.minDate = date;

                $scope.filterBy();
            }
        }

        $scope.endDateSelected = function (datestr) {
            if (datestr) {
                var date = moment(datestr).toDate()
                $scope.config.params.endDate = date;
                $scope.startDateOptions.maxDate = date;
                $scope.filterBy();
            }
        }
        $scope.filterBy = function (model) {
            refresh();
        }
        
        function reemplaceTotals(category, total) {
            var index = _.findIndex($scope.total.data.rows,_.head(_.filter($scope.total.data.rows, function (o) {
                return o.c[0].v == category;
            })));
            $scope.total.data.rows[index].c[1].v = total;
        }
        
        function updateByVendorChart() {
            $scope.byVendor = $scope.byVendor;
        }

        $scope.chartIsReady = function (chart) {
            console.log(chart);
        }

        var chartArea = {
            "left": "50",
            "top": "50",
            "width": "90%",
        };


        $scope.byVendor = {
            "type": "ColumnChart",
            "displayed": false,
            "data": {
                "cols": [
                    {
                        "id": "vend",
                        "label": "Vendedor",
                        "type": "string",
                        "p": {}
                    },
                    {
                        "id": "comb-id",
                        "label": "Combustible",
                        "type": "number",
                        "p": {}
                    },
                    {
                        "id": "aloj-id",
                        "label": "Alojamiento",
                        "type": "number",
                        "p": {}
                    },
                    {
                        "id": "com-id",
                        "label": "Comida",
                        "type": "number",
                        "p": {}
                    },
                    {
                        "id": "otr-id",
                        "label": "Otros",
                        "type": "number",
                        "p": {}
                    },
                    {
                        "id": "tooltip",
                        "role": "tooltip",
                        "type": "string",
                        "p": {
                            "role": "tooltip",
                            "html": true
                        }
                    }
                ],
                "rows":
                [

                ],
            },
            "options": {
                "colors": [
                    '#7BA529', '#F3C330', '#AF4D42', '#AF9442'
                ],
                "forceRedrawNow": true,
                "redrawTrigger": 0,
                "title": "Por vendedor",
                "titleTextStyle": {
                    "fontSize": "30",
                    "textPosition": "out",
                    "fontName": "Ubuntu",
                    "bold": false
                },
                "explorer": {
                    "axis": "horizontal",
                    "keepInBounds": true
                },
                "chartArea": chartArea,
                "legend": { "position": 'bottom', "maxLines": 4 },
                "displayExactValues": true,
                "axisTitlesPosition": "out",
                "isStacked": "percent",
                "fill": 20,
                "vAxis": {
                    "gridlines": {
                        "count": 10
                    },
                    "format": '###.##%',
                    "minValue": 0,
                    "viewWindow": {
                        "min": 0
                    }
                },
                "tooltip": {
                    "isHtml": true
                }
            },
            "formatters": {},
            "view": {}
        }

        function refresh(newCompanyId) {
            //clean graphs
            $scope.byVendor.data.rows = [];
            $scope.byVendorIsReady = false;
            $scope.totalChartReady = false;
            // clean graphs
            var dto = {
                params: {
                    startDate: moment($scope.startDate).toISOString(),
                    endDate: moment($scope.endDate).toISOString(),
                    companyId: newCompanyId ? newCompanyId : $rootScope.currentSelectedCompany,
                    page: 1,
                    size: -1,
                    partialDescription: '',
                    orderBy: 'Date',
                    userId: 0
                }
            };
            var defer = $q.defer();

            apiService.get('api/expense/analytics', dto, function (result) {
                $scope.totalChart = {
                    "type": "PieChart",
                    "displayed": true,
                    "data": {
                        "cols": [
                            { id: "t", label: "Categorias", type: "string" },
                            { id: "s", label: "generado", type: "number" }
                        ],
                        "rows": [
                            {
                                c: [
                                    { v: "Sin datos" },
                                    { v: 100 },
                                ]
                            }
                        ]
                    },
                    "options": {
                        "title": "Total",
                        "isStacked": "true",
                        "fill": 20,
                        "forceRedrawCount": 0,
                        "forceRedrawNow": true,
                        "titleTextStyle": {
                            "fontSize": "30",
                            "textPosition": "out",
                            "fontName": "Ubuntu",
                            "bold": false
                        },
                        "chartArea": chartArea,
                        "legend": { "position": 'bottom', "maxLines": 4 },
                        "displayExactValues": true,
                        "colors": [
                            '#7BA529', '#F3C330', '#AF4D42', '#AF9442'
                        ]
                    },
                    "formatters": {}
                }
                if (result.data.totalFuel || result.data.totalHotel || result.data.totalFood || result.data.totalOther) {

                    $scope.totalChart.data.rows = [
                        {
                            c: [
                                { v: "Combustible" },
                                { v: result.data.totalFuel },
                            ]
                        },
                        {
                            c: [
                                { v: "Alojamiento" },
                                { v: result.data.totalHotel }
                            ]
                        },
                        {
                            c: [
                                { v: "Comida" },
                                { v: result.data.totalFood }
                            ]
                        },
                        {
                            c: [
                                { v: "Otros" },
                                { v: result.data.totalOther },
                            ]
                        }
                    ];
                } else {
                    $scope.totalChart.data.rows = [
                        {
                            c: [
                                { v: "Sin datos" },
                                { v: 100 },
                            ]
                        }
                    ];
                    $scope.totalChart.options.colors = ['#A1A1A1'];
                }

                
                    $scope.totals.food = result.data.totalFood; 
                    //reemplaceTotals('Comida', result.data.totalFood);
                    $scope.totals.hotels = result.data.totalHotel;
                    //reemplaceTotals('Alojamiento', result.data.totalHotel);
                    $scope.totals.fuel = result.data.totalFuel;
                    //reemplaceTotals('Combustible', result.data.totalFuel);
                    $scope.totals.other = result.data.totalOther;

                if (result.data.vendors) {
                    result.data.vendors.forEach(function (vendor) {
                        var totalFuelOfVendor = $filter('currency')(vendor.totalFuel, '$', 2);
                        var totalHotelOfVendor = $filter('currency')(vendor.totalHotel, '$', 2);
                        var totalFoodOfVendor = $filter('currency')(vendor.totalFood, '$', 2);
                        var totalOtherOfVendor = $filter('currency')(vendor.totalOther, '$', 2);
                        $scope.byVendor.data.rows.push(
                            {
                                "c": [
                                    {
                                        "v": vendor.fullName
                                    },
                                    {
                                        "f": totalFuelOfVendor,
                                        "v": vendor.totalFuel
                                    },
                                    {
                                        "f": totalHotelOfVendor,
                                        "v": vendor.totalHotel,
                                    },
                                    {
                                        "f": totalFoodOfVendor,
                                        "v": vendor.totalFood,
                                    },
                                    {
                                        "f": totalOtherOfVendor,
                                        "v": vendor.totalOther,
                                    }
                                ]
                            }
                        );
                    });
                    }
                $scope.byVendor.options.redrawTrigger++;
                $scope.byVendorIsReady = true;
                $scope.totalChartReady = true;
                var intervalLoop = 0;
                var updateCharts = setInterval(function () {
                    var vendorChartElement = document.getElementById('byVendorChartId');
                    var totalChartElement = document.getElementById('totalChartId');
                    var vendorChart = vendorChartElement ? vendorChartElement.children[0] : false;
                    var totalChart = totalChartElement ? totalChartElement.children[0]: false;
                    var vendorChartExist = vendorChart && vendorChart.children[0] && vendorChart.children[0].children.length > 0;
                    var totalChartExist = totalChart && totalChart.children[0] && totalChart.children[0].children.length > 0;
                    if (vendorChartExist && totalChartExist) {
                        if (intervalLoop < 5) {
                            $rootScope.$broadcast('resizeMsg');
                            intervalLoop++;
                        } else {
                            /// last chart update
                            $rootScope.$broadcast('resizeMsg');
                            clearInterval(updateCharts);
                        }
                    } else {
                        $rootScope.$broadcast('resizeMsg');
                    }
                }, 40);
                defer.resolve();
            }, function (error) {
                notificationService.displayError(error);
                defer.reject();
            });
        }

        refresh();

        var refreshRatio = setInterval(function () {
            refresh();
        }, ($rootScope.contextConfig.syncIntervalInSeconds ? $rootScope.contextConfig.syncIntervalInSeconds : 300)*1000);
    }
})();