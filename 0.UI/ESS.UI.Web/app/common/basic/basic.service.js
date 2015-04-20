'use strict';
angular.module('EssApp').factory('Brand', [
    'Restangular',
    function (Restangular) {
        var Module = Restangular.service("Brand");
        return Module;
    }
]).factory('BrandType', [
    'Restangular',
    function (Restangular) {
        var Fields = Restangular.service("BrandType");
        return Fields;
    }
]).factory('Floor', [
    'Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Floor");
        return Entity;
    }
]).factory('Bank', [
    'Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Bank");
        return Entity;
    }
]);