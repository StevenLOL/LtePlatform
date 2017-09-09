﻿angular.module('topic.dialog.parameters', ['myApp.url', 'myApp.region', 'myApp.kpi', 'topic.basic', "ui.bootstrap"])
    .controller('town.eNodeb.dialog',
        function($scope,
            $uibModalInstance,
            dialogTitle,
            city,
            district,
            town,
            networkElementService,
            appRegionService,
            parametersChartService) {
            $scope.dialogTitle = dialogTitle;
            networkElementService.queryENodebsInOneTown(city, district, town).then(function(eNodebs) {
                $scope.eNodebList = eNodebs;
            });
            networkElementService.queryBtssInOneTown(city, district, town).then(function(btss) {
                $scope.btsList = btss;
            });
            appRegionService.queryTownLteCount(city, district, town, true).then(function(stat) {
                $("#indoorChart").highcharts(parametersChartService
                    .getLteCellCountOptions(city + district + town + '室内', stat));
            });
            appRegionService.queryTownLteCount(city, district, town, false).then(function(stat) {
                $("#outdoorChart").highcharts(parametersChartService
                    .getLteCellCountOptions(city + district + town + '室外', stat));
            });

            $scope.ok = function() {
                $uibModalInstance.close($scope.eNodeb);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('hot.spot.info.dialog',
        function($scope, $uibModalInstance, dialogTitle, hotSpotList) {
            $scope.dialogTitle = dialogTitle;
            $scope.hotSpotList = hotSpotList;

            $scope.ok = function() {
                $uibModalInstance.close($scope.neighbor);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };

        })
    .controller('map.sectors.dialog',
        function($scope, $uibModalInstance, sectors, dialogTitle, networkElementService) {
            $scope.sectors = sectors;
            $scope.dialogTitle = dialogTitle;
            if (sectors.length > 0) {
                networkElementService.queryCellInfo(sectors[0].eNodebId, sectors[0].sectorId).then(function(result) {
                    $scope.currentCell = result;
                });
            }
            $scope.ok = function() {
                $uibModalInstance.close($scope.sectors);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };

        })
    .controller('map.site.dialog',
        function($scope, $uibModalInstance, site, dialogTitle, appFormatService, networkElementService) {
            $scope.itemGroups = appFormatService.generateSiteGroups(site);
            $scope.detailsGroups = appFormatService.generateSiteDetailsGroups(site);
            $scope.cellGroups = [];
            networkElementService.queryENodebByPlanNum(site.planNum).then(function(eNodeb) {
                if (eNodeb) {
                    $scope.eNodebGroups = appFormatService.generateENodebGroups(eNodeb);
                    $scope.eNodeb = eNodeb;
                } else {
                    networkElementService.queryLteRrusFromPlanNum(site.planNum).then(function(cells) {
                        angular.forEach(cells,
                            function(cell) {
                                $scope.cellGroups.push({
                                    cellName: cell.cellName,
                                    cellGroup: appFormatService.generateCellGroups(cell),
                                    rruGroup: appFormatService.generateRruGroups(cell)
                                });
                            });
                        if (cells.length) {
                            networkElementService.queryENodebInfo(cells[0].eNodebId).then(function(item) {
                                if (item) {
                                    $scope.eNodebGroups = appFormatService.generateENodebGroups(item);
                                    $scope.eNodeb = item;
                                    networkElementService.queryCellViewsInOneENodeb(item.eNodebId)
                                        .then(function(cellList) {
                                            $scope.cellList = cellList;
                                        });
                                }
                            });
                        }
                    });
                }
            });
            $scope.dialogTitle = dialogTitle;
            $scope.ok = function() {
                $uibModalInstance.close($scope.site);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        })
    .controller('map.building.dialog',
        function($scope, $uibModalInstance, building, dialogTitle) {
            $scope.building = building;
            $scope.dialogTitle = dialogTitle;

            $scope.ok = function() {
                $uibModalInstance.close($scope.building);
            };

            $scope.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            };
        });