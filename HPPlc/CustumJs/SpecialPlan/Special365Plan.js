
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	DisplayCount: 0
};
$.post("/umbraco/Surface/SpecialPlan/GetWorkSheetOfSpecialPlans",
	{
		Input
	},
	function (data, status) {
		$("#SpecialWorksheetList").html("").append(data);

		slicjFunction();

		$('#worksheetDetailsLoading').hide();
	});


$("#SpecialPlan").on('click', function () {

	try {
		commonLayer('learn-365', "3-6 Years 365 days Buy Now");

	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlayLogin").show();
});

$("#PreSignInBtn").on('click', function () {

	//var pageId = $('#pageId').val();

	try {
		commonLayer('learn-365', "Next Button");
	}
	catch (ex) { //console.log(ex);
	}

	$('.error').hide();
	var PreLoginEmailId = $('#PreLoginEmail').val();
	var maskingEmailCmsText = $('#maskingEmail').val();
	var maskingMobileCmsText = $('#maskingMobile').val();
	$('#PreLoginEmail').focus();

	var token = $('input[name="__RequestVerificationToken"]').val();

	if (PreLoginEmailId == "" || (validateEmail(PreLoginEmailId) === false && validateMobile(PreLoginEmailId) === false)) {
		$('#UserNameRequired').show();
		$('#PreLoginEmail').focus();
	}
	else {
		$("#loader").css('display', 'block');
		$.ajax({
			//async: false,
			type: "POST",
			url: "/umbraco/Surface/Home/PreLoginCheck",
			data: { __RequestVerificationToken: token, "UserName": PreLoginEmailId, "Source": "Plan365l" },
			//contentType: "application/json; charset=utf-8",
			//dataType: "JSON",
			success: function (e) {
				//debugger
				if (e.Status == "Success") {

					$('#otpVerifiedMode').val(e.UserName);
					$('#userIsExistingOrNew').val(e.LoginType);

					if (e.UserName == 'mobile') {
						$('#modeToEnter').html('Enter Email Id');
					}
					else if (e.UserName == 'email') {
						$('#modeToEnter').html('Enter WhatsApp no.');
					}

					$('#SP365Login').hide();
					$('#SP365Otp').show();
					$('#digit-1').focus();

					ClearOtp();

					try {
						//commonLayer('Login', 'Generate OTP Registration');
						commonLayer('learn-365', 'Generate OTP');
					}
					catch (ex) {
						//console.log(ex);
					}


					try {
						if (e.mobmasking != '' && e.mobmasking != "" && e.mobmasking != null) {
							$('#MaskingMessage').html(maskingMobileCmsText.toString().replace('{mobile}', e.mobmasking));
						}
						else if (e.mailmasking != "" && e.mailmasking != null) {
							$('#MaskingMessage').html(maskingEmailCmsText.toString().replace('{email}', e.mailmasking));
						}
					}
					catch {
						//console.log();
					}

					countdown();
				}
				else if (e.status === "EmailExt") {
					$("#MsgOtp").show();
					var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();
					$("#MsgOtp").html(emailAlreadyRegistered);

					return false;
				}
				else if (e.Status === "Exceed") {
					$("#MsgOtp").show();
					var oTPAttemptedMaximumTimes = $('#oTPAttemptedMaximumTimes').val();
					$("#MsgOtp").html(oTPAttemptedMaximumTimes);

					return false;
				}
				else if (e.Status === "MAXFATT") {

					$("#MsgOtp").show();
					var msg = $('#resendOtpTimerTitle').val();
					$("#MsgOtp").html(msg);
					countdownMaxFailedRoot("5:0", msg);

					return false;
				}
				else if (e.Status === "MAXRATT") {//Resend Case

					var msgg = $('#resendOtpTimerTitle').val();
					countdownMaxFailedRoot("5:0", msgg);

					return false;
				}
				else if (e.Status === "MobileExt") {
					$("#MsgOtp").show();
					var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();

					$("#MsgOtp").html(mobileAlreadyRegistered);
					return false;
				}
			},
			error: function (error) {
				//alert('error');
				$("#loader").css('display', 'none');
			}, complete: function () {
				//alert('complete');
				$("#loader").css('display', 'none');
			}
		});
	}
});

$(".otpspecialplanfield").keyup(function () {

	$('.error').hide();

	if (this.value.length == this.maxLength) {
		$(this).next('.otpspecialplanfield').focus();
	}
	else {
		$(this).prev('.otpspecialplanfield').focus();
	}

	var otp1 = $("#digit-1").val();
	var otp2 = $("#digit-2").val();
	var otp3 = $("#digit-3").val();
	var otp4 = $("#digit-4").val();

	var Otp = otp1.concat(otp2, otp3, otp4);

	if ((Otp != null || Otp != "") && Otp.length > 0) {
		$('#clear').show();
	}
	else {
		$('#clear').hide();
	}

	if (!Otp) {
		$('#MsgOtp').show();
		var enterOTPValidation = $('#enterOTPValidation').val();

		$('#MsgOtp').html(enterOTPValidation);
		return false;
	}
	else {
		if ((Otp != null || Otp != "") && Otp.length == 4) {
			$("#loader").css('display', 'block');

			var token = $('input[name="__RequestVerificationToken"]').val();


			var Input = {
				OneTimePwd: Otp,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/OtpVerification",
				data: Input,
				success: function (e) {
					//debugger
					if (e.Status == "SUCCESS") {
						try {
							commonLayer('learn-365', 'OTP Successfull');

							//facebook pixel
							SpecialPlanFacebookPicelCode_Login();
						} catch (ex) {
							//console.log(ex);
						}

						var userIsExistingOrNew = $('#userIsExistingOrNew').val();
						
						if (userIsExistingOrNew != null && userIsExistingOrNew == 'new') {
							$("#sp365Consent").show();
							$('#SP365Login').hide();
							$('#SP365Otp').hide();

							//var otpVerifiedMode = $('#otpVerifiedMode').val();

							//if (otpVerifiedMode == 'mobile') {
							//	//validate for email
							//	$('#useremailormobile').maxLength = 200;
							//}
							//else if (otpVerifiedMode == 'email') {
							//	//validate for mobile
							//	//$('#useremailormobile').maxLength = 10;
							//	$('#useremailormobile').attr("maxLength",10);
							//}
						}
						else {
							
							if (e.UserId == 0) {
								paynowSpecialPlan();
							}
							else {
								$(".agegroupfixedrow").hide();
								$(".overlayLogin").hide();

								//Swal.fire('Hi, your 365 plan is already activated.');
								location.href = "/learn-365/explore-worksheets";
							}
						}
					}
					else if (e.Status == "OTP_NM") {
						//Swal.fire('Please enter valid Otp!');
						$("#error").show();
						var wrongOTPValidation = $('#wrongOTPValidation').val();
						$('#error').html(wrongOTPValidation);

						commonLayer(e.Page, 'Invalid OTP');
					}
					else if (e.Status == "Exceed") {

						$("#error").show();
						var oTPAttemptMaximumLimit = $('#oTPAttemptMaximumLimit').val();

						$('#error').html(oTPAttemptMaximumLimit);
					}
					else if (e.Status === "MAXFATT") {
						//maximum validate attempt
						var msg = $('#validateAttemptTitle').val();
						$("#countdown").show();
						$("#timerMsg").html(msg);
						countdownMaxFailed();

						return false;
					}
					else if (e.Status === "MAXRATT") {//Resend Case
						var msgg = $('#ResendBlockingMessage').val();
						$("#countdown").show();
						$("#timerMsg").html(msgg);
						countdownMaxFailed();
						return false;
					}
					else if (e.Status === "Fail") {
						//$("#error").show();

						//$('#error').html("Otp not matched!");
					}
				},
				error: function (error) {
					$("#loader").css('display', 'none');
				}, complete: function () {
					$("#loader").css('display', 'none');
				}
			});
		}
	}
});

$("#VerifyandPayment").on('click', function (e) {

	try {
		//var url = window.location.href;
		SpecialPlanFacebookPicelCode_Registration();
	}
	catch (ex) {
		//console.log(ex);
	}
	var userEmailormobile = $('#useremailormobile').val();
	var name = $('#name').val();
	var otpVerifiedMode = $('#otpVerifiedMode').val();


	if (userEmailormobile != '' && userEmailormobile != "" && userEmailormobile != null) {

		if (otpVerifiedMode == 'mobile' && validateEmail(userEmailormobile) == false) {
			//validate email
			/*userEmailormobile = name + "@gmail.com";*/
			$('#emailFormat').show();
			$('#mobilenoFormat').show();
			$('#useremailormobile').focus();

			return false;
		}
		else if (otpVerifiedMode == 'email' && validateMobile(userEmailormobile) == false) {
			//validate mobile
			//var number = number;
			//userEmailormobile = "@gmail.com";
			$('#mobilenoFormat').show();
			$('#mobilenoFormat').show();
			$('#useremailormobile').focus();

			return false;
		}
		if (name == '' || name == "" || name == null) {

			$('#nameRequired').show();
			$('#name').focus();

			return false;
		}
		else {

			//if (otpVerifiedMode == 'mobile') {
			//	//validate email

			//	userEmailormobile = name.replace(' ','') + "23@gmail.com";
			//}
			//else if (otpVerifiedMode == 'email') {
			//	//validate mobile

			//	var number = Math.floor(Math.random() * 899999999 + 100000000)
			//	userEmailormobile = number;
			//}

			$("#loader").css('display', 'block');

			var firstSource = $('#PreLoginEmail').val();

			var supportOnWhatsupFromHP_v = $('input[name=supportOnWhatsupFromHP]:radio:checked').val();
			var supportOnPhoneFromHP_v = $('input[name=supportOnPhoneFromHP]:radio:checked').val();
			var supportOnEmailFromHP_v = $('input[name=supportOnEmailFromHP]:radio:checked').val();

			//var TC = $('input[name=termsChecked]:checkbox:checked').length;
			/*mobileno: userEmailormobile,*/
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				email: firstSource,
				name: name,
				mobileno: userEmailormobile,
				supportOnEmailFromHP: supportOnEmailFromHP_v,
				supportOnWhatsupFromHP: supportOnWhatsupFromHP_v,
				supportOnPhoneFromHP: supportOnPhoneFromHP_v,
				termsChecked: "1",
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/SpecialPlanRegistration",
				data: Input,
				success: function (e) {
					
					if (e.status == "Success") {

						try {

							commonLayer('learn-365', 'Registration');

							RegistrationLayer(e.UserUniqueCode, "");

						} catch (ex) {
							//console.log(ex);
						}

						paynowSpecialPlan();
					}
					else if (e.status === "EmailExt") {
						$('#fieldRequired').show();

						var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();
						$("#fieldRequired").html(emailAlreadyRegistered);
						$('#useremailormobile').focus();

						return false;
					}
					else if (e.status === "MobileExt") {
						$("#fieldRequired").show();

						var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();
						$("#fieldRequired").html(mobileAlreadyRegistered);
						$('#useremailormobile').focus();

						return false;
					}
					else if (e.status === "Regist") {
						$("#message").show();
						$("#message").html(e.message);

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
	}
	else {
		if (otpVerifiedMode == 'mobile') {
			//validate email
			$('#fieldRequired').show();
			$('#fieldRequired').html("Please enter Email Id");
		}
		else if (otpVerifiedMode == 'email') {
			//validate mobile
			$('#fieldRequired').show();
			$('#fieldRequired').html("Please enter WhatsApp no.");
		}
	}
});

$(".loginClose").click('click', function () {
	try {
		commonLayer('learn-365', "Close on 365 days Plan");
	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlayLogin").hide();
});


$('.errorclean').keypress(function (e) {
	$(".error").hide();
});


function RegistrationResendOtp() {
	$('.error').hide();
	try {
		commonLayer('learn-365', 'ReGenerate OTP');
	}
	catch (ex) {
		//console.log(ex);
	}

	$("#digit-1").val('');
	$("#digit-2").val('');
	$("#digit-3").val('');
	$("#digit-4").val('');

	$("#loader").css('display', 'block');

	var userName = $('#PreLoginEmail').val();

	var token = $('input[name="__RequestVerificationToken"]').val();

	var Input = {
		UserName: userName,
		type: 'resend',
		__RequestVerificationToken: token
	};

	$.ajax({
		type: "POST",
		//contentType: "application/json",
		//dataType: "JSON",
		url: "/umbraco/Surface/Home/RegistrationOtp",
		data: Input,
		success: function (e) {

			if (e.status == "Success") {

				$("#MsgOtp").show();
				var resendOTPSent = $('#resendOTPSent').val();
				$("#MsgOtp").html(resendOTPSent);

				try {
					commonLayer('learn-365', 'ReSend OTP Sent');

				} catch (ex) {
					//console.log(ex);
				}

				//Timer
				countdown();
			}
			else if (e.status === "Exceed") {
				$("#MsgOtp").show();
				var oTPAttemptedMaximumTimes = $('#oTPAttemptedMaximumTimes').val();

				$("#MsgOtp").html(oTPAttemptedMaximumTimes);
				return false;
			}
			//else if (e.status === "REGIST") {
			//	$("#Msg").show();

			//	$("#Msg").html("The EmailId already registered, Please login to continue.");
			//	return false;
			//}
			//else if (e.status === "EmailExt") {
			//	$("#MsgOtp").show();
			//	var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();

			//	$("#MsgOtp").html(emailAlreadyRegistered);
			//	return false;
			//}
			//else if (e.status === "MobileExt") {
			//	$("#MsgOtp").show();
			//	var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();

			//	$("#MsgOtp").html(mobileAlreadyRegistered);
			//	return false;
			//}
			//else if (e.status === "MAXFATT") {
			//	if (e.message != null || e.message != "") {//maximum validate attempt
			//		$("#countdown").show();
			//		var msg = $('#validateAttemptTitle').val();
			//		$("#timerMsg").html(msg);
			//		countdownMaxFailed();
			//	}
			//	return false;
			//}
			else if (e.status === "MAXRATT") {//Resend Case
				var msgg = $('#ResendBlockingMessage').val();
				console.log(msgg);
				$("#countdown").show();
				$("#timerMsg").html(msgg);
				countdownMaxFailed();

				return false;
			}
			else if (e.status === "Fail") {
				$("#MsgOtp").show();
				if (e.navigation == "referral") {
					commonLayer('Registration', 'Invalid Referral')
				}
				$("#MsgOtp").html("Sorry, Otp fail to send");
				return false;
			}
			else if (e.status === "Regist") {
				$("#MsgOtp").show();

				$("#MsgOtp").html(e.message);
				return false;
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


function paynowSpecialPlan() {

	try {
		commonLayer('learn-365', 'Pay Now - 365 Days Special Plan');
	}
	catch (ex) {
		//console.log('');
	}
	
	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Subscription/SpecialPlanPayNow",
		//data: { "CouponCode": couponCode },
		success: function (e) {
			console.log(e);
			if (e.status == "Success") {
				//alert(e.message);
				
				window.location = e.message;
				//$('#paymentForm').attr('action', e.message);
				//document.getElementById('paymentForm').submit();
			}
			//else if (e.status == "lgn") {
			//	window.location = e.navigation;
			//}
			else if (e.status == "Fail") {
				console.log(' fail PLC');
				$("#msg_pay").show();
				$("#msg_pay").html(e.message);
				return false;
			}
		},
		error: function (error) {
			console.log('PLC');
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
}