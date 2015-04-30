"use strict";
angular.module("EssApp").factory("Association", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Association");
        return Entity;
    }
]);