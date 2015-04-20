'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('EssApp').
    value('version', '0.1')
    .service("query", [
        "$http", function($http) {
            this.query = function(param) {
                return $http.post("/query", { key: param });
            };

            return this;
        }
    ]);