(function (app) {
    'use strict';

    app.directive('mvzTree', mvzTree);

    function mvzTree($compile) {

        var singleSelection = "";

        return {
            restrict: 'E', //attribute or element
            scope: {
                branch: '=',
                singleSelection: "@",
            },
            templateUrl: '/scripts/app/directives/controls/mvzTree.html',
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
            controller: function ($scope) {

                if ($scope.singleSelection) {
                    singleSelection = $scope.singleSelection;
                }

                $scope.toggle = function (root, setting, depth) {
                    if (!depth) {
                        depth = 0
                    }
                    if (setting == null || setting == undefined) {
                        setting = !root.isSelected;
                    }

                    if (setting && singleSelection) {
                        unckeckAll();
                        root.isSelected = setting;
                    }
                    else {
                        root.isSelected = setting;
                        //root.children.forEach(function (branch) {
                        //    $scope.toggle(branch, setting, depth + 1);
                        //});
                        if (depth == 0 && root.parent && setting) {
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
                    //var selected = false;
                    //root.children.forEach(function (branch) {
                    //    if (branch.isSelected) {
                    //        selected = true;
                    //    }
                    //});
                    item.isSelected = true;
                    if (item.parent) {
                        $scope.checkParent(item.parent);
                    }
                    //$scope.checkParent(root.parent);
                }

                function unckeckAll() {
                    if (singleSelection) {
                        $(".md-nav-item i.fa.fa-check-square-o").click();
                    }
                }
            }
        };
    };

})(angular.module('common.ui'));