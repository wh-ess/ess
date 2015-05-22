"use strict";

angular.module("EssApp").controller("PartyRoleController", [
    "$scope", "PartyRole", "CategoryTypeScheme", "Category", "$routeParams", "$timeout", "$mdDialog",
function ($scope, PartyRole, CategoryTypeScheme, Category, $routeParams, $timeout, $mdDialog) {

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

    $scope.cur = { item: {} };
    fetchPartyRoles();

    $scope.addPartyRole = function (ev) {
        $mdDialog.show({
            templateUrl: "/app/foundation/moduleConifg/fieldSelect.html",
            controller: "PartySelectController",
            targetEvent: ev
        }).then(function (item) {
            var p = {
                Party: item
            }
            $scope.cur = { item: p };
        });
    };


    $scope.savePartyRole = function (a, type) {
        a.TypeId = type.Id;
        if (a.Id) {
            PartyRole.one(a.Id).doPUT(a);
        } else {
            PartyRole.post(a);
            fetchPartyRoles();
        }
        return true;
    };
    $scope.delPartyRole = function (a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.partyRoles.indexOf();
        $scope.partyRoles.splice(index, 1);
    };
    //#endregion
}
]).controller("PartySelectController", ["$scope", "$mdDialog", "Party", function ($scope, $mdDialog, Party) {
    $scope.selected = {
        item: {}
    };
    Party.getList().then(function (items) {
        $scope.items = items;
    });

    $scope.ok = function () {
        $mdDialog.hide($scope.selected.item);
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

}]);