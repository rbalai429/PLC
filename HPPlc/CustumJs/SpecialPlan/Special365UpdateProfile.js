$(document).ready(function () {

	$("#alternatenumberadd").on('click', function () {

		try {
			commonLayer('learn-365', "Click on add alternate mobile no.");
		}
		catch (ex) {
			//console.log('error');
		}
		$(".overlayLogin").show();
	});


	$("#AddAlternateNoNext").on('click', function () {

		try {
			commonLayer('learn-365', "Alternate no. Add Next Button");
		}
		catch (ex) { //console.log(ex);
		}
		debugger
		$('.error').hide();
		var alternatemobileno = $('#newalternatemobileno').val();
		if (validateMobile(alternatemobileno) == false) {
			$('#cMobileNoRequired').show();
			$('#cMobileNoRequired').focus();
			return false;
		}
		else {
			var token = $('input[name="__RequestVerificationToken"]').val();

			$.ajax({
				//async: false,
				type: "POST",
				url: "/umbraco/Surface/Home/AddAlternateNumberSpcialPlan",
				data: { __RequestVerificationToken: token, "alternatemobileno": alternatemobileno, "Source": "Plan365l" },
				//contentType: "application/json; charset=utf-8",
				//dataType: "JSON",
				success: function (e) {
					debugger
					if (e.Status == "SUCCESS" || e.Status == "Success") {

						$("#SP365Login").hide();
						$("#SP365Otp").show();

						ClearOtp();

						try {
							//commonLayer('Login', 'Generate OTP Registration');
							commonLayer('learn-365', 'Generate OTP - Add Alternate No.');
						}
						catch (ex) {
							//console.log(ex);
						}

						var maskingMobileCmsText = $('#maskingMobile').val();

						try {
							if (e.mobmasking != '' && e.mobmasking != "" && e.mobmasking != null) {
								$('#MaskingMessage').html(maskingMobileCmsText.toString().replace('{mobile}', e.mobmasking));
							}
						}
						catch {
							//console.log();
						}

						countdown();
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


	$(".otpspecialplanalternatemobilefield").keyup(function () {

		$('.error').hide();

		if (this.value.length == this.maxLength) {
			$(this).next('.otpspecialplanalternatemobilefield').focus();
		}
		else {
			$(this).prev('.otpspecialplanalternatemobilefield').focus();
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
						debugger
						if (e.Status == "SUCCESS") {
							try {
								commonLayer('learn-365', 'Alternate Mobile no. OTP Successfull');

							} catch (ex) {
								//console.log(ex);
							}

							$('.overlayLogin').hide();
							$('.alternatemobile').show();
							$('.addalternatemobile').hide();

							var altMob = $('#newalternatemobileno').val();
							$('#alternatemobileno').val(altMob);

							$('#alternatemobileno').attr('disabled', 'disabled');
						}
						else if (e.Status == "OTP_NM") {
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

	$("#UpdateProfile").on('click', function () {
		$('.error').hide();

		var name = $('#name').val();
		var alternatemobileno = $('#alternatemobileno').val();

		if (!name) {
			$('#nameRequired').show();
			$('#name').focus();
			return false;
		}
		else if (alternatemobileno && validateMobile(alternatemobileno) == false) {
			$('#alternateNoFormat').show();
			$('#alternatemobileno').focus();
			return false;
		}
		else {
			$("#loader").css('display', 'block');

			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				name: name,
				alternatemobileno: alternatemobileno,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json; charset=utf-8",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/UpdateProfile_SpecialPlan",
				data: Input,
				success: function (e) {
					if (e.status == "Success") {
						try {
							commonLayer("365Plan - Update profile", "Save button");
						}
						catch (ex) { //console.log(ex);
						}

						$("#message").show();
						$("#message").html("Profile has been updated.");
						$("#UpdateProfile").hide();

						return false;
					}
					else if (e.status === "altmexts") {
						$("#message").show();
						$("#message").html("Alternate mobile no. not in correct format.");
						return false;
					}
					else if (e.status === "Fail") {
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
	});
});


$(".loginClose").click('click', function () {
	try {
		commonLayer('learn-365', "Add Alternate Number Close on 365 days Plan");
	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlayLogin").hide();
});
