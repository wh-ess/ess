﻿"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp", [
    "ngResource",
    "restangular",
    "ngRoute",
    "ngMaterial",
    "ngMessages",
    "ngSanitize",
    "ui.utils",
    "LocalStorageModule",
    "ui.bootstrap",
    "ngFileUpload"
]).run(["$rootScope", "$mdSidenav", "DDL", "authService", "Upload", function ($rootScope, $mdSidenav, DDL, authService, Upload) {
    $rootScope.pageTitle = "Index";
    $rootScope.pageSubTitle = "";

    $rootScope.auth = authService;
    $rootScope.auth.fillAuthData();
    $rootScope.$watch("auth.authentication.isAuth", function (newVal) {
        $rootScope.isAuth = newVal;

    });

    $rootScope.changeTitle = function (title, subTitle) {
        $rootScope.pageTitle = title;
        $rootScope.pageSubTitle = subTitle;
    };

    $rootScope.toggleSidenav = function (menuId) {
        $mdSidenav(menuId).toggle();
    };

    $rootScope.upload = function (files,model) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                Upload.upload({
                    url: 'api/upload',
                    file: file
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' + evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    model.Image = "upload/"+data[0];
                    console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                });
            }
        }
    };

    $rootScope.ddl = {};
    $rootScope.getDdl = function (key) {
        $rootScope.ddl[key] = [];
        DDL.one(key).getList().then(function (data) {
            $rootScope.ddl[key] = data;
            return data;
        });
    };

}]).config(["$httpProvider", function ($httpProvider) {

    $httpProvider.interceptors.push("authInterceptorService");
    $httpProvider.interceptors.push("errorInterceptorService");

}]).config(["RestangularProvider", function (RestangularProvider) {
    RestangularProvider.setBaseUrl("/api");
    // add a response intereceptor
    RestangularProvider.addResponseInterceptor(function (data, operation, what, url, response, deferred) {

        return data;
    });

}])
;


angular.isUndefinedOrNull = function (val) {
    return angular.isUndefined(val) || val === null;
}

"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp").run([
    '$rootScope', '$route', '$routeParams', '$location',
    function ($rootScope, $route, $routeParams, $location) {
        $rootScope.$route = $route;
        $rootScope.$location = $location;
        $rootScope.$routeParams = $routeParams;
    }
]).config([
    "$routeProvider", "$locationProvider", function route($routeProvider, $locationProvider) {
        $routeProvider.otherwise({ redirectTo: "/help" });
        //$locationProvider.html5Mode(true);

        $routeProvider

                //#region foundation
                //login
                .when("/login", { templateUrl: "/app/foundation/AccessControl/login.html", controller: "loginController" })

                //module
                .when("/module", { templateUrl: "/app/foundation/moduleConifg/moduleTable.html", controller: "ModuleController" })
                .when("/module/:moduleNo/actions", { templateUrl: "/app/foundation/moduleConifg/actionTable.html", controller: "ActionController" })
                .when("/module/:moduleNo/actions/:actionName/fields", { templateUrl: "/app/foundation/moduleConifg/fieldTable.html", controller: "FieldController" })

                //readModels
                .when("/readModels", { templateUrl: "/app/foundation/moduleConifg/readModels.html", controller: "ReadModelController" })

                //user
                .when("/user", { templateUrl: "/app/foundation/AccessControl/user.html", controller: "UserController" })
                .when("/user/edit/:id?", { templateUrl: "/app/foundation/AccessControl/userEdit.html", controller: "UserController" })

                //role
                .when("/role", { templateUrl: "/app/foundation/AccessControl/role.html", controller: "RoleController" })
                .when("/role/edit/:id?", { templateUrl: "/app/foundation/AccessControl/roleEdit.html", controller: "RoleController" })

                //help
                .when("/help", { templateUrl: "/app/shared/help/help.html" })

                //#endregion


                //#region common
                //svg
                .when("/svg/svgEditor", { templateUrl: "/app/common/svg/svgEditor.html", controller: "SvgEditorController" })

                //category
                .when("/categoryType", { templateUrl: "/app/common/category/categoryType.html", controller: "CategoryController" })

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

                //#endregion

                //#region mall
                //pop
                .when("/popTemplate", { templateUrl: "/app/mall/pop/popTemplate.html", controller: "PopTemplateController" })
                .when("/popTemplate/edit/:id?", { templateUrl: "/app/mall/pop/popTemplateEdit.html", controller: "PopTemplateController" })

        //#endregion
        ;
    }
]);

function getUrl(path) {
    return "/app/" + path.name + ".html";
}

function getController(path) {
    return path.name.substring(path.name.lastIndexOf("/") + 1) + "Controller";
}
(function () {
  angular
    .module('template/treeGrid/treeGrid.html', [])
    .run([
      '$templateCache',
      function ($templateCache) {
        $templateCache.put('template/treeGrid/treeGrid.html',
          "<div class=\"table-responsive\">\n" +
          " <table class=\"table tree-grid\">\n" +
          "   <thead>\n" +
          "     <tr>\n" +
          "       <th>{{expandingProperty.displayName || expandingProperty.field || expandingProperty}}</th>\n" +
          "       <th ng-repeat=\"col in colDefinitions\">{{col.displayName || col.field}}</th>\n" +
          "     </tr>\n" +
          "   </thead>\n" +
          "   <tbody>\n" +
          "     <tr ng-repeat=\"row in tree_rows | filter:{visible:true} track by row.branch.uid\"\n" +
          "       ng-class=\"'level-' + {{ row.level }} + (row.branch.selected ? ' active':'')\" class=\"tree-grid-row\">\n" +
          "       <td><a ng-click=\"user_clicks_branch(row.branch)\"><i ng-class=\"row.tree_icon\"\n" +
          "              ng-click=\"row.branch.expanded = !row.branch.expanded\"\n" +
          "              class=\"indented tree-icon\"></i>\n" +
          "           </a><span class=\"indented tree-label\" ng-click=\"on_user_click(row.branch)\">\n" +
          "             {{row.branch[expandingProperty.field] || row.branch[expandingProperty]}}</span>\n" +
          "       </td>\n" +
          "       <td ng-repeat=\"col in colDefinitions\">\n" +
          "         <div ng-if=\"col.cellTemplate\" compile=\"col.cellTemplate\"></div>\n" +
          "         <div ng-if=\"!col.cellTemplate\">{{row.branch[col.field]}}</div>\n" +
          "       </td>\n" +
          "     </tr>\n" +
          "   </tbody>\n" +
          " </table>\n" +
          "</div>\n" +
          "");
      }]);

  angular
    .module('treeGrid', [
      'template/treeGrid/treeGrid.html'
    ])

    .directive('compile', [
      '$compile',
      function ($compile) {
        return {
          restrict: 'A',
          link    : function (scope, element, attrs) {
            // Watch for changes to expression.
            scope.$watch(attrs.compile, function (new_val) {
              /*
               * Compile creates a linking function
               * that can be used with any scope.
               */
              var link = $compile(new_val);

              /*
               * Executing the linking function
               * creates a new element.
               */
              var new_elem = link(scope);

              // Which we can then append to our DOM element.
              element.append(new_elem);
            });
          }
        };
      }])

    .directive('treeGrid', [
      '$timeout',
      'treegridTemplate',
      function ($timeout,
                treegridTemplate) {

        return {
          restrict   : 'E',
          templateUrl: function (tElement, tAttrs) {
            return tAttrs.templateUrl || treegridTemplate.getPath();
          },
          replace    : true,
          scope      : {
            treeData        : '=',
            colDefs         : '=',
            expandOn        : '=',
            onSelect        : '&',
            onClick         : '&',
            initialSelection: '@',
            treeControl     : '='
          },
          link       : function (scope, element, attrs) {
            var error, expandingProperty, expand_all_parents, expand_level, for_all_ancestors, for_each_branch, get_parent, n, on_treeData_change, select_branch, selected_branch, tree;

            error = function (s) {
              console.log('ERROR:' + s);
              debugger;
              return void 0;
            };

            attrs.iconExpand = attrs.iconExpand ? attrs.iconExpand : 'icon-plus  glyphicon glyphicon-plus  fa fa-plus';
            attrs.iconCollapse = attrs.iconCollapse ? attrs.iconCollapse : 'icon-minus glyphicon glyphicon-minus fa fa-minus';
            attrs.iconLeaf = attrs.iconLeaf ? attrs.iconLeaf : 'icon-file  glyphicon glyphicon-file  fa fa-file';
            attrs.expandLevel = attrs.expandLevel ? attrs.expandLevel : '3';
            expand_level = parseInt(attrs.expandLevel, 10);

            if (!scope.treeData) {
              alert('No data was defined for the tree, please define treeData!');
              return;
            }

            var getExpandingProperty = function getExpandingProperty() {
              if (attrs.expandOn) {
                expandingProperty = scope.expandOn;
                scope.expandingProperty = scope.expandOn;
              } else {
                if (scope.treeData.length) {
                  var _firstRow = scope.treeData[0],
                    _keys = Object.keys(_firstRow);
                  for (var i = 0, len = _keys.length; i < len; i++) {
                    if (typeof (_firstRow[_keys[i]]) === 'string') {
                      expandingProperty = _keys[i];
                      break;
                    }
                  }
                  if (!expandingProperty) expandingProperty = _keys[0];
                  scope.expandingProperty = expandingProperty;
                }
              }
            };

            getExpandingProperty();

            if (!attrs.colDefs) {
              if (scope.treeData.length) {
                var _col_defs = [],
                  _firstRow = scope.treeData[0],
                  _unwantedColumn = ['children', 'level', 'expanded', expandingProperty];
                for (var idx in _firstRow) {
                  if (_unwantedColumn.indexOf(idx) === -1) {
                    _col_defs.push({
                      field: idx
                    });
                  }
                }
                scope.colDefinitions = _col_defs;
              }
            } else {
              scope.colDefinitions = scope.colDefs;
            }

            for_each_branch = function (f) {
              var do_f, root_branch, _i, _len, _ref, _results;
              do_f = function (branch, level) {
                var child, _i, _len, _ref, _results;
                f(branch, level);
                if (branch.children != null) {
                  _ref = branch.children;
                  _results = [];
                  for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                    child = _ref[_i];
                    _results.push(do_f(child, level + 1));
                  }
                  return _results;
                }
              };
              _ref = scope.treeData;
              _results = [];
              for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                root_branch = _ref[_i];
                _results.push(do_f(root_branch, 1));
              }
              return _results;
            };
            selected_branch = null;
            select_branch = function (branch) {
              if (!branch) {
                if (selected_branch != null) {
                  selected_branch.selected = false;
                }
                selected_branch = null;
                return;
              }
              if (branch !== selected_branch) {
                if (selected_branch != null) {
                  selected_branch.selected = false;
                }
                branch.selected = true;
                selected_branch = branch;
                expand_all_parents(branch);
                if (branch.onSelect != null) {
                  return $timeout(function () {
                    return branch.onSelect(branch);
                  });
                } else {
                  if (scope.onSelect != null) {
                    return $timeout(function () {
                      return scope.onSelect({
                        branch: branch
                      });
                    });
                  }
                }
              }
            };
            scope.on_user_click = function (branch) {
              if (scope.onClick) {
                scope.onClick({
                  branch: branch
                });
              }
            };
            scope.user_clicks_branch = function (branch) {
              if (branch !== selected_branch) {
                return select_branch(branch);
              }
            };
            get_parent = function (child) {
              var parent;
              parent = void 0;
              if (child.parent_uid) {
                for_each_branch(function (b) {
                  if (b.uid === child.parent_uid) {
                    return parent = b;
                  }
                });
              }
              return parent;
            };
            for_all_ancestors = function (child, fn) {
              var parent;
              parent = get_parent(child);
              if (parent != null) {
                fn(parent);
                return for_all_ancestors(parent, fn);
              }
            };
            expand_all_parents = function (child) {
              return for_all_ancestors(child, function (b) {
                return b.expanded = true;
              });
            };

            scope.tree_rows = [];

            on_treeData_change = function () {
              getExpandingProperty();

              var add_branch_to_list, root_branch, _i, _len, _ref, _results;
              for_each_branch(function (b, level) {
                if (!b.uid) {
                  return b.uid = "" + Math.random();
                }
              });
              for_each_branch(function (b) {
                var child, _i, _len, _ref, _results;
                if (angular.isArray(b.children)) {
                  _ref = b.children;
                  _results = [];
                  for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                    child = _ref[_i];
                    _results.push(child.parent_uid = b.uid);
                  }
                  return _results;
                }
              });
              scope.tree_rows = [];
              for_each_branch(function (branch) {
                var child, f;
                if (branch.children) {
                  if (branch.children.length > 0) {
                    f = function (e) {
                      if (typeof e === 'string') {
                        return {
                          label   : e,
                          children: []
                        };
                      } else {
                        return e;
                      }
                    };
                    return branch.children = (function () {
                      var _i, _len, _ref, _results;
                      _ref = branch.children;
                      _results = [];
                      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                        child = _ref[_i];
                        _results.push(f(child));
                      }
                      return _results;
                    })();
                  }
                } else {
                  return branch.children = [];
                }
              });
              add_branch_to_list = function (level, branch, visible) {
                var child, child_visible, tree_icon, _i, _len, _ref, _results;
                if (branch.expanded == null) {
                  branch.expanded = false;
                }
                if (!branch.children || branch.children.length === 0) {
                  tree_icon = attrs.iconLeaf;
                } else {
                  if (branch.expanded) {
                    tree_icon = attrs.iconCollapse;
                  } else {
                    tree_icon = attrs.iconExpand;
                  }
                }
                branch.level = level;
                scope.tree_rows.push({
                  level    : level,
                  branch   : branch,
                  label    : branch[expandingProperty],
                  tree_icon: tree_icon,
                  visible  : visible
                });
                if (branch.children != null) {
                  _ref = branch.children;
                  _results = [];
                  for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                    child = _ref[_i];
                    child_visible = visible && branch.expanded;
                    _results.push(add_branch_to_list(level + 1, child, child_visible));
                  }
                  return _results;
                }
              };
              _ref = scope.treeData;
              _results = [];
              for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                root_branch = _ref[_i];
                _results.push(add_branch_to_list(1, root_branch, true));
              }
              return _results;
            };

            scope.$watch('treeData', on_treeData_change, true);

            if (attrs.initialSelection != null) {
              for_each_branch(function (b) {
                if (b.label === attrs.initialSelection) {
                  return $timeout(function () {
                    return select_branch(b);
                  });
                }
              });
            }
            n = scope.treeData.length;
            for_each_branch(function (b, level) {
              b.level = level;
              return b.expanded = b.level < expand_level;
            });
            if (scope.treeControl != null) {
              if (angular.isObject(scope.treeControl)) {
                tree = scope.treeControl;
                tree.expand_all = function () {
                  return for_each_branch(function (b, level) {
                    return b.expanded = true;
                  });
                };
                tree.collapse_all = function () {
                  return for_each_branch(function (b, level) {
                    return b.expanded = false;
                  });
                };
                tree.get_first_branch = function () {
                  n = scope.treeData.length;
                  if (n > 0) {
                    return scope.treeData[0];
                  }
                };
                tree.select_first_branch = function () {
                  var b;
                  b = tree.get_first_branch();
                  return tree.select_branch(b);
                };
                tree.get_selected_branch = function () {
                  return selected_branch;
                };
                tree.get_parent_branch = function (b) {
                  return get_parent(b);
                };
                tree.select_branch = function (b) {
                  select_branch(b);
                  return b;
                };
                tree.get_children = function (b) {
                  return b.children;
                };
                tree.select_parent_branch = function (b) {
                  var p;
                  if (b == null) {
                    b = tree.get_selected_branch();
                  }
                  if (b != null) {
                    p = tree.get_parent_branch(b);
                    if (p != null) {
                      tree.select_branch(p);
                      return p;
                    }
                  }
                };
                tree.add_branch = function (parent, new_branch) {
                  if (parent != null) {
                    parent.children.push(new_branch);
                    parent.expanded = true;
                  } else {
                    scope.treeData.push(new_branch);
                  }
                  return new_branch;
                };
                tree.add_root_branch = function (new_branch) {
                  tree.add_branch(null, new_branch);
                  return new_branch;
                };
                tree.expand_branch = function (b) {
                  if (b == null) {
                    b = tree.get_selected_branch();
                  }
                  if (b != null) {
                    b.expanded = true;
                    return b;
                  }
                };
                tree.collapse_branch = function (b) {
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    b.expanded = false;
                    return b;
                  }
                };
                tree.get_siblings = function (b) {
                  var p, siblings;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    p = tree.get_parent_branch(b);
                    if (p) {
                      siblings = p.children;
                    } else {
                      siblings = scope.treeData;
                    }
                    return siblings;
                  }
                };
                tree.get_next_sibling = function (b) {
                  var i, siblings;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    siblings = tree.get_siblings(b);
                    n = siblings.length;
                    i = siblings.indexOf(b);
                    if (i < n) {
                      return siblings[i + 1];
                    }
                  }
                };
                tree.get_prev_sibling = function (b) {
                  var i, siblings;
                  if (b == null) {
                    b = selected_branch;
                  }
                  siblings = tree.get_siblings(b);
                  n = siblings.length;
                  i = siblings.indexOf(b);
                  if (i > 0) {
                    return siblings[i - 1];
                  }
                };
                tree.select_next_sibling = function (b) {
                  var next;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    next = tree.get_next_sibling(b);
                    if (next != null) {
                      return tree.select_branch(next);
                    }
                  }
                };
                tree.select_prev_sibling = function (b) {
                  var prev;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    prev = tree.get_prev_sibling(b);
                    if (prev != null) {
                      return tree.select_branch(prev);
                    }
                  }
                };
                tree.get_first_child = function (b) {
                  var _ref;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    if (((_ref = b.children) != null ? _ref.length : void 0) > 0) {
                      return b.children[0];
                    }
                  }
                };
                tree.get_closest_ancestor_next_sibling = function (b) {
                  var next, parent;
                  next = tree.get_next_sibling(b);
                  if (next != null) {
                    return next;
                  } else {
                    parent = tree.get_parent_branch(b);
                    return tree.get_closest_ancestor_next_sibling(parent);
                  }
                };
                tree.get_next_branch = function (b) {
                  var next;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    next = tree.get_first_child(b);
                    if (next != null) {
                      return next;
                    } else {
                      next = tree.get_closest_ancestor_next_sibling(b);
                      return next;
                    }
                  }
                };
                tree.select_next_branch = function (b) {
                  var next;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    next = tree.get_next_branch(b);
                    if (next != null) {
                      tree.select_branch(next);
                      return next;
                    }
                  }
                };
                tree.last_descendant = function (b) {
                  var last_child;
                  if (b == null) {
                    debugger;
                  }
                  n = b.children.length;
                  if (n === 0) {
                    return b;
                  } else {
                    last_child = b.children[n - 1];
                    return tree.last_descendant(last_child);
                  }
                };
                tree.get_prev_branch = function (b) {
                  var parent, prev_sibling;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    prev_sibling = tree.get_prev_sibling(b);
                    if (prev_sibling != null) {
                      return tree.last_descendant(prev_sibling);
                    } else {
                      parent = tree.get_parent_branch(b);
                      return parent;
                    }
                  }
                };
                return tree.select_prev_branch = function (b) {
                  var prev;
                  if (b == null) {
                    b = selected_branch;
                  }
                  if (b != null) {
                    prev = tree.get_prev_branch(b);
                    if (prev != null) {
                      tree.select_branch(prev);
                      return prev;
                    }
                  }
                };
              }
            }
          }
        };
      }
    ])

    .provider('treegridTemplate', function () {
      var templatePath = 'template/treeGrid/treeGrid.html';

      this.setPath = function (path) {
        templatePath = path;
      };

      this.$get = function () {
        return {
          getPath: function () {
            return templatePath;
          }
        };
      };
    });
}).call(window);
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
'use strict';
angular.module("EssApp").factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serviceBase = "/";
    var clientId = "ESS";
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        //if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + clientId;
        //}

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            }
            else {
                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
            }
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            _authentication.useRefreshTokens = loginData.useRefreshTokens;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
        }

    };

    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}]);
'use strict';


angular.module('EssApp').directive('back', ['$window', function ($window) {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            elem.bind('click', function () {
                $window.history.back();
            });
        }
    };
}]);;

'use strict';
angular.module("EssApp").factory('authInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

    var authInterceptorServiceFactory = {};

    var _request = function(config) {

        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    };

    var _responseError = function(rejection) {
        if (rejection.status === 401) {
            $location.path('/login');
        }
        return $q.reject(rejection);
    };

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);
'use strict';
angular.module("EssApp").factory('errorInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

    var errorInterceptorServiceFactory = {};
    
    var _responseError = function (rejection) {
        if (rejection.status === 500) {
            alert(rejection.data.ExceptionMessage);
        }
        else if (rejection.status === 0) {
            alert("offline");
            //offline
        } else if (rejection.status === 401) {
            //其它地方会跳转
        } else {
            alert("Error!");
        }
        return $q.reject(rejection);
    }

    errorInterceptorServiceFactory.responseError = _responseError;

    return errorInterceptorServiceFactory;
}]);
'use strict';
angular.module('EssApp').factory('Menu', [
    'Restangular',
    function(Restangular) {
        var Menu = Restangular.service("Menu");
        return Menu;
    }
]);
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
]).controller("ReadModelController", [
    "$scope", "ReadModel",
    function ($scope, ReadModel) {
        ReadModel.getList().then(function (items) {
            $scope.readModels = items;
        });
        $scope.rebuild = function (id) {
            ReadModel.one(id).post();
        };

        $scope.getData=function(id) {
            ReadModel.one(id).getList().then(function (items) {
                $scope.items = items;
            });
        }
    }
]);
'use strict';
angular.module('EssApp').factory('Module', ['Restangular',
    function (Restangular) {
        var Module = Restangular.service("Module");
        return Module;
    }
]).factory('Fields', ['Restangular',
    function (Restangular) {
        var Fields = Restangular.service("Fields");
        return Fields;
    }
]).factory('Entity', ['Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Entity");
        return Entity;
    }
]).factory('DDL', ['Restangular',
    function (Restangular) {
        var DDL = Restangular.service("Ddl");
        return DDL;
    }
]).factory('ReadModel', ['Restangular',
    function (Restangular) {
        var DDL = Restangular.service("ReadModel");
        return DDL;
    }
]);

var Enums = {};
$.getJSON("/api/enum/enums", function (data) {
    Enums = JSON.parse(data);
});
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

"use strict";

angular.module("EssApp").controller("UserController", [
    "$scope", "User", "$routeParams", "$timeout",
function ($scope, User, $routeParams, $timeout) {
    var fetchUsers = function () {
        $timeout(function () {
            User.getList().then(function (data) {
                $scope.users = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        User.one($routeParams.id).get().then(function (user) {
            $scope.current.item = user;
        });
    } else {
        fetchUsers();
    }

    $scope.addUser = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.changeUserInfo = function () {
        if ($scope.current.item.Id) {
            User.changeUserInfo($scope.current.item);
        } else {
            User.createUser($scope.current.item);
        }
        fetchUsers();
    };

    $scope.changePassword = function () {
        User.changePassword($scope.current.item);
    };
    $scope.lock = function (user) {
        User.lock(user);
        user.Locked = true;
    }

    $scope.unlock = function (user) {
        User.unlock(user);
        user.Locked = false;
    }
}
]);
'use strict';

angular.module('EssApp').factory('User', ['Restangular',
    function (Restangular) {

        var User = Restangular.service("User");

        User.createUser = function(user) {
            User.post(user);
        };

        User.changeUserInfo = function(user) {
            User.one(user.Id).doPUT(user);
        };

        User.changePassword = function (user) {
            User.one(user.Id).doPOST(user, "ChangePassword");
        };
        User.lock = function (user) {
            User.one(user.Id).doPOST(user, "Lock");
        };
        User.unlock = function (user) {
            User.one(user.Id).doPOST(user, "Unlock");
        };
        return User;
    }
]);
'use strict';

angular.module('EssApp').factory('Role', ['Restangular',
    function (Restangular) {

        var Role = Restangular.service("Role");

        Role.createRole = function(role) {
            Role.post(role);
        };

        Role.changeRoleInfo = function(role) {
            Role.one(role.Id).doPUT(role);
        };

        Role.assignUser = function (role) {
            Role.one(role.Id).doPOST(role, "AssignUser");
        };

        Role.lock = function (role) {
            role.Locked = true;
            Role.one(role.Id).doPOST(role, "Lock");
        };
        Role.unlock = function (role) {
            role.Locked = false;
            Role.one(role.Id).doPOST(role, "Unlock");
        };
        return Role;
    }
]);
'use strict';

angular.module('EssApp').controller('RoleController', [
    '$scope', 'Role', "User", '$routeParams',
function ($scope, Role, User, $routeParams) {

    var fetchRoles = function () {
        Role.getList().then(function (data) {
            $scope.roles = data;
        });
    };
    $scope.current = { item: {} };
    if ($routeParams.id) {
        Role.one($routeParams.id).get().then(function (role) {
            $scope.current.item = role;
        });
    } else {
        fetchRoles();
    }

    $scope.ddl["Users"] = User.getList().$object;

    $scope.addRole = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.changeRoleInfo = function (role) {
        if (role.id) {
            Role.changeRoleInfo(role);
        } else {
            Role.createRole(role);
        }
        fetchRoles();
    };

    $scope.assignUser = function (role) {
        Role.assignUser(role);
    }

    $scope.lock = function (role) {
        Role.lock(role);
    }

    $scope.unlock = function (role) {
        Role.unlock(role);
    }
}
]);
'use strict';
angular.module('EssApp').controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/');

        });
    };

}]);
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
"use strict";
angular.module("EssApp").factory("CategoryTypeScheme", [
    "Restangular",
    function (Restangular) {
        var Module = Restangular.service("CategoryTypeScheme");
        return Module;
    }
]).factory("CategoryType", [
    "Restangular",
    function (Restangular) {
        var Fields = Restangular.service("CategoryType");
        return Fields;
    }
]).factory("Category", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Category");
        return Entity;
    }
]);
"use strict";

angular.module("EssApp").controller("CategoryController", [
    "$scope", "CategoryTypeScheme", "CategoryType", "Category", "$routeParams", "$timeout",
function ($scope, CategoryTypeScheme, CategoryType, Category, $routeParams, $timeout) {
    $scope.cur = {
        type: {},
        scheme: {}
    };
    //#region CategoryTypeScheme
    var fetchCategoryTypeSchemes = function () {
        $timeout(function () {
            CategoryTypeScheme.getList().then(function (data) {
                $scope.categoryTypeSchemes = data;
            });
        }, 100);
    };
    fetchCategoryTypeSchemes();

    $scope.categoryTypeScheme = [{Name:"fdsfs"}, {Name:"fdsfds"}];

    $scope.editCategoryTypeScheme = function (scheme) {
        if (scheme.Id) {
            CategoryTypeScheme.one(scheme.Id).doPUT(scheme);
        } else {
            CategoryTypeScheme.post(scheme);
            fetchCategoryTypeSchemes();
        }
        return true;
    };
    $scope.delCategoryTypeScheme = function (scheme) {
        if (scheme.Id) {
            scheme.remove({ Id: scheme.Id });

        }
        var index = $scope.categoryTypeSchemes.indexOf(scheme);
        $scope.categoryTypeSchemes.splice(index, 1);
    };
    //#endregion

    //#region CategoryType
    var fetchCategoryTypes = function () {
        $timeout(function () {
            CategoryType.getList().then(function (data) {
                $scope.categoryTypes = data;
            });
        }, 100);
    };

    fetchCategoryTypes();

    $scope.editCategoryType = function (type, scheme) {
        type.Scheme = scheme;

        if (type.Id) {
            CategoryType.one(type.Id).doPUT(type);
        } else {
            CategoryType.post(type);
            fetchCategoryTypes();
        }
        return true;
    };
    $scope.editCategoryTypeWithParent = function (type, parent, scheme) {
        type.ParentId = parent.Id;
        type.Scheme = scheme;

        if (type.Id) {
            CategoryType.one(type.Id).doPUT(type);
        } else {
            CategoryType.post(type);
            fetchCategoryTypes();
        }
        return true;
    };
    $scope.delCategoryType = function (type) {
        if (type.Id) {
            type.remove({ Id: type.Id });

        }
        var index = $scope.categoryTypes.indexOf(type);
        $scope.categoryTypes.splice(index, 1);
    };
    //#endregion

    //#region Category
    var fetchCategorys = function () {
        $timeout(function () {
            Category.getList().then(function (data) {
                $scope.categorys = data;
            });
        }, 100);
    };

    fetchCategorys();

    $scope.editCategory = function (cat, type) {
        if (cat.Id) {
            Category.one(cat.Id).doPUT();
        } else {
            cat.Type = type;
            Category.post(cat);
            fetchCategorys();
        }
        return true;
    };
    $scope.delCategory = function (cat) {
        if (cat.Id) {
            cat.remove({ Id: cat.Id });
        }

        var index = $scope.categorys.indexOf();
        $scope.categorys.splice(index, 1);
    };
    //#endregion
}
]);
"use strict";
angular.module("EssApp").factory("Association", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Association");
        return Entity;
    }
]);
"use strict";

angular.module("EssApp").controller("AssociationController", [
    "$scope", "Association", "CategoryTypeScheme", "Category", "$routeParams", "$timeout",
function ($scope, Association, CategoryTypeScheme, Category, $routeParams, $timeout) {

    CategoryTypeScheme.one("Association").one("CategoryType", "Brand").getList().then(function (data) {
        $scope.types = data;
    });

    Category.getList().then(function (data) {
        $scope.categorys = data;
    });

    $scope.rules = $scope.$parent.getDdl("AssociationRule");

    //#region Association
    var fetchAssociations = function () {
        $timeout(function () {
            Association.getList().then(function (data) {
                $scope.Associations = data;
            });
        }, 100);
    };

    fetchAssociations();
    $scope.addAssociation = function () {
        $scope.Associations.push({});
    };
    $scope.saveAssociation = function (a, type) {
        if (a.Id) {
            Association.one(a.Id).doPUT(a);
        } else {
            a.Type = type;
            Association.post(a);
            fetchAssociations();
        }
        return true;
    };
    $scope.delAssociation = function (a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.Associations.indexOf();
        $scope.Associations.splice(index, 1);
    };
    //#endregion
}
]);
"use strict";
angular.module("EssApp").factory("Party", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("Party");
        return Entity;
    }
]).factory("PartyRole", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("PartyRole");
        return Entity;
    }
]);
"use strict";

angular.module("EssApp").controller("PartyRoleController", [
    "$scope", "PartyRole", "CategoryTypeScheme", "Category", "$routeParams", "$timeout", "$mdDialog",
function ($scope, PartyRole, CategoryTypeScheme, Category, $routeParams, $timeout, $mdDialog) {

    CategoryTypeScheme.one("PartyRole").one("CategoryType", "PartyRole").getList().then(function (data) {
        $scope.types = data;
    });

    //#region PartyRole
    var fetchPartyRoles = function () {
        $timeout(function () {
            PartyRole.getList().then(function (data) {
                $scope.partyRoles = data;
            });
        }, 100);
    };

    $scope.cur = { item: {} };
    fetchPartyRoles();

    $scope.addPartyRole = function (ev) {
        $mdDialog.show({
            templateUrl: "/app/foundation/moduleConifg/fieldSelect.html",
            controller: "PartySelectController",
            targetEvent: ev
        }).then(function (item) {
            var p = {
                Party: item
            }
            $scope.cur = { item: p };
        });
    };


    $scope.savePartyRole = function (a, type) {
        a.TypeId = type.Id;
        if (a.Id) {
            PartyRole.one(a.Id).doPUT(a);
        } else {
            PartyRole.post(a);
            fetchPartyRoles();
        }
        return true;
    };
    $scope.delPartyRole = function (a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.partyRoles.indexOf();
        $scope.partyRoles.splice(index, 1);
    };
    //#endregion
}
]).controller("PartySelectController", ["$scope", "$mdDialog", "Party", function ($scope, $mdDialog, Party) {
    $scope.selected = {
        item: {}
    };
    Party.getList().then(function (items) {
        $scope.items = items;
    });

    $scope.ok = function () {
        $mdDialog.hide($scope.selected.item);
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

}]);
"use strict";

angular.module("EssApp").controller("PartyController", [
    "$scope", "Party", "CategoryTypeScheme", "Category", "$routeParams", "$timeout",
function ($scope, Party, CategoryTypeScheme, Category, $routeParams, $timeout) {

    CategoryTypeScheme.one("PartyRole").one("CategoryType", "Party").getList().then(function (data) {
        $scope.types = data;
    });
    
    //#region Party
    var fetchPartys = function () {
        $timeout(function () {
            Party.getList().then(function (data) {
                $scope.partys = data;
            });
        }, 100);
    };

    $scope.cur = { item:{ }};
    fetchPartys();
    $scope.addParty = function () {
        $scope.cur = { item: {} };
    };
    $scope.saveParty = function (a, type) {
        if (a.Id) {
            Party.one(a.Id).doPUT(a);
        } else {
            a.TypeId = type.Id;
            Party.post(a);
            fetchPartys();
        }
        return true;
    };
    $scope.delParty = function(a) {
        if (a.Id) {
            a.remove({ Id: a.Id });
        }
        var index = $scope.partys.indexOf();
        $scope.partys.splice(index, 1);
    };
    //#endregion
}
]);
'use strict';
angular.module('EssApp').factory('Brand', [
    'Restangular',
    function (Restangular) {
        var Module = Restangular.service("Brand");
        return Module;
    }
]).factory('BrandType', [
    'Restangular',
    function (Restangular) {
        var Fields = Restangular.service("BrandType");
        return Fields;
    }
]).factory('Floor', [
    'Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Floor");
        return Entity;
    }
]).factory('Bank', [
    'Restangular',
    function (Restangular) {
        var Entity = Restangular.service("Bank");
        return Entity;
    }
]);
"use strict";

angular.module("EssApp");
"use strict";

angular.module("EssApp").controller("FloorController", [
    "$scope", "Floor", "$routeParams", "$timeout",
function ($scope, Floor, $routeParams, $timeout) {
    var fetchFloors = function () {
        $timeout(function() {
            Floor.getList().then(function (data) {
                $scope.floors = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Floor.one($routeParams.id).get().then(function (floor) {
            $scope.current.item = floor;
        });
    } else {
        fetchFloors();
    }

    $scope.addFloor = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editFloor = function () {
        if ($scope.current.item.Id) {
            Floor.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Floor.post($scope.current.item);
            fetchFloors();
        }
    };

}
]);
"use strict";

angular.module("EssApp").controller("BrandController", [
    "$scope", "Brand", "$routeParams", "$timeout",
function ($scope, Brand, $routeParams, $timeout) {
    var fetchBrands = function () {
        $timeout(function() {
            Brand.getList().then(function (data) {
                $scope.brands = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Brand.one($routeParams.id).get().then(function (brand) {
            $scope.current.item = brand;
        });
    } else {
        fetchBrands();
    }

    $scope.addBrand = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBrand = function () {
        if ($scope.current.item.Id) {
            Brand.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Brand.post($scope.current.item);
            fetchBrands();
        }
    };

}
]);
"use strict";

angular.module("EssApp").controller("BrandTypeController", [
    "$scope", "BrandType", "$routeParams", "$timeout",
function ($scope, BrandType, $routeParams, $timeout) {
    var fetchBrandTypes = function () {
        $timeout(function() {
            BrandType.getList().then(function (data) {
                $scope.brandTypes = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        BrandType.one($routeParams.id).get().then(function (brandType) {
            $scope.current.item = brandType;
        });
    } else {
        fetchBrandTypes();
    }

    $scope.addBrandType = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBrandType = function () {
        if ($scope.current.item.Id) {
            BrandType.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            BrandType.post($scope.current.item);
            fetchBrandTypes();
        }
    };

}
]);
"use strict";

angular.module("EssApp").controller("BankController", [
    "$scope", "Bank", "$routeParams", "$timeout",
function ($scope, Bank, $routeParams, $timeout) {
    var fetchBanks = function () {
        $timeout(function() {
            Bank.getList().then(function (data) {
                $scope.banks = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {} };

    if ($routeParams.id) {
        Bank.one($routeParams.id).get().then(function (bank) {
            $scope.current.item = bank;
        });
    } else {
        fetchBanks();
    }

    $scope.addBank = function () {
        $scope.mode = "edit";
        $scope.current.item = {};
    }

    $scope.editBank = function () {
        if ($scope.current.item.Id) {
            Bank.one($scope.current.item.Id).doPUT($scope.current.item);
        } else {
            Bank.post($scope.current.item);
            fetchBanks();
        }
    };

}
]);
"use strict";
angular.module("EssApp").factory("PopTemplate", [
    "Restangular",
    function (Restangular) {
        var Entity = Restangular.service("PopTemplate");
        return Entity;
    }
]);
"use strict";

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
