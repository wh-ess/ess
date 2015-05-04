"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp", [
    "ngResource",
    "restangular",
    "ngRoute",
    "ngMaterial",
    "ngMessages",
    "ngSanitize",
    "ui.utils",
    "ui.tree",
    "LocalStorageModule"
]).run(["$rootScope", "DDL", function ($rootScope, DDL) {
    $rootScope.pageTitle = "Title";
    $rootScope.pageSubTitle = "subTitle";

        $rootScope.ddl = {};
        $rootScope.getDdl = function (key) {
            $rootScope.ddl[key] = [];
            DDL.one(key).getList().then(function(data) {
                $rootScope.ddl[key] = data;
                return data;
            });
        }

}]).config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push(["$q", function ($q) {
        return {
            responseError: function (rejection) {
                if (rejection.status == 500) {
                    alert(rejection.data.ExceptionMessage);
                } else if (rejection.status == 401) {
                    //其它地方会跳转
                } else {
                    alert("Error!");
                }
                return $q.reject(rejection);
            },
            response: function (response) {
                return response;
            }

        };
    }]);
    $httpProvider.interceptors.push("authInterceptorService");
}]).config(["RestangularProvider", function (RestangularProvider) {
    RestangularProvider.setBaseUrl("/api");
    // add a response intereceptor
    RestangularProvider.addResponseInterceptor(function (data, operation, what, url, response, deferred) {

        return data;
    });

}])
;


angular.isUndefinedOrNull = function (val) {
    return angular.isUndefined(val) || val === null;
}
