$(document).ready(function () {

	var couponCode = $('#couponCode').val();
	var ranking = $('#ranking').val();
	var IsCouponCodeOffer = $('#IsCouponCodeOffer').val();
	subscriptionMapping(ranking, 'onload');
	
	//Coupon Code popup
	//if ((couponCode != null || couponCode != '') && (IsCouponCodeOffer != null && IsCouponCodeOffer == "Y")) {
	//	CouponCodeCheck();
	//}

	if (couponCode != null || couponCode != 'TATA899') {
		CouponCodeCheck();
	}

	//Age group selection
	var lastChecked;

	var $checks = $('.selectage-inputs ul li input:checkbox').click(function (e) {
		var i = 0;
		var numinc = i++;
		//$(this).parents("li").addClass("active");
		// var lengthList = $(".selectage-inputs ul li.active").length;
		var numChecked = $checks.filter(':checked').length;
		$(".selected-itemdiv p span").text(numChecked);
		if (numChecked > 3) {
			var alrValue = numChecked - 1;
			$(".errorpqr").show();
			$(".selected-itemdiv p span").text(alrValue);
			//$(this).parents("li").siblings().addClass("scdf");
			lastChecked.checked = false;
		}
		else if (numChecked == 3) {
			$(".errorpqr").hide();
		}
		else {
			$(".errorpqr").hide();
		}
		lastChecked = this;
	});


	//$('#btnAddAgeGroup').click(function (event) {
	//event.preventDefault();
	//event.stopImmediatePropagation();

	//$(".duc-top").click(function () {
	//	$(".couponbox").slideToggle();
	//	$(this).toggleClass("active");
	//});


});

function AddAgeGroup() {

	var agegroups = [];
	$.each($("input[name='ageSelected']:checked"), function () {
		agegroups.push($(this).val());
	});
	agegroups = agegroups.join(",");

	if (agegroups == '') {
		$('#msgbox').show();
		$('#msgbox').html('Please choose atleast one age group');
	}
	else {
		$('#msgbox').hide();

		$("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/AddAgeGroup", { ageGroup: agegroups }, function (responseTxt, statusTxt, xhr) {

			if (statusTxt == "success") {
				$('.age-option').hide();

				//$.each(agegroups, function (index, value) {
				//	alert(value);
				//});

				//$.each($("input[name='ageSelected']:checked"), function () {
				//	$(this).prop('disabled', true);
				//});
				$("#dvSubscriptionAddAgeGroup").load("/Umbraco/Surface/Subscription/AgeGroupLoad", function (responseTxt, statusTxt, xhr) {
					if (statusTxt == "success") {

						//AddAgeGroup();
						$(".hd-age").click(function () {
							$(this).toggleClass("active-hd");
							$(".age-option").slideToggle().trigger();
						});


					}
				});
			}
			if (statusTxt == "error") {
				//alert(statusTxt);
			}
		});
	}
}
function changePlan(SrNo) {
	try {
		commonLayer("Lesson", "Buy Now");
	}
	catch (ex) {
		//console.log('error');
	}
	$(".chng-planbox").show();
	$('#SubscriptionPlanId').val(SrNo);
}

function subscriptionMapping(ranking, eventtyp) {

	var subscriptionPlanId = $('#SubscriptionPlanId').val();
	var IsBotRequest = $('#IsBotRequest').val();
	var agegroup = $('#agegroup').val();

	if ((ranking != '' && IsBotRequest == '') || (IsBotRequest == 'Yes')) {
		$("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/SubscriptionMapping", { Ranking: ranking, SrNo: subscriptionPlanId, eventtype: eventtyp, agegroup: agegroup, IsBotRequest: IsBotRequest }, function (responseTxt, statusTxt, xhr) {
			if (statusTxt == "success") {
				$(".chng-planbox").hide();
				//$('#subscriptionplanlist').hide();
				$("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoad", function (responseTxt, statusTxt, xhr) {
					//Toggle();
				});
			}
			if (statusTxt == "error") {
				//alert(statusTxt);
			}
		});
	}
}

function deleteItem(srno) {
	var DeleteTitle = $("#DeleteTitle").val();
	var DeleteDetailsMessage = $("#DeleteDetailsMessage").val();
	var DeleteConfirmButtonText = $("#DeleteConfirmButtonText").val();
	var DeleteCancelButtonText = $("#DeleteCancelButtonText").val();

	if (srno != null) {
		Swal.fire({
			title: DeleteTitle,
			text: DeleteDetailsMessage,
			//icon: 'success',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			cancelButtonText: DeleteCancelButtonText,
			confirmButtonText: DeleteConfirmButtonText
		}).then((result) => {
			if (result.isConfirmed) {
				$("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/DeleteSelectedItem", { srno: srno }, function (responseTxt, statusTxt, xhr) {
					if (statusTxt == "success") {
						$("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoad", function (responseTxt, statusTxt, xhr) {
							if (statusTxt == "success") {
								//Toggle();
							}
						});


						$("#dvSubscriptionAddAgeGroup").load("/Umbraco/Surface/Subscription/AgeGroupLoad", function (responseTxt, statusTxt, xhr) {
							if (statusTxt == "success") {
								AddAgeGroup();
								$(".hd-age").click(function () {
									$(this).toggleClass("active-hd");
									$(".age-option").slideToggle().trigger();
								});
							}
						});

					}
					if (statusTxt == "error") {
						//alert(statusTxt);
					}
				});
			}
		});
	}
}

function closePlan() {
	$(".chng-planbox").hide();
}

$('#btnAddExistingUserAgeGroup').click(function (event) {
	var agegroups = [];
	$.each($("input[name='ageSelected']:checked"), function () {
		agegroups.push($(this).val());
	});
	agegroups = agegroups.join(",");

	if (agegroups == '') {
		$('#msgbox').show();
		$('#msgbox').html('Please choose atleast one age group');
	}
	else {
		$("#addagegroup").css('display', 'block');
		$.ajax({
			type: "GET",
			contentType: "application/json; charset=utf-8",
			dataType: "JSON",
			url: "/Umbraco/Surface/Subscription/AddAgeGroupExistingGroup",
			data: { ageGroup: agegroups },
			success: function (e) {
				//alert(e.message);
				if (e.status == "OK") {
					var redirectparam = $('#redirectparam').val();
					var culture = $('#culture').val();
					var usertype = $('#usertype').val();
					/*window.location = culture + '/subscription/buy-now?' + redirectparam;*/
					if (usertype === 'existing')
						window.location = culture + 'subscription';
					else
						window.location = culture + '/';
				}
				else if (e.status == "Fail") {
					$('#msgbox').show();
					$('#msgbox').html(e.message);
				}
			},
			error: function (error) {
				$("#addagegroup").css('display', 'none');
			}, complete: function () {
				$("#addagegroup").css('display', 'none');
			}
		});
	}
});

function CouponCodeCheck() {

	var couponCode = $('#couponCode').val();
	//var couponCodednmc = $('#cpncd').val();

	//if (couponCodednmc != null || couponCodednmc != "") {
	//	couponCode = couponCodednmc;
	//}

	if (couponCode == '' || couponCode == null) {
		$('#couponmessage').show();
		$('#couponmessage').html('Please enter coupon code.');
	}
	else {
		/*if (couponCode) {*/
		$("#loader").css('display', 'block');
		$("#SubsCouponCode").load("/umbraco/Surface/Subscription/AvailCouponCode", { CouponCode: couponCode,sourceOfCoupon: "lesson" }, function (responseTxt, statusTxt, xhr) {
			if (statusTxt == "success") {

				try {
					if (responseTxt == "valid") {
						CouponValid(couponCode);
					}
					else if (responseTxt == "notvalid") {
						CouponNotValid(couponCode);
					}
				}
				catch (cx) {
					//console.log('');
				}

				$("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoad", function (responseTxt, statusTxt, xhr) {
					if (statusTxt == "success") {
						//Toggle();
					}
				});

				$("#loader").css('display', 'none');
			}
			if (statusTxt == "error") {
				$("#loader").css('display', 'none');
			}
		});
	}
	/*}*/
}

function CancelCouponCode() {
	$("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionCancelCouponPayLoad", function (responseTxt, statusTxt, xhr) {
		if (statusTxt == "success") {
			//Toggle();
		}
	});
}

$(".hd-coupon").click(function () {
	$(this).toggleClass("active-hd");
	$(".coupon-option").slideToggle().trigger();
});

//function Toggle() {
//	$(".duc-top").click(function () {

//		$(".couponbox").slideToggle();
//		$(this).toggleClass("active");
//	});
//}
