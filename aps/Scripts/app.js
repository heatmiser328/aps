angular.module('ica.aps.app', ['ngRoute'])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $routeProvider
        .when('/', {
            exposed: false,
            title: 'Home',
            templateUrl: 'main',
            controller: 'MainController'
        })
        .otherwise({ redirectTo: '/' });

        $locationProvider.html5Mode(true).hashPrefix('!');
    } ])

    