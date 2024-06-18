app.controller('CouponCodeLogController', function ($scope, $http, $timeout, Comman, $location, notificationsService, userService) {
    $scope.CouponCodeLogList = [];
    $scope.Title="Coupon Code Log"
    $scope.pagination = [];
    $scope.search = "";
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.itemsPerPage = "25";
    $scope.totalPages = 0;
    $scope.currentPage = 1;
    $scope.currentStart = 1;
    $scope.start = 0;
    $scope.RequestDownload = false;
    $scope.User;
    $scope.init = function () {
        userService.getCurrentUser().then(function (user) {
            $scope.User = user;
        });
        var usert = userService.getCurrentUser();
        $scope.GetCouponCodeLogList($scope.currentPage, $scope.itemsPerPage, $scope.search);
    }
    $scope.GetCouponCodeLogList = function (Page, itemsPerPage,search) {
        $scope.itemsPerPage = itemsPerPage
        $scope.search = search
        $scope.CouponCodeLogLoad = true;
        var sdate = '';
        var edate = '';
        if ($scope.startDate != '') {
            sdate = new Date($scope.startDate).toISOString();
        } if ($scope.endDate != '') {
            edate = new Date($scope.endDate).toISOString();
        }
        var Input = {
            QType: "1",
            Search: $scope.search,
            itemsPerPage: $scope.itemsPerPage,
            Page: Page,
            Status: '',
            StartDate: sdate,
            EndDate: edate,
        }
        $http.post(Comman.GetCouponCodeLogListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.CouponCodeLogList = [];
                    $scope.CouponCodeLogList = response.data.Result;
                    $scope.totalPages = response.data.TotalPage;
                    $scope.currentPage = Page;
                    $scope.start = 1;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.CouponCodeLogLoad = false;
            });
    }

    $scope.rangeCreator = function (minVal, maxVal) {
        var arr = [];
        for (var i = minVal; i <= maxVal; i++) {
            arr.push(i);
        }
        return arr;
    };
    $scope.ResetSearch = function () {
        $scope.search = "";
        $scope.GetCouponCodeLogList($scope.currentPage, $scope.itemsPerPage, $scope.search);
    }
    $scope.ExportToExcel = function () {
        $scope.RequestDownload = true;
        window.open(Comman.DownloadCouponCodeLogListUrl() + "?Search=" + $scope.search + "&StartDate=" + $scope.startDate + "&EndDate=" + $scope.endDate);
        $scope.RequestDownload = false;
    }
});