'use strict';
angular.module('EssApp').factory('Module', ['Restangular',
    function (Restangular) {
        var Module = Restangular.service("Module");
        return Module;
    }
]).factory('Fields', ['Restangular',
    function (Restangular) {
        var Fields = Restangular.service("Fields");
        return Fields;
    }
]).factory('Entity', ['Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Entity");
        return Entity;
    }
]).factory('DDL', ['Restangular',
    function (Restangular) {
        var DDL = Restangular.service("Ddl");
        return DDL;
    }
]).factory('ReadModel', ['Restangular',
    function (Restangular) {
        var DDL = Restangular.service("ReadModel");
        return DDL;
    }
]);

var Enums = {};
$.getJSON("/api/enum/enums", function (data) {
    Enums = JSON.parse(data);
});