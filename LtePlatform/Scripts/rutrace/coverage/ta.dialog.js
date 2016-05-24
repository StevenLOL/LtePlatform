﻿app.controller('coverage.ta.dialog', function ($scope, $uibModalInstance, dialogTitle, cellId, sectorId, date,
    topPreciseService, cellPreciseService) {
    $scope.dialogTitle = dialogTitle;
    $scope.chartView = {
        options: ['平均RSRP', '覆盖率'],
        selected: '平均RSRP'
    };
    topPreciseService.queryAverageRsrpTaStastic(cellId, sectorId, date).then(function (result) {
        var options = cellPreciseService.getAverageRsrpTaOptions(result, cellId + '-' + sectorId + '平均RSRP统计');
        $("#average-rsrp").highcharts(options);
    });
    topPreciseService.queryAbove110TaRate(cellId, sectorId, date).then(function (above110Stat) {
        topPreciseService.queryAbove105TaRate(cellId, sectorId, date).then(function (above105Stat) {
            var options = cellPreciseService.getAboveRateTaOptions(above110Stat, above105Stat, cellId + '-' + sectorId + '覆盖率统计');
            $("#coverage-rate").highcharts(options);
        });
    });

    $scope.ok = function () {
        $uibModalInstance.close('已处理');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});