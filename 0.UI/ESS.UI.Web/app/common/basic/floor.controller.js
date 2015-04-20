"use strict";

angular.module("EssApp").controller("FloorController", [
    "$scope", "Floor", "$routeParams", "$timeout",
function ($scope, Floor, $routeParams, $timeout) {
    var fetchFloors = function () {
        $timeout(function() {
            Floor.getList().then(function (data) {
                $scope.floors = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Floor.one($routeParams.id).get().then(function (floor) {
            $scope.current.item = floor;
        });
    } else {
        fetchFloors();
    }

    $scope.addFloor = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editFloor = function () {
        if ($scope.current.item.Id) {
            Floor.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Floor.post($scope.current.item);
            fetchFloors();
        }
    };

}
]);