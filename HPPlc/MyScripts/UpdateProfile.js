$(document).ready(function () {

	$("#UpdateProfile").on('click', function () {
		$('.error').hide();
		var name = $('#name').val();
		var mobileno = $('#mobileno').val();
		var agegroup = $('#ageGroup').val();
		var RuParentOrStudent = $("#RuParentOrStudent").val();

		//var supportOnWhatsAppFromHP = $('input[name=supportOnWhatsupFromHP]:radio:checked').length;
		/*var supportOnPhoneFromHP = $('input[name=supportOnPhoneFromHP]:radio:checked').length;*/
		//var supportOnEmailFromHP = $('input[name=supportOnEmailFromHP]:radio:checked').length;
		var supportOnWhatsupFromHP_v = $('input[name=supportOnWhatsupFromHP]:checkbox:checked').val();
		var supportOnPhoneFromHP_v = $('input[name=supportOnPhoneFromHP]:checkbox:checked').val();
		var supportOnEmailFromHP_v = $('input[name=supportOnEmailFromHP]:checkbox:checked').val();

		debugger
		/*var TC = $('input[name=termsChecked]:checkbox:checked').length;*/
		//if (!name) {
		//	$('#nameRequired').show();
		//	$('#name').focus();
		//	return false;
		//}
		
		if (mobileno && validateMobile(mobileno) == false) {
			$('#whatsupFormat').show();
			$('#whatsupnumber').focus();
			return false;
		}

		else if (agegroup == "0") {
			$('#agegroupRequired').show();
			$('#agegroupRequired').focus();
			return false;
		}
		
		else if ((mobileno == "" || mobileno == null) && supportOnWhatsupFromHP_v == "Yes") {
			$('#ConsentWhatsAppMessage').show();
			return false;
		}
		//else if (TC == "0") {
		//	$('#termsChecked').show();
		//	$('#termsChecked').focus();
		//	return false;
		//}
		//else if ((email == "" || email == null) && supportOnEmailFromHP_v == "Yes") {
		//	$('#supportOnEmailFromHP').show();
		//	$('#supportOnEmailFromHP').focus();
		//	return false;
		//}
		//else if (supportOnPhoneFromHP == "0") {
		//	$('#supportOnPhoneFromHP').show();
		//	return false;
		//}
		//else if (supportOnEmailFromHP == "0") {
		//	$('#supportOnEmailFromHP').show();
		//	return false;
		//}
		else {
			$("#loader").css('display', 'block');
			//var data = $("#registrationForm").serialize();

			var token = $('input[name="__RequestVerificationToken"]').val();

			var Input = {
				name: name,
				mobileno: mobileno,
				ageGroup: agegroup,
				supportOnEmailFromHP: supportOnEmailFromHP_v,
				supportOnWhatsupFromHP: supportOnWhatsupFromHP_v,
				supportOnPhoneFromHP: supportOnPhoneFromHP_v,
				RuParentOrStudent: RuParentOrStudent,
				__RequestVerificationToken: token
			};

			$.ajax({
				type: "POST",
				//contentType: "application/json; charset=utf-8",
				//dataType: "JSON",
				url: "/umbraco/Surface/Home/UpdateProfile",
				data: Input,
				success: function (e) {
					if (e.status == "Success") {
						try {
							commonLayer("My Account", "Save button");
						}
						catch (ex) { //console.log(ex);
						}
						$("#Msg").hide();
						//$("#otpPop").hide();
						Swal.fire({
							//title: e.message,
							html: 'Your profile has been successfully updated!!<br> Do you want to go to home page?',
							//icon: 'success',
							showCancelButton: true,
							confirmButtonColor: '#3085d6',
							cancelButtonColor: '#d33',
							confirmButtonText: 'Yes'
						}).then((result) => {
							if (result.isConfirmed) {
								window.location = '/';
							}
							else {
								window.location.reload();
							}
						});

						return false;
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
						$("#otpPop").hide();
						$("#Msg").show();
						$("#Msg").html(e.message);
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

	$("#CancelUpdateProfile").on('click', function () {
		window.location = window.location.pathname;
	});

	GetShareText();
	function GetShareText() {
		$.ajax({
			type: "GET",
			contentType: "application/json; charset=utf-8",
			dataType: "JSON",
			url: "/umbraco/Surface/Home/GetShareText?ReferralCode=" + $("#ReferralCodeText").val(),
			success: function (result) {
				if (result.responce.StatusCode == 200) {
					$("#sharebutton").html(result.responce.Result);
					share();
				}
			},
			error: function (error) {

			}
		});
	}
});

//function share() {

//	$(".aFBShare").click(function () {
//		var ItemVal = $(this).find('span').html();
//		publish(ItemVal);
//	});
//	$(".aWHTAppSH").click(function () {
//		var ItemVal = $(this).find('span').html();
//		if (/Mobi/.test(navigator.userAgent)) {
//			window.open('whatsapp://send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
//		}
//		else {
//			window.open('https://web.whatsapp.com/send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
//		}
//	});
//	$(".aMailSh").click(function () {
//		var FullString = $(this).find('span').html();
//		var emailBody = FullString.split('`')[0];
//		var email = '';
//		var subject = FullString.split('`')[1];
//		window.location = 'mailto:' + email + '?body=' + encodeURIComponent(emailBody) + '&subject=' + subject;
//	});
//}


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



$("input[type='radio'][name='supportOnWhatsupFromHP']").click(function () {
	$("#learningMaterialFromHP").hide();
});

//$("input[type='radio'][name='supportOnPhoneFromHP']").click(function () {
//	$("#supportOnPhoneFromHP").hide();
//});

$("input[type='radio'][name='supportOnEmailFromHP']").click(function () {
	$("#supportOnEmailFromHP").hide();
});


$("#ageGroup").change(function () {
	if ($('#ageGroup').val() != "0" || $('#ageGroup').val() != "" || $('#ageGroup').val() != null)
		$("#agegroupRequired").hide();
});


$('#mobileno').keyup(function () {
	if (this.value.match(/[^0-9]/g) && this.value.length == 10) {
		$('#whatsupFormat').hide();
	}
	else {
		$('#whatsupFormat').show();
	}
});
