
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	Mode :"mydownloads",
	DisplayCount: 0
};
$.post("/umbraco/Surface/SpecialPlan/GetMyDownloads",
	{
		Input
	},
	function (data, status) {
		debugger
		$("#SpecialPlanWorksheet").html("").append(data);

		slicjFunction();

		$('#worksheetDetailsLoading').hide();

		$(".clsPrintDoc").click(function () {
		
			var vURLPath = $(this).find("span").html();

			try {
				var layerDetail = $(this).find('span').next('span').html();
				printTracker(layerDetail, 'My Downloads');
			}
			catch (ex) {
				//console.log('error');
			}
			PrintWorkSheet(vURLPath);
		});
	});
