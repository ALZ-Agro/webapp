(function (app) {
    'use strict';
    app.directive('ivhTreeviewCheckbox', ivhTreeviewCheckbox);
    function ivhTreeviewCheckbox($compile, ivhTreeviewMgr) {
        return {
            scope: false,
            template:
            [
            '<md-checkbox',
              'style="line-height: 0"',
              'aria-label="checked"',
                'ng-model="node.selected"',
              '>',
              '</md-checkbox>',
              '<span>{{length}}</span>'
            ].join('\n'),
            link: function (scope, element, attrs, trvw) {
                scope.setChildren = setChildren;
                element.on('click', function (event) {
                    setChildren(scope.node, scope.node.selected);
                    scope.trvw.onCbChange(scope.node, scope.node.selected);
                    scope.$apply();
                });
            }
        };
        function setChildren(node, selected) {
            if ('children' in node) {
                node.children.forEach(function (e) {
                    e.selected = selected;
                    setChildren(e, selected);
                });
            }
            node.selected = selected;
        }
    }
})(angular.module('common.ui'));