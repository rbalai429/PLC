$(document).ready(function () {
	$('.doneBtn').click(function () {
		$(this).parents(".chchlist").removeClass("listFltr");
	});
});

$("#filterApply").click(function () {
	var pageName = $("#pageName").val();
	var CurrentHomePage = $("#IsCurrentHomePage").val();
	var vrAgrGroupe = '';
	var vrSubjectGroupe = '';
	var vrTopicsGroupe = '';
	var vrCBSEContent = false;

	var allcount = Number($('#hdTotalNoOfDisplayWorksheet').val());
	$('#hdTobeDisplayWorksheet').val(allcount);

	//if ($("selector").hasClass("classes")) {
	//	vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
	//}
	vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");

	vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");

	vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
	
	vrCBSEContent = $(".CbseContent:checked").map(function () { return this.value }).get().join(", ");

	try {
		if (vrAgrGroupe != null || vrAgrGroupe != '') {
			var vrAgrGroupesTitle = $(".classes:checked").map(function () { return this.title }).get().join(", ");
			
			commonFilterLayer("Select Class Filter - " + pageName, vrAgrGroupesTitle);
		}

		if (vrSubjectGroupe != null || vrSubjectGroupe != '') {
			var vrSubjectGroupeTitle = $(".subjects:checked").map(function () { return this.title }).get().join(", ");

			commonFilterLayer("Select Subject Filter - " + pageName, vrSubjectGroupeTitle);
		}
	}
	catch (ex) { //console.log(ex);
	}

	if ((vrAgrGroupe === null || vrAgrGroupe === '') && (vrSubjectGroupe === null || vrSubjectGroupe === '') && (vrCBSEContent === null || vrCBSEContent === '') && (vrTopicsGroupe === '' || vrTopicsGroupe === null)) {
		$('#validation').show();
	}
	else {
		$('#validation').hide();
		
		if (pageName !== null && (pageName.toLowerCase() === "home" || pageName.toLowerCase() === "worksheets") || (pageName.toLowerCase() === "lesson-plan") || (CurrentHomePage !== null && CurrentHomePage === "yes")) {
			
			GetWorkSheetListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrCBSEContent);
		}
		else {
			WorksheetTutorialGetById(0);
		}

		$('html, body').animate({ scrollTop: $('.ritListingPlc').offset().top - 100 }, 800);

		if ($(window).width() < 1199) {
			/*$(".filtrActn .btn").click(function () {*/
			$(".leftFltr").removeClass("slideFltr");
			/*});*/
		}
		
	}
});
