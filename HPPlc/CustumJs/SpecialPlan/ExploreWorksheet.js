
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	DisplayCount: 0
};
$.post("/umbraco/Surface/SpecialPlan/GetExploreWorkSheetOfSpecialPlans",
	{
		Input
	},
	function (data, status) {
		$("#SpecialPlanWorksheet").html("").append(data);

		slicjFunction();

		$('#worksheetDetailsLoading').hide();
	});
