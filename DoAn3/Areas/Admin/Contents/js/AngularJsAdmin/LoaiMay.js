var myApp = angular.module("myApp", []);

myApp.controller("LoaiMayIndex", LoaiMayIndex);
function LoaiMayIndex($scope, $http) {

    $scope.DeleteButton = function (id) {
        var deleteItem = window.confirm("Bạn có muốn xóa loại máy này không ?")
        if (deleteItem == true) {

            $scope.MaMay = Number(id);
            window.location.reload();
        }
        $http({
            method: "post",
            url: "/Admin/LoaiMays/DeleteConfirmed/?id=" + $scope.MaMay,
            dataType: "json",
            data: JSON.stringify($scope.MaMay)

        }).then(function (res) {
            $scope.alert = res.data;
            alert($scope.alert)
        });

    }
    $http({
        method: "get",
        url: "/Admin/LoaiMays/GetDataLoaiMay",

    }).then(function (res) {
        $scope.datas = res.data;

    });


}

myApp.controller('Create', function ($scope, $http, $httpParamSerializerJQLike) {
    $scope.loaiMay = {};
    $scope.addData = function () {
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.loaiMay.__RequestVerificationToken = Token;
        dataRequest = $scope.loaiMay;
        console.log(dataRequest);
        $http({
            method: 'POST',
            url: '/Admin/LoaiMays/CreateLM',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Loi them ban ghi");
            }
            else {
                alert("Them ban ghi thanh cong");
                window.location.href = "/Admin/LoaiMays/Index";
            }
        });
    };
});


myApp.controller('Edit', function ($scope, $http, $httpParamSerializerJQLike) {

    var mamay = document.getElementById("MaMay").value;
    $scope.loaiMay = {};
    $scope.loaiMay.MaMay = mamay;


    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);


    $http({
        method: "GET",
        url: "/Admin/LoaiMays/EditData/" + Number(id)

    }).then(function (res) {
        $scope.infor = res.data;
        $scope.loaiMay.TenMay = $scope.infor.TenMay;
        $scope.loaiMay.MoTa = $scope.infor.MoTa;
    });

    ///////////////////////////////////////////////////////////////////////////////////////
    $scope.addData = function () {
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.loaiMay.__RequestVerificationToken = Token;
        dataRequest = $scope.loaiMay;
        console.log(dataRequest);
        $http({
            method: 'POST',
            url: '/Admin/LoaiMays/EditLM',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Loi Sua ban ghi");
            }
            else {
                alert("Sua ban ghi thanh cong");
                window.location.href = "/Admin/LoaiMays/Index";
            }
        });
    };
});