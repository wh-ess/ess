'use strict';

angular.module('EssApp').factory('User', ['Restangular',
    function (Restangular) {

        var User = Restangular.service("User");

        User.createUser = function(user) {
            User.post(user);
        };

        User.changeUserInfo = function(user) {
            User.one(user.Id).doPUT(user);
        };

        User.changePassword = function (user) {
            User.one(user.Id).doPOST(user, "ChangePassword");
        };
        User.lock = function (user) {
            User.one(user.Id).doPOST(user, "Lock");
        };
        User.unlock = function (user) {
            User.one(user.Id).doPOST(user, "Unlock");
        };
        return User;
    }
]);