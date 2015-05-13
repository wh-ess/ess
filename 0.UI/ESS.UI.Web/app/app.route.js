"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp").run(['$rootScope','$route', '$routeParams', '$location',
  function ($rootScope,$route, $routeParams, $location) {
    $rootScope.$route = $route;
    $rootScope.$location = $location;
    $rootScope.$routeParams = $routeParams;
  }]).config(["$routeProvider", "$locationProvider", function route($routeProvider, $locationProvider) {
    $routeProvider.otherwise({ redirectTo: "/help" });
    //$locationProvider.html5Mode(true);

        $routeProvider
            //login
            .when("/login", { templateUrl: "/app/foundation/AccessControl/login.html", controller: "loginController" })

            //module
            .when("/module", { templateUrl: "/app/foundation/moduleConifg/moduleTable.html", controller: "ModuleController" })
            .when("/module/:moduleNo/actions", { templateUrl: "/app/foundation/moduleConifg/actionTable.html", controller: "ActionController" })
            .when("/module/:moduleNo/actions/:actionName/fields", { templateUrl: "/app/foundation/moduleConifg/fieldTable.html", controller: "FieldController" })

            //user
            .when("/user", { templateUrl: "/app/foundation/AccessControl/user.html", controller: "UserController" })

            //role
            .when("/role", { templateUrl: "/app/foundation/AccessControl/role.html", controller: "RoleController" })

            //help
            .when("/help", { templateUrl: "/app/shared/help/help.html" })

            //common
            //category
            .when("/categoryType", { templateUrl: "/app/common/category/categoryType.html", controller: "CategoryController" })
            .when("/category", { templateUrl: "/app/common/category/category.html", controller: "CategoryController" })

            //association
            .when("/association", { templateUrl: "/app/common/association/association.html", controller: "AssociationController" })

            //partyrole
            .when("/party", { templateUrl: "/app/common/partyRole/party.html", controller: "PartyController" })
            .when("/partyRole", { templateUrl: "/app/common/partyRole/partyRole.html", controller: "PartyRoleController" })

            //basic
            .when("/bank", { templateUrl: "/app/common/basic/bank.html", controller: "BankController" })
            .when("/floor", { templateUrl: "/app/common/basic/floor.html", controller: "FloorController" })
            .when("/brand", { templateUrl: "/app/common/basic/brand.html", controller: "BrandController" })
            .when("/brandType", { templateUrl: "/app/common/basic/brandType.html", controller: "BrandTypeController" })
      ;
    }
]);

function getUrl(path) {
    return "/app/" + path.name + ".html";
}
function getController(path) {
    return path.name.substring(path.name.lastIndexOf("/") + 1) + "Controller";
}
