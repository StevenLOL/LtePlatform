﻿angular.module('app.common', [
        'app.filters',
        'app.module',
        'baidu.map',
        'cgBusy',
        'college.module',
        'customer.module',
        "highcharts-ng",
        'myApp.kpi',
        'myApp.region',
        'myApp.url',
        'ngAnimate',
        'ngRoute',
        'ngTouch',
        'parameters.module',
        'rutrace.module',
        "ui.bootstrap",
        'ui.grid',
        'ui.grid.edit',
        'ui.grid.exporter',
        'ui.grid.pagination',
        'ui.grid.selection',
        'ui.router',
        'workitem.module'
    ])
    .run(function($rootScope) {
        $rootScope.sideBarShown = true;
    });