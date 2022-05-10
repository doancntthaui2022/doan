/// <binding ProjectOpened='async_watch' />
const gulp = require("gulp");
const autoprefixer = require("gulp-autoprefixer");
const concat = require("gulp-concat");
const sass = require('gulp-sass')(require('sass'));
const terser = require("gulp-terser");

sass.compiler = require("dart-sass");

var paths = {
    styles: {
        src: "HoangNV.HotelBooking.Web/Styles/**/*.scss",
        dst: "HoangNV.HotelBooking.Web/wwwroot/css/",
    },
    scripts: {
        src: "HoangNV.HotelBooking.Web/Scripts/**/*.js",
        dst: "HoangNV.HotelBooking.Web/wwwroot/js/",
    },
};

function styles() {
    return gulp
        .src(paths.styles.src)
        .pipe(sass({ outputStyle: "compressed" }).on("error", sass.logError))
        .pipe(autoprefixer({ cascade: false }))
        .pipe(concat("site.min.css"))
        .pipe(gulp.dest(paths.styles.dst));
}

function scripts() {
    return gulp
        .src(paths.scripts.src)
        .pipe(terser())
        .pipe(concat("site.min.js"))
        .pipe(gulp.dest(paths.scripts.dst));
}

function scriptsDevelopment() {
    return gulp
        .src(paths.scripts.src)
        .pipe(concat("site.min.js"))
        .pipe(gulp.dest(paths.scripts.dst));
}

function watch() {
    gulp.watch(paths.styles.src, styles);
    gulp.watch(paths.scripts.src, scripts);
}

function watchDevelopment() {
    gulp.watch(paths.styles.src, styles);
    gulp.watch(paths.scripts.src, scriptsDevelopment);
}

const build = gulp.parallel(styles, scripts);
const buildDevelopment = gulp.parallel(styles, scriptsDevelopment);

exports.default = build;
exports.watch = watch;
exports.buildDevelopment = buildDevelopment;

gulp.task('async_watch',async function () {
    buildDevelopment();
    watchDevelopment();
});
