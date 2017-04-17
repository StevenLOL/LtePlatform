﻿angular.module('parameters.module', ['ui.grid', 'myApp.region', 'myApp.url', 'myApp.kpi'])
    .constant('parametersRoot', '/directives/parameters/')

    .directive('cityInfrastructure', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                districtStats: '=',
                currentDistrict: '='
            },
            templateUrl: parametersRoot + 'CityInfrastructure.Tpl.html',
            link: function(scope, element, attrs) {
                scope.showDistrictDetails = function(district) {
                    scope.currentDistrict = district;
                };
                scope.$watch('districtStats', function(stats) {
                    if (stats === undefined) return;
                    scope.showDistrictDetails(stats[0].district);
                });
            }
        };
    })
    .directive('districtInfrastructure', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                townStats: '=',
                rootPath: '=',
                city: '=',
                currentDistrict: '='
            },
            templateUrl: parametersRoot + 'DistrictInfrastructure.Tpl.html'
        }
    })

    .directive('alarmTable', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                alarms: '='
            },
            templateUrl: parametersRoot + 'kpi/AlarmTable.html'
        }
    })
    .directive('flowTable', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                flowList: '='
            },
            templateUrl: parametersRoot + 'kpi/FlowTable.html'
        }
    })

    .controller('LteENodebController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'eNodebId', name: 'LTE基站编号' },
                { field: 'name', name: '基站名称', width: 150 },
                { field: 'planNum', name: '规划编号' },
                { field: 'openDate', name: '入网日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'address', name: '地址', width: 300, enableColumnResizing: false },
                { field: 'factory', name: '厂家' },
                { field: 'isInUse', name: '是否在用', cellFilter: 'yesNoChinese' }
            ],
            data: []
        };
    })
    .directive('eNodebTable', function ($compile) {
        return {
            controller: 'LteENodebController',
            restrict: 'EA',
            replace: true,
            scope: {
                items: '='
            },
            template: '<div></div>',
            link: function (scope, element, attrs) {
                scope.initialize = false;
                scope.$watch('items', function (items) {
                    scope.gridOptions.data = items;
                    if (!scope.initialize) {
                        var linkDom = $compile('<div ui-grid="gridOptions"></div>')(scope);
                        element.append(linkDom);
                        scope.initialize = true;
                    }
                });
            }
        };
    })

    .controller('PlanningSiteController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'district', name: '区县' },
                { field: 'town', name: '镇街' },
                { field: 'formalName', name: '正式名称' },
                { field: 'contractDate', name: '合同日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'finishedDate', name: '完工日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'planNum', name: '规划编号' },
                { field: 'antennaHeight', name: '天线挂高（米）' },
                { field: 'towerType', name: '杆塔类型' }
            ],
            data: []
        };
    })
    .directive('planningSiteTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'PlanningSiteController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('TopDropController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'cellId', name: '小区编号' },
                { field: 'cdmaName', name: 'CDMA基站名称' },
                { field: 'lteName', name: 'LTE基站名称' },
                { field: 'sectorId', name: '扇区编号' },
                { field: 'frequency', name: '频点' },
                { field: 'drops', name: '掉话次数' },
                { field: 'moAssignmentSuccess', name: '主叫次数' },
                { field: 'mtAssignmentSuccess', name: '被叫次数' },
                {
                    name: '掉话率(%)',
                    cellTemplate: '<span class="text-primary">{{row.entity.dropRate * 100 | number: 2}}</span>'
                },
                {
                    name: '查看信息',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="showHistory(row.entity)"> \
                            <span class="glyphicon glyphicon-search"></span>历史信息 \
                        </button>'
                }
            ],
            data: []
        };
    })
    .directive('topDropTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'TopDropController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('TopDropTrendController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'cellName', name: '小区名称' },
                {
                    name: 'LTE小区名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
                },
                { field: 'totalDrops', name: '掉话总数' },
                { field: 'moAssignmentSuccess', name: '主叫次数' },
                {
                    field: 'mtAssignmentSuccess',
                    name: '被叫次数'
                },
                { field: 'topDates', name: 'TOP天数' },
                {
                    name: '掉话率（%）',
                    cellTemplate: '<span class="text-primary">{{row.entity.dropRate * 100 | number: 2}}</span>'
                },
                {
                    name: '查看信息',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="showHistory(row.entity)"> \
                            <span class="glyphicon glyphicon-search"></span>历史信息 \
                        </button>'
                }
            ],
            data: []
        };
    })
    .directive('topDropTrendTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'TopDropTrendController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('TopConnectionController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'cellId', name: '小区编号' },
                { field: 'cdmaName', name: 'CDMA基站名称' },
                { field: 'lteName', name: 'LTE基站名称' },
                { field: 'sectorId', name: '扇区编号' },
                { field: 'wirelessDrop', name: '无线掉线次数' },
                { field: 'connectionAttempts', name: '连接尝试次数' },
                { field: 'connectionFails', name: '连接失败次数' },
                {
                    field: 'linkBusyRate',
                    name: '链路繁忙率（%）',
                    cellFilter: 'number: 2'
                },
                {
                    name: '连接成功率（%）',
                    cellTemplate: '<span class="text-primary">{{row.entity.connectionRate * 100 | number: 2}}</span>'
                },
                {
                    name: '掉线率（%）',
                    cellTemplate: '<span class="text-primary">{{row.entity.dropRate * 100 | number: 2}}</span>'
                },
                {
                    name: '查看信息',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="showHistory(row.entity)"> \
                            <span class="glyphicon glyphicon-search"></span>历史信息 \
                        </button>'
                }
            ],
            data: []
        };
    })
    .directive('topConnectionTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'TopConnectionController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('TopConnectionTrendController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'cellName', name: '小区名称' },
                {
                    name: 'LTE小区名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
                },
                {
                    field: 'wirelessDrop',
                    name: '无线掉线次数'
                },
                { field: 'connectionAttempts', name: '连接尝试次数' },
                { field: 'connectionFails', name: '连接失败次数' },
                { field: 'topDates', name: 'TOP天数' },
                {
                    field: 'linkBusyRate',
                    name: '链路繁忙率(%)',
                    cellFilter: 'number: 2'
                },
                {
                    name: '连接成功率(%)',
                    cellTemplate: '<span class="text-primary">{{row.entity.connectionRate * 100 | number: 2}}</span>'
                },
                {
                    name: '掉线率(%)',
                    cellTemplate: '<span class="text-primary">{{row.entity.dropRate * 100 | number: 2}}</span>'
                },
                {
                    name: '查看信息',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="showHistory(row.entity)"> \
                            <span class="glyphicon glyphicon-search"></span>历史信息 \
                        </button>'
                }
            ],
            data: []
        };
    })
    .directive('topConnectionTrendTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'TopConnectionTrendController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('ENodebPlainController', function ($scope, workItemDialog) {
        $scope.gridOptions = {
            paginationPageSizes: [20, 40, 60],
            paginationPageSize: 20,
            columnDefs: [
                { field: 'eNodebId', name: 'LTE基站编号' },
                { field: 'name', name: '基站名称', width: 120 },
                { field: 'planNum', name: '规划编号' },
                { field: 'openDate', name: '入网日期', cellFilter: 'date: "yyyy-MM-dd"' },
                { field: 'address', name: '地址', width: 300, enableColumnResizing: false },
                { field: 'factory', name: '厂家' },
                {
                    name: 'IP',
                    cellTemplate: '<span class="text-primary">{{row.entity.ip.addressString}}</span>',
                    width: 100
                },
                {
                    name: '是否在用',
                    cellTemplate: '<span ng-class="{\'btn-default\': !row.entity.isInUse, \'btn-success\': row.entity.isInUse}">\
                        {{row.entity.isInUse}}</span>'
                },
                {
                    name: '查询',
                    cellTemplate: '<a ng-click="grid.appScope.showFlow(row.entity)" class="btn btn-sm btn-default">流量查询</a>'
                }
            ],
            data: []
        };
        $scope.showFlow = function(eNodeb) {
            workItemDialog.showENodebFlow(eNodeb, $scope.beginDate, $scope.endDate);
        };
    })
    .directive('eNodebPlainTable', function ($compile, calculateService) {
        return calculateService.generatePagingGridDirective({
            controllerName: 'ENodebPlainController',
            scope: {
                items: '=',
                beginDate: '=',
                endDate: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .directive('eNodebDetailsTable', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                eNodebDetails: '='
            },
            templateUrl: parametersRoot + 'eNodeb/ENodebDetailsTable.html'
        }
    })

    .controller('CdmaBtsController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                { field: 'btsId', name: 'CDMA基站编号' },
                { field: 'name', name: '基站名称', width: 150 },
                { field: 'bscId', name: 'BSC编号' },
                { field: 'districtName', name: '区域' },
                { field: 'townName', name: '镇区' },
                {
                     field: 'address', 
                     name: '地址',
                     width: 300,
                     enableColumnResizing: false,
                     cellTooltip: function (row) {
                         return row.entity.address;
                     }
                },
                { field: 'isInUse', name: '是否在用', cellFilter: 'yesNoChinese' }
            ],
            data: []
        };
    })
    .directive('btsTable', function ($compile) {
        return {
            controller: 'CdmaBtsController',
            restrict: 'EA',
            replace: true,
            scope: {
                items: '='
            },
            template: '<div></div>',
            link: function (scope, element, attrs) {
                scope.initialize = false;
                scope.$watch('items', function (items) {
                    scope.gridOptions.data = items;
                    if (!scope.initialize) {
                        var linkDom = $compile('<div ui-grid="gridOptions"></div>')(scope);
                        element.append(linkDom);
                        scope.initialize = true;
                    }
                });
            }
        };
    })
    .controller('BtsPlainController', function ($scope) {
        $scope.gridOptions = {
            paginationPageSizes: [20, 40, 60],
            paginationPageSize: 20,
            columnDefs: [
                { field: 'btsId', name: 'CDMA基站编号' },
                { field: 'name', name: '基站名称', width: 120 },
                { field: 'btsId', name: 'BSC编号' },
                { field: 'longtitute', name: '经度' },
                { field: 'lattitute', name: '纬度' },
                { field: 'address', name: '地址', width: 300, enableColumnResizing: false },
                { field: 'isInUse', name: '是否在用', cellFilter: 'yesNoChinese' }
            ],
            data: []
        };
    })
    .directive('btsPlainTable', function ($compile) {
        return {
            controller: 'BtsPlainController',
            restrict: 'EA',
            replace: true,
            scope: {
                items: '='
            },
            template: '<div></div>',
            link: function (scope, element, attrs) {
                scope.initialize = false;
                scope.$watch('items', function (items) {
                    scope.gridOptions.data = items;
                    if (!scope.initialize) {
                        var linkDom = $compile('<div ui-grid="gridOptions" ui-grid-pagination style="height: 600px"></div>')(scope);
                        element.append(linkDom);
                        scope.initialize = true;
                    }
                });
            }
        };
    })

    .controller('CellDialogController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                {
                     name: '小区名称',
                     cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
                },
                { field: 'frequency', name: '频点' },
                { field: 'pci', name: 'PCI' },
                { field: 'prach', name: 'PRACH' },
                { field: 'rsPower', name: 'RS功率' },
                { field: 'indoor', name: '室内外' },
                { field: 'height', name: '高度' },
                { field: 'azimuth', name: '方位角' },
                { field: 'downTilt', name: '下倾' },
                { field: 'antennaGain', name: '天线增益(dB)' }
            ],
            data: []
        };
    })
    .directive('cellDialogTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CellDialogController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);;
    })

    .controller('CellRruController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                {
                    name: '小区名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
                },
                { field: 'frequency', name: '频点' },
                {
                    name: 'RRU名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.rruName}}</span>'
                },
                { field: 'eNodebId', name: '基站编号' },
                { field: 'antennaFactoryDescription', name: '天线厂家' },
                { field: 'indoor', name: '室内外' },
                { field: 'antennaInfo', name: '天线信息' },
                { field: 'antennaModel', name: '天线型号' },
                { field: 'downTilt', name: '下倾' },
                { field: 'antennaGain', name: '天线增益(dB)' }
            ],
            data: []
        };
    })
    .directive('cellRruTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'CellRruController',
            scope: {
                items: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('LteCellController', function ($scope, networkElementService, neighborDialogService) {
        $scope.gridOptions = {
            columnDefs: [
                {
                    name: '小区名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
                },
                {
                    field: 'frequency',
                    name: '频点',
                    cellTooltip: function(row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'indoor',
                    name: '室内外',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'height',
                    name: '高度',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'azimuth',
                    name: '方位角',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'downTilt',
                    name: '下倾',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'antennaGain',
                    name: '天线增益(dB)',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    },
                    headerTooltip: function(col) {
                        return col.displayName;
                    }
                },
                {
                    field: 'rruName',
                    name: 'RRU名称',
                    cellTooltip: function (row, col) {
                        return '天线厂家：' + row.entity.antennaFactoryDescription + '；天线类型：'
                            + row.entity.antennaInfo + '；天线型号：' + row.entity.antennaModel;
                    }
                },
                {
                    name: '信息',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.showDetails(row.entity)">显示</button>'
                },
                {
                    name: 'MR',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.dumpMr(row.entity)">导入</button>'
                },
                {
                    name: '指标情况',
                    cellTemplate: '<div class="btn-group-sm" uib-dropdown dropdown-append-to-body> \
                        <button id="single-button" type="button" class="btn btn-sm btn-default" \
                            uib-dropdown-toggle ng-disabled="disabled"> \
                            查询 <span class="caret"></span> \
                        </button> \
                        <ul uib-dropdown-menu role="menu" aria-labelledby="single-button"> \
                            <li role="menuitem"> \
                                <a class="btn btn-warning btn-sm" ng-click="grid.appScope.showInterference(row.entity)"> \
                                    MR干扰 \
                                </a> \
                            </li> \
                            <li role="menuitem"> \
                                <a class="btn btn-success btn-sm" ng-click="grid.appScope.showCoverage(row.entity)"> \
                                    MR覆盖 \
                                </a> \
                            </li> \
                        </ul> \
                    </div>'
                }
            ],
            data: []
        };
        $scope.dumpMr = function (cell) {
            networkElementService.queryCellInfo(cell.eNodebId, cell.sectorId).then(function (info) {
                neighborDialogService.dumpMongo({
                    eNodebId: cell.eNodebId,
                    sectorId: cell.sectorId,
                    pci: info.pci,
                    name: cell.eNodebName
                }, $scope.beginDate.value, $scope.endDate.value);
            });
        };
        $scope.showDetails = function(cell) {
            neighborDialogService.showCell(cell);
        };
        $scope.showInterference = function (cell) {
            neighborDialogService.showInterference({
                cellId: cell.eNodebId,
                sectorId: cell.sectorId,
                name: cell.eNodebName
            }, $scope.beginDate.value, $scope.endDate.value);
        };
        $scope.showCoverage = function (cell) {
            neighborDialogService.showCoverage({
                cellId: cell.eNodebId,
                sectorId: cell.sectorId,
                name: cell.eNodebName
            }, $scope.beginDate.value, $scope.endDate.value);
        };
    })
    .directive('lteCellTable', function ($compile, calculateService) {
        return calculateService.generateGridDirective({
            controllerName: 'LteCellController',
            scope: {
                items: '=',
                beginDate: '=',
                endDate: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('CellSelectionController', function ($scope) {
        $scope.gridOptions = {
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: true
        };
        $scope.gridOptions.multiSelect = true;
        $scope.gridOptions.columnDefs = [
            {
                name: '小区名称',
                cellTemplate: '<span class="text-primary">{{row.entity.eNodebName}}-{{row.entity.sectorId}}</span>'
            },
            { field: 'frequency', name: '频点' },
            { field: 'pci', name: 'PCI' },
            { field: 'tac', name: 'TAC' },
            { field: 'prach', name: 'PRACH' },
            { field: 'rsPower', name: 'RS功率' },
            { field: 'indoor', name: '室内外' },
            { field: 'height', name: '高度' },
            { field: 'azimuth', name: '方位角' },
            { field: 'downTilt', name: '下倾' },
            { field: 'antennaGain', name: '天线增益(dB)' }
        ];
        $scope.gridOptions.data = [];
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
    })
    .directive('cellSelectionTable', function ($compile, calculateService) {
        return calculateService.generateSelectionGridDirective({
            controllerName: 'CellSelectionController',
            scope: {
                items: '=',
                gridApi: '='
            },
            argumentName: 'items'
        }, $compile); 
    })

    .controller('CellRruSelectionController', function ($scope) {
        $scope.gridOptions = {
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: true
        };
        $scope.gridOptions.multiSelect = true;
        $scope.gridOptions.columnDefs = [
            {
                name: '小区名称',
                cellTemplate: '<span class="text-primary">{{row.entity.cellName}}</span>'
            },
          { name: '方位角', field: 'azimuth' },
          { name: '下倾角', field: 'downTilt' },
          { name: '频点', field: 'frequency' },
          { name: '天线挂高', field: 'height' },
          { name: '室内外', field: 'indoor' },
            {
                name: 'RRU名称',
                cellTemplate: '<span class="text-primary">{{row.entity.rruName}}</span>'
            },
          { name: '与中心点距离', field: 'distance' }
        ];

        $scope.gridOptions.data = [];
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
    })
    .directive('cellRruSelectionTable', function ($compile, calculateService) {
        return calculateService.generateSelectionGridDirective({
            controllerName: 'CellRruSelectionController',
            scope: {
                items: '=',
                gridApi: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .controller('CdmaCellController', function ($scope) {
        $scope.gridOptions = {
            columnDefs: [
                {
                    name: '小区名称',
                    cellTemplate: '<span class="text-primary">{{row.entity.btsName}}-{{row.entity.sectorId}}</span>'
                },
                { field: 'frequency', name: '频点' },
                { field: 'cellType', name: '小区类别' },
                { field: 'frequencyList', name: '频点列表' },
                { field: 'cellId', name: '小区编号' },
                { field: 'lac', name: 'LAC' },
                { field: 'pn', name: 'PN' },
                { field: 'height', name: '高度' },
                { field: 'azimuth', name: '方位角' },
                { field: 'downTilt', name: '下倾' },
                { field: 'antennaGain', name: '天线增益(dB)' }
            ],
            data: []
        };
    })
    .directive('cdmaCellTable', function ($compile) {
        return {
            restrict: 'EA',
            controller: 'CdmaCellController',
            replace: true,
            scope: {
                items: '='
            },
            template: '<div></div>',
            link: function (scope, element, attrs) {
                scope.initialize = false;
                scope.$watch('items', function (items) {
                    scope.gridOptions.data = items;
                    if (!scope.initialize) {
                        var linkDom = $compile('<div ui-grid="gridOptions"></div>')(scope);
                        element.append(linkDom);
                        scope.initialize = true;
                    }
                });
            }
        }
    })

    .controller('CellDetailsController', function ($scope, networkElementService, calculateService) {
        networkElementService.queryCellInfo($scope.eNodebId, $scope.sectorId).then(function (result) {
            $scope.itemGroups = calculateService.generateCellDetailsGroups(result);
        });
    })
    .directive('cellDetails', function() {
        return {
            controller: 'CellDetailsController',
            restrict: 'EA',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: '/appViews/Home/GeneralTableDetails.html'
        }
    })

    .controller('CellSectorDetailsController', function ($scope, calculateService) {
        $scope.$watch('cell', function(cell) {
            if (cell) {
                $scope.itemGroups = calculateService.generateCellDetailsGroups($scope.cell);
            }
        });

    })
    .directive('cellSectorDetails', function () {
        return {
            controller: 'CellSectorDetailsController',
            restrict: 'EA',
            replace: true,
            scope: {
                cell: '='
            },
            templateUrl: '/appViews/Home/GeneralTableDetails.html'
        }
    })

    .controller('FlowSectorDetailsController', function ($scope, calculateService) {
        $scope.$watch('cell', function (cell) {
            if (cell) {
                $scope.itemGroups = calculateService.generateFlowDetailsGroups($scope.cell);
            }
        });

    })
    .directive('flowSectorDetails', function () {
        return {
            controller: 'FlowSectorDetailsController',
            restrict: 'EA',
            replace: true,
            scope: {
                cell: '='
            },
            templateUrl: '/appViews/Home/GeneralTableDetails.html'
        }
    })

    .controller('CellMongoController', function ($scope, cellHuaweiMongoService, calculateService) {
        cellHuaweiMongoService.queryCellParameters($scope.eNodebId, $scope.sectorId).then(function (info) {
            $scope.itemGroups = calculateService.generateCellMongoGroups(info);
        });
    })
    .directive('cellMongoInfo', function () {
        return {
            controller: 'CellMongoController',
            restrict: 'EA',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: '/appViews/Home/GeneralTableDetails.html'
        }
    })

    .directive('planSiteDetails', function() {
        return {
            replace: true,
            scope: {
                itemGroups: '='
            },
            templateUrl: '/appViews/Home/GeneralTableDetails.html'
        }
    })

    .controller('CellsBasicInfoController', function ($scope, networkElementService) {
        $scope.gridOptions = {
            columnDefs: [
                {
                    name: '小区名称',
                    field: 'cellName'
                },
                {
                    field: 'frequency',
                    name: '频点',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'indoor',
                    name: '室内外',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'height',
                    name: '天线挂高',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'azimuth',
                    name: '方位角',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'downTilt',
                    name: '下倾',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    }
                },
                {
                    field: 'antennaGain',
                    name: '天线增益(dB)',
                    cellTooltip: function (row, col) {
                        return row.entity.otherInfos;
                    },
                    headerTooltip: function (col) {
                        return col.displayName;
                    }
                }, {
                    name: '查询',
                    cellTemplate: '<button class="btn btn-sm btn-primary" ng-click="grid.appScope.showCurrent(row.entity)"> \
                            <span class="glyphicon glyphicon-search"></span>详细信息 \
                        </button>'
                }
            ],
            data: []
        };
        $scope.showCurrent = function(cell) {
            networkElementService.queryCellInfo(cell.eNodebId, cell.sectorId).then(function (result) {
                $scope.currentCell = result;
            });
        };
    })
    .directive('cellsBasicInfoTable', function ($compile, calculateService) {
        return calculateService.generateShortGridDirective({
            controllerName: 'CellsBasicInfoController',
            scope: {
                items: '=',
                currentCell: '='
            },
            argumentName: 'items'
        }, $compile);
    })

    .directive('lteCellBasicInfo', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                neighbor: '='
            },
            templateUrl: parametersRoot + 'cell/BasicInfo.html'
        }
    })

    .directive('eNodebIntraFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                intraFreqHo: '='
            },
            templateUrl: parametersRoot + 'eNodeb/ENodebIntraFreq.html'
        }
    })
    .directive('eNodebInterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'eNodeb/ENodebInterFreq.html'
        }
    })
    .controller('CellIntraFreqController', function($scope, intraFreqHoService) {
        intraFreqHoService.queryCellParameters($scope.eNodebId, $scope.sectorId).then(function(result) {
            $scope.intraFreqHo = result;
        });
    })
    .directive('cellIntraFreq', function(parametersRoot) {
        return {
            restrict: 'EA',
            controller: 'CellIntraFreqController',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: parametersRoot + 'cell/CellIntraFreq.html'
        }
    })
    .directive('cellIntraFreqTable', function (parametersRoot) {
        return {
            restrict: 'EA',
            controller: 'CellIntraFreqController',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: parametersRoot + 'cell/CellIntraFreqTable.html'
        }
    })
    .directive('a1InterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'cell/A1InterFreq.html'
        }
    })
    .directive('a2InterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'cell/A2InterFreq.html'
        }
    })
    .directive('a3InterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'cell/A3InterFreq.html'
        }
    })
    .directive('a4InterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'cell/A4InterFreq.html'
        }
    })
    .directive('a5InterFreq', function(parametersRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                interFreqHo: '='
            },
            templateUrl: parametersRoot + 'cell/A5InterFreq.html'
        }
    })
    .controller('CellInterFreqController', function($scope, interFreqHoService) {
        interFreqHoService.queryCellParameters($scope.eNodebId, $scope.sectorId).then(function(result) {
            $scope.interFreqHo = result;
        });
    })
    .directive('cellInterFreq', function(parametersRoot) {
        return {
            restrict: 'EA',
            controller: 'CellInterFreqController',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: parametersRoot + 'cell/CellInterFreq.html'
        }
    })

    .controller('CellChannelPowerControl', function($scope, cellPowerService) {
        cellPowerService.queryCellParameters($scope.eNodebId, $scope.sectorId).then(function(result) {
            $scope.cellPower = result;
        });
    })
    .directive('cellChannelPower', function(parametersRoot) {
        return {
            restrict: 'EA',
            controller: 'CellChannelPowerControl',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: parametersRoot + 'cell/Power.html'
        }
    })
    .controller('UplinkOpenLoopPowerControl', function($scope, cellPowerService) {
        cellPowerService.queryUlOpenLoopPowerControll($scope.eNodebId, $scope.sectorId).then(function(result) {
            $scope.item = result;
        });
    })
    .directive('uplinkOpenLoopPower', function(parametersRoot) {
        return {
            restrict: 'EA',
            controller: 'UplinkOpenLoopPowerControl',
            replace: true,
            scope: {
                eNodebId: '=',
                sectorId: '='
            },
            templateUrl: parametersRoot + 'cell/UlOpenLoopPowerControl.html'
        }
    });