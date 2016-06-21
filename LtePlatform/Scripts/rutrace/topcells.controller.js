﻿app.controller("rutrace.top", function ($scope, $http, preciseInterferenceService, kpiPreciseService, workitemService) {
    $scope.page.title = "TOP指标分析";
    $scope.topCells = [];
    $scope.orderPolicy = {
        options: [],
        selected: ""
    };
    $scope.topCount = {
        options: [5, 10, 15, 20, 30],
        selected: 15
    };
    $scope.updateMessages = [];

    $scope.query = function () {
        $scope.topCells = [];
        kpiPreciseService.queryTopKpis($scope.beginDate.value, $scope.endDate.value, $scope.topCount.selected,
            $scope.orderPolicy.selected).then(function (result) {
                $scope.topCells = result;
            angular.forEach(result, function(cell) {
                workitemService.queryByCellId(cell.cellId, cell.sectorId).then(function(items) {
                    if (items.length > 0) {
                        for (var j = 0; j < $scope.topCells.length; j++) {
                            if (items[0].eNodebId === $scope.topCells[j].cellId && items[0].sectorId === $scope.topCells[j].sectorId) {
                                $scope.topCells[j].hasWorkItems = true;
                                break;
                            }
                        }
                    }
                });
                preciseInterferenceService.queryMonitor(cell.cellId, cell.sectorId).then(function (monitored) {
                    cell.isMonitored = monitored;
                });
            });
        });
    };
    $scope.monitorAll = function () {
        angular.forEach($scope.topCells, function(cell) {
            if (cell.isMonitored === false) {
                preciseInterferenceService.addMonitor(cell);
            }
        });
    };
    
    $scope.closeAlert = function (index) {
        $scope.updateMessages.splice(index, 1);
    };


    kpiPreciseService.getOrderSelection().then(function (result) {
        $scope.orderPolicy.options = result;
        $scope.orderPolicy.selected = result[0];
        $scope.query();
    });
});