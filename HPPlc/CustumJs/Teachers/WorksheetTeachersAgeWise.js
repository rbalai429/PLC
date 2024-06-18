var currentPage_lazy = 1;
//var vrSubjectGroupe = '';
//var vrTopicsGroupe = '';

//vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
//vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");

var selectedAgeGroup = $(".filterClass:checked").map(function () { return this.value }).get().join(", ");
var filterType = $("#filterType").val();
if (filterType != null && (filterType == "class" || filterType == "list")) {
	selectedAgeGroup = $("#hdnqtype").val();
}

if (selectedAgeGroup == null || selectedAgeGroup == '' || selectedAgeGroup == undefined) {
	selectedAgeGroup = "3-4";
}

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: selectedAgeGroup,
	currentPage: currentPage_lazy
};
$.post("/umbraco/Surface/Teachers/GetTeachersListDetails",
	{
		Input
	},
	function (data, status) {
		
		$("#WorksheetDetails").html("").append(data);
		$(window).data('ajaxready', true);

		var filterType = $("#filterType").val();

		var classSeletedVal = $("#hdnqtype").val();
		
		if ((filterType != '' || filterType != null) && filterType == 'class') {
			$('.classname').filter('[data-val=' + classSeletedVal + ']').addClass('fltrTabActive');
			$('html, body').animate({ scrollTop: $(".fltrTab").offset().top - 150 }, 500);
		}
		else {
			$("input[name='classname'][value='3-4']").prop("checked", true);
		}

		var titleForClass = $("#titleForClass").val();
		var descForClass = $("#descForClass").val();

		$("#titleOfClassName").html(titleForClass);
		$("#descriptionOfClassName").html(descForClass);


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


$(window).data('ajaxready', false).scroll(function (e) {

	
	if ($(window).data('ajaxready') == false) return;

	var faqboxHeight = $('.faqbox-row').height();

	if (faqboxHeight == null || faqboxHeight == 'undefined' || faqboxHeight == '') {
		faqboxHeight = 0;
	}

	var position = $(window).scrollTop();
	var bottom = $(document).height() - $(window).height();
	var footht = faqboxHeight + $('footer').height() + 100;
	

	row = Number($('#hdTobeDisplayWorksheet').val());
	allcount = Number($('#hdTotalNoOfDisplayWorksheet').val());
	rowperpage = Number($('#hdAlreadyDisplayedWorksheet').val());
	//alert(row);

	var top_of_element = $("#pagediv").offset().top;
	var bottom_of_element = $("#pagediv").offset().top + $("#pagediv").height();
	var bottom_of_screen = $(window).scrollTop() + $(window).height();
	var top_of_screen = $(window).scrollTop();


	/*    if (position > (bottom - footht)) {*/

	if ((bottom_of_screen > top_of_element) && (top_of_screen < bottom_of_element)) {

		$(window).data('ajaxready', false);

		//row = row + rowperpage;
		//alert(row);
		//console.log("row:" + row + ":::: allcount:"+ allcount +"::::rowperpage:"+ rowperpage)
		//alert(row);
		
		if (row <= allcount + 1) {
			
			$("#worksheetDetailsLoading").css('display', 'block');
			currentPage_lazy++;
			//alert(currentPage_lazy);
			GetWorkSheetListTeachers(currentPage_lazy);
		}
	}
});


function GetWorkSheetListTeachers(currentPage_lazy) {
	//var selectedAgeGroup = $("#hdnqtype").val();
	
	var selectedAgeGroup = $(".filterClass:checked").map(function () { return this.value }).get().join(", ");
	var filterType = $("#filterType").val();
	if (filterType != null && filterType == "class") {
		selectedAgeGroup = $("#hdnqtype").val();
	}
	
	//console.log(selectedAgeGroup.toString());
	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: selectedAgeGroup,
		currentPage: currentPage_lazy
	};
	$.ajax({
		type: "POST",
		//"async": true,
		//"crossDomain": true,
		//cache: true,
		dataType: "html",
		url: "/umbraco/Surface/Teachers/GetTeachersListDetails",
		data: Input,

		success: function (responce) {

			$('#hdTobeDisplayWorksheet').val(currentPage_lazy);
			$("#WorksheetDetails").append(responce);

			var titleForClass = $("#titleForClass").val();
			var descForClass = $("#descForClass").val();

			$("#titleOfClassName").html(titleForClass);
			$("#descriptionOfClassName").html(descForClass);
			
			
			slicj4Function();
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
			$("#worksheetDetailsLoading").hide();
		}, complete: function () {
			$("#worksheetDetailsLoading").hide();
			//console.log($(window).data('ajaxready'));
			$(window).data('ajaxready', true);
		}
	});
}


$(".filterClass").on('click', function () {
	$('html, body').animate({ scrollTop: $(".fltrTab").offset().top - 150 }, 500);
	
	var vrClassGroupe = $(".filterClass:checked").map(function () { return this.value }).get().join(", ");

	$("#worksheetDetailsLoading").show();
	$("#WorksheetDetails").html("");
	$("#NoResultFound").css('display', 'block');

	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: vrClassGroupe
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/Teachers/GetTeachersListDetails",
		data: JSON.stringify(Input),
		success: function (responce) {
			
			$("#WorksheetDetails").html("").append(responce);

			var titleForClass = $("#titleForClass").val();
			var descForClass = $("#descForClass").val();

			$("#titleOfClassName").html(titleForClass);
			$("#descriptionOfClassName").html(descForClass);

			slicjFunction();
			share();
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				//alert(vURLPath);
				try {
					var layerDetail = $(".clsPrintDoc").find('span').next('span').html();
					printTracker(layerDetail, 'Teacher');
				}
				catch (ex) {
					//console.log('error');
				}

				PrintWorkSheet(vURLPath);
			});
		},
		error: function (error) {
			$("#worksheetDetailsLoading").hide();
		}, complete: function () {
			$("#worksheetDetailsLoading").hide();
		}
	});
});


$('.classSeletedVal').click(function () {
	var classval = $(this).attr("data-val");

	$("#hdnqtype").val(classval);
});
