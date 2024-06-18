$(document).ready(function () {
	
		var token = $('input[name="__RequestVerificationToken"]').val();
		var downldpm = window.location.href;
		$('#worksheetLoading').show();

		if (downldpm != null && downldpm != '') {
			$.ajax({
				type: "POST",
				url: "/umbraco/Surface/Notification/DownloadDoc",
				data: { __RequestVerificationToken: token, "downpm": downldpm},
				success: function (e) {
					if (e.status == "done") {
						location.href = e.navigation;


						commonLayer("WhatsAppDownload", "WhatsApp Worksheet Download");

						$("#worksheetLoading").hide();
						$("#successmsg").show();
						//location.href = "/";
					}
					if (e.status == "fail") {
						$(".descTitlePlc").show();
					}
					//window.open('', '_self', '');
					//window.close();
				},
				error: function (error) {
					//$("#worksheetLoading").hide();
				}, complete: function () {
					$("#worksheetLoading").hide();
					//window.open('', '_self', '');
					//window.close();
					//$("#worksheetLoading").hide();
					//console.log($(window).data('ajaxready'));
					//$(window).data('ajaxready', true);
				}
			});
		}
		else {
			$("#worksheetLoading").hide();
			location.href = "/";
		}
	
});
