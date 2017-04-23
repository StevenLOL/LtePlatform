﻿angular.module("myApp", ['app.common'])
    .config(function($stateProvider, $urlRouterProvider) {
        var viewDir = "/appViews/Home/";
        $stateProvider
            .state('list', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.root"
                    },
                    "contents": {
                        templateUrl: viewDir + "Network.html",
                        controller: "home.network"
                    }
                },
                url: "/"
            })
            .state('query', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.query"
                    },
                    "contents": {
                        templateUrl: viewDir + "Query.html",
                        controller: "home.query"
                    }
                },
                url: "/query"
            })
            .state('flow', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.query"
                    },
                    "contents": {
                        templateUrl: viewDir + "Flow.html",
                        controller: "home.flow"
                    }
                },
                url: "/flow"
            })
            .state('dt', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.dt"
                    },
                    "contents": {
                        templateUrl: viewDir + "Network.html",
                        controller: "home.dt"
                    }
                },
                url: "/dt"
            })
            .state('kpi', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.kpi"
                    },
                    "contents": {
                        templateUrl: viewDir + "Kpi.html",
                        controller: "home.kpi"
                    }
                },
                url: "/kpi"
            })
            .state('plan', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.plan"
                    },
                    "contents": {
                        templateUrl: viewDir + "Plan.html",
                        controller: "home.plan"
                    }
                },
                url: "/plan"
            })
            .state('complain', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.complain"
                    },
                    "contents": {
                        templateUrl: viewDir + "Complain.html",
                        controller: "home.complain"
                    }
                },
                url: "/complain"
            })
            .state('mr', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.mr"
                    },
                    "contents": {
                        templateUrl: viewDir + "Mr.html",
                        controller: "home.mr"
                    }
                },
                url: "/mr"
            })
            .state('grid', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.mr"
                    },
                    "contents": {
                        templateUrl: viewDir + "MrGrid.html",
                        controller: "mr.grid"
                    }
                },
                url: "/grid"
            })
            .state('app', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.mr"
                    },
                    "contents": {
                        templateUrl: viewDir + "MrGrid.html",
                        controller: "mr.app"
                    }
                },
                url: "/app"
            })
            .state('analysis', {
                views: {
                    'menu': {
                        templateUrl: "/appViews/DropDownMenu.html",
                        controller: "menu.mr"
                    },
                    "contents": {
                        templateUrl: viewDir + "MrGrid.html",
                        controller: "network.analysis"
                    }
                },
                url: "/analysis"
            });
        $urlRouterProvider.otherwise('/');
    })
    .run(function ($rootScope, appUrlService, appRegionService, geometryService) {
        $rootScope.rootPath = "/#/";

        $rootScope.page = {
            title: "基础数据总览"
        };
        appUrlService.initializeAuthorization();
        $rootScope.legend = {
            criteria: 0,
            sign: 0,
            title: ''
        };

        var lastWeek = new Date();
        lastWeek.setDate(lastWeek.getDate() - 7);
        $rootScope.beginDate = {
            value: new Date(lastWeek.getFullYear(), lastWeek.getMonth(), lastWeek.getDate(), 8),
            opened: false
        };
        var today = new Date();
        $rootScope.endDate = {
            value: new Date(today.getFullYear(), today.getMonth(), today.getDate(), 8),
            opened: false
        };
        $rootScope.closeAlert = function(messages, index) {
            messages.splice(index, 1);
        };
        var yesterday = new Date();
        yesterday.setDate(yesterday.getDate() - 1);
        $rootScope.statDate = {
            value: yesterday,
            opened: false
        };

        $rootScope.status = {
            isopen: false
        };
        $rootScope.city = {
            selected: "",
            options: []
        };
        appRegionService.initializeCities()
            .then(function(result) {
                $rootScope.city.options = result;
                $rootScope.city.selected = result[0];
            });

        $rootScope.indexedDB = {
            name: 'ouyh18',
            version: 7,
            db: null
        };
        $rootScope.colors = geometryService.queryMapColors();
    })
    .controller("menu.root", function($scope, appUrlService) {
        var rootUrl = "/#";
        $scope.menuItem = {
            displayName: "站点信息",
            subItems: [
                {
                    displayName: "数据总览",
                    url: rootUrl + "/"
                }, {
                    displayName: "历史信息查询",
                    url: appUrlService.getParameterUrlHost() + 'cell-modify-record.php'
                }, {
                    displayName: "边界漫游",
                    url: appUrlService.getParameterUrlHost() + 'lteboundary.html'
                }
            ]
        };
    })
    .controller("menu.query", function ($scope, appUrlService) {
        var rootUrl = "/#";
        $scope.menuItem = {
            displayName: "站点查询",
            subItems: [
                {
                    displayName: "数据查询",
                    url: rootUrl + "/query"
                }, {
                    displayName: "数据维护",
                    url: appUrlService.getParameterUrlHost() + 'main-lte-cell-info.php'
                }, {
                    displayName: "流量经营",
                    url: rootUrl + '/flow'
                }
            ]
        };
    })
    .controller('menu.plan', function($scope, appUrlService) {
        $scope.menuItem = {
            displayName: "规划支撑",
            subItems: [
                {
                    displayName: "规划辅助",
                    url: appUrlService.getPlanUrlHost() + 'guihua'
                }, {
                    displayName: "完整准确性保障",
                    url: appUrlService.getParameterUrlHost() + 'zhunquexing2.php'
                }, {
                    displayName: "工程入网",
                    url: appUrlService.getParameterUrlHost() + 'zhunquexing2.php'
                }
            ]
        };
    })
    .controller('menu.complain', function ($scope) {
        $scope.menuItem = {
            displayName: "投诉管理",
            subItems: [
                {
                    displayName: "统计分析",
                    url: '/#/complain/stat'
                }
            ]
        };
    })
    .controller('menu.kpi', function($scope, appUrlService) {
        $scope.menuItem = {
            displayName: "指标优化",
            subItems: [
                {
                    displayName: "指标总览",
                    url: "/Rutrace",
                    tooltip: "4G总体指标"
                }, {
                    displayName: "质量分析",
                    url: appUrlService.getTopnUrlHost() + '2g_home',
                    tooltip: "4G网络质量分析与日常优化"
                }, {
                    displayName: "负荷评估",
                    url: appUrlService.getParameterUrlHost() + 'ltecapability.html'
                }
            ]
        };
    })
    .controller('menu.mr', function ($scope) {
        var rootUrl = "/#";
        $scope.menuItem = {
            displayName: "MR分析",
            subItems: [
                {
                    displayName: "模拟路测",
                    url: rootUrl + "/mr"
                },{
                    displayName: "栅格分析",
                    url: rootUrl + "/grid"
                }, {
                    displayName: "APP分析",
                    url: rootUrl + "/app"
                }, {
                    displayName: "工单管控",
                    url: "/Kpi/WorkItem",
                    tooltip: "对接本部优化部4G网优平台，结合日常优化，实现对日常工单的监控和分析"
                }
            ]
        }
    })
    .controller('menu.dt', function($scope, appUrlService) {
        $scope.menuItem = {
            displayName: "路测管理",
            subItems: [
                {
                    displayName: "路测分析",
                    url: appUrlService.getDtUrlHost() + 'admin'
                }, {
                    displayName: "路测管理",
                    url: appUrlService.getDtUrlHost2(),
                    tooltip: "路测综合管理"
                }
            ]
        }
    })
    .controller("home.network", function ($scope, appRegionService, networkElementService, baiduMapService, coverageDialogService,
        geometryService, parametersDialogService, neGeometryService, baiduQueryService, dumpPreciseService) {
        baiduMapService.initializeMap("map", 11);

        $scope.showDistrictOutdoor = function(district, color) {
            var city = $scope.city.selected;
            baiduMapService.addDistrictBoundary(district, color);
            networkElementService.queryOutdoorCellSites(city, district).then(function(sites) {
                baiduQueryService.transformToBaidu(sites[0].longtitute, sites[0].lattitute).then(function(coors) {
                    var xOffset = coors.x - sites[0].longtitute;
                    var yOffset = coors.y - sites[0].lattitute;
                    baiduMapService.drawMultiPoints(sites, color, -xOffset, -yOffset, function(e) {
                        var xCenter = e.point.lng - xOffset;
                        var yCenter = e.point.lat - yOffset;
                        networkElementService.queryRangeSectors(
                            neGeometryService.queryNearestRange(xCenter, yCenter), []).then(function(sectors) {
                            parametersDialogService.showCellsInfo(sectors);
                        });
                    });
                });
            });
        };
        
        $scope.showOutdoorSites = function () {
            $scope.currentView = "室外小区";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showDistrictOutdoor(district, $scope.colors[$index]);
            });
        };

        $scope.showDistrictIndoor = function(district, color) {
            var city = $scope.city.selected;
            baiduMapService.addDistrictBoundary(district, color);
            networkElementService.queryIndoorCellSites(city, district).then(function(sites) {
                baiduQueryService.transformToBaidu(sites[0].longtitute, sites[0].lattitute).then(function(coors) {
                    var xOffset = coors.x - sites[0].longtitute;
                    var yOffset = coors.y - sites[0].lattitute;
                    baiduMapService.drawMultiPoints(sites, color, -xOffset, -yOffset, function(e) {
                        var xCenter = e.point.lng - xOffset;
                        var yCenter = e.point.lat - yOffset;
                        networkElementService.queryRangeSectors(
                            neGeometryService.queryNearestRange(xCenter, yCenter), []).then(function(sectors) {
                            parametersDialogService.showCellsInfo(sectors);
                        });
                    });
                });
            });
        };

        $scope.showIndoorSites = function () {
            $scope.currentView = "室内小区";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            angular.forEach($scope.districts, function (district, $index) {
                $scope.showDistrictIndoor(district, $scope.colors[$index]);
            });
        };

        $scope.showLteTownStats = function () {
            var city = $scope.city.selected;
            coverageDialogService.showTownStats(city);
        };
        $scope.showCdmaTownStats = function () {
            var city = $scope.city.selected;
            coverageDialogService.showCdmaTownStats(city);
        };
        $scope.districts = [];
        
        $scope.$watch('city.selected', function(city) {
            if (city) {
                dumpPreciseService.generateUsersDistrict(city, $scope.districts, function(district, $index) {
                    $scope.showDistrictOutdoor(district, $scope.colors[$index]);
                });
            }
        });
    })
    .controller('home.flow', function ($scope, baiduMapService, baiduQueryService, coverageDialogService, flowService) {
        baiduMapService.initializeMap("map", 11);
        $scope.showFeelingRate = function () {
            if (!$scope.flowGeoPoints) {
                alert("计算未完成！请稍后点击。");
                return;
            }
            $scope.currentView = "下行感知速率";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            baiduQueryService.transformToBaidu($scope.flowGeoPoints[0].longtitute, $scope.flowGeoPoints[0].lattitute).then(function (coors) {
                var xOffset = coors.x - $scope.flowGeoPoints[0].longtitute;
                var yOffset = coors.y - $scope.flowGeoPoints[0].lattitute;
                var points = _.map($scope.flowGeoPoints, function (stat) {
                    return { "lng": stat.longtitute + xOffset, "lat": stat.lattitute + yOffset, "count": stat.downlinkFeelingRate };
                });
                var heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": 20 });
                baiduMapService.addOverlays([heatmapOverlay]);
                heatmapOverlay.setDataSet({ data: points, max: 50 });
                heatmapOverlay.show();
            });
        };

        $scope.showUplinkFeelingRate = function () {
            if (!$scope.flowGeoPoints) {
                alert("计算未完成！请稍后点击。");
                return;
            }
            $scope.currentView = "上行感知速率";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            baiduQueryService.transformToBaidu($scope.flowGeoPoints[0].longtitute, $scope.flowGeoPoints[0].lattitute).then(function (coors) {
                var xOffset = coors.x - $scope.flowGeoPoints[0].longtitute;
                var yOffset = coors.y - $scope.flowGeoPoints[0].lattitute;
                var points = _.map($scope.flowGeoPoints, function (stat) {
                    return { "lng": stat.longtitute + xOffset, "lat": stat.lattitute + yOffset, "count": stat.uplinkFeelingRate };
                });
                var heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": 20 });
                baiduMapService.addOverlays([heatmapOverlay]);
                heatmapOverlay.setDataSet({ data: points, max: 10 });
                heatmapOverlay.show();
            });

        };


        $scope.showFlow = function () {
            coverageDialogService.showFlowStats($scope.statDate.value || new Date());
        };
        $scope.showFlowTrend = function () {
            coverageDialogService.showFlowTrend($scope.city.selected, $scope.beginDate, $scope.endDate);
        };
        $scope.showUsersTrend = function () {
            coverageDialogService.showUsersTrend($scope.city.selected, $scope.beginDate, $scope.endDate);
        };
        $scope.showFeelingRateTrend = function () {
            coverageDialogService.showFeelingRateTrend($scope.city.selected, $scope.beginDate, $scope.endDate);
        };
        flowService.queryENodebGeoFlowByDateSpan($scope.beginDate.value, $scope.endDate.value).then(function (result) {
            $scope.flowGeoPoints = result;
            $scope.showFeelingRate();
        });
    })
    .controller("home.dt", function ($scope, baiduMapService, coverageService, appFormatService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
        coverageService.queryAreaTestDate().then(function (result) {
            angular.forEach(result, function (item) {
                baiduMapService.drawCustomizeLabel(item.longtitute, item.lattitute, item.cityName + item.districtName + item.townName,
                    '4G测试日期:' + appFormatService.getDateString(appFormatService.getDate(item.latestDate4G), 'yyyy-MM-dd')
                    + '<br/>3G测试日期:' + appFormatService.getDateString(appFormatService.getDate(item.latestDate3G), 'yyyy-MM-dd')
                    + '<br/>2G测试日期:' + appFormatService.getDateString(appFormatService.getDate(item.latestDate2G), 'yyyy-MM-dd'), 3);
            });

        });

    })
    .controller("home.mr", function ($scope, baiduMapService, coverageService, kpiDisplayService, parametersMapService, appUrlService,
    coverageDialogService) {
        baiduMapService.initializeMap("map", 13);
        
        var legend = kpiDisplayService.queryCoverageLegend('RSRP');
        $scope.legend.title = 'RSRP';
        $scope.legend.criteria = legend.criteria;
        $scope.legend.sign = legend.sign;
        $scope.currentDataLabel = "districtPoints";
        $scope.showStats = function() {
            coverageDialogService.showAgpsStats($scope.data);
        };
        $scope.showTelecomCoverage = function () {
            $scope.currentView = "电信";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            $scope.coveragePoints = kpiDisplayService.initializeCoveragePoints($scope.legend);
            kpiDisplayService.generateTelecomRsrpPoints($scope.coveragePoints, $scope.data);
            parametersMapService.showIntervalPoints($scope.coveragePoints.intervals);
        };
        $scope.showUnicomCoverage = function () {
            $scope.currentView = "联通";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            $scope.coveragePoints = kpiDisplayService.initializeCoveragePoints($scope.legend);
            kpiDisplayService.generateUnicomRsrpPoints($scope.coveragePoints, $scope.data);
            parametersMapService.showIntervalPoints($scope.coveragePoints.intervals);
        };
        $scope.showMobileCoverage = function () {
            $scope.currentView = "移动";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            $scope.coveragePoints = kpiDisplayService.initializeCoveragePoints($scope.legend);
            kpiDisplayService.generateMobileRsrpPoints($scope.coveragePoints, $scope.data);
            parametersMapService.showIntervalPoints($scope.coveragePoints.intervals);
        };
        $scope.updateIndexedData = function() {
            var items = [];
            angular.forEach($scope.data, function(data) {
                data['topic'] = data.longtitute + ',' + data.lattitute;
                items.push(data);
            });
            appUrlService.refreshIndexedDb($scope.indexedDB.db, $scope.currentDataLabel, 'topic', items);
        };
        $scope.showDistrictRange = function() {
            coverageService.queryAgisDtPointsByTopic($scope.beginDate.value, $scope.endDate.value, '禅城').then(function (result) {
                $scope.data = result;
                $scope.currentDataLabel = 'districtPoints';
                $scope.updateIndexedData();
                $scope.showTelecomCoverage();
            });
        };
        $scope.showSmallRange = function() {
            coverageService.queryAgisDtPointsByTopic($scope.beginDate.value, $scope.endDate.value, '小范围').then(function(result) {
                $scope.data = result;
                $scope.currentDataLabel = 'rangePoints';
                $scope.updateIndexedData();
                $scope.showTelecomCoverage();
            });
        };
        $scope.switchData = function() {
            if ($scope.currentDataLabel === 'districtPoints') {
                $scope.currentDataLabel = 'rangePoints';
            } else {
                $scope.currentDataLabel = 'districtPoints';
            }
            appUrlService.fetchStoreByCursor($scope.indexedDB.db, $scope.currentDataLabel, function (items) {
                $scope.data = items;
                $scope.showTelecomCoverage();
            });
        };
        appUrlService.initializeIndexedDb($scope.indexedDB, ['districtPoints','rangePoints'], "topic", function () {
            appUrlService.fetchStoreByCursor($scope.indexedDB.db, 'districtPoints', function(items) {
                $scope.data = items;
                $scope.showTelecomCoverage();
            });
        });
    })
    .controller("mr.grid", function ($scope, baiduMapService, coverageService, authorizeService, kpiDisplayService,
        baiduQueryService, coverageDialogService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
        $scope.currentView = "自身覆盖";
        $scope.setRsrpLegend = function() {
            var legend = kpiDisplayService.queryCoverageLegend('rsrpInterval');
            $scope.legend.title = 'RSRP区间';
            $scope.legend.criteria = legend.criteria;
            $scope.colorDictionary = {};
            angular.forEach(legend.criteria, function(info) {
                $scope.colorDictionary[info.threshold] = info.color;
            });
            $scope.legend.sign = legend.sign;
        };
        $scope.setCompeteLegend = function() {
            var legend = kpiDisplayService.queryCoverageLegend('competeResult');
            $scope.legend.title = '竞争结果';
            $scope.legend.criteria = legend.criteria;
            $scope.colorDictionary = {};
            angular.forEach(legend.criteria, function(info) {
                $scope.colorDictionary[info.threshold] = info.color;
                
            });
            $scope.legend.sign = legend.sign;
        };
        $scope.showGridStats = function () {
            var keys = [];
            angular.forEach($scope.legend.criteria, function (info) {
                keys.push(info.threshold);
            });
            coverageDialogService.showGridStats($scope.currentDistrict, $scope.currentView, $scope.legend.title, $scope.areaStats, keys);
        };
        $scope.showDistrictSelfCoverage = function (district, color) {
            baiduMapService.clearOverlays();
            baiduMapService.addDistrictBoundary(district, color);
            $scope.areaStats = {};
            angular.forEach($scope.legend.criteria, function(info) {
                $scope.areaStats[info.threshold] = 0;
            });
            coverageService.queryMrGridSelfCoverage(district, $scope.endDate.value).then(function (result) {
                baiduQueryService.transformToBaidu(113, 23).then(function(coors) {
                    var xOffset = coors.x - 113;
                    var yOffset = coors.y - 23;
                    angular.forEach(result, function(item) {
                        var gridColor = $scope.colorDictionary[item.rsrpLevelDescription];
                        var polygon = baiduMapService.drawPolygonWithColor(item.coordinates, gridColor, -xOffset, -yOffset);
                        var area = BMapLib.GeoUtils.getPolygonArea(polygon);
                        
                        if (area > 0) {
                            $scope.areaStats[item.rsrpLevelDescription] += area;
                        }
                    });
                });
            });
        };
        $scope.showDistrictCompeteCoverage = function (district, color, competeDescription) {
            baiduMapService.clearOverlays();
            baiduMapService.addDistrictBoundary(district, color);
            $scope.areaStats = {};
            angular.forEach($scope.legend.criteria, function (info) {
                $scope.areaStats[info.threshold] = 0;
            });
            coverageService.queryMrGridCompete(district, $scope.endDate.value, competeDescription).then(function(result) {
                baiduQueryService.transformToBaidu(113, 23).then(function (coors) {
                    var xOffset = coors.x - 113;
                    var yOffset = coors.y - 23;
                    angular.forEach(result, function (item) {
                        var gridColor = $scope.colorDictionary[item.rsrpLevelDescription];
                        var polygon = baiduMapService.drawPolygonWithColor(item.coordinates, gridColor, -xOffset, -yOffset);
                        var area = BMapLib.GeoUtils.getPolygonArea(polygon);

                        if (area > 0) {
                            $scope.areaStats[item.rsrpLevelDescription] += area;
                        }
                    });
                });
            });
        };
        $scope.showMrGrid = function (district) {
            $scope.currentDistrict = district;
            if ($scope.currentView === '自身覆盖') {
                $scope.setRsrpLegend();
                $scope.showDistrictSelfCoverage(district, $scope.colors[0]);
            } else {
                $scope.showDistrictCompeteCoverage(district, $scope.colors[0], $scope.currentView);
            }
            
        };
        $scope.showTelecomCoverage = function () {
            $scope.currentView = "自身覆盖";
            $scope.setRsrpLegend();
            $scope.showDistrictSelfCoverage($scope.currentDistrict, $scope.colors[0]);
        };
        $scope.showMobileCompete = function () {
            $scope.currentView = "移动竞对";
            $scope.setCompeteLegend();
            $scope.showDistrictCompeteCoverage($scope.currentDistrict, $scope.colors[0], $scope.currentView);
        };
        $scope.showUnicomCompete = function() {
            $scope.currentView = "联通竞对";
            $scope.setCompeteLegend();
            $scope.showDistrictCompeteCoverage($scope.currentDistrict, $scope.colors[0], $scope.currentView);
        };
        $scope.showOverallCompete = function() {
            $scope.currentView = "竞对总体";
            $scope.setCompeteLegend();
            $scope.showDistrictCompeteCoverage($scope.currentDistrict, $scope.colors[0], $scope.currentView);
        };
        $scope.districts = [];
        $scope.setRsrpLegend();
        authorizeService.queryCurrentUserName().then(function (userName) {
            authorizeService.queryRolesInUser(userName).then(function (roles) {
                angular.forEach(roles, function (role) {
                    var district = authorizeService.queryRoleDistrict(role);
                    if (district) {
                        $scope.districts.push(district);
                    }
                });
                if ($scope.districts.length > 0) {
                    $scope.currentDistrict = $scope.districts[0];
                    $scope.showDistrictSelfCoverage($scope.districts[0], $scope.colors[0]);
                }
            });
        });
    })
    .controller("mr.app", function ($scope, baiduMapService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");

    })
    .controller("home.complain", function ($scope, baiduMapService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");

    })
    .controller("network.analysis", function($scope, baiduMapService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
    })
    .controller("home.query", function ($scope, baiduMapService, neighborDialogService, dumpPreciseService, appRegionService,
        parametersDialogService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
        $scope.queryConditions = function () {
            baiduMapService.clearOverlays();
            neighborDialogService.setQueryConditions($scope.city, $scope.beginDate, $scope.endDate);
        };
        $scope.queryByTowns = function() {
            baiduMapService.clearOverlays();
            neighborDialogService.queryList($scope.city);
        };
        $scope.districts = [];
        $scope.$watch('city.selected', function (city) {
            if (city) {
                dumpPreciseService.generateUsersDistrict(city, $scope.districts, function (district, $index) {
                    baiduMapService.addDistrictBoundary(district, $scope.colors[$index]);
                    appRegionService.queryTownInfrastructures($scope.city.selected, district).then(function (result) {
                        angular.forEach(result, function(stat) {
                            appRegionService.queryTown(city, district, stat.town).then(function(town) {
                                angular.extend(stat, town);
                                var marker = baiduMapService.generateIconMarker(stat.longtitute, stat.lattitute,
                                    "/Content/Images/Hotmap/site_or.png");
                                baiduMapService.addOneMarkerToScope(marker, function (item) {
                                    parametersDialogService.showTownENodebInfo(item, city, district);
                                }, stat);
                            });
                        });
                    });
                });
                
            }
        });
    })
    .controller("home.kpi", function ($scope, baiduMapService, dumpPreciseService, kpiPreciseService, networkElementService, baiduQueryService,
    neighborDialogService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
        $scope.currentView = "精确覆盖率";

        $scope.showPreciseRate = function(city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            kpiPreciseService.queryTopKpisInDistrict($scope.beginDate.value, $scope.endDate.value, 10,
                '按照精确覆盖率升序', city, district).then(function(result) {
                networkElementService.queryCellSectors(result).then(function(cells) {
                    baiduQueryService.transformToBaidu(cells[0].longtitute, cells[0].lattitute).then(function(coors) {
                        var xOffset = coors.x - cells[0].longtitute;
                        var yOffset = coors.y - cells[0].lattitute;
                        angular.forEach(cells, function(cell) {
                            cell.longtitute += xOffset;
                            cell.lattitute += yOffset;
                            var sectorTriangle = baiduMapService.generateSector(cell, "blue", 5);
                            baiduMapService.addOneSectorToScope(sectorTriangle, neighborDialogService.showPrecise, cell);
                        });
                    });
                });
            });
        };
        $scope.showTopPrecise = function() {
            $scope.currentView = "精确覆盖率";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showPreciseRate(city, district, $scope.colors[$index]);
            });
        };
        $scope.showDownSwitchSite = function (city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            kpiPreciseService.queryTopDownSwitchInDistrict($scope.beginDate.value, $scope.endDate.value, 10,
                city, district).then(function (result) {
                angular.forEach(result, function(item) {
                    networkElementService.queryCellInfo(item.eNodebId, item.sectorId).then(function(cell) {
                        baiduQueryService.transformToBaidu(cell.longtitute, cell.lattitute).then(function(coors) {
                            item = angular.extend(item, cell);
                            cell.longtitute = coors.x;
                            cell.lattitute = coors.y;
                            var sectorTriangle = baiduMapService.generateSector(cell, "blue", 5);
                            baiduMapService.addOneSectorToScope(sectorTriangle, neighborDialogService.showFlowCell, {
                                item: item,
                                beginDate: $scope.beginDate,
                                endDate: $scope.endDate
                            });
                        });
                    });
                });

            });
        };
        $scope.showTopDownSwitch = function() {
            $scope.currentView = "4G下切3G";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showDownSwitchSite(city, district, $scope.colors[$index]);
            });
        };
        $scope.showRank2Site = function (city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            kpiPreciseService.queryTopRank2InDistrict($scope.beginDate.value, $scope.endDate.value, 10,
                city, district).then(function (result) {
                    angular.forEach(result, function (item) {
                        networkElementService.queryCellInfo(item.eNodebId, item.sectorId).then(function (cell) {
                            baiduQueryService.transformToBaidu(cell.longtitute, cell.lattitute).then(function (coors) {
                                item = angular.extend(item, cell);
                                cell.longtitute = coors.x;
                                cell.lattitute = coors.y;
                                var sectorTriangle = baiduMapService.generateSector(cell, "blue", 5);
                                baiduMapService.addOneSectorToScope(sectorTriangle, neighborDialogService.showFlowCell, {
                                    item: item,
                                    beginDate: $scope.beginDate,
                                    endDate: $scope.endDate
                                });
                            });
                        });
                    });

                });
        };
        $scope.showTopScheduling = function () {
            $scope.currentView = "4G双流比";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function (district, $index) {
                $scope.showRank2Site(city, district, $scope.colors[$index]);
            });
        };
        $scope.districts = [];
        dumpPreciseService.generateUsersDistrict($scope.city.selected || "佛山", $scope.districts, function(district, $index) {
            $scope.showPreciseRate($scope.city.selected || "佛山", district, $scope.colors[$index]);
        });

    })
    .controller("home.plan", function ($scope, baiduMapService, dumpPreciseService, networkElementService, geometryService,
        neGeometryService, parametersDialogService, baiduQueryService) {
        baiduMapService.initializeMap("map", 11);
        baiduMapService.addCityBoundary("佛山");
        $scope.currentView = "所有站点";
        $scope.showPlanningSite = function(city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            networkElementService.queryPlanningSites(city, district).then(function(sites) {
                baiduQueryService.transformToBaidu(sites[0].longtitute, sites[0].lattitute).then(function(coors) {
                    var xOffset = coors.x - sites[0].longtitute;
                    var yOffset = coors.y - sites[0].lattitute;
                    baiduMapService.drawMultiPoints(sites, color, -xOffset, -yOffset, function(e) {
                        var xCenter = e.point.lng - xOffset;
                        var yCenter = e.point.lat - yOffset;
                        networkElementService.queryRangePlanningSites(
                            neGeometryService.queryNearestRange(xCenter, yCenter)).then(function(sectors) {
                            if (sectors.length > 0) {
                                parametersDialogService.showPlanningSitesInfo(sectors[0]);
                            }

                        });
                    });
                });
            });
        };
        $scope.showAllSites = function() {
            $scope.currentView = "所有站点";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showPlanningSite(city, district, $scope.colors[$index]);
            });
        };
        $scope.showOpenningSite = function(city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            networkElementService.queryOpenningSites(city, district).then(function(sites) {
                baiduQueryService.transformToBaidu(sites[0].longtitute, sites[0].lattitute).then(function(coors) {
                    var xOffset = coors.x - sites[0].longtitute;
                    var yOffset = coors.y - sites[0].lattitute;
                    baiduMapService.drawMultiPoints(sites, color, -xOffset, -yOffset, function(e) {
                        var xCenter = e.point.lng - xOffset;
                        var yCenter = e.point.lat - yOffset;
                        networkElementService.queryRangePlanningSites(
                            neGeometryService.queryNearestRange(xCenter, yCenter)).then(function(sectors) {
                            if (sectors.length > 0) {
                                parametersDialogService.showPlanningSitesInfo(sectors[0]);
                            }

                        });
                    });
                });
            });
        };
        $scope.showOpenningSites = function() {
            $scope.currentView = "待开通站点";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showOpenningSite(city, district, $scope.colors[$index]);
            });
        };
        $scope.showOpenSite = function(city, district, color) {
            baiduMapService.addDistrictBoundary(district, color);
            networkElementService.queryOpennedSites(city, district).then(function(sites) {
                baiduQueryService.transformToBaidu(sites[0].longtitute, sites[0].lattitute).then(function(coors) {
                    var xOffset = coors.x - sites[0].longtitute;
                    var yOffset = coors.y - sites[0].lattitute;
                    baiduMapService.drawMultiPoints(sites, color, -xOffset, -yOffset, function(e) {
                        var xCenter = e.point.lng - xOffset;
                        var yCenter = e.point.lat - yOffset;
                        networkElementService.queryRangePlanningSites(
                            neGeometryService.queryNearestRange(xCenter, yCenter)).then(function(sectors) {
                            if (sectors.length > 0) {
                                parametersDialogService.showPlanningSitesInfo(sectors[0]);
                            }

                        });
                    });
                });
            });
        };
        $scope.showOpenSites = function() {
            $scope.currentView = "已开通站点";
            baiduMapService.clearOverlays();
            baiduMapService.addCityBoundary("佛山");
            var city = $scope.city.selected;
            angular.forEach($scope.districts, function(district, $index) {
                $scope.showOpenSite(city, district, $scope.colors[$index]);
            });
        };
        $scope.districts = [];
        dumpPreciseService.generateUsersDistrict($scope.city.selected || "佛山", $scope.districts, function(district, $index) {
            $scope.showPlanningSite($scope.city.selected || "佛山", district, $scope.colors[$index]);
        });
    });
