
$('.playvideobanner').on("click", function (e) {

	var vimeoId = $(this).next().attr('data-video');
	var divid = $(this).next().attr('id');
	$(this).hide();

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
		GetVimeoVideoUrl2(vimeoId, 'hls', divid)
	} else {
		GetVimeoVideoUrl2(vimeoId, 'dash', divid)
	}
});


function GetVimeoVideoUrl2(videoId, type, divid) {
	//var nextelemt = $('.active-video').parents('.slick-slide').next();
	//var redircturl = nextelemt.find('a').attr('href');

	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/GetVideoUrl",
		data: { 'videoId': videoId, 'type': type },
		dataType: "json",
		success: function (data) {
			//console.log('log');
			if (divid != null) {
				var bannerfixture = document.getElementById(divid);
				//var fixture = document.querySelector("data-id");
				var bannervideoEl = document.createElement('video-js');
				//console.log(bannerfixture);
				if (bannervideoEl != null) {
					bannervideoEl.setAttribute('controls', '');
					bannervideoEl.setAttribute('preload', 'auto');
					bannervideoEl.setAttribute('playsinline', '');
					bannervideoEl.className = 'vjs-default-skin';
					bannerfixture.appendChild(bannervideoEl);

					var player = videojs(bannervideoEl, {
						//autoplay: true,
						muted: true,
						fill: true,
						//fluid: true,
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
						
						
						//this.muted(false);
					});

					player.one('loadedmetadata', function () {
						var duration = player.duration();
						
						OpenVideo("Why Subscribe Print Learn Center -				Video", duration);

						this.play();
					});

					player.on('play', function (data) {
						//var PlayTime = data.seconds;
						var PlayTime = player.currentTime();
						if (PlayTime == null || PlayTime == 0) {
							PlayTime = 0.1;
						}
						PlayVideo("Why Subscribe Print Learn Center - Video", PlayTime)

					});

					player.on('pause', function (data) {

						PauseVideo("Why Subscribe Print Learn Center - Video")
					});
				}
			}


			$('.slider-banner').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
				player.pause();
			});
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
////Start or resume play
//function PlayVideo(VideoId,VideoPlayedInSec) {
//	dataLayer.push({
//		event: 'e_videoPlay',
//		videoName: VideoId,
//		videoCuePoint: VideoPlayedInSec
//	});
//}
////Stop or pause play (user initiated)
//function PauseVideo(VideoId) {
//	dataLayer.push({
//		event: 'e_videoPause',
//		videoName: VideoId
//	});
//}
