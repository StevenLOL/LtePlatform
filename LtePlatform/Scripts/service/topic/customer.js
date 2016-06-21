﻿angular.module('customer.service', ['myApp.url'])
    .factory('customerDiloagService', function($uibModal, $log) {
        return {
            constructEmergencyCommunication: function() {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: '/appViews/Customer/Dialog/Emergency.html',
                    controller: 'emergency.new.dialog',
                    size: 'lg',
                    resolve: {
                        dialogTitle: function() {
                            return "新增应急通信需求";
                        }
                    }
                });

                modalInstance.result.then(function(info) {
                    console.log(info);
                }, function() {
                    $log.info('Modal dismissed at: ' + new Date());
                });
            }
        };
    });