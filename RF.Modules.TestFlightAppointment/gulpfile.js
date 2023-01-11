/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

var gulp = require("gulp");
var sass = require("gulp-sass")(require('sass'));
var strip = require("gulp-strip-css-comments");

gulp.task('module-styles', function () {
    return gulp
        .src('Assets/Styles/module.scss')
        .pipe(sass())
        .pipe(strip())
        .pipe(gulp.dest('./'));
});

gulp.task('build', gulp.parallel('module-styles'));
