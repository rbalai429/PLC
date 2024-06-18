app.controller('ImportExcelFileLocalController', function ($scope, $http, $timeout, Comman, $location, notificationsService) {
    $scope.RegisterTempList = [];
    $scope.ReportDownload = false;
    $scope.Title = "Import Excel File";
    $scope.files;
    $scope.FileUploaded = false;
    $scope.CreateZipStart = false;
    $scope.ImageValid = false;
    $scope.init = function () {
        $scope.GetRegister_TempList();
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
                    url: Comman.GetImportLocalExcelFilesUplaodUmbracoAuthorizedApiUrl(),
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
                        $scope.RegisterTempList = [];
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
    $scope.ImportExcelToTable = function () {
        $scope.ImportExcelToTableLoad = true;
        $http.get(Comman.GetImportExcelToTableUmbracoAuthorizedApiUrl())
            .then(function (response) {
                if (response.status == 200 && response.data.StatusCode == 200) {
                    $scope.GetRegister_TempList();
                } else {
                    notificationsService.warning("Warning", response.data.Message);
                }
            }).finally(function () {
                $scope.ImportExcelToTableLoad = false;
            });
    }
    //$scope.CreateZip = function () {
    //    $scope.ReportDownload = true;
    //    window.open(Comman.CreateZip(), '_blank');
    //    $scope.ReportDownload = false;
    //    //$scope.CreateZipStart = true;
    //    //$http.get(Comman.CreateZip())
    //    //    .then(function (response) {
    //    //        if (response.status == 200 && response.data.StatusCode == 200) {
    //    //            notificationsService.success("Success", "Temp Data Move to Back Up Table");
    //    //        } else {
    //    //            notificationsService.error("Fail", response.data.Message);
    //    //        }
    //    //    }).finally(function () {
    //    //        $scope.CreateZipStart = false;
    //    //    });
    //}
});