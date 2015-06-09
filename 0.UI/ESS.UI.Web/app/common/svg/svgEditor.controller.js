"use strict";

angular.module("EssApp").controller("SvgEditorController", [
    "$scope",
function ($scope) {
    var svgCanvas = null;

    $scope.init_embed = function () {
        var frame = document.getElementById('svgedit');
        svgCanvas = new embedded_svg_edit(frame);

        // Hide main button, as we will be controlling new/load/save etc from the host document
        var doc;
        doc = frame.contentDocument;
        if (!doc) {
            doc = frame.contentWindow.document;
        }

        var mainButton = doc.getElementById('main_button');
        mainButton.style.display = 'none';
    }

    $scope.handleSvgData = function (data, error) {
        if (error) {
            alert('error ' + error);
        }
        else {
            alert('Congratulations. Your SVG string is back in the host page, do with it what you will\n\n' + data);
        }
    }

    $scope.loadSvg = function () {
        var svgexample = '<svg width="640" height="480" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns="http://www.w3.org/2000/svg"><g><title>Layer 1</title><rect stroke-width="5" stroke="#000000" fill="#FF0000" id="svg_1" height="35" width="51" y="35" x="32"/><ellipse ry="15" rx="24" stroke-width="5" stroke="#000000" fill="#0000ff" id="svg_2" cy="60" cx="66"/></g></svg>';
        svgCanvas.setSvgString(svgexample);
    }

    $scope.saveSvg = function () {
        svgCanvas.getSvgString()(handleSvgData);
    }
}
]);