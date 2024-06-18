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


$(document).ready(function () {
	//VideoTutorialDetails();
	GetPlayVideosDetails();
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
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				PrintWorkSheet(vURLPath);
			});
			PlayVideoView();

			


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
			//alert(data.status);
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

function GetPlayVideosDetails() {
	$(".dvVideoPlay").show();
	var Input = {
		CultureInfo: $("#hdnCultureInfo").val(),
		FilterType: $("#hdnqtype").val()
	};
	$("#videoLoading").css('display', 'block');
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/PlayVideos",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (responce) {
			$("#PlayVideoDiv").html("");
			$("#PlayVideoDiv").append(responce);
			share();
			$(".clsPrintDoc").click(function () {
				var vURLPath = $(this).find("span").html();
				PrintWorkSheet(vURLPath);
			});
			PlayVideoView();
		}, error: function (error) {

		}, complete: function () {
			$("#videoLoading").css('display', 'none');
		}
	});
}

function PlayVideoView() {

	var filterType = $("#hdnqtype").val();
	//var allVideos = $("#hdnVideos").val();
	var allVideos = $("#NewhdnVideos").val();

	//alert(allVideos);
	$.ajax({
		type: 'GET',
		url: "/umbraco/Surface/Video/VideoTrackingView",
		data: { 'filtertype': filterType },
		dataType: "json",
		success: function (data) {

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


			if (data.VideoExecutionTimeInMin != null) {
				vid_executionTime = data.VideoExecutionTimeInMin;
			}

			//console.log(index);
			//console.log(video_ids);
			//console.log(vid_executionTime);
			//console.log(palyVid);
			//console.log(video_ids);
			//console.log(allVideos);

			var options = {
				url: 'https://vimeo.com/' + video_ids[index],
				width: '100%',
				responsive: true,
				loop: false,
				autoplay: true,
				muted: true,
			};



			var player = new Vimeo.Player('HpPlcvid', options);
			$('.item-col .card-box').removeClass('active-video');
			$(".card-video a[data-id='" + palyVid + "']").parent().parent().addClass('active-video');
			vediothumslid.slick('slickGoTo', index);


			var playNext = function (data) {
				player.pause();
				index++;
				if (index <= video_ids.length)
					//player.loadVideo(video_ids[index])
					var vidthumbId = video_ids[index];

				//var redircturl = $(".card-video a[data-id='"+vidthumbId+"']").attr('href');


				//console.log("url.....    " + redircturl);
				//console.log("dopwnload link...    " + downllink);

				player.loadVideo('https://vimeo.com/' + video_ids[index]).then(function (id) {
					player.setMuted(false);
					player.setVolume(0.5)
					$('.item-col .card-box').removeClass('active-video');
					$(".card-video a[data-id='" + vidthumbId + "']").parent().parent().addClass('active-video');
					var cardtxt = $(".card-video a[data-id='" + vidthumbId + "']").parents('.card-box').find('.card-dscptn').html();
					var downllink = $(".card-video a[data-id='" + vidthumbId + "']").parent().parent().find('.downllink').attr('href');
					$('.video-play-cont .card-dscptn').html(cardtxt);
					$('.video-play-cont .downllink').attr('href', downllink);

					vediothumslid.slick('slickGoTo', index)
				});
			};

			player.on('play', function () {
				console.log('played the video!');
				player.setMuted(false);
				player.setVolume(0.5)
			});

			player.on('ended', playNext);
			// player.on('ended', function() {

			// 	var PlayEnd = 1;
			// 	var PlayDuration = 0;
			// 	PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd);
			// 	playNext
			// 	console.log('played ended');

			// });



			player.setCurrentTime(vid_executionTime).then(function (seconds) {
				// seconds = the actual time that the player seeked to
			})





			player.getCurrentTime().then(function (seconds) {
				// seconds = the current playback position
				console.log(seconds);
			});

			player.on('pause', function (data) {
				var PlayDuration = data.seconds;
				var PlayEnd = 0;
				PlayVideoUpdate(UniqueId, PlayDuration, PlayEnd)
			});





			// player.setCurrentTime(120).then(function(seconds) {
			// 	// seconds = the actual time that the player seeked to
			// })



		}
	});
}