
var row = $('#hdAlreadyDisplayedWorksheet').val();

var allcount = '';
var rowperpage = '';

var timeoutId;


var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: "",
	selectedSubject: "",
	IsCbseContent: "",
	DisplayAgeGroup: row
};
$.post("/umbraco/Surface/WorkSheet/GetWorksheetList",
	{
		Input
	},
	function (data, status) {
		//alert("Data: " + data + "\nStatus: " + status);
		$('#hdTobeDisplayWorksheet').val(row);
		$("#WorksheetList").html("").append(data);
		$(window).data('ajaxready', true);

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


$(window).data('ajaxready', false).scroll(function (e) {


	if ($(window).data('ajaxready') == false) return;

	var faqboxHeight = $('.faqbox-row').height();

	if (faqboxHeight == null || faqboxHeight == 'undefined' || faqboxHeight == '') {
		faqboxHeight = 0;
	}

	var position = $(window).scrollTop();
	var bottom = $(document).height() - $(window).height();
	var footht = faqboxHeight + $('footer').height() + 100;
	//console.log(position +"::::"+bottom+ ':::::'+(bottom-footht));


	row = Number($('#hdTobeDisplayWorksheet').val());
	allcount = Number($('#hdTotalNoOfDisplayWorksheet').val());
	rowperpage = Number($('#hdAlreadyDisplayedWorksheet').val());
	//alert(row);


	if (position > (bottom - footht)) {



		row = row + rowperpage;

		//console.log("row:" + row + ":::: allcount:"+ allcount +"::::rowperpage:"+ rowperpage)

		if (row <= allcount + 1) {
			$(window).data('ajaxready', false);


			$("#worksheetLoading").css('display', 'block');

			GetWorkSheetList("", "", "");
		}
	}
});

function GetWorkSheetList(selectedAgeGroup, selectedSubject, CbseContent) {

	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: selectedAgeGroup,
		selectedSubject: selectedSubject,
		IsCbseContent: CbseContent,
		DisplayAgeGroup: row
	};
	$.ajax({
		type: "POST",
		//"async": true,
		//"crossDomain": true,
		//cache: true,
		dataType: "html",
		url: "/umbraco/Surface/WorkSheet/GetWorksheetList",
		data: Input,

		success: function (responce) {

			$('#hdTobeDisplayWorksheet').val(row);
			$("#WorksheetList").append(responce);

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

		},
		error: function (error) {
			$("#worksheetLoading").hide();
		}, complete: function () {
			$("#worksheetLoading").hide();
			//console.log($(window).data('ajaxready'));
			$(window).data('ajaxready', true);
		}
	});
}

function GetWorkSheetListFilter(selectedAgeGroup, selectedSubject, vrTopicsGroupe, CbseContent) {
	$("#WorksheetList").html("");
	$("#NoResultFound").css('display', 'block');
	$('#worksheetLoading').show();
	//console.log("row:" + row);
	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: selectedAgeGroup,
		selectedSubject: selectedSubject,
		selectedTopics: vrTopicsGroupe,
		IsCbseContent: CbseContent,
		DisplayAgeGroup: 0
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/WorkSheet/GetWorksheetList",
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
}
