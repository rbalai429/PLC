$(document).ready(function () {
	var resendCnt = 0;
	$("#username").keypress(function (event) {
		if (event.which == 13) {
			$("#generateOtp").click();
			return false;
		}
	});
	$("#uOtp").keypress(function (event) {
		if (event.which == 13) {
			$("#OtpVerify").click();
			return false;
		}
	});
	$("#confirmnewpassword").keypress(function (event) {
		if (event.which == 13) {
			$("#ForgotPassword").click();
			return false;
		}
	});

	$("#generateOtp").on('click', function (e) {
		$('.error').hide();
		
		var username = $('#username').val().trim();
		//alert('gg');
		if (username == "") {
			$('#usernameRequired').show();
			$('#username').focus();
			return false;
		}
		else if (validateEmail(username) == false) {
			$('#usernameFormat').show();
			$('#username').focus();
			return false;
		}
		
		//else if (whatsupnumber != "" && (whatsupnumber == "0000000000" || whatsupnumber.length != 10)) {
		//	$('#whatsupFormat').show();
		//	$('#whatsupnumber').focus();
		//	return false;
		//}
		else {
			$("#loader").css('display', 'block');
			$.ajax({
				type: "POST",
				contentType: "application/json",
				dataType: "JSON",
				url: "/umbraco/Surface/Home/ForgotPasswordSendOtp",
				data: JSON.stringify({ "email": username, "subject": "Forgot Password OTP for HP Print Learn Center", "type": "forgotpassword" }),
				success: function (e) {
					if (e.status == "Success") {
						$("#EmailId").val(username);
						$("#otpverif").show();
						$("#otpverif").html("Otp has been sent on your registered email id and Mobile no.");
						$("#step-1").hide();
						$("#step-2").show();
					}
					if (e.status == "Set") {
						$("#EmailId").val(username);
						$("#otpverif").hide();

						Swal.fire({
							//title: e.message,
							text: e.message,
							//icon: 'success',
							showCancelButton: false,
							confirmButtonColor: '#3085d6',
							cancelButtonColor: '#d33',
							confirmButtonText: 'Set Password'
						}).then((result) => {
							if (result.isConfirmed) {
								window.location = e.navigation;
							}
						});
					}
					else if (e.status == "Fail") {
						$("#otpMg").show();
						$("#otpMg").html(e.message);
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

	$("#OtpVerify").on('click', function () {
		$('.error').hide();

		var uOtp = $('#uOtp').val().trim();

		if (uOtp == "") {
			$('#otpValidate').show();
			$('#uOtp').focus();
			return false;
		}
		else {
			$("#loader").css('display', 'block');
			$.ajax({
				type: "POST",
				contentType: "application/json",
				dataType: "JSON",
				url: "/umbraco/Surface/Home/OtpVerification",
				data: JSON.stringify({ "otp": uOtp }),
				success: function (e) {
					if (e.status == "Success") {
						//window.location = "login";
						$("#step-2").hide();
						$("#step-3").show();
					}
					else if (e.status == "Fail") {
						$("#otpverif").show();
						$("#otpverif").html("Verification failed, Please try again.");
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
	});

	$("#ForgotPassword").on('click', function () {
		$('.error').hide();

		var newpassword = $('#newpassword').val().trim();
		var confirmnewpassword = $('#confirmnewpassword').val().trim();

		if (newpassword == "") {
			$('#passwordRequired').show();
			$('#newpassword').focus();
			return false;
		}

		else if (newpassword.length < 3 || newpassword.length > 15) {
			$('#passwordLengthRequired').show();
			$('#newpassword').focus();
			return false;
		}

		else if (validatePassword(newpassword) == false) {
			$('#passwordValidate').show();
			$('#newpassword').focus();
			return false;
		}

		else if (confirmnewpassword == "") {
			$('#confirmpasswordRequired').show();
			$('#confirmnewpassword').focus();

			return false;
		}
		else if (newpassword != "" && confirmnewpassword != "" && (newpassword != confirmnewpassword)) {

			$('#confirmpasswordValidate').show();
			$('#confirmnewpassword').focus();

			return false;
		}
		else {
			var culture = $('#cultureSessionName').val().trim();
			if (culture == "/")
				culture = "";
			$("#loader").css('display', 'block');
			$.ajax({
				type: "POST",
				contentType: "application/json",
				dataType: "JSON",
				url: "/umbraco/Surface/Home/ForgotPassword",
				data: JSON.stringify({ "newpassword": newpassword, "confirmnewpassword": confirmnewpassword }),
				success: function (e) {
					if (e.status == "Success") {
						//window.location = "login";
						//$("#msgBox").show();
						//$("#msgBox").html(e.message);
						//return false;

						Swal.fire({
							//title: 'Success',
							text: e.message + '. Sign-in with click on below button.',
							//icon: 'success',
							showCancelButton: false,
							confirmButtonColor: '#3085d6',
							cancelButtonColor: '#d33',
							confirmButtonText: 'Sign In'
						}).then((result) => {
							if (result.isConfirmed) {
								window.location = culture + '/my-account/login';
							}
						});
					}
					else if (e.status == "Fail") {
						$("#msgBox").show();
						$("#msgBox").html(e.message);
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
	});
});


function validatePassword(password) {
	//var filter = /^(?=.*[0-9])(?=.*[A-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{3,15}$/;
	var filter = /^[a-zA-Z0-9]{6,8}$/;
	if (!filter.test(password))
		return false;
	else
		return true;
}

$('#username').keyup(function (e) {
	$('.error').hide();
});

$('#uOtp').keyup(function (e) {
	$('.error').hide();
});

function validateEmail(email) {
	var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
	if (!filter.test(email))
		return false;
	else
		return true;
}


function ResendOTP() {
	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Home/ResendOtp",
		data: { "subject": "HP PLC Forgot Password OTP", "type": "forgotpassword" },
		success: function (e) {
			if (e.status == "Success") {
				$("#otpverif").show();
				$("#otpverif").html("Otp has been sent on your registered email id.");
				return false;
			}
		},
		error: function (error) {

		}
	});
}