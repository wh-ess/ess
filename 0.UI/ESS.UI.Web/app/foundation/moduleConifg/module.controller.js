"use strict";

angular.module("EssApp").controller("ModuleController", [
    "$scope", "Module",
    function($scope, Module) {

        $scope.modules = Module.getList().$object;

        $scope.save = function() {
            Module.post($scope.modules);
        };
    }
]).controller("FieldController", [
    "$scope", "Module", "$routeParams", "$mdDialog",
    function($scope, Module, $routeParams, $mdDialog) {

        if ($routeParams.moduleNo) {
            var Field = Module.one($routeParams.moduleNo).one("actions", $routeParams.actionName);
            Field.getList("Fields").then(function(data) {
                $scope.fields = data;
            });
        }

        $scope.add = function() {
            $scope.fields.push({ Name: "*", Index: 0 });
        };
        $scope.del = function(index) {
            $scope.fields.splice(index, 1);
        };

        $scope.select = function (ev) {

            $mdDialog.show({
                templateUrl: "/app/foundation/moduleConifg/fieldSelect.html",
                controller: "FieldSelectController",
                targetEvent: ev
            }).then(function(selectedItem) {
                $scope.fields.push(selectedItem);
            });
        };

        $scope.save = function() {
            $scope.fields.post();
        };

    }
]).controller("FieldSelectController", [
    "$scope", "$mdDialog", "Fields", function($scope, $mdDialog, Fields) {
        $scope.selected = {
            item: {}
        };
        Fields.getList().then(function(items) {
            $scope.items = items;
        });

        $scope.ok = function() {
            $mdDialog.hide($scope.selected.item);
        };

        $scope.cancel = function() {
            $mdDialog.cancel();
        };

    }
]).controller("ActionController", [
    "$scope", "Module", '$routeParams',
    function($scope, Module, $routeParams) {
        if ($routeParams.moduleNo) {
            var Actions = Module.one($routeParams.moduleNo);
            $scope.actions = Actions.getList("Actions").$object;
        }
        $scope.save = function() {
            $scope.actions.post();
        };
    }
]);