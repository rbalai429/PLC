
var pathname = window.location.pathname;
var offerPageName = pathname.replace("/", "").replace("/", "").replace("/", "");

if (window.location.href.indexOf("offer")) {
	var offerName = getParameterByName("offer", window.location.href);
	$("#page").val(offerName);

	//commonLayer("Offer Name", offerName);
}
var pageName = $("#page").val();

$(document).ready(function (e) {
	if ($('#PreLoginEmail').length) {
	$(document).keypress(function (event) {
		var keycode = (event.keyCode ? event.keyCode : event.which);
		if (keycode == '13') {
			$("#PreSignInBtn").click();
			return false;
		}
	});
	}

	if ($('input[name=termsChecked]:checkbox:checked').length > 0) {
		$(document).keypress(function (event) {
			var keycode = (event.keyCode ? event.keyCode : event.which);
			if (keycode == '13') {
				$("#register").click();
				return false;
			}
		});
	}
});

if (pageName == "" || pageName == null || pageName == undefined) {
	pageName = "Header";
}

if (offerPageName == "" || offerPageName == null || offerPageName == undefined) {
	offerPageName = pageName;
}

if (offerPageName == "my-accountlogin") {
	offerPageName = pageName;
}

$(".loginClose").click('click', function () {
	try {
		var closeDesc = $("#closeDesc").val();
		commonLayer(pageName, closeDesc);
	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlayLogin").hide();
});


$("#PreSignInBtn").on('click', function () {
	
	var pageId = $('#pageId').val();
	
	try {
		commonLayer(offerPageName, "Next Button");

		if (pageId != null && pageId == "offer") {
			//var url = window.location.href;
			OfferPageClaimNowButton();
		}
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
		$("#loader").show();
		$.ajax({
			//async: false,
			type: "POST",
			url: "/umbraco/Surface/Home/PreLoginCheck",
			data: { __RequestVerificationToken: token, "UserName": PreLoginEmailId, "Source": pageName},
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
						$('#modeToEnter').html('Enter Mobile no.');
					}

					$('#SP365Login').hide();
					$('#SP365Otp').show();
					$('.forFocus').focus();

					$('input.otpspecialplanfield').bind('copy paste', function (e) {
						e.preventDefault();
					});

					ClearOtp();

					try {
						//commonLayer('Login', 'Generate OTP Registration');
						commonLayer(offerPageName, 'Generate OTP');
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
				$("#loader").hide();
			}, complete: function () {
				//alert('complete');
				$("#loader").hide();
			}
		});
	}
});


$("#PreSignInBtnFreeTrial").on('click', function () {

	//var pageId = $('#pageId').val();
	//debugger
	try {
		commonLayer(offerPageName, "Next Button");
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
		$("#loader").show();
		$.ajax({
			//async: false,
			type: "POST",
			url: "/umbraco/Surface/Home/PreLoginCheck",
			data: { __RequestVerificationToken: token, "UserName": PreLoginEmailId, "Source": pageName },
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
						$('#modeToEnter').html('Enter Mobile no.');
					}

					$('#sp365Consent').hide();
					$(".overlayLogin").show();
					$('#SP365Login').hide();
					$('#SP365Otp').show();
					$('.forFocus').focus();
					

					$('input.otpspecialplanfield').bind('copy paste', function (e) {
						e.preventDefault();
					});

					ClearOtp();

					try {
						//commonLayer('Login', 'Generate OTP Registration');
						commonLayer(offerPageName, 'Generate OTP');
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

					var remember = $('input[name=R_U_Want_to_LoggedIn]:checkbox:checked').length;
					if (remember == "0") {
						$("#R_U_Want_to_LoggedIn").prop("checked", false);
					}
					else if (remember == "1") {
						$("#R_U_Want_to_LoggedIn").prop("checked", true);
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
				$("#loader").hide();
			}, complete: function () {
				//alert('complete');
				$("#loader").hide();
			}
		});
	}
});

//const isBrowserSupport = () => globalThis?.OTPCredential;

//const abortAutoFill = (abort, time = 2) => {
//		setTimeout (() => {
//					abort.abort();
//				}, time * 60 * 1000);
//		// Abort the controller in 2sec by default
//		};

//if(isBrowserSupport()) {
//	const abort =  new	AbortController();

//	abortAutoFill(abort);

//	navigator.credentials.get({
//		otp	: {	transport: ['sms']},
//		signal	: abort.signal,
//	}).then(
//		(otp) => {
//			const code = otp;
//			alert(otp);
//			var optparra = otp.split('');
//			$.each(optparra, function (index, value) {
//				const otpindex = index;
//				const otpVal = value;
//				$('#otp-input input').each(function (index, value) {
//					if (otpindex == index) {
//						$(this).val(otpVal);
						
//					}
//				});

//			});

//			OtpValidation(otp);
//	}).catch((error) => console.log('ERROR'));
//}
//else {
//	console.log('UN_SUPPORTED_FEATURE');
//}

/*$("#otp-input input").on('change click keyup input paste', function (e) {*/
function OtpValidation(inputValue) {
	//console.log(inputValue);
	//alert(inputValue);
	$('.error').hide();


	/*$(document).on('change click keyup input paste', '#otp-input input', function (e) {*/
	//var pageId = $('#pageId').val();


	//if (this.value.length == this.maxLength) {
	//	$(this).next('.otpspecialplanfield').focus();
	//}
	//else {
	//	$(this).prev('.otpspecialplanfield').focus();
	//}

	//var otp1 = $("#digit-1").val();
	//var otp2 = $("#digit-2").val();
	//var otp3 = $("#digit-3").val();
	//var otp4 = $("#digit-4").val();

	//var Otp = otp1.concat(otp2, otp3, otp4);

	var Otp = $("#otp").val();

	if ((inputValue != null || inputValue != "") && inputValue.length > 0) {
		$('#clear').show();
	}
	else {
		$('#clear').hide();
	}

	if (!inputValue) {
		$('#MsgOtp').show();
		var enterOTPValidation = $('#enterOTPValidation').val();

		$('#MsgOtp').html(enterOTPValidation);
		return false;
	}
	else {
		if ((inputValue != null || inputValue != "") && inputValue.length == 4) {
			//console.log('Wrongg otp');
			$("#loader").show();

			var token = $('input[name="__RequestVerificationToken"]').val();
			var R_U_Want_to_LoggedIn;
			var remember = $('input[name=R_U_Want_to_LoggedIn]:checkbox:checked').length;
			if (remember == "0") {
				R_U_Want_to_LoggedIn = "No";
			}
			else if (remember == "1") {
				R_U_Want_to_LoggedIn = "Yes";
			}

			var Input = {
				OneTimePwd: Otp,
				"Rememberme": R_U_Want_to_LoggedIn,
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
							commonLayer(offerPageName, 'OTP Successfull');

							//	//commonLayer(pageName, 'Login Successfull');

							//	//facebook pixel
							//	SpecialPlanFacebookPicelCode_Login();
						} catch (ex) {
							//console.log(ex);
						}
						//var userIsExistingOrNew = $('#userIsExistingOrNew').val();

						if (e.LoginType == 'new') {
							$("#sp365Consent").show();
							$('#SP365Login').hide();
							$('#SP365Otp').hide();

							var ReferralCode = getUrlVars()["referralcode"];
							if (ReferralCode == "undefined" || ReferralCode == undefined) { ReferralCode = ''; }
							$("#ReferralCode").val(ReferralCode);

							//if User tries for Teacher then 
							var hdnSourceOfProgram = $("#hdnSourceOfProgram").val();

							if (hdnSourceOfProgram != '' && hdnSourceOfProgram == "teacher") {

								$("#RuParentOrStudent option[value='0']").prop('disabled', true);
								$("#RuParentOrStudent option[value='Parent']").prop('disabled', true);
								$("#RuParentOrStudent option[value='Teacher']").prop('selected', true);

							}
							else {

								$("#RuParentOrStudent option[value='0']").prop('disabled', false);
								$("#RuParentOrStudent option[value='Parent']").prop('disabled', false);
							}

							$("#RuParentOrStudent").multipleSelect('refresh');
							//$('.multiple-select').multipleSelect({
							//	placeholder: 'Select Age',
							//	selectAll: false
							//});

							var otpVerifiedMode = $('#otpVerifiedMode').val();
							$("#name").keyup(function (event) {

								var regex = new RegExp("^[a-zA-Z ]+$");
								var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);

								if (!regex.test(key)) {

									event.preventDefault();

									$("#name").val($("#name").val().replace(/[^a-zA-z ]/gi, ''));

									return false;
								}
							});

							if (otpVerifiedMode == 'email') {
								$("#useremailormobile").attr('maxlength', '10');
								$("#useremailormobile").keyup(function (event) {
									var regex = new RegExp("^[0-9]+$");
									var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);

									if (!regex.test(key)) {

										event.preventDefault();

										$("#useremailormobile").val($("#useremailormobile").val().replace(/[^0-9]/gi, ''));

										return false;
									}
								});
							}
							else {
								if (otpVerifiedMode == 'mobile') {
									$("#useremailormobile").attr('maxlength', '200');
									//$("#useremailormobile").keyup(function (event) {
									//	debugger
									//	/*var regex = new RegExp('[a-z0-9]+@[a-z]+\.[a-z]{2,3}');*/
									//	var regex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i

									//	if (!regex.test($(this).val())) {
									//		return false;
									//	} else {
									//		 return true;
									//	}
									//	//if (!regex.test($(this).val())) {

									//	//	event.preventDefault();

									//	//	$("#useremailormobile").val($("#useremailormobile").val().replace(regex, ''));

									//	//	return false;

									//	//}

									//});

								}
							}
						}
						else {
							$(".agegroupfixedrow").hide();
							$(".overlayLogin").hide();

							try {
								Loginlayer(e.UserUniqueCode);
								//commonLayer(offerPageName, 'Login Successfull');
								//Loginlayer(e.UniqueUserId);
								//facebook pixel
								SpecialPlanFacebookPicelCode_Login();
							} catch (ex) {
								//console.log(ex);
							}
							//Swal.fire('Hi, your 365 plan is already activated.');
							//location.href = window.location.href;

							var mode = $("#hdnClickedWorksheetMode").val();
							var downloadUrl = $("#hdnClickedWorksheetDownloadUrl").val();
							var IsPaidWorksheet = $("#hdnClickedWorksheetIsPaid").val();
							var trackdata = $("#hdnClickedWorksheetTracked").val();
							var source = $("#hdnClickedSource").val();
							var nodeId = $("#hdnDownloadNodeIdSource").val();
							var sourceOfPrm = $("#hdnSourceOfProgram").val();
							var ageGroup = $("#hdnAgeGroup").val();

							//Header Login
							//var IsLoginWithHeader = $("#hdnLoginClickedMode").val();

							if (mode != null && mode != "" && mode != undefined && sourceOfPrm != '' && sourceOfPrm == 'teacher') {
								PrintDownloadForWorksheetsTeachers(mode, downloadUrl, IsPaidWorksheet, trackdata, 'justlogin', source, nodeId, ageGroup);
							}
							else if (mode != null && mode != "" && mode != undefined && sourceOfPrm == 'specialoffer') {
								SpecialOffersDownload(mode, downloadUrl, 'justlogin');
							}
							else if (mode != null && mode != "" && mode != undefined && sourceOfPrm != '') {
								PrintDownloadForWorksheets(mode, downloadUrl, IsPaidWorksheet, trackdata, 'justlogin', source, nodeId);
							}

							else if (e.Navigation != null && e.Navigation != "" && e.Navigation != undefined) {
								window.location = e.Navigation;
							}
							//else if (IsLoginWithHeader != null && IsLoginWithHeader != "" && IsLoginWithHeader != undefined && IsLoginWithHeader == "H")
							//{
							//	CheckUserIsEligibeleForMessage();
							//}
							else {
								window.location = window.location.href;
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
					$("#loader").hide();
				}, complete: function () {
					$("#loader").hide();
				}
			});
		}
	}
}
/*});*/

$("#register").on('click', function (e) {
	//var pageId = $('#pageId').val();
	//try {
	//	//var url = window.location.href;
	//	SpecialPlanFacebookPicelCode_Registration();
	//}
	//catch (ex) {
	//	//console.log(ex);
	//}

	$(".error").hide();

	var userEmailormobile = $('#useremailormobile').val();
	var name = $('#name').val();
	var otpVerifiedMode = $('#otpVerifiedMode').val();
	var agegroup = $('#ageGroup').val();
	var RuParentOrStudent = $("#RuParentOrStudent").val();
	var ReferedBy = $("#ReferralCode").val();

	if (userEmailormobile != '' && userEmailormobile != "" && userEmailormobile != null && userEmailormobile != undefined) {

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
		else if (name == '' || name == "" || name == null) {

			$('#nameRequired').show();
			$('#name').focus();

			return false;
		}
		else if (agegroup == "0" || agegroup == null || agegroup == "") {
			$('#agegroupRequired').show();
			$('#ageGroup').focus();
			return false;
		}
		else if (RuParentOrStudent == "" || RuParentOrStudent == null || agegroup == undefined) {
			$('#AreYouParentOrStudent').show();
			$('#RuParentOrStudent').focus();
			return false;
		}
		else {

			$("#loader").show();

			var firstSource = $('#PreLoginEmail').val();

			var supportOnWhatsupFromHP_v = $('input[name=supportOnWhatsupFromHP]:checkbox:checked').val();
			var supportOnPhoneFromHP_v = $('input[name=supportOnPhoneFromHP]:checkbox:checked').val();
			var supportOnEmailFromHP_v = $('input[name=supportOnEmailFromHP]:checkbox:checked').val();


			var TC = "0";//$('input[name=termsChecked]:checkbox:checked').length;
			/*mobileno: userEmailormobile,*/
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				email: firstSource,
				name: name,
				mobileno: userEmailormobile,
				supportOnEmailFromHP: supportOnEmailFromHP_v,
				supportOnWhatsupFromHP: supportOnWhatsupFromHP_v,
				supportOnPhoneFromHP: supportOnPhoneFromHP_v,
				ageGroup: agegroup,
				termsChecked: TC,
				page: pageName,
				RuParentOrStudent: RuParentOrStudent,
				ReferedBy: ReferedBy,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/SecondFormRegistration",
				data: Input,
				success: function (e) {
					
					if (e.status == "Success") {
						
						try {

							//commonLayer(offerPageName, 'Registration');

							RegistrationLayer(e.UserUniqueCode, "");
							RegistrationLayerAvatar();
							RegistrationLayerAvatarUserRegister();

						} catch (ex) {
							//console.log(ex);
						}

						//location.href = window.location.href;
						//paynowSpecialPlan();

						//$(".loginPopup").hide();
						$("#overlayLoginDiv").hide();

						var mode = $("#hdnClickedWorksheetMode").val();
						var downloadUrl = $("#hdnClickedWorksheetDownloadUrl").val();
						var IsPaidWorksheet = $("#hdnClickedWorksheetIsPaid").val();
						var trackdata = $("#hdnClickedWorksheetTracked").val();
						var source = $("#hdnClickedSource").val();
						var nodeId = $("#hdnDownloadNodeIdSource").val();
						var sourceOfPrm = $("#hdnSourceOfProgram").val();
						var ageGroup = $("#hdnAgeGroup").val();

						//Header Login
						//var IsLoginWithHeader = $("#hdnLoginClickedMode").val();

						
						//Header Login
						//var IsLoginWithHeader = $("#hdnLoginClickedMode").val();

						//if (mode == null && mode != "" && mode != undefined) {
						//	PrintDownloadForWorksheets(mode, downloadUrl, IsPaidWorksheet, trackdata, 'justlogin', source);
						//}
						if (mode != null && mode != "" && mode != undefined && sourceOfPrm != '' && sourceOfPrm == 'teacher') {
							PrintDownloadForWorksheetsTeachers(mode, downloadUrl, IsPaidWorksheet, trackdata, 'justlogin', source, nodeId, ageGroup);
						}
						else if (mode != null && mode != "" && mode != undefined && sourceOfPrm == 'specialoffer') {
							SpecialOffersDownload(mode, downloadUrl, 'justlogin');
						}
						else if (mode != null && mode != "" && mode != undefined && sourceOfPrm != '') {
							PrintDownloadForWorksheets(mode, downloadUrl, IsPaidWorksheet, trackdata, 'justlogin', source, nodeId);
						}
						else if (e.Navigation != null && e.Navigation != "" && e.Navigation != undefined) {
							window.location = e.Navigation;
						}
						else {
							window.location = window.location.href;
						}
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
					else if (e.status === "referral") {
						$("#message").show();
						$("#message").html(e.message);

						return false;
					}
					else if (e.status === "Fail") {
						$("#message").show();
						$("#message").html("Registration unsuccessfull, Please try again!");

						return false;
					}
				},
				error: function (error) {
					$("#loader").hide();
				}, complete: function () {
					$("#loader").hide();
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



function Login(mode, downloadUrl, Ispaid, trackingdata, source, nodeId, msgIdforLoginwndw) {

	try {

		CaptureClickedEvent(mode, downloadUrl, Ispaid, trackingdata, source, nodeId,"worksheet");

		FetchLoginMessageAjax(msgIdforLoginwndw);
		
		var pageName = $("#page").val();
		if (mode == "print") {
			commonLayer(pageName, "Clicked on Print");
		}
		else if (mode == "dwnld") {
			commonLayer(pageName, "Clicked on Download");
		}
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlayLogin").show();
}

function downloadDocmnt(mode, downloadUrl, Ispaid, trackdata, source, nodeId, msgIdforLoginwndw) {

	//try {
	//	if (mode == "print") {
	//		commonLayer(pageName, "Clicked on Print Icon");
	//	}
	//	else if (mode == "dwnld") {
	//		commonLayer(pageName, "Clicked on Download Icon");
	//	}
	//}
	//catch (ex) {
	//	//console.log('error');
	//}
	
	CaptureClickedEvent(mode, downloadUrl, Ispaid, trackdata, source, nodeId,"worksheet","");

	PrintDownloadForWorksheets(mode, downloadUrl, Ispaid, trackdata, '', source, nodeId);

	try {
		FetchLoginMessageAjax(msgIdforLoginwndw);
	}
	catch {
		//console.log('');
	}
}

function PrintDownloadForWorksheets(mode, downloadUrl, Ispaid, trackdata, evnt, source, nodeId) {
	
	if (mode != '' && mode != "" && mode != null && mode != undefined && Ispaid != '' && Ispaid != "" && Ispaid != null && Ispaid != undefined) {

		try {
			if (mode == "dwnld") {
				addDownloadDataLayer("Bonus", trackdata, pageName);
			}
			if (mode == "print") {
				printTracker("Bonus", trackdata, pageName);
			}
		}
		catch (ex) {
			//console.log('error');
		}

		//Free Worksheet Download
		if (Ispaid == "Free" && mode == "dwnld") {

			if (evnt != null && evnt != '' && evnt != "" && evnt == "justlogin") {
				$("#freeDwnldJustAfterLogin").show();
			}
			else {
				$("#freeDwnld").show();
			}

			location.href = downloadUrl;
		}

		else if (Ispaid == "Free" && mode == "print") {
			if (evnt == "justlogin") {
				location.href = window.location.href;
			}
			else {
				//try {
				//	printTracker(trackdata, pageName);
				//}
				//catch (ex) {
				//	//console.log('error');
				//}

				PrintWorkSheet(downloadUrl);
			}
		}//Free Worksheet Download end
		else if (Ispaid == "Paid") {

			var Input = {
				Source: source,
				NodeId: nodeId
			};

			$.ajax({
				type: "POST",
				url: "/umbraco/Surface/StructuredProgram/DownloadEligibility",
				data: Input,
				success: function (e) {
					debugger
					if (e.RemainingValidityInDays <= 0 || e.RemainingValidityInDays == 100099878) {//plan expired
						var ValidityExp = "You've exhausted your download/print limit under the current plan. Renew today to enjoy uninterrupted access.";

						$('.cntOfDownload').html(ValidityExp);
						$("#PayRequirement").show();
						$(".closebtn").hide();
					} else {
						var popMessage = "";
						var SevenDaysLeft = "";
						var TwentyDownloadsLeft = "";
						var viewpricingactive = 0;

						if (e.RemainingValidityInDays > 0 && e.RemainingValidityInDays <= 7) {
							viewpricingactive = "1";
							SevenDaysLeft = "Your plan is about to expire! Renew today to enjoy uninterrupted access.";
						}
						if (e.RemainingWorksheetForDwnld > 0 && e.RemainingWorksheetForDwnld <= 20) {
							viewpricingactive = "1";
							TwentyDownloadsLeft = "You’re about to exhaust the number of downloads in your plan! Renew today to enjoy uninterrupted access.";
						}

						if (e.CurrentWorksheetDownloaded > 0) {

							if (mode == "print") {
								if (evnt == "justlogin") {
									location.href = window.location.href;
								}
								else {
									PrintWorkSheet(downloadUrl);
								}
							}
							else {

								downloadStrs = "Worksheet downloaded successfully! ";

								if (SevenDaysLeft == "" && TwentyDownloadsLeft == "") {
									popMessage = downloadStrs;
								}
								else if (SevenDaysLeft != "" && TwentyDownloadsLeft != "") {
									popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
								}
								else if (SevenDaysLeft == "") {
									popMessage = downloadStrs + " <br/><br/>" + TwentyDownloadsLeft;
								}
								else if (TwentyDownloadsLeft == "") {
									popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
								}
								else {
									popMessage = downloadStrs;
								}

								//popMessage = "Your worksheet is downloaded successfully " + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" +	TwentyDownloadsLeft;

								$(".cntOfDownload").html(popMessage);


								if (evnt == "justlogin") {
									$("#paidDwnldJustLgn").show();
									if (viewpricingactive == "1") {
										$(".pricing").show();
									}
								}
								else {
									$("#paidDwnld").show();
									if (viewpricingactive == "1") {
										$(".pricing").show();
									}
								}

								location.href = downloadUrl;
							}
						}
						else {
							if (source != null && source != "" && source != undefined && source == "video") {
								if (e.Result == "0" && (mode == "dwnld" || mode == "print")) {
									if (e.DownloadedVideo >= 1) {
										var downloadLimitTextVideo = "You’ve reached a download/print limit!";

										if (evnt == "justlogin") {
											$('.cntOfDownload').html(downloadLimitTextVideo);
											$("#PayRequirementJustLogin").show();
										}
										else {
											$('.cntOfDownload').html(downloadLimitTextVideo);
											$("#PayRequirement").show();
										}
									}
									else {

										if (mode == "print") {
											if (evnt == "justlogin") {
												location.href = window.location.href;
											}
											else {
												PrintWorkSheet(downloadUrl);
											}
										}
										else {
											var downloadStrsVideo = "Congratulations! Your 1 free video mapped worksheet is downloaded successfully.";

											if (evnt == "justlogin") {
												$('.cntOfDownload').html(downloadStrsVideo);
												$("#PayRequirementJustLogin").show();
											}
											else {
												$('.cntOfDownload').html(downloadStrsVideo);
												$("#PayRequirement").show();
											}

											location.href = downloadUrl;
										}
									}
								}
								else if (e.Result == "1" && (mode == "dwnld" || mode == "print")) {
									$(".cntOfDownload").html("You’ve reached a download/print limit!");
									//if (e.DownloadedVideo <= e.RemainingVideoWorksheetForDwnld) {

									//	$(".cntOfDownload").html("Your worksheet is downloaded successfully");
									//	location.href = downloadUrl;

									//}
									if (e.RemainingVideoWorksheetForDwnld > 0) {

										downloadStrs = "Worksheet downloaded successfully! ";

										if (SevenDaysLeft == "" && TwentyDownloadsLeft == "") {
											popMessage = downloadStrs;
										}
										else if (SevenDaysLeft != "" && TwentyDownloadsLeft != "") {
											popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else if (SevenDaysLeft == "") {
											popMessage = downloadStrs + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else if (TwentyDownloadsLeft == "") {
											popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else {
											popMessage = downloadStrs;
										}

										//popMessage = "Worksheet downloaded successfully!" + " <br/> <br/>" +  SevenDaysLeft + " <br/>" + TwentyDownloadsLeft;

										$(".cntOfDownload").html(popMessage);

										if (mode == "print") {
											if (evnt == "justlogin") {
												$("#paidDwnldJustLgn").show();
											}
											else {
												PrintWorkSheet(downloadUrl);
											}

										}
										else {

											if (evnt == "justlogin") {
												$("#paidDwnldJustLgn").show();
											}
											else {
												$("#paidDwnld").show();
											}

											location.href = downloadUrl;
										}
									}
									else {


										if (evnt == "justlogin") {
											$("#paidDwnldJustLgn").show();
										}
										else {
											$("#paidDwnld").show();
										}
									}

								}

							}//Video End --- Worksheet Start
							else {
								if (e.Result == "0" && mode == "print") {
									if (evnt == "justlogin") {
										location.href = window.location.href;
									}
									else {
										
										if (e.DownloadedWorksheet >= e.NoOfEligibleForDwnldWorksheet) {//User already 10 worksheets downloaded
											var downloadLimitTextPrint = "You’ve reached a download/print limit!";

											$('.cntOfDownload').html(downloadLimitTextPrint);
											$("#PayRequirement").show();
										}
										else {
											PrintWorkSheet(downloadUrl);
										}
									}
								}
								else if (e.Result == "0" && mode == "dwnld") {

									if (e.DownloadedWorksheet >= e.NoOfEligibleForDwnldWorksheet) {//User already 10 worksheets downloaded
										var downloadLimitText = "You’ve reached a download/print limit!";

										$('.cntOfDownload').html(downloadLimitText);
										$("#PayRequirementJustLogin").show();
									}
									else {
										var downloadStrs = "";
										var CntOfDownloaded = 0;
										if (e.CurrentWorksheetDownloaded > 0)
											CntOfDownloaded = e.DownloadedWorksheet
										else
											CntOfDownloaded = (e.DownloadedWorksheet + 1)

										if (e.DownloadedWorksheet == 0) {
											downloadStrs = "Congratulations! You’ve unlocked " + e.NoOfEligibleForDwnldWorksheet.toString() + " premium downloads and unlimited access to free worksheets. " + (e.NoOfEligibleForDwnldWorksheet - CntOfDownloaded).toString() + " downloads left.";
										}
										else {
											downloadStrs = "Worksheet downloaded successfully! " + (e.NoOfEligibleForDwnldWorksheet - CntOfDownloaded).toString() + " downloads left.";

											//popMessage = SevenDaysLeft + " <br/>" + downloadStrs;

										}

										$('.cntOfDownload').html(downloadStrs);

										if (evnt == "justlogin") {
											$("#freetrialJustLogin").show();
										}
										else {
											$("#freetrialLoggrdIn").show();
										}

										location.href = downloadUrl;
									}
								}
								else if (e.Result == "1" && mode == "print") {
									//User Subscribed
									if (evnt == "justlogin") {
										location.href = window.location.href;
									}
									else {
										//try {
										//	printTracker(trackdata, pageName);
										//}
										//catch (ex) {
										//	//console.log('error');
										//}

										if (e.RemainingWorksheetForDwnld > 0) {
											PrintWorkSheet(downloadUrl);
										}
										else {
											$(".cntOfDownload").html("You’ve reached a download/print limit!");
											$("#paidDwnld").show();
										}
									}
								}
								else if (e.Result == "1" && mode == "dwnld") {

									//if (e.DownloadedWorksheet <= e.RemainingWorksheetForDwnld) {

									//	$(".cntOfDownload").html("Your worksheet is downloaded successfully");
									//	location.href = downloadUrl;

									//}
									if (e.RemainingWorksheetForDwnld > 0) {

										downloadStrs = "Worksheet downloaded successfully! ";

										if (SevenDaysLeft == "" && TwentyDownloadsLeft == "") {
											popMessage = downloadStrs;
										}
										else if (SevenDaysLeft != "" && TwentyDownloadsLeft != "") {
											popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else if (SevenDaysLeft == "") {
											popMessage = downloadStrs + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else if (TwentyDownloadsLeft == "") {
											popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
										}
										else {
											popMessage = downloadStrs;
										}
										

										$(".cntOfDownload").html(popMessage);
										location.href = downloadUrl;

									}
									else {
										$(".cntOfDownload").html("You’ve reached a download/print limit!");
									}

									if (evnt == "justlogin") {
										$("#paidDwnldJustLgn").show();
										if (viewpricingactive == "1") {
											$(".pricing").show();
										}
									}
									else {
										$("#paidDwnld").show();
										if (viewpricingactive == "1") {
											$(".pricing").show();
										}
									}
								}
							}
						}
					}
				},
				error: function (error) {

				}
			});
		}
	}
}

function CaptureClickedEvent(mode, downloadUrl, Ispaid, tracingData, source, nodeId, SourceofPrm, AgeGroup) {
	$("#hdnClickedWorksheetMode").val(mode);
	$("#hdnClickedWorksheetDownloadUrl").val(downloadUrl);
	$("#hdnClickedWorksheetIsPaid").val(Ispaid);
	$("#hdnClickedWorksheetTracked").val(tracingData);
	$("#hdnClickedSource").val(source);
	$("#hdnDownloadNodeIdSource").val(nodeId);
	$("#hdnSourceOfProgram").val(SourceofPrm);
	$("#hdnAgeGroup").val(AgeGroup);
}



function HeaderLogin(pageName) {

	try {
		commonLayer("Header", "Clicked on Header Login");
	}
	catch (ex) {
		//console.log('error');
	}

	$("#hdnLoginClickedMode").val("H");
	$(".overlayLogin").show();
}


function PlayVideoLogin(pageName) {

	try {
		commonLayer(pageName, "Clicked on Play Video Login");
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlayLogin").show();
}


function SubscriptionByNowLogin() {

	try {
		commonLayer("Buy Now", "Clicked on Play Video Login");
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlayLogin").show();
}

function CheckUserIsEligibeleForMessage() {
	$.ajax({
		type: "POST",
		url: "/umbraco/Surface/StructuredProgram/DownloadEligibility",
		success: function (e) {
			//debugger

			if (e.Result == "0" && e.DownloadedWorksheet == "0") {
				$("#HeaderLogin").show('slow', function () {

				});
			}
			else {
				window.location = window.location.href;
			}
		},
		error: function (error) {

		}
	});
}


function DefaultLogin() {

	try {

		var pageName = $("#page").val();

		commonLayer(pageName, "Default Open");

	}
	catch (ex) {
		//console.log('error');
	}
	$(".loginClose").hide();
	$(".overlayLogin").show();
}

function RegistrationResendOtp() {
	$('.error').hide();

	try {
		commonLayer('Registration', 'ReGenerate OTP');
	}

	catch (ex) {
		//console.log(ex);
	}

	$(".ofrfrm-input").val('');
	//$("#digit-2").val('');
	//$("#digit-3").val('');
	//$("#digit-4").val('');
	$("#loader").show();

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

			$("#loader").hide();

			console.log(error);

		}, complete: function () {

			$("#loader").hide();

		}

	});

}



function LoginForSpecialOffers(mode, downloadUrl) {

	try {

		FetchLoginMessageAjax(9);
		
		CaptureClickedEvent(mode, downloadUrl, '', '', '', '', 'specialoffer', '');

		var pageName = 'SpecialOffer';
		$("#page").val('SpecialOffer');
		if (mode == "print") {
			commonLayer(pageName, "Clicked on Print");
		}
		else if (mode == "dwnld") {
			commonLayer(pageName, "Clicked on Download");
		}
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlayLogin").show();
}

function SpecialOffersDownload(mode, downloadUrl, evnt) {

	
	if (mode == "print") {
		if (evnt == "justlogin") {
			location.href = window.location.href;
		}
		else {
			PrintWorkSheet(downloadUrl);
		}
	}
	else if (mode == "dwnld") {
		if (evnt != null && evnt != '' && evnt != "" && evnt == "justlogin") {
			$("#freeDwnldJustAfterLoginSpecialOffer").show();
		}
		else {
			$("#freeDwnldSpecialOffer").show();
		}

		location.href = downloadUrl;
		
	}
}

function closeWindow() {
	try {
		commonLayer("Login Popup", "Close login popup");
	}
	catch (ex) { //console.log(ex);
	}

	if (window.location.href.indexOf("free-trial") > -1) {
		$(".overlayLogin").hide();
	}
	else if (window.location.href.indexOf("my-account") > -1 || window.location.href.indexOf("lesson") > -1 || window.location.href.indexOf("trial") > -1) {
		window.location.href = "/";
	}
	else {
		$(".overlayLogin").hide();
	}
}

function getParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}
