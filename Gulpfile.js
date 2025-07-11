var gulp = require('gulp');
var sass = require('gulp-sass')(require('sass'));
var cleanCSS = require("gulp-clean-css");
var postcss = require('gulp-postcss');
var autoprefixer = require('autoprefixer');
var concat = require("gulp-concat");

var paths = {
    scss: "./wwwroot/scss/**/*.scss",
    css: "./wwwroot/css/",
    minCss: "./wwwroot/css/min/",
    concatCssDest: "./wwwroot/css/site.min.css",
};

gulp.task('sass', function (cb) {
    gulp
        .src(paths.scss)
        .pipe(sass())
        .pipe(gulp.dest(paths.css))
        .pipe(cleanCSS({ compatibility: 'ie8' })) // dùng cleanCSS thay vì cssmin
        .pipe(gulp.dest(paths.minCss));
    cb();
});

gulp.task(
    'default',
    gulp.series('sass', function (cb) {
        gulp.watch(paths.scss, gulp.series('sass'));
        cb();
    })
);
