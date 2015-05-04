"use strict";

angular.module("EssApp").controller("AssociationController", [
    "$scope", "CategoryTypeScheme", "Association", "$routeParams", "$timeout",
function ($scope, CategoryTypeScheme, Association, $routeParams, $timeout) {

    CategoryTypeScheme.one("Association").getList("CategoryType").then(function (data) {
        $scope.categoryTypes = data;
    });
    //#region Association
    var fetchAssociations = function () {
        $timeout(function () {
            Association.getList().then(function (data) {
                $scope.Associations = data;
            });
        }, 100);
    };

    fetchAssociations();

    $scope.editAssociation = function (cat, type) {
        if (cat.Id) {
            Association.one(cat.Id).doPUT();
        } else {
            cat.TypeId = type.Id;
            Association.post(cat);
            fetchAssociations();
        }
        return true;
    };
    $scope.delAssociation = function (cat) {
        if (cat.Id) {
            cat.remove({ Id: cat.Id });

            var index = $scope.Associations.indexOf();
            $scope.Associations.splice(index, 1);
        }
    }
    //#endregion
}
]);