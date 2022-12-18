
var myApp = angular.module("myApp", []);
myApp.controller("getLoaiMay", getLoaiMay);
function getLoaiMay($scope, $http) {

    $http({
        method: "GET",
        url: "/Home/LayLoaimay"

    }).then(function (res) {
        $scope.loaimay = res.data;

    });
}
