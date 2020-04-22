// Initialize modules
// Importing specific gulp API functions lets us write them below as series() instead of gulp.series()
const { src, dest, series, parallel } = require('gulp');
// Importing all the Gulp-related packages we want to use
const gulp = require('gulp');
const sourcemaps = require('gulp-sourcemaps');
const sass = require('gulp-sass');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const postcss = require('gulp-postcss');
const autoprefixer = require('autoprefixer');
const cssnano = require('cssnano');
const browserSync = require('browser-sync').create();


// File paths
const files = {
    scssPath: './wwwwroot/src/scss/**/*.scss',
    jsPath: './wwwroot/src/js/**/*.js'
}

// Sass task: compiles the style.scss file into style.css
function scssTask(){
    return src(files.scssPath)
        .pipe(sourcemaps.init()) // initialize sourcemaps first
        .pipe(sass()) // compile SCSS to CSS
        .pipe(postcss([ autoprefixer(), cssnano() ])) // PostCSS plugins
        .pipe(sourcemaps.write('.')) // write sourcemaps file in current directory
        .pipe(dest('./wwwroot/dist/css')
    ); // put final CSS in assets css folder
}

// JS task: concatenates and uglifies JS files to script.js
function jsTask(){
    return src([
        files.jsPath
        //,'!' + 'includes/js/jquery.min.js', // to exclude any specific files
        ])
        .pipe(concat('script.js'))
        .pipe(uglify())
        .pipe(dest('./wwwroot/dist/js')
    );
}

// Watch for changes to any scss or js file
//function watchTask() {
//    browserSync.init({
//        server: {
//            baseDir: './'
//        }
//    });
//    gulp.watch(files.scssPath, scssTask);
//    gulp.watch('./wwwroot/html/*.html').on('change', browserSync.reload);
//    gulp.watch(files.jsPath).on('change', browserSync.reload);
//}

// Export the default Gulp task so it can be run
// Runs the scss and js tasks simultaneously
// then runs cacheBust, then watch task
exports.default = series(
    parallel(scssTask, jsTask),
    //watchTask
);