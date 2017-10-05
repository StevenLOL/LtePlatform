﻿angular.module('kpi.core', ['myApp.url', 'myApp.region'])
    .factory('kpiDisplayService',
        function(appFormatService,
            coverageService,
            topPreciseService,
            calculateService,
            chartCalculateService,
            generalChartService) {
            return {
                generatePreciseBarOptions: function(districtStats, cityStat) {
                    var chart = new BarChart();
                    chart.title.text = cityStat.city + "精确覆盖率统计";
                    chart.legend.enabled = false;
                    var category = [];
                    var precise = [];
                    angular.forEach(districtStats,
                        function(stat) {
                            category.push(stat.district);
                            precise.push(stat.preciseRate);
                        });
                    category.push(cityStat.city);
                    precise.push(cityStat.preciseRate);
                    chart.xAxis.categories = category;
                    chart.xAxis.title.text = '区域';
                    chart.setDefaultYAxis({
                        title: '精确覆盖率',
                        min: 70,
                        max: 100
                    });
                    var series = {
                        name: '精确覆盖率',
                        data: precise
                    };
                    chart.asignSeries(series);
                    return chart.options;
                },
                generateDownSwitchOptions: function(districtStats, city, cityDownSwitch) {
                    var chart = new BarChart();
                    chart.title.text = city + "4G用户3G流量比统计";
                    chart.legend.enabled = false;
                    var category = [];
                    var precise = [];
                    angular.forEach(districtStats,
                        function(stat) {
                            category.push(stat.region);
                            precise.push(stat.downSwitchRate);
                        });
                    category.push(city);
                    precise.push(cityDownSwitch);
                    chart.xAxis.categories = category;
                    chart.xAxis.title.text = '区域';
                    chart.setDefaultYAxis({
                        title: '4G用户3G流量比',
                        min: 0,
                        max: 10
                    });
                    var series = {
                        name: '4G用户3G流量比',
                        data: precise
                    };
                    chart.asignSeries(series);
                    return chart.options;
                },
                generateComboChartOptions: function(data, name) {
                    var setting = {
                        title: name,
                        xtitle: '日期',
                        ytitle: name
                    };
                    var categories = data.statDates;
                    var dataList = [];
                    var seriesTitle = [];
                    var typeList = [];
                    var kpiOption = appFormatService.lowerFirstLetter(name);
                    var type = kpiOption === "2G呼建(%)" ? 'line' : 'column';
                    angular.forEach(data.regionList,
                        function(item, $index) {
                            typeList.push($index === data.regionList.length - 1 ? 'spline' : type);
                            dataList.push(data.kpiDetails[kpiOption][$index]);
                            seriesTitle.push(item);
                        });

                    return generalChartService
                        .queryMultipleComboOptions(setting, categories, dataList, seriesTitle, typeList);
                },
                getMrsOptions: function(stats, title) {
                    var chart = new ComboChart();
                    chart.title.text = title;
                    var categoryKey = 'dateString';
                    var dataKeys = [
                        'totalMrs',
                        'firstNeighbors',
                        'secondNeighbors',
                        'thirdNeighbors'
                    ];
                    var seriesInfo = {
                        totalMrs: {
                            type: 'column',
                            name: "MR总数"
                        },
                        firstNeighbors: {
                            type: "spline",
                            name: "第一邻区MR数"
                        },
                        secondNeighbors: {
                            type: "spline",
                            name: "第二邻区MR数"
                        },
                        thirdNeighbors: {
                            type: "spline",
                            name: "第三邻区MR数"
                        }
                    };
                    var seriesData = chartCalculateService.generateSeriesInfo(seriesInfo, stats, categoryKey, dataKeys);
                    chart.xAxis[0].categories = seriesData.categories;
                    chart.yAxis[0].title.text = "MR数量";
                    chart.xAxis[0].title.text = '日期';
                    chartCalculateService.writeSeriesData(chart.series, seriesData.info, dataKeys);
                    return chart.options;
                },
                getPreciseOptions: function(stats, title) {
                    var chart = new ComboChart();
                    chart.title.text = title;
                    var statDates = [];
                    var firstRate = [];
                    var secondRate = [];
                    var thirdRate = [];
                    angular.forEach(stats,
                        function(stat) {
                            statDates.push(stat.dateString);
                            firstRate.push(100 - parseFloat(stat.firstRate));
                            secondRate.push(100 - parseFloat(stat.secondRate));
                            thirdRate.push(100 - parseFloat(stat.thirdRate));
                        });
                    chart.xAxis[0].categories = statDates;
                    chart.xAxis[0].title.text = '日期';
                    chart.yAxis[0].title.text = "精确覆盖率";
                    chart.series.push({
                        type: "spline",
                        name: "第一邻区精确覆盖率",
                        data: firstRate
                    });
                    chart.series.push({
                        type: "spline",
                        name: "第二邻区精确覆盖率",
                        data: secondRate
                    });
                    chart.series.push({
                        type: "spline",
                        name: "第三邻区精确覆盖率",
                        data: thirdRate
                    });
                    return chart.options;
                },
                getInterferencePieOptions: function(interferenceCells, currentCellName) {
                    var over6DbPie = new GradientPie();
                    var over10DbPie = new GradientPie();
                    var mod3Pie = new GradientPie();
                    var mod6Pie = new GradientPie();
                    over6DbPie.series[0].name = '6dB干扰日平均次数';
                    over10DbPie.series[0].name = '10dB干扰日平均次数';
                    over6DbPie.title.text = currentCellName + ': 6dB干扰日平均次数';
                    over10DbPie.title.text = currentCellName + ': 10dB干扰日平均次数';
                    mod3Pie.series[0].name = 'MOD3干扰日平均次数';
                    mod6Pie.series[0].name = 'MOD6干扰日平均次数';
                    mod3Pie.title.text = currentCellName + ': MOD3干扰日平均次数';
                    mod6Pie.title.text = currentCellName + ': MOD6干扰日平均次数';
                    angular.forEach(interferenceCells,
                        function(cell) {
                            over6DbPie.series[0].data.push({
                                name: cell.neighborCellName,
                                y: cell.overInterferences6Db
                            });
                            over10DbPie.series[0].data.push({
                                name: cell.neighborCellName,
                                y: cell.overInterferences10Db
                            });
                            if (cell.mod3Interferences > 0) {
                                mod3Pie.series[0].data.push({
                                    name: cell.neighborCellName,
                                    y: cell.mod3Interferences
                                });
                            }
                            if (cell.mod6Interferences > 0) {
                                mod6Pie.series[0].data.push({
                                    name: cell.neighborCellName,
                                    y: cell.mod6Interferences
                                });
                            }
                        });
                    return {
                        over6DbOption: over6DbPie.options,
                        over10DbOption: over10DbPie.options,
                        mod3Option: mod3Pie.options,
                        mod6Option: mod6Pie.options
                    };
                },
                getStrengthColumnOptions: function(interferenceCells, mrCount, currentCellName) {
                    var over6DbColumn = new Column3d();
                    var over10DbColumn = new Column3d();
                    over6DbColumn.series[0].name = '6dB干扰强度';
                    over10DbColumn.series[0].name = '10dB干扰强度';
                    over6DbColumn.title.text = currentCellName + ': 6dB干扰干扰强度';
                    over10DbColumn.title.text = currentCellName + ': 10dB干扰干扰强度';

                    angular.forEach(interferenceCells,
                        function(cell) {
                            over6DbColumn.series[0].data.push(cell.overInterferences6Db / mrCount * 100);
                            over10DbColumn.series[0].data.push(cell.overInterferences10Db / mrCount * 100);
                            over6DbColumn.xAxis.categories.push(cell.neighborCellName);
                            over10DbColumn.xAxis.categories.push(cell.neighborCellName);
                        });
                    return {
                        over6DbOption: over6DbColumn.options,
                        over10DbOption: over10DbColumn.options
                    };
                },
                calculatePreciseChange: function(input) {
                    var preKpis = input.slice(0, 7);
                    var postKpis = input.slice(input.length - 7);
                    var preSum = 0;
                    var postSum = 0;
                    angular.forEach(preKpis,
                        function(kpi) {
                            preSum += kpi.secondRate;
                        });
                    angular.forEach(postKpis,
                        function(kpi) {
                            postSum += kpi.secondRate;
                        });
                    return {
                        pre: 100 - preSum / 7,
                        post: 100 - postSum / 7
                    };
                },
                queryKpiOptions: function(network) {
                    switch (network) {
                    case '2G':
                        return {
                            options: ['Ec/Io', 'RxAGC', 'TxPower'],
                            selected: 'Ec/Io'
                        };
                    case '3G':
                        return {
                            options: ['SINR(3G)', 'RxAGC0', 'RxAGC1'],
                            selected: 'SINR(3G)'
                        };
                    default:
                        return {
                            options: ['RSRP', 'SINR'],
                            selected: 'RSRP'
                        };
                    }
                },
                queryCoverageLegend: function(kpi) {
                    switch (kpi) {
                    case 'Ec/Io':
                        return {
                            criteria: coverageService.defaultEcioCriteria,
                            sign: true
                        };
                    case 'RxAGC':
                        return {
                            criteria: coverageService.defaultRxCriteria,
                            sign: true
                        };
                    case 'TxPower':
                        return {
                            criteria: coverageService.defaultTxCriteria,
                            sign: false
                        };
                    case 'SINR(3G)':
                        return {
                            criteria: coverageService.defaultSinr3GCriteria,
                            sign: true
                        };
                    case 'RxAGC0':
                        return {
                            criteria: coverageService.defaultRxCriteria,
                            sign: true
                        };
                    case 'RxAGC1':
                        return {
                            criteria: coverageService.defaultRxCriteria,
                            sign: true
                        };
                    case 'RSRP':
                        return {
                            criteria: coverageService.defaultRsrpCriteria,
                            sign: true
                        };
                    case 'rsrpInterval':
                        return {
                            criteria: coverageService.rsrpIntervalCriteria,
                            sign: true
                        };
                    case 'competeResult':
                        return {
                            criteria: coverageService.competeCriteria,
                            sign: true
                        };
                    default:
                        return {
                            criteria: coverageService.defaultSinrCriteria,
                            sign: true
                        };
                    }
                },
                initializeCoveragePoints: function(legend) {
                    var pointDef = {
                        sign: legend.sign,
                        intervals: []
                    };
                    angular.forEach(legend.criteria,
                        function(interval) {
                            pointDef.intervals.push({
                                color: interval.color,
                                threshold: interval.threshold,
                                coors: []
                            });
                        });
                    pointDef.intervals.push({
                        color: "#077f07",
                        threshold: legend.sign ? 10000 : -10000,
                        coors: []
                    });
                    return pointDef;
                },
                generateCoveragePoints: function(pointDef, points, kpi) {
                    calculateService.generateCoveragePointsWithFunc(pointDef,
                        points,
                        function(point) {
                            switch (kpi) {
                            case 'Ec/Io':
                                return point.ecio;
                            case 'RxAGC':
                                return point.rxAgc;
                            case 'TxPower':
                                return point.txPower;
                            case 'SINR(3G)':
                                return point.sinr;
                            case 'RxAGC0':
                                return point.rxAgc0;
                            case 'RxAGC1':
                                return point.rxAgc1;
                            case 'RSRP':
                                return point.rsrp;
                            default:
                                return point.sinr;
                            }
                        });
                },
                generateRealRsrpPoints: function(pointDef, points) {
                    calculateService.generateCoveragePointsWithOffset(pointDef,
                        points,
                        function(point) {
                            return point.rsrp;
                        },
                        0.000245,
                        0.000225);
                },
                generateMobileRsrpPoints: function(pointDef, points) {
                    calculateService.generateCoveragePointsWithFunc(pointDef,
                        points,
                        function(point) {
                            return point.mobileRsrp;
                        });
                },
                generateTelecomRsrpPoints: function(pointDef, points) {
                    calculateService.generateCoveragePointsWithOffset(pointDef,
                        points,
                        function(point) {
                            return point.telecomRsrp - 140;
                        },
                        0.000245,
                        0.000225);
                },
                generateUnicomRsrpPoints: function(pointDef, points) {
                    calculateService.generateCoveragePointsWithFunc(pointDef,
                        points,
                        function(point) {
                            return point.unicomRsrp;
                        });
                },
                generateAverageRsrpPoints: function(pointDef, points) {
                    calculateService.generateCoveragePointsWithOffset(pointDef,
                        points,
                        function(point) {
                            return point.averageRsrp - 140;
                        },
                        0.000245,
                        0.000225);
                },
                updateCoverageKpi: function(neighbor, cell, dateSpan) {
                    topPreciseService.queryCoverage(dateSpan.begin,
                        dateSpan.end,
                        cell.cellId,
                        cell.sectorId).then(function(coverage) {
                        if (coverage.length > 0) {
                            var coverageRate = calculateService.calculateWeakCoverageRate(coverage);
                            neighbor.weakBelow115 = coverageRate.rate115;
                            neighbor.weakBelow110 = coverageRate.rate110;
                            neighbor.weakBelow105 = coverageRate.rate105;
                        }

                    });
                    topPreciseService.queryTa(dateSpan.begin,
                        dateSpan.end,
                        cell.cellId,
                        cell.sectorId).then(function(taList) {
                        if (taList.length > 0) {
                            neighbor.overCover = calculateService.calculateOverCoverageRate(taList);
                        }
                    });
                }
            };
        })
    .constant('kpiRatingDivisionDefs',
    {
        precise: [94.6, 83.6, 72.6, 61.6, 50],
        downSwitch: [3, 5, 8, 10, 15],
        drop: [0.2, 0.3, 0.35, 0.4, 0.5]
    })
    .factory('appKpiService',
        function(
            chartCalculateService,
            generalChartService,
            kpiRatingDivisionDefs,
            flowService,
            calculateService,
            appFormatService,
            preciseChartService) {
            return {
                getDownSwitchRate: function(stats) {
                    var flow3G = 0;
                    var flow4G = 0;
                    angular.forEach(stats,
                        function(stat) {
                            flow3G += stat.downSwitchFlow3G;
                            flow4G += stat.flow4G;
                        });
                    return 100 * flow3G / flow4G;
                },
                getCityStat: function(districtStats, currentCity) {
                    var stat = calculateService.initializePreciseCityStat(currentCity);
                    angular.forEach(districtStats,
                        function(districtStat) {
                            calculateService.accumulatePreciseStat(stat, districtStat);
                        });
                    return calculateService.calculateDistrictRates(stat);
                },
                getRrcCityStat: function(districtStats, currentCity) {
                    var stat = calculateService.initializeRrcCityStat(currentCity);
                    angular.forEach(districtStats,
                        function(districtStat) {
                            calculateService.accumulateRrcStat(stat, districtStat);
                        });
                    return calculateService.calculateDistrictRrcRates(stat);
                },
                getCqiCityStat: function(districtStats, currentCity) {
                    var stat = calculateService.initializeCqiCityStat(currentCity);
                    angular.forEach(districtStats,
                        function(districtStat) {
                            calculateService.accumulateCqiStat(stat, districtStat);
                        });
                    return calculateService.calculateDistrictCqiRates(stat);
                },
                getFlowCityStat: function(districtStats, currentCity) {
                    var stat = calculateService.initializeFlowCityStat(currentCity);
                    angular.forEach(districtStats,
                        function(districtStat) {
                            calculateService.accumulateFlowStat(stat, districtStat);
                        });
                    return calculateService.calculateDistrictFlowRates(stat);
                },
                calculatePreciseRating: function(precise) {
                    return calculateService.getValueFromDivisionAbove(kpiRatingDivisionDefs.precise, precise);
                },
                calculateDownSwitchRating: function(rate) {
                    return calculateService.getValueFromDivisionBelow(kpiRatingDivisionDefs.downSwitch, rate);
                },
                calculateDropStar: function(drop) {
                    return calculateService.getValueFromDivisionBelow(kpiRatingDivisionDefs.drop, drop);
                },
                calculateFlowStats: function(cellList, flowStats, mergeStats, beginDate, endDate) {
                    flowStats.length = 0;
                    mergeStats.length = 0;
                    angular.forEach(cellList,
                        function(cell) {
                            flowService.queryCellFlowByDateSpan(cell.eNodebId,
                                cell.sectorId,
                                beginDate.value,
                                endDate.value).then(function(flowList) {
                                cell.flowList = flowList;
                                if (flowList.length > 0) {
                                    flowStats.push(chartCalculateService.calculateMemberSum(flowList,
                                        [
                                            'averageActiveUsers',
                                            'averageUsers',
                                            'maxActiveUsers',
                                            'maxUsers',
                                            'pdcpDownlinkFlow',
                                            'pdcpUplinkFlow'
                                        ],
                                        function(stat) {
                                            stat.cellName = cell.eNodebName + '-' + cell.sectorId;
                                        }));
                                    calculateService.mergeDataByKey(mergeStats,
                                        flowList,
                                        'statTime',
                                        [
                                            'averageActiveUsers',
                                            'averageUsers',
                                            'maxActiveUsers',
                                            'maxUsers',
                                            'pdcpDownlinkFlow',
                                            'pdcpUplinkFlow'
                                        ]);
                                }
                            });
                        });
                },
                calculateFeelingStats: function(cellList, flowStats, mergeStats, beginDate, endDate) {
                    flowStats.length = 0;
                    mergeStats.length = 0;
                    angular.forEach(cellList,
                        function(cell) {
                            flowService.queryCellFlowByDateSpan(cell.eNodebId,
                                cell.sectorId,
                                beginDate.value,
                                endDate.value).then(function(flowList) {
                                cell.feelingList = flowList;
                                if (flowList.length > 0) {
                                    flowStats.push(chartCalculateService.calculateMemberSum(flowList,
                                        [
                                            'downlinkFeelingThroughput',
                                            'downlinkFeelingDuration',
                                            'uplinkFeelingThroughput',
                                            'uplinkFeelingDuration',
                                            'pdcpDownlinkFlow',
                                            'pdcpUplinkFlow',
                                            'schedulingRank2',
                                            'schedulingTimes',
                                            'redirectCdma2000'
                                        ],
                                        function(stat) {
                                            stat.cellName = cell.eNodebName + '-' + cell.sectorId;
                                            stat
                                                .downlinkFeelingRate =
                                                cell.downlinkFeelingThroughput / cell.downlinkFeelingDuration;
                                            stat
                                                .uplinkFeelingRate =
                                                cell.uplinkFeelingThroughput / cell.uplinkFeelingDuration;
                                            stat.rank2Rate = cell.schedulingRank2 * 100 / cell.schedulingTimes;
                                        }));
                                    calculateService.mergeDataByKey(mergeStats,
                                        flowList,
                                        'statTime',
                                        [
                                            'downlinkFeelingThroughput',
                                            'downlinkFeelingDuration',
                                            'uplinkFeelingThroughput',
                                            'uplinkFeelingDuration',
                                            'pdcpDownlinkFlow',
                                            'pdcpUplinkFlow',
                                            'schedulingRank2',
                                            'schedulingTimes',
                                            'redirectCdma2000'
                                        ]);
                                }
                            });
                        });
                },
                getMrPieOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.totalMrs;
                            }),
                        {
                            title: "分镇区测量报告数分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getRrcRequestOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.totalRrcRequest;
                            }),
                        {
                            title: "分镇区RRC连接数分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getCqiCountsOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.cqiCounts.item1 + stat.cqiCounts.item2;
                            }),
                        {
                            title: "分镇区CQI调度数分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getDownSwitchCountsOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.redirectCdma2000;
                            }),
                        {
                            title: "4G下切3G次数分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getPreciseRateOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.preciseRate;
                            }),
                        {
                            title: "分镇区精确覆盖率分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getRrcRateOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.rrcSuccessRate;
                            }),
                        {
                            title: "分镇区RRC连接成功率分布图",
                            seriesName: "区域",
                            yMin: 99,
                            yMax: 100
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getCqiRateOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.cqiRate;
                            }),
                        {
                            title: "分镇区CQI优良比分布图",
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getMoSignallingRrcRateOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.moSiganllingRrcRate;
                            }),
                        {
                            title: "分镇区主叫信令RRC连接成功率分布图",
                            seriesName: "区域",
                            yMin: 99,
                            yMax: 100
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getMtAccessRrcRateOptions: function(districtStats, townStats) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.mtAccessRrcRate;
                            }),
                        {
                            title: "分镇区被叫接入RRC连接成功率分布图",
                            seriesName: "区域",
                            yMin: 99,
                            yMax: 100
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getDownlinkRateOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.downlinkFeelingRate;
                            }),
                        {
                            title: "分镇区下行感知速率分布图（Mbit/s）-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域",
                            yMin: 5,
                            yMax: 40
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getUplinkRateOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.uplinkFeelingRate;
                            }),
                        {
                            title: "分镇区上行感知速率分布图（Mbit/s）-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域",
                            yMin: 0,
                            yMax: 15
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getDownSwitchTimesOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.redirectCdma2000;
                            }),
                        {
                            title: "分镇区4G下切3G次数-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getSchedulingTimesOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownPieOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.schedulingTimes;
                            }),
                        {
                            title: "分镇区调度次数-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getDownSwitchRateOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.downSwitchRate;
                            }),
                        {
                            title: "分镇区4G下切3G比例（次/GB）-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getRank2RateOptions: function(districtStats, townStats, frequency) {
                    return chartCalculateService.generateDrillDownColumnOptionsWithFunc(chartCalculateService
                        .generateDrillDownData(districtStats,
                            townStats,
                            function(stat) {
                                return stat.rank2Rate;
                            }),
                        {
                            title: "分镇区双流比（%）-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            seriesName: "区域"
                        },
                        appFormatService.generateDistrictPieNameValueFuncs());
                },
                getMrsDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.mr;
                        },
                        {
                            title: "MR总数变化趋势图",
                            xTitle: '日期',
                            yTitle: "MR总数"
                        });
                },
                getRrcRequestDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.request;
                        },
                        {
                            title: "RRC连接请求变化趋势图",
                            xTitle: '日期',
                            yTitle: "RRC连接请求数"
                        });
                },
                getCqiCountsDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.request;
                        },
                        {
                            title: "调度次数变化趋势图",
                            xTitle: '日期',
                            yTitle: "调度次数"
                        });
                },
                getDownlinkRateDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.downlinkFeelingRate;
                        },
                        {
                            title: "下行感知速率变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "下行感知速率（Mbit/s）"
                        });
                },
                getUplinkRateDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.uplinkFeelingRate;
                        },
                        {
                            title: "上行感知速率变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "上行感知速率（Mbit/s）"
                        });
                },
                getDownSwitchTimesDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.downSwitchTimes;
                        },
                        {
                            title: "下切次数变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "下切次数"
                        });
                },
                getSchedulingTimesDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.schedulingTimes;
                        },
                        {
                            title: "调度次数变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "调度次数"
                        });
                },
                getDownSwitchRateDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.downSwitchRate;
                        },
                        {
                            title: "下切比例变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "下切比例（次/GB）"
                        });
                },
                getRank2RateDistrictOptions: function(stats, inputDistricts, frequency) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.rank2Rate;
                        },
                        {
                            title: "双流比变化趋势图-" + (frequency === 'all' ? frequency : frequency + 'M'),
                            xTitle: '日期',
                            yTitle: "双流比（%）"
                        });
                },
                getPreciseDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.precise;
                        },
                        {
                            title: "精确覆盖率变化趋势图",
                            xTitle: '日期',
                            yTitle: "精确覆盖率"
                        });
                },
                getRrcRateDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.rate;
                        },
                        {
                            title: "RRC连接成功率变化趋势图",
                            xTitle: '日期',
                            yTitle: "RRC连接成功率"
                        });
                },
                getCqiRateDistrictOptions: function(stats, inputDistricts) {
                    var districts = inputDistricts.concat("全网");
                    return preciseChartService.generateDistrictTrendOptions(stats,
                        districts,
                        function(stat) {
                            return stat.rate;
                        },
                        {
                            title: "CQI优良比变化趋势图",
                            xTitle: '日期',
                            yTitle: "CQI优良比"
                        });
                },
                generateFeelingRateDistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.totalUplinkDuration = 0;
                                generalStat.totalUplinkThroughput = 0;
                                generalStat.totalDownlinkDuration = 0;
                                generalStat.totalDownlinkThroughput = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    uplinkFeelingRate: view.uplinkFeelingRate,
                                    downlinkFeelingRate: view.downlinkFeelingRate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.totalUplinkDuration += view.uplinkFeelingDuration;
                                generalStat.totalUplinkThroughput += view.uplinkFeelingThroughput;
                                generalStat.totalDownlinkDuration += view.downlinkFeelingDuration;
                                generalStat.totalDownlinkThroughput += view.downlinkFeelingThroughput;
                            },
                            zeroFunc: function() {
                                return {
                                    totalUplinkDuration: 0,
                                    totalUplinkThroughput: 0,
                                    totalDownlinkDuration: 0,
                                    totalDownlinkThroughput: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    uplinkFeelingRate: generalStat.totalUplinkThroughput /
                                        generalStat.totalUplinkDuration,
                                    downlinkFeelingRate: generalStat.totalDownlinkThroughput /
                                        generalStat.totalDownlinkDuration
                                };
                            }
                        });
                },
                generateDownSwitchDistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.totalDownSwitchTimes = 0;
                                generalStat.totalUplinkThroughput = 0;
                                generalStat.totalDownlinkThroughput = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    downSwitchTimes: view.redirectCdma2000,
                                    downSwitchRate: view.downSwitchRate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.totalDownSwitchTimes += view.redirectCdma2000;
                                generalStat.totalUplinkThroughput += view.pdcpUplinkFlow;
                                generalStat.totalDownlinkThroughput += view.pdcpDownlinkFlow;
                            },
                            zeroFunc: function() {
                                return {
                                    totalDownSwitchTimes: 0,
                                    totalUplinkThroughput: 0,
                                    totalDownlinkThroughput: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    downSwitchTimes: generalStat.totalDownSwitchTimes,
                                    downSwitchRate: 1024 *
                                        8 *
                                        generalStat.totalDownSwitchTimes /
                                        (generalStat.totalUplinkThroughput + generalStat.totalDownlinkThroughput)
                                };
                            }
                        });
                },
                generateRank2DistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.totalRank2Times = 0;
                                generalStat.totalSchedulingTimes = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    schedulingTimes: view.schedulingTimes,
                                    rank2Rate: view.rank2Rate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.totalRank2Times += view.schedulingRank2;
                                generalStat.totalSchedulingTimes += view.schedulingTimes;
                            },
                            zeroFunc: function() {
                                return {
                                    totalRank2Times: 0,
                                    totalSchedulingTimes: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    schedulingTimes: generalStat.totalSchedulingTimes,
                                    rank2Rate: 100 * generalStat.totalRank2Times / generalStat.totalSchedulingTimes
                                };
                            }
                        });
                },
                generateDistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.totalMrs = 0;
                                generalStat.totalSecondNeighbors = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    mr: view.totalMrs,
                                    precise: view.preciseRate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.totalMrs += view.totalMrs;
                                generalStat.totalSecondNeighbors += view.secondNeighbors;
                            },
                            zeroFunc: function() {
                                return {
                                    mr: 0,
                                    precise: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    mr: generalStat.totalMrs,
                                    precise: 100 - 100 * generalStat.totalSecondNeighbors / generalStat.totalMrs
                                }
                            }
                        });
                },
                generateRrcDistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.totalRrcRequest = 0;
                                generalStat.totalRrcSuccess = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    request: view.totalRrcRequest,
                                    rate: view.rrcSuccessRate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.totalRrcRequest += view.totalRrcRequest;
                                generalStat.totalRrcSuccess += view.totalRrcSuccess;
                            },
                            zeroFunc: function() {
                                return {
                                    request: 0,
                                    rate: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    request: generalStat.totalRrcRequest,
                                    rate: 100 * generalStat.totalRrcSuccess / generalStat.totalRrcRequest
                                }
                            }
                        });
                },
                generateCqiDistrictStats: function(districts, stats) {
                    return chartCalculateService.generateDistrictStats(districts,
                        stats,
                        {
                            districtViewFunc: function(stat) {
                                return stat.districtViews;
                            },
                            initializeFunc: function(generalStat) {
                                generalStat.goodCounts = 0;
                                generalStat.totalCounts = 0;
                            },
                            calculateFunc: function(view) {
                                return {
                                    request: view.cqiCounts.item1 + view.cqiCounts.item2,
                                    rate: view.cqiRate
                                };
                            },
                            accumulateFunc: function(generalStat, view) {
                                generalStat.goodCounts += view.cqiCounts.item2;
                                generalStat.totalCounts += view.cqiCounts.item1 + view.cqiCounts.item2;
                            },
                            zeroFunc: function() {
                                return {
                                    request: 0,
                                    rate: 0
                                };
                            },
                            totalFunc: function(generalStat) {
                                return {
                                    request: generalStat.totalCounts,
                                    rate: 100 * generalStat.goodCounts / generalStat.totalCounts
                                }
                            }
                        });
                },
                calculateAverageRates: function(stats) {
                    var result = {
                        statDate: "平均值",
                        values: calculateService.calculateAverageValues(stats, ['mr', 'precise'])
                    };
                    return result;
                },
                calculateAverageRrcRates: function(stats) {
                    var result = {
                        statDate: "平均值",
                        values: calculateService.calculateAverageValues(stats, ['request', 'rate'])
                    };
                    return result;
                },
                calculateAverageDownSwitchRates: function(stats) {
                    var result = {
                        statDate: "平均值",
                        values: calculateService.calculateAverageValues(stats, ['downSwitchTimes', 'downSwitchRate'])
                    };
                    return result;
                },
                generateTrendStatsForPie: function(trendStat, result) {
                    chartCalculateService.generateStatsForPie(trendStat,
                        result,
                        {
                            districtViewsFunc: function(stat) {
                                return stat.districtViews;
                            },
                            townViewsFunc: function(stat) {
                                return stat.townViews;
                            },
                            accumulateFunc: function(source, accumulate) {
                                calculateService.accumulatePreciseStat(source, accumulate);
                            },
                            districtCalculate: function(stat) {
                                calculateService.calculateDistrictRates(stat);
                            },
                            townCalculate: function(stat) {
                                calculateService.calculateTownRates(stat);
                            }
                        });
                },
                generateRrcTrendStatsForPie: function(trendStat, result) {
                    chartCalculateService.generateStatsForPie(trendStat,
                        result,
                        {
                            districtViewsFunc: function(stat) {
                                return stat.districtViews;
                            },
                            townViewsFunc: function(stat) {
                                return stat.townViews;
                            },
                            accumulateFunc: function(source, accumulate) {
                                calculateService.accumulateRrcStat(source, accumulate);
                            },
                            districtCalculate: function(stat) {
                                calculateService.calculateDistrictRrcRates(stat);
                            },
                            townCalculate: function(stat) {
                                calculateService.calculateTownRrcRates(stat);
                            }
                        });
                },
                generateCqiTrendStatsForPie: function(trendStat, result) {
                    chartCalculateService.generateStatsForPie(trendStat,
                        result,
                        {
                            districtViewsFunc: function(stat) {
                                return stat.districtViews;
                            },
                            townViewsFunc: function(stat) {
                                return stat.townViews;
                            },
                            accumulateFunc: function(source, accumulate) {
                                calculateService.accumulateCqiStat(source, accumulate);
                            },
                            districtCalculate: function(stat) {
                                calculateService.calculateDistrictRrcRates(stat);
                            },
                            townCalculate: function(stat) {
                                calculateService.calculateTownRrcRates(stat);
                            }
                        });
                },
                getPreciseObject: function(district) {
                    var objectTable = {
                        "禅城": 89.8,
                        "南海": 90,
                        "三水": 90,
                        "高明": 90,
                        "顺德": 90.2
                    };
                    return objectTable[district] === undefined ? 90 : objectTable[district];
                },
                getRrcObject: function(district) {
                    var objectTable = {
                        "禅城": 99,
                        "南海": 99,
                        "三水": 99,
                        "高明": 99,
                        "顺德": 99
                    };
                    return objectTable[district] === undefined ? 99 : objectTable[district];
                },
                getCqiObject: function(district) {
                    var objectTable = {
                        "禅城": 92,
                        "南海": 92,
                        "三水": 93,
                        "高明": 94,
                        "顺德": 92
                    };
                    return objectTable[district] === undefined ? 99 : objectTable[district];
                },
                getDownSwitchObject: function(district) {
                    var objectTable = {
                        "禅城": 25,
                        "南海": 25,
                        "三水": 15,
                        "高明": 15,
                        "顺德": 25
                    };
                    return objectTable[district] === undefined ? 99 : objectTable[district];
                },
                generateComplainTrendOptions: function(dates, counts, objects) {
                    var chart = new TimeSeriesLine();
                    chart.title.text = '月度抱怨量变化趋势图';
                    chart.setDefaultXAxis({
                        title: '日期',
                        categories: dates
                    });
                    chart.setDefaultYAxis({
                        title: '抱怨量'
                    });
                    chart.insertSeries({
                        name: '指标值',
                        data: counts
                    });
                    chart.insertSeries({
                        name: '目标值',
                        data: objects
                    });
                    return chart.options;
                },
                generateColumnOptions: function(stat, title, xtitle, ytitle) {
                    return generalChartService.getColumnOptions(stat,
                        {
                            title: title,
                            xtitle: xtitle,
                            ytitle: ytitle
                        },
                        function(data) {
                            return data.item1;
                        },
                        function(data) {
                            return data.item2;
                        });
                }
            }
        });