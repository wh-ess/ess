/// <reference path="foundation/fieldConifg/fieldModule.html" />
"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp").run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams, $previousState) {

    // It's very handy to add references to $state and $stateParams to the $rootScope
    // so that you can access them from any scope within your applications.For example,
    // <li ng-class="{ active: $state.includes('contacts.list') }"> will set the <li>
    // to active whenever 'contacts.list' or one of its decendents is active.
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.goPrevious = function () {
        $previousState.go();
    };


}]).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    // Redirect any unmatched url
    $urlRouterProvider.otherwise("/");

    $stateProvider
        //login
        .state("login", { url: "/login", templateUrl: "/app/foundation/AccessControl/login.html", controller: "loginController" })
        //module
        .state('module', {
            abstract: true, url: "/module", template: "<div ui-view></div>",
            data: { pageTitle: 'Module', pageSubTitle: 'Module & reports' }
        })
        .state("module.list", { url: "/list", templateUrl: "/app/foundation/moduleConifg/moduleTable.html", controller: "ModuleController" })
        .state("module.actions", { url: "/:moduleNo/actions", templateUrl: "/app/foundation/moduleConifg/actionTable.html", controller: "ActionController" })
        .state("module.fields", { url: "/:moduleNo/actions/:actionName/fields", templateUrl: "/app/foundation/moduleConifg/fieldTable.html", controller: "FieldController" })

        //user
        .state('user', {
            abstract: true, url: "/user", template: "<div ui-view></div>",
            data: { pageTitle: 'User', pageSubTitle: 'User & reports' }
        })
        .state("user.list", { url: "/list", templateUrl: "/app/foundation/AccessControl/userTable.html", controller: "UserController" })
    .state('user.add', { url: "/add", templateUrl: "/app/foundation/AccessControl/userEdit.html", controller: "UserController", })
    .state('user.edit', { url: "/edit/:id", templateUrl: "/app/foundation/AccessControl/userEdit.html", controller: "UserController", })

        //role
    .state('role', {
        abstract: true, url: "/role", template: "<div ui-view></div>",
        data: { pageTitle: 'Role', pageSubTitle: 'Role & reports' }
    })
        .state("role.list", { url: "/list", templateUrl: "/app/foundation/AccessControl/roleTable.html", controller: "RoleController" })
    .state('role.add', { url: "/add", templateUrl: "/app/foundation/AccessControl/roleEdit.html", controller: "RoleController", })
    .state('role.edit', { url: "/edit/:id", templateUrl: "/app/foundation/AccessControl/roleEdit.html", controller: "RoleController", });

}
]);
