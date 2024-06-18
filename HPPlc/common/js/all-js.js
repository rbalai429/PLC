$(document).ready(function () {

	$(".hd-age").click(function () {
		$(".age-option").slideUp();
		$(".hd-age").removeClass("active-hd");
		$(this).parent().find(".age-option").slideDown();
		$(this).addClass("active-hd");
	});

	$(".otp-inputs").keyup(function () {
		if (this.value.length == this.maxLength) {
			$(this).next('.otp-inputs').focus();
		}
	});

	$(".bannerPopupCont").click(function (e) {
		e.stopPropagation();
	});

	$(document).click(function () {
		$(".overlayBanner").hide();
	});


	$('.NavPlc').show();

	$(".clsBtn").click(function () {
		$(".overlayFltr").hide();
		$(".leftFltrNew").removeClass("openFltr");
	});

	//$(".chng-plan").click(function () {
	//	$(".chng-planbox").show();
	//});
	//$(".close-plan").click(function () {
	//	$(".chng-planbox").hide();
	//});

	if ($('.header .topband').length > 0) {
		$('.wrapper').addClass('topbandspec');
	};

	$(".gotobtm").click(function () {
		$('html, body').animate({ scrollTop: $(".faqbox-row").offset().top - 300 }, 2000);
	});

	$(".fltrMob").click(function () {
		$(".leftFltrNew").addClass("openFltr");
		$(".overlayFltr").show();
	});
	$(".overlayFltr").click(function () {
		$(this).hide();
		$(".leftFltrNew").removeClass("openFltr");

	});

	$(".ftrArrow").click(function () {
		$(this).toggleClass("arrowAtive");
		$(".ftrFixd").toggleClass("openFtr");
	});

	$(window).scroll(function () {
		if ($(document).scrollTop() > 280) {
			$(".srchBar").addClass("srchBarFixed");
		} else {
			$(".srchBar").removeClass("srchBarFixed");
		}
	});
	//$(".hlp-cntr span").click(function () {
	//	$(".ddhlp-cnt").slideToggle();
	//});


	$(".pckge").click(function () {
		$(".choosePckge").removeClass("activePckge");
		$(this).find(".choosePckge").addClass("activePckge");
	});


	$(".pckgeTab li a").click(function () {
		$(".pckgeTab li a").removeClass("pckgeTabActive");
		$(this).addClass("pckgeTabActive");
		$(".pckgeTabCont").hide();
		var activeTab = $(this).attr("href");
		$(activeTab).show();
		return false;
	});

	$('.slider4item').slick({
		dots: false,
		infinite: false,
		speed: 300,
		slidesToShow: 4,
		slidesToScroll: 1,
		responsive: [
			{
				breakpoint: 1023,
				settings: {
					arrows: false,
					slidesToShow: 4,
					slidesToScroll: 1

				}
			},
			{
				breakpoint: 767,
				settings: {
					arrows: true,
					slidesToShow: 2.3,
					slidesToScroll: 1
				}
			},
			{
				breakpoint: 480,
				settings: {
					arrows: true,
					slidesToShow: 2.3,
					slidesToScroll: 1
				}
			}

		]
	});

	$('.mmbrSPk-slidr').slick({
		dots: true,
		arrows: false,
		infinite: false,
		speed: 300,
		slidesToShow: 1,
		slidesToScroll: 1,

	});



	mediaCheck({
		media: '(max-width: 767px)',
		entry: function () {

			$('.philoshyMnin').slick({
				dots: true,
				arrows: true,
				infinite: false,
				speed: 300,
				slidesToShow: 1.2,
				slidesToScroll: 1,
			});

		},
		exit: function () {

		}
	});


	$(".readmr-link").click(function (e) {
		e.preventDefault();
		var hashtag = $(this).attr("href");
		$(hashtag).parent('.eductnl-prson-pp').css("display", "flex");
		$(hashtag).show();
	});
	$('.closePrsonpp').click(function (e) {
		e.preventDefault();
		$('.eductnl-prson-pp').hide();
		$('.whiteBxpp').hide();
	});

	$('.slider-banner').slick({
		dots: true,
		infinite: false,
		autoplay: false,
		speed: 400,
		slidesToShow: 1
	});




	$('.slider-banner').on('afterChange', function (event, slick, currentSlide, nextSlide) {
		var imagesToLoad = document.querySelectorAll('.b-lazy');
		bLazy.revalidate();
		bLazy.load(imagesToLoad);
	});

	$(".language-slct").click(function () {
		$(".language-overlay").show();
	});
	$(".language-drop .hd-title a, .language-overlay").click(function () {
		$(".language-overlay").hide();
	});

	$(".age-slct").click(function () {
		$(".age-overlay").show();
	});
	$(".age-drop .hd-title a, .age-overlay").click(function () {
		$(".age-overlay").hide();
	});

	$('.multiple-select').multipleSelect({
		placeholder: 'Select Age',
		selectAll: false
	});

	//$('.select-age').multipleSelect({
	//	placeholder: 'Select Age',
	//	selectAll: false
	//});

	//$('.select-category').multipleSelect({
	//	placeholder: 'Select Category',
	//	selectAll: false

	//});
	//$('.select-week').multipleSelect({
	//	placeholder: 'Select Weeks',
	//	selectAll: false

	//});

	$(".fa-eye").click(function () {
		$(this).toggleClass("eye-active");
	});

	//fltr js	
	$(".mob-fliter").click(function () {
		$(".fltr-overlay").toggle();
		$(".fltr-top").toggleClass("open");
		$(".fltr-col").toggleClass("open");
		$(".title-fltr .reset-btn").toggleClass("open");
	});

	$(".mob-fliter-video").click(function () {
		$(".fltr-overlay").toggle();
		$(".fltr-top").toggleClass("open");
		$(".fltr-col").toggleClass("open");
		$(".title-fltr .reset-btn").toggleClass("open");
	});
	$(".mob-fliter-2").click(function () {
		$(".fltr-overlay").toggle();
		$(".fltr-btm").toggleClass("open");
	});


	$(".fltr-overlay").click(function () {
		$(this).hide();
		$(".fltr-col").removeClass("open");
		$(".title-fltr .reset-btn").removeClass("open");
	});

	$(".fltr-close").click(function () {
		$(".fltr-overlay").hide();
		$(".fltr-col").removeClass("open");
	});

	$(".fltr-col-close").click(function () {
		$(".fltr-overlay").hide();
		$(".fltr-col").removeClass("open");
		$(".title-fltr .reset-btn").removeClass("open");
	});



	//language active js	
	$(".language-drop li a").click(function (e) {
		e.preventDefault();
		$(".language-drop li a").removeClass("active");
		$(this).addClass("active");
	});

	$(".signin-drop").hide();
	if (window.innerWidth > 1199) {
		$(".sinin").mouseenter(function () {
			$(".signin-drop").show();
		});
		$(".sinin").mouseleave(function () {
			$(".signin-drop").hide();
		});
	}
	$(".signin-arrow").click(function () {
		$(".sinin").toggleClass("active");
		$(".signin-drop").toggle();
	});
	//$(this).bind('mouseup touchend', function (open_list) {
	//	if (!($(open_list.target).parent('.signin-drop').length > 0)) {
	//		$('.signin-drop').hide();
	//	}
	//});

	$(document).on('click', function (event) {
		if (!$(event.target).parents().addBack().is('.signin-arrow')) {
			$('.signin-drop').hide();
		}
	});


	$('.signin-drop').on('click', function (event) {
		event.stopPropagation();
	});

	//home tab active js	
	//$(".tab-hm li a").click(function(e){
	// e.preventDefault();	
	//$(".tab-hm li a").removeClass("active");	
	//$(this).addClass("active");		
	//});

	//$(".duc-top").click(function () {
	//	$(".couponbox").slideToggle();
	//	$(this).toggleClass("active");
	//});

	var bLazy = new Blazy({
		container: '.video-thum-row'
	});



	var modelValue = $(".open-model").attr("href");
	$(".open-model").click(function (e) {
		e.preventDefault();
		$(modelValue).show();
	});

	$(".close-model").click(function () {
		$(modelValue).hide();
	});



	$(".scrollto_top").click(function () {
		$('html, body').animate({ scrollTop: $(".main-wrp").offset().top }, 1500);
	});

	


	//Media check js start	
	mediaCheck({
		media: '(max-width: 1023px)',
		entry: function () {
			$(window).bind('scroll', function () {

				if ($(this).scrollTop() > 350) {
					//$(".mob-fliter").fadeIn("slow");
				}


				var $fixednav = $('.header-subscribe');
				if ($(".ofrlist").length > 0) {
					if ($fixednav.offset().top + $fixednav.outerHeight() > $('.ofrlist').offset().top - 400) {
						$('.mob-fliter').hide();
						$('.mob-fliter-2').show();
					}
					else {
						$('.mob-fliter').show();
						$('.mob-fliter-2').hide();
					}
				}

				if ($(this).scrollTop() > 350) {
					$(".mob-fliter-video").fadeIn("slow");
				}
				else {
					$(".mob-fliter-video").fadeOut("slow");
				}
			});
		},
		exit: function () {
			$(".mob-fliter-video").fadeOut("slow");
		}

	});



	//Media check js end



	/* add New Js */
	if ($('.selectFrm').length > 0) {
		$(".js-example-tokenizer").select2({
			tags: true,
			minimumResultsForSearch: Infinity
		});
	}

	$(".srcField").focus(function () {
		$(".SerchContnt").show();
	});
	$(".SerchContnt").focus(function () {
		$(".SerchContnt").show();
	});

	$(document).on('click', function (event) {
		if (!$(event.target).parents().addBack().is('.srcField, .SerchContnt')) {
			$(".SerchContnt").hide();
		}
	});

	$(".srcField, .SerchContnt").on('focus', function (event) {
		event.stopPropagation();
	});


	$(".header .srchIcon").click(function () {
		$(".srchBar").show();
	});
	$(".srchBx .clsBtn").click(function () {
		$(".srchBar").hide();
	});
	$('.headerMMM').click(function () {
		$(".NavPlc").addClass("menuSlide");
		$(".ovrLaySkin").addClass("show-overlay-skin");
	});
	$('.NavPlc .clsBtn').click(function () {
		$(".NavPlc").removeClass("menuSlide");
		$(".ovrLaySkin").removeClass("show-overlay-skin");
	});

	$(".NavPlc ul ul").find("ul").parent("li").addClass("ChiledLavls");
	$('.NavPlc ul ul').prepend("<li class='backMM'></li>");
	$('.NavPlc ul').parent("li").prepend("<em></em>");

	// $(".NavPlc li > ul").wrap('<div class="secsroll"><div class="scrollin"></div></div>');

	$('.NavPlc li em').click(function () {
		$(this).next('a').next('ul').addClass("slideMnu");
		$(this).next('a').next('ul').find('.backMM').text($(this).next('a').text())
		$('.backMM').prepend("<b></b>");
		var dataId = $(this).parent("li").attr("class");
		$(this).next('a').next('ul').find('.backMM').attr("class", "backMM " + dataId)
	});

	$('.backMM').click(function () {
		$(this).parent("ul").removeClass("slideMnu");
	});

	$(".chchlist .chkBxItem label").on("change", function () {
		$(this).toggleClass("BoldTxt");

	});

	$('.aFilterReset').clone().appendTo(".filtrActn");


	//$('.video-thum-slider').slick({
	//	  dots: false,
	//	  infinite: false,
	//			arrows:true,
	//	  speed: 300,
	//	  slidesToShow: 3,
	//	  slidesToScroll:3,
	//	  responsive: [
	//		{
	//		  breakpoint: 1199,
	//		  settings: {
	//	arrows:false,
	//			slidesToShow: 3.1,
	//			slidesToScroll: 3

	//		  }
	//		},
	//		{
	//		  breakpoint: 767,
	//		  settings: {
	//	arrows:false,
	//			slidesToShow: 2.3,
	//			slidesToScroll: 2
	//		  }
	//		},
	//		{
	//		  breakpoint: 480,
	//		  settings: {
	//			arrows:false,
	//			slidesToShow: 2.1,
	//			slidesToScroll: 1
	//		  }
	//		}

	//	  ]
	//	});

	$('.crslItm4').slick({
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

	$('.ctgryListSld').slick({
		dots: false,
		infinite: false,
		variableWidth: true,
		arrows: true,
		speed: 300,
		slidesToShow: 10,
		slidesToScroll: 1,
		responsive: [
			{
				breakpoint: 1199,
				settings: {
					slidesToShow: 8,
					slidesToScroll: 3.5

				}
			},
			{
				breakpoint: 767,
				settings: {
					slidesToShow: 3.2,
					slidesToScroll: 1
				}
			},
			{
				breakpoint: 480,
				settings: {
					slidesToShow: 3.2,
					slidesToScroll: 1
				}
			}

		]
	});


	$(".vwAll").click(function () {
		$(".leftFltr").toggleClass("slideFltr");
	});

	$('.chchlist').prepend("<div class='backFltr'></div>");

	//Media check js start	
	mediaCheck({
		media: '(max-width: 1199px)',
		entry: function () {
			$('.acrdHd').click(function () {
				$(this).next('.chchlist').addClass("listFltr");
				$(this).next('.chchlist').find('.backFltr').text($(this).text());
				$('.backFltr').prepend("<b></b>");
			});
			$('.backFltr').click(function () {
				$(this).parent(".chchlist").removeClass("listFltr");
			});
			$(".NavPlc .navigation").removeAttr("style");
		},
		exit: function () {
			$('.acrdHd').click(function () {
				$(this).next('.chchlist').slideToggle();
				$(this).toggleClass('active');
			});
			$(".NavPlc .navigation").removeAttr("style");
			$(".NavPlc li > ul").wrap('<div class="secsroll"><div class="scrollin"></div></div>');
		}

	});
	/* end add New Js */


	$(".faqbox h5:first").addClass("acc-active");
	$(".faqbox:first").show();
	$(".faqbox h5").click(function () {
		$(this).next(".faq-ans").slideDown("slow")
			.siblings(".faq-ans:visible").slideUp("slow");
		$(this).addClass("acc-active");
		$(this).siblings("h5").removeClass("acc-active");

	});

	//Changes for Index Code
	$(".NavPlcin ul li").each(function () {
		if ($(this).find('ul').length < 1) {
			$(this).addClass("noDD");
		}
	});


	if ($('.language-overlay').length > 0) {
		$(document).mouseup(function (e) {
			if (!$(e.target).is(".language-overlay")) {
				$('.language-overlay').hide();
			}
		});
	}

	//$(".openpdf").fancybox({
	//	'width': '90%',
	//	'height': '90%',
	//	'autoScale': false,
	//	'transitionIn': 'none',
	//	'transitionOut': 'none',
	//	'type': 'iframe'
	//});
	
	
	//$(".ms-choice, .ms-drop").click(function (e) {
	//	e.stopPropagation();
		
	//});

	//$(document).click(function () {
	//	$(".ms-drop").hide();
	//});
});


$(window).bind('scroll', function () {
	if ($(this).scrollTop() > 600) {
		$('.tab-row').addClass('fixed-tab');
		$('.title-fltr').addClass('fixed');
	}
	else {
		$('.tab-row').removeClass('fixed-tab');
		$('.title-fltr').removeClass('fixed');
	}

	

	if ($(this).scrollTop() > 600) {
		$('.fltr-top').addClass('fixed-fltr');
	}
	else {
		$('.fltr-top').removeClass('fixed-fltr');
	}

	var $fixednav = $('.header-subscribe');
	if ($(".ofrlist").length > 0) {
		if ($fixednav.offset().top + $fixednav.outerHeight() > $('.ofrlist').offset().top - 400) {
			$('.fltr-btm').addClass('fixed-fltr');
			$('.fltr-top').hide();
		}
		else {
			$('.fltr-btm').removeClass('fixed-fltr');
			$('.fltr-top').show();
		}
	}


	if ($(this).scrollTop() > 250) {
		$(".scrollto_top").fadeIn("slow");
	}
	else {
		$(".scrollto_top").fadeOut("slow");
	}
});

var lastScrollTop = 0;
$(window).scroll(function () {
	var scrollTop = $(this).scrollTop();
	if (scrollTop > lastScrollTop) {
		$('.floater-whatsapp').hide();
	} else {
		$('.floater-whatsapp').show();
	}
	lastScrollTop = scrollTop;
});

$(".faqs-acrd h5").click(function () {
	$(this).next(".acrdCont").slideToggle("slow")
		.siblings(".acrdCont:visible").slideUp("slow");
	$(this).toggleClass("active");
	$(this).siblings(".faqs-acrd h5").removeClass("active");
});

if ($('.language-text').length > 0) {
	if ($(window).width() <= 960) {
		var languageTrim = $('.language-text').text();
		var strtwo = languageTrim.substr(0, 2);
		$('.language-text').text(strtwo);
	}
}

$(".hlp-cntr span").click(function () {
	$(".ddhlp-cnt").slideToggle();
});

$(".rqst-call-bckBtn").click(function (e) {
	e.preventDefault();
	$(".getCall-bck").slideToggle();
	var hash = this.hash;
	$('html, body').animate({ scrollTop: $(hash).offset().top - 140 }, 900);
});

if ($(".faqMn").length > 0) {
	$('.ddhlp-cnt a').on('click', function (event) {
		event.preventDefault();
		var hash = this.hash;
		$('html, body').animate({ scrollTop: $(hash).offset().top }, 900);
		$(".ddhlp-cnt").slideToggle();
		return false;
	});
}
