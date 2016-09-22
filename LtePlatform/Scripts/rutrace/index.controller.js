﻿angular.module('myApp', ['rutrace.main', 'rutrace.precise', 'precise.interference', 'precise.workitem']);

angular.module('rutrace.main', ['app.common'])
    .config([
        '$routeProvider', function($routeProvider) {
            var viewDir = "/appViews/Rutrace/";
            $routeProvider
                .when('/', {
                    templateUrl: viewDir + "Index.html",
                    controller: "rutrace.index"
                })
                .when('/trend', {
                    templateUrl: viewDir + "Trend.html",
                    controller: "rutrace.trend"
                })
                .when('/top', {
                    templateUrl: viewDir + "Top.html",
                    controller: "rutrace.top"
                })
                .when('/topDistrict/:district', {
                    templateUrl: viewDir + "Top.html",
                    controller: "rutrace.top.district"
                })
                .when('/chart', {
                    templateUrl: viewDir + "Chart.html",
                    controller: "rutrace.chart"
                })
                .when('/trendchart', {
                    templateUrl: viewDir + "TrendChart.html",
                    controller: "rutrace.trendchart"
                })
                .when('/top', {
                    templateUrl: viewDir + "Top.html",
                    controller: "rutrace.top"
                })
                .when('/import/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + "Import.html",
                    controller: "rutrace.import"
                })
                .when('/interference/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + "Interference/Index.html",
                    controller: "rutrace.interference"
                })
                .when('/coverage/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + "Coverage/Index.html",
                    controller: "rutrace.coverage"
                })
                .when('/baidumap/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + "Map/Index.html",
                    controller: "rutrace.map"
                })
                .when('/details/:number', {
                    templateUrl: viewDir + "WorkItem/AnalyticDetails.html",
                    controller: "workitem.details"
                })
                .when('/workItems/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + 'WorkItem/ForCell.html',
                    controller: "rutrace.workitems"
                })
                .when('/workitemDistrict/:district', {
                    templateUrl: viewDir + "WorkItem/ForCity.html",
                    controller: "workitem.district"
                })
                .when('/workitemCity', {
                    templateUrl: viewDir + "WorkItem/ForCity.html",
                    controller: "workitem.city"
                })
                .when('/cellTrend/:cellId/:sectorId/:name', {
                    templateUrl: viewDir + "WorkItem/CellTrend.html",
                    controller: "cell.trend"
                })
                .when('/mongo', {
                    templateUrl: viewDir + 'FromMongo.html',
                    controller: 'interference.mongo'
                })
                .otherwise({
                    redirectTo: '/'
                });
        }
    ])
    .run(function($rootScope) {
        var rootUrl = "/Rutrace#";
        $rootScope.menuItems = [
            {
                displayName: "总体情况",
                isActive: true,
                subItems: [
                    {
                        displayName: "指标总体情况",
                        url: rootUrl + "/"
                    }, {
                        displayName: "指标变化趋势",
                        url: rootUrl + "/trend"
                    }
                ]
            }, {
                displayName: "详细查询",
                isActive: false,
                subItems: []
            }, {
                displayName: "TOP指标",
                isActive: true,
                subItems: [
                    {
                        displayName: "分析",
                        url: rootUrl + "/top"
                    }
                ]
            }, {
                displayName: "辅助功能",
                isActive: false,
                subItems: [
                    {
                        displayName: "从MongoDB导入",
                        url: rootUrl + "/mongo"
                    }
                ]
            }
        ];
        $rootScope.rootPath = rootUrl + "/";

        $rootScope.viewData = {
            workItems: []
        };
        var lastSeason = new Date();
        lastSeason.setDate(lastSeason.getDate() - 100);
        var lastWeek = new Date();
        lastWeek.setDate(lastWeek.getDate() - 7);
        $rootScope.beginDate = {
            value: new Date(lastWeek.getFullYear(), lastWeek.getMonth(), lastWeek.getDate(), 8),
            opened: false
        };
        $rootScope.seasonDate = {
            value: new Date(lastSeason.getFullYear(), lastSeason.getMonth(), lastSeason.getDate(), 8),
            opened: false
        };
        var today = new Date();
        $rootScope.endDate = {
            value: new Date(today.getFullYear(), today.getMonth(), today.getDate(), 8),
            opened: false
        };
    })
    .controller("rutrace.root", function($scope, appRegionService, menuItemService) {
        $scope.page = { title: "指标总体情况" };
        $scope.overallStat = {
            currentDistrict: "",
            districtStats: [],
            townStats: [],
            cityStat: {},
            dateString: "",
            city: ""
        };
        $scope.trendStat = {
            stats: [],
            districts: [],
            districtStats: [],
            townStats: [],
            beginDateString: "",
            endDateString: ""
        };
        $scope.topStat = {
            current: {},
            cells: {},
            interference: {},
            victims: {},
            coverages: {},
            updateInteferenceProgress: {},
            updateVictimProgress: {},
            mongoNeighbors: {},
            pieOptions: {},
            columnOptions: {}
        };
        $scope.updateTopCells = function(cell) {
            var cellName = cell.eNodebName + "-" + cell.sectorId;
            if ($scope.topStat.cells[cellName] === undefined) {
                $scope.topStat.cells[cellName] = cell;
            }
        };
        $scope.city = {
            selected: "",
            options: []
        };

        appRegionService.initializeCities().then(function(result) {
            $scope.overallStat.city = result[0];
            $scope.city.options = result;
            $scope.city.selected = result[0];
            appRegionService.queryDistricts(result[0]).then(function(districts) {
                angular.forEach(districts, function(district) {
                    menuItemService.updateMenuItem($scope.menuItems, 2,
                        "TOP指标分析-" + district, $scope.rootPath + "topDistrict/" + district);
                });
            });
        });
    })
    .controller("rutrace.index", function($scope, appRegionService, appKpiService, kpiPreciseService, appFormatService) {
        $scope.page.title = "指标总体情况";
        var yesterday = new Date();
        yesterday.setDate(yesterday.getDate() - 1);
        $scope.statDate = {
            value: yesterday,
            opened: false
        };
        $scope.showKpi = function(city) {
            kpiPreciseService.getRecentPreciseRegionKpi(city, $scope.statDate.value)
                .then(function(result) {
                    $scope.statDate.value = appFormatService.getDate(result.statDate);
                    angular.forEach(result.districtPreciseViews, function(view) {
                        view.objectRate = appKpiService.getPreciseObject(view.district);
                    });
                    $scope.overallStat.districtStats = result.districtPreciseViews;
                    $scope.overallStat.townStats = result.townPreciseViews;
                    $scope.overallStat.currentDistrict = result.districtPreciseViews[0].district;
                    $scope.overallStat.districtStats.push(appKpiService.getCityStat($scope.overallStat.districtStats, city));
                    $scope.overallStat.dateString = appFormatService.getDateString($scope.statDate.value, "yyyy年MM月dd日");
                });
        };
        $scope.$watch('city.selected', function(city) {
            if (city) {
                $scope.showKpi(city);
            }
        });
    })
    .controller('dump.cell.mongo', function($scope, $uibModalInstance, dumpProgress, appFormatService, dumpPreciseService,
        dialogTitle, eNodebId, sectorId, pci, begin, end) {
        $scope.dialogTitle = dialogTitle;

        $scope.dateRecords = [];
        $scope.currentDetails = [];

        $scope.ok = function() {
            $uibModalInstance.close($scope.dateRecords);
        };

        $scope.cancel = function() {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.queryRecords = function() {
            angular.forEach($scope.dateRecords, function(record) {
                dumpProgress.queryExistedItems(eNodebId, sectorId, record.date).then(function(result) {
                    for (var i = 0; i < $scope.dateRecords.length; i++) {
                        if ($scope.dateRecords[i].date === record.date) {
                            $scope.dateRecords[i].existedRecords = result;
                            break;
                        }
                    }
                });
                dumpProgress.queryMongoItems(eNodebId, sectorId, record.date).then(function(result) {
                    for (var i = 0; i < $scope.dateRecords.length; i++) {
                        if ($scope.dateRecords[i].date === record.date) {
                            $scope.dateRecords[i].mongoRecords = result;
                            break;
                        }
                    }
                });
            });
        };

        $scope.updateDetails = function(records, recordDate) {
            $scope.currentDetails = records;
            $scope.recordDate = recordDate;
        };

        $scope.dumpAllRecords = function() {
            dumpPreciseService.dumpAllRecords($scope.dateRecords, 0, 0, eNodebId, sectorId, $scope.queryRecords);
        };

        var startDate = new Date(begin);
        while (startDate < end) {
            var date = new Date(startDate);
            $scope.dateRecords.push({
                date: date,
                existedRecords: 0,
                existedStat: false
            });
            startDate.setDate(date.getDate() + 1);
        }
        $scope.queryRecords();
    })
    .controller("rutrace.coverage", function ($scope, $routeParams, $uibModal, topPreciseService, preciseInterferenceService) {
        $scope.currentCellName = $routeParams.name + "-" + $routeParams.sectorId;
        $scope.page.title = "TOP指标覆盖分析: " + $scope.currentCellName;
        $scope.orderPolicy = topPreciseService.getOrderPolicySelection();
        $scope.detailsDialogTitle = $routeParams.name + "-" + $routeParams.sectorId + "详细小区统计";
        $scope.cellId = $routeParams.cellId;
        $scope.sectorId = $routeParams.sectorId;
        $scope.showCoverage = function () {
            topPreciseService.queryCoverage($scope.beginDate.value, $scope.endDate.value,
                $routeParams.cellId, $routeParams.sectorId).then(function (result) {
                    $scope.coverageList = result;
                });
            preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                $routeParams.cellId, $routeParams.sectorId).then(function (result) {
                    $scope.interferenceCells = result;
                    angular.forEach($scope.interferenceCells, function (neighbor) {
                        if (neighbor.destENodebId > 0) {
                            topPreciseService.queryCellStastic(neighbor.destENodebId, neighbor.destPci,
                                $scope.beginDate.value, $scope.endDate.value).then(function (coverage) {
                                    if (coverage) {
                                        neighbor.mrCount = coverage.mrCount;
                                        neighbor.weakCoverCount = coverage.weakCoverCount;
                                        neighbor.overCoverCount = coverage.overCoverCount;
                                    }
                                });
                        }
                    });
                });
            preciseInterferenceService.queryInterferenceVictim($scope.beginDate.value, $scope.endDate.value,
                $routeParams.cellId, $routeParams.sectorId).then(function (result) {
                    $scope.interferenceVictims = result;
                    angular.forEach($scope.interferenceVictims, function (victim) {
                        if (victim.victimENodebId > 0) {
                            topPreciseService.queryCellStastic(victim.victimENodebId, victim.victimPci,
                                $scope.beginDate.value, $scope.endDate.value).then(function (coverage) {
                                    if (coverage) {
                                        victim.mrCount = coverage.mrCount;
                                        victim.weakCoverCount = coverage.weakCoverCount;
                                        victim.overCoverCount = coverage.overCoverCount;
                                    }
                                });
                        }
                    })
                });
        };
        $scope.showCoverage();
    })
    .controller('coverage.details.dialog', function ($scope, $uibModalInstance, dialogTitle, cellId, sectorId, date,
        kpiDisplayService, topPreciseService) {
        $scope.dialogTitle = dialogTitle;
        $scope.chartView = {
            options: ['覆盖指标', '干扰指标'],
            selected: '覆盖指标'
        };
        topPreciseService.queryOneDayCellStastic(cellId, sectorId, date).then(function (result) {
            var options = kpiDisplayService.getCoverageOptions(result, cellId + '-' + sectorId + '覆盖指标变化趋势');
            $("#weak-and-over-coverage").highcharts(options);
            var interferenceOptions = kpiDisplayService.getCoverageInterferenceOptions(result, cellId + '-' + sectorId + '干扰指标变化趋势');
            $("#interference-db").highcharts(interferenceOptions);
        });

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller('coverage.ta.dialog', function ($scope, $uibModalInstance, dialogTitle, cellId, sectorId, date,
        topPreciseService, cellPreciseService, kpiDisplayService) {
        $scope.dialogTitle = dialogTitle;
        $scope.chartView = {
            options: ['平均RSRP', '覆盖率'],
            selected: '平均RSRP'
        };
        topPreciseService.queryAverageRsrpTaStastic(cellId, sectorId, date).then(function (result) {
            var options = kpiDisplayService.getAverageRsrpTaOptions(result, cellId + '-' + sectorId + '平均RSRP统计');
            $("#average-rsrp").highcharts(options);
        });
        topPreciseService.queryAbove110TaRate(cellId, sectorId, date).then(function (above110Stat) {
            topPreciseService.queryAbove105TaRate(cellId, sectorId, date).then(function (above105Stat) {
                var options = kpiDisplayService.getAboveRateTaOptions(above110Stat, above105Stat, cellId + '-' + sectorId + '覆盖率统计');
                $("#coverage-rate").highcharts(options);
            });
        });

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller('coverage.ta.query.dialog', function ($scope, $uibModalInstance, dialogTitle, cellId, sectorId,
        topPreciseService, cellPreciseService, kpiDisplayService) {
        $scope.dialogTitle = dialogTitle;
        $scope.chartView = {
            options: ['平均RSRP', '覆盖率'],
            selected: '平均RSRP'
        };
        var lastWeek = new Date();
        lastWeek.setDate(lastWeek.getDate() - 7);
        $scope.beginDate = {
            value: lastWeek,
            opened: false
        };
        $scope.endDate = {
            value: new Date(),
            opened: false
        };
        $scope.query = function () {
            topPreciseService.queryAverageRsrpTaStastic(cellId, sectorId, $scope.beginDate.value, $scope.endDate.value).then(function (result) {
                var options = kpiDisplayService.getAverageRsrpTaOptions(result, cellId + '-' + sectorId + '平均RSRP统计');
                $("#average-rsrp").highcharts(options);
            });
            topPreciseService.queryAbove110TaRate(cellId, sectorId, $scope.beginDate.value, $scope.endDate.value).then(function (above110Stat) {
                topPreciseService.queryAbove105TaRate(cellId, sectorId, $scope.beginDate.value, $scope.endDate.value).then(function (above105Stat) {
                    var options = kpiDisplayService.getAboveRateTaOptions(above110Stat, above105Stat, cellId + '-' + sectorId + '覆盖率统计');
                    $("#coverage-rate").highcharts(options);
                });
            });
        };

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.query();
    });
