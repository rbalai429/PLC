
$(document).ready(function () {
	//VideoTutorialDetails(0);
	//GetVideosDetailsById(0);
});

var Input = {
	CurrentNode: $("#hdnCurrentNode").val(),
	CultureInfo: $("#hdnCultureInfo").val(),
	FilterType: $("#hdnqtype").val(),
	FilterId: $("#hdnAgeName").val(),
	DisplayCount: 0
};
$.post("/umbraco/Surface/Video/GetVideosById",
	{
		Input
	},
	function (responce, status) {
		VideoDetailsInternalData(responce);

		if (status == 'success') {
			$("#videoLoading").css('display', 'none');
		}
	});


function VideoTutorialDetails(displayCount) {

	$(".dvVideoSectionContainer").show();

	var vrCultureInfo = $("#hdnCultureInfo").val();
	var DownloadText = $("#hdnDownloadText").val();
	var SubscribeforDownload = $("#hdnSubscribeforDownload").val();
	var BuyNow = $("#hdnBuyNow").val();
	var BuyNewSubscription = $("#hdnBuyNewSubscription").val();

	var filterType = $("#hdnqtype").val();
	var filterId = $("#hdnqtypeid").val();

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoDetailsPage",
		data: { 'vrFilterType': filterType, 'vrFilterId': filterId, 'CultureInfo': vrCultureInfo, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload, 'vrDisplayOnPage': displayCount, "upgradeToPremiumText": BuyNow, "BuyNewSubscription": BuyNewSubscription },
		dataType: "html",
		success: function (data) {
			//$(".ytp-chrome-top-buttons").hide();
			$(".dvVideoSectionContainer").html("");
			$(".dvVideoSectionContainer").html(data);

			share();
		}
	});
}

function loadMoreData(displayCount) {
	VideoTutorialDetails(displayCount);
}

function GetVideosDetailsById(displayCount) {
	$("#videoLoading").css('display', 'block');
	var Input = {
		CurrentNode: $("#hdnCurrentNode").val(),
		CultureInfo: $("#hdnCultureInfo").val(),
		FilterType: $("#hdnqtype").val(),
		FilterId: $("#hdnAgeName").val(),
		DisplayCount: displayCount
	};

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/GetVideosById",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (data) {
			//$(".ytp-chrome-top-buttons").hide();
			VideoDetailsInternalData(data);
		}, error: function (error) {

		}, complete: function () {
			$("#videoLoading").css('display', 'none');
		}
	});
}

function VideoDetailsInternalData(data) {
	$("#VideoDetailsDiv").html("");
	$("#VideoDetailsDiv").html(data);
	$("#videosByIdLoadMore").click(function () {
		GetVideosDetailsById($("#DisplayCount").val());
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
					slidesToShow: 1.1,
					slidesToScroll: 1
				}
			}

		]
	});
	palyvedio();
	share();
	$(".clsPrintDoc").click(function () {
		var vURLPath = $(this).find("span").html();
		try {
			var layerDetail = $(this).find('span').next('span').html();

			printVideoTracker(layerDetail, 'Video-Detail');
		}
		catch (ex) {
			//cpnsole.log(ex);
		}
		PrintWorkSheet(vURLPath);
	});
}




var hovS = 0;
var timer;
function palyvedio() {




	const isIOS = [
		'iPad Simulator',
		'iPhone Simulator',
		'iPod Simulator',
		'iPad',
		'iPhone',
		'iPod',
	].indexOf(navigator.platform) !== -1;


	$('.card-video').mouseover(function () {

		var vidID = $(this).attr('data-vid');
		var position = $(this).offset();
		var heading = $(this).parent().find('.card-title').html();

		console.log(heading);

		setTimeout(function () {
			$(".vid_preview").css({
				"--plc-video-preview-top-position": position.top - 20 + "px",
				"--plc-video-preview-original-top-position": position.top + "px",
				"--plc-video-preview-horizontal-position": position.left - 20 + "px",
				"--plc-video-preview-original-horizontal-position": position.left + "px",
				"--plc-video-preview-initial-scale": 'scale(0.755556)'

			});
			$('.preview_hd').html(heading);
		}, 500);


		if (vidID != '' && (hovS == 0)) {

			timer = setTimeout(function () {

				if (isIOS == true) {
					GetVimeoVideoUrls(vidID, 'hls')
				} else {
					GetVimeoVideoUrls(vidID, 'dash')
				}

			}, 1000);




		}


	});





	$('.vid_preview').mouseleave(function () {

		$(this).removeClass('active');
		clearTimeout(timer);
		hovS = 0
	});

	$('.card-video').mouseleave(function () {

		if (hovS == 0) {
			$(".vid_preview").removeClass('active');
			clearTimeout(timer);
			hovS = 0

		}

	});

	const $popup = $('.vid_preview');

	$(document).mouseup(e => {
		if (!$popup.is(e.target) // if the target of the click isn't the container...
			&& $popup.has(e.target).length === 0) // ... nor a descendant of the container
		{
			$popup.removeClass('active');
			clearTimeout(timer);
			hovS = 0
		}
	});

}



function GetVimeoVideoUrls(videoId, type) {

	console.log(videoId)


	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/GetVideoUrl",
		data: { 'videoId': videoId, 'type': type },
		dataType: "json",
		success: function (data) {

			console.log(data.VimeoUrl)


			if (document.contains(document.querySelector('video-js'))) {
				document.querySelector('video-js').remove();
			}


			//	elem.parentNode.removeChild(elem);

			var fixture = document.getElementById('vimiovids');
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
				fullscreen: false,
				controlBar: {
					playToggle: true,
					captionsButton: false,
					chaptersButton: false,
					subtitlesButton: false,
					remainingTimeDisplay: true,
					volumePanel: false,
					pictureInPictureToggle: false,
					progressControl: {
						seekBar: true
					},
					fullscreenToggle: false,
					playbackRateMenuButton: false
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

				this.tech_.off('mousedown');
				this.tech_.on('mousedown', function (e) {
					e.preventDefault();
					console.log("paly")
				});

			});

			$(".vid_preview").addClass('active');
			hovS = 1

		}
	});

}
