var myApp = angular.module("myApp", []).config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);;

myApp.controller("getLoaiMay", getLoaiMay);
function getLoaiMay($scope, $http) {

    $http({
        method: "GET",
        url: "/Home/LayLoaimay"

    }).then(function (res) {
        $scope.loaimay = res.data;

    });
}

myApp.controller("getListNews", getListNews);
function getListNews($scope, $http) {

    $http({
        method: "GET",
        url: "/NewsFront/ListNews"

    }).then(function (res) {
        $scope.datas = res.data;
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

myApp.controller("DetailNew", DetailNew);
function DetailNew($scope, $http) {
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);

    $http({
        method: "GET",
        url: "/NewsFront/DetaiNews?id=" + Number(id)

    }).then(function (res) {
        $scope.news = res.data;
        console.log($scope.news)

    });

    $http({
        method: "GET",
        url: "/NewsFront/RecommendPost"

    }).then(function (res) {
        $scope.datas = res.data;
        console.log($scope.datas)

    });

}

myApp.controller("getListMachine", getListMachine);
function getListMachine($scope, $http) {

    $http({
        method: "GET",
        url: "/NewsFront/ListMachine"

    }).then(function (res) {
        $scope.datas = res.data;
    });
}


myApp.controller("getListGame", getListGame);
function getListGame($scope, $http) {

    $http({
        method: "GET",
        url: "/NewsFront/ListGame"

    }).then(function (res) {
        $scope.datas = res.data;
    });
}


myApp.controller("getLoaiMay", getLoaiMay);
function getLoaiMay($scope, $http) {

    $http({
        method: "GET",
        url: "/Home/LayLoaimay"

    }).then(function (res) {
        $scope.loaimay = res.data;

    });
}