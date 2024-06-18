$(document).ready(function () {
	var resendCnt = 0;
	$("#username").keypress(function (event) {
		if (event.which == 13) {
			$("#generateOtp").click();
			return false;
		}
	});
	//$("#uOtp").keypress(function (event) {
	//	if (event.which == 13) {
	//		$("#OtpVerify").click();
	//		return false;
	//	}
	//});
	//$("#confirmnewpassword").keypress(function (event) {
	//	if (event.which == 13) {
	//		$("#ForgotPassword").click();
	//		return false;
	//	}
	//});

	$("#generateOtp").on('click', function (e) {
		$('.error').hide();

		var username = $('#username').val();
		//alert('gg');
		if (!username) {
			$('#usernameRequired').show();
			$('#username').focus();
			return false;
		}
		//else if (validateEmail(username) == false) {
		//	$('#usernameFormat').show();
		//	$('#username').focus();
		//	return false;
		//}
		
		//else if (whatsupnumber != "" && (whatsupnumber == "0000000000" || whatsupnumber.length != 10)) {
		//	$('#whatsupFormat').show();
		//	$('#whatsupnumber').focus();
		//	return false;
		//}
		else {
			$("#loader").css('display', 'block');
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				UserName: username,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/ForgotPasswordSendOtp",
				data: Input,
				success: function (e) {
					if (e.status === "Success") {
						try {
							commonLayer('ForgotPassword', 'Generated OTP')
						}
						catch (ex) {
							//console.log(ex);
						}

						location.href = e.navigation;
					}
					else if (e.status == "Exceed") {

						$("#MsgOtp").show();
						var oTPAttemptMaximumLimit = $('#oTPAttemptMaximumLimit').val();

						$('#MsgOtp').html(oTPAttemptMaximumLimit);
					}
					else if (e.status == "Fail") {
						$("#otpMg").show();
						$("#otpMg").html("Your maximum attempts have been exhausted, please try again after 3 minutes.");

						commonLayer('ForgotPassword', 'Invalid OTP');
						return false;
					}
				},
				error: function (error) {
					$("#loader").css('display', 'none');
				}, complete: function () {
					$("#loader").css('display', 'none');
				}
			});
		}

		return false;
	});

});



$('#username').keyup(function (e) {
	$('.error').hide();
});
