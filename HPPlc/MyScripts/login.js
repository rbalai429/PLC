$(document).ready(function (e) {
	GetResponseFromHPId();
	if ($('#PreLoginEmail').length){
		$(document).keypress(function(event){
			var keycode = (event.keyCode ? event.keyCode : event.which);
			if (keycode == '13') {
				try {
					commonLayer("login", "Next Button");
				}
				catch (ex) { //console.log(ex);
				}
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
	// $("#PreLoginEmail").keypress(function (event) {
	// 	if (event.which == 13) {
	// 		$("#PreSignInBtn").click();
	// 		return false;
	// 	}
	// });

	// $(".enterEvent").keypress(function (event) {
	// 	if (event.which == 13) {
	// 		$("#SignInBtn").click();
	// 		return false;
	// 	}
	// });

	$('#dvPreLogin').show();
	$('#dvLogin').hide();
	
	
});



$('#UserId').keyup(function (e) {
	$('#UserIdRequired').hide();
	$('#Msg').hide();
});

$('#PreLoginEmail').keydown(function (e) {
	$('#UserNameRequired').hide();
	$('#preloginmessage').hide();
});


$('#UserPassword').keydown(function (e) {
	$('#UserPasswordRequired').hide();
	$('#UserIdRequired').hide();
	$('#Msg').hide();
});









