/* To avoid CSS expressions while still supporting IE 7 and IE 6, use this script */
/* The script tag referencing this file must be placed before the ending body tag. */

/* Use conditional comments in order to target IE 7 and older:
	<!--[if lt IE 8]><!-->
	<script src="ie7/ie7.js"></script>
	<!--<![endif]-->
*/

(function() {
	function addIcon(el, entity) {
		var html = el.innerHTML;
		el.innerHTML = '<span style="font-family: \'AlzAgro-Icons-Font\'">' + entity + '</span>' + html;
	}
	var icons = {
		'alz-rotate-image': '&#xe900;',
		'alz-seed': '&#xe901;',
		'alz-solidum': '&#xe902;',
		'alz-switch-camera': '&#xe903;',
		'alz-angle-left': '&#xe904;',
		'alz-angle-right': '&#xe905;',
		'alz-arrow-left': '&#xe906;',
		'alz-arrow-right': '&#xe907;',
		'alz-bell': '&#xe908;',
		'alz-bell-alt': '&#xe909;',
		'alz-bolt': '&#xe90a;',
		'alz-camera': '&#xe90b;',
		'alz-cancel': '&#xe90c;',
		'alz-cancel-circle': '&#xe90d;',
		'alz-caret-down': '&#xe90e;',
		'alz-caret-up': '&#xe90f;',
		'alz-chart-bar': '&#xe910;',
		'alz-checksquare': '&#xe911;',
		'alz-check-circle': '&#xe912;',
		'alz-coffee': '&#xe913;',
		'alz-coins': '&#xe914;',
		'alz-compress': '&#xe915;',
		'alz-expand': '&#xe916;',
		'alz-folder': '&#xe917;',
		'alz-folder-open': '&#xe918;',
		'alz-gas-pump': '&#xe919;',
		'alz-home': '&#xe91a;',
		'alz-hotel': '&#xe91b;',
		'alz-images': '&#xe91c;',
		'alz-menu': '&#xe91d;',
		'alz-more': '&#xe91e;',
		'alz-plus': '&#xe91f;',
		'alz-plus-circle': '&#xe920;',
		'alz-question-circle': '&#xe921;',
		'alz-search': '&#xe922;',
		'alz-cloud-download': '&#xe923;',
		'alz-sign-out': '&#xe924;',
		'alz-square': '&#xe925;',
		'alz-store': '&#xe926;',
		'alz-sync': '&#xe927;',
		'alz-trash': '&#xe928;',
		'alz-users': '&#xe929;',
		'alz-seeding': '&#xe92a;',
		'0': 0
		},
		els = document.getElementsByTagName('*'),
		i, c, el;
	for (i = 0; ; i += 1) {
		el = els[i];
		if(!el) {
			break;
		}
		c = el.className;
		c = c.match(/alz-[^\s'"]+/);
		if (c && icons[c[0]]) {
			addIcon(el, icons[c[0]]);
		}
	}
}());
