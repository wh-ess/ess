'use strict';
angular.module('EssApp').factory('Menu', [
    'Restangular',
    function(Restangular) {
        var Menu = Restangular.service("Menu");
        return Menu;
    }
]);