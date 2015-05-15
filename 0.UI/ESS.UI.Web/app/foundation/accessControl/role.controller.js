'use strict';

angular.module('EssApp').controller('RoleController', [
    '$scope', 'Role', "User", '$routeParams',
function ($scope, Role, User, $routeParams) {

    var fetchRoles = function () {
        Role.getList().then(function (data) {
            $scope.roles = data;
        });
    };
    $scope.current = { item: {} };
    if ($routeParams.id) {
        Role.one($routeParams.id).get().then(function (role) {
            $scope.current.item = role;
        });
    } else {
        fetchRoles();
    }

    $scope.ddl["Users"] = User.getList().$object;

    $scope.addRole = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.changeRoleInfo = function (role) {
        if (role.id) {
            Role.changeRoleInfo(role);
        } else {
            Role.createRole(role);
        }
        fetchRoles();
    };

    $scope.assignUser = function (role) {
        Role.assignUser(role);
    }

    $scope.lock = function (role) {
        Role.lock(role);
    }

    $scope.unlock = function (role) {
        Role.unlock(role);
    }
}
]);