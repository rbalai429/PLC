$(document).ready(function () {

	$('#bundlingPopup #congrats-close').on('click', function () {
		$("#congratulations").hide();
		$('.claimoferbtn').show();
	});
	$('.claimoferbtn').on('click', function () {
		$("#congratulations").show();
		$(this).hide();
	});

	try {
		var currenturlpath = window.location.href;
		if (currenturlpath.indexOf("type") > -1) {
			var worksheetId = $('#hdnNodeIdChild').val();
			var pageId = $('#PageId').val();

			if (worksheetId > 0) {
				$.ajax({
					type: "POST",
					url: "/umbraco/Surface/Notification/DownloadPDF_SFMC",
					data: { "worksheetId": worksheetId, "pageId": pageId },
					success: function (e) {
						if (e.status == "done") {


							try {
								commonLayer("SFMCDownload", "SFMC Worksheet Download");
							}
							catch (ex) { //console.log('error');
							}

							location.href = e.navigation;
							Swal.fire("Your worksheet is waiting to be printed in the downloads folder. Print now to get started.");
						}
						if (e.status == "fail") {
							//$(".descTitlePlc").show();
						}
					},
					error: function (error) {
					}, complete: function () {
					}
				});
			}
		}
	}
	catch (er) {
		// console.log("");
	}

	try {
		//Content menu selection
		var urlpath = window.location.href;

		if (urlpath.indexOf("free-downloads") > -1) {
			var pathArray = urlpath.split('/');
			var lastsegment = pathArray[pathArray.length - 2];

			if (lastsegment == null || lastsegment == '' || lastsegment == 'free-downloads') {
				$(".navigation").removeClass("active");
				$(".navigationall").addClass("active");
			}
			else {
				$('.navigationfreedwn[href*="' + lastsegment + '"]').addClass("active");
				$(".navigationall").removeClass("active");
			}
		}
		else {
			$('.navigation li a').each(function () {
				var tabValue = this.href.toLowerCase();
				var withoutHash = tabValue.substr(0, tabValue.indexOf('#'));
				//alert(urlpath.indexOf(withoutHash));

				if (urlpath.indexOf(withoutHash) > 0) {
					//console.log(urlpath.indexOf(withoutHash));
					$(".navigation li a.active").removeClass("active");

					$(this).addClass("active");

				}
				else {

					var withoutSlash = tabValue.substr(0, tabValue.lastIndexOf('/'));
					//alert('test-' + withoutSlash);
					if (urlpath.indexOf(withoutSlash) > -1) {
						$(".navigation li a.active").removeClass("active");

						$(this).addClass("active");
					}

				}
			});
		}

		var activeCnt = $('.navigation li a.active').length
		if (activeCnt == 0) {
			$(".navigation li a").first().addClass("active");
		}

		//console.log(activeCnt);
		$('.navigationblog a').each(function () {
			if (urlpath.indexOf(this.href.toLowerCase()) > -1) {

				$(".navigationblog a.active").removeClass("active");

				$(this).addClass("active");

			}
			//if (this.href === urlpath) {
			//	$(this).addClass('active');
			//}
		});


		try {
			//Mobile navigation
			
			var current = location.pathname; 
			$('.mobNavtr').each(function () {
				var $this = $(this);
				// if the current path is like this link, make it active
				if ($this.attr('href').indexOf(current) !== -1) {
					$this.removeClass('mobileMenuActive');
					$this.addClass('mobileMenuActive');
				}
			})

			if (current == "/") {
				$(".mobNavtr").removeClass('mobileMenuActive');
				$(".mobNavtr").first().addClass("mobileMenuActive");
			}
		}
		catch {
			//console.log(""); 
		}

		if (window.location.href.toLowerCase().indexOf('#tab') > 0) {
			//$('html, body').animate({ scrollTop: $("#navigation").offset().top }, 1500);
			$('html, body').animate({ scrollTop: $(".navigation").offset().top - 150 }, 500);
		}
		//}
		//}
	}
	catch (err) {
		//console.log("");
	}
});


function share() {

	$(".aFBShare").click(function () {
		var ItemVal = $(this).find('span').html();
		publish(encodeURIComponent(ItemVal));
	});
	$(".aFBShareFeed").click(function () {
		publishFeed();
	});
	$(".aWHTAppSH").click(function () {
		var ItemVal = $(this).find('span').html();
		if (/Mobi/.test(navigator.userAgent)) {
			window.open('whatsapp://send?text=' + encodeURIComponent(ItemVal) + ' ' + window.location.href, 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
		else {
			window.open('https://web.whatsapp.com/send?text=' + encodeURIComponent(ItemVal) + ' ' + window.location.href, 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
	});
	$(".aMailSh").click(function () {
		var FullString = $(this).find('span').html();
		var emailBody = FullString.split('`')[0];
		var email = '';
		var subject = FullString.split('`')[1];
		window.location = 'mailto:' + email + '?body=' + encodeURIComponent(emailBody.trimStart()) + '&subject=' + subject;
	});
	$(".aEPrint").click(function () {
		var culture = $('#cultureSessionName').val().trim();
		if (culture == "/")
			culture = "";

		var FullStringFile = $(this).find('span#spnfile').html();
		var FullStringWid = $(this).find('span#wid').html();
		var FullStringItem = $(this).find('span#itemname').html();

		window.open(culture + '/e-print/?file=' + FullStringFile + '&wid=' + FullStringWid + '&itemname=' + FullStringItem, 'sharer', 'toolbar=0,status=0,width=850,height=600');
	});

	$(".aCopyText").click(function () {
		var $temp = $("<input>");
		$("body").append($temp);
		$temp.val($(this).find('span').text()).select();
		document.execCommand("copy");
		$("#copylinkId").text("Copied.");
		$("#copyImageId").hide();
		$temp.remove();

	});
	$(".aMSGShare").click(function () {
		var $temp = $("<input>");
		$("body").append($temp);
		$temp.val($(this).find('span').text()).select();
		document.execCommand("copy");
		$temp.remove();

	});
}

function DownloadData(pdfFile, worksheetId, ageTitle, sourceType) {
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/DownloadData",
		data: {
			'vrPdfFile': pdfFile, 'vrWorksheetId': worksheetId, 'VrAgeTitle': ageTitle, 'sourceType': sourceType
		},
		//dataType: "json",
		success: function (data) {
			window.location.href = 'https://printlearncenter.com/umbraco/Surface/WorkSheet/DownloadData?vrPdfFile=' + pdfFile + '&vrWorksheetId=' + worksheetId + '&VrAgeTitle=' + ageTitle + '&sourceType=' + sourceType;
		}
	});
}


$('#subscribeOpener').on('click', function () {
	$("#loaderpop").css('display', 'block');
	var CultureName = $('#currentCultureName').val().trim();
	$('#plnspp-Mnpp').show();
	$('#plnspp-Mnpp-data').load("/umbraco/Surface/Home/GetMySubscription", { currentCultureName: CultureName }, function (responseTxt, statusTxt, xhr) {
		if (statusTxt == "success") {
			$("#loaderpop").css('display', 'none');
		}
		if (statusTxt == "error") {
			$("#loaderpop").css('display', 'none');
		}
	});
	return false;
});



function openSubscribedData() {
	var currentCultureName = $('#currentCultureName').val().trim();

	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		url: "/umbraco/Surface/Home/GetMySubscription",
		data: { "currentCultureName": currentCultureName },
		success: function (e) {

			if (e.status == "Success") {
				window.location = e.navigation;
			}
		},
		error: function (error) {

		}
	});
}


function WonkBanner(isWonkBanner, wonkMessage, wonkUrl, redirectUrl, redirectTarget, isEnableTracker, pageName, title, subtitle) {

	if (isWonkBanner == "True" && ((wonkUrl != null) || (wonkUrl != '') || (wonkUrl != ' ') || (wonkUrl != 'undefined'))) {

		Swal.fire({
			text: wonkMessage,
			showCancelButton: true,
			confirmButtonColor: '#1c2e4a',
			cancelButtonColor: '#7e7e7e',
			confirmButtonText: 'OK'
		}).then(function (result) {

			if (result.isConfirmed) {

				if (isEnableTracker == "Yes") {
					try {
						addDownloadDataLayer(title, subtitle, pageName);
					}
					catch (ex) {
						//console.log('error');
					}
				}
				var a = document.createElement('a');
				a.setAttribute("href", wonkUrl);
				a.target = "_blank";
				//a.href = wonkUrl.trim();
				a.click();
			}
		});
	}
	else if (redirectUrl != null || redirectUrl != '' || redirectUrl != 'undefined') {
		if (isEnableTracker == "Yes") {
			try {
				addDownloadDataLayer(title, subtitle, pageName);
			}
			catch (ex) {
				//console.log('error');
			}
		}
		var a = document.createElement('a');
		a.target = redirectTarget;
		a.href = redirectUrl;
		a.click();
	}

	else if (isEnableTracker == "Yes") {
		try {
			addDownloadDataLayer(title, subtitle, pageName);
		}
		catch (ex) {
			//console.log('error');
		}
	}
}



function countdown(maskingTitle) {

	var maskingTitle = $('#maskingTitle').val();

	if ((maskingTitle != null || maskingTitle != "") && maskingTitle != 'undefined') {
		var timer2 = "3:00";
		var interval = setInterval(function () {


			var timer = timer2.split(':');
			//by parsing integer, I avoid all extra string processing
			var minutes = parseInt(timer[0], 10);
			var seconds = parseInt(timer[1], 10);
			--seconds;
			minutes = (seconds < 0) ? --minutes : minutes;
			if (minutes < 0) clearInterval(interval);
			seconds = (seconds < 0) ? 59 : seconds;
			seconds = (seconds < 10) ? '0' + seconds : seconds;
			//minutes = (minutes < 10) ?  minutes : minutes;
			$('#timer').html(maskingTitle + ' <span style="color:#000000"> ' + minutes + ' min. ' + seconds + ' sec. </span>');
			timer2 = minutes + ':' + seconds;
			if (minutes === -1) {
				clearInterval(timer);
				$("#resendotp").show();
				$("#timerdiv").hide();
				$(".error").hide();
			}
			else {
				$("#resendotp").hide();
				$("#timerdiv").show();
			}
		}, 1000);
	}
	else {
		$("#resendotp").show();
		$("#timerdiv").hide();
	}
}


function countdownMaxFailedRoot(timer, msg) {
	if (timer != null || timer != "") {
		var min = timer;
		var minutes = 0;
		var seconds = 0;
		var timer2 = min;

		var interval = setInterval(function () {
			var timer = timer2.split(':');
			//by parsing integer, I avoid all extra string processing
			if (timer[0]) {
				minutes = parseInt(timer[0], 10);
			}
			if (timer[1]) {
				seconds = parseInt(timer[1], 10);
			}
			--seconds;
			minutes = (seconds < 0) ? --minutes : minutes;
			if (minutes < 0) clearInterval(interval);
			seconds = (seconds < 0) ? 59 : seconds;
			seconds = (seconds < 10) ? '0' + seconds : seconds;
			//minutes = (minutes < 10) ?  minutes : minutes;
			if (minutes >= 0 && seconds >= 0) {
				$('#Msg').html(msg + "<span style='color:#000000'>" + ' ' + minutes + ' min. ' + seconds + ' sec.</span>');
			}
			else {
				$('#Msg').html(msg + "<span style='color:#000000'>" + ' ' + 2 + ' min. ' + 0 + ' sec.</span>');
			}
			timer2 = minutes + ':' + seconds;
			if (minutes === -1) {
				clearInterval(timer);
				$("#Msg").hide();
				$('#Registration').attr("disabled", false);
				$('#NextBtn').show();
			}
			else {
				$("#Msg").show();
				$('#Registration').attr("disabled", true);
				$('#NextBtn').hide();
			}
		}, 1000);
	}
	else {
		$("#resendotp").show();
		$("#timerdiv").hide();
	}
}


function countdownMaxFailed() {
	$("#OtpBase").hide();
	$("#countdown").show();
	$(".error").hide();
	var countdown = $("#countdown").countdown360({
		radius: 60,
		seconds: 180,
		fontColor: '#FFFFFF',
		fontSize: 40,
		autostart: false,
		onComplete: function () {
			$("#OtpBase").show();
			$("#countdown").hide();
			ClearOtp();
			$('.styledisable').attr("disabled", true);
		}
	});
	countdown.start();
}

function validateEmail(email) {
	var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
	if (!filter.test(email))
		return false;
	else
		return true;
}


function validateMobile(mobile) {
	var filter = /^[0-9]{10}$/
	if (!filter.test(mobile))
		return false;
	else
		return true;
}

function validatePassword(password) {
	//var filter = /^(?=.*[0-9])(?=.*[A-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,8}$/;
	var filter = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@#$!%*?&#.)(_-])[A-Za-z\d@$!%*?&#.)(_-]{8,}$/
	if (!filter.test(password))
		return false;
	else
		return true;
}

function showPass(fieldName) {
	var x = document.getElementById(fieldName);
	if (x.type === "password") {
		x.type = "text";
	} else {
		x.type = "password";
	}
}

function isNumberKey(evt) {

	var charCode = (evt.which) ? evt.which : event.keyCode
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}

	return true;
}

function gotobottomfaq() {
	$('html, body').animate({ scrollTop: $(".faqbox-row").offset().top - 300 }, 500);
}

function WorksheetReset() {
	commonFilterLayer("Reset Button", 'Worksheet');
	window.location.reload();
}

function VideosReset() {
	commonFilterLayer("Reset Button", 'Videos');
	window.location.reload();
}

function PrintWorkSheet(vPath) {
	$("#loader").css('display', 'block');
	//alert(vPath);
	//var form = $('#__AjaxAntiForgeryForm');
	//var token = $('input[name="__RequestVerificationToken"]', form).val();
	var vTemp = vPath.split('$');
	var WorkSheetId = vTemp[0];
	var vFrom = vTemp[1];
	var IsPaid = null;
	if (vTemp[2] != undefined && vTemp[2] != null) {
		if (vTemp[2] == "Paid") {
			IsPaid = true;
		}
		if (vTemp[2] == "Free") {
			IsPaid = false;
		}
	}
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/WorkSheet/WorksheetPrint",
		data: { 'WorkSheetId': WorkSheetId, 'source': vFrom, 'Type': 'Print', 'IsPaid': IsPaid },
		dataType: "TEXT",
		success: async function (data) {
			//console.log(data);
			//
			const isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent); if (isMobile) {
				const base64Response = await fetch("data:application/pdf;base64," + data);
				const blob = await base64Response.blob();
				const blobUrl = URL.createObjectURL(blob);
				console.log(blobUrl);
				window.open(blobUrl, "_blank");
			}
			else { printJS({ printable: data, type: 'pdf', base64: true });}

		},
		error: function (error) {
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
}


function GetVimeoVideoUrl(videoId, type) {
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Video/GetVideoUrl",
		data: { 'videoId': videoId, 'type': type },
		dataType: "json",
		success: function (data) {
			console.log(data.VimeoUrl);
		}
	});
}

function ResendOtp() {
	$('.error').hide();
	$("#loader").css('display', 'block');

	var token = $('input[name="__RequestVerificationToken"]').val();

	$.ajax({
		type: "POST",
		//contentType: "application/json",
		//dataType: "JSON",
		url: "/umbraco/Surface/Home/ResendOtp",
		data: { __RequestVerificationToken: token },
		success: function (e) {
			if (e.status === "Success") {

				$("#MsgOtp").show();
				$("#MsgOtp").html("OTP has been sent.");

				//timer
				countdown();

				commonLayer(e.page, "Resent OTP");
			}
			else if (e.status == "Exceed") {
				var msg = $("#oTPAttemptMaximumLimit").val();
				$("#MsgOtp").show();
				$("#MsgOtp").html(msg);
			}
			else if (e.status == "MAXFATT") {

				var msg = $('#validateAttemptTitle').val();
				$("#timerMsg").html(msg);
				countdownMaxFailed();

				return false;
			}
			else if (e.status == "MAXRATT") {//Resend Case

				var msgg = $('#resendOtpTimerTitle').val();
				$("#timerMsg").html(msgg);
				countdownMaxFailed();

				return false;
			}
			else if (e.status == "Fail") {
				$("#MsgOtp").show();
				//$("#MsgOtp").html("Otp Sent Issue!!");
			}
		},
		error: function (error) {
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
}

function SignOut(logouttype) {

	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Home/Logout",
		data: { "logOutSource": logouttype },
		success: function (e) {
			if (e.status == "Success") {
				try {
					commonLayer("My Account", "User Profile Icon Logout");
				}
				catch (ex) { //console.log(ex);
				}
				window.location = e.navigation;
			}
		},
		error: function (error) {

		}
	});
}


function SignOutSpecialPlan(logouttype) {

	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Home/Logout",
		data: { "logOutSource": logouttype },
		success: function (e) {
			if (e.status == "Success") {
				try {
					commonLayer("learn-365", "User Profile Icon Logout");
				}
				catch (ex) { //console.log(ex);
				}
				window.location = "/learn-365";
			}
		},
		error: function (error) {

		}
	});
}


function SignOutBonusPlan(logouttype) {

	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Home/Logout",
		data: { "logOutSource": logouttype },
		success: function (e) {
			if (e.status == "Success") {
				try {
					commonLayer("Bonus", "User Profile Icon Logout");
				}
				catch (ex) { //console.log(ex);
				}
				window.location = "/structure-program";
			}
		},
		error: function (error) {

		}
	});
}



function IsBundlingUser(evet) {

	var IsBundleUser = $("#IsBundleUser").val();
	var pageName = $("#pageName").val();
	if (IsBundleUser == "YES") {

		$("#bundlingDataBind").load("/umbraco/Surface/Home/GetBundlingPopup", function (responseTxt, statusTxt, xhr) {
			if (statusTxt == "success") {
				$("#bundlingPopup").show();
				if (evet == 'click') {

					//	//HP Coupon Bundling Tracker
					commonLayer(pageName, "Claim your offer popup");

				}
			}
		});

	}
}


function get_query() {
	var vars = [], hash;
	var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
	for (var i = 0; i < hashes.length; i++) {
		hash = hashes[i].split('?');
		vars.push(hash[0]);
		vars[hash[0]] = hash[1];
	}
	return vars;
}


function getUrlVars() {
	var vars = [], hash;
	var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
	for (var i = 0; i < hashes.length; i++) {
		hash = hashes[i].split('=');
		vars.push(hash[0]);
		vars[hash[0]] = hash[1];
	}
	//console.log(vars)
	return vars;
}



function slicjFunction() {

	$('.video-thum-slider').not('.slick-initialized').slick({
		dots: false,
		infinite: false,
		arrows: true,
		speed: 300,
		slidesToShow: 3,
		slidesToScroll: 3,
		responsive: [
			{
				breakpoint: 1199,
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
					slidesToShow: 2.3,
					slidesToScroll: 2
				}
			},
			{
				breakpoint: 480,
				settings: {
					arrows: false,
					slidesToShow: 2.1,
					slidesToScroll: 1
				}
			}

		]
	});

	//var bLazy = new Blazy();
	var bLazy = new Blazy({
		selector: '.b-lazy',
		loadInvisible: true,
		offset: 200
	});

	$('.video-thum-slider').on('afterChange', function (event, slick, currentSlide, nextSlide) {
		var imagesToLoad = document.querySelectorAll('.b-lazy');
		bLazy.revalidate();
		bLazy.load(imagesToLoad);
	});
}


function slicj4Function() {

	$('.video-thum-slider').not('.slick-initialized').slick({
		dots: false,
		infinite: false,
		arrows: true,
		speed: 300,
		slidesToShow: 4,
		slidesToScroll: 4,
		responsive: [
			{
				breakpoint: 1199,
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
					slidesToShow: 2.3,
					slidesToScroll: 2
				}
			},
			{
				breakpoint: 480,
				settings: {
					arrows: false,
					slidesToShow: 2.1,
					slidesToScroll: 1
				}
			}

		]
	});


	var bLazy = new Blazy({
		selector: '.b-lazy',
		loadInvisible: true,
		offset: 200
	});

	$('.video-thum-slider').on('afterChange', function (event, slick, currentSlide, nextSlide) {
		var imagesToLoad = document.querySelectorAll('.b-lazy');
		bLazy.revalidate();
		bLazy.load(imagesToLoad);
	});
}


$(".otpfield").keyup(function () {
	if (this.value.length == this.maxLength) {
		$(this).next('.otpfield').focus();
	}
	else {
		$(this).prev('.otpfield').focus();
	}

	var otp1 = $("#digit-1").val();
	var otp2 = $("#digit-2").val();
	var otp3 = $("#digit-3").val();
	var otp4 = $("#digit-4").val();

	var Otp = otp1.concat(otp2, otp3, otp4);

	if ((Otp != null || Otp != "") && Otp.length == 4) {
		$('#VerifyOtp').attr("disabled", false);
	}
	else {
		$('#VerifyOtp').attr("disabled", true);
	}

	if ((Otp != null || Otp != "") && Otp.length > 0) {
		$('#clear').show();
	}
	else {
		$('#clear').hide();
	}
});


$(".otpfield").keypress(function () {
	if ($(this).val().length == $(this).attr("maxlength")) {
		return false;
	}
});


$(".otpspecialplanfield").keypress(function () {
	if ($(this).val().length == $(this).attr("maxlength")) {
		return false;
	}
});

function ClearOtp() {
	$("#otp-input input").val("");
	$('.styledisable').attr("disabled", true);
	$(".error").hide();
	$(".clearhide").hide();
}
$(".close-pb").click(function () {
	$(".overlayBanner").hide();
});

$(".payClose").click(function () {
	try {
		commonLayer("Bonus", "Download Popup Close click");
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlaySD").hide();
});
$(".payCloselgn").click(function () {
	try {
		commonLayer("Bonus", "Download Popup Close click");
	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlaySD").hide();

	location.href = window.location.href;
});


$(".payCloseSpecialOffer").click(function () {
	try {
		commonLayer("Special Offer", "Download Popup Close click");
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlaySD").hide();
});
$(".payCloselgnSpecialOffer").click(function () {
	try {
		commonLayer("Special Offer", "Download Popup Close click");
	}
	catch (ex) {
		//console.log('error');
	}
	$(".overlaySD").hide();

	location.href = window.location.href;
});
$(".viewpricing").click(function () {
	try {
		commonLayer("Bonus", "Explore Plan");
	}
	catch (ex) {
		//console.log('error');
	}

	window.location = "/subscription";
});

$(".exploreworksheetredirect").click(function () {
	try {
		commonLayer("Bonus", "Explore Worksheet");
	}
	catch (ex) {
		//console.log('error');
	}

	window.location = "/";
});

function DontShowAgain(couponCode) {

	var token = $('input[name="__RequestVerificationToken"]').val();
	$.ajax({
		type: "POST",
		//contentType: "application/json; charset=utf-8",
		//dataType: "JSON",
		url: "/umbraco/Surface/Home/DontShowAgain",
		data: { __RequestVerificationToken: token, "CouponCode": couponCode },
		success: function (e) {
			if (e.status == "Success") {
				try {
					commonLayer("DontShowAgain", "Dont Show Again");
				}
				catch (ex) { //console.log(ex);
				}
				$(".overlayBanner").hide();
			}
		},
		error: function (error) {

		}
	});
}

function GotoSubscription() {
	try {
		commonLayer("Upgrade Now Popup", "Clicked on UpgradeNow");
	}
	catch (ex) { //console.log(ex);
	}
}


function TeachersProgramReset(redirectUrl) {
	window.location.href = redirectUrl;
}


//function SubscriptionChange(detailsData, subscribeToDownload, SubscriptionUrl, pageName) {

//	try {
//		addSubscribeDataLayerWithoutRedirection(detailsData, subscribeToDownload, SubscriptionUrl, pageName);
//	}
//	catch {
//		//console.log('');
//	}

//	Swal.fire({
//		//title: e.message,
//		html: 'Subscription program Change message',
//		//icon: 'success',
//		showCancelButton: true,
//		confirmButtonColor: '#3085d6',
//		cancelButtonColor: '#d33',
//		confirmButtonText: 'Yes'
//	}).then((result) => {
//		if (result.isConfirmed) {
//			window.location = '/subscription-structure';
//		}
//		//else {
//		//	window.location.reload();
//		//}
//	});
//}
