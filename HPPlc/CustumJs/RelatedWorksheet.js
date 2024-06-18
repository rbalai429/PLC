var InputWorksheet = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: $("#hdnAgeGroup").val(),
	selectedSubject: $("#hdnSubject").val(),
	worksheetId: $("#hdnWorksheet").val(),
	Mode: 'single'
};
$.post("/umbraco/Surface/WorkSheet/GetSingleWorkSheet",
	{
		InputWorksheet
	},
	function (data, status) {

		$("#WorksheetSingle").html("").append(data);

		//slicj4Function();
		share();
		$(".clsPrintDoc").click(function () {
			var vURLPath = $(this).find("span").html();

			try {
				var layerDetail = $(this).find('span').next('span').html();
				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);
			}
			catch (ex) {
				//cpnsole.log(ex);
			}

			PrintWorkSheet(vURLPath);

		});

		
	});


var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: $("#hdnAgeGroup").val(),
	selectedSubject: $("#hdnSubject").val(),
	worksheetId: $("#hdnWorksheet").val(),
	Mode: 'related'
};
$.post("/umbraco/Surface/WorkSheet/GetRelatedWorkSheetDetails",
	{
		Input
	},
	function (data, status) {

		$("#WorksheetList").html("").append(data);

		slicj4Function();
		share();
		$(".clsPrintDoc").click(function () {
			var vURLPath = $(this).find("span").html();

			try {
				var layerDetail = $(this).find('span').next('span').html();
				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);
			}
			catch (ex) {
				//cpnsole.log(ex);
			}

			PrintWorkSheet(vURLPath);

		});
		
		$("#worksheetDetailsLoading").hide();
	});