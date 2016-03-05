﻿angular.module('myApp.parameters', ['myApp.url'])
    .factory('neighborService', function($q, $http, appUrlService) {
        return {
            queryCellNeighbors: function(cell) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('NearestPciCell'),
                    params: {
                        'cellId': cell.cellId,
                        'sectorId': cell.sectorId
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            updateCellPci: function (cell) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NearestPciCell'), cell)
                    .success(function(result) {
                        deferred.resolve(result);
                    })
                    .error(function(reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            monitorNeighbors: function(cell) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NeighborMonitor'), {
                        cellId: cell.nearestCellId,
                        sectorId: cell.nearestSectorId
                    })
                    .success(function(result) {
                        deferred.resolve(result);
                    })
                    .error(function(reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            queryNearestCells: function(eNodebId, sectorId, pci) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('Cell'),
                    params: {
                        'eNodebId': eNodebId,
                        'sectorId': sectorId,
                        'pci': pci
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            updateNeighbors: function(cellId, sectorId, pci, nearestCellId, nearestSectorId) {
                var deferred = $q.defer();
                $http.put(appUrlService.getApiUrl('NearestPciCell'), {
                    cellId: cellId,
                    sectorId: sectorId,
                    pci: pci,
                    nearestCellId: nearestCellId,
                    nearestSectorId: nearestSectorId
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            }
        };
    })
    .factory('networkElementService', function($q, $http, appUrlService) {
        return {
            queryCellInfo: function(cellId, sectorId) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('Cell'),
                    params: {
                        eNodebId: cellId,
                        sectorId: sectorId
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryCdmaCellInfo: function (btsId, sectorId) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('CdmaCell'),
                    params: {
                        btsId: btsId,
                        sectorId: sectorId
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryENodebInfo: function (eNodebId) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('ENodeb'),
                    params: {
                        eNodebId: eNodebId
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryBtsInfo: function (btsId) {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('Bts'),
                    params: {
                        btsId: btsId
                    }
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryCellSectors: function (cells) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('Cell'), {
                    views: cells
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryRangeSectors: function(range, excludedIds) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('SectorView'), {
                    west: range.west,
                    east: range.east,
                    south: range.south,
                    north: range.north,
                    excludedCells: excludedIds
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            }
        };
    })
    .factory('basicImportService', function($q, $http, appUrlService) {
        return {
            queryENodebExcels: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('NewENodebExcels'),
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryCellExcels: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('NewCellExcels'),
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryBtsExcels: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('NewBtsExcels'),
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryCdmaCellExcels: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('NewCdmaCellExcels'),
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryVanishedENodebs: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('DumpENodebExcel'),
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            queryVanishedCells: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: appUrlService.getApiUrl('DumpCellExcel')
                }).success(function (result) {
                    deferred.resolve(result);
                })
                .error(function (reason) {
                    deferred.reject(reason);
                });
                return deferred.promise;
            },
            dumpOneENodebExcel: function(item) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('DumpENodebExcel'), item)
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpOneBtsExcel: function (item) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('DumpBtsExcel'), item)
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpOneCellExcel: function (item) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('DumpCellExcel'), item)
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpOneCdmaCellExcel: function (item) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('DumpCdmaCellExcel'), item)
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpMultipleENodebExcels: function(items) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NewENodebExcels'), {
                        infos: items
                    })
                    .success(function(result) {
                        deferred.resolve(result);
                    })
                    .error(function(reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpMultipleBtsExcels: function (items) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NewBtsExcels'), {
                    infos: items
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpMultipleCellExcels: function (items) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NewCellExcels'), {
                    infos: items
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            dumpMultipleCdmaCellExcels: function (items) {
                var deferred = $q.defer();
                $http.post(appUrlService.getApiUrl('NewCdmaCellExcels'), {
                    infos: items
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            vanishENodebIds: function(ids) {
                var deferred = $q.defer();
                $http.put(appUrlService.getApiUrl('DumpENodebExcel'), {
                    eNodebIds: ids
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            },
            vanishCellIds: function (ids) {
                var deferred = $q.defer();
                $http.put(appUrlService.getApiUrl('DumpCellExcel'), {
                    cellIdPairs: ids
                })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (reason) {
                        deferred.reject(reason);
                    });
                return deferred.promise;
            }
        };
    });