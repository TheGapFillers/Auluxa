// Sets the header and the navigation menu.
// The html is in Views/Admin/Index.cshtml.
angular.module('auluxa').controller('mainCtrl', ['$scope',
    function ($scope) {
        $scope.firstName = "Ambroise";
        $scope.lastName = "Coussin";
        $scope.accountType = "Admin";
        $scope.accountStatus = "Free";
        $scope.couponCode = "1231233";
        $scope.version = "1.7";
    }]);