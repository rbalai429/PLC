$(document).ready(function () {
	
	$(".hd-age").click(function () {
		$(this).toggleClass("active-hd");
		$(".age-option").slideToggle();
	});

	$(".otp-inputs").keyup(function () {
		if (this.value.length == this.maxLength) {
			$(this).next('.otp-inputs').focus();
		}
	});

	

	//$(".chng-plan").click(function () {
	//	$(".chng-planbox").show();
	//});
	//$(".close-plan").click(function () {
	//	$(".chng-planbox").hide();
	//});

	if($('.header .topband').length > 0) {
		$('.wrapper').addClass('topbandspec');
	};

	$(".gotobtm").click(function () {
		$('html, body').animate({ scrollTop: $(".faqbox-row").offset().top - 300 }, 2000);
	});

	//$(".hlp-cntr span").click(function () {
	//	$(".ddhlp-cnt").slideToggle();
	//});

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

	$(".signin-arrow").click(function () {
		$(".signin-drop").show();
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
					$(".mob-fliter").fadeIn("slow");
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

				//if($(this).scrollTop() > 350){
				//	$(".mob-fliter").fadeIn("slow");
				//}
				//else{
				//$(".mob-fliter").fadeOut("slow");
				//	}	
			});
		},
		exit: function () {
		}

	});



	//Media check js end	



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

	//if($(this).scrollTop() > 600){
	//  $('.fltr-col').addClass('fixed-fltr');
	//}
	//else{
	// $('.fltr-col').removeClass('fixed-fltr');
	//	}	

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
