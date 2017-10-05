﻿angular.module('workitem.module', ['workitem.module.table', 'workitem.module.feedback', 'workitem.module.details'])
    .constant('workitemRoot', '/directives/workitem/');

angular.module('workitem.module.table', ['myApp.kpi'])
    .controller('WorkItemTableController', function ($scope, workitemRoot, workItemDialog) {
        $scope.gridOptions = {
            paginationPageSizes: [20, 40, 60],
            paginationPageSize: 20,
            columnDefs: [
                { field: 'district', name: '区域' },
                {
                    field: 'serialNumber',
                    name: '工单编号',
                    cellTooltip: function(row) {
                        return '工单编号: ' + row.entity.serialNumber;
                    } 
                },
                {
                    field: 'eNodebName',
                    name: '基站名称',
                    cellTooltip: function (row) {
                        return '基站名称: ' + row.entity.eNodebName;
                    }
                },
                { field: 'workItemType', name: '类型' },
                { field: 'workItemSubType', name: '子类型' },
                { field: 'workItemState', name: '状态' },
                { field: 'deadline', name: '时限', cellFilter: 'date: "yyyy-MM-dd"' },
                {
                    name: '详情',
                    cellTemplate: '<button class="btn btn-xs btn-default" ng-click="grid.appScope.showDetails(row.entity)">详情</button>'
                },
                {
                    name: '详细分析',
                    cellTemplate: '<a ng-href="{{row.entity.detailsPath}}" ng-if="row.entity.detailsPath" class="btn btn-sm btn-success">详细分析</a>'
                },
                {
                    name: '反馈',
                    cellTemplate: '<button class="btn btn-xs btn-info" ng-click="grid.appScope.feedback(row.entity)">反馈</button>'
                }
            ],
            data: []
        };
        $scope.feedback = function(view) {
            workItemDialog.feedback(view, $scope.queryWorkItems);
        };
        $scope.showDetails = function(view) {
            workItemDialog.showDetails(view, $scope.queryWorkItems);
        };
    })
    .directive('workitemTable', function ($compile) {
        return {
            restrict: 'EA',
            replace: true,
            controller: 'WorkItemTableController',
            scope: {
                items: '=',
                queryWorkItems: '&'
            },
            template: '<div></div>',
            link: function(scope, element, attrs) {
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
    });

angular.module('workitem.module.feedback', ['myApp.kpi'])
    .directive('platformAndFeedbackInfo', function(workitemRoot, workItemDialog, workitemService) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                currentView: '=',
                serialNumber: '='
            },
            templateUrl: workitemRoot + 'PlatformAndFeedbackInfo.html',
            link: function(scope, element, attrs) {
                scope.feedback = function() {
                    workItemDialog.feedback(scope.currentView, scope.updateWorkItems);
                };
                scope.$watch('currentView', function(view) {
                    if (view) {
                        scope.platformInfos = workItemDialog.calculatePlatformInfo(view.comments);
                        scope.feedbackInfos = workItemDialog.calculatePlatformInfo(view.feedbackContents);
                    }
                });

                scope.updateWorkItems = function() {
                    workitemService.querySingleItem(scope.serialNumber).then(function(result) {
                        scope.currentView = result;
                    });
                };
            }
        };
    });

angular.module('workitem.module.details', [])
    .directive('workItemDetails', function(workitemRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                currentView: '=',
            },
            templateUrl: workitemRoot + 'WorkItem.Details.html',
            transclude: true
        };
    })
    .directive('preciseWorkItemCells', function(workitemRoot) {
        return {
            restrict: 'ECMA',
            replace: true,
            scope: {
                cells: '=',
            },
            templateUrl: workitemRoot + 'precise/Cell.html',
            transclude: true
        };
    })
    .directive('coverageWorkItemDialogCells', function (workitemRoot) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                cells: '='
            },
            templateUrl: workitemRoot + 'precise/CoverageCell.html',
            transclude: true
        };
    })
    .directive('workItemDetailsTable', function (workitemRoot, workitemService, preciseWorkItemGenerator) {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                currentView: '=',
                gobackPath: '=',
                gobackTitle: '=',
                rootPath: '=',
                preventChangeParentView: '=',
                platformInfos: '=',
                feedbackInfos: '='
            },
            templateUrl: workitemRoot + 'precise/DetailsTable.html',
            link: function(scope, element, attrs) {
                scope.signIn = function () {
                    workitemService.signIn(scope.currentView.serialNumber).then(function (result) {
                        if (result) {
                            scope.currentView = result;
                            scope.feedbackInfos = preciseWorkItemGenerator.calculatePlatformInfo(scope.currentView.feedbackContents);
                        }
                    });
                };
            }
        };
    });