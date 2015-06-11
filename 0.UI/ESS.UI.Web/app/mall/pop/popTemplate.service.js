"use strict";
angular.module("EssApp").factory("PopTemplate", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("PopTemplate");
        return Entity;
    }
]);