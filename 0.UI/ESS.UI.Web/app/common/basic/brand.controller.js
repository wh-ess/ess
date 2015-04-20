"use strict";

angular.module("EssApp").controller("BrandController", [
    "$scope", "Brand", "$routeParams", "$timeout",
function ($scope, Brand, $routeParams, $timeout) {
    var fetchBrands = function () {
        $timeout(function() {
            Brand.getList().then(function (data) {
                $scope.brands = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Brand.one($routeParams.id).get().then(function (brand) {
            $scope.current.item = brand;
        });
    } else {
        fetchBrands();
    }

    $scope.addBrand = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBrand = function () {
        if ($scope.current.item.Id) {
            Brand.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Brand.post($scope.current.item);
            fetchBrands();
        }
    };

}
]);