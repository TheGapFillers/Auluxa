angular.module('auluxa.dashboard', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/dashboard', {
        templateUrl: '/app/dashboard/dashboard.html',
        controller: 'DashboardCtrl'
    });
}])

.controller('DashboardCtrl', ['$scope', 'proxy', function ($scope, proxy) {
    $scope.numberOfUsers = 0;

    // Gets the users;
    proxy.getUsers().then(function (response) {
        $scope.numberOfUsers = response.length;
    })
}]);