app.directive("datepicker", function () {

	function link(scope, element, attrs, controller) {
		// CALL THE "datepicker()" METHOD USING THE "element" OBJECT.
		element.datepicker({
			onSelect: function (dt) {
				scope.$apply(function () {
					// UPDATE THE VIEW VALUE WITH THE SELECTED DATE.
					controller.$setViewValue(dt);
				});
			},
			dateFormat: "dd/mm/yy"      // SET THE FORMAT.
		});
	}

	return {
		require: 'ngModel',
		link: link
	};
});
app.directive('minDate', function () {
	return {
		require: 'ngModel',
		link: function (scope, element, attrs, ngModelController) {

			var minDate = scope.$eval(attrs.minDate) || new Date(new Date().setHours(0, 0, 0, 0));

			ngModelController.$validators.minDate = function (value) {
				return value >= minDate;
			};
		}
	};
});
app.directive('disallowSpaces', function () {
	return {
		restrict: 'A',

		link: function ($scope, $element) {
			$element.bind('input', function () {
				$(this).val($(this).val().replace(/ /g, ''));
			});
		}
	};
});
app.directive('onlyDigits', function () {

		return {
			restrict: 'A',
			require: '?ngModel',
			link: function (scope, element, attrs, modelCtrl) {
				modelCtrl.$parsers.push(function (inputValue) {
					if (inputValue == undefined) return '';
					var transformedInput = inputValue.replace(/[^0-9]/g, '');
					if (transformedInput !== inputValue) {
						modelCtrl.$setViewValue(transformedInput);
						modelCtrl.$render();
					}
					return transformedInput;
				});
			}
		};
});
app.directive('onlyChar', function () {

	return {
		restrict: 'A',
		require: '?ngModel',
		link: function (scope, element, attrs, modelCtrl) {
			modelCtrl.$parsers.push(function (inputValue) {
				if (inputValue == undefined) return '';
				var transformedInput = inputValue.replace(/[^a-zA-Z]/g, '');
				if (transformedInput !== inputValue) {
					modelCtrl.$setViewValue(transformedInput);
					modelCtrl.$render();
				}
				return transformedInput;
			});
		}
	};
});
app.service('Comman', function () {
    //Experts Talk Start
    this.GetExpertsTalkUrl = function () {
        return "/umbraco/surface/ExpertTalks/GetExpertTaskList";
    }
    this.GetExportToExcelUmbracoAuthorizedApiUrl = function () {
        return "/umbraco/backoffice/api/ExpertTalksAuthorizeApi/ExportToExcel";
    }
    //Experts Talk End


    //Import Excel File Start
    this.GetImportExcelToTableUmbracoAuthorizedApiUrl = function () {
        return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/ImportExcelToTableAsync";
    }
    this.GetImportExcelFilesUmbracoAuthorizedApiUrl = function () {
        return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/ImportExcelFiles";
	}
	this.GetImportLocalExcelFilesUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/ImportExcelFilesLocal";
	}
	this.GetImportLocalExcelFilesUplaodUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/PostSaveFile";
	}
	//this.CreateZip = function () {
	//	return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/CreateZip";
	//}
    this.GetRegisterTempListUmbracoAuthorizedApiUrl = function () {
        return "/umbraco/backoffice/api/ImportExcelFileAuthorizeApi/GetRegister_TempList";
    }
    this.GetInsertTemptoBackUpUmbracoAuthorizedApiUrl = function () {
        return "/umbraco/backoffice/api/+/InsertRegisterBackUpData";
    }
     //Import Excel File End

	this.GetWebinarsListUrl = function (query) {
		return "/umbraco/surface/WebinarManagement/GetWebinarsList?filter=" + query;
	}
	this.DeleteWebinarUrl = function () {
		return "/umbraco/surface/WebinarManagement/DeleteWebinarByID";
	}
	this.GetRegistrationUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllRegistrationList";
	}
	this.GetRegistrationExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/RegistrationExportToExcel";
	}
	this.GetSubscriptionUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllSubscriptionList";
	}
	this.GetSubscriptionExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/SubscriptionExportToExcel";
	}
	this.GetReferralDetailUrl = function (query) {
		return "/umbraco/surface/Report/GetAllReferralDetailList?filter=" + query;
	}
	this.GetReferralDetailUrlExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportAuthorizeApi/ExportToExcelReferralDetails";
	}
	this.GetReferralTransactionUrl = function (query) {
		return "/umbraco/surface/Report/GetAllReferralTransactionList?filter=" + query;
	}
	this.GetReferralTransactionExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportAuthorizeApi/ExportToExcelReferralTransaction";
	}
	//FAQ Start Url
	this.GetFaqRequestListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetFAQRequestList";
	}
	this.DownloadFaqRequestListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/FAQRequestListExportToExcel";
	}
	this.UploadFAQFilesUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/PostFAQRequestSaveFile";
	}
	this.GetRequestDetailsUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetFAQRequestList";
	}
	//FAQ End Url

	//User Login Start
	this.GetUserLoginListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/UserLoginList";
	}
	this.DownloadUserLoginListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/DownloadUserLoginListExportToExcel";
	}
	//User Login End

	// WorksSheet Download Start
	this.GetWorksSheetDownloadListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/WorksSheetDownloadList";
	}
	this.DownloadWorksSheetListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/DownloadWorksSheetListExportToExcel";
	}
	// WorksSheet Download End
	//Coupon Code Start
	this.GetCouponCodeListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeList";
	}
	this.DownloadCouponCodeListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeListExportToExcel";
	}
	this.GetCreateCouponCodeDetailsUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetCreateCouponCodeDetails";
	}
	//with form
	this.CreateEditCouponCodeUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CreateEditCouponCode";
	}
	//with excel serial
	this.CreateEditCouponCodeUrlExcelSerial = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CreateEditCouponCodeExcelSerial";
	}
	this.CreateEditCouponCodeUrlExcelFileSave = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CreateEditCouponCodeExcelCouponFileSave";
	}
	//with excel coupon
	this.CreateEditCouponCodeUrlExcelCoupon = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CreateEditCouponCodeExcelCoupon";
	}
	this.PostCouponCodeSaveFileUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/PostCouponCodeSaveFile";
	}
	this.GetCouponCodeEditDetailsUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetCouponCodeEditDetails";
	}
	this.CouponCodeStatusChangeUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeStatusChange";
	}
	this.CouponCodeDeleteUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeDelete";
	}
	//Coupon Code End
	//Coupon Code Log Start
	this.GetCouponCodeLogListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeLogList";
	}
	this.DownloadCouponCodeLogListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/CouponCodeLogListExportToExcel";
	}
	//Coupon Code Log End

	//Start Deepak Changes
	this.GetNotificationListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllNotificatoinList";
	}
	this.GetNotificationExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/NotificatoinExportToExcel";
	}
	this.GetOTPListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllOTPList";
	}
	this.GetOTPExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/OTPExportToExcel";
	}
	this.GetDownlaodDataListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetUserReportDownloadDataList";
	}
	this.GetDownlaodDataExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/DownloadDataExportToExcel";
	}
	this.GetDownlaodDataByUserExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/DownloadDataExportToExcelByUser";
	}

	this.GetReferralListUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/GetAllReferralList";
	}
	this.GetReferralExportToExcelUmbracoAuthorizedApiUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/ReferralExportToExcel";
	}
	this.InsertURLManipulationUrl = function () {
		return "/umbraco/backoffice/api/ReportsAuthorizeApi/InsertURLManipulationList";
	}
	//End Deepak Changes
});
