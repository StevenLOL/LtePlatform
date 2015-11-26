﻿function TopDrop2GViewModel(app, dataModel) {
    var self = this;

    app.currentCity = ko.observable();
    app.cities = ko.observableArray([]);
    app.statDate = ko.observable((new Date()).getDateFromToday(-1).Format("yyyy-MM-dd"));
    app.oneDayCells = ko.observableArray([]);

    app.initialize = function () {
        $("#StatDate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#BeginDate").datepicker({ dateFormat: 'yy-mm-dd' });
        $("#EndDate").datepicker({ dateFormat: 'yy-mm-dd' });

        initializeCityKpi();
    };

    return self;
}

app.addViewModel({
    name: "TopDrop2G",
    bindingMemberName: "topDrop2G",
    factory: TopDrop2GViewModel
});