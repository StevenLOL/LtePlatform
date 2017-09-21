﻿angular.module('kpi.work.dialog', ['myApp.url', 'myApp.region', "ui.bootstrap", "kpi.core"])
    .controller('workitem.feedback.dialog',
        function($scope, $uibModalInstance, input, dialogTitle) {
            $scope.item = input;
            $scope.dialogTitle = dialogTitle;
            $scope.message = "";

            $scope.ok = function() {
                $uibModalInstance.close($scope.message);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('workitem.details.dialog',
        function($scope, $uibModalInstance, input, dialogTitle, preciseWorkItemGenerator) {
            $scope.currentView = input;
            $scope.dialogTitle = dialogTitle;
            $scope.message = "";
            $scope.platformInfos = preciseWorkItemGenerator.calculatePlatformInfo($scope.currentView.comments);
            $scope.feedbackInfos = preciseWorkItemGenerator.calculatePlatformInfo($scope.currentView.feedbackContents);
            $scope.preventChangeParentView = true;

            $scope.ok = function() {
                $uibModalInstance.close($scope.message);
            };
            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('map.station.dialog',
        function($scope,
            $uibModalInstance,
            station,
            dialogTitle,
            beginDate,
            endDate,
            appFormatService,
            networkElementService) {
            $scope.beginDate = beginDate;
            $scope.endDate = endDate;
            $scope.itemGroups = appFormatService.generateStationGroups(station);
            $scope.cellList = [];
            networkElementService.queryENodebStationInfo(station.StationId).then(function(eNodeb) {
                if (eNodeb) {
                    $scope.eNodebGroups = appFormatService.generateENodebGroups(eNodeb);
                }

            });
            networkElementService.queryCellStationInfo(station.StationId).then(function(cellList) {
                $scope.cellList = cellList;
            });
            $scope.dialogTitle = dialogTitle;
            $scope.ok = function() {
                $uibModalInstance.close($scope.site);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('map.indoor.dialog',
        function($scope,
            $uibModalInstance,
            station,
            dialogTitle,
            beginDate,
            endDate,
            appFormatService) {
            $scope.beginDate = beginDate;
            $scope.endDate = endDate;
            $scope.itemGroups = appFormatService.generateIndoorGroups(station);

            $scope.dialogTitle = dialogTitle;
            $scope.ok = function() {
                $uibModalInstance.close($scope.site);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('map.distribution.dialog',
        function($scope,
            $uibModalInstance,
            distribution,
            dialogTitle,
            appFormatService,
            networkElementService,
            alarmsService) {
            $scope.distribution = distribution;
            $scope.dialogTitle = dialogTitle;
            $scope.distributionGroups = appFormatService.generateDistributionGroups(distribution);
            $scope.alarmLevel = {
                options: ["严重告警", "重要以上告警", "所有告警"],
                selected: "重要以上告警"
            };
            $scope.alarms = [];
            $scope.searchAlarms = function() {
                alarmsService.queryENodebAlarmsByDateSpanAndLevel(distribution.eNodebId,
                    $scope.beginDate.value,
                    $scope.endDate.value,
                    $scope.alarmLevel.selected).then(function(result) {
                    $scope.alarms = result;
                });
            };
            if (distribution.eNodebId > 0) {
                networkElementService.queryCellInfo(distribution.eNodebId, distribution.lteSectorId)
                    .then(function(cell) {
                        $scope.lteGroups = appFormatService.generateCellGroups(cell);
                    });
                $scope.searchAlarms();
            }
            if (distribution.btsId > 0) {
                networkElementService.queryCdmaCellInfo(distribution.btsId, distribution.cdmaSectorId)
                    .then(function(cell) {
                        $scope.cdmaGroups = appFormatService.generateCdmaCellGroups(cell);
                    });
            }

            $scope.ok = function() {
                $uibModalInstance.close($scope.distributionGroups);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        });