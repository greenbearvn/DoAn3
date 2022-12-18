var myApp = angular.module("myApp", ['ui.bootstrap']).config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);

myApp.controller("DonHangIndex", DonHangIndex);
function DonHangIndex($scope, $http) {

    $scope.DeleteButton = function (id) {
        var deleteItem = window.confirm("Bạn có muốn xóa đơn hàng này không ?")
        if (deleteItem == true) {

            $scope.MaDH = Number(id);
            window.location.reload();
        }
        $http({
            method: "post",
            url: "/Admin/DonHangs/DeleteConfirmed/?id=" + $scope.MaDH,
            dataType: "json",
            data: JSON.stringify($scope.MaDH)

        }).then(function (res) {
            $scope.alert = res.data;
            alert($scope.alert)
        });

    }

    $scope.filteredGames = {}
        , $scope.currentPage = 1
        , $scope.numPerPage = 10
        , $scope.maxSize = 5;

    $http({
        method: "get",
        url: "/Admin/DonHangs/DataIndex",
    }).then(function (res) {
        $scope.games = res.data;

        $scope.$watch('currentPage + numPerPage', function () {
            var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

            if (!$scope.games || !$scope.games.length) { return }
            $scope.filteredTodos = $scope.games.slice(begin, end);
        });
    });


    $http({
        method: "get",
        url: "/Admin/DonHangs/DataIndex",

    }).then(function (res) {
        $scope.datas = res.data;

    });
}

myApp.controller('Create', function ($scope, $http, $httpParamSerializerJQLike,$window) {
    
    
    $scope.dh = {};
    $scope.addData = function () {
        
        $scope.dh.Tongtien = document.getElementById("totalMoney").innerHTML;
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.dh.__RequestVerificationToken = Token;
        dataRequest = $scope.dh;
        console.log(dataRequest);
        $http({
            method: 'post',
            url: '/Admin/DonHangs/CreateDH',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == true) {
                alert("Thêm đơn hàng thành công");
                window.location.href = "/Admin/DonHangs/Index";
            }
            else {
                alert("Thêm đơn hàng không thành công");
            }
        });
    };

    $http({
        method: "get",
        url: "/Admin/DonHangs/getKhachHang"


    }).then(function (res) {
        $scope.kh = res.data;
    });




    $scope.GetAllGame = function () {

        var searchString = document.getElementById('SearchName').value;
        /// game pagination
        $scope.filteredGames = {}
            , $scope.currentPage = 1
            , $scope.numPerPage = 10
            , $scope.maxSize = 5;

        $http({
            method: "get",
            url: "/Admin/Games/DataSearch/?name=" + searchString
        }).then(function (res) {
            $scope.games = res.data;

            $scope.$watch('currentPage + numPerPage', function () {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

                if (!$scope.games || !$scope.games.length) { return }
                $scope.filteredTodos = $scope.games.slice(begin, end);
            });
        });
        
    };

    $scope.Reset = function () {

        var searchString = document.getElementById('SearchName').value;
        searchString = " "
        /// game pagination
        $scope.filteredGames = {}
            , $scope.currentPage = 1
            , $scope.numPerPage = 10
            , $scope.maxSize = 5;

        $http({
            method: "get",
            url: "/Admin/Games/DataSearch/?name=" + searchString
        }).then(function (res) {
            $scope.games = res.data;

            $scope.$watch('currentPage + numPerPage', function () {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

                if (!$scope.games || !$scope.games.length) { return }
                $scope.filteredTodos = $scope.games.slice(begin, end);
            });
        });

    };

   
});


