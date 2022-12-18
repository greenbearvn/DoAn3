var myApp = angular.module("myApp", []);

myApp.controller("getSPTheoLoaiMay", getSPTheoLoaiMay);
function getSPTheoLoaiMay($scope, $http) {
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);
    $scope.GetData = function (id) {
        $http({
            method: "GET",
            url: "/Category/LaySPTheoLoaiMay?id=" + Number(id)

        }).then(function (res) {
            $scope.sp = res.data;
            console.log($scope.sp)

        });
    }

}