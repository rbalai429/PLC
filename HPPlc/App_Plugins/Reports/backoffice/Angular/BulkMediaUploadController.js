app.controller('BulkMediaUploadController', function ($scope, $http, $timeout, Comman, $location, notificationsService) {

    $scope.FilesPath = [];

    $scope.ExportToExcel = function () {
        $scope.RequestDownload = true;
        window.open(Comman.DownloadBulkMediaUploadSheetListUrl() + "?filesPath=" + $scope.FilesPath.join(','));
        $scope.RequestDownload = false;
    }

    $scope.FileUpload = function () {

        $('.bulk-media-upload-container #dvLession').hide();
        $('.bulk-media-upload-container #tbodyLession').html('');
        $('.bulk-media-upload-container #dvFinalButton').hide();

        $scope.FileUploaded = true;
        var fileUpload = $("#fileupload").get(0);
        var files = fileUpload.files;
        $scope.FilesPath = [];

        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append(files[i].name, files[i]);
        }

        $http({
            url: "/umbraco/backoffice/api/ReportsAuthorizeApi/BulkMediaUpload",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: function () {
                return formData;
            },
            data: { files: formData }
        }).then(function (response) {

            if (response.data.StatusCode === 200) {
                notificationsService.success("Buld media upload", "Media uploaded successfully!");

                $('.bulk-media-upload-container #dvLession').show();
                $('.bulk-media-upload-container #dvFinalButton').show();

                if (response.data.Result.length > 0) {
                    var $tr = '';
                    $.each(response.data.Result, function (key, value) {
                        $scope.FilesPath.push(value);
                        $tr += `<tr>
                                <td>${value}</td>
                            </tr>`;
                    });

                    $('.bulk-media-upload-container #tbodyLession').html($tr);
                }
            }
            else {
                notificationsService.error("Buld media upload", response.data.Message);
            }
        }).finally(function () {
            $("#fileupload").val("");
            $scope.FileUploaded = false;
        });;
    }
});