$(document).ready(function () {

	const urlSearchParams = new URLSearchParams(window.location.search);
	const params = Object.fromEntries(urlSearchParams.entries());

	var id = params.id;

	var culture = $('#cultureSessionName').val();
	if (culture == "/")
		culture = "";

	if (id != undefined && id != "") {
		$.ajax({
			type: "GET",
			contentType: "application/json; charset=utf-8",
			dataType: "JSON",
			url: "/umbraco/Surface/WebinarManagement/GetWebinarDetail",
			data: { "webinarId": id },
			success: function (response) {
				if (response.StatusCode == 200 && response.Result != null) {
					//console.log(response.Result);
					$("#ddlLanguage").val(response.Result[0].Language);
					$("#ddlLanguage").change();
					if (response.Result[0].ImageFile != "") {
						$("#divImgWebinar").show();
						$("#imgWebinar").attr("src", "/WebinarImage/" + response.Result[0].ImageFile);
					}
					$("#ddlCategory").val(response.Result[0].Category);
					$("#ddlAgeGroupe").val(response.Result[0].AgeGroupe);
					$("#ddlSubscription").val(response.Result[0].SubscriptionType);
					$("#inptMeetingTitle").val(response.Result[0].MeetingTitle);
					$("#inptMeetingDetails").val(response.Result[0].MeetingUrl);
					$("#inptMeetingAgenda").val(response.Result[0].MeetingAgenda);
					$("#inptMeetingDuration").val(response.Result[0].MeetingDuration);
					$("#inptAuthorName").val(response.Result[0].AuthorName);
					$("#inptDatetime").val(response.Result[0].MeetingDate);
					$("#hdnWebinarId").val(id);
					$("#hdnImageFile").val(response.Result[0].ImageFile);
					$("#hdnAction").val("edit");
				}

			},
			error: function (error) {

			}
		});
	}

	$("#ddlLanguage").change(function () {
		var vrSelectedVal = $(this).val();
		if (vrSelectedVal != "0") {
			$.ajax({
				type: 'POST',
				url: "/umbraco/Surface/WebinarManagement/GetCategory",
				data: { 'CultureInfo': vrSelectedVal },
				dataType: "html",
				success: function (data) {
					$("#ddlCategory").html("");
					$("#ddlCategory").html(data);

					if (vrSelectedVal != "0") {
						$.ajax({
							type: 'POST',
							url: "/umbraco/Surface/WebinarManagement/GetAgeGroupe",
							data: { 'CultureInfo': vrSelectedVal },
							dataType: "html",
							success: function (data) {
								$("#ddlAgeGroupe").html("");
								$("#ddlAgeGroupe").html(data);
								if (vrSelectedVal != "0") {
									$.ajax({
										type: 'POST',
										url: "/umbraco/Surface/WebinarManagement/GetSubscriptions",
										data: { 'CultureInfo': vrSelectedVal },
										dataType: "html",
										success: function (data) {
											$("#ddlSubscription").html("");
											$("#ddlSubscription").html(data);
										}
									});
								}
								else {
									alert("Please select language");
								}
							}
						});
					}
					else {
						alert("Please select language");
					}
				}
			});
		}
		else {
			alert("Please select language");
		}

	});


	$("#aSubmitMeetingDetails").click(function () {

		var vrSavedFileName = "";

		var fileUpload = $('#fluWebinarImage').get(0);
		var files = fileUpload.files;
		if (files.length > 0) {
			var d = new Date();
			var strGetDate = d.getFullYear() + '' + d.getMonth() + d.getDate() + d.getHours() + d.getMinutes() + d.getMilliseconds();
			var filename = files[0].name;
			var fileNameExt = filename.substr(filename.lastIndexOf('.') + 1);
			filename = strGetDate + '.' + fileNameExt;
			var data = new FormData();
			for (var i = 0; i < files.length; i++) {
				data.append(filename, files[i]);
			}
			vrSavedFileName = filename;

			$.ajax({
				url: "/ImageFileUpload.ashx?upload=" + filename,
				type: "POST",
				data: data,
				contentType: false,
				processData: false,
				async: false,
				success: function (result) { },
				error: function (err) { alert(err.statusText); }

			});

			//alert(vrSavedFileName);
		}
		else {
			vrSavedFileName = $("#hdnImageFile").val();
		}

		var vrLanguage = $("#ddlLanguage").val();
		var vrCategory = $("#ddlCategory").val();
		var vrAgeGroupe = $("#ddlAgeGroupe").val();
		var vrSubscription = $("#ddlSubscription").val();
		var vrDtTime = $("#inptDatetime").val();
		var vrMeetingTitle = $("#inptMeetingTitle").val();
		var vrMeetingDetails = $("#inptMeetingDetails").val();
		var vrMeetingAgenda = $("#inptMeetingAgenda").val();
		var vrMeetingDuration = $("#inptMeetingDuration").val();
		var vrAuthorName = $("#inptAuthorName").val();
		var vrWebinarId = $("#hdnWebinarId").val();
		//alert(vrLanguage + "|" + vrCategory + "|" + vrAgeGroupe + "|" + vrSubscription + "|" + vrDtTime + "|" + vrMeetingDetails + "|" + vrMeetingTitle);


		if (vrLanguage != undefined && vrLanguage != null && vrLanguage != ""
			&& vrCategory != undefined && vrCategory != null && vrCategory != ""
			&& vrAgeGroupe != undefined && vrAgeGroupe != null && vrAgeGroupe != ""
			&& vrSubscription != undefined && vrSubscription != null && vrSubscription != ""
			&& vrDtTime != undefined && vrDtTime != null && vrDtTime != ""
			&& vrMeetingTitle != undefined && vrMeetingTitle != null && vrMeetingTitle != ""
			&& vrMeetingDetails != undefined && vrMeetingDetails != null && vrMeetingDetails != ""
			&& vrMeetingAgenda != undefined && vrMeetingAgenda != null && vrMeetingAgenda != ""
			&& vrMeetingDuration != undefined && vrMeetingDuration != null && vrMeetingDuration != ""
			&& vrAuthorName != undefined && vrAuthorName != null && vrAuthorName != ""
			&& vrWebinarId != undefined && vrWebinarId != null && vrWebinarId != "") {

			$.ajax({
				type: 'POST',
				url: "/umbraco/Surface/WebinarManagement/GenerateZoomMeeting",
				data: {
					'vLanguage': vrLanguage, 'vCategory': vrCategory, 'vSubCategory': '', 'vAgeGroupe': vrAgeGroupe, 'vSubscriptionType': vrSubscription,
					'vMeetingDate': vrDtTime, 'vMeetingTitle': vrMeetingTitle, 'vMeetingUrl': vrMeetingDetails, 'vMeetingAgenda': vrMeetingAgenda,
					'vMeetingDuration': vrMeetingDuration, 'vThumnailImage': vrSavedFileName, 'vAuthorName': vrAuthorName, 'vWebinarId': vrWebinarId,
				},
				dataType: "html",
				success: function (data) {
					//alert(data);
					if (data != "0") {
						if ($("#hdnAction").val() == "edit") {
							alert("Meeting Updated");
							window.opener.location.reload(true);
							window.open('', '_self').close();
						}
						else {
							alert("Meeting Generated");
						}
					}
					else {
						alert("Error");
					}
				}
			});


		}
		else {
			alert("Please fill all the data");
		}

	});

	$("#aGenerateMeeting").click(function () {
		var vrDtTime = $("#inptDatetime").val();
		var vrMeetingTitle = $("#inptMeetingTitle").val();
		var vrMeetingAgenda = $("#inptMeetingAgenda").val();
		if (vrDtTime != undefined && vrDtTime != null && vrDtTime != ""
			&& vrMeetingTitle != undefined && vrMeetingTitle != null && vrMeetingTitle != ""
			&& vrMeetingAgenda != undefined && vrMeetingAgenda != null && vrMeetingAgenda != "") {
			$.ajax({
				type: 'GET',
				url: "/umbraco/Surface/ZoomJwt/GetZoomAPI",
				data: { 'vMeetingTitle': vrMeetingTitle, 'vMeetingDateTime': vrDtTime, 'vMeetingAgenda': vrMeetingAgenda },

				success: function (data) {
					$("#inptMeetingDetails").val(data.status);
				},
				error: function (jqXHR, exception) {
					var msg = '';
					if (jqXHR.status === 0) {
						msg = 'Not connect.\n Verify Network.';
					} else if (jqXHR.status == 404) {
						msg = 'Requested page not found. [404]';
					} else if (jqXHR.status == 500) {
						msg = 'Internal Server Error [500].';
					} else if (exception === 'parsererror') {
						msg = 'Requested JSON parse failed.';
					} else if (exception === 'timeout') {
						msg = 'Time out error.';
					} else if (exception === 'abort') {
						msg = 'Ajax request aborted.';
					} else {
						msg = 'Uncaught Error.\n' + jqXHR.responseText;
					}
					alert(msg);
				}
			});
		}
		else {
			alert("Datetime ,title ,agenda is mandatory for generate meeting id");
		}
	});
});