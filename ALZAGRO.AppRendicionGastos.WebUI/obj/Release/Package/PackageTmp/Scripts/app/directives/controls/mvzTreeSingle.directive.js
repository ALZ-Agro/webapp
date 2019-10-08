(function (app) {
    'use strict';

    app.directive('mvzTreeSingle', mvzTreeSingle);

    function mvzTreeSingle($compile, $timeout) {

        var singleSelection = "";

        return {
            restrict: 'E', //attribute or element
            scope: {
                branch: '='
            },
            templateUrl: '/scripts/app/directives/controls/mvzTreeSingle.html',
            compile: function (tElement, tAttr) {
                var contents = tElement.contents().remove();
                var compiledContents;
                return function (scope, iElement, iAttr) {
                    if (!compiledContents) {
                        compiledContents = $compile(contents);
                    }
                    compiledContents(scope, function (clone, scope) {
                        iElement.append(clone);
                    });
                };
            },
            controller: function ($scope, $timeout) {

                $scope.toggle = function (root, setting, depth) {
                    if (!depth) {
                        depth = 0
                    }
                    if (setting === null || setting === undefined) {
                        setting = !root.isSelected;
                    }

                    if (setting && true) {
                        unckeckAll(root, setting);
                        
                    }
                    else {
                        root.isSelected = setting;
                        if (depth === 0 && root.parent && setting) {
                            $scope.checkParent(root);
                        }
                    }
                }

                $scope.expand = function (root, setting) {
                    if (!setting) {
                        setting = !root.isExpanded;
                    }
                    root.isExpanded = setting;
                    root.children.forEach(function (branch) {
                        $scope.expand(branch, setting);
                    });
                }

                $scope.checkParent = function (item) {
                    item.isSelected = true;
                    if (item.parent) {
                        $scope.checkParent(item.parent);
                    }
                }

                function unckeckAll(root, setting) {
                    if (true) {
                        $timeout(function () {
                            $(".md-nav-item i.fa.fa-check-square-o").trigger('click');
                            root.isSelected = setting;
                        });
                    }
                }
            }
        };
    };

})(angular.module('common.ui'));