'use strict';

// contains
function contains(arr, item) {
    if (arr && !angular.isArray(arr)) {
        arr = arr.split(',');
    }
    if (angular.isArray(arr)) {
        for (var i = 0; i < arr.length; i++) {
            if (angular.equals(arr[i].trim(), item)) {
                return true;
            }
        }
    }
    return false;
}


function formField(scope, Module, DDL, module, filterFilter) {
    var moduleNo = module.split(".")[0];
    var actionName = module.split(".")[1];
    scope.fields = [];
    scope.ddl = {};
    if (moduleNo) {
        var Field = Module.one(moduleNo).one("actions", actionName);

        scope.querySearch = function (source, search, selected) {
            var results = search ? filterFilter(source, search) : source;
            return results;
        }

        scope.files = { };

        scope.getModelParent = function (model, path) {
            var segs = path.split(".");
            var root = model;

            while (segs.length > 1) {
                var pathStep = segs.shift();
                if (typeof root[pathStep] === "undefined") {
                    root[pathStep] = {};
                }
                root = root[pathStep];
            }
            return root;
        };

        scope.getModelLeaf = function (path) {
            var segs = path.split(".");
            return segs[segs.length - 1];
        };

        Field.getList("Fields").then(function (data) {
            angular.forEach(data, function (d, i) {
                if (d.ShowIn && d.ShowIn.indexOf(Enums.fieldShowType.inForm) >= 0) {
                    scope.fields.push(d);
                    if ((d.Type == Enums.fieldType.select
                        || d.Type == Enums.fieldType.selectMulti
                        || d.Type == Enums.fieldType.checkList) && d.SourceName) {
                        scope.ddl[d.SourceName] = [];
                        DDL.one(d.SourceName).getList().then(function (data) {
                            scope.ddl[d.SourceName] = data;
                        });
                    }
                }
            });
        });
    };
}

//tableView
angular.module("EssApp").directive("tableView", ["Module",
    function (Module) {
        return {

            transclude: true,
            templateUrl: "/app/foundation/moduleConifg/tableView.html",
            controller: ["$scope", "$element", "$attrs",
                function ($scope, $element, $attrs) {
                    var module = $attrs["tableView"];
                    var moduleNo = module.split(".")[0];
                    var actionName = module.split(".")[1];
                    $scope.fields = [];
                    $scope.actions = [];
                    $scope.models = $scope[$attrs["models"]];
                    if (moduleNo) {
                        var module = Module.one(moduleNo);
                        module.one("actions", actionName).getList("Fields").then(function (data) {
                            angular.forEach(data, function (d, i) {
                                $scope.fields.push(d);
                            });
                        });
                        module.getList("Actions").then(function (data) {
                            angular.forEach(data, function (d, i) {
                                if (!d.IsBatch) { $scope.actions.push(d); }
                            });
                        });
                    }

                }]
        };
    }
])
    //listView
    .directive("listView", [
    function () {
        return {
            scope: {
                models: "=",
                selected: "=",
                icon: "@",
                title: "@",
                subTitle: "@",
                mode: "=",
                editable: "@"
            },
            transclude: true,
            templateUrl: "/app/foundation/moduleConifg/listView.html",
            controller: ["$scope", "$element", "$attrs",
                function ($scope, $element, $attrs) {
                    $scope.edit = function () {
                        $scope.mode = "edit";
                    }

                }]
        };
    }
    ])
    //formView
    .directive("formView", ["Module", "DDL",
    function (Module, DDL) {
        return {
            scope: {
                okButtonText: "@",
                saveClick: "&",
                model: "=",
                mode: "="
            },
            transclude: true,
            templateUrl: "/app/foundation/moduleConifg/formView.html",
            controller: ["$scope", "$element", "$attrs", "filterFilter",
                function ($scope, $element, $attrs, filterFilter) {
                    var module = $attrs["formView"];

                    formField($scope, Module, DDL, module, filterFilter);
                    $scope.contains = contains;
                    $scope.style = function () {
                        return {
                            height: ($scope.fields.length / 2 + 1) * 50 + "px"
                        };
                    };
                }]
        };
    }
    ])
    //formList
    .directive("formList", ["Module", "DDL",
    function (Module, DDL) {
        return {
            scope: {
                models: "="
            },
            transclude: true,
            templateUrl: "/app/foundation/moduleConifg/formList.html",
            controller: ["$scope", "$element", "$attrs", "filterFilter",
        function ($scope, $element, $attrs, filterFilter) {
            var module = $attrs["formList"];

            formField($scope, Module, DDL, module, filterFilter);
            $scope.del = function (index) {
                $scope.models.splice(index, 1);
            };
        }
            ]
        };
    }
    ])
//checklistModel
    .directive("checklistModel", ["$parse", "$compile", function ($parse, $compile) {

        // add
        function add(arr, item) {
            if (arr && !angular.isArray(arr)) {
                arr = arr.split(',');
            }
            arr = angular.isArray(arr) ? arr : [];
            for (var i = 0; i < arr.length; i++) {
                if (angular.equals(arr[i].trim(), item)) {
                    return arr.join();
                }
            }
            arr.push(item);
            return arr.join();
        }

        // remove
        function remove(arr, item) {
            if (arr && !angular.isArray(arr)) {
                arr = arr.split(',');
            }
            if (angular.isArray(arr)) {
                for (var i = 0; i < arr.length; i++) {
                    if (angular.equals(arr[i].trim(), item)) {
                        arr.splice(i, 1);
                        break;
                    }
                }
            }
            return arr.join();
        }

        // http://stackoverflow.com/a/19228302/1458162
        function postLinkFn(scope, elem, attrs) {
            // compile with `ng-model` pointing to `checked`
            $compile(elem)(scope);

            // getter / setter for original model
            var getter = $parse(attrs.checklistModel);
            var setter = getter.assign;

            // value added to list
            var value = $parse(attrs.checklistValue)(scope.$parent);

            // watch UI checked change
            scope.$watch("checked", function (newValue, oldValue) {
                if (newValue === oldValue) {
                    return;
                }
                var current = getter(scope.$parent);
                if (newValue === true) {
                    setter(scope.$parent, add(current, value));
                } else {
                    setter(scope.$parent, remove(current, value));
                }
            });

            // watch original model change
            scope.$parent.$watch(attrs.checklistModel, function (newArr, oldArr) {
                scope.checked = contains(newArr, value);
            }, true);
        }

        return {
            restrict: "A",
            priority: 1000,
            terminal: true,
            scope: true,
            compile: function (tElement, tAttrs) {
                if (tElement[0].tagName !== "INPUT" || tAttrs.type !== "checkbox") {
                    throw "checklist-model should be applied to `input[type=\"checkbox\"]`.";
                }

                if (!tAttrs.checklistValue) {
                    throw "You should provide `checklist-value`.";
                }

                // exclude recursion
                tElement.removeAttr("checklist-model");

                // local scope var storing individual checkbox model
                tElement.attr("ng-model", "checked");

                return postLinkFn;
            }
        };
    }])
    //toolbar
    .directive("toolbar", ["$compile",
    function ($compile) {
        var template = "<toolbar-button ng-repeat=\"action in actions\" ></toolbar-button>";

        return {
            replace: true,
            link: function (scope, element, attrs) {
                scope.$watch('actions', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var content = $compile(template)(scope);
                        element.append(content);
                    }
                }, true);
            }
        }
    }
    ])
    //toolbarButton
    .directive("toolbarButton", ["$compile",
    function ($compile) {
        return {

            replace: true,
            link: function (scope, element, attrs) {
                var template = "<a class=\"btn btn-xs default\" [Action]><i class=\"fa fa-{{action.Icon| default:'pencil'}}\"></i>{{action.Text}}</a>";

                if (scope.action.Type == Enums.actionType.link && scope.action.Action) {
                    template = template.replace("[Action]", "href=" + scope.action.Action);
                } else if (scope.action.Type == Enums.actionType.button && scope.action.Action) {
                    template = template.replace("[Action]", "ng-click=" + scope.action.Action);
                } else {
                    template = template.replace("[Action]", "");
                }
                var content = $compile(template)(scope);
                element.append(content);

            }
        }
    }
    ]);
