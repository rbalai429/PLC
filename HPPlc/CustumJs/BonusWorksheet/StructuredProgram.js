
var currentPage_lazy = 1;
var text_lazy = "";

//var scrollHandler = function () {
    //alert(isReachedScrollEnd);
    //alert($(document).scrollTop());
    //alert($(document).height());
    //alert($(window).height());

    //if (isReachedScrollEnd == false &&
    //    ($(document).scrollTop() <= $(document).height() - $(window).height())) {
    //    alert('hi');
    //    //loadProducts(ajaxCallUrl);
    //    if (currentPage > -1 && !inCallback) {
    //        inCallback = true;
    //        currentPage++;
    //    }
    //    alert(currentPage)
    //    GetStructuredProgramListFilter("", "", "", "", "", "", "");
    //}
//}

//$(window).scroll(function () {
//    var top_of_element = $("#pagediv").offset().top;
//    var bottom_of_element = $("#pagediv").offset().top + $("#pagediv").outerHeight();
//    var bottom_of_screen = $(window).scrollTop() + $(window).innerHeight();
//    var top_of_screen = $(window).scrollTop();

//    if (currentPage1 > -1) {



//    if ((bottom_of_screen > top_of_element) && (top_of_screen < bottom_of_element)) {
//        // the element is visible, do something
//        //alert('yes')

//        //DivCallback = true;

//        DivCallbackY = true;
//        DivCallbackN = false;

//       // alert(DivCallbackY);
//       // alert(DivCallbackN);
//        if (DivCallbackN == false && DivCallbackY == true) {

//            DivCallbackY == false;
//            DivCallbackN = true;
//            //GetStructuredProgramListFilter("", "", "", "", "2", "", "");
//            currentPage1++;
//            //inCallback == true
//            alert(currentPage1);
//        }
//    } else {
//        //alert('No');
//        // the element is not visible, do something else
//        DivCallback = false;
//        inCallback == true;

//        DivCallbackN = true;
//        DivCallbackY = false;
//        }

//    }
//});

//var win = $(window);

//// Each time the user scrolls
//win.scroll(function () {

//    alert($(document).height());
//    alert($(win.height()));
//    alert($(win.scrollTop());
//    // End of the document reached?
//    if ($(document).height() - win.height() == win.scrollTop()) {
//        //$('#loading').show();
//        currentPage1++;
//        alert(currentPage1);
//    }
//});

//var row = $('#hdAlreadyDisplayedWorksheet').val();

var row = "";
var allcount = '';
var rowperpage = '';

$(window).data('ajaxready', false).scroll(function (e) {
    //alert(currentPage_lazy);
    var searchtext_lazy = $("#txtSearch").val();
    if ($(window).data('ajaxready') == false) return;
    //alert(currentPage_lazy);    
    var position = $(window).scrollTop();
    var bottom = $(document).height() - $(window).height();
    var footht = $('.faqbox-row').height() + $('footer').height() + 100;
    //console.log(position +"::::"+bottom+ ':::::'+(bottom-footht));
    if (isNaN(footht)) {
        footht = 100;
    }


    var top_of_element = $("#pagediv").offset().top;
    var bottom_of_element = $("#pagediv").offset().top + $("#pagediv").height();
    var bottom_of_screen = $(window).scrollTop() + $(window).height();
    var top_of_screen = $(window).scrollTop();

    
    /*    if (position > (bottom - footht)) {*/

     if ((bottom_of_screen > top_of_element) && (top_of_screen < bottom_of_element)) {

        $(window).data('ajaxready', false);
       
        var SortBy_lazy = "";
        var selectedAgeGroup_lazy = "";
        var selectedSubject_lazy = "";
        var vrTopicsGroupe_lazy = "";
        var vrPaidGroupe_lazy = "";
        //alert(currentPage1);
        
        if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
            SortBy_lazy = $("#dropsortBy").val();
        }
        if ($("#contentType").val() == "worksheetListingAgeWise" || $("#contentType").val() == "worksheetCategory" || $("#contentType").val() == "topicsName" || $("#contentType").val() == "subjects" || $("#contentType").val() == "topics") {
            if (selectedAgeGroup_lazy == undefined || selectedAgeGroup_lazy == "" || selectedAgeGroup_lazy == null) {
                selectedAgeGroup_lazy = $("#class").val()
            }
            if (selectedSubject_lazy == undefined || selectedSubject_lazy == "" || selectedAgeGroup_lazy == null) {
                selectedSubject = $("#subject").val()
            }
            if (vrTopicsGroupe_lazy == undefined || vrTopicsGroupe_lazy == "" || selectedAgeGroup_lazy == null) {
                vrTopicsGroupe_lazy = $("#topic").val()
            }
        }

        selectedAgeGroup_lazy = $(".classes:checked").map(function () { return this.value }).get().join(",");
        selectedSubject_lazy = $(".subjects:checked").map(function () { return this.value }).get().join(",");
        //vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
         //vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");

         
         vrTopicsGroupe_lazy = $(".topics:checked").map(function () { return this.value }).get().join(",");
         vrPaidGroupe_lazy = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");


        if (!$("#WorksheetList div").hasClass("fit-hd")) {
           
            if ($.isNumeric(text_lazy) == false) {
                $("#worksheetLoading").show();
                currentPage_lazy++;
                GetStructuredProgramListFilter(selectedAgeGroup_lazy, selectedSubject_lazy, vrTopicsGroupe_lazy, vrPaidGroupe_lazy, currentPage_lazy, searchtext_lazy, SortBy_lazy);
            }
        }
        else {
            //item-col
            if ($("#WorksheetList div").hasClass("item-col")) {
                $("#WorksheetList div.fit-hd").hide();
            }
            $(window).data('ajaxready', true);
        }
       
        
      //  $(window).data('ajaxready', true);
        //console.log(row);

        //row = row + rowperpage;

        //console.log("row:" + row + ":::: allcount:"+ allcount +"::::rowperpage:"+ rowperpage)

        //if (row <= allcount + 1) {
        //    $(window).data('ajaxready', false);

        //    $("#worksheetLoading").css('display', 'block');
        //    alert(row);
        //    //GetWorkSheetList("", "", "");

        //}
    }

});

var pageName = $("#page").val();

var searchitemslist = [];
var row = $('#hdAlreadyDisplayedWorksheet').val();

var NoRecordFound = false;

var allcount = '';
var rowperpage = '';

var timeoutId;
var currentLI = -1;

var currentpage = 1;
if ($("#bonusworksheetcurrentpage").val() != undefined && $("#bonusworksheetcurrentpage").val() != null && $("#bonusworksheetcurrentpage").val() != 0) {
    currentpage = $("#bonusworksheetcurrentpage").val();
}

var VideoInput = {
    CurrentNode: $("#hdnCurrentNode").val(),
    CultureInfo: $("#hdnCultureInfo").val(),
    selectedAgeGroup: $("#class").val(),
    selectedVolume: "",
    selectedCategory: $("#subject").val(),
    FilterType: 'age'
}

var Input = {
    CultureInfo: $("#hdnCultureInfo").val(),
    selectedAgeGroup: $("#class").val(),
    selectedSubject: $("#subject").val(),
    selectedTopics: $("#topic").val(),
    IsCbseContent: "",
    DisplayAgeGroup: row,
    currentPage: currentpage,
    VideosInput: VideoInput
};

//$.post("/umbraco/Surface/StructuredProgram/GetStructuredProgramList",
//    {
//        Input
//    },
//    function (data, status) {
//        //alert("Data: " + data + "\nStatus: " + status);
//        $('#hdTobeDisplayWorksheet').val(row);
//        $("#WorksheetList").html("").append(data);
//        $(window).data('ajaxready', true);

//        slicjFunction();
//        share();
//        $(".clsPrintDoc").click(function () {

//            var vURLPath = $(this).find("span").html();

//            try {
//                var layerDetail = $(this).find('span').next('span').html();
//                printTracker(layerDetail, 'Home');
//            }
//            catch (ex) {
//                //console.log('error');
//            }
//            PrintWorkSheet(vURLPath);
//        });

//        BindPaging();
//        $('#worksheetLoading').hide();
//    });

function GetWorkSheetList(selectedAgeGroup, selectedSubject, CbseContent) {
    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: selectedAgeGroup,
        selectedSubject: selectedSubject,
        IsCbseContent: CbseContent,
        DisplayAgeGroup: row
    };
    $.ajax({
        type: "POST",
        dataType: "html",
        url: "/umbraco/Surface/StructuredProgram/GetStructuredProgramList",
        data: Input,

        success: function (responce) {

            $('#hdTobeDisplayWorksheet').val(row);
            $("#WorksheetList").append(responce);

            slicjFunction();
            share();
            $(".clsPrintDoc").click(function () {
                var vURLPath = $(this).find("span").html();

                try {
                    var layerDetail = $(this).find('span').next('span').html();
                    printTracker(layerDetail, 'Home');
                }
                catch (ex) {
                    //console.log('error');
                }
                PrintWorkSheet(vURLPath);
            });
            BindPaging();
        },
        error: function (error) {
            $("#worksheetLoading").hide();
        }, complete: function () {
            $("#worksheetLoading").hide();
            //console.log($(window).data('ajaxready'));
            $(window).data('ajaxready', true);
        }
    });
}

function GetStructuredProgramListFilter(selectedAgeGroup, selectedSubject, vrTopicsGroupe, vrPaidGroupe, currentPage, searchText, SortBy) {
    //setTimeout(function () {
    //    loadvideo()
    //}, 200);
    //alert(2);
    //alert(vrPaidGroupe);
    
    if ($("#contentType").val() == "worksheetListingAgeWise" || $("#contentType").val() == "worksheetCategory" || $("#contentType").val() == "topicsName" || $("#contentType").val() == "subjects" || $("#contentType").val() == "topics") {
        if (selectedAgeGroup == undefined || selectedAgeGroup == "" || selectedAgeGroup == null) {
            selectedAgeGroup = $("#class").val()
        }
        if (selectedSubject == undefined || selectedSubject == "" || selectedAgeGroup == null) {
            selectedSubject = $("#subject").val()
        }
        if (vrTopicsGroupe == undefined || vrTopicsGroupe == "" || selectedAgeGroup == null) {
            vrTopicsGroupe = $("#topic").val()
        }
    }

    //$("#WorksheetList").html("");
    $("#NoResultFound").css('display', 'block');
    //$('#worksheetLoading').show();
    //console.log("row:" + row);

    

    var VideoInput = {
        CurrentNode: $("#hdnCurrentNode").val(),
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: selectedAgeGroup,
        selectedVolume: "",
        selectedCategory: selectedSubject,
        FilterType: 'age'
    }
    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: selectedAgeGroup,
        selectedSubject: selectedSubject,
        selectedTopics: vrTopicsGroupe,
        selectedPaid: vrPaidGroupe,
        DisplayAgeGroup: 0,
        currentPage: currentPage,
        searchText: searchText,
        sortBy: SortBy,
        VideosInput: VideoInput,
        NoRecordFound: NoRecordFound
    };
    
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "/umbraco/Surface/StructuredProgram/GetStructuredProgramList",
        data: JSON.stringify(Input),
        success: function (responce) {
            //$("#WorksheetList").html("").append(responce);
            $(".fit-hd").hide();
            $("#WorksheetList").append(responce);



            //console.log(responce);
            //var paid = $("#WorksheetList .list-items .paidIcon");
            //var free = $("#WorksheetList .list-items .freeLab");
            //if (paid.length != 0 && free.length == 0) {
            //    $(".paymenttypes").filter(function (index, obj) {
            //        if (obj.value == "1") {
            //            $(obj).prop('checked', true);
            //        }
            //    });
            //}
            //else if (paid.length == 0) {
            //    $(".paymenttypes").filter(function (index, obj) {
            //        if (obj.value == "1") {
            //            $(obj).prop('checked', true);
            //        }
            //    });
            //}
            //if (free.length != 0 && paid.length == 0) {
            //    $(".paymenttypes").filter(function (index, obj) {
            //        if (obj.value == "0") {
            //            $(obj).prop('checked', true);
            //        }
            //    });
            //} else if (free.length == 0) {
            //    $(".paymenttypes").filter(function (index, obj) {
            //        if (obj.value == "0") {
            //            $(obj).prop('disabled', true);
            //        }
            //    });
            //}
            //if ($("#WorksheetList .list-items div").hasClass("fit-hd")) {
            //    if ($("#contentType").val() == "worksheetListingAgeWise" || $("#contentType").val() == "worksheetCategory" || $("#contentType").val() == "topicsName" || $("#contentType").val() == "subjects" || $("#contentType").val() == "topics") {
            //        if (selectedAgeGroup == undefined || selectedAgeGroup == "" || selectedAgeGroup == null) {
            //            selectedAgeGroup = $("#class").val()
            //        } else {
            //            selectedAgeGroup = "";
            //        }
            //        if (selectedSubject == undefined || selectedSubject == "" || selectedAgeGroup == null) {
            //            selectedSubject = $("#subject").val()
            //        } else {
            //            selectedSubject = "";
            //        }
            //        if (vrTopicsGroupe == undefined || vrTopicsGroupe == "" || selectedAgeGroup == null) {
            //            vrTopicsGroupe = $("#topic").val()
            //        } else {
            //            vrTopicsGroupe = "";
            //        }
            //        GetStructuredProgramListFilter(selectedAgeGroup, selectedSubject, vrTopicsGroupe, "0", 1, "", -1);
            //    } else {
            //        GetStructuredProgramListFilter("", "", "", "0", 1, "", -1);
            //    }
            //}

            

            //}
            //if ($("#WorksheetList .list-items div").hasClass("fit-hd")) {
                //alert('');
                if (Input.searchText != "")
                {
                    if ($("#WorksheetList .item-col").length < 1 )
                    {
                    $("#WorksheetList .list-items div.fit-hd").hide();
                    $("#usernorecordfoundmessage").val("");
                    $(".overlaynorecordfound").show();

                    NoRecordFound = true;

                    $("#btnnorecordfoundData").click(function () {
                        $("#txtSearch").val("");
                        $(".srchClear").hide();
                        var message = $("#usernorecordfoundmessage").val();
                        if (message != "" && message != undefined && message != null) {
                            CaptureUserSubmitData(Input.searchText);
                            GetStructuredProgramListFilter(Input.selectedAgeGroup, Input.selectedSubject, Input.selectedTopics, Input.selectedPaid, Input.currentPage, "", Input.sortBy);
                        } else {
                            $("#usernorecordfounderror").show();
                            $("#usernorecordfounderror").html("Please enter the message");
                        }
                    });

                    $(".btnnorecordfoundclose").click(function () {
                        $("#txtSearch").val("");
                        $(".srchClear").hide();
                        $(".overlaynorecordfound").hide();

                        GetStructuredProgramListFilter(Input.selectedAgeGroup, Input.selectedSubject, Input.selectedTopics, Input.selectedPaid, Input.currentPage, "", Input.sortBy);

                    });

                }

            }

            //$("html").animate(
            //    {
            //        scrollTop: ($(".descTitlePlc").offset().top - 130)
            //    },
            //    100 //speed
            //);
            slicjFunction();
            share();
            $(".clsPrintDoc").click(function () {
                var vURLPath = $(this).find("span").html();
                //alert(vURLPath);
                try {
                    var layerDetail = $(".clsPrintDoc").find('span').next('span').html();
                    printTracker(layerDetail, 'Home');
                }
                catch (ex) {
                    //console.log('error');
                }

                PrintWorkSheet(vURLPath);
            });

            BindPaging();
        },
        error: function (error) {
            $("#worksheetLoading").hide();
        }, complete: function () {
            $("#worksheetLoading").hide();
            $(window).data('ajaxready', true);
            
        }
    });
}


function GetSortedStructuredProgramList(selectedAgeGroup, selectedSubject, CbseContent, SortBy) {
    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: selectedAgeGroup,
        selectedSubject: selectedSubject,
        IsCbseContent: CbseContent,
        DisplayAgeGroup: row,
        sortBy: SortBy
    };
    $.post("/umbraco/Surface/StructuredProgram/GetStructuredProgramList",
        {
            Input
        },
        function (data, status) {
            //alert("Data: " + data + "\nStatus: " + status);
            $('#hdTobeDisplayWorksheet').val(row);
            $("#WorksheetList").html("").append(data);
            $(window).data('ajaxready', true);

            slicjFunction();
            share();
            $(".clsPrintDoc").click(function () {

                var vURLPath = $(this).find("span").html();

                try {
                    var layerDetail = $(this).find('span').next('span').html();
                    printTracker(layerDetail, 'Home');
                }
                catch (ex) {
                    //console.log('error');
                }
                PrintWorkSheet(vURLPath);
            });
            BindPaging();
            $('#worksheetLoading').hide();
        });

}

$("#dropsortBy").change(function () {
    //var vrAgrGroupe = '';
    //var vrSubjectGroupe = '';
    //var vrTopicsGroupe = '';
    //var vrPaidGroupe = '';
    //var currentPage = 1;


    //vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
    //vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
    //vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
    //vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");

    $(window).data('ajaxready', true);
    currentPage_lazy = 1;
    try {
        //commonLayer('Login', 'Generate OTP Registration');
        commonLayer(pageName, 'Sort By - ' + $(this).val());
    }
    catch (ex) {
        //console.log(ex);
    }

    var searchtext = $("#txtSearch").val();
    ApplyingFilter(searchtext)
    //GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, currentPage, "", $(this).val());

});
//$("#dropsortBy").change(function () {
//    var vrAgrGroupe = '';
//    var vrSubjectGroupe = '';
//    var vrTopicsGroupe = '';
//    var vrPaidGroupe = '';
//    var currentPage = 1;


//    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
//    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
//    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
//    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");



//    try {
//        //commonLayer('Login', 'Generate OTP Registration');
//        commonLayer(pageName, 'Sort By - ' + $(this).val());
//    }
//    catch (ex) {
//        //console.log(ex);
//    }

//    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, currentPage, "", $(this).val());
//});

$("#txtSearch").focus(function () {
    var recentSearchItemslistArr = localStorage.getItem("recentsearchItems");
    var recentSearchItemslist = JSON.parse(recentSearchItemslistArr);
    currentLI = -1;

    var searchText = $(this).val();
    if (searchText.length >= 1) {
        $(".srchClear").show();
    } else {
        $(".srchClear").hide();
    }

    if (recentSearchItemslist != null) {
        $("#recentsearchitemlist").html("");
        var recentsearchitemli = "";
        recentSearchItemslist.forEach(function (item, index) {
            recentsearchitemli += '<li><em></em><span onclick="searchitemclick(\'' + item.trim() + '\')">' + item.trim() + '</span><i class="recentsearchclear" onclick="recentsearchclear(\'' + item.trim() + '\')" data-val=' + item.trim() + '>x</i></li>';
        });

        $("#recentsearchitemlist").html(recentsearchitemli);
        if (recentSearchItemslist.length == 0) {
            $("#rowSerchlist").hide();

        } else {
            $("#rowSerchlist").show();
        }
    }
})


$("#txtSearch").keyup(function () {
    //currentLI = -1;
    var searchText = $(this).val();

    //try {
    //    //commonLayer('Login', 'Generate OTP Registration');
    //    commonLayer(pageName, 'Search - ' + searchText);
    //}
    //catch (ex) {
    //    //console.log(ex);
    //}
    if (searchText.length >= 1) {
        $(".srchClear").show();
    } else {
        $(".srchClear").hide();
    }

    if (searchText.length >= 3) {
        var selectedAgeGroup = '';
        var selectedSubject = '';
        var vrTopicsGroupe = '';
        var vrPaidGroupe = '';
        var SortBy = -1;
        var currentPage = -1;

        selectedAgeGroup = $(".classes:checked").map(function () { return this.value }).get().join(",");
        selectedSubject = $(".subjects:checked").map(function () { return this.value }).get().join(",");
        vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
        vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");
        var VideoInput = {
            CurrentNode: $("#hdnCurrentNode").val(),
            CultureInfo: $("#hdnCultureInfo").val(),
            selectedAgeGroup: selectedAgeGroup,
            selectedVolume: "",
            selectedCategory: selectedSubject,
            FilterType: 'age'
        }
        var Input = {
            CultureInfo: $("#hdnCultureInfo").val(),
            selectedAgeGroup: selectedAgeGroup,
            selectedSubject: selectedSubject,
            selectedTopics: vrTopicsGroupe,
            selectedPaid: vrPaidGroupe,
            DisplayAgeGroup: 0,
            currentPage: currentPage,
            searchText: searchText,
            sortBy: SortBy,
            VideosInput: VideoInput
        };
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/umbraco/Surface/StructuredProgram/GetStructuredProgramSearchAutoComplete",
            data: JSON.stringify(Input),
            success: function (responce) {

                $("#searchitemlist").html("");
                var searchitemli = "";
                responce.forEach(function (item, index) {
                    searchitemli += "<li class='searchitem'  data-val='" + item.WorkSheetId + "'  data-title='" + item.WorkSheetTitle + "'  onclick=\"searchitemclick('" + item.WorkSheetTitle + "','searchlistitem','" + item.WorkSheetId + "')\"><a href='javascript: myfunction(x, y);'><strong>" + String(item.WorkSheetTitle).replace(
                        new RegExp("^" + searchText, "gi"),
                        "<b>$&</b>") + "</strong><em>" +
                        String(item.ClassTitle).replace(
                            new RegExp("^" + searchText, "gi"),
                            "<b>$&</b>") + " | " + String(item.SubjectTitle).replace(
                                new RegExp("^" + searchText, "gi"),
                                "<b>$&</b>")
                        + "</em></a></li>";
                    //item.WorkSheetTitle
                });
                $("#searchitemlist").html(searchitemli);

                var recentSearchItemslistArr = localStorage.getItem("recentsearchItems");
                var recentSearchItemslist = JSON.parse(recentSearchItemslistArr);
                if (recentSearchItemslist != null) {
                    $("#recentsearchitemlist").html("");
                    var recentsearchitemli = "";
                    recentSearchItemslist.forEach(function (item, index) {
                        recentsearchitemli += '<li><em></em><span onclick="searchitemclick(\'' + item.trim() + '\')">' + item.trim() + '</span><i class="recentsearchclear" onclick="recentsearchclear("' + item.trim() + '")" data-val=' + item.trim() + '>x</i></li>';
                    });

                    $("#recentsearchitemlist").html(recentsearchitemli);
                    if (recentSearchItemslist.length == 0) {
                        $("#rowSerchlist").hide();

                    } else {
                        $("#rowSerchlist").show();
                    }
                }

                $(".SerchContnt").show();
            },
            error: function (error) {
                $("#worksheetLoading").hide();
            }, complete: function () {
                $("#worksheetLoading").hide();
            }
        });
    }

    else {
        if (searchText.length >= 1) {
            $(".srchClear").show();
        } else {
            $(".srchClear").hide();
        }
    }
});


function searchitemclick(searchText, clickedby, workseetid) {
    
    text_lazy = "";
    $(window).data('ajaxready', true);
    if (searchText.length < 1) {

        $("#txtSearch").val("");
    }
    $(".SerchContnt").hide();

    if ($("#txtSearch").val().length >= 1) {
        $(".srchClear").show();
    } else {
        $(".srchClear").hide();
    }

    try {
        //commonLayer('Login', 'Generate OTP Registration');
        commonLayer(pageName, 'Search - ' + searchText);
    }
    catch (ex) {
        //console.log(ex);
    }

    var searchitemslistArr = localStorage.getItem("recentsearchItems");
    searchitemslist = JSON.parse(searchitemslistArr);
    if (searchitemslist == null) {
        searchitemslist = [];
    }
    if (searchitemslist.length >= 5) {
        searchitemslist.shift();
    }
    searchitemslist.push(searchText);
    var uniquesearchitemslist = searchitemslist.filter((value, index, array) => array.indexOf(value) === index);
    localStorage.setItem("recentsearchItems", JSON.stringify(uniquesearchitemslist));

    $("#searchitemlist").html("");
    if (clickedby == "searchlistitem") {
        searchText = workseetid;
    }
    ApplyingFilter(searchText)

}

$(".srchIcon").click(function () {
    var searchtext = $("#txtSearch").val();

    try {
        //commonLayer('Login', 'Generate OTP Registration');
        commonLayer(pageName, 'Search Icon - ' + searchtext);
    }
    catch (ex) {
        //console.log(ex);
    }

    if (searchtext != undefined && searchtext != "") {
        var searchitemslistArr = localStorage.getItem("recentsearchItems");
        searchitemslist = JSON.parse(searchitemslistArr);
        if (searchitemslist == null) {
            searchitemslist = [];
        }
        if (searchitemslist.length >= 5) {
            searchitemslist.shift();
        }
        searchitemslist.push(searchtext);
        var uniquesearchitemslist = searchitemslist.filter((value, index, array) => array.indexOf(value) === index);
        localStorage.setItem("recentsearchItems", JSON.stringify(uniquesearchitemslist));

        ApplyingFilter(searchtext)
    }
})

function recentsearchclear(clearitem) {
    var recentSearchItemslistArr = localStorage.getItem("recentsearchItems");
    var recentSearchItemslist = JSON.parse(recentSearchItemslistArr);
    if (recentSearchItemslist != null) {
        var recentSearchItemsindex = recentSearchItemslist.indexOf(clearitem);
        if (recentSearchItemsindex > -1) {
            recentSearchItemslist.splice(recentSearchItemsindex, 1)
            var uniquesearchitemslist = recentSearchItemslist.filter((value, index, array) => array.indexOf(value) === index);
            localStorage.setItem("recentsearchItems", JSON.stringify(uniquesearchitemslist));
        }
    }

    $("#recentsearchitemlist").html("");
    var recentsearchitemli = "";
    recentSearchItemslist.forEach(function (item, index) {
        recentsearchitemli += '<li><em></em><span onclick="searchitemclick(\'' + item.trim() + '\')">' + item.trim() + '</span><i class="recentsearchclear" onclick="recentsearchclear(\'' + item.trim() + '\')" data-val=' + item.trim() + '>x</i></li>';
    });
    $("#recentsearchitemlist").html(recentsearchitemli);
    if (recentSearchItemslist.length == 0) {
        setTimeout(function () {
            $("#rowSerchlist").hide();
            $("#rowSerchlistSerchContnt").show();
            $("#txtSearch").focus();
        });
    } else {
        setTimeout(function () {
            $("#rowSerchlistSerchContnt").show();
            $("#rowSerchlist").show();
            $("#txtSearch").focus();
        });
    }
}

$(document).click(function () {
    var searchText = $("#txtSearch").val();
    if (searchText != "" && searchText != undefined) {
        if (searchText.length == 0) { $("#searchitemlist").html(""); }
    }
});

$(document).on('click', '.srchClear', function () {
    $("#txtSearch").focus();
    $("#txtSearch").val("");
    setTimeout(function () {
        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext)
        $("#searchitemlist").html("");
        $("#rowSerchlist").show();
        $("#rowSerchlistSerchContnt").show();
    });
});

$(document).on('keydown', '#txtSearch', function (e) {
    setTimeout(function () {
        var listItems = $("#searchitemlist li");
        switch (e.keyCode) {
            case 38:
                if (listItems != undefined && listItems != null && listItems != "") {
                    $("#searchitemlist").focus();
                    listItems[currentLI].classList.remove("highlight");
                    currentLI = currentLI > 0 ? --currentLI : 0;
                    listItems[currentLI].classList.add("highlight"); // Highlight the new element
                }
                break;
            case 40:
                if (listItems != undefined && listItems != null && listItems != "") {
                    $("#searchitemlist").focus();
                    if (currentLI == -1) {
                        currentLI = 0;
                        listItems[currentLI].classList.add("highlight");

                        // Increase counter 
                    } else {
                        // Remove the highlighting from the previous element
                        listItems[currentLI].classList.remove("highlight");

                        currentLI = currentLI < listItems.length - 1 ? ++currentLI : listItems.length - 1; // Increase counter 
                        listItems[currentLI].classList.add("highlight");       // Highlight the new element
                    }
                }
                break;
            case 13:
                if (listItems != undefined && listItems != null && listItems != "") {
                    if (currentLI != -1) {
                        searchitemclick(listItems[currentLI].attributes['data-title'].value);
                    }
                    else {
                        searchitemclick($("#txtSearch").val());
                    }
                }
                break;
        }
    }, 500);
});

function BindPaging() {

    $(".first-page").click(function () {

        try {
            commonLayer(pageName, 'Paging - First Page');
        }
        catch (ex) {
            //console.log(ex);
        }

        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, $(this).attr("data-val"))
    })

    $(".previous-page").click(function () {

        try {
            commonLayer(pageName, 'Paging - Previous Page');
        }
        catch (ex) {
            //console.log(ex);
        }

        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, $(this).attr("data-val"))
    })

    $(".current-page").click(function () {

        try {
            commonLayer(pageName, 'Paging - Current Page ' + currentPage);
        }
        catch (ex) {
            //console.log(ex);
        }

        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, $(this).attr("data-val"))
    })

    $('.next-page').click(function () {

        try {
            commonLayer(pageName, 'Paging - Next Page');
        }
        catch (ex) {
            //console.log(ex);
        }

        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, $(this).attr("data-val"))
    })

    $(".last-page").click(function () {
        try {
            commonLayer(pageName, 'Paging - Last Page');
        }
        catch (ex) {
            //console.log(ex);
        }
        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, $(this).attr("data-val"))
    })
}

function ApplyingFilter(searchtext, currentPage) {
    var currentPage = currentPage;
    var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    var SortBy = -1;

    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");

    if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
        SortBy = $("#dropsortBy").val();
    }
    $("#WorksheetList").html("");
    text_lazy = searchtext;
    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, currentPage, searchtext, SortBy);
}

function StructuredProgramReset() {
    window.location.href = "/worksheets";
}

function CaptureUserSubmitData(searchtxt) {

    var message = $("#usernorecordfoundmessage").val();
    if (message != '' || message != "" || message != null) {
        var searchText = searchtxt;//Input.searchText;

        var userInput = {
            message: message,
            searchText: searchText
        };

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/umbraco/Surface/StructuredProgram/CaptureUserSubmitData",
            data: JSON.stringify(userInput),
            success: function (responce) {
                $("#usernorecordfoundmessage").val('');
                $(".overlaynorecordfound").hide();
            },

            error: function (error) {
                $("#worksheetLoading").hide();
            }, complete: function () {
                $("#worksheetLoading").hide();
            }

        });
    }
}