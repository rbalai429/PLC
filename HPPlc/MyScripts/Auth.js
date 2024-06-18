$(document).ready(function (e) {

	var maskingEmailCmsText = $('#maskingEmail').val();
	var maskingEmailMobileCmsText = $('#maskingEmailMobile').val();

	var mobmasking = $('#mobmasking').val();
	var mailmasking = $('#mailmasking').val();

	if (mobmasking != "" && mailmasking != "") {
		$('#MaskingMessage').html(maskingEmailMobileCmsText.toString().replace('{email}', mailmasking).replace('{mobile}', mobmasking));
	}
	else {
		$('#MaskingMessage').html(maskingEmailCmsText.toString().replace('{email}', mailmasking));
	}

/*	$("#VerifyRegistration").attr("disabled", true);*/

	//Timer
	var maskingTitle = $('maskingTitle').val();
	countdown(maskingTitle);
});

$("#VerifyRegistration").on('click', function () {
	
	$('#error').hide();
	try {
		commonLayer('OTPVerify', 'OTP Verify Button Click')
	}
	catch (ex) {
		//console.log(ex);
	}

	var otp1 = $("#digit-1").val();
	var otp2 = $("#digit-2").val();
	var otp3 = $("#digit-3").val();
	var otp4 = $("#digit-4").val();

	var _otp = otp1.concat(otp2, otp3, otp4);
	if ((otp1 != null && otp2 != null && otp3 != null && otp4 != null) && _otp.length == 4) {

		$("#loader").css('display', 'block');

		var token = $('input[name="__RequestVerificationToken"]').val();

		var Input = {
			OneTimePwd: _otp,
			__RequestVerificationToken: token
		};

		$.ajax({
			type: "POST",
			//contentType: "application/json",
			//dataType: "JSON",
			url: "/umbraco/Surface/Home/OtpVerification",
			data: Input,
			success: function (e) {
				debugger
				if (e.Status == "SUCCESS") {
					
					try {
						commonLayer('OTPVerification', 'OTP is Valid');
					}
					catch { console.log(''); }

					location.href = e.Navigation;
				}
				else if (e.Status == "MAXFATT") {

					var msg = $('#validateAttemptTitle').val();
					$("#timerMsg").html(msg);
					countdownMaxFailed();

					return false;
				}
				else if (e.Status == "MAXRATT") {//Resend Case

					var msgg = $('#resendOtpTimerTitle').val();
					$("#timerMsg").html(msgg);
					countdownMaxFailed();

					return false;
				}
				else if (e.Status == "OTP_NM") {

					//Swal.fire('Please enter valid Otp!');
					$("#MsgOtp").show();
					var InvalidOtp = $('#InvalidOtp').val();
					$("#MsgOtp").html(InvalidOtp);

					try {
						commonLayer('OTPVerification', 'Invalid OTP');
					}
					catch { console.log(''); }

				}
				else if (e.Status === "Exceed") {
					$("#MsgOtp").show();
					var oTPAttemptedMaximumTimes = $('#oTPAttemptedMaximumTimes').val();

					$("#MsgOtp").html(oTPAttemptedMaximumTimes);
					return false;
				}
				else if (e.status === "Fail") {
					//Swal.fire('Otp not sent!');
				}
			},
			error: function (error) {
				$("#loader").css('display', 'none');
				console.log(error);
			}, complete: function () {
				$("#loader").css('display', 'none');
			}
		});
	}
	else {
		$("#MsgOtp").show();
		var EnterOtpmessage = $('#EnterOtpmessage').val();
		$("#MsgOtp").html(EnterOtpmessage);

	}
});

