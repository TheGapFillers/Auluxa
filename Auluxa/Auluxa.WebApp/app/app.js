// Declare app level module which depends on views, and components
angular.module('auluxa', [
  'ngRoute',
  'auluxa.dashboard',
  'auluxa.store',
  'auluxa.users',
]).

config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider.otherwise({ redirectTo: '/dashboard' });
}])
