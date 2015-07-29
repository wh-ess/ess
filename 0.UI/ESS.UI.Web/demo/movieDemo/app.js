"use strict";
function arrayObjectIndexOf(myArray, searchTerm) {
    for (var i = 0, len = myArray.length; i < len; i++) {
        if (myArray[i] === searchTerm) return i;
    }
    return -1;
}

// Declare app level module which depends on filters, and services
angular.module("EssApp", [
    "ngResource",
    "restangular",
    "ngRoute",
    "ngMaterial",
    "ngMessages",
    "ngSanitize",
    "ui.utils",
    "LocalStorageModule",
    "ui.bootstrap",
    "ngFileUpload"
]).run([
    "$rootScope", function($rootScope) {


    }
]).config([
    "$routeProvider", "$locationProvider", function route($routeProvider, $locationProvider) {
        $routeProvider.otherwise({ redirectTo: "/list" });
        //$locationProvider.html5Mode(true);

        $routeProvider
                //login
                .when("/list", { templateUrl: "/demo/movieDemo/movieList.html", controller: "MovieController" })
                .when("/selectSeat/:id", { templateUrl: "/demo/movieDemo/seatSelect.html", controller: "MovieController" })
        //#endregion
        ;
    }
]).controller("MovieController", ["$scope", "$routeParams", "$location", function ($scope, $routeParams, $location) {
    $scope.movies = [{id:1, img: "/demo/movieDemo/1.jpg", title: "´óÊ¥¹éÀ´", note: "ds", score: 9.0 },
        { id: 2, img: "/demo/movieDemo/2.jpg", title: "¼å±ýÏÀ", note: "¼å±ýÏÀ", score: 8.6 }];

    $scope.selectSeat = function (id) {
        $scope.current = $scope.movies[id];
    };
    $scope.go = function (path) {
        $location.path(path);
    };
    if ($routeParams.id) {
        $scope.current = $scope.movies[$routeParams.id-1];
    };

    $scope.seats = [
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "1-1", background: "cwEceu" }, { no: "1-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "1-3", background: "cwEceu" }, { no: "1-4", background: "cwEceu" }, { no: "1-5", background: "cwEceu" }, { no: "1-6", background: "cwEceu" }, { no: "1-7", background: "cwEceu" }, { no: "1-8", background: "cwEceu" }, { no: "1-9", background: "cwEceu" }, { no: "1-10", background: "cwEceu" }, { no: "1-11", background: "cwEceu" }, { no: "1-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "2-1", background: "cwEceu" }, { no: "2-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "2-3", background: "cwEceu" }, { no: "2-4", background: "cwEceu" }, { no: "2-5", background: "cwEceu" }, { no: "2-6", background: "cwEceu" }, { no: "2-7", background: "cwEceu" }, { no: "2-8", background: "cwEceu" }, { no: "2-9", background: "cwEceu" }, { no: "2-10", background: "cwEceu" }, { no: "2-11", background: "cwEceu" }, { no: "2-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "3-1", background: "cwEceu" }, { no: "3-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "3-3", background: "cwEceu" }, { no: "3-4", background: "cwEceu" }, { no: "3-5", background: "cwEceu" }, { no: "3-6", background: "cwEceu" }, { no: "3-7", background: "cwEceu" }, { no: "3-8", background: "cwEceu" }, { no: "3-9", background: "cwEceu" }, { no: "3-10", background: "cwEceu" }, { no: "3-11", background: "cwEceu" }, { no: "3-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "4-1", background: "cwEceu" }, { no: "4-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "4-3", background: "cwEceu" }, { no: "4-4", background: "cwEceu" }, { no: "4-5", background: "cwEceu" }, { no: "4-6", background: "cwEceu" }, { no: "4-7", background: "cwEceu" }, { no: "4-8", background: "cwEceu" }, { no: "4-9", background: "cwEceu" }, { no: "4-10", background: "cwEceu" }, { no: "4-11", background: "cwEceu" }, { no: "4-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "5-1", background: "cwEceu" }, { no: "5-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "5-3", background: "cwEceu" }, { no: "5-4", background: "cwEceu" }, { no: "5-5", background: "cwEceu" }, { no: "5-6", background: "cwEceu" }, { no: "5-7", background: "cwEceu" }, { no: "5-8", background: "cwEceu" }, { no: "5-9", background: "cwEceu" }, { no: "5-10", background: "cwEceu" }, { no: "5-11", background: "cwEceu" }, { no: "5-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "6-1", background: "cwEceu" }, { no: "6-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "6-3", background: "cwEceu" }, { no: "6-4", background: "cwEceu" }, { no: "6-5", background: "cwEceu" }, { no: "6-6", background: "cwEceu" }, { no: "6-7", background: "cwEceu" }, { no: "6-8", background: "cwEceu" }, { no: "6-9", background: "cwEceu" }, { no: "6-10", background: "cwEceu" }, { no: "6-11", background: "cwEceu" }, { no: "6-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "7-1", background: "cwEceu" }, { no: "7-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "7-3", background: "cwEceu" }, { no: "7-4", background: "cwEceu" }, { no: "7-5", background: "cwEceu" }, { no: "7-6", background: "cwEceu" }, { no: "7-7", background: "cwEceu" }, { no: "7-8", background: "cwEceu" }, { no: "7-9", background: "cwEceu" }, { no: "7-10", background: "cwEceu" }, { no: "7-11", background: "cwEceu" }, { no: "7-12", background: "cwEceu" }],
    [{ background: "noSeat" }, { background: "noSeat" }, { background: "noSeat" }, { no: "8-1", background: "cwEceu" }, { no: "8-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "8-3", background: "cwEceu" }, { no: "8-4", background: "cwEceu" }, { no: "8-5", background: "cwEceu" }, { no: "8-6", background: "cwEceu" }, { no: "8-7", background: "cwEceu" }, { no: "8-8", background: "cwEceu" }, { no: "8-9", background: "cwEceu" }, { no: "8-10", background: "cwEceu" }, { no: "8-11", background: "cwEceu" }, { no: "8-12", background: "cwEceu" }],
    [{ no: "9-13", background: "cwEceu" }, { no: "9-14", background: "cwEceu" }, { no: "9-15", background: "cwEceu" }, { no: "9-1", background: "cwEceu" }, { no: "9-2", background: "cwEceu" }, { background: "noSeat" }, { background: "noSeat" }, { no: "9-3", background: "cwEceu" }, { no: "9-4", background: "cwEceu" }, { no: "9-5", background: "cwEceu" }, { no: "9-6", background: "cwEceu" }, { no: "9-7", background: "cwEceu" }, { no: "9-8", background: "cwEceu" }, { no: "9-9", background: "cwEceu" }, { no: "9-10", background: "cwEceu" }, { no: "9-11", background: "cwEceu" }, { no: "9-12", background: "cwEceu" }],
    [{ no: "10-13", background: "cwEceu" }, { no: "10-14", background: "cwEceu" }, { no: "10-15", background: "cwEceu" }, { no: "10-1", background: "cwEceu" }, { no: "10-2", background: "cwEceu" }, { no: "10-16", background: "cwEceu" }, { no: "10-17", background: "cwEceu" }, { no: "10-3", background: "cwEceu" }, { no: "10-4", background: "cwEceu" }, { no: "10-5", background: "cwEceu" }, { no: "10-6", background: "cwEceu" }, { no: "10-7", background: "cwEceu" }, { no: "10-8", background: "cwEceu" }, { no: "10-9", background: "cwEceu" }, { no: "10-10", background: "cwEceu" }, { no: "10-11", background: "cwEceu" }, { no: "10-12", background: "cwEceu" }]
    
    ];

    $scope.orderSeats = [];
    $scope.clickSeat = function (seat) {
        var index = arrayObjectIndexOf($scope.orderSeats, seat);
        if (index >= 0) {
            seat.background = "cwEceu";
            $scope.orderSeats.splice(index);
        } else {
            seat.background = "bPRxja";
            $scope.orderSeats.push(seat);
        }
    };
}]);

