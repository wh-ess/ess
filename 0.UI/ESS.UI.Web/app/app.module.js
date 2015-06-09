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
    "LocalStorageModule",
    "ui.bootstrap"
]).run(["$rootScope", "$mdSidenav", "DDL", "authService", function ($rootScope, $mdSidenav, DDL, authService) {
    $rootScope.pageTitle = "Index";
    $rootScope.pageSubTitle = "";

    $rootScope.auth = authService;
    $rootScope.auth.fillAuthData();
    $rootScope.$watch("auth.authentication.isAuth", function (newVal) {
        $rootScope.isAuth = newVal;
        
    });

    $rootScope.changeTitle = function (title, subTitle) {
        $rootScope.pageTitle = title;
        $rootScope.pageSubTitle = subTitle;
    };

    $rootScope.toggleSidenav = function (menuId) {
        $mdSidenav(menuId).toggle();
    };

    $rootScope.ddl = {};
    $rootScope.getDdl = function (key) {
        $rootScope.ddl[key] = [];
        DDL.one(key).getList().then(function (data) {
            $rootScope.ddl[key] = data;
            return data;
        });
    };

}]).config(["$httpProvider", function ($httpProvider) {

    $httpProvider.interceptors.push("authInterceptorService");
    $httpProvider.interceptors.push("errorInterceptorService");

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
