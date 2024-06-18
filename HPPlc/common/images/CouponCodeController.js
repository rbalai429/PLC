app.controller('CouponCodeController', function ($scope, $http, $timeout, Comman, $location, notificationsService, userService) {
    $scope.CouponCodeList = [];
    var currntDate = new Date();
    currntDate.setDate(currntDate.getDate() - 1);
    $scope.cstartDate = currntDate;
    $scope.EndDateErrorMessage = '';
    $scope.onlyNumbers = /^[1-9]+[0-9]*$/;
    $scope.Title = "Coupon Code";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.ImageValid = false;
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
    $scope.User;
    $scope.IsActing = false;
    $scope.IsStartDateValid = false;
    $scope.SelectAll = false;
    $scope.SelectedListForDelete = []
    $scope.model = {
        show: false,
        showing: false,
        close: false,
        saving: false,
    };
    $scope.modelDetails = {
        show: false,
        showing: false,
        close: false,
        saving: false,
    };
    $scope.CouponCode = {
        TransactionId: null,
        CouponCodeId: 0,
        CouponCode: '',
        CouponCodePrefix: '',
        NoOfCouponCode: '',
        CouponCodeList: [],
        CouponType: '',
        CouponUsagesType: '',
        NoOfUsages: '1',
        ValidityStartDate: '',
        ValidityEndDate: '',
        CouponSource: '',
        DiscountType: '',
        DiscountValue: 0.0,
        IsAppliedForSubscription: false,
        SubscriptionItems: '',
        IsCouponAppliedForAgeGroup: false,
        AgeGroupItems: '',
        IsAppliedForMultipleUserType: false,
        UserType: '',
        UserTypeItems: '',
        DomainId: '',
    };

    $scope.init = function () {
        userService.getCurrentUser().then(function (user) {
            $scope.User = user;
        });
        var usert = userService.getCurrentUser();
        $scope.GetCouponCodeList($scope.currentPage, $scope.itemsPerPage, $scope.status);
    }

    $scope.GetCouponCodeList = function (Page, itemsPerPage, status) {
        $scope.SelectAll = false;
        $scope.status = status
        $scope.itemsPerPage = itemsPerPage
        if (!$scope.IsActing) {
            $scope.CouponCodeLoad = true;
        }
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
        $http.post(Comman.GetCouponCodeListUrl(), Input)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.CouponCodeList = [];
                    $scope.CouponCodeList = response.data.Result;
                    $scope.totalPages = response.data.TotalPage;
                    $scope.currentPage = Page;
                    $scope.start = 1;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.CouponCodeLoad = false;
            });
    }

    $scope.SelectAllCouponCode = function (Select) {
        if ($scope.CouponCodeList != '' && $scope.CouponCodeList.length > 0) {
            angular.forEach($scope.CouponCodeList, function (value, key) {
                value.Selected = Select;
            });
        }
    }

    $scope.SelectCouponCode = function (Select, CouponCodeId) {
        if ($scope.CouponCodeList != '' && $scope.CouponCodeList.length > 0) {
            angular.forEach($scope.CouponCodeList, function (value, key) {
                if (value.CouponCodeId == CouponCodeId) {
                    value.Selected = Select;
                }
            });
            var result = $scope.CouponCodeList.filter(obj => obj.Selected);
            if (result != null && result.length > 0) {
                if ($scope.CouponCodeList.length == result.length) {
                    $scope.SelectAll = true;
                } else {
                    $scope.SelectAll = false;
                }
            }

        }
    }

    $scope.DeleteCouponCode = function () {
        var result = $scope.CouponCodeList.filter(obj => obj.Selected);
        if (result != null && result != undefined && result.length > 0) {
            if (confirm("Are you sure to delete!") == true) {
                var ids = result.map(function (e) {
                    return e.CouponCodeId;
                }).join(',');
                $http.delete(Comman.CouponCodeDeleteUrl() + "?CouponCodeId=" + ids + "&UserId=" + $scope.User.id)
                    .then(function (response) {
                        if (response.status == 200 && response.data.StatusCode == 200) {
                            $scope.init();
                            notificationsService.success("Success", response.data.Message);
                        } else {
                            alert(response.data.Message);
                        }
                    }).finally(function () {
                        $scope.IsActing = false;
                    });
            }
        } else {
            notificationsService.warning("Warnig", "Please select any items");
        }
    }
    $scope.rangeCreator = function (minVal, maxVal) {
        var arr = [];
        for (var i = minVal; i <= maxVal; i++) {
            arr.push(i);
        }
        return arr;
    };

    $scope.CouponChangeStatusChange = function (Item) {
        Item.Status = !Item.Status;
        $scope.IsActing = true;
        $scope.CouponCodeList.find(v => v.CouponCodeId == Item.CouponCodeId).Status = Item.Status;
        $http.get(Comman.CouponCodeStatusChangeUrl() + "?CouponCodeId=" + Item.CouponCodeId + "&Status=" + Item.Status + "&UserId=" + $scope.User.id)
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.init();
                    notificationsService.success("Success", response.data.Message);
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.IsActing = false;
            });
    }


    $scope.ResetSearch = function () {
        $scope.search = '';
        $scope.startDate = '';
        $scope.endDate = '';
        $scope.status = '';
        $scope.GetCouponCodeList($scope.currentPage, $scope.itemsPerPage, $scope.status);
    }

    $scope.ExportToExcel = function () {
        $scope.RequestDownload = true;
        window.open(Comman.DownloadCouponCodeListUrl() + "?Search=" + $scope.search + "&StartDate=" + $scope.startDate + "&EndDate=" + $scope.endDate + "&Status=" + $scope.status);
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
                    url: Comman.PostCouponCodeSaveFileUrl() + "?UserId=" + $scope.User.id,
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
                        $scope.init();
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

    $scope.AddNewCouponCode = function () {
        $scope.IsStartDateValid = false;
        $scope.model.show = true;
        $scope.model.showing = true;
        $scope.ErrorForAgeRequired = '';
        $http.get(Comman.GetCreateCouponCodeDetailsUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.CouponCode = response.data.Result
                    if ($scope.CouponCode != null && $scope.CouponCode != undefined) {
                        if ($scope.CouponCode.CouponTypeList != null && $scope.CouponCode.CouponTypeList != undefined && $scope.CouponCode.CouponTypeList.length > 0) {
                            $scope.CouponCode.CouponTypeList[0].Selected = true;
                            $scope.CouponCode.CouponType = $scope.CouponCode.CouponTypeList[0].CouponTypeId;
                        }
                        if ($scope.CouponCode.CouponUsagesTypeList != null && $scope.CouponCode.CouponUsagesTypeList != undefined && $scope.CouponCode.CouponUsagesTypeList.length > 0) {
                            $scope.CouponCode.CouponUsagesTypeList[0].Selected = true;
                            $scope.CouponCode.CouponUsagesType = $scope.CouponCode.CouponUsagesTypeList[0].CouponUsagesTypeId;
                        }
                        if ($scope.CouponCode.DiscountTypesList != null && $scope.CouponCode.DiscountTypesList != undefined && $scope.CouponCode.DiscountTypesList.length > 0) {
                            $scope.CouponCode.DiscountTypesList[0].Selected = true;
                            $scope.CouponCode.DiscountType = $scope.CouponCode.CouponUsagesTypeList[0].CouponUsagesTypeId;
                        }
                        //if ($scope.CouponCode.UserTypeList != null && $scope.CouponCode.UserTypeList != undefined && $scope.CouponCode.UserTypeList.length > 0) {
                        //    $scope.CouponCode.UserTypeList[0].Selected = true;
                        //    $scope.CouponCode.UserType = $scope.CouponCode.UserTypeList[0].UserTypeId;
                        //}
                        $scope.CouponCode.NoOfUsages = '1';
                        $scope.CouponCode.IsAppliedForSubscription = false;
                        $scope.CouponCode.IsCouponAppliedForAgeGroup = false;
                        $scope.CouponCode.IsAppliedForMultipleUserType = false;
                        $scope.CouponCode.CouponCodeList = [];
                        $scope.CouponCode.NoOfCouponCode = null;
                    }
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.model.showing = false;
            });
    }

    $scope.CouponUsagesTypeChange = function (change) {
        if (change != null && change != undefined && change != "") {
            if (change == "1") {
                $scope.CouponCode.NoOfUsages = '1';
            }
        }
    }

    $scope.SaveCouponCode = function (CreateCouponCodeForm, CouponCode) {
        $scope.ErrorForAgeRequired = '';
        $scope.ErrorForSubscriptionRequired = '';
        $scope.ErrorForMultipleUserType = '';
        if (CreateCouponCodeForm.$invalid || $scope.EndDateErrorMessage != '') {
            return;
        } else {
            var SelectedUserType = "";
            var SelectedSubscription = "";
            var SelectedAgeGroup = "";
         
            if (!CouponCode.IsAppliedForMultipleUserType) {
                $scope.ErrorForMultipleUserType = "Please Select User Type";
                return;
            }
            if (CouponCode.IsAppliedForMultipleUserType) {
                SelectedUserType = CouponCode.UserTypeList.filter(x => x.Selected);
                if (SelectedUserType == null || SelectedUserType == undefined || SelectedUserType.length == 0) {
                    $scope.ErrorForMultipleUserType = "Please Select User Type";
                    return;
                } else {
                    SelectedUserType = SelectedUserType.map(x => x.UserTypeId).join(",");
                }
            }
            if (CouponCode.IsAppliedForSubscription) {
                SelectedSubscription = CouponCode.SubscriptionsItemForCouponCodeList.filter(x => x.Selected);
                if (SelectedSubscription == null || SelectedSubscription == undefined || SelectedSubscription.length == 0) {
                    $scope.ErrorForAgeRequired = "Please Select Atleast One Subscription";
                    return;
                } else {
                    SelectedSubscription = SelectedSubscription.map(x => x.Ranking).join(",");
                }
            }
            if (CouponCode.IsCouponAppliedForAgeGroup) {
                SelectedAgeGroup = CouponCode.AgeGroupItemForCouponCodeList.filter(x => x.Selected);
                if (SelectedAgeGroup == null || SelectedAgeGroup == undefined || SelectedAgeGroup.length == 0) {
                    $scope.ErrorForSubscriptionRequired = "Please Select Atleast One Age Group";
                    return;
                } else {
                    SelectedAgeGroup = SelectedAgeGroup.map(x => x.AgeGroupName).join(",");
                }
            }
            $scope.model.saving = true;
            //if (CouponCode.CouponCodeId == 0) {
            var Input = {
                TransactionId: CouponCode.TransactionId,
                CouponCodeId: CouponCode.CouponCodeId,
                CouponCodeName: CouponCode.CouponCodeName,
                CouponCodePrefix: CouponCode.CouponCodePrefix,
                NoOfCouponCode: CouponCode.NoOfCouponCode,
                CouponType: CouponCode.CouponType,
                CouponUsagesType: CouponCode.CouponUsagesType,
                NoOfUsages: CouponCode.NoOfUsages,
                ValidityStartDate: CouponCode.ValidityStartDate,
                ValidityEndDate: CouponCode.ValidityEndDate,
                CouponSource: CouponCode.CouponSource,
                DiscountType: CouponCode.DiscountType,
                DiscountValue: CouponCode.DiscountValue,
                IsAppliedForSubscription: CouponCode.IsAppliedForSubscription,
                SelectedSubscription: SelectedSubscription,
                IsCouponAppliedForAgeGroup: CouponCode.IsCouponAppliedForAgeGroup,
                SelectedAgeGroup: SelectedAgeGroup,
                IsAppliedForMultipleUserType: CouponCode.IsAppliedForMultipleUserType,
                UserType: CouponCode.UserType,
                SelectedUserType: SelectedUserType,
                UserId: $scope.User.id,
                TransactionId: CouponCode.TransactionId,
                IsAppliedForDomain: CouponCode.IsAppliedForDomain,
                DomainId: CouponCode.DomainId,
            };
            debugger
            $http.post(Comman.CreateEditCouponCodeUrl(), Input)
                .then(function (response) {
                    if (response.status == 200 && response.data.StatusCode == 200) {
                        notificationsService.success("Success", response.data.Message);
                        $scope.init();
                        $scope.model.show = false;
                    } else {
                        notificationsService.error("Error", response.data.Message);
                    }
                }).finally(function () {
                    $scope.model.saving = false;
                });
            //} else
            //    $scope.model.saving = false;
        }
    }

    $scope.CheckValidEndDate = function (startDate, endDate) {
        $scope.EndDateErrorMessage = '';
        var curDate = new Date();
        if (endDate != null && endDate != undefined && endDate != "") {
            if (new Date(startDate) > new Date(endDate)) {
                $scope.EndDateErrorMessage = 'End Date should be greater than Start Date';
                return false;
            }
            if (new Date(startDate) < curDate) {
                $scope.StartDateErrorMessage = 'Start date should not be before today.';
                return false;
            }
        }
    };

    $scope.EditCopuponCodeDetails = function (CouponCodeId) {
        $scope.ErrorForAgeRequired = "";
        $scope.ErrorForSubscriptionRequired = "";
        $scope.model.show = true;
        $scope.model.showing = true;
        if (CouponCodeId != null && CouponCodeId != undefined) {
            $http.get(Comman.GetCouponCodeEditDetailsUrl() + "?CouponCodeId=" + CouponCodeId)
                .then(function (response) {
                    if (response.status == 200 && response.data.StatusCode == 200) {
                        debugger
                        $scope.CouponCode = response.data.Result;
                        $scope.CouponCode.ValidityStartDate = new Date($scope.CouponCode.ValidityStartDate);
                        $scope.CouponCode.ValidityEndDate = new Date($scope.CouponCode.ValidityEndDate);
                        $scope.cstartDate = $scope.CouponCode.ValidityStartDate;
                        if (!$scope.CouponCode.IsAppliedForMultipleUserType) {
                            $scope.CouponCode.UserTypeList[0].Selected = true;
                            $scope.CouponCode.UserType = $scope.CouponCode.UserTypeList[0].UserTypeId;
                        }
                        if ($scope.CouponCode.CouponType != '') {
                            angular.forEach($scope.CouponCode.CouponTypeList, function (value, key) {
                                if (value.CouponTypeId == $scope.CouponCode.CouponType) {
                                    value.Selected = true;
                                }
                            });
                        }
                        if ($scope.CouponCode.UserType != '') {
                            angular.forEach($scope.CouponCode.UserTypeList, function (value, key) {
                                if (value.UserTypeId == $scope.CouponCode.UserType) {
                                    value.Selected = true;
                                }
                            });
                        }
                        if ($scope.CouponCode.CouponUsagesType != '') {
                            angular.forEach($scope.CouponCode.CouponUsagesTypeList, function (value, key) {
                                if (value.CouponUsagesTypeId == $scope.CouponCode.CouponUsagesType) {
                                    value.Selected = true;
                                }
                            });
                        }
                        if ($scope.CouponCode.DiscountType != '') {
                            angular.forEach($scope.CouponCode.DiscountTypesList, function (value, key) {
                                if (value.DiscountType == $scope.CouponCode.DiscountType) {
                                    value.Selected = true;
                                }
                            });
                        }
                        if ($scope.CouponCode.DomainId != '') {
                            angular.forEach($scope.CouponCode.DomainNameMasterList, function (value, key) {
                                if (value.DomainId == $scope.CouponCode.DomainId) {
                                    value.Selected = true;
                                }
                            });
                        }
                        if ($scope.CouponCode.CouponCodeId != null) {
                            var curDate = new Date();
                            if ($scope.CouponCode.ValidityStartDate != null && $scope.CouponCode.ValidityStartDate != undefined && $scope.CouponCode.ValidityStartDate != "") {
                                var startdate = new Date($scope.CouponCode.ValidityStartDate)
                                startdate.setHours(0, 0, 0, 0)
                                curDate.setHours(0, 0, 0, 0)
                                if (new Date(startdate) >= new Date(curDate)) {
                                    $scope.IsStartDateValid = false;
                                } else {
                                    $scope.IsStartDateValid = true;
                                }

                            }
                        }
                    } else {
                        alert(response.data.Message);
                    }
                }).finally(function () {
                    $scope.model.showing = false;
                });
        }
    }

    $scope.GetCopuponCodeDetails = function (CouponCodeId) {
        if (CouponCodeId != null && CouponCodeId != undefined) {
            $scope.modelDetails.show = true;
            $scope.modelDetails.showing = true;
            if (CouponCodeId != null && CouponCodeId != undefined) {
                $http.get(Comman.GetCouponCodeEditDetailsUrl() + "?CouponCodeId=" + CouponCodeId)
                    .then(function (response) {
                        if (response.status == 200 && response.data.StatusCode == 200) {
                            $scope.CouponCode = response.data.Result;
                            $scope.CouponCode.ValidityStartDate = new Date($scope.CouponCode.ValidityStartDate);
                            $scope.CouponCode.ValidityEndDate = new Date($scope.CouponCode.ValidityEndDate);
                            $scope.cstartDate = $scope.CouponCode.ValidityStartDate;
                        } else {
                            alert(response.data.Message);
                        }
                    }).finally(function () {
                        $scope.modelDetails.showing = false;
                    });
            }
        }
    }

    $scope.GenerateCouponCode = function (Prefix, NoOfCode) {
        $scope.CouponCode.CouponCodeList = [];
        if (Prefix != null && Prefix != undefined && Prefix != '' && NoOfCode != null && NoOfCode != undefined && NoOfCode != '') {
            var result = '';
            var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
            var charactersLength = characters.length;
            var NumberOfLength = 0;
            switch (Prefix.length) {
                case 1:
                    NumberOfLength = 6
                    break;
                case 2:
                    NumberOfLength = 5
                    break;
                case 3:
                    NumberOfLength = 4
                    break;
                case 4:
                    NumberOfLength = 3
                    break;
                default:
                    NumberOfLength = 3
            }
            for (var j = 0; j < NoOfCode; j++) {
                for (var i = 0; i < NumberOfLength; i++) {
                    result += Prefix + characters.charAt(Math.floor(Math.random() *
                        charactersLength));
                }
                $scope.CouponCode.CouponCodeList.push({ Id: j + 1, Name: result.toUpperCase() });
                result = '';
            }
        }
    }
});