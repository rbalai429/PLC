
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: '',
	selectedSubject: $("#hdnSubject").val(),
	IsCbseContent: 0,
	DisplayCount: 0,
	Mode: 'subject'
};
$.post("/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
	{
		Input
	},
	function (data, status) {
		//alert("Data: " + data + "\nStatus: " + status);
		//$('#hdTobeDisplayWorksheet').val(row);
		$("#WorksheetList").html("").append(data);
		//$(window).data('ajaxready', true);

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

function WorksheetTutorialGetById(displayCount) {
	$('#worksheetLoading').show();
	$("#WorksheetList").html("");
	
	var vrAgeGroup = $(".classes:checked").map(function () { return this.value }).get().join(", ");

	var vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");

	
	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: vrAgeGroup,
		selectedSubject: $("#hdnSubject").val(),
		IsCbseContent: vrCBSEContent,
		DisplayCount: 0,
		Mode: 'subject'
	};
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (data) {
			
			$("#WorksheetList").html(data);
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

			//if (vrAgeGroup != null) {
			//	commonFilterLayer("Select Age Filter", vrAgeGroup);
			//}
			
			//var bLazy = new Blazy();
			var bLazy = new Blazy({
				selector: '.b-lazy',
				loadInvisible: true,
				offset: 200
			});

			var imagesToLoad = document.querySelectorAll('.b-lazy');
			bLazy.revalidate();
			bLazy.load(imagesToLoad);
		}, error: function (error) {
			$("#worksheetLoading").hide();
		}, complete: function () {
			$("#worksheetLoading").hide();
		}
	});
}