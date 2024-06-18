
$(document).ready(function () {

	//var vrCurrentNode = $("#hdnCurrentNode").val();
	//var vrCultureInfo = $("#hdnCultureInfo").val();
	//var DownloadText = $("#hdnDownloadText").val();
	//var SubscribeforDownload = $("#hdnSubscribeforDownload").val();

	//VideoData('age');
});


function VideoData(filtertype) {

	//var vrAgeRange = $("#slect-age").val();
	//var vrCategoryItem = $("#select-categorys").val();
	//var vrPathwayItem = $("#select-pathways").val();
	var vrCurrentNode = $("#hdnCurrentNode").val();
	var vrCultureInfo = $("#hdnCultureInfo").val();

	var DownloadText = $("#hdnDownloadText").val();
	var SubscribeforDownload = $("#hdnSubscribeforDownload").val();
	var SeeMore = $("#hdnSeeMore").val();
	var BuyNow = $("#hdnBuyNow").val();
	var BuyNewSubscription = $("#hdnBuyNewSubscription").val();

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoTutorial",
		data: { 'vrNoreId': vrCurrentNode, 'CultureInfo': vrCultureInfo, 'filterType': filtertype, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload, "SeeMoreText": SeeMore, "upgradeToPremiumText": BuyNow, "BuyNewSubscription": BuyNewSubscription  },
		dataType: "html",
		success: function (data) {

			$(".dvVideoSectionContainer").html("");
			$(".dvVideoSectionContainer").html(data);

			//$(".dvVideoSectionContainerDetails").html(data);

			//$("#dvVideoTuturial").show();
			share();

			var bLazy = new Blazy({
				// Options
			});

			$('.video-thum-slider').slick({
				dots: false,
				infinite: false,
				speed: 300,
				slidesToShow: 4,
				slidesToScroll: 4,
				responsive: [
					{
						breakpoint: 1023,
						settings: {
							arrows: false,
							slidesToShow: 3.1,
							slidesToScroll: 3

						}
					},
					{
						breakpoint: 767,
						settings: {
							arrows: false,
							slidesToShow: 2.1,
							slidesToScroll: 2
						}
					},
					{
						breakpoint: 480,
						settings: {
							arrows: false,
							slidesToShow: 2.3,
							slidesToScroll: 1
						}
					}

				]
			});
		}
	});
}


function TrackVideo(VideoId, nodeId, VideoType) {
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoViewsTracker",
		data: { 'vrNoreId': nodeId, 'vrVideoId': VideoId, 'VideoType': VideoType },
		dataType: "json",
		success: function (data) {

			//$(".dvVideoSectionContainerDetails").html("");
			//$(".dvVideoSectionContainerDetails").html(data);

			//share();
		}
	});
}

function VideoTutorial(filtertype) {
	VideoData(filtertype);
}

GetVideosList('age');
function GetVideosList(filtertype) {
	$("#videoLoading").css('display', 'block');
	$("#NoResultFound").css('display', 'none');
	var Input = {
		CurrentNode: $("#hdnCurrentNode").val(),
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: $("#vrAgrGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
		selectedVolume: $("#vrVolumeGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
		selectedCategory: $("#vrCategoriesVideo option:selected").map(function () { return this.value }).get().join(", "),
		FilterType: filtertype
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/Video/GetVideosList",
		data: JSON.stringify(Input),
		success: function (responce) {
			$("#VideosList").html("");
			$("#VideosList").append(responce);
			$('.video-thum-slider').slick({
				dots: false,
				infinite: false,
				speed: 300,
				slidesToShow: 4,
				slidesToScroll: 4,
				responsive: [
					{
						breakpoint: 1023,
						settings: {
							arrows: false,
							slidesToShow: 3.1,
							slidesToScroll: 3

						}
					},
					{
						breakpoint: 767,
						settings: {
							arrows: false,
							slidesToShow: 2.1,
							slidesToScroll: 2
						}
					},
					{
						breakpoint: 480,
						settings: {
							arrows: false,
							slidesToShow: 1.4,
							slidesToScroll: 1
						}
					}

				]
			});
			share();
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				//PrintWorkSheet(vURLPath);
			});
		},
		error: function (error) {
			console.log(error)
		}, complete: function () {
			$("#videoLoading").css('display', 'none');
		}
	});
}
function PrintWorkSheet(vPath) {
	var vTemp = vPath.split('$');
	var vCInfo = vTemp[0];
	var vUserId = vTemp[1];
	var AgeTitleDesc = vTemp[2];
	var WorkSheetId = vTemp[3];
	var PDF_File = vTemp[4];
	var vFrom = vTemp[5];
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/InsertDownloadPrint",
		data: { 'CultureInfo': vCInfo, 'RefUserId': vUserId, 'Age': AgeTitleDesc, 'WorkSheetId': WorkSheetId, 'WorkshhetPDFUrl': PDF_File, 'vFrom': vFrom },
		dataType: "html",
		success: function (data) {
			window.open(PDF_File, '_blank', 'fullscreen=yes');
		}
	});

}

$('.fltr-box #vrAgrGroupeVideo').change(function () {
	GetVideosList();
})
$('.fltr-box #vrVolumeGroupeVideo').change(function () {
	GetVideosList();
})
$('.fltr-box #vrCategoriesVideo').change(function () {
	GetVideosList();
})