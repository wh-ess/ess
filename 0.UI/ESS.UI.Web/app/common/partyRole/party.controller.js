"use strict";

angular.module("EssApp").controller("PartyController", [
    "$scope", "Party", "CategoryTypeScheme", "Category", "$routeParams", "$timeout",
function ($scope, Party, CategoryTypeScheme, Category, $routeParams, $timeout) {

    CategoryTypeScheme.one("PartyRole").one("CategoryType", "Party").getList().then(function (data) {
        $scope.types = data;
    });
    
    //#region Party
    var fetchPartys = function () {
        $timeout(function () {
            Party.getList().then(function (data) {
                $scope.partys = data;
            });
        }, 100);
    };

    $scope.cur = { item:{ }};
    fetchPartys();
    $scope.addParty = function () {
        $scope.cur = { item: {} };
    };
    $scope.saveParty = function (a, type) {
        if (a.Id) {
            Party.one(a.Id).doPUT(a);
        } else {
            a.TypeId = type.Id;
            Party.post(a);
            fetchPartys();
        }
        return true;
    };
    $scope.delParty = function(a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.partys.indexOf();
        $scope.partys.splice(index, 1);
    };
    //#endregion
}
]);