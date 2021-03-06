﻿"use strict";

angular.module("EssApp").controller("CategoryController", [
    "$scope", "CategoryTypeScheme", "CategoryType", "Category", "$routeParams", "$timeout",
function ($scope, CategoryTypeScheme, CategoryType, Category, $routeParams, $timeout) {
    $scope.cur = {
        type: {},
        scheme: {}
    };
    //#region CategoryTypeScheme
    var fetchCategoryTypeSchemes = function () {
        $timeout(function () {
            CategoryTypeScheme.getList().then(function (data) {
                $scope.categoryTypeSchemes = data;
            });
        }, 100);
    };
    fetchCategoryTypeSchemes();

    $scope.categoryTypeScheme = [{Name:"fdsfs"}, {Name:"fdsfds"}];

    $scope.editCategoryTypeScheme = function (scheme) {
        if (scheme.Id) {
            CategoryTypeScheme.one(scheme.Id).doPUT(scheme);
        } else {
            CategoryTypeScheme.post(scheme);
            fetchCategoryTypeSchemes();
        }
        return true;
    };
    $scope.delCategoryTypeScheme = function (scheme) {
        if (scheme.Id) {
            scheme.remove({ Id: scheme.Id });

        }
        var index = $scope.categoryTypeSchemes.indexOf(scheme);
        $scope.categoryTypeSchemes.splice(index, 1);
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

    $scope.editCategoryType = function (type, scheme) {
        type.Scheme = scheme;

        if (type.Id) {
            CategoryType.one(type.Id).doPUT(type);
        } else {
            CategoryType.post(type);
            fetchCategoryTypes();
        }
        return true;
    };
    $scope.editCategoryTypeWithParent = function (type, parent, scheme) {
        type.ParentId = parent.Id;
        type.Scheme = scheme;

        if (type.Id) {
            CategoryType.one(type.Id).doPUT(type);
        } else {
            CategoryType.post(type);
            fetchCategoryTypes();
        }
        return true;
    };
    $scope.delCategoryType = function (type) {
        if (type.Id) {
            type.remove({ Id: type.Id });

        }
        var index = $scope.categoryTypes.indexOf(type);
        $scope.categoryTypes.splice(index, 1);
    };
    //#endregion

    //#region Category
    var fetchCategorys = function () {
        $timeout(function () {
            Category.getList().then(function (data) {
                $scope.categorys = data;
            });
        }, 100);
    };

    fetchCategorys();

    $scope.editCategory = function (cat, type) {
        if (cat.Id) {
            Category.one(cat.Id).doPUT();
        } else {
            cat.Type = type;
            Category.post(cat);
            fetchCategorys();
        }
        return true;
    };
    $scope.delCategory = function (cat) {
        if (cat.Id) {
            cat.remove({ Id: cat.Id });
        }

        var index = $scope.categorys.indexOf();
        $scope.categorys.splice(index, 1);
    };
    //#endregion
}
]);