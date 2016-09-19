var gulp = require('gulp');
var babel = require('gulp-babel');
var sass = require('gulp-sass');

gulp.task('es6', function () {
    return gulp.src('app/**/*.js')
        .pipe(babel({
            presets: ['es2015']
        }))
        .pipe(gulp.dest('wwwroot'));
});

gulp.task('sass', function() {
    return gulp.src('app/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('wwwroot'));
});

gulp.task('default', ['es6'], function() {
});

gulp.task('watch-js', function() {
    return gulp.watch('app/**/*.js', ['es6']);
});

gulp.task('watch-css', function() {
    return gulp.watch('app/**/*.scss', ['sass']);
});

gulp.task('watch', ['watch-js', 'watch-css']);
