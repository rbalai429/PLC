$(document).ready(function () {
	$("input:text,form").attr("autocomplete", "off");
	$("input:password,form").attr("autocomplete", "off");
	$(".enterEvent").keypress(function (event) {
		if (event.which == 13) {
			$("#Registration").click();
			return false;
		}
	});

	$("#CancelRegistration").on('click', function () {
		try {
			commonLayer('Registration', 'Cancel Button')
		}
		catch (ex) {
			//console.log(ex);
		}
		window.location = window.location.pathname;
	});

	$("#Registration").on('click', function () {

		//Get OTP Click
		try {
			//var url = window.location.href;
			commonLayer('Registration', 'Submit Button');
			RegistrationPage_GetOtp_gtag_report_conversion();
		}
		catch (ex) {
			//console.log(ex);
		}

		$('.error').hide();
		var name = $('#name').val();
		var email = $('#email').val();
		var mobileno = $('#mobileno').val();
		var agegroup = $('#ageGroup').val();
		//var password = $('#yourpwdhere').val();
		//var cnfmpassword = $('#yourconfirmpwdhere').val();
		var ReferralCode = $('#ReferralCode').val();

		//var supportOnWhatsupFromHP = $('input[name=supportOnWhatsupFromHP]:checkbox:checked').length;
		//var supportOnPhoneFromHP = $('input[name=supportOnMobileFromHP]:radio:checked').length;
		//var supportOnEmailFromHP = $('input[name=supportOnEmailFromHP]:checkbox:checked').length;
		debugger
		var supportOnWhatsupFromHP_v = $('input[name=supportOnWhatsupFromHP]:checkbox:checked').val();
		var supportOnPhoneFromHP_v = $('input[name=supportOnPhoneFromHP]:checkbox:checked').val();
		var supportOnEmailFromHP_v = $('input[name=supportOnEmailFromHP]:checkbox:checked').val();
		var RuParentOrStudent = $("#RuParentOrStudent").val();

		var TC = "0";//$('input[name=termsChecked]:checkbox:checked').length;

		var cultureSessionName = $('#cultureSessionName').val().trim();
		if (cultureSessionName == "/")
			cultureSessionName = "";

		//if (!name) {
		//	$('#nameRequired').show();
		//	$('#name').focus();
		//	return false;
		//}
		if (!email) {
			$('#emailRequired').show();
			$('#email').focus();
			return false;
		}
		else if (validateEmail(email) == false) {
			$('#emailRequired').hide();
			$('#emailFormat').show();
			$('#email').focus();
			return false;
		}
		else if (mobileno && validateMobile(mobileno) == false) {
			$('#mobilenoFormat').show();
			$('#mobileno').focus();
			return false;
		}
		else if (agegroup == "0" || agegroup == null || agegroup == "") {
			$('#agegroupRequired').show();
			$('#ageGroup').focus();
			return false;
		}
		//else if (!password) {
		//	$('#yourpwdhereReq').show();
		//	$('#yourpwdhere').focus();
		//	return false;
		//}
		//else if (validatePassword(password) == false) {
		//	$('#yourpwdhereReqFormat').show();
		//	$('#yourpwdhere').focus();
		//	return false;
		//}
		//else if (password !== cnfmpassword) {
		//	$('#yourconfirmpwdhereReq').show();
		//	$('#yourconfirmpwdhere').focus();
		//	return false;
		//}
		else if ((mobileno == "" || mobileno == null) && supportOnWhatsupFromHP_v == "Yes") {
			$('#ConsentWhatsAppMessage').show();
			$('#ConsentWhatsAppMessage').focus();
			return false;
		}
		else if ((email == "" || email == null) && supportOnEmailFromHP_v == "Yes") {
			$('#ConsentEmailMessage').show();
			$('#ConsentEmailMessage').focus();
			return false;
		}
		//else if (mobileno && supportOnWhatsupFromHP == "No") {
		//	$('#learningMaterialFromHP').show();
		//	$('#learningMaterialFromHP').focus();
		//	return false;
		//}
		//else if (supportOnPhoneFromHP == "0") {
		//	$('#supportOnPhoneFromHP').show();
		//	$('#supportOnPhoneFromHP').focus();
		//	return false;
		//}
		//else if (supportOnEmailFromHP == "0") {
		//	$('#supportOnEmailFromHP').show();
		//	$('#supportOnEmailFromHP').focus();
		//	return false;
		//}
		//else if (TC == "0") {
		//	$('#termsChecked').show();
		//	$('#termsChecked').focus();
		//	return false;
		//}
		else {

			$("#loader").css('display', 'block');
			$('#Registration').attr("disabled", true);

			var hdRegistrationMsg = $('#hdHpSuccessMsg').val();
			var hdRegistrationMsgButton = $('#hdRegistrationMsgButton').val();
			var bundlingRedirectUrl = $('#bundlingRedirectUrl').val();
			var Culture = $('#Culture').val();
			//var maskingEmailCmsText = $('#maskingEmail').val();
			//var maskingEmailMobileCmsText = $('#maskingEmailMobile').val();
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				name: name,
				email: email,
				mobileno: mobileno,
				referralCode: ReferralCode,
				supportOnEmailFromHP: supportOnEmailFromHP_v,
				supportOnWhatsupFromHP: supportOnWhatsupFromHP_v,
				supportOnPhoneFromHP: supportOnPhoneFromHP_v,
				ageGroup: agegroup,
				termsChecked: TC,
				RuParentOrStudent: RuParentOrStudent,
				type: 'send',
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/Registration",
				data: Input,
				success: function (e) {
					
					if (e.status == "Success") {
						try {

							referCode = $("#ReferralCode").val();
							//commonLayer('Registration', 'OTP Successfull');
							RegistrationLayer(e.UniqueUserId, referCode);
							

						} catch (ex) {
							//console.log(ex);
						}

						if (bundlingRedirectUrl != null && bundlingRedirectUrl != "" && bundlingRedirectUrl == "free") {
							location.href = "/subscription";
						}
						else if (bundlingRedirectUrl != null && bundlingRedirectUrl != "") {
							location.href = bundlingRedirectUrl;
						}
						else {
							location.href = e.navigation;
						}
					}
					else if (e.status == "vldtnmail") {
						$('#emailRequired').show();
						$('#email').focus();

						return false;
					}
					else if (e.status == "vldtnage") {
						$('#agegroupRequired').show();
						$('#ageGroup').focus();
						
						return false;
					}
					else if (e.status == "vldtnmob") {
						$('#mobilenoFormat').show();
						$('#mobileno').focus();
						return false;
					}
					else if (e.status === "Fail") {
						
						if (e.navigation == "referral") {

							$("#invalidReferral").show();

							commonLayer('Registration', 'Invalid Referral');
						}

						//$("#MsgOtp").html(e.message);
						//$('#VerifyRegistration').attr("disabled", false);
						return false;
					}
					else if (e.status === "Regist") {
						$("#Msg").show();
						$("#Msg").html(e.message);
					}
					//else if (e.status === "EmailExt") {
					//	$("#Msg").show();
					//	var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();
					//	$("#Msg").html(emailAlreadyRegistered);
					//	$('#Registration').attr("disabled", false);
					//	return false;
					//}
					//else if (e.status === "Exceed") {
					//	$("#Msg").show();
					//	var oTPAttemptedMaximumTimes = $('#oTPAttemptedMaximumTimes').val();
					//	$("#Msg").html(oTPAttemptedMaximumTimes);

					//	$('#Registration').attr("disabled", false);
					//	return false;
					//}
					//else if (e.status === "MAXFATT") {
					//	if (e.message != null || e.message != "") {//maximum validate attempt
					//		$("#Msg").show();
					//		var msg = $('#resendOtpTimerTitle').val();
					//		$("#Msg").html(msg);
					//		countdownMaxFailedRoot(e.message, msg);
					//	}
					//	return false;
					//}
					//else if (e.status === "MAXRATT") {//Resend Case
					//	if (e.message != null || e.message != "") {
					//		var msgg = $('#resendOtpTimerTitle').val();
					//		countdownMaxFailedRoot(e.message, msgg);
					//	}
					//	return false;
					//}
					//else if (e.status === "REGIST") {
					//	$("#Msg").show();
					//	$('#Registration').attr("disabled", false);

					//	$("#Msg").html("The EmailId already registered, Please login to continue.");
					//	return false;
					//}
					//else if (e.status === "MobileExt") {
					//	$("#Msg").show();
					//	var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();
					//	$('#Registration').attr("disabled", false);

					//	$("#Msg").html(mobileAlreadyRegistered);
					//	return false;
					//}
					//else if (e.status === "Fail") {
					//	if (e.navigation == "referral") {
					//		$("#invalidReferral").show();
							
					//		commonLayer('Registration', 'Invalid Referral');
					//	}

					//	return false;
					//}
					//else if (e.status === "Regist") {
					//	$("#Msg").show();
					//	$('#Registration').attr("disabled", false);

					//	$("#Msg").html(e.message);
					//	return false;
					//}
				},
				error: function (error) {
					$("#loader").css('display', 'none');
					$('#Registration').attr("disabled", false);
					//console.log(error);
				}, complete: function () {
					$("#loader").css('display', 'none');
					$('#Registration').attr("disabled", false);
					//$('#VerifyRegistration').attr("disabled", true);
				}
			});
			//var Input = $("#registrationForm").serialize();

			//Registration(Input);
		}
		//if (whatsappno == '' || whatsappno == null || whatsappno == 'undefined' || whatsappno.slice(0, 1) == '0') {
		//	$('#spwhatsappnorequired').show();
		//	$('#whatsupnumber').focus();

		//	$('#spwhatsappnolength').hide();

		//	return false;
		//}
		//if ((whatsappno !== '' || whatsappno !== null || whatsappno !== 'undefined') && whatsappno.length < 10) {
		//	$('#spwhatsappnorequired').hide();

		//	$('#spwhatsappnolength').show();
		//	$('#whatsupnumber').focus();

		//	return false;
		//}
	});

	//$("#VerifyRegistration").on('click', function () {
	//	$('.error').hide();

	//	try {
	//		commonLayer('Registration', 'Generate OTP');
	//	}
	//	catch (ex) {
	//		//console.log(ex);
	//	}

	//	var name = $('#name').val();
	//	var email = $('#email').val();
	//	var mobileno = $('#mobileno').val();
	//	var agegroup = $('#ageGroup').val();
	//	var password = $('#yourpwdhere').val();
	//	var cnfmpassword = $('#yourconfirmpwdhere').val();
	//	var ReferralCode = $('#ReferralCode').val();
	//	/*var Otp = $('#otpHere').val();*/
	//	var otp1 = $("#digit-1").val();
	//	var otp2 = $("#digit-2").val();
	//	var otp3 = $("#digit-3").val();
	//	var otp4 = $("#digit-4").val();
	//	var Otp = otp1.concat(otp2, otp3, otp4);

	//	//var supportOnWhatsupFromHP = $('input[name=supportOnWhatsupFromHP]:radio:checked').length;
	//	/*var supportOnPhoneFromHP = $('input[name=supportOnPhoneFromHP]:radio:checked').length;*/
	//	//var supportOnEmailFromHP = $('input[name=supportOnEmailFromHP]:radio:checked').length;

	//	var supportOnWhatsupFromHP_v = $('input[name=supportOnWhatsupFromHP]:radio:checked').val();
	//	var supportOnPhoneFromHP_v = $('input[name=supportOnPhoneFromHP]:radio:checked').val();
	//	var supportOnEmailFromHP_v = $('input[name=supportOnEmailFromHP]:radio:checked').val();

	//	var TC = $('input[name=termsChecked]:checkbox:checked').length;

	//	var cultureSessionName = $('#cultureSessionName').val().trim();
	//	if (cultureSessionName == "/")
	//		cultureSessionName = "";

	//	//if (!name) {
	//	//	$('#nameRequired').show();
	//	//	$('#name').focus();
	//	//	return false;
	//	//}
	//	if (!email) {
	//		$('#emailRequired').show();
	//		$('#email').focus();
	//		return false;
	//	}
	//	else if (validateEmail(email) == false) {
	//		$('#emailRequired').hide();
	//		$('#emailFormat').show();
	//		$('#email').focus();
	//		return false;
	//	}
	//	else if (mobileno && validateMobile(mobileno) == false) {
	//		$('#mobilenoFormat').show();
	//		$('#mobileno').focus();
	//		return false;
	//	}
	//	else if (agegroup == "0" || agegroup == null || agegroup == "") {
	//		$('#agegroupRequired').show();
	//		$('#ageGroup').focus();
	//		return false;
	//	}
	//	else if (!password) {
	//		$('#yourpwdhereReq').show();
	//		$('#yourpwdhere').focus();
	//		return false;
	//	}
	//	else if (validatePassword(password) == false) {
	//		$('#yourpwdhereReqFormat').show();
	//		$('#yourpwdhere').focus();
	//		return false;
	//	}
	//	else if (password !== cnfmpassword) {
	//		$('#yourconfirmpwdhereReq').show();
	//		$('#yourconfirmpwdhere').focus();
	//		return false;
	//	}
	//	else if ((mobileno == "" || mobileno == null) && supportOnWhatsupFromHP_v == "Yes") {
	//		$('#ConsentWhatsAppMessage').show();
	//		$('#ConsentWhatsAppMessage').focus();
	//		return false;
	//	}
	//	else if ((email == "" || email == null) && supportOnEmailFromHP_v == "Yes") {
	//		$('#supportOnEmailFromHP').show();
	//		$('#supportOnEmailFromHP').focus();
	//		return false;
	//	}
	//	//else if (TC == "0") {
	//	//	$('#termsChecked').show();
	//	//	$('#termsChecked').focus();
	//	//	return false;
	//	//}
	//	//else if (mobileno && supportOnWhatsupFromHP == "0") {
	//	//	$('#learningMaterialFromHP').show();
	//	//	$('#learningMaterialFromHP').focus();
	//	//	return false;
	//	//}
	//	//else if (supportOnPhoneFromHP == "0") {
	//	//	$('#supportOnPhoneFromHP').show();
	//	//	$('#supportOnPhoneFromHP').focus();
	//	//	return false;
	//	//}
	//	//else if (supportOnEmailFromHP == "0") {
	//	//	$('#supportOnEmailFromHP').show();
	//	//	$('#supportOnEmailFromHP').focus();
	//	//	return false;
	//	//}
	//	//else if (TC == "0") {
	//	//	$('#termsChecked').show();
	//	//	$('#termsChecked').focus();
	//	//	return false;
	//	//}
	//	else if (!Otp) {
	//		$('#MsgOtp').show();
	//		var enterOTPValidation = $('#enterOTPValidation').val();

	//		$('#MsgOtp').html(enterOTPValidation);
	//		return false;
	//	}
	//	else {

	//		$("#loader").css('display', 'block');
	//		$('#VerifyRegistration').attr("disabled", true);

	//		var hdRegistrationMsg = $('#hdHpSuccessMsg').val();
	//		var hdRegistrationMsgButton = $('#hdRegistrationMsgButton').val();
	//		var bundlingRedirectUrl = $('#bundlingRedirectUrl').val();
	//		var Culture = $('#Culture').val();
	//		var coupontobeApplied = $('#CoupontobeApplied').val();
	//		var token = $('input[name="__RequestVerificationToken"]').val();

	//		var Input = {
	//			name: name,
	//			email: email,
	//			mobileno: mobileno,
	//			ageGroup: agegroup,
	//			regpassword: password,
	//			regpasswordconfirm: cnfmpassword,
	//			referralCode: ReferralCode,
	//			supportOnEmailFromHP: supportOnEmailFromHP_v,
	//			supportOnWhatsupFromHP: supportOnWhatsupFromHP_v,
	//			supportOnPhoneFromHP: supportOnPhoneFromHP_v,
	//			termsChecked: TC,
	//			Otp: Otp,
	//			__RequestVerificationToken: token
	//		};

	//		$.ajax({
	//			type: "POST",
	//			//contentType: "application/json",
	//			//dataType: "JSON",
	//			url: "/umbraco/Surface/Home/Registration",
	//			data: Input,
	//			success: function (e) {
					
	//				if (e.status == "Success") {
	//					try {

	//						referCode = $("#ReferralCode").val();
	//						commonLayer('Registration', 'OTP Successfull');
	//						RegistrationLayer(e.UniqueUserId, referCode);
	//						//commonLayer('Registration', 'Submit Button');

	//					} catch (ex) {
	//						//console.log(ex);
	//					}
						
	//					if (bundlingRedirectUrl != null && bundlingRedirectUrl != "" && bundlingRedirectUrl == "free") {
	//						location.href = "/subscription";
	//					}
	//					else if (bundlingRedirectUrl != null && bundlingRedirectUrl != "") {
	//						location.href = bundlingRedirectUrl;
	//					}
	//					else if (e.message == "SPR_365Plan") {
	//						paynowSpecialPlan();
	//					}
	//					else {
	//						location.href = e.navigation;
	//					}
	//				}
	//				else if (e.status == "vldtnmail") {
	//					$('#emailRequired').show();
	//					$('#email').focus();

	//					$('#VerifyRegistration').attr("disabled", false);

	//					return false;
	//				}
	//				else if (e.status == "vldtnage") {
	//					$('#agegroupRequired').show();
	//					$('#ageGroup').focus();
	//					$('#VerifyRegistration').attr("disabled", false);
	//					return false;
	//				}
	//				else if (e.status == "vldtnmob") {
	//					$('#mobilenoFormat').show();
	//					$('#mobileno').focus();
	//					return false;
	//				}
	//				else if (e.status == "vldtnpasswd") {
	//					$('#yourpwdhereReqFormat').show();
	//					$('#yourpwdhere').focus();
	//					$('#VerifyRegistration').attr("disabled", false);
	//					return false;
	//				}
	//				else if (e.status == "OTP_NM") {
	//					//Swal.fire('Please enter valid Otp!');
	//					$("#MsgOtp").show();
	//					var wrongOTPValidation = $('#wrongOTPValidation').val();
	//					$('#VerifyRegistration').attr("disabled", false);
	//					$('#MsgOtp').html(wrongOTPValidation);

	//					commonLayer('Registration', 'Invalid OTP');
	//				}
	//				else if (e.status == "Exceed") {

	//					$("#MsgOtp").show();
	//					var oTPAttemptMaximumLimit = $('#oTPAttemptMaximumLimit').val();

	//					$('#MsgOtp').html(oTPAttemptMaximumLimit);
	//					$('#VerifyRegistration').attr("disabled", false);
	//				}
	//				else if (e.status === "MAXFATT") {
	//					//maximum validate attempt
	//					var msg = $('#validateAttemptTitle').val();
	//					$("#timerMsg").html(msg);
	//					countdownMaxFailed();

	//					return false;
	//				}
	//				else if (e.status === "MAXRATT") {//Resend Case
	//					var msgg = $('#ResendBlockingMessage').val();
	//					$("#timerMsg").html(msgg);
	//					countdownMaxFailed();
	//					return false;
	//				}
	//				else if (e.status === "Fail") {
	//					$("#MsgOtp").show();
	//					//if (e.navigation == "referral") {
	//					//	commonLayer('Registration', 'Invalid Referral')
	//					//}
	//					$("#MsgOtp").html(e.message);
	//					$('#VerifyRegistration').attr("disabled", false);
	//					return false;
	//				}
	//				else if (e.status === "Regist") {
	//					location.href = "/my-account/login";
	//				}
	//			},
	//			error: function (error) {
	//				$("#loader").css('display', 'none');
	//				$('#VerifyRegistration').attr("disabled", false);
	//				//console.log(error);
	//			}, complete: function () {
	//				$("#loader").css('display', 'none');
	//			}
	//		});
	//	}
	//});
});

//function RegistrationResendOtp() {
//	$('.error').hide();
//	try {
//		commonLayer('Registration', 'ReGenerate OTP');
//	}
//	catch (ex) {
//		//console.log(ex);
//	}

//	$("#digit-1").val('');
//	$("#digit-2").val('');
//	$("#digit-3").val('');
//	$("#digit-4").val('');

//	$("#loader").css('display', 'block');
//	var name = $('#name').val();
//	var email = $('#email').val();
//	var mobileno = $('#mobileno').val();
//	var maskingEmailCmsText = $('#maskingEmail').val();
//	var maskingEmailMobileCmsText = $('#maskingEmailMobile').val();
//	var token = $('input[name="__RequestVerificationToken"]').val();

//	var Input = {
//		name: name,
//		email: email,
//		mobileno: mobileno,
//		type: 'resend',
//		__RequestVerificationToken: token
//	};

//	$.ajax({
//		type: "POST",
//		//contentType: "application/json",
//		//dataType: "JSON",
//		url: "/umbraco/Surface/Home/RegistrationOtp",
//		data: Input,
//		success: function (e) {

//			if (e.status == "Success") {

//				$("#MsgOtp").show();
//				var resendOTPSent = $('#resendOTPSent').val();
//				$("#MsgOtp").html(resendOTPSent);

//				try {
//					if (email != "" && mobileno != "") {
//						$('#MaskingMessage').html(maskingEmailMobileCmsText.toString().replace('{email}', e.mailmasking).replace('{mobile}', e.mobmasking));
//					}
//					else {
//						$('#MaskingMessage').html(maskingEmailCmsText.toString().replace('{email}', e.mailmasking));
//					}
//				}
//				catch {
//					//console.log();
//				}

//				try {
//					commonLayer('Registration', 'ReSend OTP Sent');

//				} catch (ex) {
//					//console.log(ex);
//				}

//				//Timer
//				countdown();
//			}
//			else if (e.status === "Exceed") {
//				$("#MsgOtp").show();
//				var oTPAttemptedMaximumTimes = $('#oTPAttemptedMaximumTimes').val();

//				$("#MsgOtp").html(oTPAttemptedMaximumTimes);
//				return false;
//			}
//			else if (e.status === "REGIST") {
//				$("#Msg").show();

//				$("#Msg").html("The EmailId already registered, Please login to continue.");
//				return false;
//			}
//			else if (e.status === "EmailExt") {
//				$("#MsgOtp").show();
//				var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();

//				$("#MsgOtp").html(emailAlreadyRegistered);
//				return false;
//			}
//			else if (e.status === "MobileExt") {
//				$("#MsgOtp").show();
//				var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();

//				$("#MsgOtp").html(mobileAlreadyRegistered);
//				return false;
//			}
//			//else if (e.status === "MAXFATT") {
//			//	if (e.message != null || e.message != "") {//maximum validate attempt
//			//		$("#countdown").show();
//			//		var msg = $('#validateAttemptTitle').val();
//			//		$("#timerMsg").html(msg);
//			//		countdownMaxFailed();
//			//	}
//			//	return false;
//			//}
//			else if (e.status === "MAXRATT") {//Resend Case
//				var msgg = $('#ResendBlockingMessage').val();
//				console.log(msgg);
//				$("#countdown").show();
//				$("#timerMsg").html(msgg);
//				countdownMaxFailed();
				
//				return false;
//			}
//			else if (e.status === "Fail") {
//				$("#MsgOtp").show();
//				if (e.navigation == "referral") {
//					commonLayer('Registration', 'Invalid Referral')
//				}
//				$("#MsgOtp").html("Sorry, Otp fail to send");
//				return false;
//			}
//			else if (e.status === "Regist") {
//				$("#MsgOtp").show();

//				$("#MsgOtp").html(e.message);
//				return false;
//			}
//		},
//		error: function (error) {
//			$("#loader").css('display', 'none');
//			console.log(error);
//		}, complete: function () {
//			$("#loader").css('display', 'none');
//		}
//	});
//}

//$('#name').keypress(function (e) {
//	var regex = new RegExp("^[a-zA-Z ]+$");
//	var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
//	//var regEx = /^[a-z][a-z\s]*$/;
//	//if (this.value.match(regEx) == true) {
//	//	return true;
//	//} else {
//	//	//e.preventDefault();
//	//	return false;
//	//}
//});


$('#name').keypress(function (e) {
	var regex = new RegExp("^[a-zA-Z ]+$");
	var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
	if (regex.test(str)) {
		return true;
	} else {
		e.preventDefault();
		return false;
	}
});

$('#name').keyup(function (e) {
	$('#nameRequired').hide();
});

$('#email').keyup(function (e) {
	$('#emailRequired').hide();
	$('#emailFormat').hide();
});

$('#yourpwdhere').keyup(function (e) {
	$('#yourpwdhereReq').hide();
	$('#yourpwdhereReqFormat').hide();
});

$('#yourconfirmpwdhere').keyup(function (e) {
	$('#yourconfirmpwdhereReq').hide();
});

$('#mobileno').keyup(function (e) {
	$('#mobilenoFormat').hide();
});

$('#ReferralCode').keyup(function (e) {
	$('#invalidReferral').hide();
});
//$('#whatsupnumber').keyup(function (e) {
//	$('#whatsupFormat').hide();
//});

$("input[type='radio'][name='supportOnEmailFromHP']").click(function () {
	$("#ConsentEmailMessage").hide();
});

$("input[type='radio'][name='supportOnWhatsupFromHP']").click(function () {
	$("#ConsentWhatsAppMessage").hide();
});

//$("input[type='radio'][name='supportOnPhoneFromHP']").click(function () {
//	$("#supportOnPhoneFromHP").hide();
//});

$("input[type='checkbox'][name='termsChecked']").click(function () {
	$("#termsChecked").hide();
});


$("#ageGroup").on('change', function () {
	var IsAgeSelected = $(this).val();
	if (IsAgeSelected !== 0) {
		$("#agegroupRequired").hide();
	}
});


$('#whatsupnumber').keyup(function () {

	if (this.value.match(/[^0-9]/g) === true || this.value.length === 10) {
		$('#spwhatsappnorequired').hide();
		$('#spwhatsappnolength').hide();
	}
	else {
		$('#spwhatsappnorequired').show();
		$('#spwhatsappnolength').hide();
	}
});

