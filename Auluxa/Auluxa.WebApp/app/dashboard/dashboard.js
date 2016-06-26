angular.module('auluxa.dashboard', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/dashboard', {
        templateUrl: '/app/dashboard/dashboard.html',
        controller: 'DashboardCtrl'
    });
}])

.controller('DashboardCtrl', ['$scope',
    function ($scope) {
        $scope.name = 'Hello community';
    }]);