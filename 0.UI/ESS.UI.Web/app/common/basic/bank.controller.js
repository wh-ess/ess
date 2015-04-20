"use strict";

angular.module("EssApp").controller("BankController", [
    "$scope", "Bank", "$routeParams", "$timeout",
function ($scope, Bank, $routeParams, $timeout) {
    var fetchBanks = function () {
        $timeout(function() {
            Bank.getList().then(function (data) {
                $scope.banks = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Bank.one($routeParams.id).get().then(function (bank) {
            $scope.current.item = bank;
        });
    } else {
        fetchBanks();
    }

    $scope.addBank = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBank = function () {
        if ($scope.current.item.Id) {
            Bank.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Bank.post($scope.current.item);
            fetchBanks();
        }
    };

}
]);