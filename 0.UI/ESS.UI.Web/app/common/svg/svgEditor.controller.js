"use strict";

angular.module("EssApp").controller("SvgEditorController", [
    "$scope",
function ($scope) {
    var canvas = SVG("canvas");
    var canvasBackground = canvas.nested();
    var canvasContent = canvas.nested();
    canvasContent.rect(200, 100).draggy();
    canvasContent.rect(200, 100).fill('#f03').draggy();
}
]);