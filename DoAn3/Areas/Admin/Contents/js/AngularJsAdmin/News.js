var myApp = angular.module("myApp", []).config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);

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

myApp.controller("NewsIndex", NewsIndex);
function NewsIndex($scope, $http) {

    $scope.DeleteButton = function (id) {
        var deleteItem = window.confirm("Bạn có muốn xóa bài viết này không ?")
        if (deleteItem == true) {

            $scope.NewsID = Number(id);
            window.location.reload();
        }
        $http({
            method: "post",
            url: "/Admin/News/DeleteConfirmed/?id=" + $scope.NewsID,
            dataType: "json",
            data: JSON.stringify($scope.NewsID)

        }).then(function (res) {
            $scope.alert = res.data;
            if ($scope.alert == true) {
                alert("Xóa bài viết thành công")
            }
            else {
                alert("Xóa bài viết không thành công")
            }
        });

    }
    $http({
        method: "get",
        url: "/Admin/News/GetAllNews",

    }).then(function (res) {
        $scope.datas = res.data;

    });
}


myApp.controller('Create', function ($scope, $http, $httpParamSerializerJQLike) {
    $scope.news = {};

    $scope.addData = function () {
        
        $scope.news.Banner = document.getElementById("Banner").value;
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.news.__RequestVerificationToken = Token;
        dataRequest = $scope.news;
        console.log(dataRequest);
        $http({
            method: 'post',
            url: '/Admin/News/CreateNews',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Thêm bài viết không thành công");
            }
            else {
                alert("Thêm bài viết thành công");
                window.location.href = "/Admin/News/Index";
            }
        });

    };

    $http({
        method: "get",
        url: "/Admin/News/GetLoaiGame",

    }).then(function (res) {
        $scope.lgs = res.data;

    });

    $http({
        method: "get",
        url: "/Admin/News/GetAllUser",

    }).then(function (res) {
        $scope.users = res.data;

    });
});


//////////////////////////////////////////////////////////////////////



/// add Ckeditor
$(document).ready(function () {
    CKEDITOR.replace('MoTa', {
        customConfig: '/Content/ckeditor/config.js',
        extraAllowedContent: 'span',
    });
});


/// add Ckfinder
function UpLoad(feild) {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileURL) {
        document.getElementById(feild).value = fileURL;

    };
    finder.popup();
}

////////////////////////////////////////////////////////////


myApp.controller('Edit', function ($scope, $http, $httpParamSerializerJQLike) {

    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);

    $scope.news = {};
    $scope.news.NewsID = id;

    $http({
        method: "GET",
        url: "/Admin/News/GetDetailNews/" + Number(id)

    }).then(function (res) {
        $scope.infor = res.data;
        $scope.news.Title = $scope.infor.Title;
        $scope.news.Content = $scope.infor.Content;
        $scope.news.Banner = $scope.infor.Banner;
        $scope.news.PublicDate = $scope.infor.PublicDate;
        $scope.news.MaLoai = $scope.infor.MaLoai;
        $scope.news.UserID = $scope.infor.UserID;
       
    });

    ///////////////////////////////////////////////////////////////////////////////////////
    $scope.addData = function () {
        $scope.news.Banner = document.getElementById("Banner").value;
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.news.__RequestVerificationToken = Token;
        dataRequest = $scope.news;
        console.log(dataRequest);
        $http({
            method: 'POST',
            url: '/Admin/News/EditNews',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Sửa bài viết không thành công");
            }
            else {
                alert("Sửa bài viết thành công");
                window.location.href = "/Admin/News/Index";
            }
        });
    };

    $http({
        method: "get",
        url: "/Admin/News/GetLoaiGame",

    }).then(function (res) {
        $scope.lgs = res.data;

    });

    $http({
        method: "get",
        url: "/Admin/News/GetAllUser",

    }).then(function (res) {
        $scope.users = res.data;

    });
});