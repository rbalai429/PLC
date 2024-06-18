$(document).ready(function () {
	$(".enterEvent").keypress(function (event) {
		if (event.which == 13) {
			$("#PasswordSubmit").click();
			return false;
		}
	});


	$("#SetPassword").on('click', function () {
		$('.error').hide();

		try {
			commonLayer('SetPassword', 'SetPassword Button Click')
		}
		catch (ex) {
			//console.log(ex);
		}

		var password = $('#yourpwdhere').val();
		var cnfmpassword = $('#yourconfirmpwdhere').val();

		if (!password) {
			$('#yourpwdhereReq').show();
			$('#yourpwdhere').focus();
			return false;
		}
		else if (validatePassword(password) == false) {
			$('#yourpwdhereReqFormat').show();
			$('#yourpwdhere').focus();
			return false;
		}
		else if (password !== cnfmpassword) {
			$('#yourconfirmpwdhereReq').show();
			$('#yourconfirmpwdhere').focus();
			return false;
		}
		else {
			$("#loader").css('display', 'block');
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				regpassword: password,
				regpasswordconfirm: cnfmpassword,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				url: "/umbraco/Surface/Home/AddPassword",
				data: Input,
				//contentType: "application/json",
				//dataType: "JSON",
				success: function (e) {
					if (e.status == "Success") {
						//alert(e.navigation);

						try {
							Loginlayer(e.UniqueUserId);
						}
						catch (ex) {
							//console.log(ex);
						}

						window.location = e.navigation;
					}
					else if (e.status == "Fail") {
						$("#otpMsg").show();
						$("#otpMsg").html(e.message);
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



$('#password').keyup(function (e) {
	$('.error').hide();
});

$('#confirmpassword').keyup(function (e) {
	$('.error').hide();
});
