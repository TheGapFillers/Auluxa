angular.module('auluxa.users', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/users', {
        templateUrl: '/app/users/users.html',
        controller: 'UsersCtrl'
    });
}])

.controller('UsersCtrl', ['$scope', 'currentUser', 'proxy', function ($scope, currentUser, proxy) {
    $scope.currentUser = currentUser;
    $scope.users = [];
    $scope.email = '';
    $scope.role = 'Admin';
    $scope.enableSave = false;
    $scope.showNewUserMessage = false;

    // Gets the users;
    proxy.getUsers().then(function (response) {
        $scope.users = response;
        $scope.users.map(function (u) {
            u.isToEdit = false;
            u.role = '' ? 'Normal' : u.role;
        });
    })

    // Checks for a change in the new user email.
    $scope.emailChange = function () {
        var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (regex.test($scope.email)) {
            $scope.enableSave = true;
            $scope.showNewUserMessage = false;
        } else {
            $scope.enableSave = false;
            $scope.showNewUserMessage = false;
        }
    };

    // Saves the user.
    $scope.saveUser = function () {
        var newUser = {
            "email": $scope.email,
            "role": $scope.role
        }

        proxy.saveUser(newUser).then(function (response) {
            // Gets all the users to re-populate the grid
            proxy.getUsers().then(function (response) {
                $scope.users = response;
            });

            $scope.email = '';
            $scope.role = 'Admin';
            $scope.enableSave = false;
            $scope.showNewUserMessage = true;
        })
    }

    $scope.deleteUser = function (user) {
        proxy.deleteUser(user.email).then(function (response) {
            // Gets all the users to re-populate the grid
            proxy.getUsers().then(function (response) {
                $scope.users = response;
            });
        })
    }
}]);