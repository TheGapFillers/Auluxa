angular.module('auluxa.proxy', [])

.service('proxy', ['bearer', '$http', function (bearer, $http) {
    var basePath = "http://auluxawebapp-prod.ap-southeast-1.elasticbeanstalk.com";
    var basePath = "http://localhost:57776";

    // Adds the bearer.
    $http.defaults.headers.common['Authorization'] = "Bearer " + bearer;

    this.getUsers = function () {
        $http.get("/api/users").then(function (response) {
        });
    };

    this.createUsers = function (model) {
        $http.post("/api/users", model).then(function (response) {
        });
    };
}]);
