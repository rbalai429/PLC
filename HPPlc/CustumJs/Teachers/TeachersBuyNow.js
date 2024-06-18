$(document).ready(function () {

    var ranking = $('#ranking').val(); 
    var IsCouponCodeOffer = $('#IsCouponCodeOffer').val();
    subscriptionMapping(ranking, 'onload');

});


$(window).on('load', function () {
    var couponCode = $('#cpncd').val();
    
    if (couponCode != null && couponCode != '' && couponCode != undefined) {
        $("#couponCode").html(couponCode);
        CouponCodeCheck();
    }
});
//function changePlan(SrNo) {
//    $(".chng-planbox").show();
//    $('#SubscriptionPlanId').val(SrNo);
//}

function changePlan(SrNo) {

    try {
        commonLayer("Teachers", "Buy Now");
    }
    catch (ex) {
        //console.log('error');
    }
    $("#subscriptionplanlist").show();
    $(".chng-planbox").show();
    $('#SubscriptionPlanId').val(SrNo);
}


function subscriptionMapping(ranking, eventtyp) {
    
    var subscriptionPlanId = $('#SubscriptionPlanId').val();
    var agegroup = $('#agegroup').val();
    
    if (ranking != '') {
        $("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/SubscriptionMappingTeachersWorksheet", { Ranking: ranking, SrNo: subscriptionPlanId, eventtype: eventtyp, agegroup: agegroup }, function (responseTxt, statusTxt, xhr) {
            
            if (statusTxt == "success") {
                $(".chng-planbox").hide();
                //$('#subscriptionplanlist').hide();
                $("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoadTeachersWorksheet", function (responseTxt, statusTxt, xhr) {
                    //Toggle();
                });
            }
            if (statusTxt == "error") {
                //alert(statusTxt);
            }
        });
    }
}


function ChangeAgeGroup() {

    var agegroups = [];
    $.each($("input[name='ageSelected']:checked"), function () {
        agegroups.push($(this).val());
    });

    agegroups = agegroups.join(",");

    $('#msgbox').hide();

    $("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/ChangeAgeGroupTeachers", { ageGroup: agegroups, ranking:2 }, function (responseTxt, statusTxt, xhr) {

            if (statusTxt == "success") {
                $('.age-option').hide();
                
                $("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoadTeachersWorksheet", function (responseTxt, statusTxt, xhr) {
                    //Toggle();
                });

                $("#dvSubscriptionAddAgeGroup").load("/Umbraco/Surface/Subscription/AgeGroupLoadTeachers", function (responseTxt, statusTxt, xhr) {
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
                $("#dvSubscriptionCart").load("/Umbraco/Surface/Subscription/DeleteSelectedItemTeachers", { srno: srno }, function (responseTxt, statusTxt, xhr) {
                    if (statusTxt == "success") {
                        $("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoadTeachersWorksheet", function (responseTxt, statusTxt, xhr) {
                            if (statusTxt == "success") {
                                //Toggle();
                            }
                        });


                        $("#dvSubscriptionAddAgeGroup").load("/Umbraco/Surface/Subscription/AgeGroupLoadTeachers", function (responseTxt, statusTxt, xhr) {
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
    $("#subscriptionplanlist").hide();
    $(".chng-planbox").hide();
}

function CouponCodeCheck() {
    //debugger
    var couponCode = $('#couponCode').val();
    var couponCodednmc = $('#cpncd').val();

    if ((couponCode == null || couponCode == "") && (couponCodednmc != null || couponCodednmc != "")) {
    	couponCode = couponCodednmc;
    }

    if (couponCode == '' || couponCode == null) {
        $('.error').hide();
        $('#couponmessage').show();
        $('#couponmessage').html('Please enter coupon code.');
    }
    else {
        /*if (couponCode) {*/
        $("#loader").show();
        $("#SubsCouponCode").load("/umbraco/Surface/Subscription/AvailCouponCode", { CouponCode: couponCode, sourceOfCoupon: "teachers" }, function (responseTxt, statusTxt, xhr) {
            
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

                $("#SubsCouponCode").load("/Umbraco/Surface/Subscription/SubscriptionCouponLoadBonusWorksheet", function (responseTxt, statusTxt, xhr) {
                    if (statusTxt == "success") {
                        //Toggle();
                    }
                });

                $("#SubscriptionPay").load("/Umbraco/Surface/Subscription/SubscriptionPayLoadTeachersWorksheet", function (responseTxt, statusTxt, xhr) {
                    if (statusTxt == "success") {
                        //Toggle();
                    }
                });

                $("#loader").hide();
            }
            if (statusTxt == "error") {
                $("#loader").hide();
            }
        });

        //$("#loader").hide();
    }
    /*}*/
}

function CancelCouponCode() {
    $("#SubscriptionPay").load("/Umbraco/Surface/Subscription/TeachersSubscriptionCancelCouponPayLoad", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            //Toggle();
        }
    });

    $("#SubsCouponCode").load("/Umbraco/Surface/Subscription/SubscriptionCouponLoadBonusWorksheet", function (responseTxt, statusTxt, xhr) {
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
    //debugger
    try {
        commonLayer('BuyNow', 'Teachers Program Pay Now');
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
        url: "/umbraco/Surface/Subscription/PayNowTeachersProgram",
        data: { "CouponCode": couponCode },
        success: function (e) {
            if (e.status == "Success") {
                
                if (e.message == 'teachers') {
                    window.location = "/teachers";
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

//$(document).on("click", "#buynowbutton", function () {
//    debugger
//    var selectedpackage = $(".subscriptionradio:checked").map(function () { return $(this).attr("data-amount") }).get().join(", ");
//    if (selectedpackage != undefined && selectedpackage != null && selectedpackage != "") {
//        console.log(selectedpackage);


//        //window.location.href = selectedpackage;
//    } else {
//        $("#subscriptionnotseletederrorMessage").show();
//    }
//});

//$(document).on("click", "#buynowbutton", function () {
    
//    var selectedpackage = $(".subscriptionradio:checked").map(function () { return $(this).attr("data-amount") }).get().join(", ");
//    var subscriptionid = $(".subscriptionradio:checked").map(function () { return $(this).attr("data-subscriptionid") }).get().join(", ");
//    var selectedpackagelocate = $(".subscriptionradio:checked").map(function () { return this.value }).get().join(", ");
//    if (selectedpackage != undefined && selectedpackage != null && selectedpackage != "") {
//        //console.log(selectedpackage);
//        //changePlan(subscriptionid);

//        try {
//            commonLayer("Teachers", "Buy Now - " + selectedpackage);
//        }
//        catch (ex) {
//            //console.log('error');
//        }

//        window.location.href = selectedpackagelocate;
//    } else {
//        $("#subscriptionnotseletederrorMessage").show();
//    }
//});
