$(document).ready(function () {

	var token = $('input[name="__RequestVerificationToken"]').val();
	var downldpm = window.location.href;
	$('#worksheetLoading').show();
	
	if (downldpm != null && downldpm != '') {
		$.ajax({
			type: "POST",
			url: "/umbraco/Surface/Notification/DownloadPDF_SFMCPaid",
			data: { __RequestVerificationToken: token, "downpm": downldpm },
			success: function (e) {
				if (e.status == "done") {
					location.href = e.navigation;


					commonLayer("SFMCPaidDownload", "SFMC Paid Worksheet Download");

					$("#worksheetLoading").hide();
					$("#successmsg").show();
					//location.href = "/";
				}
				if (e.status == "fail") {
					$(".descTitlePlc").show();
				}
			},
			error: function (error) {
				//$("#worksheetLoading").hide();
			}, complete: function () {
				$("#worksheetLoading").hide();
			}
		});
	}
	else {
		$("#worksheetLoading").hide();
		location.href = "/";
	}
});
