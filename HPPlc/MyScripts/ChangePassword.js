$(document).ready(function () {
	$("#changePassword").on('click', function (e) {
		
		$('.error').hide();

		try {
			var txtusercode = $('#txtusercode').val();
			TrackWithUserId('ChangePassword','Changepassword', txtusercode);
		}
		catch (ex) {
			//console.log(ex);
		}

		var currentpassword = $('#currentpassword').val().trim();
		var newpassword = $('#newpassword').val().trim();
		var confirmnewpassword = $('#confirmnewpassword').val().trim();
		
		if (currentpassword == '' || currentpassword == null) {
			$('#currentpasswordRequired').show();
			$('#currentpassword').focus();
			return false;
		}

		else if (newpassword == '' || newpassword == null) {
			$('#passwordRequired').show();
			$('#newpassword').focus();
			return false;
		}

		//else if (newpassword.length < 3 || newpassword.length > 15) {
		//	$('#passwordLengthRequired').show();
		//	$('#newpassword').focus();
		//	return false;
		//}
			
		else if (validatePassword(newpassword) == false) {
			$('#passwordValidate').show();
			$('#newpassword').focus();
			return false;
		}
		else if (confirmnewpassword == '' || confirmnewpassword == null) {
			$('#confirmpasswordRequired').show();
			$('#confirmnewpassword').focus();

			return false;
		}
		else if (newpassword !== "" && confirmnewpassword !== "" && (newpassword !== confirmnewpassword)) {

			$('#confirmpasswordValidate').show();
			$('#confirmnewpassword').focus();

			return false;
		}
		else {
			$("#loader").css('display', 'block');
			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				CurrentPassword: currentpassword,
				NewPassword: newpassword,
				ConfirmNewPassword: confirmnewpassword,
				__RequestVerificationToken: token
			};


			$.ajax({
				type: "POST",
				//contentType: "application/json",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/ChangePassword",
				data: Input,
				success: function (e) {
					if (e.status == "Success") {
						$("#msgBox").show();
						$("#msgBox").html(e.message);

						$('#currentpassword').val('');
						$('#newpassword').val('');
						$('#confirmnewpassword').val('');

						return false;
					}
					else if (e.status == "Fail") {
						$("#msgBox").show();
						$("#msgBox").html(e.message);
						return false;
					}
					else if (e.status == "Error") {
						$("#msgBox").show();
						$("#msgBox").html("Password do not change.");

						$('#currentpassword').val('');
						$('#newpassword').val('');
						$('#confirmnewpassword').val('');

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




$('#currentpassword').keyup(function (e) {
	$('.error').hide();
});

$('#newpassword').keyup(function (e) {
	$('.error').hide();
});

$('#confirmnewpassword').keyup(function (e) {
	$('.error').hide();
});
