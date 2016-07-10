angular.module('auluxa.proxy', [])

.service('proxy', ['bearer', '$http', '$location', function (bearer, $http, $location) {
    // Get the base path.
    var basePath = $location.protocol() + "://" + location.host;

    // Adds the bearer.
    $http.defaults.headers.common['Authorization'] = "Bearer " + bearer;

    // Gets the current user.
    this.getCurrentUser = function () {
        return $http.get("/api/users").then(function (response) {
            return response.data;
        });
    };

    // Gets the current subscription.
    this.getCurrentSubscription = function () {
        return $http.get("/subscriptions").then(function (response) {
            return response.data;
        });
    };

    // Gets the user profiles.
    this.getUsers = function () {
        return $http.get("/api/users/profiles").then(function (response) {
            return response.data;
        });
    };
    // Saves a user profiles.
    this.saveUser = function (model) {
        return $http.post("/api/users/profiles", model).then(function (response) {
            return response.data;
        });
    };
    // Deletes a user profile.
    this.deleteUser = function (email) {
        return $http.delete("/api/users/profiles?email=" + email).then(function (response) {
            return response.data;
        });
    };
}]);
