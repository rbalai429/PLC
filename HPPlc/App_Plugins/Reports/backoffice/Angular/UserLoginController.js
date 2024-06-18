app.controller('UserLoginController', function ($scope, $http, $timeout, Comman, $location, notificationsService) {
    $scope.UserLoginList = [];
    $scope.Title = "User Login Reports";
    $scope.search = "";
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.status = '';
    $scope.pagination = [];
    $scope.itemsPerPage = "25";
    $scope.totalPages = 0;
    $scope.currentPage = 1;
    $scope.currentStart = 1;
    $scope.start = 0;
    $scope.init = function () {
        $scope.GetUserLoginList($scope.currentPage, $scope.itemsPerPage, $scope.status);
    }
    $scope.GetUserLoginList = function (Page, itemsPerPage, status) {
        $scope.status = status
        $scope.itemsPerPage = itemsPerPage
        $scope.GetUserLoginLoad = true;
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
            Status: $scope.status,
            StartDate: sdate,
            EndDate: edate,
        }
        $http.post(Comman.GetUserLoginListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.UserLoginList = [];
                    $scope.UserLoginList = response.data.Result;
                    $scope.totalPages = response.data.TotalPage;
                    $scope.currentPage = Page;
                    $scope.start = 1;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.GetUserLoginLoad = false;
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
        $scope.search = '';
        $scope.startDate = '';
        $scope.endDate = '';
        $scope.status = '';
        $scope.GetUserLoginList($scope.currentPage, $scope.itemsPerPage, $scope.status);
    }
    $scope.ExportToExcel = function () {
        $scope.RequestDownload = true;
        window.open(Comman.DownloadUserLoginListUrl() + "?Search=" + $scope.search + "&StartDate=" + $scope.startDate + "&EndDate=" + $scope.endDate + "&Status=" + $scope.status);
        $scope.RequestDownload = false;
    }
});