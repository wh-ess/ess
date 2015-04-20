"use strict";
angular.module("EssApp").factory("CategoryTypeScheme", [
    "Restangular",
    function (Restangular) {
        var Module = Restangular.service("CategoryTypeScheme");
        return Module;
    }
]).factory("CategoryType", [
    "Restangular",
    function (Restangular) {
        var Fields = Restangular.service("CategoryType");
        return Fields;
    }
]).factory("Category", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Category");
        return Entity;
    }
]);