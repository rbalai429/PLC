var PublicHolidayList = []
$(document).ready(function () {
    GetHolidayList();
    GetTimeList();
});
function GetHolidayList() {
    $.ajax({
        type: "GET",
        url: "/umbraco/Surface/FAQ/GetHolidayList",
        success: function (data) {
            $('[data-toggle="datepicker"]').datepicker();
            if (data != null && data.StatusCode == 200) {
                if (data.Result != null && data.Result != undefined) {
                    $.each(data.Result, function (key, value) {
                        PublicHolidayList.push(value);
                    })
                }
               
                $('#SelectDate').datepicker(
                    {
                        defaultDate: "+1d",
                        minDate: 1,
                        beforeShowDay: function (date) {
                            var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
                            var isDisabled = ($.inArray(string, PublicHolidayList) != -1);
                            return [includeDate(date) && !isDisabled];
                        },

                        maxDate: (function (max) {
                            var today = new Date();
                            var nextAvailable = getTomorrow(today);
                            var count = 0;
                            var newMax = 0;
                            while (count < max) {
                                if (includeDate(nextAvailable)) {
                                    count++;
                                }
                                newMax++;
                                nextAvailable = getTomorrow(nextAvailable);
                            }
                            return newMax;
                        })
                        ($("#MaxCountAllowToSelect").val())
                    });
            }
        },
        error: function (error) {
        },
        complete: function () {
        }
    });
}
function GetTimeList() {
    $.ajax({
        type: "GET",
        url: "/umbraco/Surface/FAQ/GetTimeList?SelectedDate=" + $("#SelectDate").val(),
        success: function (data) {
            if (data != null && data.StatusCode == 200) {
                if (data.Result != null && data.Result != undefined) {
                    $("#SelectTime option").remove();
                    $('#SelectTime').append($('<option>', {
                        value: "",
                        text: "Select Time"
                    }));
                    $.each(data.Result, function (i, item) {
                        $('#SelectTime').append($('<option>', {
                            value: item.Value,
                            text: item.Title
                        }));
                    });

                }
            }
        },
        error: function (error) {
        },
        complete: function () {
        }
    });
}
//dd-mm-yyyy
function includeDate(date) {
    //$().datepicker('formatDate', new Date(2014, 1, 14));
    var string = jQuery.datepicker.formatDate('dd-mm-yy', date);
    var isDisabled = ($.inArray(string, PublicHolidayList) != -1);
    return date.getDay() !== 7 && date.getDay() !== 0 && !isDisabled;
}

function getTomorrow(date) {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
}
$("#SelectDate").change(function () {
    GetTimeList();
    var SelectDate = $("#SelectDate").val();
    if (SelectDate == null || SelectDate == undefined || SelectDate == '') {
        $('#DateRequired').fadeIn();
        return false;
    } else {
        $('#DateRequired').fadeOut();
    }
})
$("#SelectTime").change(function () {
    var SelectTime = $("#SelectTime").val();
    if (SelectTime == null || SelectTime == undefined || SelectTime == '') {
        $('#TimeRequired').fadeIn();
        return false;
    } else {
        $('#TimeRequired').fadeOut();
    }
})

//request submit start
$(".frmRow .sbmt-btn").click(function () {
    var FullName = $("#FullName").val().trim();
    var Mobile = $("#Mobile").val().trim();
    var SelectDate = $("#SelectDate").val();
    var SelectTime = $("#SelectTime").val();
    //var Remark = $("#Remark").val();
    //var Consent = $("#Consent").val();
	var Remark = null;
	var Consent = null;

    if (FullName == null || FullName == undefined || FullName == '') {
        $('#FullNameRequired').fadeIn();
        return false;
    } else {
        $('#FullNameRequired').fadeOut();
    }
    if (Mobile == null || Mobile == undefined || Mobile == '') {
        $('#MobileRequired').fadeIn();
        $('#InvalidMobile').fadeOut();
        return false;
    } else {
        $('#MobileRequired').fadeOut();
        if (Mobile.length != 10) {
            $('#InvalidMobile').fadeIn();
            return false;
        } else {
            $('#InvalidMobile').fadeOut();
        }
    }
    if (SelectDate == null || SelectDate == undefined || SelectDate == '') {
        $('#DateRequired').fadeIn();
        return false;
    } else {
        $('#DateRequired').fadeOut();
    }
    if (SelectTime == null || SelectTime == undefined || SelectTime == '') {
        $('#TimeRequired').fadeIn();
        return false;
    } else {
        $('#TimeRequired').fadeOut();
    }
    var Request = {
        'FullName': FullName,
        'Mobile': Mobile,
        'SelectDate': SelectDate,
        'SelectTime': SelectTime,
        'Remark': Remark,
        'Consent': Consent
    };
    SaveRequestDetails(Request);
});
$("#AgreeTermText").change(function () {
    if ($(this).prop("checked") == true) {
        $(".frmRow .sbmt-btn").removeClass("disable-click");
    } else {
        $(".frmRow .sbmt-btn").addClass("disable-click");
    }
});
function SaveRequestDetails(Request) {
    $(".frmRow .sbmt-btn").css({ 'pointer-events': 'none', 'cursor': 'default', 'opacity': '0.6' });
    $.ajax({
        type: "POST",
        url: "/umbraco/Surface/FAQ/SaveRequestDetails",
        data: Request,
        success: function (data) {
            if (data != null && data.StatusCode == 200) {
                $("#SubmitSuccess").fadeIn();
                $("#SubmitSuccess").html(data.Message);
                ResetForm();
                setTimeout(function () {
                    $("#SubmitSuccess").fadeOut();
                }, 4000);

            } else {
               // alert(data.Message);
            }
        },
        fail: function (data) {

        },
        complete: function (data) {
            $(".frmRow .sbmt-btn").css({ 'pointer-events': '', 'cursor': '', 'opacity': '' });
        }
    });
}
function ResetForm() {
    $("#FullName").val('');
    $("#Mobile").val('');
    $("#SelectDate").val('');
    $("#SelectTime").val('');
    $("#AgreeTermText").prop("checked", false);
    $(".frmRow .sbmt-btn").addClass("disable-click");
}
$("#FullName").keyup(function () {
    if ($("#FullName").val().trim() == null || $("#FullName").val().trim() == undefined || $("#FullName").val().trim() == '') {
        $('#FullNameRequired').fadeIn();
        return false;
    } else {
        $('#FullNameRequired').fadeOut();
    }
});
$('#FullName').keypress(function (e) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    } else {
        e.preventDefault();
        return false;
    }
});
$("#Mobile").keyup(function () {
    if ($("#Mobile").val().trim() == null || $("#Mobile").val().trim() == undefined || $("#Mobile").val().trim() == '') {
        $('#MobileRequired').fadeIn();
        $('#InvalidMobile').fadeOut();
        return false;
    } else {
        $('#MobileRequired').fadeOut();
        if ($("#Mobile").val().trim().length != 10) {
            $('#InvalidMobile').fadeIn();
            return false;
        } else {
            $('#InvalidMobile').fadeOut();

        }
    }
});

//request submit end

