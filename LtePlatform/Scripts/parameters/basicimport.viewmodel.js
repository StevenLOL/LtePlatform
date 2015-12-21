﻿function BasicImportViewModel(app, dataModel) {
    var self = this;

    self.newENodebs = ko.observableArray([]);
    self.newCells = ko.observableArray([]);
    self.newBtss = ko.observableArray([]);
    self.newCdmaCells = ko.observableArray([]);
    self.newENodebsImport = ko.observable(true);
    self.newCellsImport = ko.observable(true);
    self.newBtssImport = ko.observable(true);
    self.newCdmaCellsImport = ko.observable(true);

    self.newENodebLonLatEdits = ko.observableArray([]);
    self.newCellLonLatEdits = ko.observableArray([]);
    self.newBtsLonLatEdits = ko.observableArray([]);
    self.newCdmaCellLonLatEdits = ko.observableArray([]);
    self.dumpResultMessage = ko.observable("");

    self.editENodeb = ko.observable(null);
    self.editCell = ko.observable(null);
    self.editBts = ko.observable(null);
    self.editCdmaCell = ko.observable(null);

    Sammy(function () {
        this.get('#basicImport', function () {
            $("#open-date").datepicker({ dateFormat: 'yy-mm-dd' });
            sendRequest(app.dataModel.newENodebExcelsUrl, "GET", null, function(data) {
                self.newENodebs(data);
            });
            sendRequest(app.dataModel.newCellExcelsUrl, "GET", null, function (data) {
                self.newCells(data);
            });
            sendRequest(app.dataModel.newBtsExcelsUrl, "GET", null, function (data) {
                self.newBtss(data);
            });
            sendRequest(app.dataModel.newCdmaCellExcelsUrl, "GET", null, function (data) {
                self.newCdmaCells(data);
            });
        });
        this.get('/Parameters/BasicImport', function () { this.app.runRoute('get', '#basicImport'); });
    });

    self.checkENodebsLonLat = function() {
        self.newENodebLonLatEdits(queryENodebLonLatEdits(self.newENodebs()));
        $('#eNodeb-lon-lat').modal('show');
    };

    self.postENodebLonLat = function() {
        mapLonLatEdits(self.newENodebs, self.newENodebLonLatEdits());
        $('#eNodeb-lon-lat').modal('hide');
    };

    self.checkCellsLonLat = function() {
        self.newCellLonLatEdits(queryCellLonLatEdits(self.newCells()));
        $('#cell-lon-lat').modal('show');
    };

    self.postCellLonLat = function() {
        mapLonLatEdits(self.newCells, self.newCellLonLatEdits());
        $('#cell-lon-lat').modal('hide');
    };

    self.checkBtssLonLat = function() {
        self.newBtsLonLatEdits(queryBtsLonLatEdits(self.newBtss()));
        $('#bts-lon-lat').modal('show');
    };

    self.postBtsLonLat = function() {
        mapLonLatEdits(self.newBtss, self.newBtsLonLatEdits());
        $('#bts-lon-lat').modal('hide');
    };

    self.checkCdmaCellsLonLat = function() {
        self.newCdmaCellLonLatEdits(queryCdmaCellLonLatEdits(self.newCdmaCells()));
        $('#cdmaCell-lon-lat').modal('show');
    };

    self.postCdmaCellLonLat = function() {
        mapLonLatEdits(self.newCdmaCells, self.newCdmaCellLonLatEdits());
        $('#cdmaCell-lon-lat').modal('hide');
    };

    self.postAll = function () {
        if (self.newENodebsImport() === true) postAllENodebs(self);
        if (self.newBtssImport() === true) postAllBtss(self);
        if (self.newCellsImport() === true) postAllCells(self);
        if (self.newCdmaCellsImport() === true) postAllCdmaCells(self);
    };

    self.postSingleENodeb = function() {
        if (self.editENodeb() === null && self.newENodebs().length > 0) {
            self.editENodeb(self.newENodebs().pop());
        }
        $("#editENodeb").modal("show");
    };

    self.saveENodeb = function() {
        sendRequest(app.dataModel.dumpENodebExcelUrl, "POST", self.editENodeb(), function(result) {
            if (result === true) {
                self.editENodeb(null);
                self.dumpResultMessage("完成一个LTE基站导入数据库！");
            }
            $("#editENodeb").modal("hide");
        });
    };

    self.postSingleBts = function () {
        if (self.editBts() === null && self.newBtss().length > 0) {
            self.editBts(self.newBtss().pop());
        }
        $("#editBts").modal("show");
    };

    self.saveBts = function () {
        sendRequest(app.dataModel.dumpBtsExcelUrl, "POST", self.editBts(), function (result) {
            if (result === true) {
                self.editBts(null);
                self.dumpResultMessage("完成一个CDMA基站导入数据库！");
            }
            $("#editBts").modal("hide");
        });
    };

    self.postSingleCell=function() {
        if (self.editCell() === null && self.newCells().length > 0) {
            self.editCell(self.newCells().pop());
        }
        $("#editCell").modal("show");
    }

    self.saveCell = function () {
        sendRequest(app.dataModel.dumpCellExcelUrl, "POST", self.editCell(), function (result) {
            if (result === true) {
                self.editCell(null);
                self.dumpResultMessage("完成一个LTE小区导入数据库！");
            }
            $("#editCell").modal("hide");
        });
    };

    self.postSingleCdmaCell = function () {
        if (self.editCdmaCell() === null && self.newCdmaCells().length > 0) {
            self.editCdmaCell(self.newCdmaCells().pop());
        }
        $("#editCdmaCell").modal("show");
    }

    self.saveCdmaCell = function () {
        sendRequest(app.dataModel.dumpCdmaCellExcelUrl, "POST", self.editCdmaCell(), function (result) {
            if (result === true) {
                self.editCdmaCell(null);
                self.dumpResultMessage("完成一个CDMA小区导入数据库！");
            }
            $("#editCdmaCell").modal("hide");
        });
    };

    return self;
}

app.addViewModel({
    name: "BasicImport",
    bindingMemberName: "basicImport",
    factory: BasicImportViewModel
});