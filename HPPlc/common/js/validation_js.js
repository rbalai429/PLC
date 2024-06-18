$(document).ready(function () {
	$("#SignInBtn").on('click', function () {
		var name = $('#name').val().trim();
        var email = $('#email').val().trim();
		var mobile = $('#mobile').val().trim();
		var whatsupnumber = $('#whatsupnumber').val().trim();

		var supportOnEmailFromHP = $('input[name=supportOnEmailFromHP]:radio:checked').length;
		var supportOnWhatsupFromHP = $('input[name=supportOnWhatsupFromHP]:radio:checked').length;
		var supportOnPhoneFromHP = $('input[name=supportOnPhoneFromHP]:radio:checked').length;

		var termsChecked = $('input[name=termsChecked]:checkbox:checked').length;
		
		//$(".error").remove();
		if (name == "") {
			$('#nameRequired').show();
			$('#name').focus();
			//$('.error').show();
		}

        else if (email == "") {
            //$('<span class="error">Please enter email</span>').insertAfter($("#email"));
			$('#emailRequired').show();
            $('#email').focus();
            //$('.error').show();
        }
        else if (validateEmail(email) == false) {
			$('#emailFormat').show();
            $('#email').focus();
            //$('.error').show();
		}

		else if (mobile == "") {
			$('#mobileRequired').show();
			$('#mobile').focus();
            //$('.error').show();
		}
		else if (mobile == "0000000000") {
			$('#mobileFormat').show();
			$('#mobile').focus();
			//$('.error').show();
		}
		else if (mobile.length != 10) {
			$('#mobileFormat').show();
			$('#mobile').focus();
			//$('.error').show();
		}

		else if (whatsupnumber != "" && (whatsupnumber == "0000000000" || whatsupnumber.length != 10)) {
			$('#whatsupFormat').show();
			$('#whatsupnumber').focus();
			//$('.error').show();
		}

		else if (supportOnEmailFromHP == "0") {
			//alert('email');
			$('#supportOnEmail').show();
			//$('#supportOnEmail').focus();
			//$('.error').show();
		}

		else if (supportOnWhatsupFromHP == "0") {
			$('#learningMaterialFromHP').show();
			$('#learningMaterialFromHP').focus();
			//$('.error').show();
		}

		else if (supportOnPhoneFromHP == "0") {
			$('#supportOnPhoneFromHP').show();
			$('#supportOnPhoneFromHP').focus();
			//$('.error').show();
		}

		else if (termsChecked == "0") {
			$('#termsChecked').show();
			$('#termsChecked').focus();
			//$('.error').show();
		}
		
    });
});
function validateEmail(email) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
    if (!filter.test(email))
        return false;
    else
        return true;
}

$('#mobile').keyup(function () {
	if (this.value.match(/[^0-9]/g)) {
		this.value = this.value.replace(/[^0-9]/g, '');
	}
	if ($('#mobile').val().length >= 10) {

		$('#mobile').val($('#mobile').val().slice(0, 10));
	}
});

$('#whatsupnumber').keyup(function () {
	if (this.value.match(/[^0-9]/g)) {
		this.value = this.value.replace(/[^0-9]/g, '');
	}
	if ($('#whatsupnumber').val().length >= 10) {

		$('#whatsupnumber').val($('#whatsupnumber').val().slice(0, 10));
	}
});

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

$('#mobile').keyup(function (e) {
	$('#mobileRequired').hide();
	$('#mobileFormat').hide();
});

$('#whatsupnumber').keyup(function (e) {
	$('#whatsupFormat').hide();
});

$("input[type='radio'][name='supportOnEmailFromHP']").click(function () {
	$("#supportOnEmail").hide();
});

$("input[type='radio'][name='supportOnWhatsupFromHP']").click(function () {
	$("#learningMaterialFromHP").hide();
});

$("input[type='radio'][name='supportOnPhoneFromHP']").click(function () {
	$("#supportOnPhoneFromHP").hide();
});

$("input[type='checkbox'][name='termsChecked']").click(function () {
	$("#termsChecked").hide();
});

