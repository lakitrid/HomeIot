/// <binding ProjectOpened='default' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.initConfig({
        uglify: {
            my_target: {
                options: {
                    beautify: true,
                    mangle: false
                },
                files: {
                    'wwwroot/app/app.min.js': [
                        'wwwroot/app/sources/**/*.js'
                    ]
                }
            }
        },

        copy: {
            main: {
                files: [
                    { expand: true, flatten: true, cwd: 'node_modules/font-awesome/fonts', src: ['**'], dest: 'wwwroot/fonts/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['**/*.min.css'], dest: 'wwwroot/css/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['angular*/**/*.min.js'], dest: 'wwwroot/lib/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['@angular*/**/angular_1_*.js'], dest: 'wwwroot/lib/' },
                    { expand: true, flatten: true, cwd: 'node_modules/', src: ['font-awesome/**/*.min.js'], dest: 'wwwroot/lib/' }
                ]
            }
        },

        cssmin: {
            options: {
                shorthandCompacting: false,
                roundingPrecision: -1,
                keepSpecialComments: 0
            },
            target: {
                files: {
                    'wwwroot/css/content.min.css': [
                        'wwwroot/css/sources/**/*.css'
                    ]
                }
            }
        },

        watch: {
            scripts: {
                files: ['wwwroot/app/sources**/*.js', 'wwwroot/css/sources/**/*.css'],
                tasks: ['cssmin', 'uglify']
            }
        }
    });

    grunt.registerTask('default', ['cssmin', 'uglify', 'copy', 'watch']);
    grunt.registerTask('build', ['cssmin', 'uglify', 'copy']);
};