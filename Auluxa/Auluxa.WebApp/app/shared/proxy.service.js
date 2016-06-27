angular.module('auluxa.proxy', [])

.service('proxy', ['$http', function($http) {

    this.getToken = function () {
        debugger;

        
        $http.get("http://auluxawebapp-prod.ap-southeast-1.elasticbeanstalk.com/token").then(function (response) {
            debugger;
            var a = 100;

        });

    };


}]);
