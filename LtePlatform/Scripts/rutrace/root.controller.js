﻿
app.controller("rutrace.root", function ($scope, appRegionService) {
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
        mrStats: [],
        preciseStats: [],
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
        updateVictimProgress: {}
    };
    $scope.updateTopCells = function(cell) {
        var cellName = cell.eNodebName + "-" + cell.sectorId;
        if ($scope.topStat.cells[cellName] === undefined) {
            $scope.topStat.cells[cellName] = cell;
        }
    };

    appRegionService.initializeCities().then(function(result) {
        $scope.overallStat.city = result.selected;
        appRegionService.queryDistricts(result.selected).then(function (districts) {
            for (var i = 0; i < districts.length; i++) {
                $scope.menuItems.push({
                    displayName: "TOP指标分析-" + districts[i],
                    url: "/Rutrace#/topDistrict/" + districts[i]
                });
            }
        });
    });
});
