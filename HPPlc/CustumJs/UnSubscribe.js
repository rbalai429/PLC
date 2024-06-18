$("#btnUnSubscribe").on('click', function () {
	$('.error').hide();
	
	var UnSubscribeOther = $('#txtUnSubscribeOther').val();
	var UnSubOption = $('input[name=UnSubOption]:radio:checked').length;
	var UnSubOptionVal = $('input[name=UnSubOption]:radio:checked').val();
	
	//var CheckboxOptions =
	//	document.getElementsByName('UnSubOption');
	//var options = "";
	//for (var i = 0; i < CheckboxOptions.length; i++) {
	//	if (CheckboxOptions[i].checked) {
	//		options += CheckboxOptions[i].value + ",";
	//	}
	//}
	
	if (UnSubOption == 0) {
		$('#errorMsg').show();
	}
	else if (UnSubOptionVal == "Others (please specify)" && (UnSubscribeOther == "" || UnSubscribeOther == null)) {
		$('#errorMsg').show();
	}
	else {
		$("#loader").css('display', 'block');
		var token = $('input[name="__RequestVerificationToken"]').val();
		var downldpm = window.location.href;

		var Input = {
			OtherContent: UnSubscribeOther,
			UnsubscribeOpt: UnSubOptionVal,
			CurrentUrl: downldpm,
			__RequestVerificationToken: token
		};

		$.ajax({
			type: "POST",
			//contentType: "application/json",
			//dataType: "JSON",
			url: "/umbraco/Surface/Subscription/UnSubscribe",
			data: Input,
			success: function (e) {
				
				if (e.returnStatus == "Success") {
					$("#loader").css('display', 'none');
					$('#error').hide();
					$('#unsubscribeTab').hide();
					$('#unsubscribeMsgTab').show();
					try {

						commonLayer('Unsubscribe', 'User has been un-subscribed');

					} catch (ex) {
						//console.log(ex);
					}
				}
				else if (e.returnStatus == "Fail") {
					$("#loader").css('display', 'none');
					$('.error').hide();
					$('#Msg').show();
					$('#Msg').html(e.returnMessage);
				}
				else if (e.returnStatus == "vald") {
					$("#loader").css('display', 'none');
					$('.error').hide();
					$('#Msg').show();
					$('#Msg').html(e.returnMessage);
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