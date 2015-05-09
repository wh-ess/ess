"use strict";

angular.module("EssApp").controller("PartyRoleController", [
    "$scope", "PartyRole", "CategoryTypeScheme", "Category", "$routeParams", "$timeout",
function ($scope, PartyRole, CategoryTypeScheme, Category, $routeParams, $timeout) {

    CategoryTypeScheme.one("PartyRole").one("CategoryType", "PartyRole").getList().then(function (data) {
        $scope.types = data;
    });
    
    //#region PartyRole
    var fetchPartyRoles = function () {
        $timeout(function () {
            PartyRole.getList().then(function (data) {
                $scope.partyRoles = data;
            });
        }, 100);
    };

    fetchPartyRoles();
    $scope.addPartyRole = function() {
        $scope.partyRoles.push({});
    };
    $scope.savePartyRole = function (a, type) {
        if (a.Id) {
            PartyRole.one(a.Id).doPUT(a);
        } else {
            a.TypeId = type.Id;
            PartyRole.post(a);
            fetchPartyRoles();
        }
        return true;
    };
    $scope.delPartyRole = function(a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.partyRoles.indexOf();
        $scope.partyRoles.splice(index, 1);
    };
    //#endregion
}
]);