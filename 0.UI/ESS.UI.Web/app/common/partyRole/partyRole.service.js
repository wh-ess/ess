"use strict";
angular.module("EssApp").factory("Party", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Party");
        return Entity;
    }
]).factory("PartyRole", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("PartyRole");
        return Entity;
    }
]);