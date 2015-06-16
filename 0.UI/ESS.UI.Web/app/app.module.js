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
    "ui.bootstrap",
    "ngFileUpload"
]).run(["$rootScope", "$mdSidenav", "DDL", "authService", "Upload", function ($rootScope, $mdSidenav, DDL, authService, Upload) {
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

    $rootScope.upload = function (files,model) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                Upload.upload({
                    url: 'api/upload',
                    file: file
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' + evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    model.Image = "upload/"+data[0];
                    console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                });
            }
        }
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
