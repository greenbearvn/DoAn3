var myApp = angular.module("myApp", []).config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);;

myApp.controller("nphIndex", nphIndex);
function nphIndex($scope, $http) {

    $scope.DeleteButton = function (id) {
        var deleteItem = window.confirm("Bạn có muốn xóa nhà phát hành này không ?")
        if (deleteItem == true) {

            $scope.MaNPH = Number(id);
            window.location.reload();
        }
        $http({
            method: "post",
            url: "/Admin/NhaPhatHanhs/DeleteConfirmed/?id=" + $scope.MaNPH,
            dataType: "json",
            data: JSON.stringify($scope.MaNPH)

        }).then(function (res) {
            $scope.alert = res.data;
            if ($scope.alert == true) {
                alert("Xóa nhà phát hành thành công")
            }
            else {
                alert("Xóa nhà phát hành không thành công")
            }
        });

    }
    $http({
        method: "get",
        url: "/Admin/NhaPhatHanhs/GetAllNPH",

    }).then(function (res) {
        $scope.datas = res.data;

    });
}

myApp.controller('Create', function ($scope, $http, $httpParamSerializerJQLike) {
    $scope.nphs = {};

    $scope.addData = function () {

       
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.nphs.__RequestVerificationToken = Token;
        dataRequest = $scope.nphs;
        console.log(dataRequest);
        $http({
            method: 'post',
            url: '/Admin/NhaPhatHanhs/CreateNPH',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Thêm nhà phát hành không thành công");
            }
            else {
                alert("Thêm nhà phát hành thành công");
                window.location.href = "/Admin/NhaPhatHanhs/Index";
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

myApp.controller('Edit', function ($scope, $http, $httpParamSerializerJQLike) {

    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);

    $scope.nphs = {};
    $scope.nphs.MaNPH = id;

    $http({
        method: "GET",
        url: "/Admin/NhaPhatHanhs/GetDetailNPH/" + Number(id)

    }).then(function (res) {
        $scope.infor = res.data;
        $scope.nphs.TenNPH = $scope.infor.TenNPH;
        $scope.nphs.TruSo = $scope.infor.TruSo;
    });

    ///////////////////////////////////////////////////////////////////////////////////////
    $scope.addData = function () {
        
        Token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $scope.nphs.__RequestVerificationToken = Token;
        dataRequest = $scope.nphs;
        console.log(dataRequest);
        $http({
            method: 'POST',
            url: '/Admin/NhaPhatHanhs/EditNPH',
            data: $httpParamSerializerJQLike(dataRequest),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }

        }).then(function (res) {
            $scope.data = res.data;
            if ($scope.data == false) {
                alert("Sửa nhà phát hành không thành công");
            }
            else {
                alert("Sửa nhà phát hành thành công");
                window.location.href = "/Admin/NhaPhatHanhs/Index";
            }
        });
    };

    
});