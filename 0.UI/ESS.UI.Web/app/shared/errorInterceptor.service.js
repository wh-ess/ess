'use strict';
angular.module("EssApp").factory('errorInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

    var errorInterceptorServiceFactory = {};
    
    var _responseError = function (rejection) {
        if (rejection.status === 500) {
            alert(rejection.data.ExceptionMessage);
        }
        else if (rejection.status === 0) {
            alert("offline");
            //offline
        } else if (rejection.status === 401) {
            //其它地方会跳转
        } else {
            alert("Error!");
        }
        return $q.reject(rejection);
    }

    errorInterceptorServiceFactory.responseError = _responseError;

    return errorInterceptorServiceFactory;
}]);