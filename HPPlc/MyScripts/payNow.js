$(document).ready(function () {

	//var isOpenPopup = $("#poupOpen").val();
	//var loggedIn = $("#loggedIn").val();
	//var NoOfSubscribed = $("#NoOfSubscribed").val();
});

function paynow() {
	
	try {
		commonLayer('BuyNow', 'Pay Now');
	}
	catch (ex) {
		//console.log('');
	}

	$("#loader").css('display', 'block');

	var cultureSessionName = $("#cultureSessionName").val();
	var couponCode = $("#couponCode").val();


	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Subscription/PayNow",
		data: { "CouponCode": couponCode },
		success: function (e) {
			if (e.status == "Success") {

				if (e.message == 'lesson') {
					window.location = cultureSessionName + "/lesson-plan";
				}
				else {
					//alert('pay');
					$('#paymentForm').attr('action', e.message);
					document.getElementById('paymentForm').submit();
				}
			}
			else if (e.status == "Validate") {
				$('#spValidationMsg').show();
			}
			else if (e.status == "Fail") {
				$("#msg_pay").show();
				$("#msg_pay").html(e.message);
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


//$("#paynow").click(function () {
//	alert('Hi');
//	var cultureSessionName = $("#cultureSessionName").val();
//	var couponCode = $("#couponCode").val();
//	//var ageGroup = $("input:radio[name='ageGroup']:checked").val();

//	//if (ageGroup == null || ageGroup == "" || ageGroup == "undefined") {
//	//	$('#spValidationMsg').show();
//	//	return false;
//	//}
//	//else {
//		$.ajax({
//			type: "GET",
//			contentType: "application/json; charset=utf-8",
//			dataType: "JSON",
//			url: "/umbraco/Surface/Home/PayNow",
//			data: { "agegroup": ageGroup, "CouponCode": couponCode },
//			success: function (e) {
//				if (e.status == "Success") {
//					if (e.message == 'home') {
//						window.location = cultureSessionName + "/";
//					}
//					else {
//						$('#paymentForm').attr('action', e.message);
//						document.getElementById('paymentForm').submit();
//					}
//				}
//				else if (e.status == "Validate") {
//					$('#spValidationMsg').show();
//				}
//				else if (e.status == "Fail") {
//					$("#msg_pay").show();
//					$("#msg_pay").html(e.message);
//					return false;
//				}
//			},
//			error: function (error) {

//			}
//		});
//	//}
//});

$('#couponCode').keypress(function (e) {
	var regex = new RegExp("^[a-zA-Z0-9]+$");
	var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
	if (regex.test(str)) {
		return true;
	} else {
		e.preventDefault();
		return false;
	}
});

function changeAgeGroup(ageGroup) {
	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Home/AgeGroupSelection",
		data: { "ageGroup": ageGroup },
		success: function (status) {
			if (status == 'ok') {
				window.location.reload();
			}
		},
		error: function (error) {

		}
	});
}

function changeVariant(languageisoUrl, isoCode, languageName) {
	$.ajax({
		url: '/umbraco/Surface/Home/LanguageStoreInSession',
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		data: { langName: languageName, cultureName: isoCode },
		success: function (data) {
			try {
				commonLayer('Change Language', languageName);
			}
			catch (ex) {
				//console.log('error');
			}

			window.location = languageisoUrl;
		}
	});
}

$('input[type=radio][name=ageGroup]').change(function () {
	$('#spValidationMsg').hide();
});
