"use strict";

angular.module("EssApp").controller("CategoryController", [
    "$scope", "CategoryTypeScheme","CategoryType", "$routeParams", "$timeout",
function ($scope, CategoryTypeScheme, CategoryType,$routeParams, $timeout) {
    //#region CategoryTypeScheme
    var fetchCategoryTypeSchemes = function () {
        $timeout(function () {
            CategoryTypeScheme.getList().then(function (data) {
                $scope.categoryTypeSchemes = data;
            });
        }, 100);
    };

    fetchCategoryTypeSchemes();

    $scope.addCategoryTypeScheme = function () {
        $scope.mode = "edit";
        $scope.categoryTypeSchemes.push({});
    }
    $scope.editCategoryTypeScheme = function (sheme) {
        if (sheme.Id) {
            CategoryTypeScheme.one(sheme.Id).doPUT(sheme);
        } else {
            CategoryTypeScheme.post(sheme);
            fetchCategoryTypeSchemes();
        }
        return true;
    };
    //#endregion

    //#region CategoryType
    var fetchCategoryTypes = function () {
        $timeout(function () {
            CategoryType.getList().then(function (data) {
                $scope.categoryTypes = data;
            });
        }, 100);
    };

    fetchCategoryTypes();

    $scope.addCategoryType = function () {
        $scope.mode = "edit";
        $scope.categoryTypes.push({});
    }
    $scope.editCategoryType = function (sheme) {
        if (sheme.Id) {
            CategoryType.one(sheme.Id).doPUT(sheme);
        } else {
            CategoryType.post(sheme);
            fetchCategoryTypes();
        }
        return true;
    };
    //#endregion
}
]);