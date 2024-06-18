$(document).ready(function () {
	//OTPInput();
	
	try {
		//Content menu selection
		var path = location.pathname.length ? location.pathname : window.location.href;
		
		if (path) {
			var pathArray;
			var lastsegment;
			if (path === "/" || path === "") {
				lastsegment = "/";
			}
			else {
				pathArray = path.split('/');
				lastsegment = pathArray[pathArray.length - 1];

				if (!lastsegment) {
					if (pathArray.length > 3) {
						lastsegment = pathArray[pathArray.length - 3];
					}
					else {
						lastsegment = pathArray[pathArray.length - 2];
					}
				}
			}

			if (lastsegment) {
				if (window.location.href.indexOf('?') > 0) {
					var param = window.location.href.slice(window.location.href.indexOf('?') + 1);
					lastsegment = lastsegment + '/?' + param;
				}

				$('#navigation a').removeClass('active');
				if (lastsegment !== '' && location.pathname.length > 1) {
					var segmentCount = $("#navigation").find('a[href*="' + lastsegment + '"]').length;
					if (segmentCount > 1) {
						$("#navigation a").first().addClass("active");
					}
					else {
						if ($("#navigation").find('a[href*="' + lastsegment + '"]').length == 0) {
							$("#navigation a").first().addClass("active");
						} else {
							$("#navigation").find('a[href*="' + lastsegment + '"]').addClass("active");
						}
					}
				}
				else {
					$("#navigation a").first().addClass("active");
				}

				if (window.location.href.toLowerCase().indexOf('#tab') > 0) {
					//$('html, body').animate({ scrollTop: $("#navigation").offset().top }, 1500);
					$('html, body').animate({ scrollTop: $("#navigation").offset().top - 150 }, 500);
				}
			}
		}
	}
	catch (err)
	{
		//console.log("");
	}
});


function share() {

	$(".aFBShare").click(function () {
		var ItemVal = $(this).find('span').html();
		publish(ItemVal);
	});
	$(".aWHTAppSH").click(function () {
		var ItemVal = $(this).find('span').html();
		if (/Mobi/.test(navigator.userAgent)) {
			window.open('whatsapp://send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
		else {
			window.open('https://web.whatsapp.com/send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
	});
	$(".aMailSh").click(function () {
		var FullString = $(this).find('span').html();
		var emailBody = FullString.split('`')[0];
		var email = '';
		var subject = FullString.split('`')[1];
		//var mailLink = "<a href='https://google.com'>Send Mail</a>";
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
		$("#copylinkId").show();
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
			//alert('del');
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

//$('#plnspp-close').click(function () {
//	//$('.ovrlyScin').hide();
//});
//$('#plnspp-close').on('click', function () {
//	$('#plnspp-Mnpp').hide();
//});
//$('.whtBx.plnspp').on('click', function (e) {
//	e.stopPropagation();
//});
//$('#plnspp-Mnpp').on('click', function () {
//	$('#plnspp-Mnpp').hide();
//});


//$("#subscribeOpener").click(function () {
//	openSubscribedData();
//});


function openSubscribedData() {
	//$("#dialog").hide();
	//$(".ui-dialog").hide();
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
			//else {
			//	//$('#dialog').html(e);
			//	//$('#dialog').dialog('open');
			//	//$("#dialog").show();
			//	//$(".ui-dialog").show();

			//}
		},
		error: function (error) {

		}
	});
}


//$(".faqs-acrd h5").click(function () {
//	$(this).next(".acrdCont").slideToggle("slow")
//		.siblings(".acrdCont:visible").slideUp("slow");
//	$(this).toggleClass("active");
//	$(this).siblings(".faqs-acrd h5").removeClass("active");
//});

function WonkBanner(isWonkBanner, wonkMessage, wonkUrl, redirectUrl, redirectTarget, isEnableTracker, pageName, title, subtitle) {
	debugger
	if (isWonkBanner == "True" && (wonkUrl != null || wonkUrl != '' || wonkUrl != ' ' || wonkUrl != 'undefined')) {
		//alert(wonkUrl);
		Swal.fire({
			text: wonkMessage,
			showCancelButton: true,
			confirmButtonColor: '#1c2e4a',
			cancelButtonColor: '#7e7e7e',
			confirmButtonText: 'OK'
		}).then((result) => {
			if (result.isConfirmed) {
				//alert(wonkUrl);
				//window.location.href = wonkUrl;
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
	
	if (isEnableTracker == "Yes") {
		try {
			addDownloadDataLayer(title, subtitle, pageName);
		}
		catch (ex) {
			//console.log('error');
		}
	}
}


var timer;
function countdown() {
	var sec = 20;
	if (timer) clearInterval(timer);
	timer = setInterval(function () {
		var elem = document.getElementById('timer');
		elem.innerHTML = ' in ' + sec-- + ' sec.';
		if (sec === -1) {
			clearInterval(timer);
			$("#resendotp").removeClass("disabled");
			$("#timer").hide();
		}
		else {
			$("#resendotp").addClass("disabled");
			$("#timer").show();
		}
	}, 1000);
}


function validateEmail(email) {
	var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
	if (!filter.test(email))
		return false;
	else
		return true;
}

//$('.otpmob').on('keyup', function () {
//	$("#eMsg").hide();
//	$("#eOtpMsg").hide();
//	var otpfirst = $('#Otpdigit1').val();
//	var otpscnd = $('#Otpdigit2').val();
//	var otpthrd = $('#Otpdigit3').val();
//	var otpfrth = $('#Otpdigit4').val();
//	var otpfth = $('#Otpdigit5').val();
//	var otpsxth = $('#Otpdigit6').val();

//	if (otpfirst && otpscnd && otpthrd && otpfrth && otpfth && otpsxth) {

//		$("#btnRegistrationOtp").removeClass("disabled");
//	}
//	else {

//		$("#btnRegistrationOtp").addClass("disabled");
//	}
//});

function gotobottomfaq() {
	$('html, body').animate({ scrollTop: $(".faqbox-row").offset().top - 300 }, 2000);
}

function WorksheetReset() {
	commonFilterLayer("Reset Button", 'Worksheet');
	window.location.reload();
}
function VideosReset() {
	commonFilterLayer("Reset Button", 'Videos');
	window.location.reload();
}
//function OTPInput() {
//	const inputs = document.querySelectorAll('#Otp > *[id]');
//	for (let i = 0; i < inputs.length; i++) {
//		inputs[i].addEventListener('keydown', function (event) {
//			if (event.key === "Backspace") {
//				inputs[i].value = '';
//				if (i !== 0)
//					inputs[i - 1].focus();
//			} else {
//				if (i === inputs.length - 1 && inputs[i].value !== '') {
//					return true;
//				} else if (event.keyCode > 47 && event.keyCode < 58) {
//					inputs[i].value = event.key;
//					if (i !== inputs.length - 1)
//						inputs[i + 1].focus();
//					event.preventDefault();
//				} else if (event.keyCode > 64 && event.keyCode < 91) {
//					inputs[i].value = String.fromCharCode(event.keyCode);
//					if (i !== inputs.length - 1)
//						inputs[i + 1].focus();
//					event.preventDefault();
//				}
//			}
//		});
//	}
//}
