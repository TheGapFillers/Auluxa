// Sets the header and the navigation menu.
// The html is in Views/Admin/Index.cshtml.
angular.module('auluxa')

.controller('mainCtrl', ['$scope', 'proxy', function ($scope, proxy) {
    $scope.user = {};
    $scope.subscription = {};

    // Gets the current user;
    proxy.getCurrentUser().then(function (response) {
        $scope.user = response;
        if (!$scope.user.role) {
            $scope.user.role = 'Normal';
        }
    });

    proxy.getCurrentSubscription().then(function (response) {
        $scope.subscription = response;

        // Capuitalizes the subscriptionType.
        var subscriptionType = response.subscriptionType;
        let capitalizeSubscriptionType = subscriptionType.charAt(0).toUpperCase() + subscriptionType.substr(1).toLowerCase()
        $scope.subscription.subscriptionType = capitalizeSubscriptionType;
    });

    $scope.version = "1.7";
}]);
