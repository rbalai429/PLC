//const { default: de } = require("../Umbraco/lib/flatpickr/l10n/de");

$(document).ready(function () {
	BindExpertTalk();
	BindExpertVideos();
});

function BindExpertTalk() {
	var vrCurrentNode = $("#hdnCurrentNode").val();
	var vrCultureInfo = $("#hdnCultureInfo").val();
	var Input = {
		CultureInfo: vrCultureInfo,
		CurrentNode: vrCurrentNode
	};

	$.post("/umbraco/Surface/ExpertTalks/GetExpertTalkList",
		{
			Input
		},
		function (responce, status) {

			$("#upcomingwebinar").show();
			$(".dvExpertWiseContainer").html(responce);
			$("#dvExpertYTalks").show();
			ExpertJoinNowFuncation();
			share();
			var bLazy = new Blazy({
				// Options
			});

			if (status == 'success') {
				$("#videoLoading").css('display', 'none');
			}

		});

	//$.ajax({
	//    type: 'POST',
	//    contentType: "application/json",
	//    url: "/umbraco/Surface/ExpertTalks/GetExpertTalkList",
	//    data: JSON.stringify(Input),
	//    success: function (responce) {
	//        $(".dvExpertWiseContainer").html(responce);
	//        $("#dvExpertYTalks").show();
	//        ExpertJoinNowFuncation();
	//        var bLazy = new Blazy({
	//            // Options
	//        });
	//    },
	//    error: function (error) {
	//        //console.log(error)
	//    }, complete: function () {
	//        $("#dvExpertYTalks").css('display', 'block');
	//        //console.log($(window).data('ajaxready'));
	//        $(window).data('ajaxready', true);
	//    }
	//});
}

function BindExpertVideos() {
	var vrCurrentNode = $("#hdnCurrentNode").val();
	var vrCultureInfo = $("#hdnCultureInfo").val();
	var Input = {
		CultureInfo: vrCultureInfo,
		CurrentNode: vrCurrentNode
	};

	$.post("/umbraco/Surface/ExpertTalks/GetExpertTalkVideoList",
		{
			Input
		},
		function (responce, status) {
			$(".dvExpertTalkVideoContainer").html(responce);
			$("#dvExpertYTalks").show();
			ExpertJoinNowFuncation();
			share();
			var bLazy = new Blazy({
				// Options
			});

			if (status == 'success') {
				$("#videoLoading").css('display', 'none');
			}
		});

	//$.ajax({
	//    type: 'POST',
	//    contentType: "application/json",
	//    url: "/umbraco/Surface/ExpertTalks/GetExpertTalkVideoList",
	//    data: JSON.stringify(Input),
	//    success: function (responce) {
	//        $(".dvExpertTalkVideoContainer").html(responce);
	//        $("#dvExpertYTalks").show();
	//        ExpertJoinNowFuncation();
	//        var bLazy = new Blazy({
	//            // Options
	//        });
	//    },
	//    error: function (error) {
	//        //console.log(error)
	//    }, complete: function () {
	//        $("#dvExpertYTalks").css('display', 'block');
	//        //console.log($(window).data('ajaxready'));
	//        $(window).data('ajaxready', true);
	//    }
	//});
}


function ExpertJoinNowFuncation() {
	$(".joinNowButton").click(function () {
		$.ajax({
			type: 'GET',
			url: "/umbraco/Surface/ExpertTalks/InsertExpertTalkHistory",
			dataType: "JSON",
			success: function (result) {
				window.location.href = result.navigation;
			}
		});
	})
}

