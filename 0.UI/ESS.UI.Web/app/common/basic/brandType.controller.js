"use strict";

angular.module("EssApp").controller("BrandTypeController", [
    "$scope", "BrandType", "$routeParams", "$timeout",
function ($scope, BrandType, $routeParams, $timeout) {
    var fetchBrandTypes = function () {
        $timeout(function() {
            BrandType.getList().then(function (data) {
                $scope.brandTypes = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        BrandType.one($routeParams.id).get().then(function (brandType) {
            $scope.current.item = brandType;
        });
    } else {
        fetchBrandTypes();
    }

    $scope.addBrandType = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBrandType = function () {
        if ($scope.current.item.Id) {
            BrandType.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            BrandType.post($scope.current.item);
            fetchBrandTypes();
        }
    };

}
]);