angular.module('auluxa.store', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/store', {
        templateUrl: '/app/store/store.html',
        controller: 'StoreCtrl'
    });
}])

.controller('StoreCtrl', ['$scope',
    function ($scope) {
        $scope.name = 'Hello community';
    }]);