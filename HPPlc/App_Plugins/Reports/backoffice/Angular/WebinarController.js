app.controller('ReportsController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.vm = this;
    $scope.ReportLoad = false;
    $scope.Title = "Webinar Reports";
    $scope.init = function () {

    }
    $scope.vm.webinar = {
        loading: false,
        indexSetSearcher: "",
    };
    $scope.vm.pagination = {
        pageIndex: 0,
        pageNumber: 1,
        totalPages: 1,
        pageSize: 5
    };
    $scope.vm.goToPage = goToPage;
    $scope.vm.search = search;
    $scope.vm.search();
    function goToPage(pageNumber) {
        $scope.vm.pagination.pageIndex = pageNumber - 1;
        $scope.vm.pagination.pageNumber = pageNumber;
        $scope.vm.search();
    }

    function search() {
        $scope.ReportLoad = true;
        var url = Comman.GetExpertsTalkUrl() + "?page=" + $scope.vm.pagination.pageNumber + "&pageSize=" + $scope.vm.pagination.pageSize + "&indexSetSearcher=" + $scope.vm.webinar.indexSetSearcher;
        $http.get(url).then(function (response) {
            $scope.vm.results = response.data;
            angular.forEach($scope.vm.results.Result, function (value, key) {
                if (value.MettingDate != null && value.MettingDate != undefined && value.MettingDate != "") {
                    value.MettingDate = new Date(parseInt(value.MettingDate.replace(/[^0-9]/g, "")));
                }
                if (value.JoinDate != null && value.JoinDate != undefined && value.JoinDate != "") {
                    value.JoinDate = new Date(parseInt(value.JoinDate.replace(/[^0-9]/g, "")));
                }
            })
            $scope.vm.pagination.PageNumber = $scope.vm.results.Page;
            $scope.vm.pagination.totalPages = $scope.vm.results.PageCount;

        }).finally(function () {
            $scope.ReportLoad = false;
        });
    }
    $scope.ExportToExcel = function () {
        $scope.ReportDownload = true;
        window.open(Comman.GetExportToExcelUmbracoAuthorizedApiUrl(), '_blank');
        $scope.ReportDownload = false;
    }
});


app.controller('WebinarListController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.WebinarsList = [];
    $scope.Query = "";
    $scope.init = function () {
        $scope.GetWebinarsList($scope.Query)
        $scope.Title = "Webinar List";
    }
    $scope.GetWebinarsList = function (query) {
        $scope.WebinarLoad = true;
        $http.get(Comman.GetWebinarsListUrl(query))
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.WebinarsList = response.data.Result;
                    angular.forEach($scope.WebinarsList, function (value, key) {
                        if (value.MettingDate != null && value.MettingDate != undefined && value.MettingDate != "") {
                            value.MettingDate = new Date(parseInt(value.MettingDate.replace(/[^0-9]/g, "")));
                        }
                    });
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.WebinarLoad = false;
            });
    }
    $scope.WebinarEdit = function (id) {
        console.log(id);
        window.open('/expert-talks/create-webinar/?id=' + id, 'sharer', 'toolbar=0,status=0,width=850,height=600');
    }
    $scope.WebinarDelete = function (id) {
        if (confirm("Are you sure to delete this record ?")) {
            $http.get(Comman.DeleteWebinarUrl() + "?id=" + id)
                .then(function (response) {
                    if (response.status == 200 && response.data.StatusCode == 200) {
                        if (response.data.Result == "Success") {
                            $scope.GetWebinarsList();
                        }
                    } else {
                        alert(response.data.Message);
                    }
                }).finally(function () {
                    $scope.ReportLoad = false;
                });
        }

    }
    $scope.FilterData = function (value) {
        $scope.datevalue = value;
        $scope.GetWebinarsList(value);
    }
    $scope.ResetData = function () {
        $scope.datevalue = "";
        $scope.GetWebinarsList("");
    }
});

app.controller('RegistrationReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.Query = "";
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetRegistrationList($scope.Query, $scope.StartDate, $scope.EndDate);
        $scope.Title = "Registration Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.GetRegistrationList = function (query, sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "1",
            Search: query,
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetRegistrationUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (query, sdt, edt) {
        if (query != null && query != undefined && query != '' && query != "") {
            query = query
        } else {
            query = "";
        }
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetRegistrationExportToExcelUmbracoAuthorizedApiUrl() + "?Search=" + query + "&StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function (value, sdt, edt) {
        $scope.inputValue = value;
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.GetRegistrationList(value, sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetRegistrationList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('SubscriptionsReportController', function ($scope, $http, $timeout, Comman, $location) {

    
    $scope.SubscriptionList = [];
    $scope.SubscriptionReportDownload = false;
    $scope.Query = "";
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetSubscriptionList($scope.Query, $scope.StartDate, $scope.EndDate)
        $scope.Title = "Subscriptions Report";
    }
    $scope.GetSubscriptionList = function (query, sdt, edt) {
        $scope.SubscriptionLoad = true;
        var Input = {
            QType: "1",
            Search: query,
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetSubscriptionUrl(), Input)
            .then(function (response) {
                //console.log(response);

                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.SubscriptionList = response.data.Result;
                    angular.forEach($scope.SubscriptionList, function (value, key) {
                        if (value.DOC != null && value.DOC != undefined && value.DOC != "") {
                            value.DOC = new Date(parseInt(value.DOC.replace(/[^0-9]/g, ""))).toLocaleDateString();
                        }
                    });
                } else {
                    alert(response.data.Message);
                }

            }).finally(function () {
                $scope.SubscriptionLoad = false;
            });
    }
    $scope.ExportToExcel = function (query, sdt, edt) {
        debugger
        if (query != null && query != undefined && query != '' && query != "") {
            query = query
        } else {
            query = "";
        }
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.SubscriptionReportDownload = true;
        window.open(Comman.GetSubscriptionExportToExcelUmbracoAuthorizedApiUrl() + "?Search=" + query + "&StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.SubscriptionReportDownload = false;
    }
    $scope.FilterData = function (value, sdt, edt) {
        $scope.inputValue = value;
        $scope.StartDate = sdt;
        $scope.EndDate = edt;
        $scope.GetSubscriptionList(value, sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.GetSubscriptionList("");
    }
});

app.controller('ReferralDetailReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.ReferralDetailList = [];
    $scope.ReferralDetailReportDownload = false;
    $scope.Query = "";
    $scope.init = function () {
        $scope.GetReferralDetailList($scope.Query)
        $scope.Title = "Referral Detail Report";
    }
    $scope.GetReferralDetailList = function (query) {
        $scope.ReferralDetailLoad = true;
        $http.get(Comman.GetReferralDetailUrl(query))
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.ReferralDetailList = response.data.Result;
                    angular.forEach($scope.ReferralDetailList, function (value, key) {
                        if (value.DOC != null && value.DOC != undefined && value.DOC != "") {
                            value.DOC = new Date(parseInt(value.DOC.replace(/[^0-9]/g, ""))).toLocaleDateString();
                        }
                    });
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.ReferralDetailLoad = false;
            });
    }
    $scope.ExportToExcel = function () {
        $scope.ReferralDetailReportDownload = true;
        window.open(Comman.GetReferralDetailUrlExportToExcelUmbracoAuthorizedApiUrl(), '_blank');
        $scope.ReferralDetailReportDownload = false;
    }
    $scope.FilterData = function (value) {
        $scope.inputValue = value;
        $scope.GetReferralDetailList(value);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.GetReferralDetailList("");
    }
});

app.controller('ReferralTransactionReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.ReferralTransactionList = [];
    $scope.ReferralTransactionReportDownload = false;
    $scope.Query = "";
    $scope.init = function () {
        $scope.GetReferralTransactionList($scope.Query)
        $scope.Title = "Referral Transaction Report";
    }
    $scope.GetReferralTransactionList = function (query) {
        $scope.ReferralTransactionLoad = true;
        $http.get(Comman.GetReferralTransactionUrl(query))
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.ReferralTransactionList = response.data.Result;
                    angular.forEach($scope.ReferralTransactionList, function (value, key) {
                        if (value.DOC != null && value.DOC != undefined && value.DOC != "") {
                            value.DOC = new Date(parseInt(value.DOC.replace(/[^0-9]/g, ""))).toLocaleDateString();
                        }
                        if (value.StartDate != null && value.StartDate != undefined && value.StartDate != "") {
                            value.StartDate = new Date(parseInt(value.StartDate.replace(/[^0-9]/g, ""))).toLocaleDateString();
                        }
                        if (value.EndDate != null && value.EndDate != undefined && value.EndDate != "") {
                            value.EndDate = new Date(parseInt(value.EndDate.replace(/[^0-9]/g, ""))).toLocaleDateString();
                        }
                    });
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.ReferralTransactionLoad = false;
            });
    }
    $scope.ExportToExcel = function () {
        $scope.ReferralTransactionReportDownload = true;
        window.open(Comman.GetReferralTransactionExportToExcelUmbracoAuthorizedApiUrl(), '_blank');
        $scope.ReferralTransactionReportDownload = false;
    }
    $scope.FilterData = function (value) {
        $scope.inputValue = value;
        $scope.GetReferralTransactionList(value);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.GetReferralTransactionList("");
    }
});



/*Start Deepak Changes*/
app.controller('UserDownloadDataReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.Query = "";
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.Status = "";
    $scope.init = function () {
        $scope.GetDownloadDataList($scope.StartDate, $scope.EndDate);
        $scope.Title = "User Download Data Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }


    $scope.GetDownloadDataList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "1",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetDownlaodDataListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetDownlaodDataExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.ExportToExcelAllData = function (sdt, edt) {
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetDownlaodDataExportToExcelAllUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.UserFilterData = function (userid, sdt, edt) {

        //$scope.Status = userid;
        //$scope.StartDate = sdt;
        //$scope.EndDate = edt;

        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetDownlaodDataByUserExportToExcelUmbracoAuthorizedApiUrl() + "?&userId=" + userid + "&StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
        //alert(userid + ":" + sdt + ":" +edt)
        //$scope.GetDownloadDataList(userid,sdt, edt);
    }
    $scope.FilterData = function (sdt, edt) {
        $scope.StartDate = sdt;
        $scope.EndDate = edt;
        $scope.GetDownloadDataList(sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetDownloadDataList("", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('ReferralDetailsReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetReferralList($scope.StartDate, $scope.EndDate);
        $scope.Title = "Referral Details Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.GetReferralList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "2",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetReferralListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetReferralExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function (sdt, edt) {
        
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.GetReferralList(sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetReferralList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('NotificationDataReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetnotificationList($scope.StartDate, $scope.EndDate);
        $scope.Title = "Notification Data Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.GetnotificationList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "3",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetNotificationListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {
        //if (query != null && query != undefined && query != '' && query != "") {
        //    query = query
        //} else {
        //    query = "";
        //}
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetNotificationExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function (sdt, edt) {
        
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.GetnotificationList(sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetnotificationList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('OTPDataReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetOTPList($scope.StartDate, $scope.EndDate);
        $scope.Title = "OTP Data Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.GetOTPList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "4",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetOTPListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {

        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetOTPExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function (sdt, edt) {
        
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.GetOTPList( sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetOTPList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('URLRedirectionEntryTableReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.Query = ""; // Old URL
    $scope.QType = ""; // New Url



    $scope.SubmitData = function () {
        var oldUrl = $("#inpuOldUrl").val();
        var newUrl = $("#inpuNewUrl").val();
        //alert(oldUrl);
        var ListItem = {};
        ListItem = {
            OldUrl: oldUrl,
            NewUrl: newUrl
        };
        var data = $.param(ListItem);
        var AddRequest = new FormData();
        AddRequest.append('OldUrl', oldUrl);
        AddRequest.append('NewUrl', newUrl);
        $http({
            method: "POST",
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/InsertURLManipulationList",
            dataType: 'json',
            data: { "OldUrl": oldUrl, "NewUrl": newUrl },
            processData: false,
            contentType: false,
            headers: { "Content-Type": "application/json" }
        }).then(function (response) {
            $("#inpuOldUrl").val("");
            $("#inpuNewUrl").val("");
            alert("url added successfully");
        }, function (error) {
        });





    }
});

app.controller('WorkSheetBulkUploadController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegisterTempList = [];
    $scope.ReportDownload = false;
    $scope.Title = "Import Excel File";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.CreateZipStart = false;
    $scope.ImageValid = false;

    $http({
        method: "POST",
        url: "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAgeGoupe_Language_Master",
        dataType: 'json',
        processData: false,
        contentType: false,
        headers: { "Content-Type": "application/json" }
    }).then(function (response) {
        var collectionString = response.data;
        var AgeGroup = collectionString.split('|')[0];
        var Language = collectionString.split('|')[1];
        var ProgramType = collectionString.split('|')[2];
        $("#ddlAgeGroupe").html(AgeGroup);
        $("#ddlLanguage").html(Language);
        $("#ddlProgramType").html(ProgramType);
    }, function (error) {
    });
    $scope.FileSelected = function (files) {
        $scope.files = files;
        var file = $scope.files.files[0];
        if (file != null && file != undefined) {
            var allowedExtensions =
                /(\.xlsx|\.csv|\.xls)$/i;
            if (!allowedExtensions.exec(file.name)) {
                $scope.ImageValid = false;
                $("#fileupload").val("");
                alert("Warning", "Please Select Valid Excel File..");
                $scope.files = null;
                return false;
            }
            else {
                $scope.ImageValid = true;
            }
        }
    };
    $scope.FileUpload = function () {
        $("#dvLession").hide();
        $("#dvStructure").hide();

        $("#hdnjson").val("");
        $("#div_msg").html("");
        $("#dvFinalButton").hide();
        $("#dvOffer").hide();

        $scope.FileUploaded = true;
        var pgt = $("#ddlProgramType").val();
        if (pgt === "1" || pgt === "2" || pgt === "3") {
        }
        else {
            pgt = $("#ddlProgramType :selected").text();
        }
        $http({
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/upload_excel",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: function () {
                var formData = new FormData();
                formData.append("files[0]", $("#fileupload").get(0).files[0]);
                formData.append("ProgramType", $("#ddlProgramType").val())
                return formData;
            },
            data: { files: $("#fileupload").get(0).files[0], "ProgramType": $("#ddlProgramType").val() }
        }).then(function (response) {

            //var pgtype = $("#ddlProgramType").val();
            if (pgt === "1") {
                var LessionList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(LessionList));
                $("#dvFinalButton").show();
                var lessionItem = "";
                $.each(LessionList, function (i, item) {
                    lessionItem = lessionItem + "<tr>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SubjectID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WorksheetTitle + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Title + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SubTitle + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].ShareContent + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SharingText + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WeekID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].TopicID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Description + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsGuestUserSheet + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].CBSEContentIncluded + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsQuizWorksheet + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsEnableforDetailsPage + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DescriptionPageContent + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DesktopImageID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DesktopImageWebpID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].MobileImageID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].MobileIamgeWebpID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WhatsAppBannerID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Subscription + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].UploadPDF + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].UploadPreviewPDF + "</td>";
                    lessionItem = lessionItem + "</tr>";
                });
                $("#tbodyLession").html("");
                $("#tbodyLession").html(lessionItem);
                $("#dvLession").show();
                $("#dvStructure").hide();
                $("#dvTeacher").hide();
                $("#dvOffer").hide();
            }
            else if (pgt === "2") {
                var StructureList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(StructureList));
                $("#dvFinalButton").show();
                var StructureItem = "";
                $.each(StructureList, function (i, item) {
                    StructureItem = StructureItem + "<tr>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].SubjectID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].TopicID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].WorksheetTitle + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].umbracoUrlAlias + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Title + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].SubTitle + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Description + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].IsGuestUserSheet + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].IsEnableForDetailsPage + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Paid + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].RankingIndex + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].DesktopImageID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].DesktopImageWebpID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].MobileImageID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].MobileIamgeWebpID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].UploadPDF + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].UploadPreviewPDF + "</td>";
                    StructureItem = StructureItem + "</tr>";
                });
                $("#tbodyStructure").html("");
                $("#tbodyStructure").html(StructureItem);
                $("#dvLession").hide();
                $("#dvStructure").show();
                $("#dvTeacher").hide();
                $("#dvOffer").hide();
            }
            else if (pgt === "3") {
                var TeacherList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(TeacherList));
                $("#dvFinalButton").show();
                var TeacherItem = "";
                $.each(TeacherList, function (i, item) {
                    TeacherItem = TeacherItem + "<tr>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].WorksheetTitle + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].umbracoUrlAlias + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Title + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].SubTitle + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Description + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].NoofDays + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].IsGuestUserSheet + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].IsEnableForDetailsPage + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Paid + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].RankingIndex + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].DesktopImageID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].DesktopImageWebpID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].MobileImageID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].MobileIamgeWebpID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].UploadPDF + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].UploadPreviewPDF + "</td>";
                    TeacherItem = TeacherItem + "</tr>";
                });
                $("#tbodyTeacher").html("");
                $("#tbodyTeacher").html(TeacherItem);
                $("#dvLession").hide();
                $("#dvStructure").hide();
                $("#dvTeacher").show();
                $("#dvOffer").hide();
            }
            else {
                var OfferList = response.data.Result;
                console.log(OfferList);
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(OfferList));
                $("#dvFinalButton").show();
                var OfferItem = "";
                $.each(OfferList, function (i, item) {
                    OfferItem = OfferItem + "<tr>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].WorksheetTitle + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].umbracoUrlAlias + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].Title + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].SubTitle + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].Description + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].IsEnableForDetailsPage + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].Paid + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].DesktopImageID + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].DesktopImageWebpID + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].MobileImageID + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].MobileIamgeWebpID + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].UploadPDF + "</td>";
                    OfferItem = OfferItem + "<td>" + OfferList[i].UploadPreviewPDF + "</td>";
                    OfferItem = OfferItem + "</tr>";
                });
                $("#tbodyoffer").html("");
                $("#tbodyoffer").html(OfferItem);
                $("#dvLession").hide();
                $("#dvStructure").hide();
                $("#dvTeacher").hide();
                $("#dvOffer").show();
            }
            console.log($("#hdnjson").val())

        }).finally(function () {
            $("#fileupload").val("");
            $scope.FileUploaded = false;
        });;
    }

    $("#aUploadWorksheet").click(function () {
        $(this).hide();
        $("#div_msg").html("Task is in progress!!");
        $("#loader").show();
        var ProgramType = $("#ddlProgramType").val();
        var AgeGroupe = $("#ddlAgeGroupe").val();
        var Language = $("#ddlLanguage").val();
        var JsonData = $("#hdnjson").val();
        if (ProgramType === null || ProgramType === "0" || ProgramType === "" || ProgramType === undefined) {
            alert("Please select program type from dropdown");
            return false;
        }
        if (AgeGroupe === null || AgeGroupe === "0" || AgeGroupe === "" || AgeGroupe === undefined) {
            alert("Please select age group from dropdown");
            return false;
        }
        if (Language === null || Language === "0" || Language === "" || Language === undefined) {
            alert("Please select language from dropdown");
            return false;
        }
        if (JsonData === null || JsonData === "0" || JsonData === "" || JsonData === undefined) {
            alert("Incorrect worksheet json file. Please re-try to upload program type excel");
            return false;
        }
        if (ProgramType === "1" || ProgramType === "2" || ProgramType === "3") {
        }
        else {
            ProgramType = $("#ddlProgramType :selected").text();
        }
        $http({
            method: "POST",
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/upload_Worksheet",
            dataType: 'json',
            data: { "ProgramType": ProgramType, "AgeGroupe": AgeGroupe, "Language": Language, "JsonData": JsonData },
            processData: false,
            contentType: false,
            headers: { "Content-Type": "application/json" }
        }).then(function (response) {
            console.log(response);
            $("#div_msg").html(response.data);
            if (response.data == "ok") {

                $("#tbodyStructure").hide();
                $("#tbodyStructure").hide();
                $("#tbodyLession").hide();
                $("#tbodyLession").hide();
                $("#tbodyTeacher").hide();
                $("#tbodyTeacher").hide();
                $("#dvLession").hide();
                $("#dvStructure").hide();
                $("#dvTeacher").hide();
                $("#dvFinalButton").hide();

                $("#aUploadWorksheet").hide();
                $("#div_msg").hide();

                $("#dvOffer").hide();
                $("#tbodyoffer").hide();
                alert("file has been uploaded!!");
            }
            else if (response.data == "") {
                $("#aUploadWorksheet").show();
                $("#div_msg").hide();
                alert("fail");
            }
            else {
                $("#aUploadWorksheet").show();
                $("#div_msg").hide();
                alert(response.data);
            }
            
        }, function (error) {
        });

    });

});
app.controller('ImageByNodeController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.Title = "Image By Node";
    $("#aExportImageIntoExcel").click(function () {
        window.open("/umbraco/backoffice/api/ReportsAuthorizeApi/ExportNodeByImage?Node=" + $("#inptImageByNode").val(), '_blank');
    });
});

app.controller('SendWhatsAppNotificationDataReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.GetSendWhatsAppNotificationList($scope.StartDate, $scope.EndDate);
        $scope.Title = "Send WhatsApp Notification Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.GetSendWhatsAppNotificationList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "5",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetSendWhatsAppNotificationListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {
        //if (query != null && query != undefined && query != '' && query != "") {
        //    query = query
        //} else {
        //    query = "";
        //}
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetSendWhatsAppNotificationExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function ( sdt, edt) {
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.GetSendWhatsAppNotificationList( sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.GetSendWhatsAppNotificationList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
    $scope.FileUpload = function () {
        $scope.FileUploaded = true;
        $http({
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/upload_excel_SendWhatsAppNotificatoin",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: function () {
                var formData = new FormData();
                formData.append("files[0]", $("#fileupload").get(0).files[0]);
                return formData;
            },
            data: { files: $("#fileupload").get(0).files[0] }
        }).then(function (response) {
            if (response.data.StatusCode == 200) {
                var message = response.data.Message;
                if (message == "Y") {
                    alert("Data inserted successfully");
                }
                else {
                    alert("Error");
                }
            }
            console.log(response);
        }).finally(function () {
            $("#fileupload").val("");
            $scope.FileUploaded = false;
        });;
    }
});
app.controller('AgeGroupSynonimsNameForAdvanceSearchController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.Query = ""; // Old URL
    $scope.QType = ""; // New Url



    $scope.SubmitData = function () {
        var ClassName = $("#inpuClassName").val();
        var SynonymsName = $("#inpuSynonymsName").val();

        $http({
            method: "POST",
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/InsertAgeGroupeAdvancedSearch",
            dataType: 'json',
            data: { "ClassName": ClassName, "SynonymsName": SynonymsName },
            processData: false,
            contentType: false,
            headers: { "Content-Type": "application/json" }
        }).then(function (response) {
            console.log(response.data);
            var rowid = response.data;
            if (rowid === "0") {
                alert("Record already exists");
            }
            else {
                $("#inpuClassName").val("");
                $("#inpuSynonymsName").val("");
                alert("Record added successfully");
            }
        }, function (error) {
        });
    }
    $scope.ExportToExcel = function () {

        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetAgeGroupSynonimsNameForAdvanceSearchExportToExcelUmbracoAuthorizedApiUrl(), '_blank');
        $scope.RegistrationReportDownload = false;
    }
});

app.controller('NoRecordFoundSearchReportController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegistrationList = [];
    $scope.filteredItemsList = [];
    $scope.RegistrationReportDownload = false;
    $scope.StartDate = "";
    $scope.EndDate = "";
    $scope.init = function () {
        $scope.NoRecordFoundList($scope.StartDate, $scope.EndDate);
        $scope.Title = "No Record Found Search Data Report";
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
        $scope.currentPage = 1,
            $scope.itemsPerPage = 10,
            $scope.totalItems = 5;
        $scope.maxSize = 5;
    }
    $scope.NoRecordFoundList = function (sdt, edt) {
        $scope.RegistrationLoad = true;
        var Input = {
            QType: "7",
            StartDate: sdt,
            EndDate: edt
        }
        $http.post(Comman.GetNoRecordFoundListUrl(), Input)
            .then(function (response) {
                console.log(response);
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegistrationList = response.data.Result;
                    $scope.filteredItemsList = response.data.Result;
                    $scope.totalItems = $scope.RegistrationList.length;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegistrationLoad = false;
            });
    }
    $scope.ExportToExcel = function (sdt, edt) {
        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {
            sdt = $.datepicker.formatDate('dd-M-yy', sdt);
            sdt = sdt
        } else {
            sdt = "";
        }
        if (edt != null && edt != undefined && edt != '' && edt != "") {
            edt = $.datepicker.formatDate('dd-M-yy', edt);
            edt = edt
        } else {
            edt = "";
        }
        $scope.RegistrationReportDownload = true;
        window.open(Comman.GetNoRecordFoundExportToExcelUmbracoAuthorizedApiUrl() + "?StartDate=" + sdt + "&EndDate=" + edt, '_blank');
        $scope.RegistrationReportDownload = false;
    }
    $scope.FilterData = function (sdt, edt) {
        //$scope.inputValue = value;
        $scope.StartDate = sdt;
        $scope.EndDate = edt;

        $scope.NoRecordFoundList(sdt, edt);
    }
    $scope.ResetData = function () {
        $scope.inputValue = "";
        $scope.StartDate = "";
        $scope.EndDate = "";
        $scope.NoRecordFoundList("", "", "");
    }
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    }
});

app.controller('WorkSheetBulkUploadUpdateController', function ($scope, $http, $timeout, Comman, $location) {
    $scope.RegisterTempList = [];
    $scope.ReportDownload = false;
    $scope.Title = "Import Excel File";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.CreateZipStart = false;
    $scope.ImageValid = false;

    $http({
        method: "POST",
        url: "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAgeGoupe_Language_Master",
        dataType: 'json',
        processData: false,
        contentType: false,
        headers: { "Content-Type": "application/json" }
    }).then(function (response) {
        var collectionString = response.data;
        var AgeGroup = collectionString.split('|')[0];
        var ProgramType = collectionString.split('|')[2];
        $("#ddlAgeGroupe").html(AgeGroup);
        $("#ddlProgramType").html(ProgramType);
    }, function (error) {
    });
    $scope.FileSelected = function (files) {
        $scope.files = files;
        var file = $scope.files.files[0];
        if (file != null && file != undefined) {
            var allowedExtensions =
                /(\.xlsx|\.csv|\.xls)$/i;
            if (!allowedExtensions.exec(file.name)) {
                $scope.ImageValid = false;
                $("#fileupload").val("");
                alert("Warning", "Please Select Valid Excel File..");
                $scope.files = null;
                return false;
            }
            else {
                $scope.ImageValid = true;
            }
        }
    };
    $scope.FileUpload = function () {


        var PT = $("#ddlProgramType").val();
        var AG = $("#ddlAgeGroupe").val();
        if (PT === null || PT === "0" || PT === "" || PT === undefined) {
            alert("Please select program type");
            return;
        }
        if (AG === null || AG === "0" || AG === "" || AG === undefined) {
            alert("Please select age group");
            return;
        }


        $("#dvLession").hide();
        $("#dvStructure").hide();
        $("#hdnjson").val("");
        $("#div_msg").html("");
        $("#dvFinalButton").hide();
        $scope.FileUploaded = true;
        $http({
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/upload_excel_bulk_update",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: function () {
                var formData = new FormData();
                formData.append("files[0]", $("#fileupload").get(0).files[0]);
                formData.append("ProgramType", $("#ddlProgramType").val())
                return formData;
            },
            data: { files: $("#fileupload").get(0).files[0], "ProgramType": $("#ddlProgramType").val() }
        }).then(function (response) {

            var pgtype = $("#ddlProgramType").val();
            if (pgtype === "1") {
                var LessionList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(LessionList));
                $("#dvFinalButton").show();
                var lessionItem = "";
                $.each(LessionList, function (i, item) {
                    lessionItem = lessionItem + "<tr>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].NodeID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsPublished + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].languageKey + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].languageName + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].AgeGroup + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SubjectID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WeekID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].TopicID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SubjectName + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WorksheetTitle + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Title + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SubTitle + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].ShareContent + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].SharingText + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Description + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsGuestUserSheet + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].CBSEContentIncluded + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsQuizWorksheet + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].IsEnableforDetailsPage + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DescriptionPageContent + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DesktopImageID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].DesktopImageWebpID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].MobileImageID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].MobileIamgeWebpID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].WhatsAppBannerID + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].Subscription + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].UploadPDF + "</td>";
                    lessionItem = lessionItem + "<td>" + LessionList[i].UploadPreviewPDF + "</td>";
                    lessionItem = lessionItem + "</tr>";
                });
                $("#tbodyLession").html("");
                $("#tbodyLession").html(lessionItem);
                $("#dvLession").show();
                $("#dvStructure").hide();
                $("#dvTeacher").hide();
            }
            else if (pgtype === "2") {
                var StructureList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(StructureList));
                $("#dvFinalButton").show();
                var StructureItem = "";
                $.each(StructureList, function (i, item) {
                    StructureItem = StructureItem + "<tr>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].NodeID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].IsPublished + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].languageKey + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].languageName + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].AgeGroup + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].SubjectID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].SubjectName + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].TopicID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].WorksheetTitle + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].umbracoUrlAlias + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Title + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].SubTitle + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Description + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].IsGuestUserSheet + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].IsEnableForDetailsPage + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].Paid + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].RankingIndex + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].DesktopImageID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].DesktopImageWebpID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].MobileImageID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].MobileIamgeWebpID + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].UploadPDF + "</td>";
                    StructureItem = StructureItem + "<td>" + StructureList[i].UploadPreviewPDF + "</td>";
                    StructureItem = StructureItem + "</tr>";
                });
                $("#tbodyStructure").html("");
                $("#tbodyStructure").html(StructureItem);
                $("#dvLession").hide();
                $("#dvStructure").show();
                $("#dvTeacher").hide();
            }
            else if (pgtype === "3") {
                var TeacherList = response.data.Result;
                $("#hdnjson").val("");
                $("#hdnjson").val(JSON.stringify(TeacherList));
                $("#dvFinalButton").show();
                var TeacherItem = "";
                $.each(TeacherList, function (i, item) {
                    TeacherItem = TeacherItem + "<tr>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].NodeID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].IsPublished + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].languageKey + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].languageName + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].AgeGroup + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].WorksheetTitle + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].umbracoUrlAlias + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Title + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].SubTitle + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Description + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].NoofDays + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].IsGuestUserSheet + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].IsEnableForDetailsPage + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].Paid + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].RankingIndex + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].DesktopImageID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].DesktopImageWebpID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].MobileImageID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].MobileIamgeWebpID + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].UploadPDF + "</td>";
                    TeacherItem = TeacherItem + "<td>" + TeacherList[i].UploadPreviewPDF + "</td>";
                    TeacherItem = TeacherItem + "</tr>";
                });
                $("#tbodyTeacher").html("");
                $("#tbodyTeacher").html(TeacherItem);
                $("#dvLession").hide();
                $("#dvStructure").hide();
                $("#dvTeacher").show();
            }

            //console.log($("#hdnjson").val())

        }).finally(function () {
            $("#fileupload").val("");
            $scope.FileUploaded = false;
        });;
    }
    $scope.ExportToExcelForUpdate = function (sdt, edt) {
        var PT = $("#ddlProgramType").val();
        var AG = $("#ddlAgeGroupe").val();
        if (PT === null || PT === "0" || PT === "" || PT === undefined) {
            alert("Please select program type");
            return;
        }
        if (AG === null || AG === "0" || AG === "" || AG === undefined) {
            alert("Please select age group");
            return;
        }
        window.open("/umbraco/backoffice/api/ReportsAuthorizeApi/WorkSheetExportToExcel" + "?ProgramType=" + $("#ddlProgramType").val() + "&AgeGroup=" + $("#ddlAgeGroupe").val(), '_blank');

    }
    $("#aUploadWorksheet").click(function () {
        $(this).hide();
        $("#div_msg").html("Task is in progress!!");
        $("#loader").show();
        var ProgramType = $("#ddlProgramType").val();
        var AgeGroupe = $("#ddlAgeGroupe").val();
        var Language = "hi";// $("#ddlLanguage").val();
        var JsonData = $("#hdnjson").val();
        if (ProgramType === null || ProgramType === "0" || ProgramType === "" || ProgramType === undefined) {
            alert("Please select program type from dropdown");
            return false;
        }
        if (AgeGroupe === null || AgeGroupe === "0" || AgeGroupe === "" || AgeGroupe === undefined) {
            alert("Please select age group from dropdown");
            return false;
        }
        //if (Language === null || Language === "0" || Language === "" || Language === undefined) {
        //    alert("Please select language from dropdown");
        //    return false;
        //}
        if (JsonData === null || JsonData === "0" || JsonData === "" || JsonData === undefined) {
            alert("Incorrect worksheet json file. Please re-try to upload program type excel");
            return false;
        }
        $http({
            method: "POST",
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/upload_Worksheet_update",
            dataType: 'json',
            data: { "ProgramType": ProgramType, "AgeGroupe": AgeGroupe, "Language": Language, "JsonData": JsonData },
            processData: false,
            contentType: false,
            headers: { "Content-Type": "application/json" }
        }).then(function (response) {
            console.log(response);
            $("#div_msg").html(response.data);
            if (response.data == "ok") {

                $("#tbodyStructure").hide();
                $("#tbodyStructure").hide();
                $("#tbodyLession").hide();
                $("#tbodyLession").hide();
                $("#tbodyTeacher").hide();
                $("#tbodyTeacher").hide();
                $("#dvLession").hide();
                $("#dvStructure").hide();
                $("#dvTeacher").hide();
                $("#dvFinalButton").hide();

                $("#aUploadWorksheet").hide();
                $("#div_msg").hide();

                alert("file has been uploaded!!");
            }
            else if (response.data == "") {
                $("#aUploadWorksheet").show();
                $("#div_msg").hide();
                alert("fail");
            }
            else {
                $("#aUploadWorksheet").show();
                $("#div_msg").hide();
                alert(response.data);
            }

        }, function (error) {
        });

    });

});


app.controller('UserTransactionReportController', function ($scope, $http, $timeout, Comman, $location) {

    $scope.RegistrationList = [];

    $scope.filteredItemsList = [];

    $scope.RegistrationReportDownload = false;

    $scope.StartDate = "";

    $scope.EndDate = "";

    $scope.init = function () {

        $scope.UserTransactionList($scope.StartDate, $scope.EndDate);

        $scope.Title = "User Transaction Report";

        $scope.bigTotalItems = 175;

        $scope.bigCurrentPage = 1;

        $scope.currentPage = 1,

            $scope.itemsPerPage = 10,

            $scope.totalItems = 5;

        $scope.maxSize = 5;

    }

    $scope.UserTransactionList = function (sdt, edt) {

        $scope.RegistrationLoad = true;

        var Input = {

            QType: "8",

            StartDate: sdt,

            EndDate: edt

        }

        $http.post("/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllUserTransactionList", Input)

            .then(function (response) {

                console.log(response);

                if (response.status == 200 && response.data.StatusCode == 200) {

                    $scope.RegistrationList = response.data.Result;

                    $scope.filteredItemsList = response.data.Result;

                    $scope.totalItems = $scope.RegistrationList.length;

                } else {

                    alert(response.data.Message);

                }

            }).finally(function () {

                $scope.RegistrationLoad = false;

            });

    }

    $scope.ExportToExcel = function (sdt, edt) {

        if (sdt != null && sdt != undefined && sdt != '' && sdt != "") {

            sdt = $.datepicker.formatDate('dd-M-yy', sdt);

            sdt = sdt

        } else {

            sdt = "";

        }

        if (edt != null && edt != undefined && edt != '' && edt != "") {

            edt = $.datepicker.formatDate('dd-M-yy', edt);

            edt = edt

        } else {

            edt = "";

        }

        $scope.RegistrationReportDownload = true;

        window.open("/umbraco/backoffice/api/ReportsAuthorizeApi/UserTransactionExportToExcel?StartDate=" + sdt + "&EndDate=" + edt, '_blank');

        $scope.RegistrationReportDownload = false;

    }

    $scope.FilterData = function (sdt, edt) {

        //$scope.inputValue = value;

        $scope.StartDate = sdt;

        $scope.EndDate = edt;

        $scope.UserTransactionList(sdt, edt);

    }

    $scope.ResetData = function () {

        $scope.inputValue = "";

        $scope.StartDate = "";

        $scope.EndDate = "";

        $scope.UserTransactionList("", "", "");

    }

    $scope.setPage = function (pageNo) {

        $scope.currentPage = pageNo;

    };

    $scope.pageChanged = function () {

        console.log('Page changed to: ' + $scope.currentPage);

    };

    $scope.setItemsPerPage = function (num) {

        $scope.itemsPerPage = num;

        $scope.currentPage = 1; //reset to first page

    }

});

/*Deepak Deepak Changes*/