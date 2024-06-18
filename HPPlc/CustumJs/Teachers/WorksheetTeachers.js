

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: "",
	selectedSubject: "",
	IsCbseContent: "",
	DisplayAgeGroup: 0
};
$.post("/umbraco/Surface/Teachers/GetTeachersList",
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
				printTracker(layerDetail, 'Home');
			}
			catch (ex) {
				//console.log('error');
			}
			PrintWorkSheet(vURLPath);
		});

		$('#worksheetLoading').hide();
	});


//function GetWorkSheetList(selectedAgeGroup, selectedSubject) {

//	var Input = {
//		CultureInfo: $("#hdnCultureInfo").val(),
//		selectedAgeGroup: selectedAgeGroup,
//		selectedSubject: selectedSubject,
//		IsCbseContent: CbseContent,
//		DisplayAgeGroup: row
//	};
//	$.ajax({
//		type: "POST",
//		//"async": true,
//		//"crossDomain": true,
//		//cache: true,
//		dataType: "html",
//		url: "/umbraco/Surface/WorkSheet/GetWorksheetList",
//		data: Input,

//		success: function (responce) {

//			$('#hdTobeDisplayWorksheet').val(row);
//			$("#WorksheetList").append(responce);

//			slicjFunction();
//			share();
//			$(".clsPrintDoc").click(function () {
//				var vURLPath = $(this).find("span").html();

//				try {
//					var layerDetail = $(this).find('span').next('span').html();
//					printTracker(layerDetail, 'Home');
//				}
//				catch (ex) {
//					//console.log('error');
//				}
//				PrintWorkSheet(vURLPath);
//			});

//		},
//		error: function (error) {
//			$("#worksheetLoading").hide();
//		}, complete: function () {
//			$("#worksheetLoading").hide();
//			//console.log($(window).data('ajaxready'));
//			$(window).data('ajaxready', true);
//		}
//	});
//}


$("#filterApply").on('click', function () {
	$('html, body').animate({ scrollTop: $(".ritListingPlcTop").offset().top - 150 }, 500);
	var vrAgeGroupe = '';
	var vrSubjectGroupe = '';

	vrAgeGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
	vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");

	$("#WorksheetList").html("");
	$("#NoResultFound").css('display', 'block');
	$('#worksheetLoading').show();

	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: vrAgeGroupe,
		selectedSubject: vrSubjectGroupe
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/Teachers/GetTeachersList",
		data: JSON.stringify(Input),
		success: function (responce) {

			$("#WorksheetList").html("").append(responce);

			slicjFunction();
			share();
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				//alert(vURLPath);
				try {
					var layerDetail = $(".clsPrintDoc").find('span').next('span').html();
					printTracker(layerDetail, 'Home');
				}
				catch (ex) {
					//console.log('error');
				}

				PrintWorkSheet(vURLPath);
			});
		},
		error: function (error) {
			$("#worksheetLoading").hide();
		}, complete: function () {
			$("#worksheetLoading").hide();
		}
	});
});
