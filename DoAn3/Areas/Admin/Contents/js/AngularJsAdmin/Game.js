var myApp = angular.module("myApp", ['ui.bootstrap']).config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);

myApp.directive('ckEditor', function () {
    return {
        require: '?ngModel',
        link: function (scope, elm, attr, ngModel) {
            var ck = CKEDITOR.replace(elm[0]);

            if (!ngModel) return;

            ck.on('instanceReady', function () {
                ck.setData(ngModel.$viewValue);
            });

            function updateModel() {
                scope.$apply(function () {
                    ngModel.$setViewValue(ck.getData());
                });
            }

            ck.on('change', updateModel);
            ck.on('key', updateModel);
            ck.on('dataReady', updateModel);

            ngModel.$render = function (value) {
                ck.setData(ngModel.$viewValue);
            };
        }
    };
});

myApp.controller("getDataGameIndex", getDataGameIndex);
function getDataGameIndex($scope, $http) {

    $scope.filteredGames = {}
        , $scope.currentPage = 1
        , $scope.numPerPage = 10
        , $scope.maxSize = 5;


    $scope.DeleteButton = function (id) {
        var deleteItem = window.confirm("Bạn có muốn xóa sản phẩm này không ?")
        if (deleteItem == true) {
            $scope.Game = {};
            $scope.Game.MaGame = id;
            
        }
        $http({
            method: "post",
            url: "/Admin/Games/DeleteGame/?id=" + $scope.Game,
            dataType: "json",
            data: JSON.stringify($scope.Game)

        }).then(function (res) {
            $scope.games = res.data;

            $scope.$watch('currentPage + numPerPage', function () {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

                if (!$scope.games || !$scope.games.length) { return }
                $scope.filteredTodos = $scope.games.slice(begin, end);
            });
        });

    }

    
    $http({
        method: "GET",
        url: "/Admin/Games/GameIndex"


    }).then(function (res) {
        $scope.games = res.data;

        $scope.$watch('currentPage + numPerPage', function () {
            var begin = (($scope.currentPage - 1) * $scope.numPerPage), end = begin + $scope.numPerPage;

            if (!$scope.games || !$scope.games.length) { return }
            $scope.filteredTodos = $scope.games.slice(begin, end);
        });
    });
}
//////////////////////////////////////////////////////////////////////

$(document).ready(function () {
    CKEDITOR.replace('MoTa', {
        customConfig: '/Content/ckeditor/config.js',
        extraAllowedContent: 'span',
    });
});

function UpLoad(feild) {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileURL) {
        document.getElementById(feild).value = fileURL;

    };
    finder.popup();
}

/// Post data
myApp.controller('Create', function ($scope, $http, $httpParamSerializerJQLike) {
    $scope.game = {};

    /*$scope.game.AnhGame = document.getElementById("AnhGame").value;*/
    $scope.addData = function () {
        $scope.game.AnhGame = document.getElementById("AnhGame").value;
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.game.__RequestVerificationToken = Token;
        dataRequest = $scope.game;
        console.log(dataRequest);
        $http({
            method: 'post',
            url: '/Admin/Games/CreateGame',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            alert($scope.data)
            window.location.reload();
        });
    };

    $http({
        method: "GET",
        url: "/Admin/Games/GetLoaiMay"


    }).then(function (res) {
        $scope.loaimays = res.data;
    });


    $http({
        method: "GET",
        url: "/Admin/Games/GetLoaiGame"


    }).then(function (res) {
        $scope.loaigames = res.data;
    });


    $http({
        method: "GET",
        url: "/Admin/Games/GetNPH"


    }).then(function (res) {
        $scope.nphs = res.data;
    });

});

myApp.controller('Edit', function ($scope, $http, $httpParamSerializerJQLike) {

    /// get value ckeditor
    myApp.directive('ckEditor', function () {
        return {
            require: '?ngModel',
            link: function (scope, elm, attr, ngModel) {
                var ck = CKEDITOR.replace(elm[0]);

                if (!ngModel) return;

                ck.on('instanceReady', function () {
                    ck.setData(ngModel.$viewValue);
                });

                function updateModel() {
                    scope.$apply(function () {
                        ngModel.$setViewValue(ck.getData());
                    });
                }

                ck.on('change', updateModel);
                ck.on('key', updateModel);
                ck.on('dataReady', updateModel);

                ngModel.$render = function (value) {
                    ck.setData(ngModel.$viewValue);
                };
            }
        };
    });






    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);


    $scope.game = {};
    $scope.game.MaGame = id;

    $http({
        method: "GET",
        url: "/Admin/Games/DataEditGame/" + Number(id)

    }).then(function (res) {
        $scope.infor = res.data;
        $scope.game.TenGame = $scope.infor.TenGame;
        $scope.game.AnhGame = $scope.infor.AnhGame;
        $scope.game.MoTa = $scope.infor.MoTa;
        $scope.game.MaLoai = $scope.infor.MaLoai;
        $scope.game.MaMay = $scope.infor.MaMay;
        $scope.game.MaNPH = $scope.infor.MaNPH;
        $scope.game.SoLuong = $scope.infor.SoLuong;
        $scope.game.GiaTien = $scope.infor.GiaTien;
        $scope.game.Likes = $scope.infor.Likes;
        $scope.game.Views = $scope.infor.Views;
        $scope.game.Favouries = $scope.infor.Favouries;
        $scope.game.TrangThai = $scope.infor.TrangThai;

    });

    $http({
        method: "GET",
        url: "/Admin/Games/GetLoaiMay"


    }).then(function (res) {
        $scope.loaimays = res.data;
    });


    $http({
        method: "GET",
        url: "/Admin/Games/GetLoaiGame"


    }).then(function (res) {
        $scope.loaigames = res.data;
    });


    $http({
        method: "GET",
        url: "/Admin/Games/GetNPH"


    }).then(function (res) {
        $scope.nphs = res.data;
    });

    ///////////////////////////////////////////////////////////////////////////////////////
    $scope.addData = function () {
        $scope.game.AnhGame = document.getElementById("AnhGame").value;
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.game.__RequestVerificationToken = Token;
        dataRequest = $scope.game;
        console.log(dataRequest);
        $http({
            method: 'POST',
            url: '/Admin/Games/EditGame',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Loi Sua ban ghi");
            }
            else {
                alert("Sua ban ghi thanh cong");
                window.location.href = "/Admin/Games/Index";
            }
        });



    };
});