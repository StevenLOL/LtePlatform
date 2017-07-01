﻿angular.module('college.module', ['ui.grid', 'myApp.region', 'myApp.url'])

    .controller('CollegeDtController', function($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'name', name: '校园名称' },
                { field: 'area', name: '区域面积（平方米）', cellFilter: 'number: 2' },
                { field: 'centerX', name: '中心经度', cellFilter: 'number: 4' },
                { field: 'centerY', name: '中心纬度', cellFilter: 'number: 4' },
                {
                    field: 'file2Gs.length',
                    name: '2G文件数',
                    cellTooltip: function(row, col) {
                        var html = '';
                        angular.forEach(row.entity.file2Gs, function(file) {
                            html += file.csvFileName + ', 网格数: ' + file.rasterNums.length + '\n';
                        });
                        return html;
                    }
                },
                {
                    field: 'file3Gs.length',
                    name: '3G文件数',
                    cellTooltip: function (row, col) {
                        var html = '';
                        angular.forEach(row.entity.file3Gs, function (file) {
                            html += file.csvFileName + ', 网格数: ' + file.rasterNums.length + '\n';
                        });
                        return html;
                    }
                },
                {
                    field: 'file4Gs.length',
                    name: '4G文件数',
                    cellTooltip: function (row, col) {
                        var html = '';
                        angular.forEach(row.entity.file4Gs, function (file) {
                            html += file.csvFileName + ', 网格数: ' + file.rasterNums.length + '\n';
                        });
                        return html;
                    }
                }
            ],
            data: $scope.colleges
        };
    })
    .directive('collegeDtList', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CollegeDtController',
            scope: {
                colleges: '='
            },
            argumentName: 'colleges'
        }, $compile);
    })
    .controller('College3GController', function ($scope, uiGridConstants) {
        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            columnDefs: [
                { field: 'place', name: '测试地点' },
                { field: 'tester', name: '测试人员' },
                { field: 'testTime', name: '测试时间', cellFilter: 'date: "yyyy-MM-dd HH"' },
                {
                    field: 'accessUsers', name: '接入用户数',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'downloadRate', name: '下载速率（kbps）', cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'maxRssi', name: 'RSSI最大值',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'minRssi', name: 'RSSI最小值',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'vswr', name: '驻波比',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                }
            ],
            data: []
        };
    })
    .directive('collegeTest3List', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'College3GController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })
    .controller('College4GController', function ($scope, uiGridConstants) {
        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            columnDefs: [
                { field: 'place', name: '测试地点' },
                { field: 'tester', name: '测试人员' },
                { field: 'testTime', name: '测试时间', cellFilter: 'date: "yyyy-MM-dd HH"' },
                {
                    field: 'accessUsers', name: '接入用户数',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'downloadRate', name: '下载速率（kbps）', cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'uploadRate', name: '上传速率（kbps）', cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                { field: 'cellName', name: '小区名称' },
                {
                    field: 'rsrp', name: 'RSRP(dBm)', cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                },
                {
                    field: 'sinr', name: 'SINR(dB)', cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true
                }
            ],
            data: []
        };
    })
    .directive('collegeTest4List', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'College4GController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile); 
    })

    .controller('CollegeInfoController', function($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'name', name: '校园名称', width: 200, enableColumnResizing: false },
                { field: 'totalStudents', name: '在校学生数' },
                { field: 'graduateStudents', name: '毕业用户数' },
                { field: 'currentSubscribers', name: '当前用户数' },
                { field: 'newSubscribers', name: '新发展用户数' },
                { field: 'expectedSubscribers', name: '预计到达用户数' },
                { field: 'oldOpenDate', name: '老生开学日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'newOpenDate', name: '新生开学日期', cellFilter: 'date: "yyyy-MM-dd"' }
            ],
            data: []
        };
    })
    .directive('collegeInfoList', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CollegeInfoController',
            scope: {
                colleges: '='
            },
            argumentName: 'colleges'
        }, $compile);
    })

    .controller('CollegeFlowController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'name', name: '校园名称', width: 200, enableColumnResizing: false },
                { field: 'totalStudents', name: '在校学生数' },
                { field: 'expectedSubscribers', name: '预计到达用户数' },
                { field: 'oldOpenDate', name: '老生开学日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'newOpenDate', name: '新生开学日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'cellCount', name: '小区数' },
                { field: 'pdcpDownlinkFlow', name: '平均下行流量(MB)', cellFilter: 'number: 1' },
                { field: 'pdcpUplinkFlow', name: '平均上行流量(MB)', cellFilter: 'number: 1' },
                { field: 'averageUsers', name: '平均用户数', cellFilter: 'number: 1' },
                { field: 'maxActiveUsers', name: '最大激活用户数', cellFilter: 'number: 1' }
            ],
            data: []
        };
    })
    .directive('collegeFlowList', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CollegeFlowController',
            scope: {
                colleges: '='
            },
            argumentName: 'colleges'
        }, $compile);
    })

    .controller('CollegeSupportController', function ($scope, emergencyService, customerDialogService, appRegionService) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'name', name: '校园名称', width: 200, enableColumnResizing: false },
                { field: 'currentSubscribers', name: '当前用户数' },
                { field: 'newSubscribers', name: '新发展用户数' },
                { field: 'expectedSubscribers', name: '预计到达用户数' },
                { field: 'oldOpenDate', name: '老生开学日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'newOpenDate', name: '新生开学日期', cellFilter: 'date: "yyyy-MM-dd"' },
                {
                    name: '支撑任务',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.constructSupport(row.entity)">新增/修改</button>'
                }
            ],
            data: []
        };
        $scope.constructSupport = function (college) {
            emergencyService.queryCollegeVipDemand($scope.year, college.name).then(function(item) {
                if (!item) {
                    emergencyService.constructCollegeVipDemand(college).then(function(count) {
                        $scope.messages.push({
                            type: 'success',
                            contents: '生成支撑任务工单：' + college.name
                        });
                        $scope.query();
                    });
                } else {
                    if (item.district) {
                        angular.forEach($scope.district.options, function (district) {
                            if (district === item.district) {
                                $scope.district.selected = item.district;
                            }
                        });
                        
                        appRegionService.queryTowns($scope.city.selected, $scope.district.selected).then(function (towns) {
                            $scope.town.options = towns;
                            $scope.town.selected = towns[0];
                            angular.forEach(towns, function(town) {
                                if (town === item.town) {
                                    $scope.town.selected = town;
                                }
                            });
                            customerDialogService.supplementCollegeDemandInfo(item, $scope.messages);
                        });
                    } else {
                        customerDialogService.supplementCollegeDemandInfo(item, $scope.messages);
                    }
                }
                
            });
        };
    })
    .directive('collegeSupportList', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CollegeSupportController',
            scope: {
                colleges: '=',
                year: '=',
                messages: '=',
                query: '&',
                city: '=',
                district: '=',
                town: '='
            },
            argumentName: 'colleges'
        }, $compile); 
    })

    .controller('CollegeStatController', function ($scope, collegeDialogService) {
        $scope.showENodebs = function(name) {
            collegeDialogService.showENodebs(name);
        };

        $scope.showCells = function(name) {
            collegeDialogService.showCells(name);
        };

        $scope.showBtss = function(name) {
            collegeDialogService.showBtss(name);
        };

        $scope.showCdmaCells = function(name) {
            collegeDialogService.showCdmaCells(name);
        };

        $scope.showLteDistributions = function(name) {
            collegeDialogService.showLteDistributions(name);
        };

        $scope.showCdmaDistributions = function(name) {
            collegeDialogService.showCdmaDistributions(name);
        };
        $scope.showDetails = function(name) {
            collegeDialogService.showCollegeDetails(name);
        };

        $scope.gridOptions = {
            columnDefs: [
                { field: 'name', name: '校园名称', width: 170, enableColumnResizing: false },
                { field: 'expectedSubscribers', name: '用户数', width: 40 },
                { field: 'area', name: '区域面积（平方米）', cellFilter: 'number: 2', width: 90, enableColumnResizing: false },
                {
                    name: '4G基站数',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.showENodebs(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalLteENodebs}}</span></button>'
                },
                {
                    name: '4G小区数',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.showCells(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalLteCells}}</span></button>'
                },
                {
                    name: '3G基站数',
                    cellTemplate: '<button class="btn btn-sm btn-default" ng-click="grid.appScope.showBtss(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalCdmaBts}}</span></button>'
                },
                {
                    name: '3G小区数',
                    cellTemplate: '<button class="btn btn-sm btn-default" ng-click="grid.appScope.showCdmaCells(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalCdmaCells}}</span></button>'
                },
                {
                    name: '4G室分数',
                    cellTemplate: '<button class="btn btn-sm btn-default" ng-click="grid.appScope.showLteDistributions(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalLteIndoors}}</span></button>'
                },
                {
                    name: '3G室分数',
                    cellTemplate: '<button class="btn btn-sm btn-default" ng-click="grid.appScope.showCdmaDistributions(row.entity.name)">' +
                        '详情<span class="badge pull-right">{{row.entity.totalCdmaIndoors}}</span></button>'
                },
                {
                    name: '详细信息',
                    cellTemplate: '<button class="btn btn-sm btn-success" ng-click="grid.appScope.showDetails(row.entity.name)">详细</button>'
                }
            ],
            data: []
        };
    })
    .directive('collegeStatTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CollegeStatController',
            scope: {
                collegeList: '='
            },
            argumentName: 'collegeList'
        }, $compile);
    })
    .controller('GridClusterController', function ($scope, alarmsService) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'clusterNumber', name: '簇编号' },
                { field: 'longtitute', name: '经度' },
                { field: 'lattitute', name: '纬度' },
                { field: 'bestLongtitute', name: '备选点经度' },
                { field: 'bestLattitute', name: '备选点纬度' },
                { field: 'rsrp', name: '平均RSRP' },
                { field: 'weakRate', name: '弱覆盖率' },
                { field: 'gridPoints.length', name: '栅格个数' },
                {
                    name: '详细信息',
                    cellTemplate: '<button class="btn btn-sm btn-success" ng-click="grid.appScope.showDetails(row.entity)">详细</button>'
                }
            ],
            data: []
        };
        $scope.showDetails = function(item) {
            alarmsService.queryClusterGridKpis(item.gridPoints).then(function(list) {
                $scope.currentCluster.list = list;
                $scope.currentCluster.stat = item;
            });
        };
    })
    .directive('gridClusterTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'GridClusterController',
            scope: {
                clusterList: '=',
                currentCluster: '='
            },
            argumentName: 'clusterList'
        }, $compile);
    })
    .controller('GridInClusterController', function($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'longtitute', name: '经度' },
                { field: 'lattitute', name: '纬度' },
                { field: 'rsrp', name: '平均RSRP' },
                { field: 'rsrpNormalize', name: '平均RSRP归一化' },
                { field: 'mrCount', name: 'MR数量' },
                { field: 'mrCountNormalize', name: 'MR数量归一化' },
                { field: 'weakCount', name: '弱覆盖' },
                { field: 'weakCountNormalize', name: '弱覆盖归一化' },
                { field: 'weakCoverageRate', name: '弱覆盖比例（%）' },
                { field: 'shortestDistance', name: '最近基站距离' }
            ],
            data: []
        };
    })
    .directive('gridInClusterTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'GridInClusterController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })
    .controller('GridDpiController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'longtitute', name: '经度' },
                { field: 'lattitute', name: '纬度' },
                { field: 'rsrp', name: '平均RSRP' },
                { field: 'rsrpNormalize', name: '平均RSRP归一化' },
                { field: 'mrCount', name: 'MR数量' },
                { field: 'weakCount', name: '弱覆盖' },
                { field: 'weakCoverageRate', name: '弱覆盖比例（%）' },
                { field: 'firstPacketDelay', name: '首包时延（毫秒）' },
                { field: 'pageOpenDelay', name: '页面打开时延（毫秒）' },
                { field: 'shortestDistance', name: '最近基站距离' }
            ],
            data: []
        };
    })
    .directive('gridDpiTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'GridDpiController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    });