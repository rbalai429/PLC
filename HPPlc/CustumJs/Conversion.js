
$("#buttonsubmit").click(function () {
	TextConvert();
});
function TextConvert() {
	var toconvertext = $("#txtConversionText").val();
	var ConvertionType = $("#ConvertionType").val();
	//alert(ConvertionType);

	$.ajax({
		type: "POST",
		contentType: "application/json",
		//dataType: "JSON",
		url: "/umbraco/Surface/Home/ToBeDecrypt",
		data: JSON.stringify({ "converttext": toconvertext, "convertiontyp": ConvertionType }),
		success: function (e) {
			if (e.status == "Success") {
				$("#spConverted").show();
				$("#spConverted").html(e.message);
				return false;
			}
		},
		error: function (error) {
			
		}, complete: function () {
			
		}
	});
}