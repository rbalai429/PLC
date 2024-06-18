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
});

function changePlan(SrNo) {
	$(".chng-planbox").show();
	$('#SubscriptionPlanId').val(SrNo);
}

function subscriptionMapping(ranking, eventtyp) {
	debugger
	var subscriptionPlanId = $('#SubscriptionPlanId').val();
	
	if (ranking != '') {
		$("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/SubscriptionMappingBonusWorksheet", { Ranking: ranking, SrNo: subscriptionPlanId, eventtype: eventtyp }, function (responseTxt, statusTxt, xhr) {
			debugger
			if (statusTxt == "success") {
				$(".chng-planbox").hide();
				//$('#subscriptionplanlist').hide();
				$("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoadBonusWorksheet", function (responseTxt, statusTxt, xhr) {
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
		$("#SubsCouponCode").load("/umbraco/Surface/Subscription/AvailCouponCode", { CouponCode: couponCode }, function (responseTxt, statusTxt, xhr) {
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


function paynowStructureProgram() {
	debugger
	try {
		commonLayer('BuyNow', 'Structure Program Pay Now');
	}
	catch (ex) {
		//console.log('');
	}

	$("#loader").css('display', 'block');

	var cultureSessionName = $("#cultureSessionName").val();
	var couponCode = $("#couponCode").val();


	$.ajax({
		type: "GET",
		contentType: "application/json; charset=utf-8",
		dataType: "JSON",
		url: "/umbraco/Surface/Subscription/PayNowstructureprogram",
		data: { "CouponCode": couponCode },
		success: function (e) {
			if (e.status == "Success") {

				if (e.message == 'home') {
					window.location = cultureSessionName + "/";
				}
				else {
					//alert('pay');
					$('#paymentForm').attr('action', e.message);
					document.getElementById('paymentForm').submit();
				}
			}
			else if (e.status == "Validate") {
				$('#spValidationMsg').show();
			}
			else if (e.status == "Fail") {
				$("#msg_pay").show();
				$("#msg_pay").html(e.message);
				return false;
			}
		},
		error: function (error) {
			$("#loader").css('display', 'none');
		}, complete: function () {
			$("#loader").css('display', 'none');
		}
	});
}

//function Toggle() {
//	$(".duc-top").click(function () {

//		$(".couponbox").slideToggle();
//		$(this).toggleClass("active");
//	});
//}
