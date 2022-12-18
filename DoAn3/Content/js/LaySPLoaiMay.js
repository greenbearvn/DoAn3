var myApp = angular.module("myApp", []);
myApp.controller("laySPTheoMay", laySPTheoMay);
function laySPTheoMay($scope, $http) {
    $scope.GetData = function (id) {
        $http({
            method: "GET",
            url: "/Home/LaySPTheoLoaiMay?id="+id,

        }).then(function (res) {
            $scope.products = res.data;
            console.log(res.data)

        });
    }
    
}