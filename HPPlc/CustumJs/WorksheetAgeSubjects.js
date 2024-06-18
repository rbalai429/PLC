
var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: $("#hdnAgeGroup").val(),
	selectedSubject: $("#hdnSubject").val(),
	IsCbseContent: 0,
	DisplayCount: 0,
	Mode: 'agewisesubject'
};
$.post("/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
	{
		Input
	},
	function (data, status) {
		$("#WorksheetDetails").html("").append(data);
		//$("#WorksheetDetails").html(data);
		share();
		//SelectedParameters();
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
		$("#worksSheetByIdLoadMore").click(function () {
			GetWorkSheetBySubject($("#DisplayCount").val());
		});

		//var bLazy = new Blazy();
		var bLazy = new Blazy({
			selector: '.b-lazy',
			loadInvisible: true,
			offset: 200
		});

		var imagesToLoad = document.querySelectorAll('.b-lazy');
		bLazy.revalidate();
		bLazy.load(imagesToLoad);
	});

//function WorksheetTutorialGetById(displayCount) {

//	var WeekName = "";
//	var vrSubjectsName = $(".subjects:checked").map(function () { return this.value }).get().join(", ");

//	var vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");

//	$("#worksheetDetailsLoading").css('display', 'block');
//	var Input = {
//		CultureInfo: $("#hdnCultureInfo").val(),
//		FilterType: $("#hdnqtype").val(),
//		FilterId: "",
//		selectedSubject: vrSubjectsName,
//		IsCbseContent: vrCBSEContent,
//		DisplayCount: displayCount
//	};
//	$.ajax({
//		type: 'POST',
//		url: "/umbraco/Surface/WorkSheet/GetWorkSheetById",
//		data: JSON.stringify(Input),
//		contentType: "application/json",
//		success: function (data) {
//			$("#WorksheetDetails").html("");
//			$("#WorksheetDetails").html(data);
//			share();
//			SelectedParameters();

//			$(".clsPrintDoc").click(function () {
//				var vURLPath = $(this).find("span").html();

//				try {
//					var layerDetail = $(this).find('span').next('span').html();
//					var pageDetail = $(this).find('span').next('span').next('span').html();

//					printTracker(layerDetail, pageDetail);
//				}
//				catch (ex) {
//					//cpnsole.log(ex);
//				}

//				PrintWorkSheet(vURLPath);
//			});
//			$("#worksSheetByIdLoadMore").click(function () {
//				WorksheetTutorialGetById($("#DisplayCount").val());
//			});

//			//var bLazy = new Blazy();
//			var bLazy = new Blazy({
//				selector: '.b-lazy',
//				loadInvisible: true,
//				offset: 200
//			});

//			var imagesToLoad = document.querySelectorAll('.b-lazy');
//			bLazy.revalidate();
//			bLazy.load(imagesToLoad);
//		}, error: function (error) {
//			$("#worksheetDetailsLoading").css('display', 'none');
//		}, complete: function () {
//			$("#worksheetDetailsLoading").css('display', 'none');
//		}
//	});
//}