$(document).ready(function () {

	var title;
	var message;
	var culture = $('#Culture').val();

	var payStatus = $("#payStatus").val().toUpperCase();

	//alert(payStatus);
	if (payStatus == "SUCCESS") {
		title = $("#SuccessTitle").val();
		message = $("#SuccessMessage").val();
	}
	else if (payStatus == "FAIL") {
		title = $("#FailedTitle").val();
		message = $("#FailedMessage").val();
	}
	else if (payStatus == "FAILURE") {
		title = $("#FailedTitle").val();
		message = $("#FailedMessage").val();
	}
	else if (payStatus == "ABORTED") {
		title = $("#AbortedTitle").val();
		message = $("#AbortedMessage").val();
	}
	else if (payStatus == "AWAITED") {
		title = $("#AwaitedTitle").val();
		message = $("#AwaitedMessage").val();
	}
	else if (payStatus == "PENDING") {
		title = $("#PendingTitle").val();
		message = $("#PendingMessage").val();
	}
	else if (payStatus == "ERROR") {
		title = $("#ErrorTitle").val();
		message = $("#ErrorMessage").val();
	}
	else {
		title = 'Payment Fail';
		message = 'Payment Failed Due to Some Technical Issue';
	}

	$("#PayResponseTitle").html(title);
	$("#PayResponseMessage").html(message);
});