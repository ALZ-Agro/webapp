/// <binding Clean='deploy' />
'use strict';

var gulp = require('gulp');
var $ = require('gulp-load-plugins')();
var wiredep = require('wiredep').stream;
var path = require('path');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var minifyCSS = require('gulp-minify-css');
var rename = require("gulp-rename");
var flatten = require('gulp-flatten');
var bower = require("gulp-bower");
var bowerFiles = require("main-bower-files");
var filter = require("gulp-filter");
var ngAnnotate = require('gulp-ng-annotate');
var bytediff = require('gulp-bytediff');
var series = require('stream-series');
var deletefile = require('gulp-delete-file');
var browserSync = require('browser-sync').create();

function isOnlyChange(event) {
  return event.type === 'changed';
}

gulp.paths = {
    src: './Scripts',
    dist: './Scripts/bundles',
    libs: "./bower_components",
    root: './'
};

var paths = gulp.paths;

gulp.task('fullwatch', ['inject'], function () {

  //gulp.watch([path.join(paths.src, '/*.html'), 'bower.json'], ['inject']);
  gulp.watch("./Script/app/**/*.*").on('change', browserSync.reload);
  gulp.watch([
    path.join(paths.src, '/app/**/*.css'),
    path.join(paths.src, '/app/**/*.scss')
  ], function(event) {
    if(isOnlyChange(event)) {
      gulp.start('styles');
    } else {
      gulp.start('inject');
    }
  });
gulp.start('vendor-assets');
  gulp.watch(path.join(paths.src, '/app/**/*.js'), function(event) {
    if(isOnlyChange(event)) {
      gulp.start('scripts');
    } else {
      gulp.start('inject');
    }
  });

  gulp.watch(path.join(paths.src, '/app/**/*.html'), function(event) {
    browserSync.reload(event.path);
  });
});

gulp.task('vendor-assets', function () {
    return gulp.src('./Content/**/*.{woff,woff2,eot,ttf}')
    .pipe($.flatten())
    .pipe(gulp.dest(paths.dist + "/fonts/"));
});

gulp.task('scripts', function () {
  return gulp.src(path.join(paths.src, '/app/**/*.js'))
    .pipe($.eslint())
    .pipe($.eslint.format())
    .pipe($.size())
});

gulp.task('triangular.scss', function () {
    return gulp.src(paths.src + '/app/triangular/**/*.scss')
      .pipe($.concat('triangular.scss'))
      .pipe(gulp.dest(paths.dist + '/css/'));
});

gulp.task('styles', ['triangular.scss'], function () {

    var sassOptions = {
        style: 'expanded',
        includePaths: [
          paths.libs
        ]
    };

    var injectFiles = gulp.src([
      paths.dist + '/css/triangular.scss',
      paths.src + '/app/**/*.scss',
      '!' + paths.src + '/app/**/_*.scss',
      '!' + paths.src + '/app/triangular/**/*',
      '!' + paths.src + '/app/app.scss'
    ], { read: false });

    var injectOptions = {
        transform: function (filePath) {
            filePath = filePath.replace(paths.src + '/app/', '');
            return '@import \'' + filePath + '\';';
        },
        starttag: '// injector',
        endtag: '// endinjector',
        addRootSlash: false
    };

    var indexFilter = $.filter('app.scss', {
        restore: true
    });

    return gulp.src([
      paths.src + '/app/app.scss'
    ])
      .pipe(indexFilter)
      .pipe($.inject(injectFiles, injectOptions))
      .pipe(indexFilter.restore)
      .pipe($.sass(sassOptions))

    .pipe($.autoprefixer({ browsers: ['> 1%', 'last 2 versions', 'Firefox ESR', 'Opera 12.1'] }))
      .on('error', function handleError(err) {
          console.error(err.toString());
          this.emit('end');
      })
      .pipe(gulp.dest(paths.dist + '/css/'));
});

gulp.task('vendor-styles', function () {
    return gulp.src('./Content/vendors/**/*.css')
     .pipe(concat('vendors.css'))
     .pipe(gulp.dest(paths.dist + "/css/"))
});

gulp.task('vendor-scripts', function () {
    return gulp.src('./Content/vendors/**/*.js')
      .pipe(uglify())
      .pipe(concat('vendors.js'))
      .pipe(gulp.dest(paths.dist + "/js/"))
});

gulp.task('assets', ["vendor-scripts", "vendor-styles", "bower-assets", "inject", "inject-svg", "inject-images"], function () {

});

gulp.task("inject-images", function () {
    return gulp.src(['bower_components/**/*-skin.{png,ico}', 'Content/**/*-skin.{png,ico}'])
        .pipe($.flatten())
        .pipe(gulp.dest(paths.dist + '/css/'));
});

gulp.task('inject-svg', function () {
    return gulp.src(['bower_components/**/*-inline.{svg,ico}', 'Content/**/*-inline.{svg,ico}'])
        .pipe($.flatten())
        .pipe(gulp.dest(paths.dist + "/res/icons/")); 
});

gulp.task('bower-assets', function () {
    return gulp.src(['bower_components/**/*.{woff,woff2,eot,ttf}','Content/**/*.{woff,woff2,eot,ttf}'])
    .pipe($.flatten())
    .pipe(gulp.dest(paths.dist + "/fonts/"));
});

gulp.task('bower-update', function () {
    return bower({ cmd: 'update' });
});
gulp.task('bower-install', function () {
    return bower();
});

gulp.task('inject', function () {

    var appStyles = gulp.src([
     paths.dist + '/css/*.css',
     '!' + paths.dist + '/css/libs.min.css',
    ], { read: false });

    var appScripts = gulp.src([
      paths.src + '/app/**/*.js',
      paths.dist + '/js/vendors.js',
      '!' + paths.src + '/app/**/*.spec.js',
      '!' + paths.src + '/app/**/*.mock.js',
    ]).pipe($.angularFilesort());

    var injectOptions = {
        addRootSlash: false
    };

    var wiredepOptions = {
        bowerJson: require('./bower.json'),
        directory: paths.libs,
        exclude: [/foundation\.css/, /material-design-iconic-font\.css/, /default\.css/]
    };

    return gulp.src('./Views/Home/Index.cshtml')
      .pipe(wiredep(wiredepOptions))
      .pipe($.inject(appStyles, injectOptions))
      .pipe($.inject(appScripts, injectOptions))
      .pipe(gulp.dest('./Views/Home/'));
});

//Deploy
gulp.task('min:Site', function () {
    return gulp.src(paths.src + '/app/**/*.js')
        .pipe($.angularFilesort())
        .pipe(concat('site.min.js', { newLine: ';' }))
        // Annotate before uglify so the code get's min'd properly.
        .pipe(ngAnnotate({
            // true helps add where @ngInject is not used. It infers.
            // Doesn't work with resolve, so we must be explicit there
            add: true
        }))
        .pipe(bytediff.start())
        .pipe(uglify({ mangle: true }))
        .pipe(bytediff.stop())
        .pipe(gulp.dest(paths.dist + '/js'))
});

gulp.task('min:Bower', function () {
    var jsFilter = filter(['**/*.js'], { restore: true });
    var cssFilter = filter(['*.css'], { restore: true });
    var fontFilter = filter(['*.eot', '*.woff', '*.woff2', '*.svg', '*.ttf'], { restore: true });
    var imageFilter = filter(['*.gif', '*.png', '*.svg', '*.jpg', '*.jpeg'], { restore: true });

    return gulp.src(bowerFiles())
        //JS
        .pipe(jsFilter)
        .pipe(concat('libs.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(paths.dist + '/js'))
        .pipe(jsFilter.restore)

        //CSS
        .pipe(cssFilter)
        .pipe(concat('libs.min.css'))
        .pipe(minifyCSS({ keepBreaks: true }))
        .pipe(gulp.dest(paths.dist + '/css'))
        .pipe(cssFilter.restore)

        //FONTS
        .pipe(fontFilter)
        .pipe(flatten())
        .pipe(gulp.dest(paths.dist + '/fonts'))
        .pipe(fontFilter.restore)

        // IMAGES
        .pipe(imageFilter)
        .pipe(flatten())
        .pipe(gulp.dest(paths.dist + '/images'))
        .pipe(imageFilter.restore)
})
gulp.task('watch', function () {
    gulp.watch(paths.src + '/app/**/*.scss', ['styles']);   
});

gulp.task('deploy', ['styles', 'assets', 'min:Bower', 'min:Site'], function () {

    var appStyles = gulp.src([
      paths.src + '/bundles/css/*.css',
      '!' + paths.dist + '/css/libs.min.css',
      '!' + paths.dist + '/css/vendors.css',
    ], { read: false });

    var bundleStyles = gulp.src([
       paths.dist + '/css/libs.min.css',
       paths.dist + '/css/vendors.css',
    ], { read: false });

    var appScripts = gulp.src([
      paths.src + '/bundles/js/*.js',
      '!' + paths.dist + '/js/libs.min.js',
      '!' + paths.dist + '/js/vendors.js',
    ]);

    var bundleScripts = gulp.src([
        paths.dist + '/js/libs.min.js',
        paths.dist + '/js/vendors.js',
    ]);

    var injectOptions = {
        addRootSlash: false,
    };

    var wiredepOptions = {
        bowerJson: null,
        directory: null,
        exclude: [/bower_components/]
    };

    return gulp.src('./Views/Home/Index.cshtml')
      .pipe(wiredep(wiredepOptions))
      .pipe($.inject(series(bundleStyles, appStyles), injectOptions))
      .pipe($.inject(series(bundleScripts, appScripts), injectOptions))
      .pipe(gulp.dest('./Views/Home/'));
});
