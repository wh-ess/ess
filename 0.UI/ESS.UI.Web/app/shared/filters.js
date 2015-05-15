'use strict';

/* Filters */

angular.module('EssApp')
.filter('interpolate', ['version', function (version) {
    return function (text) {
        return String(text).replace(/\%VERSION\%/mg, version);
    };
}])
.filter('skip', function () {
    return function (input, skipCount) {
        if (angular.isArray(input)) {
            return input.slice(skipCount);
        }
        return input;
    };
});
