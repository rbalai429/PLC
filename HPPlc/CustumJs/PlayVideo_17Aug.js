$(document).ready(function () {
	var checkCloseX = 0;
	$(document).mousemove(function (e) {
		if (e.pageY <= 5) {
			checkCloseX = 1;
		}
		else { checkCloseX = 0; }
	});

	window.onbeforeunload = function (event) {
		if (event) {
			if (checkCloseX == 1) {

				// alert('1111');

			}
		}
	};
});


//$(document).ready(function () {
//	//VideoTutorialDetails();
//	//GetPlayVideosDetails();
//});


function VideoTutorialDetails() {

	var vrCultureInfo = $("#hdnCultureInfo").val();
	var DownloadText = $("#hdnDownloadText").val();
	var SubscribeforDownload = $("#hdnSubscribeforDownload").val();
	var BuyNow = $("#hdnBuyNow").val();
	var filterType = $("#hdnqtype").val();
	var BuyNewSubscription = $("#hdnBuyNewSubscription").val();
	$(".dvVideoPlay").show();


	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoPlay",
		data: { 'CultureInfo': vrCultureInfo, 'filterType': filterType, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload, "upgradeToPremiumText": BuyNow, "BuyNewSubscription": BuyNewSubscription },
		dataType: "html",
		success: function (data) {
			//alert(data);
			var SplitData = data.split('|');

			if (SplitData[0] != null || SplitData[0] != "") {
				$(".dvVideoPlay").html("");
				$(".dvVideoPlay").html(SplitData[0]);
			}
			if (SplitData[1] != null || SplitData[1] != "") {
				$("#hdnVideos").val(SplitData[1]);
			}

			share();
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				var layerDetail = $(this).attr("data-detailDt");
				printVideoTracker(layerDetail, 'Play-Video');
				PrintWorkSheet(vURLPath);
			});
			PlayVideoView();




		}


	});
}



function PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd) {
	//alert('salik');
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoTrackingUpdate",
		data: { 'vrUniqueId': UniqueId, 'vrPlayDuration': PlayDuration, 'vrPlayEnd': PlayEnd },
		dataType: "json",
		//contentType: "application/json",
		success: function (data) {
			//alert(data.message);
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

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	FilterType: $("#hdnqtype").val()
};
$.post("/umbraco/Surface/Video/PlayVideos",
	{
		Input
	},
	function (responce, status) {
		PlayVideoInternalData(responce);

		if (status == 'success') {
			$("#videoLoading").css('display', 'none');
		}
	});

//function GetPlayVideosDetails() {
//	$(".dvVideoPlay").show();
//	var Input = {
//		CultureInfo: $("#hdnCultureInfo").val(),
//		FilterType: $("#hdnqtype").val()
//	};
//	$("#videoLoading").css('display', 'block');
//	$.ajax({
//		type: 'POST',
//		url: "/umbraco/Surface/Video/PlayVideos",
//		data: JSON.stringify(Input),
//		contentType: "application/json",
//		success: function (responce) {
//			PlayVideoInternalData(responce);
//		}, error: function (error) {

//		}, complete: function () {
//			$("#videoLoading").css('display', 'none');
//		}
//	});
//}

function PlayVideoView() {
	var filterType = $("#hdnqtype").val();
	//var allVideos = $("#hdnVideos").val();
	var allVideos = $("#NewhdnVideos").val();
	//alert(allVideos);
	//alert(allVideos);
	$.ajax({
		type: 'GET',
		url: "/umbraco/Surface/Video/VideoTrackingView",
		data: { 'filtertype': filterType },
		dataType: "json",
		success: function (data) {
			console.log('video');
			var vediothumslid = $('.video-thum-slider').slick({
				dots: false,
				infinite: false,
				speed: 300,
				slidesToShow: 3,
				slidesToScroll: 3,
				responsive: [
					{
						breakpoint: 1023,
						settings: {
							arrows: false,
							slidesToShow: 2.3,
							slidesToScroll: 3

						}
					},
					{
						breakpoint: 767,
						settings: {
							arrows: false,
							slidesToShow: 1.3,
							slidesToScroll: 2
						}
					},
					{
						breakpoint: 480,
						settings: {
							arrows: false,
							slidesToShow: 1.2,
							slidesToScroll: 1
						}
					}

				]
			});

			//alert('UniqueId - ' + data.UniqueId + " | " + 'UserId - ' + data.UserId + " | " + 'VideoId - ' + data.VideoId + " | " + 'VideoExecutionTimeInMinSec - ' + data.VideoExecutionTimeInMin + " | " + 'VideosTotalTimeInMin - ' + data.VideosTotalTimeInMin + " | " + 'VideosFinished - ' + data.VideosFinished + " | " + 'All Videos - ' + allVideos);

			var video_ids = allVideos.split(',');
			var palyVid = data.VideoId;
			var index = video_ids.indexOf(palyVid);
			var UniqueId = data.UniqueId;
			var UserId = data.UserId;
			var vid_executionTime = 0;
			const isIOS = [
				'iPad Simulator',
				'iPhone Simulator',
				'iPod Simulator',
				'iPad',
				'iPhone',
				'iPod',
			].indexOf(navigator.platform) !== -1;
			console.log(isIOS);
			if (isIOS == true) {
				GetVimeoVideoUrl2(palyVid, 'hls')
			} else {
				GetVimeoVideoUrl2(palyVid, 'dash')
			}




			if (data.VideoExecutionTimeInMin != null) {
				vid_executionTime = data.VideoExecutionTimeInMin;
			}



			$('.item-col .card-box').removeClass('active-video');
			$(".card-video a[data-id='" + palyVid + "']").parent().parent().addClass('active-video');
			vediothumslid.slick('slickGoTo', index);
		},
		error: function (xhr) {
			//alert(xhr.responseText);
		}
	});
}

function PlayVideoInternalData(responce) {
	$("#PlayVideoDiv").html("");
	$("#PlayVideoDiv").append(responce);
	PlayVideoView();
	share();
	$(".clsPrintDoc").click(function () {
		var vURLPath = $(this).find("span").html();
		PrintWorkSheet(vURLPath);
		try {
			var layerDetail = $(this).attr("data-detailDt");
			printVideoTracker(layerDetail, 'Play-Video');
		}
		catch (ex) {
			//console.log('x');
		}
	});
	
}
//function OpenVideo(VideoId,VideoLength) {
//	dataLayer.push({
//		event: 'e_videoLoad',
//		videoName: VideoId,
//		videoDuration: VideoLength
//	});
//}
//Start or resume play
//function PlayVideo(VideoId,VideoPlayedInSec) {
//	dataLayer.push({
//		event: 'e_videoPlay',
//		videoName: VideoId,
//		videoCuePoint: VideoPlayedInSec
//	});
//}
//Stop or pause play (user initiated)
//function PauseVideo(VideoId) {
//	dataLayer.push({
//		event: 'e_videoPause',
//		videoName: VideoId
//	});
//}

function GetVimeoVideoUrl2(videoId, type) {
	var nextelemt = $('.active-video').parents('.slick-slide').next();
	var redircturl = nextelemt.find('a').attr('href');

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/GetVideoUrl",
		data: { 'videoId': videoId, 'type': type },
		dataType: "json",
		success: function (data) {
			console.log(data.VimeoUrl);
			var fixture = document.getElementById('HpPlcvid');
			var videoEl = document.createElement('video-js');

			videoEl.setAttribute('controls', '');
			videoEl.setAttribute('preload', 'auto');
			videoEl.setAttribute('playsinline', '');
			videoEl.className = 'vjs-default-skin';
			fixture.appendChild(videoEl);

			var player = videojs(videoEl, {
				autoplay: true,
				muted: true,
				fluid: true,
				plugins: {
					httpSourceSelector: {
						default: 'auto'
					}
				},
				html5: {
					vhs: {
						overrideNative: true
					},
					nativeAudioTracks: false,
					nativeVideoTracks: false
				}
			});


			player.ready(function () {

				if (type == 'hls') {
					this.src({
						src: data.VimeoUrl,
						type: 'application/x-mpegURL',

					});
				}

				if (type == 'dash') {

					this.src({
						src: data.VimeoUrl,
						type: 'application/dash+xml'
					});
				}
				
				this.play();
			});

			player.on('ended', function () {
				if (redircturl != '') {
					window.location = redircturl;
				}
			});
		}
	});

}