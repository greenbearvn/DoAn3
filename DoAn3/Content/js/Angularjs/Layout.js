

myApp.controller("getLoaiMay", getLoaiMay);
function getLoaiMay($scope, $http) {

    $http({
        method: "GET",
        url: "/Home/LayLoaimay"

    }).then(function (res) {
        $scope.loaimay = res.data;

    });

}

myApp.controller("recommendPost", recommendPost);
function recommendPost($scope, $http) {

    $http({
        method: "GET",
        url: "/NewsFront/RecommendPost"

    }).then(function (res) {
        $scope.datas = res.data;
    });
}