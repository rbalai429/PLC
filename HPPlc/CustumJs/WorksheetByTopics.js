
//var row = $('#hdAlreadyDisplayedWorksheet').val();
//alert(row);
//var allcount = '';
//var rowperpage = '';

//var timeoutId;

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: '',
	selectedSubject: '',
	IsCbseContent: 0,
	DisplayCount: 0,
	Mode: 'bytopics'
};
$.post("/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
	{
		Input
	},
	function (data, status) {
		//alert("Data: " + data + "\nStatus: " + status);
		//$('#hdTobeDisplayWorksheet').val(row);
		$("#WorksheetList").html("").append(data);
		$(window).data('ajaxready', true);
		$("#worksheetLoading").css('display', 'none');
		
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
	});


//$(window).data('ajaxready', false).scroll(function (e) {
	

//	if ($(window).data('ajaxready') == false) return;

//	var faqboxHeight = $('.faqbox-row').height();

//	if (faqboxHeight == null || faqboxHeight == 'undefined' || faqboxHeight == '') {
//		faqboxHeight = 0;
//	}

//	var position = $(window).scrollTop();
//	var bottom = $(document).height() - $(window).height();
//	var footht = faqboxHeight + $('footer').height() + 100;
//	//console.log(position +"::::"+bottom+ ':::::'+(bottom-footht));


//	row = Number($('#hdTobeDisplayWorksheet').val());
//	allcount = Number($('#hdTotalNoOfDisplayWorksheet').val());
//	rowperpage = Number($('#hdAlreadyDisplayedWorksheet').val());
//	//alert(row);


//	if (position > (bottom - footht)) {

//		row = row + rowperpage;

//		//console.log("row:" + row + ":::: allcount:"+ allcount +"::::rowperpage:"+ rowperpage)

//		if (row <= allcount + 1) {
//			$(window).data('ajaxready', false);


//			$("#worksheetLoading").css('display', 'block');

//			WorksheetTutorialGetById(0);

//		}
//	}

//});

function WorksheetTutorialGetById(displayCount) {

	var vrSubjectGroupe = '';
	var vrAgeGroup = $(".classes:checked").map(function () { return this.value }).get().join(", ");

	vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");

	var vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");

	var vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");
	
	$("#WorksheetList").html("");
	$("#worksheetLoading").show();

	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: vrAgeGroup,
		selectedSubject: vrSubjectGroupe,
		selectedTopics: vrTopicsGroupe,
		IsCbseContent: vrCBSEContent,
		DisplayCount: 0,
		Mode: 'bytopics'
	};
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/GetWorkSheetBySubject",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (data) {
			$('#hdTobeDisplayWorksheet').val(row);
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

			//if (vrAgeGroup != null) {
			//	commonFilterLayer("Select Age Filter", vrAgeGroup);
			//}
			//if (vrSubjectGroupe != null) {
			//	commonFilterLayer("Select Subject Filter", vrSubjectGroupe);
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
			$(window).data('ajaxready', true);
		}
	});
}