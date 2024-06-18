$(document).ready(function () {

    const urlSearchParams = new URLSearchParams(window.location.search);
    const params = Object.fromEntries(urlSearchParams.entries());

    var file = params.file;
    var wid = params.wid;
    var itemname = params.itemname;
     
    $("#Msg").hide();
    $("#FailMsg").hide();

    $("#btnSubmit").click(function () {

        var femail = $('#fromemail').val().trim();
        var eemail = $('#eprintemail').val().trim();

        if (femail == "") {
            $('#fromRequired').show();
            $('#fromemail').focus();
            return false;
        }
        else if (validateEmail(femail) == false) {
            $('#fromFormat').show();
            $('#fromemail').focus();
            return false;
        }

        if (eemail == "") {
            $('#eprintRequired').show();
            $('#eprintemail').focus();
            return false;
        }
        else if (validateEmail(eemail) == false) {
            $('#eprintFormat').show();
            $('#eprintemail').focus();
            return false;
        }
        else if (validateEprintEmail(eemail) == false) {
            $('#eprintHPFormat').show();
            $('#eprintemail').focus();
            return false;
        }
        else {

            if (file != undefined && file != "") {
                $.ajax({
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    url: "/umbraco/Surface/Home/EprintMailSend",
                    data: { "file": file, "printemail": eemail, "fromemail": femail, "worksheetid": wid, "itemname": itemname },
                    success: function (e) {
                        if (e.status == "Success") {
                            $("#divForm").hide();
                            $("#Msg").show();
                            setTimeout(function () {
                                window.close();
                            }, 10000);
                        }
                        else if (e.status == "Fail") {
                            $("#Msg").hide();
                            $("#divForm").hide();
                            $("#FailMsg").show();
                            setTimeout(function () {
                                window.close();
                            }, 10000);
                        }
                    },
                    error: function (error) {

                    }
                });
            }
        }

    });

    $('#fromemail').keyup(function (e) {
        $('#fromRequired').hide();
        $('#fromFormat').hide();
    });

    $('#eprintemail').keyup(function (e) {
        $('#eprintRequired').hide();
        $('#eprintFormat').hide();
        $('#eprintHPFormat').hide();
    });
});

function validateEmail(email) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
    if (!filter.test(email))
        return false;
    else
        return true;
}

function validateEprintEmail(email) {
    var filter = /@hpeprint.com/i
    if (!filter.test(email))
        return false;
    else
        return true;
}