///#source 1 1 /app/app.module.js
"use strict";


// Declare app level module which depends on filters, and services
angular.module("EssApp", [
    "ngResource",
    "restangular",
    "ngRoute",
    "ngMaterial",
    "ngMessages",
    "ngSanitize",
    "ui.utils",
    "ui.tree",
    "LocalStorageModule"
]).run(["$rootScope", function ($rootScope) {
    $rootScope.pageTitle = "Title";
    $rootScope.pageSubTitle = "subTitle";
}]).config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push(["$q", function ($q) {
        return {
            responseError: function (rejection) {
                if (rejection.status == 500) {
                    alert(rejection.data.ExceptionMessage);
                } else if (rejection.status == 401) {
                    //�����ط�����ת
                } else {
                    alert("Error!");
                }
                return $q.reject(rejection);
            },
            response: function (response) {
                return response;
            }

        };
    }]);
    $httpProvider.interceptors.push("authInterceptorService");
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

///#source 1 1 /app/app.route.js
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
            .when("/user/add", { templateUrl: "/app/foundation/AccessControl/userEdit.html", controller: "UserController", })
            .when("/user/edit/:id", { templateUrl: "/app/foundation/AccessControl/userEdit.html", controller: "UserController", })

            //role
            .when("/role", { templateUrl: "/app/foundation/AccessControl/roleTable.html", controller: "RoleController" })
            .when("/role/add", { templateUrl: "/app/foundation/AccessControl/roleEdit.html", controller: "RoleController", })
            .when("/role/edit/:id", { templateUrl: "/app/foundation/AccessControl/roleEdit.html", controller: "RoleController", })
            .when("/help", { templateUrl: "/app/shared/help/help.html" })

            //common
            //category
            .when("/categoryType", { templateUrl: "/app/common/category/categoryType.html", controller: "CategoryController" })
            .when("/category", { templateUrl: "/app/common/category/category.html", controller: "CategoryController" })

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

///#source 1 1 /app/shared/common/tree-grid-directive.js
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
///#source 1 1 /app//shared/filters.js
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
}).filter('default', function () {
    return function (input,value) {
        if (angular.isUndefinedOrNull(input)) {
            return value;
        }
        return input;
    };
});

///#source 1 1 /app//shared/services.js
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
///#source 1 1 /app/shared/auth.service.js
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
///#source 1 1 /app/shared/common.directive.js
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

///#source 1 1 /app/shared/authInterceptor.service.js
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
///#source 1 1 /app/shared/header/header.service.js
'use strict';
angular.module('EssApp').factory('Menu', [
    'Restangular',
    function(Restangular) {
        var Menu = Restangular.service("Menu");
        return Menu;
    }
]);
///#source 1 1 /app/shared/header/header.controller.js
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
///#source 1 1 /app/foundation/moduleConifg/module.controller.js
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

        $scope.select = function() {

            var modalInstance = $mdDialog.open({
                templateUrl: '/app/foundation/moduleConifg/fieldSelect.html',
                controller: 'FieldSelectController',
            });

            modalInstance.result.then(function(selectedItem) {
                $scope.fields.push(selectedItem);
            });
        };

        $scope.save = function() {
            $scope.fields.post();
        };

    }
]).controller('FieldSelectController', [
    '$scope', '$mdDialogInstance', 'Fields', function($scope, $mdDialogInstance, Fields) {
        $scope.selected = {
            item: {}
        };
        Fields.getList().then(function(items) {
            $scope.items = items;
        });

        $scope.ok = function() {
            $mdDialogInstance.close($scope.selected.item);
        };

        $scope.cancel = function() {
            $mdDialogInstance.dismiss('cancel');
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
///#source 1 1 /app/foundation/moduleConifg/module.service.js
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
]);

var Enums = {};
$.getJSON("/api/enum/enums", function (data) {
    Enums = JSON.parse(data);
});
///#source 1 1 /app/foundation/moduleConifg/module.directive.js
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

function formField(scope, Module, DDL, module) {
    var moduleNo = module.split(".")[0];
    var actionName = module.split(".")[1];
    scope.fields = [];
    scope.ddl = {};
    if (moduleNo) {
        var Field = Module.one(moduleNo).one("actions", actionName);
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
                models:"=",
                selected: "=",
                icon: "@",
                title: "@",
                subTitle: "@",
                mode: "=",
                editable:"@"
            },
            transclude: true,
            templateUrl: "/app/foundation/moduleConifg/listView.html",
            controller: ["$scope", "$element", "$attrs",
                function ($scope, $element, $attrs) {
                    $scope.edit = function() {
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
                mode:"="
            },
            transclude: true,
            replace: true,
            templateUrl: "/app/foundation/moduleConifg/formView.html",
            controller: ["$scope", "$element", "$attrs",
                function ($scope, $element, $attrs) {
                    var module = $attrs["formView"];

                    formField($scope, Module, DDL, module);
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
            controller: ["$scope", "$element", "$attrs",
                function ($scope, $element, $attrs) {
                    var module = $attrs["formList"];

                    formField($scope, Module, DDL, module);
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

///#source 1 1 /app/foundation/accessControl/user.controller.js
"use strict";

angular.module("EssApp").controller("UserController", [
    "$scope", "User", "$routeParams","$timeout",
function ($scope, User, $routeParams, $timeout) {
    var fetchUsers = function () {
        $timeout(function() {
            User.getList().then(function(data) {
                $scope.users = data;
            });
        }, 100);
    };
    $scope.mode = "view";
    $scope.current = { item: {}};

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
            fetchUsers();
        }
    };

    $scope.changePassword = function () {
        User.changePassword($scope.current.item);
    };
}
]);
///#source 1 1 /app/foundation/accessControl/user.service.js
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
///#source 1 1 /app/foundation/accessControl/role.service.js
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
///#source 1 1 /app/foundation/accessControl/role.controller.js
'use strict';

angular.module('EssApp').controller('RoleController', [
    '$scope', 'toastr', 'Role',"User", '$routeParams',
function ($scope, toastr, Role, User, $routeParams) {

    var fetchRoles = function () {
        Role.getList().then(function (data) {
            $scope.roles = data;
        });
    };
    $scope.current.role = {};
    if ($routeParams.id) {
        Role.one($routeParams.id).get().then(function (role) {
            $scope.current.role = role;
        });
    } else {
        fetchRoles();
    }

    $scope.users = User.getList().$object;

    $scope.changeRoleInfo = function () {
        if ($routeParams.id) {
            Role.changeRoleInfo($scope.current.role);
        } else {
            Role.createRole($scope.current.role);
        }
    };

    $scope.assignUser = function() {
        Role.assignUser($scope.current.role);
    }

    $scope.lock = function(role) {
        Role.lock(role);
    }

    $scope.unlock = function (role) {
        Role.unlock(role);
    }
}
]);
///#source 1 1 /app/foundation/accessControl/login.controller.js
'use strict';
angular.module('EssApp').controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/user/list');

        });
    };

}]);
///#source 1 1 /app/common/category/category.service.js
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
///#source 1 1 /app/common/category/category.controller.js
"use strict";

angular.module("EssApp").controller("CategoryController", [
    "$scope", "CategoryTypeScheme", "CategoryType", "Category","$routeParams", "$timeout",
function ($scope, CategoryTypeScheme, CategoryType,Category, $routeParams, $timeout) {
    //#region CategoryTypeScheme
    var fetchCategoryTypeSchemes = function () {
        $timeout(function () {
            CategoryTypeScheme.getList().then(function (data) {
                $scope.categoryTypeSchemes = data;
            });
        }, 100);
    };
    fetchCategoryTypeSchemes();

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

            var index = $scope.categoryTypeSchemes.indexOf(scheme);
            $scope.categoryTypeSchemes.splice(index, 1);
        }
    }
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

    $scope.editCategoryType = function (type,scheme) {
        if (type.Id) {
            CategoryType.one(type.Id).doPUT(type);
        } else {
            type.SchemeId = scheme.Id;
            CategoryType.post(type);
            fetchCategoryTypes();
        }
        return true;
    };
    $scope.delCategoryType = function (type) {
        if (type.Id) {
            type.remove({ Id: type.Id });

            var index = $scope.categoryTypes.indexOf(type);
            $scope.categoryTypes.splice(index, 1);
        }
    }
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
            cat.TypeId = type.Id;
            Category.post(cat);
            fetchCategorys();
        }
        return true;
    };
    $scope.delCategory = function (cat) {
        if (cat.Id) {
            cat.remove({ Id: cat.Id });

            var index = $scope.categorys.indexOf();
            $scope.categorys.splice(index, 1);
        }
    }
    //#endregion
}
]);
///#source 1 1 /app/common/basic/basic.service.js
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
///#source 1 1 /app/common/basic/basic.controller.js
"use strict";

angular.module("EssApp");
///#source 1 1 /app/common/basic/floor.controller.js
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
///#source 1 1 /app/common/basic/brand.controller.js
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
///#source 1 1 /app/common/basic/brandType.controller.js
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
///#source 1 1 /app/common/basic/bank.controller.js
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
