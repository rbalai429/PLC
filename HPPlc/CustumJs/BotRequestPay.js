$(document).ready(function () {
	var Input = {
		email: $("#email").val(),
		pay: $("#pay").val()
	};

	$("#loader").css('display', 'block');
	$.ajax({
		type: "POST",
		dataType: "JSON",
		url: "/umbraco/surface/Bot/GetBotSubscriptionRequest",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify(Input),
		success: function (e) {
			location.href = e.navigation;
		},
		error: function (error) {
			//alert(error.responseText);
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
});