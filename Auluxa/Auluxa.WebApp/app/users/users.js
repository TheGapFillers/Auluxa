angular.module('auluxa.users', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/users', {
        templateUrl: '/app/users/users.html',
        controller: 'UsersCtrl'
    });
}])

.controller('UsersCtrl', ['$scope',
    function ($scope) {
        $scope.name = 'Hello community';
    }]);