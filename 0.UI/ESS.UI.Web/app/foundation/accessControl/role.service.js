'use strict';

angular.module('EssApp').factory('Role', ['Restangular',
    function (Restangular) {

        var Role = Restangular.service("Role");

        Role.createRole = function(role) {
            Role.post(role);
        };

        Role.changeRoleInfo = function(role) {
            Role.one(role.Id).doPUT(role);
        };

        Role.assignUser = function (role) {
            Role.one(role.Id).doPOST(role, "AssignUser");
        };

        Role.lock = function (role) {
            role.Locked = true;
            Role.one(role.Id).doPOST(role, "Lock");
        };
        Role.unlock = function (role) {
            role.Locked = false;
            Role.one(role.Id).doPOST(role, "Unlock");
        };
        return Role;
    }
]);