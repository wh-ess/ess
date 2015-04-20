'use strict';

angular.module('EssApp').controller('RoleController', [
    '$scope', 'toastr', 'Role',"User", '$routeParams',
function ($scope, toastr, Role, User, $routeParams) {

    var fetchRoles = function () {
        Role.getList().then(function (data) {
            $scope.roles = data;
        });
    };
    $scope.current.role = {};
    if ($routeParams.id) {
        Role.one($routeParams.id).get().then(function (role) {
            $scope.current.role = role;
        });
    } else {
        fetchRoles();
    }

    $scope.users = User.getList().$object;

    $scope.changeRoleInfo = function () {
        if ($routeParams.id) {
            Role.changeRoleInfo($scope.current.role);
        } else {
            Role.createRole($scope.current.role);
        }
    };

    $scope.assignUser = function() {
        Role.assignUser($scope.current.role);
    }

    $scope.lock = function(role) {
        Role.lock(role);
    }

    $scope.unlock = function (role) {
        Role.unlock(role);
    }
}
]);