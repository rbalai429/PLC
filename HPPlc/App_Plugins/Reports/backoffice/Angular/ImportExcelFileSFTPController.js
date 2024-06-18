app.controller('ImportExcelFileSFTPController', function ($scope, $http, $timeout, Comman, $location, notificationsService) {
    $scope.ImportDataList = [];
    $scope.RegisterTempList = [];
    $scope.ReportDownload = false;
    $scope.Title = "Import Excel File";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.CreateZipStart = false;
    $scope.ImageValid = false;
    $scope.init = function () {
        $scope.GetRegister_TempList();
        $scope.startJob = window.sessionStorage.getItem("startJob");
        if ($scope.startJob != null && $scope.startJob != undefined && $scope.startJob != '' && $scope.startJob != "false") {
            $scope.startJob = true;
        } else {
            $scope.startJob = false;
        }
    }
    $scope.GetRegister_TempList = function () {
        $scope.RegisterTempLoad = true;
        $http.get(Comman.GetRegisterTempListUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.RegisterTempList = [];
                    $scope.RegisterTempList = response.data.Result;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.RegisterTempLoad = false;
            });
    }
    $scope.StartImportingExcelFileFromSftp = function () {
        $scope.startJob = window.sessionStorage.getItem("startJob");
        if ($scope.startJob == null || $scope.startJob == undefined || $scope.startJob == '' || $scope.startJob == "false") {
            $scope.GetImportExcelFilesList();
        } else {
            $scope.startJob = false;
        }
    }
    $scope.GetImportExcelFilesList = function () {
        window.sessionStorage["startJob"] = true;
        $scope.startJob = window.sessionStorage.getItem("startJob");
        $http.get(Comman.GetImportExcelFilesUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    notificationsService.success("Success", "Import Excel File Success!");
                    $scope.Title = "Import Excel File Success";
                } else {
                    notificationsService.error("Fail", response.data.Message);
                }
            }).finally(function () {
                window.sessionStorage["startJob"] = false;
                $scope.startJob = false;
            });
    }


    $scope.StartImportingExcelFileFromLocal = function () {
        $scope.startJob = window.sessionStorage.getItem("startJob");
        alert($scope);
        if ($scope.startJob == null || $scope.startJob == undefined || $scope.startJob == '' || $scope.startJob == "false") {
            $scope.GetImportLocalExcelFilesList();
        } else {
            $scope.startJob = false;
        }
    }
    $scope.GetImportLocalExcelFilesList = function () {
        window.sessionStorage["startJob"] = true;
        $scope.startJob = window.sessionStorage.getItem("startJob");
        $http.get(Comman.GetImportLocalExcelFilesUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    notificationsService.success("Success", "Import Excel File Success!");
                    $scope.Title = "Import Excel File Success";
                } else {
                    notificationsService.error("Fail", response.data.Message);
                }
            }).finally(function () {
                window.sessionStorage["startJob"] = false;
                $scope.startJob = false;
            });
    }
    $scope.ImportExcelToTable = function () {
        $scope.ImportExcelToTableLoad = true;
        $http.get(Comman.GetImportExcelToTableUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.ImportDataList = [];
                    $scope.ImportDataList = response.data.Result;
                } else {
                    alert(response.data.Message);
                }
            }).finally(function () {
                $scope.ImportExcelToTableLoad = false;
            });
    }
    $scope.ValidUrl = function (Url) {
        if (Url != null && Url != undefined && Url != "") {
            return true
        } else {
            return false;
        }
    }
    $scope.InsertTempToBakcUp = function () {
        $scope.TempToBackUpLoad = true;
        $http.get(Comman.GetInsertTemptoBackUpUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    notificationsService.success("Success", "Temp Data Move to Back Up Table");
                } else {
                    notificationsService.error("Fail", response.data.Message);
                }
            }).finally(function () {
                $scope.TempToBackUpLoad = false;
            });
    }
   
});