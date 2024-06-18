var Age = null;
var Volume = null;
var Category = null;

var selectAge = $("#selectAge").val();
var selectWeek = $("#selectWeek").val();
var selectCategory = $("#selectCategory").val();
var reset = $("#reset").val();
$("#searchReset").html(reset);


$(document).ready(function () {

	Age = $('#vrAgrGroupeVideo');
	Volume = $('#vrVolumeGroupeVideo');
	Category = $('#vrCategoriesVideo');
	if (Age != null && Age != undefined && Age.length > 0) {
		GetAgeGroupList();
	}
	//else {
	//	GetVideosList("", "", "");
	//}
	if (Volume != null && Volume != undefined && Volume.length > 0) {
		GetVolumeList("");
		$('.select-volume').multipleSelect({
			placeholder: selectWeek,
			selectAll: false
		})
	}
	if (Category != null && Category != undefined && Category.length > 0) {
		$('.select-category').multipleSelect({
			placeholder: 'Select Category',
			selectAll: false
		});
		GetCategoryList("", "");
	}
	//GetVideosList("", "", "");
});

function GetAgeGroupList() {
	$.get("/umbraco/Surface/Video/GetAgeGroupList?CurrentNode=" + $("#hdnCurrentNode").val(),
		function (responce, status) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Age != null && Age != undefined && Age.length > 0) {
						$("#vrAgrGroupeVideo option").remove();
						$.each(responce.Result, function (i, item) {
							$('#vrAgrGroupeVideo').append($('<option>', {
								value: item.ItemValue,
								text: item.ItemName
							}));
						});
						$('.select-age').multipleSelect({
							placeholder: selectAge,
							selectAll: false,
						});
					}
					//if (Volume == null || Volume == undefined || Volume.length == 0) {
					//	GetVideosList("", "", "");
					//}
				}
			}
		});

	//$.ajax({
	//	type: "GET",
	//	contentType: "application/json",
	//	url: "/umbraco/Surface/Video/GetAgeGroupList?CurrentNode=" + $("#hdnCurrentNode").val(),
	//	success: function (responce) {
	//		if (responce != null && responce != undefined && responce.StatusCode == 200) {
	//			if (responce.Result != null) {
	//				if (Age != null && Age != undefined && Age.length > 0) {
	//					$("#vrAgrGroupeVideo option").remove();
	//					$.each(responce.Result, function (i, item) {
	//						$('#vrAgrGroupeVideo').append($('<option>', {
	//							value: item.ItemValue,
	//							text: item.ItemName
	//						}));
	//					});
	//					$('.select-age').multipleSelect({
	//						placeholder: 'Select Age',
	//						selectAll: false,
	//					});
	//				}
	//				//if (Volume == null || Volume == undefined || Volume.length == 0) {
	//				//	GetVideosList("", "", "");
	//				//}
	//			}
	//		}
	//	},
	//	error: function (error) {
	//		console.log(error)
	//	}, complete: function () {
	//		// $("#worksheetLoading").css('display', 'none');
	//	}
	//});
}
function GetVolumeList(SelectedAvge) {
	$.ajax({
		type: "GET",
		contentType: "application/json",
		url: "/umbraco/Surface/Video/GetVolumeList?CurrentNode=" + $("#hdnCurrentNode").val() + "&AgeItemValue=" + SelectedAvge,
		success: function (responce) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Volume != null && Volume != undefined && Volume.length > 0) {
						$("#vrVolumeGroupeVideo option").remove();
						const sb = document.querySelector('#vrVolumeGroupeVideo');
						$.each(responce.Result, function (i, item) {
							const option = new Option(item.ItemName, item.ItemValue);
							sb.add(option, undefined);
						});
						$('.select-volume').multipleSelect("destroy").multipleSelect({
							placeholder: selectWeek,
							selectAll: false
						})
					}
					if (Volume == null || Volume == undefined || Volume.length == 0) {
						if (Category == null || Category == undefined || Category.length == 0) {
							GetVideosList(
								$("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", "),
								$("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", "),
								"");
						}
					}
				}
			}
		},
		error: function (error) {
			console.log(error)
		}, complete: function () {
			// $("#worksheetLoading").css('display', 'none');
		}
	});
}
function GetCategoryList(SelectedAvge, SelectedVolume) {
	$.ajax({
		type: "GET",
		contentType: "application/json",
		url: "/umbraco/Surface/Video/GetCategoryList?CurrentNode=" + $("#hdnCurrentNode").val() + "&AgeItemValue=" + SelectedAvge + "&VolumeItemValue=" + SelectedVolume,
		success: function (responce) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Category != null && Category != undefined && Category.length > 0) {
						$("#vrCategoriesVideo option").remove();
						const sb = document.querySelector('#vrCategoriesVideo');
						$.each(responce.Result, function (i, item) {
							const option = new Option(item.ItemName, item.ItemValue);
							sb.add(option, undefined);
						});
						$('.select-category').multipleSelect("destroy").multipleSelect({
							placeholder: selectCategory,
							selectAll: false
						});
					}
					GetVideosList(
						$("#vrAgrGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
						$("#vrVolumeGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
						$("#vrCategoriesVideo option:selected").map(function () { return this.value }).get().join(", "));
				}
			}
		},
		error: function (error) {
			console.log(error)
		}, complete: function () {
			// $("#worksheetLoading").css('display', 'none');
		}
	});
}
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
		data: { 'vrNoreId': vrCurrentNode, 'CultureInfo': vrCultureInfo, 'filterType': filtertype, 'cultureDownloadText': DownloadText, 'CultureSubscribeforDownload': SubscribeforDownload, "SeeMoreText": SeeMore, "upgradeToPremiumText": BuyNow, "BuyNewSubscription": BuyNewSubscription },
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
							slidesToShow: 1.1,
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


var Input = {
	CurrentNode: $("#hdnCurrentNode").val(),
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: "",
	selectedVolume: "",
	selectedCategory: "",
	FilterType: 'age'
};
$.post("/umbraco/Surface/Video/GetVideosList",
	{
		Input
	},
	function (responce, status) {
		VideoInternalData(responce);
		
		if (status == 'success') {
			$("#videoLoading").css('display', 'none');
		}
	});


function GetVideosList(selectedAgeGroup, selectedVolume, selectedCategory) {
	$("#videoLoading").css('display', 'block');
	$("#NoResultFound").css('display', 'none');
	var Input = {
		CurrentNode: $("#hdnCurrentNode").val(),
		CultureInfo: $("#hdnCultureInfo").val(),
		selectedAgeGroup: selectedAgeGroup,
		selectedVolume: selectedVolume,
		selectedCategory: selectedCategory,
		FilterType: 'age'
	};
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "/umbraco/Surface/Video/GetVideosList",
		data: JSON.stringify(Input),
		success: function (responce) {
			VideoInternalData(responce);
		},
		error: function (error) {
			//console.log(error)
			$("#videoLoading").css('display', 'none');
		}, complete: function () {
			$("#videoLoading").css('display', 'none');
		}
	});
}

function VideoInternalData(responce) {
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
			printVideoTracker(layerDetail, 'Videos');
		}
		catch (ex) {
			//cpnsole.log(ex);
		}
		PrintWorkSheet(vURLPath);
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

$('.fltr-box #vrAgrGroupeVideo').change(function () {
	var vrAgrGroupe = $("#vrAgrGroupeVideo option:selected").map(function () { return this.value }).get().join(", ")
	var vrVolumeGroupe = $("#vrVolumeGroupeVideo option:selected").map(function () { return this.value }).get().join(", ")
	if (Volume != null && Volume != undefined && Volume.length > 0) {
		GetVolumeList(vrAgrGroupe)
	}
	if (Category != null && Category != undefined && Category.length > 0) {
		GetCategoryList(vrAgrGroupe, vrVolumeGroupe);
	}
	else {
		GetVideosList(vrAgrGroupe, "", "");
	}

	try {
		var vrAgrGroupeText = $("#vrAgrGroupeVideo option:selected").map(function () { return this.text }).get().join(", ");
		commonFilterLayer("Select age Filter", vrAgrGroupeText);
	}
	catch (ex) { //console.log(ex);
	}
})
$('.fltr-box #vrVolumeGroupeVideo').change(function () {
	var vrAgrGroupe = $("#vrAgrGroupeVideo option:selected").map(function () { return this.value }).get().join(", ")
	var vrVolumeGroupe = $("#vrVolumeGroupeVideo option:selected").map(function () { return this.value }).get().join(", ")
	if (Category != null && Category != undefined && Category.length > 0) {
		GetCategoryList(vrAgrGroupe, vrVolumeGroupe);
	} else {
		GetVideosList(vrAgrGroupe, vrVolumeGroupe, "");
	}

})
$('.fltr-box #vrCategoriesVideo').change(function () {
	GetVideosList(
		$("#vrAgrGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
		$("#vrVolumeGroupeVideo option:selected").map(function () { return this.value }).get().join(", "),
		$("#vrCategoriesVideo option:selected").map(function () { return this.value }).get().join(", "));
})


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
				"--plc-video-preview-top-position": position.top - 50 + "px",
				"--plc-video-preview-original-top-position": position.top + "px",
				"--plc-video-preview-horizontal-position": position.left - 50 + "px",
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

