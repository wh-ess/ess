"use strict";

angular.module("EssApp").controller("AssociationController", [
    "$scope", "Association", "CategoryTypeScheme", "Category", "$routeParams", "$timeout",
function ($scope, Association, CategoryTypeScheme, Category, $routeParams, $timeout) {

    CategoryTypeScheme.one("Association").getList("CategoryType").then(function (data) {
        $scope.categoryTypes = data;
    });

    Category.getList().then(function (data) {
        $scope.categorys = data;
    });

    $scope.rules = $scope.$parent.getDdl("AssociationRule");
    
    //#region Association
    var fetchAssociations = function () {
        $timeout(function () {
            Association.getList().then(function (data) {
                $scope.Associations = data;
            });
        }, 100);
    };

    fetchAssociations();
    $scope.addAssociation = function() {
        $scope.Associations.push({});
    };
    $scope.saveAssociation = function (a, type) {
        if (a.Id) {
            Association.one(a.Id).doPUT(a);
        } else {
            a.TypeId = type.Id;
            Association.post(a);
            fetchAssociations();
        }
        return true;
    };
    $scope.delAssociation = function(a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.Associations.indexOf();
        $scope.Associations.splice(index, 1);
    };
    //#endregion
}
]);