
$(document).ready(function () {
	VideoTutorialDetails();
});


function VideoTutorialDetails() {

	var vrCultureInfo = $("#hdnCultureInfo").val();
	var DownloadText = $("#hdnDownloadText").val();
	var SubscribeforDownload = $("#hdnSubscribeforDownload").val();

	var filterType = $("#hdnqtype").val();
	//var videoId = $("#hdnqvideoid").val();
	//var nodeId = $("#hdnqnodeid").val();

	$(".dvVideoPlay").show();
	

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/VideoPlay",
		data: { 'CultureInfo': vrCultureInfo, 'filterType': filterType, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload },
		dataType: "html",
		success: function (data) {

			var SplitData = data.split('|');
			
			$(".dvVideoPlay").html("");
			$(".dvVideoPlay").html(SplitData[0]);

			$("#hdnVideos").val(SplitData[1]);

			share();

			PlayVideoView();
		}
	});
}

function PlayVideoView() {

	var filterType = $("#hdnqtype").val();
	//var videoId = $("#hdnqvideoid").val();
	//var nodeId = $("#hdnqnodeid").val();
	var allVideos = $("#hdnVideos").val();
	//alert(allVideos);
	$.ajax({
		type: 'GET',
		url: "/umbraco/Surface/WorkSheet/VideoTrackingView",
		data: { 'filtertype': filterType },
		dataType: "json",
		success: function (data) {
			
			//alert('UniqueId - ' + data.UniqueId + " | " + 'UserId - ' + data.UserId + " | " + 'VideoId - ' + data.VideoId + " | " + 'VideoExecutionTimeInMinSec - ' + data.VideoExecutionTimeInMin + " | " + 'VideosTotalTimeInMin - ' + data.VideosTotalTimeInMin + " | " + 'VideosFinished - ' + data.VideosFinished + " | " + 'All Videos - ' + allVideos);

		}
	});
}

function PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd) {
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/VideoTrackingUpdate",
		data: { 'vrUniqueId': UniqueId,'vrPlayDuration': PlayDuration, 'vrPlayEnd': PlayEnd},
		dataType: "json",
		success: function (data) {
			//alert(data.status);
		}
	});
}
