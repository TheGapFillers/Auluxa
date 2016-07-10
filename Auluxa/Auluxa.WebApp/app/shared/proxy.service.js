angular.module('auluxa.proxy', [])

.service('proxy', ['bearer', '$http', function (bearer, $http) {
    var basePath = "http://auluxawebapp-prod.ap-southeast-1.elasticbeanstalk.com";
    var basePath = "http://localhost:57776";

    // Adds the bearer.
    $http.defaults.headers.common['Authorization'] = "Bearer " + bearer;

    // Gets the users.
    this.getUsers = function () {
        return $http.get("/api/users/profiles").then(function (response) {
            return response.data;
        });
    };
    // Saves a users.
    this.saveUser = function (model) {
        return $http.post("/api/users/profiles", model).then(function (response) {
            return response.data;
        });
    };
}]);
