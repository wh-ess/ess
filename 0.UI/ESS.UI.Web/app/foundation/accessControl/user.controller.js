"use strict";

angular.module("EssApp").controller("UserController", [
    "$scope", "User", "$routeParams", "$timeout",
function ($scope, User, $routeParams, $timeout) {
    var fetchUsers = function () {
        $timeout(function () {
            User.getList().then(function (data) {
                $scope.users = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        User.one($routeParams.id).get().then(function (user) {
            $scope.current.item = user;
        });
    } else {
        fetchUsers();
    }

    $scope.addUser = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.changeUserInfo = function () {
        if ($scope.current.item.Id) {
            User.changeUserInfo($scope.current.item);
        } else {
            User.createUser($scope.current.item);
        }
        fetchUsers();
    };

    $scope.changePassword = function () {
        User.changePassword($scope.current.item);
    };
    $scope.lock = function (user) {
        User.lock(user);
    }

    $scope.unlock = function (user) {
        User.unlock(user);
    }
}
]);