'use strict';

angular.module('EssApp').
    controller('MenuController', [
        '$scope', 'Menu', function ($scope, Menu) {
            Menu.getList().then(function (data) {
                $scope.menuTable = data;
                $scope.menus = arrayToTree(data, "ModuleNo", "ParentModuleNo");
            });
        }
    ]);

    
function arrayToTree(data, id, pid)      //将ID、ParentID这种数据格式转换为树格式
{
    if (!data || !data.length) return [];
    var targetData = [];                    //存储数据的容器(返回) 
    var records = {};
    var itemLength = data.length;           //数据集合的个数
    for (var i = 0; i < itemLength; i++) {
        var o = data[i];
        records[o[id]] = o;
    }
    for (var i = 0; i < itemLength; i++) {
        var currentData = data[i];
        var parentData = records[currentData[pid]];
        if (!parentData) {
            targetData.push(currentData);
            continue;
        }
        parentData.children = parentData.children || [];
        parentData.children.push(currentData);
    }
    return targetData;
}