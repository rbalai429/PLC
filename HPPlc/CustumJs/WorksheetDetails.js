
var vrSubjectGroupe = '';
var vrTopicsGroupe = '';
var vrCBSEContent = false;

vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	FilterType: $("#hdnqtype").val(),
	selectedSubject: vrSubjectGroupe,
	selectedTopics: vrTopicsGroupe,
	IsCbseContent: vrCBSEContent,
	DisplayCount: 0
};
$.post("/umbraco/Surface/WorkSheet/GetWorkSheetById",
	{
		Input
	},
	function (data, status) {
		
		$("#WorksheetDetails").html("").append(data);
		
		share();
		SelectedParameters();
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
			WorksheetTutorialGetById($("#DisplayCount").val());
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

		$("#worksheetDetailsLoading").hide();
	});

function WorksheetTutorialGetById(displayCount) {
	//alert(displayCount);
	$("#WorksheetDetails").html("");
	$("#worksheetDetailsLoading").show();
	
	var vrSubjectsName = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
	var vrTopicsGroup = $(".topics:checked").map(function () { return this.value }).get().join(", ");
	var vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");

	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		FilterType: $("#hdnqtype").val(),
		selectedSubject: vrSubjectsName,
		selectedTopics: vrTopicsGroup,
		IsCbseContent: vrCBSEContent,
		DisplayCount: displayCount
	};
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/GetWorkSheetById",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (data) {
			
			$("#WorksheetDetails").html(data);
			share();
			SelectedParameters();

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

			//if (vrSubjectsName != null) {
			//	commonFilterLayer("Select Subject Filter", vrSubjectsName);
			//}
			$("#worksSheetByIdLoadMore").click(function () {
				WorksheetTutorialGetById($("#DisplayCount").val());
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
		}, error: function (error) {
			$("#worksheetDetailsLoading").hide();
		}, complete: function () {
			$("#worksheetDetailsLoading").hide();
		}
	});
}

function SelectedParameters() {
	var selectedSubjects = $("#selectedSubjects").val();
	var CbseContentChecked = $("#CbseContentChecked").val();

	//$("input[name='subjects']").each(function () {
	//	if (selectedSubjects != null) {
	//		if (selectedSubjects.indexOf($(this).val()) != -1) {
	//			$(this).prop("checked", true);
	//		}
	//	}
	//});

	if (CbseContentChecked != null && CbseContentChecked != '') {
		$("input[name='CbseContent']").prop("checked", true);
	}
}


