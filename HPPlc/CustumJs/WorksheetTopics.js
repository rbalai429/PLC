
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedSubject: null,
	selectedTopics: $("#hdnTopic").val(),
	IsCbseContent: 0,
	DisplayCount: 0,
	Mode: 'topics'
};
$.post("/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
	{
		Input
	},
	function (data, status) {
		$("#WorksheetList").html("").append(data);
		
		slicjFunction();
		share();
		$(".clsPrintDoc").click(function () {

			var vURLPath = $(this).find("span").html();

			try {
				var layerDetail = $(this).find('span').next('span').html();
				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);
			}
			catch (ex) {
				//console.log('error');
			}
			PrintWorkSheet(vURLPath);
		});

		$('#worksheetLoading').hide();
	});
