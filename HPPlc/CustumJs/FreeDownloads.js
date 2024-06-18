

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	FilterType: $("#hdnNodeChild").val(),
	FilterId: $("#hdnNodeIdParent").val(),
	worksheetId: $("#hdnNodeIdChild").val(),
	DisplayCount: 0
};
$.post("/umbraco/Surface/FreeDownloads/GetFestivalWorksheetList",
	{
		Input
	},
	function (responce, status) {
		
		if (status == 'success') {
			$("#videoLoading").css('display', 'none');
		}
	}).done(function (responce) {
		//$('#test').empty().append('PLCHP');
		//document.getElementById("test").innerHTML = "PLCHP";
		festivalOfferInternalData(responce);
	});



function GetFestivalWorkSheetList(displayCount) {
	//var path = location.pathname.length ? location.pathname : window.location.href;
	//var param = window.location.href.slice(window.location.href.indexOf('?') + 1);
	//alert(param);
	//alert(param.slice(param.indexOf('#')));
	//if (param != null || param != '') {
	//	param = param[i];
	//}
	//alert(displayCount);
	$("#festival-offers-worksheetLoading").css('display', 'block');
	//$("#NoResultFound").css('display', 'none');
	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		FilterType: $("#hdnNodeChild").val(),
		FilterId: $("#hdnNodeIdParent").val(),
		worksheetId: $("#hdnNodeIdChild").val(),
		DisplayCount: displayCount
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/FreeDownloads/GetFestivalWorksheetList",
		data: JSON.stringify(Input),
		success: function (responce) {
			festivalOfferInternalData(responce);
		},
		error: function (error) {
			$("#festival-offers-worksheetLoading").css('display', 'none');
		}, complete: function () {
			$("#festival-offers-worksheetLoading").css('display', 'none');
		}
	});
}
//function PrintWorkSheet(vPath) {
//	var vTemp = vPath.split('$');
//	var vCInfo = vTemp[0];
//	var vUserId = vTemp[1];
//	var AgeTitleDesc = vTemp[2];
//	var WorkSheetId = vTemp[3];
//	var PDF_File = vTemp[4];
//	var vFrom = vTemp[5];
//	$.ajax({
//		type: 'POST',
//		url: "/umbraco/Surface/WorkSheet/InsertDownloadPrint",
//		data: { 'CultureInfo': vCInfo, 'RefUserId': vUserId, 'Age': AgeTitleDesc, 'WorkSheetId': WorkSheetId, 'WorkshhetPDFUrl': PDF_File, 'vFrom': vFrom },
//		dataType: "html",
//		success: function (data) {
//			window.open(PDF_File, '_blank', 'fullscreen=yes');
//		}
//	});
//}

function festivalOfferInternalData(responce) {
	/*$("#festival-offers-WorksheetList").html("").append(responce);*/
	$("#festival-offers-WorksheetList").html(responce);
	$("#festival-offers-WorksheetList").show();
	share();
	slicjFunction();
	$(".clsPrintDoc").click(function () {
		var vURLPath = $(this).find("span").html();
		//alert(vURLPath);
		PrintWorkSheet(vURLPath);

		try {
			var layerDetail = $(this).find('span').next('span').html();
			var pageName = $(this).find('span').next('span').next('span').html();
			printTracker(layerDetail, pageName);
		}
		catch (ex) {
			//console.log('error');
		}
	});

	$("#worksSheetByIdLoadMore").click(function () {
		GetFestivalWorkSheetList($("#DisplayCount").val());
	});
}