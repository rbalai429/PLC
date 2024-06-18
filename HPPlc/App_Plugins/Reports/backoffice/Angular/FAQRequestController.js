app.controller('FAQRequestController', function ($scope, $http, $timeout, Comman, $location,
    notificationsService) {
    $scope.RequestList = [];
    $scope.StatusList = [];
    $scope.FollowUpList = [];
    $scope.RequestDownload = false;
    $scope.RequestListLoad = false;
    $scope.Title = "FAQ Request List";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.ImageValid = false;
    $scope.search = '';
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.status = '';
    $scope.pagination = [];
    const itemsPerPage = 8;
    $scope.totalPages = 0;
    $scope.currentPage = 1;
    $scope.currentStart = 1;
    $scope.start = 0;
    $scope.init = function () {
        $scope.GetRequestList($scope.currentPage, true);
    }
    $scope.model = {
        show: false,
        showing: false,
        close: false,
        name: ""
    };
    $scope.GetRequestList = function (Page, PageChange) {
        var sdate = '';
        var edate = '';
        if ($scope.startDate != '') {
            sdate = new Date($scope.startDate).toISOString();
        } if ($scope.endDate != '') {
            edate = new Date($scope.endDate).toISOString();
        }
        $scope.RequestListLoad = true;
        $http.get(Comman.GetFaqRequestListUrl() + "?Search=" + $scope.search + "&StartDate=" + sdate + "&EndDate=" + edate + "&Status=" + $scope.status + "&itemsPerPage=" + itemsPerPage + "&Page=" + Page + "&")
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RequestList = [];
                    $scope.RequestList = response.data.Result;
                    $scope.totalPages = response.data.TotalPage;
                    $scope.currentPage = Page;
                    $scope.start = 1;
                    if ($scope.StatusList.length == 0 || PageChange) {
                        $scope.StatusList = [];
                        var keys = [];
                        angular.forEach($scope.RequestList, function (item) {
                            var key = item["Status"];
                            if (keys.indexOf(key) === -1) {
                                keys.push(key);
                                $scope.StatusList.push(item);
                            }
                        });
                    }
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RequestListLoad = false;
            });
    }
    $scope.getNumber = function (totalPages) {
        var numbers = [];
        for (let i = 1; i <= totalPages; i++) {
            numbers.push(i);
        }

        return numbers;
    };
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
        $scope.GetRequestList($scope.currentPage, true);
    }
    $scope.ExportToExcel = function () {
        $scope.RequestDownload = true;
        window.open(Comman.DownloadFaqRequestListUrl() + "?Search=" + $scope.search + "&StartDate=" + $scope.startDate + "&EndDate=" + $scope.endDate + "&Status=" + $scope.status);
        $scope.RequestDownload = false;
    }
    $scope.FileSelected = function (files) {
        $scope.files = files;
        var file = $scope.files.files[0];
        if (file != null && file != undefined) {
            var allowedExtensions =
                /(\.xlsx|\.csv|\.xls)$/i;

            if (!allowedExtensions.exec(file.name)) {
                $scope.ImageValid = false;
                $("#fileupload").val("");
                notificationsService.warning("Warning", "Please Select Valid Excel File..");
                $scope.files = null;
                return false;
            }
            else {
                $scope.ImageValid = true;
            }
        }
    };
    $scope.FileUpload = function () {
        if ($scope.files != null && $scope.files.files != null && $scope.files.files.length > 0) {
            if ($scope.ImageValid) {
                $scope.FileUploaded = true;
                $http({
                    url: Comman.UploadFAQFilesUrl(),
                    method: "POST",
                    headers: { "Content-Type": undefined },
                    transformRequest: function () {
                        var formData = new FormData();
                        for (var i = 0; i < $scope.files.files.length; i++) {
                            formData.append("files[" + i + "]", $scope.files.files[i]);
                        }
                        return formData;
                    },
                    data: { files: $scope.files }
                }).then(function (response) {
                    if (response.status == 200 && response.data.StatusCode == 200) {
                        $scope.GetRequestList();
                        notificationsService.success("Success", "File Successfully Uploaded...");
                    } else {
                        notificationsService.error("Fail", response.data.Message);
                    }
                }).finally(function () {
                    $("#fileupload").val("");
                    $scope.FileUploaded = false;
                });;
            } else {
                notificationsService.warning("Warning", "Please Select Valid Excel File..");
            }
        } else {
            notificationsService.warning("Warning", "Please Select  File..");
        }
    }
    $scope.GetFollowUpList = function (Request) {
        if (Request != null && Request.RequestId > 0) {
            $scope.model.show = true;
            $scope.model.showing = true;
            $scope.model.name = Request.FullName;
            $http.get(Comman.GetRequestDetailsUrl() + "?RequestId=" + Request.RequestId)
                .then(function (response) {
                    if (response.status == 200 && response.data.StatusCode == 200) {
                        $scope.FollowUpList = [];
                        $scope.FollowUpList = response.data.Result;

                    } else {
                        alert(response.data.Message);
                    }
                }).finally(function () {
                    $scope.model.showing = false;
                });
        }
    }
    $scope.CheckIsValidStatus = function (status) {
        if (status != null && status != undefined && status != "") {
            return true;
        } else {
            return false;
        }
    }
    $scope.GetStatus = function (status) {
        if (status != null && status != undefined && status != "") {
            if (status.toLowerCase().includes("ok")) {
                return "umb-badge--success"
            } else if (status.toLowerCase().includes("fail")) {
                return "umb-badge--danger"
            } else {
                return "umb-badge--default"
            }
        } else {
            return "umb-badge--default"
        }
    }

});