﻿"use strict";

angular.module("EssApp").controller("PopTemplateController", [
    "$scope", "PopTemplate", "$routeParams", "$timeout",
    function ($scope, PopTemplate, $routeParams, $timeout) {
        $scope.current = { item: {} };
        //#region PopTemplate
        var fetchPopTemplates = function () {
            $timeout(function () {
                PopTemplate.getList().then(function (data) {
                    $scope.popTemplates = data;
                });
            }, 100);
        };
        if ($routeParams.id) {
            PopTemplate.one($routeParams.id).get().then(function (data) {
                $scope.current.item = data;
            });
        }

        fetchPopTemplates();
        $scope.addPopTemplate = function () {
            $scope.popTemplates.push({});
        };
        $scope.savePopTemplate = function (a, type) {
            if (a.Id) {
                PopTemplate.one(a.Id).doPUT(a);
            } else {
                a.Type = type;
                PopTemplate.post(a);
                fetchPopTemplates();
            }
            return true;
        };
        $scope.delPopTemplate = function (a) {
            if (a.Id) {
                a.remove({ Id: a.Id });
            }
            var index = $scope.popTemplates.indexOf();
            $scope.popTemplates.splice(index, 1);
        };
        //#endregion

    }
]);