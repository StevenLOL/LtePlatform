﻿angular.module('kpi.coverage', ['myApp.url', 'myApp.region', "ui.bootstrap"])
    .controller('coverage.details.dialog', function ($scope, $uibModalInstance, cellName, cellId, sectorId,
        topPreciseService, preciseChartService) {
        $scope.dialogTitle = cellName + '：覆盖详细信息';
        $scope.showCoverage = function () {
            topPreciseService.queryRsrpTa($scope.beginDate.value, $scope.endDate.value,
                cellId, sectorId).then(function (result) {
                    for (var rsrpIndex = 0; rsrpIndex < 12; rsrpIndex++) {
                        var options = preciseChartService.getRsrpTaOptions(result, rsrpIndex);
                        $("#rsrp-ta-" + rsrpIndex).highcharts(options);
                    }
                });
            topPreciseService.queryCoverage($scope.beginDate.value, $scope.endDate.value,
                cellId, sectorId).then(function (result) {
                    var options = preciseChartService.getCoverageOptions(result);
                    $("#coverage-chart").highcharts(options);
                });
            topPreciseService.queryTa($scope.beginDate.value, $scope.endDate.value,
                cellId, sectorId).then(function (result) {
                    var options = preciseChartService.getTaOptions(result);
                    $("#ta-chart").highcharts(options);
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close($scope.coverageInfos);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.showCoverage();
    })
    .controller('interference.source.dialog', function ($scope, $uibModalInstance, dialogTitle, eNodebId, sectorId,
        preciseInterferenceService, neighborMongoService) {
        $scope.dialogTitle = dialogTitle;
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
        var options = [
            {
                name: "模3干扰数",
                value: "mod3Interferences"
            }, {
                name: "模6干扰数",
                value: "mod6Interferences"
            }, {
                name: "6dB干扰数",
                value: "overInterferences6Db"
            }, {
                name: "10dB干扰数",
                value: "overInterferences10Db"
            }, {
                name: "总干扰水平",
                value: "interferenceLevel"
            }
        ];
        $scope.orderPolicy = {
            options: options,
            selected: options[4].value
        };
        $scope.displayItems = {
            options: [5, 10, 15, 20, 30],
            selected: 10
        };

        $scope.showInterference = function () {
            $scope.interferenceCells = [];

            preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                eNodebId, sectorId).then(function (result) {
                    angular.forEach(result, function (cell) {
                        for (var i = 0; i < $scope.mongoNeighbors.length; i++) {
                            var neighbor = $scope.mongoNeighbors[i];
                            if (neighbor.neighborPci === cell.destPci) {
                                cell.isMongoNeighbor = true;
                                break;
                            }
                        }
                    });
                    $scope.interferenceCells = result;
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close($scope.interferenceCells);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        neighborMongoService.queryNeighbors(eNodebId, sectorId).then(function (result) {
            $scope.mongoNeighbors = result;
            $scope.showInterference();
        });
    })
    .controller('interference.source.db.chart', function ($scope, $uibModalInstance, dialogTitle, eNodebId, sectorId, name,
        topPreciseService, kpiDisplayService, preciseInterferenceService) {
        $scope.dialogTitle = dialogTitle;
        $scope.currentCellName = name + "-" + sectorId;
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
        $scope.showChart = function () {
            preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                eNodebId, sectorId).then(function (result) {
                    var pieOptions = kpiDisplayService.getInterferencePieOptions(result, $scope.currentCellName);
                    $("#interference-over6db").highcharts(pieOptions.over6DbOption);
                    $("#interference-over10db").highcharts(pieOptions.over10DbOption);
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.showChart();
    })
    .controller('interference.source.mod.chart', function ($scope, $uibModalInstance, dialogTitle, eNodebId, sectorId, name,
        topPreciseService, kpiDisplayService, preciseInterferenceService) {
        $scope.dialogTitle = dialogTitle;
        $scope.currentCellName = name + "-" + sectorId;
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
        $scope.showChart = function () {
            preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                eNodebId, sectorId).then(function (result) {
                    var pieOptions = kpiDisplayService.getInterferencePieOptions(result, $scope.currentCellName);
                    $("#interference-mod3").highcharts(pieOptions.mod3Option);
                    $("#interference-mod6").highcharts(pieOptions.mod6Option);
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.showChart();
    })
    .controller('interference.source.strength.chart', function ($scope, $uibModalInstance, dialogTitle, eNodebId, sectorId, name,
        topPreciseService, kpiDisplayService, preciseInterferenceService, neighborMongoService, networkElementService) {
        $scope.dialogTitle = dialogTitle;
        $scope.currentCellName = name + "-" + sectorId;
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
        $scope.showChart = function () {
            preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                eNodebId, sectorId).then(function (result) {
                    networkElementService.queryCellInfo(eNodebId, sectorId).then(function (info) {
                        topPreciseService.queryCellStastic(eNodebId, info.pci,
                            $scope.beginDate.value, $scope.endDate.value).then(function (stastic) {
                                var columnOptions = kpiDisplayService.getStrengthColumnOptions(result, stastic.mrCount,
                                    $scope.currentCellName);
                                $("#strength-over6db").highcharts(columnOptions.over6DbOption);
                                $("#strength-over10db").highcharts(columnOptions.over10DbOption);
                            });
                    });
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close('已处理');
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.showChart();
    })
    .controller('interference.victim.dialog', function ($scope, $uibModalInstance, dialogTitle, eNodebId, sectorId,
        topPreciseService, preciseInterferenceService) {
        $scope.dialogTitle = dialogTitle;
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
        var options = [
            {
                name: "模3干扰数",
                value: "mod3Interferences"
            }, {
                name: "模6干扰数",
                value: "mod6Interferences"
            }, {
                name: "6dB干扰数",
                value: "overInterferences6Db"
            }, {
                name: "10dB干扰数",
                value: "overInterferences10Db"
            }, {
                name: "总干扰水平",
                value: "interferenceLevel"
            }
        ];
        $scope.orderPolicy = {
            options: options,
            selected: options[4].value
        };
        $scope.displayItems = {
            options: [5, 10, 15, 20, 30],
            selected: 10
        };

        $scope.showVictim = function () {
            $scope.victimCells = [];

            preciseInterferenceService.queryInterferenceVictim($scope.beginDate.value, $scope.endDate.value,
                eNodebId, sectorId).then(function (victims) {
                    preciseInterferenceService.queryInterferenceNeighbor($scope.beginDate.value, $scope.endDate.value,
                        eNodebId, sectorId).then(function (result) {
                            angular.forEach(victims, function (victim) {
                                for (var j = 0; j < result.length; j++) {
                                    if (result[j].destENodebId === victim.victimENodebId
                                        && result[j].destSectorId === victim.victimSectorId) {
                                        victim.forwardInterferences6Db = result[j].overInterferences6Db;
                                        victim.forwardInterferences10Db = result[j].overInterferences10Db;
                                        break;
                                    }
                                }
                            });
                            $scope.victimCells = victims;
                        });
                });
        };

        $scope.ok = function () {
            $uibModalInstance.close($scope.victimCells);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $scope.showVictim();
    })
    .controller('interference.coverage.dialog', function($scope) {
        
    })

    .controller("grid.stats", function ($scope, dialogTitle, category, stats, $uibModalInstance, $timeout, generalChartService) {
        $scope.dialogTitle = dialogTitle;
        var options = generalChartService.getPieOptions(stats, {
            title: dialogTitle,
            seriesTitle: category
        }, function (stat) {
            return stat.key;
        }, function (stat) {
            return stat.value;
        });
        $timeout(function () {
            $("#rightChart").highcharts(options);
        }, 500);

        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("agps.stats", function ($scope, dialogTitle, stats, $uibModalInstance, $timeout, generalChartService) {
        $scope.dialogTitle = dialogTitle;
        var operatorStats = [
        {
            operator: '移动主导',
            count: _.countBy(stats, function (stat) { return stat.domination === '移动主导' })['true']
        },
        {
            operator: '联通主导',
            count: _.countBy(stats, function (stat) { return stat.domination === '联通主导' })['true']
        },
        {
            operator: '电信主导',
            count: _.countBy(stats, function (stat) { return stat.domination === '电信主导' })['true']
        }];
        var counts = stats.length;
        var operators = ['移动', '联通', '电信'];
        var coverages = [
            _.countBy(stats, function (stat) { return stat.mobileRsrp >= -110 })['true'] / counts * 100,
            _.countBy(stats, function (stat) { return stat.unicomRsrp >= -110 })['true'] / counts * 100,
            _.countBy(stats, function (stat) { return stat.telecomRsrp >= -110 })['true'] / counts * 100
        ];
        $timeout(function () {
            $("#leftChart").highcharts(generalChartService.getPieOptions(operatorStats, {
                title: '主导运营商分布比例',
                seriesTitle: '主导运营商'
            }, function (stat) {
                return stat.operator;
            }, function (stat) {
                return stat.count;
            }));
            $("#rightChart").highcharts(generalChartService.queryColumnOptions({
                title: '运营商覆盖率比较（RSRP>=-110dBm）',
                ytitle: '覆盖率（%）',
                xtitle: '运营商',
                min: 80,
                max: 100
            }, operators, coverages));
        }, 500);

        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("town.stats", function ($scope, cityName, dialogTitle, $uibModalInstance, appRegionService, parametersChartService) {
        $scope.dialogTitle = dialogTitle;
        appRegionService.queryDistrictInfrastructures(cityName).then(function (result) {
            appRegionService.accumulateCityStat(result, cityName);
            $("#leftChart").highcharts(
                parametersChartService.getDistrictLteENodebPieOptions(result.slice(0, result.length - 1), cityName));
            $("#rightChart").highcharts(
                parametersChartService.getDistrictLteCellPieOptions(result.slice(0, result.length - 1), cityName));
        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("cdma.town.stats", function ($scope, cityName, dialogTitle, $uibModalInstance, appRegionService, parametersChartService) {
        $scope.dialogTitle = dialogTitle;
        appRegionService.queryDistrictInfrastructures(cityName).then(function (result) {
            appRegionService.accumulateCityStat(result, cityName);
            $("#leftChart").highcharts(
                parametersChartService.getDistrictCdmaBtsPieOptions(result.slice(0, result.length - 1), cityName));
            $("#rightChart").highcharts(
                parametersChartService.getDistrictCdmaCellPieOptions(result.slice(0, result.length - 1), cityName));
        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("flow.stats", function ($scope, today, dialogTitle, $uibModalInstance, appRegionService, preciseChartService) {
        $scope.dialogTitle = dialogTitle;
        appRegionService.getTownFlowStats(today).then(function (result) {
            $("#leftChart").highcharts(preciseChartService.getTownFlowOption(result));
            $("#rightChart").highcharts(preciseChartService.getTownUsersOption(result));
        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("flow.trend", function ($scope, beginDate, endDate, city, dialogTitle, $uibModalInstance,
        kpiPreciseService, appFormatService, appKpiService, appRegionService) {
        $scope.dialogTitle = appFormatService.getDateString(beginDate.value, "yyyy年MM月dd日") + '-'
            + appFormatService.getDateString(endDate.value, "yyyy年MM月dd日")
            + dialogTitle;
        kpiPreciseService.getDateSpanFlowRegionKpi(city, beginDate.value, endDate.value).then(function (result) {
            appRegionService.queryDistricts(city).then(function (districts) {
                var stats = appKpiService.generateFlowDistrictStats(districts, result);
                var trendStat = {};
                appKpiService.generateFlowTrendStatsForPie(trendStat, result);
                $("#leftChart").highcharts(appKpiService.getDownlinkFlowDistrictOptions(stats, districts));
                $("#rightChart").highcharts(appKpiService.getUplinkFlowDistrictOptions(stats, districts));
                $("#thirdChart").highcharts(appKpiService.getDownlinkFlowOptions(trendStat.districtStats, trendStat.townStats));
                $("#fourthChart").highcharts(appKpiService.getUplinkFlowOptions(trendStat.districtStats, trendStat.townStats));
            });

        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("users.trend", function ($scope, beginDate, endDate, city, dialogTitle, $uibModalInstance,
        kpiPreciseService, appFormatService, appKpiService, appRegionService) {
        $scope.dialogTitle = appFormatService.getDateString(beginDate.value, "yyyy年MM月dd日") + '-'
            + appFormatService.getDateString(endDate.value, "yyyy年MM月dd日")
            + dialogTitle;
        kpiPreciseService.getDateSpanFlowRegionKpi(city, beginDate.value, endDate.value).then(function (result) {
            appRegionService.queryDistricts(city).then(function (districts) {
                var stats = appKpiService.generateUsersDistrictStats(districts, result);
                var trendStat = {};
                appKpiService.generateFlowTrendStatsForPie(trendStat, result);
                $("#leftChart").highcharts(appKpiService.getMaxUsersDistrictOptions(stats, districts));
                $("#rightChart").highcharts(appKpiService.getMaxActiveUsersDistrictOptions(stats, districts));
                $("#thirdChart").highcharts(appKpiService.getMaxUsersOptions(trendStat.districtStats, trendStat.townStats));
                $("#fourthChart").highcharts(appKpiService.getMaxActiveUsersOptions(trendStat.districtStats, trendStat.townStats));
            });

        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })
    .controller("feelingRate.trend", function ($scope, beginDate, endDate, city, dialogTitle, $uibModalInstance,
        kpiPreciseService, appFormatService, appKpiService, appRegionService) {
        $scope.dialogTitle = appFormatService.getDateString(beginDate.value, "yyyy年MM月dd日") + '-'
            + appFormatService.getDateString(endDate.value, "yyyy年MM月dd日")
            + dialogTitle;
        kpiPreciseService.getDateSpanFlowRegionKpi(city, beginDate.value, endDate.value).then(function (result) {
            appRegionService.queryDistricts(city).then(function (districts) {
                var stats = appKpiService.generateFeelingRateDistrictStats(districts, result);
                var trendStat = {};
                appKpiService.generateFlowTrendStatsForPie(trendStat, result);
                $("#leftChart").highcharts(appKpiService.getDownlinkRateDistrictOptions(stats, districts));
                $("#rightChart").highcharts(appKpiService.getUplinkRateDistrictOptions(stats, districts));
                $("#thirdChart").highcharts(appKpiService.getDownlinkRateOptions(trendStat.districtStats, trendStat.townStats));
                $("#fourthChart").highcharts(appKpiService.getUplinkRateOptions(trendStat.districtStats, trendStat.townStats));
            });

        });
        $scope.ok = function () {
            $uibModalInstance.close($scope.city);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    })

     .controller("user.roles.dialog", function ($scope, $uibModalInstance, dialogTitle, userName, authorizeService) {
         $scope.dialogTitle = dialogTitle;
         $scope.ok = function () {
             $uibModalInstance.close($scope.city);
         };

         $scope.cancel = function () {
             $uibModalInstance.dismiss('cancel');
         };

         $scope.query = function () {
             authorizeService.queryRolesInUser(userName).then(function (roles) {
                 $scope.existedRoles = roles;
             });
             authorizeService.queryCandidateRolesInUser(userName).then(function (roles) {
                 $scope.candidateRoles = roles;
             });
         };

         $scope.addRole = function (role) {
             authorizeService.assignRoleInUser(userName, role).then(function (result) {
                 if (result) {
                     $scope.query();
                 }
             });
         };

         $scope.removeRole = function (role) {
             authorizeService.releaseRoleInUser(userName, role).then(function (result) {
                 if (result) {
                     $scope.query();
                 }
             });
         };

         $scope.query();
     })

    .factory('coverageDialogService', function (menuItemService) {
        return {
            showDetails: function (cellName, cellId, sectorId) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Rutrace/Coverage/DetailsChartDialog.html',
                    controller: 'coverage.details.dialog',
                    resolve: {
                        cellName: function () {
                            return cellName;
                        },
                        cellId: function () {
                            return cellId;
                        },
                        sectorId: function () {
                            return sectorId;
                        }
                    }
                });
            },
            showSource: function (currentView, serialNumber, callback) {
                menuItemService.showGeneralDialogWithAction({
                    templateUrl: '/appViews/Rutrace/Interference/SourceDialog.html',
                    controller: 'interference.source.dialog',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "干扰源分析";
                        },
                        eNodebId: function () {
                            return currentView.eNodebId;
                        },
                        sectorId: function () {
                            return currentView.sectorId;
                        },
                        serialNumber: function () {
                            return serialNumber;
                        }
                    }
                }, function (info) {
                    callback(info);
                });
            },
            showSourceDbChart: function (currentView) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Rutrace/Interference/SourceDbChartDialog.html',
                    controller: 'interference.source.db.chart',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "干扰源图表";
                        },
                        eNodebId: function () {
                            return currentView.eNodebId;
                        },
                        sectorId: function () {
                            return currentView.sectorId;
                        },
                        name: function () {
                            return currentView.eNodebName;
                        }
                    }
                });
            },
            showSourceModChart: function (currentView, callback) {
                menuItemService.showGeneralDialogWithAction({
                    templateUrl: '/appViews/Rutrace/Interference/SourceModChartDialog.html',
                    controller: 'interference.source.mod.chart',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "MOD3/MOD6干扰图表";
                        },
                        eNodebId: function () {
                            return currentView.eNodebId;
                        },
                        sectorId: function () {
                            return currentView.sectorId;
                        },
                        name: function () {
                            return currentView.eNodebName;
                        }
                    }
                }, function (info) {
                    callback(info);
                });
            },
            showSourceStrengthChart: function (currentView, callback) {
                menuItemService.showGeneralDialogWithAction({
                    templateUrl: '/appViews/Rutrace/Interference/SourceStrengthChartDialog.html',
                    controller: 'interference.source.strength.chart',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "干扰强度图表";
                        },
                        eNodebId: function () {
                            return currentView.eNodebId;
                        },
                        sectorId: function () {
                            return currentView.sectorId;
                        },
                        name: function () {
                            return currentView.eNodebName;
                        }
                    }
                }, function (info) {
                    callback(info);
                });
            },
            showInterferenceVictim: function (currentView, serialNumber, callback) {
                menuItemService.showGeneralDialogWithAction({
                    templateUrl: '/appViews/Rutrace/Interference/VictimDialog.html',
                    controller: 'interference.victim.dialog',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "干扰小区分析";
                        },
                        eNodebId: function () {
                            return currentView.eNodebId;
                        },
                        sectorId: function () {
                            return currentView.sectorId;
                        },
                        serialNumber: function () {
                            return serialNumber;
                        }
                    }
                }, function (info) {
                    callback(info);
                });
            },
            showCoverage: function (currentView, preciseCells, callback) {
                menuItemService.showGeneralDialogWithAction({
                    templateUrl: '/appViews/Rutrace/Interference/CoverageDialog.html',
                    controller: 'interference.coverage.dialog',
                    resolve: {
                        dialogTitle: function () {
                            return currentView.eNodebName + "-" + currentView.sectorId + "覆盖分析";
                        },
                        preciseCells: function () {
                            return preciseCells;
                        }
                    }
                }, function (info) {
                    callback(info);
                });
            },///////////未完成
            showGridStats: function (district, town, theme, category, data, keys) {
                var stats = [];
                angular.forEach(keys, function (key) {
                    stats.push({
                        key: key,
                        value: data[key]
                    });
                });
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/SingleChartDialog.html',
                    controller: 'grid.stats',
                    resolve: {
                        dialogTitle: function () {
                            return district + town + theme;
                        },
                        category: function () {
                            return category;
                        },
                        stats: function () {
                            return stats;
                        }
                    }
                });
            },
            showAgpsStats: function (stats) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/DoubleChartDialog.html',
                    controller: 'agps.stats',
                    resolve: {
                        dialogTitle: function () {
                            return 'AGPS三网对比覆盖指标';
                        },
                        stats: function () {
                            return stats;
                        }
                    }
                });
            },
            showTownStats: function (cityName) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/DoubleChartDialog.html',
                    controller: 'town.stats',
                    resolve: {
                        dialogTitle: function () {
                            return "全市LTE基站小区分布";
                        },
                        cityName: function () {
                            return cityName;
                        }
                    }
                });
            },
            showCdmaTownStats: function (cityName) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/DoubleChartDialog.html',
                    controller: 'cdma.town.stats',
                    resolve: {
                        dialogTitle: function () {
                            return "全市CDMA基站小区分布";
                        },
                        cityName: function () {
                            return cityName;
                        }
                    }
                });
            },
            showFlowStats: function (today) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/DoubleChartDialog.html',
                    controller: 'flow.stats',
                    resolve: {
                        dialogTitle: function () {
                            return "全市4G流量和用户数分布";
                        },
                        today: function () {
                            return today;
                        }
                    }
                });
            },
            showFlowTrend: function (city, beginDate, endDate) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/FourChartDialog.html',
                    controller: 'flow.trend',
                    resolve: {
                        dialogTitle: function () {
                            return city + "流量变化趋势";
                        },
                        beginDate: function () {
                            return beginDate;
                        },
                        endDate: function () {
                            return endDate;
                        },
                        city: function () {
                            return city;
                        }
                    }
                });
            },
            showUsersTrend: function (city, beginDate, endDate) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/FourChartDialog.html',
                    controller: 'users.trend',
                    resolve: {
                        dialogTitle: function () {
                            return city + "用户数变化趋势";
                        },
                        beginDate: function () {
                            return beginDate;
                        },
                        endDate: function () {
                            return endDate;
                        },
                        city: function () {
                            return city;
                        }
                    }
                });
            },
            showFeelingRateTrend: function (city, beginDate, endDate) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Home/FourChartDialog.html',
                    controller: 'feelingRate.trend',
                    resolve: {
                        dialogTitle: function () {
                            return city + "感知速率变化趋势";
                        },
                        beginDate: function () {
                            return beginDate;
                        },
                        endDate: function () {
                            return endDate;
                        },
                        city: function () {
                            return city;
                        }
                    }
                });
            },
            showUserRoles: function (userName) {
                menuItemService.showGeneralDialog({
                    templateUrl: '/appViews/Manage/UserRolesDialog.html',
                    controller: 'user.roles.dialog',
                    resolve: {
                        dialogTitle: function () {
                            return userName + "角色管理";
                        },
                        userName: function () {
                            return userName;
                        }
                    }
                });
            }
        }
    })