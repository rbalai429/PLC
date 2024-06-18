$(document).ready(function (e) {
	var urlParams = new URLSearchParams(window.location.search);
	if (urlParams.has('code')) {
		$("#dvPreLogin").hide();
	}
	else if (urlParams.has('error') && urlParams.has('error_description')) {
		$("#dvPreLoginError").show();
	}
	else {
		$("#dvPreLogin").show();
		$("#dvPreLoginError").hide();
	}
	
	//Offer page onload tracker
	var pageId = $('#pageId').val();
	if (pageId != null && pageId == "offer") {
		OfferPageClaimNowLoad();
	}	

	//GetResponseFromHPId();
	if ($('#PreLoginEmail').length) {
		$(document).keypress(function (event) {
			var keycode = (event.keyCode ? event.keyCode : event.which);
			if (keycode == '13') {
				$("#PreSignInBtn").click();
				return false;
			}
		});
	}
	if ($('#PreLoginPassword').length) {
		$(document).keypress(function (event) {
			var keycode = (event.keyCode ? event.keyCode : event.which);
			if (keycode == '13') {
				try {
					commonLayer("login", "Password Submit Button");
				}
				catch (ex) { //console.log(ex);
				}
				$("#PreSignInBtnLogin").click();
				return false;
			}
		});
	}
});




/*Pre login Button Click*/
$("#PreSignInBtn").on('click', function () {
	var pageId = $('#pageId').val();
	
	try {

		commonLayer(pageId, "Next Button");

		if (pageId != null && pageId == "offer") {
			//var url = window.location.href;
			OfferPageClaimNowButton();
		}
	}
	catch (ex) { //console.log(ex);
	}

	$('.error').hide();
	var PreLoginEmailId = $('#PreLoginEmail').val();
	var hdNotification = $('#hdHpNotification').val();
	var maskingEmailCmsText = $('#maskingEmail').val();
	var maskingMobileCmsText = $('#maskingMobile').val();

	var token = $('input[name="__RequestVerificationToken"]').val();

	if (PreLoginEmailId == "" || (validateEmail(PreLoginEmailId) === false && validateMobile(PreLoginEmailId) === false)) {
		$('#UserNameRequired').show();
		$('.plcpwd').hide();
		$('#PreLoginEmail').focus();
	}
	else {
		$("#loader").css('display', 'block');
		$.ajax({
			//async: false,
			type: "POST",
			url: "/umbraco/Surface/Home/PreLoginCheck",
			data: { __RequestVerificationToken: token, "UserName": PreLoginEmailId },
			//contentType: "application/json; charset=utf-8",
			//dataType: "JSON",
			success: function (e) {
			
				if (e.Status == "Success") {
					if (e.LoginType == "new") //New User 
					{
						//location.href = e.Navigation;
						$('.ofr-frm').show();
						$('.prepwdlogin').hide();
						$('#digit-1').focus();

						ClearOtp();

						try {
							//commonLayer('Login', 'Generate OTP Registration');
							commonLayer('Registration', 'Generate OTP Registration');
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
					else if (e.LoginType == "hpidntpwd" || e.LoginType == "pwdntset") {
						$('.ofr-frm').hide();
						Swal.fire({
							//title: e.message,
							html: hdNotification,
							//icon: 'success',
							showCancelButton: false,
							confirmButtonColor: '#3085d6',
							cancelButtonColor: '#d33',
							confirmButtonText: 'OK'
						}).then((result) => {
							if (result.isConfirmed) {
								try {
									commonLayer(pageId, 'Login HPID Removal Accept');
								}
								catch {
									//console.log('');
								}

								TermsAcceptSendOtp(PreLoginEmailId);
							}
						});
					}
					else if (e.LoginType == "hpidpwd" || e.LoginType == "pwdset") {
						$('.ofr-frm').hide();
						$(".plcpwd").show();
						$(".prepwdlogin").hide();
						$(".postpwdlogin").show();
						$("#PreLoginPassword").focus();

					}
					else if (e.LoginType == "otp") {
						$('.ofr-frm').show();
						$('.prepwdlogin').hide();
						ClearOtp();
						try {
							commonLayer('Login', 'Generate OTP Login');
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
					if (e.ValidateMessage != null || e.ValidateMessage != "") {//maximum validate attempt
						$("#MsgOtp").show();
						var msg = $('#resendOtpTimerTitle').val();
						$("#MsgOtp").html(msg);
						countdownMaxFailedRoot(e.ValidateMessage, msg);
					}
					return false;
				}
				else if (e.Status === "MAXRATT") {//Resend Case
					if (e.ValidateMessage != null && e.ValidateMessage != "") {
						var msgg = $('#resendOtpTimerTitle').val();
						countdownMaxFailedRoot(e.ValidateMessage, msgg);
					}
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

/*Password Button Click*/
$("#PreSignInBtnLogin").on('click', function () {
	var pageId = $('#pageId').val();
	
	try {
		commonLayer(pageId, "Password Button Click");

		if (pageId != null && pageId == "login") {
			//var url = window.location.href;
			LoginPage_gtag_report_conversion();
		}
	}
	catch (ex) { //console.log(ex);
	}

	$('.error').hide();
	var PreLoginUserId = $('#PreLoginEmail').val();
	var PreLoginPwdId = $('#PreLoginPassword').val();


	if (PreLoginPwdId) {

		var token = $('input[name="__RequestVerificationToken"]').val();

		if (PreLoginUserId === "" || (validateEmail(PreLoginUserId) === false) && validateMobile(PreLoginUserId) === false) {
			$('#UserNameRequired').show();
			$('#PreLoginEmail').focus();
		}
		else if (PreLoginPwdId === "" || (validatePassword(PreLoginPwdId) === false)) {
			$('#PreLoginPasswordRequired').show();
			$('#PreLoginPwdId').focus();

			$('#UserNameRequired').hide();
		}
		else {
			$("#loader").css('display', 'block');

			var Input = {
				UserName: PreLoginUserId,
				PwdText: PreLoginPwdId,
				PageId: pageId,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				url: "/umbraco/Surface/Home/Login",
				data: Input,
				//contentType: "application/json",
				//dataType: "JSON",
				success: function (e) {
					
					if (e.status === "Success") {
						try {
							if (e.EnableTrackerCode == "Y") {
								Loginlayer(e.UniqueUserId);
							}
						} catch (ex) {
							//console.log(ex);
						}

						var IsBundleUser = $("#IsBundleUser").val();
						var IsOfferUser = $("#IsOfferUser").val();
						var IsMyOfferUser = $("#IsMyOfferUser").val();
						var bundlingRedirectUrl = $("#bundlingRedirectUrl").val();
						/*if (IsBundleUser.toUpperCase() == "YES" || IsOfferUser.toUpperCase() == "YES") {*/
						if (IsOfferUser === "YES" || IsMyOfferUser === "YES") {
							window.location = bundlingRedirectUrl;
						}
						else {
							window.location = e.navigation;
						}
					}
					else if (e.status == "OTP_NM") {
						//Swal.fire('Please enter valid Otp!');
						$("#MsgOtp").show();
						var wrongOTPValidation = $('#wrongOTPValidation').val();
						$('#VerifyRegistration').attr("disabled", false);
						$('#MsgOtp').html(wrongOTPValidation);

						commonLayer('Registration', 'Invalid OTP');
					}
					else if (e.status == "Exceed") {

						$("#MsgOtp").show();
						var oTPAttemptMaximumLimit = $('#oTPAttemptMaximumLimit').val();

						$('#MsgOtp').html(oTPAttemptMaximumLimit);
						$('#VerifyRegistration').attr("disabled", false);
					}
					else if (e.status === "MAXFATT") {
						//maximum validate attempt
						var msg = $('#validateAttemptTitle').val();
						$("#timerMsg").html(msg);
						countdownMaxFailed();

						return false;
					}
					else if (e.status === "error") {
						$("#dvPreLogin").hide();
						$("#error").show();

						$("#error").html(e.message);
					}
					else if (e.status === "Fail") {
						$("#preloginmessage").show();
						$("#preloginmessage").html("Entered username and password is incorrect.");
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
});


function RegistrationResendOtp() {
	$('.error').hide();
	
	try {
		commonLayer('Registration', 'ReGenerate OTP');
	}
	catch (ex) {
		//console.log(ex);
	}

	$("#digit-1").val('');
	$("#digit-2").val('');
	$("#digit-3").val('');
	$("#digit-4").val('');

	$("#loader").css('display', 'block');
	/*var name = $('#name').val();*/
	/*var email = $('#email').val();*/
	var userName = $('#PreLoginEmail').val();
	//var maskingEmailCmsText = $('#maskingEmail').val();
	//var maskingEmailMobileCmsText = $('#maskingEmailMobile').val();
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
					if (email != "" && mobileno != "") {
						$('#MaskingMessage').html(maskingEmailMobileCmsText.toString().replace('{email}', e.mailmasking).replace('{mobile}', e.mobmasking));
					}
					else {
						$('#MaskingMessage').html(maskingEmailCmsText.toString().replace('{email}', e.mailmasking));
					}
				}
				catch {
					//console.log();
				}

				try {
					commonLayer('Registration', 'ReSend OTP Sent');

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
			else if (e.status === "REGIST") {
				$("#Msg").show();

				$("#Msg").html("The EmailId already registered, Please login to continue.");
				return false;
			}
			else if (e.status === "EmailExt") {
				$("#MsgOtp").show();
				var emailAlreadyRegistered = $('#emailAlreadyRegistered').val();

				$("#MsgOtp").html(emailAlreadyRegistered);
				return false;
			}
			else if (e.status === "MobileExt") {
				$("#MsgOtp").show();
				var mobileAlreadyRegistered = $('#mobileAlreadyRegistered').val();

				$("#MsgOtp").html(mobileAlreadyRegistered);
				return false;
			}
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


/*Otp Button Click*/
$("#VerifyOtp").on('click', function () {
	$('.error').hide();
	$("#loader").css('display', 'block');
	var bundlingRedirectUrl = $('#bundlingRedirectUrl').val();

	var token = $('input[name="__RequestVerificationToken"]').val();

	var otp1 = $("#digit-1").val();
	var otp2 = $("#digit-2").val();
	var otp3 = $("#digit-3").val();
	var otp4 = $("#digit-4").val();
	var Otp = otp1.concat(otp2, otp3, otp4);
	
	if (!Otp) {
		$('#MsgOtp').show();
		var enterOTPValidation = $('#enterOTPValidation').val();

		$('#MsgOtp').html(enterOTPValidation);
		return false;
	}

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
			
			if (e.Status == "SUCCESS") {
				try {
					if (e.LoginType == "new") {
						commonLayer('Registration', 'OTP Successfull');
					}
					else {
						commonLayer('Login', 'OTP Successfull');
					}

					//Loginlayer(e.UniqueUserId);
				} catch (ex) {
					//console.log(ex);
				}
				
				if ((e.LoginType == "new" || e.LoginType == "otp") && e.IsotpVerified == 1) {
					location.href = e.Navigation;
				}
				else {
					if (bundlingRedirectUrl != null && bundlingRedirectUrl != "") {
						location.href = bundlingRedirectUrl;
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
			else {
				$("#error").show();

				$('#error').html("Otp not matched!");
			}
		},
		error: function (error) {
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
});


//$("#PreSignInBtn").on('click', function () {
//	var pageId = $('#pageId').val();

//	try {

//		commonLayer(pageId, "Next Button");
		
//		if (pageId != null && pageId == "offer") {
//			//var url = window.location.href;
//			OfferPageClaimNowButton();
//		}
//	}
//	catch (ex) { //console.log(ex);
//	}

	
	
//	$('.error').hide();
//	var PreLoginEmailId = $('#PreLoginEmail').val();
//	var hdNotification = $('#hdHpNotification').val();

//	var token = $('input[name="__RequestVerificationToken"]').val();

//	if (PreLoginEmailId == "" || (validateEmail(PreLoginEmailId) === false && validateMobile(PreLoginEmailId) === false)) {
//		$('#UserNameRequired').show();
//		$('.plcpwd').hide();
//		$('#PreLoginEmail').focus();
//	}
//	else {
//		$("#loader").css('display', 'block');
//		$.ajax({
//			//async: false,
//			type: "POST",
//			url: "/umbraco/Surface/Home/PreLoginCheck",
//			data: { __RequestVerificationToken: token, "UserName": PreLoginEmailId },
//			//contentType: "application/json; charset=utf-8",
//			//dataType: "JSON",
//			success: function (e) {
				
//				if (e.Status == "Success") {
//					if (e.UserRegistered === "0") {
//						location.href = e.Navigation;
//					}
//					else if (e.UserRegistered == "1" && e.StatusOfJrny == "1") {

//						$(".plcpwd").show();
//						$(".prepwdlogin").hide();
//						$(".postpwdlogin").show();
//						$("#PreLoginPassword").focus();
//					}
//					else if (e.UserRegistered == "1" && e.StepsCompletted.toString() == "2" && e.ProfileStatus == 1) {
//						window.location = e.Navigation;//
//					}
//					else if (e.UserRegistered == "1" && e.ProfileStatus == 2) {

//						Swal.fire({
//							//title: e.message,
//							html: hdNotification,
//							//icon: 'success',
//							showCancelButton: false,
//							confirmButtonColor: '#3085d6',
//							cancelButtonColor: '#d33',
//							confirmButtonText: 'OK'
//						}).then((result) => {
//							if (result.isConfirmed) {
//								try {
//									commonLayer(pageId, 'Login HPID Removal Accept');
//								}
//								catch {
//									//console.log('');
//								}

//								TermsAcceptSendOtp(PreLoginEmailId);

//							}
//						});
//					}
//				}
//			},
//			error: function (error) {
//				//alert('error');
//				$("#loader").css('display', 'none');
//			}, complete: function () {
//				//alert('complete');
//				$("#loader").css('display', 'none');
//			}
//		});
//	}
//});

//$("#PreSignInBtnLogin").on('click', function () {
//	var pageId = $('#pageId').val();
//	try {
//		commonLayer(pageId, "Password Button Click");

//		if (pageId != null && pageId == "login") {
//			//var url = window.location.href;
//			LoginPage_gtag_report_conversion();
//		}
//	}
//	catch (ex) { //console.log(ex);
//	}

//	$('.error').hide();
//	var PreLoginUserId = $('#PreLoginEmail').val();
//	var PreLoginPwdId = $('#PreLoginPassword').val();


//	if (PreLoginPwdId) {

//		var token = $('input[name="__RequestVerificationToken"]').val();

//		if (PreLoginUserId === "" || (validateEmail(PreLoginUserId) === false) && validateMobile(PreLoginUserId) === false) {
//			$('#UserNameRequired').show();
//			$('#PreLoginEmail').focus();
//		}
//		else if (PreLoginPwdId === "" || (validatePassword(PreLoginPwdId) === false)) {
//			$('#PreLoginPasswordRequired').show();
//			$('#PreLoginPwdId').focus();

//			$('#UserNameRequired').hide();
//		}
//		else {
//			$("#loader").css('display', 'block');

//			var Input = {
//				UserName: PreLoginUserId,
//				PwdText: PreLoginPwdId,
//				PageId: pageId,
//				__RequestVerificationToken: token
//			};

//			$.ajax({
//				type: "POST",
//				url: "/umbraco/Surface/Home/Login",
//				data: Input,
//				//contentType: "application/json",
//				//dataType: "JSON",
//				success: function (e) {

//					if (e.status === "Success") {
//						try {
//							if (e.EnableTrackerCode == "Y") {
//								Loginlayer(e.UniqueUserId);
//							}
//						} catch (ex) {
//							//console.log(ex);
//						}
					
//						var IsBundleUser = $("#IsBundleUser").val();
//						var IsOfferUser = $("#IsOfferUser").val();
//						var IsMyOfferUser = $("#IsMyOfferUser").val();
//						var bundlingRedirectUrl = $("#bundlingRedirectUrl").val();
//						/*if (IsBundleUser.toUpperCase() == "YES" || IsOfferUser.toUpperCase() == "YES") {*/
//						if (IsOfferUser === "YES" || IsMyOfferUser === "YES") {
//							window.location = bundlingRedirectUrl;
//						}
//						else if (e.message == "SPR_365Plan")
//						{
//							paynowSpecialPlan();
//						}
//						else {
//							window.location = e.navigation;
//						}
//					}
//					else if (e.status === "register") {
//						window.location = e.navigation;
//					}
//					else if (e.status === "error") {
//						$("#dvPreLogin").hide();
//						$("#error").show();

//						$("#error").html(e.message);
//					}
//					else if (e.status === "Fail") {
//						$("#preloginmessage").show();
//						$("#preloginmessage").html("Entered username and password is incorrect.");
//						return false;
//					}
//				},
//				error: function (error) {
//					$("#loader").css('display', 'none');
//				}, complete: function () {
//					$("#loader").css('display', 'none');
//				}
//			});
//		}
//	}
//});


$('#PreLoginEmail').keypress(function (e) {
	$('.error').hide();
});

$('#PreLoginPassword').keypress(function (e) {
	$('.error').hide();
});

function TermsAcceptSendOtp(username) {

	var token = $('input[name="__RequestVerificationToken"]').val();
	var Input = {
		UserName: username,
		__RequestVerificationToken: token
	};

	$("#loader").css('display', 'block');
	$.ajax({
		type: "POST",
		url: "/umbraco/Surface/Home/LoginOtpSend",
		data: Input,
		//contentType: "application/json",
		//dataType: "JSON",
		success: function (e) {

			if (e.status == "Success") {
				try {
					commonLayer('Login', 'Login HPID Removal Accept OTP Sent');
				}
				catch {// console.log();
				}

				window.location = e.navigation;
			}
			else if (e.status == "Exceed") {
				Swal.fire("You have attempted maximum, You can try after 15 min.");
			}
			else {
				Swal.fire("Something Wrong!! Please try again.");
			}
		},
		error: function (error) {
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
}

//function GetResponseFromHPId() {

//	var hpUid = $("#hdHpIdCode").val();
//	var hdRegistrationMsg = $("#hdRegistrationMsg").val();
//	var bundlingRedirectUrl = $("#bundlingRedirectUrl").val();

//	if (hpUid) {
//		$("#loader").css('display', 'block');
//		$.ajax({
//			type: "POST",
//			url: "/umbraco/Surface/Home/Login",
//			data: JSON.stringify({ "HpIdJson": hpUid }),
//			contentType: "application/json",
//			dataType: "JSON",
//			success: function (e) {

//				if (e.status == "Success") {
//					try {
//						if (e.EnableTrackerCode == "Y") {
//							Loginlayer(e.UniqueUserId);
//						}
//					} catch (ex) {
//						//console.log(ex);
//					}

//					var IsBundleUser = $("#IsBundleUser").val();
//					var IsOfferUser = $("#IsOfferUser").val();
//					/*if (IsBundleUser.toUpperCase() == "YES" || IsOfferUser.toUpperCase() == "YES") {*/
//					if (IsOfferUser.toUpperCase() == "YES") {
//						window.location = bundlingRedirectUrl;
//					}
//					else {
//						window.location = e.navigation;
//					}
//				}
//				else if (e.status == "register") {
//					window.location = e.navigation;
//				}
//				else if (e.status == "error") {
//					$("#dvPreLogin").hide();
//					$("#error").show();

//					$("#error").html(e.message);
//				}
//				else if (e.status == "Fail") {
//					$("#preloginmessage").show();
//					$("#preloginmessage").html("Please enter correct email id.");
//					return false;
//				}
//			},
//			error: function (error) {
//				$("#loader").css('display', 'none');
//			}, complete: function () {
//				$("#loader").css('display', 'none');
//			}
//		});
//	}
//	else {
//		ManageCulturePostLandFromHPID();
//	}
//}

//function ManageCulturePostLandFromHPID() {
//	$("#loader").css('display', 'block');
//	$.ajax({
//		type: "POST",
//		url: "/umbraco/Surface/Home/LoginRedirection",
//		contentType: "application/json",
//		dataType: "JSON",
//		success: function (e) {

//			if (e.status == "Success") {
//				window.location = e.navigation;
//			}
//		},
//		error: function (error) {
//			$("#loader").css('display', 'none');
//		}, complete: function () {
//			$("#loader").css('display', 'none');
//		}
//	});
//}

