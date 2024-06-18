
$(document).ready(function () {
	VideoTutorialDetails();
});


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
		data: { 'CultureInfo': vrCultureInfo, 'filterType': filterType, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload, "upgradeToPremiumText": BuyNow, "BuyNewSubscription": BuyNewSubscription  },
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
			PlayVideoView();

			$('.video-thum-slider').slick({
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
		},
		error: function (error) {
			$("#videoLoading").css('display', 'none');
		},
		complete: function () {
			$("#videoLoading").css('display', 'none');
		}
	});
}

function PlayVideoView() {

	var filterType = $("#hdnqtype").val();
	var allVideos = $("#hdnVideos").val();
	//alert(allVideos);
	$.ajax({
		type: 'GET',
		url: "/umbraco/Surface/Video/VideoTrackingView",
		data: { 'filtertype': filterType },
		dataType: "json",
		success: function (data) {

			//alert('UniqueId - ' + data.UniqueId + " | " + 'UserId - ' + data.UserId + " | " + 'VideoId - ' + data.VideoId + " | " + 'VideoExecutionTimeInMinSec - ' + data.VideoExecutionTimeInMin + " | " + 'VideosTotalTimeInMin - ' + data.VideosTotalTimeInMin + " | " + 'VideosFinished - ' + data.VideosFinished + " | " + 'All Videos - ' + allVideos);

			var video_ids = allVideos.split(',');
			var palyVid = data.VideoId;
			var index = video_ids.indexOf(palyVid);
			var UniqueId = data.UniqueId;
			var UserId =  data.UserId;
			//console.log(index);
			//console.log(video_ids)

			var options = {
				url: 'https://vimeo.com/'+video_ids[index],
				width: '100%',
				responsive: true,
				loop: false,
				autoplay: true,
				muted: true,
			};


       
			var player = new Vimeo.Player('HpPlcvid', options);
			$(".card-video a[data-id='"+palyVid+"']").parent().addClass('active');
	

			var playNext = function(data){
			player.pause();
			index++;
			if(index<=video_ids.length)
			//player.loadVideo(video_ids[index])
			var vidthumbId = video_ids[index];
           
			 player.loadVideo('https://vimeo.com/'+video_ids[index]).then(function(id) {
				player.setMuted(false);
				player.setVolume(0.5)
				$('.card-video').removeClass('active');
				$(".card-video a[data-id='"+vidthumbId+"']").parent().addClass('active');
                var cardtxt = $(".card-video a[data-id='"+vidthumbId+"']").parents('.card-box').find('.card-dscptn').html();
				$('.video-play-cont .card-dscptn').html(cardtxt);
			  });
			}

			


			player.on('play', function() {
				//console.log('played the video!');
				player.setMuted(false);
				player.setVolume(0.5)
			});

			player.on('ended', playNext);


           
			

			player.getCurrentTime().then(function(seconds) {
				// seconds = the current playback position
				//console.log(seconds);
			});

			player.on('pause', function(data) {
				var PlayDuration = data.seconds;
				var PlayEnd = 0;
				//PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd)
			});

			



			// player.setCurrentTime(120).then(function(seconds) {
			// 	// seconds = the actual time that the player seeked to
			// })

			
			
		}
	});
}

function PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd) {
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/VideoTrackingUpdate",
		data: { 'vrUniqueId': UniqueId,'vrPlayDuration': PlayDuration, 'vrPlayEnd': PlayEnd},
		dataType: "json",
		success: function (data) {
			alert(data.status);
		}
	});
}
