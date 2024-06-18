
var pageName = $("#page").val();
//var vrAgrGroupe = '';
//    var vrSubjectGroupe = '';
//    var vrTopicsGroupe = '';
//    var vrPaidGroupe = '';
//    //var SortBy = -1;

//    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
//    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
//    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
//    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");


$(document).ready(function () {
    $('.doneBtn').click(function () {
        $(this).parents(".chchlist").removeClass("listFltr");
    });
});

$(document).on('change', '.classes', function () {
    var vrAgrGroupe = '';
    //var vrSubjectGroupe = '';
    //var vrTopicsGroupe = '';
    //var vrPaidGroupe = '';
    //var SortBy = -1;

    vrAgrGroupe = $(".classes:checked").map(function () { return this.title }).get().join(",");
    //vrSubjectGroupe = $(".subjects:checked").map(function () { return this.title }).get().join(",");
    //vrTopicsGroupe = $(".topics:checked").map(function () { return this.title }).get().join(",");
    //vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");

    //if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
    //    SortBy = $("#dropsortBy").val();
    //}

    //var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");

    //GetSubjectsList(selectedvrAgrGroupe);

    //GetTopicsList(selectedvrAgrGroupe, null);
    RedirectonPage();
    try {
        commonLayer(pageName, 'Flter Class Wise - ' + vrAgrGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }


    //GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
    //var searchtext = $("#txtSearch").val();
    //ApplyingFilter(searchtext, 1)
})

//$(".classes").change(function () {
//})

$(document).on('change', '.subjects', function () {
    //var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    //var vrTopicsGroupe = '';
    //var vrPaidGroupe = '';
    //var SortBy = -1;

    //vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.title }).get().join(",");
    //vrTopicsGroupe = $(".topics:checked").map(function () { return this.title }).get().join(",");
    //vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");


    //if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
    //    SortBy = $("#dropsortBy").val();
    //}
    //var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");

    RedirectonPage();
    //GetTopicsList(selectedvrAgrGroupe, vrSubjectGroupe);
    try {
        commonLayer(pageName, 'Flter Subject Wise - ' + vrSubjectGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }
    //var searchtext = $("#txtSearch").val();
    //ApplyingFilter(searchtext, 1)
    //GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
})

//$(".subjects").change(function () {
//})

$(document).on('change', '.topics', function () {
    //var vrAgrGroupe = '';
    //var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    //var vrPaidGroupe = '';
    //var SortBy = -1;

    //vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
    //vrSubjectGroupe = $(".subjects:checked").map(function () { return this.title }).get().join(",");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.title }).get().join(",");
    //vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");


    //if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
    //    SortBy = $("#dropsortBy").val();
    //}
    //var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");

    RedirectonPage();

    try {
        commonLayer(pageName, 'Flter Topics Wise - ' + vrTopicsGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }
    //var searchtext = $("#txtSearch").val();
    //ApplyingFilter(searchtext, 1)
    //GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
})

//$(".topics").change(function () {
//})

$(document).on('change', '.paymenttypes', function () {
    //var vrAgrGroupe = '';
    //var vrSubjectGroupe = '';
    //var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    //var SortBy = -1;

    //vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(",");
    //vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
    //vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(",");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.title }).get().join(",");


    //if ($("#dropsortBy").val() != undefined && $("#dropsortBy").val() != null && $("#dropsortBy").val() != "") {
    //    SortBy = $("#dropsortBy").val();
    //}
    RedirectonPage();
    try {
        commonLayer(pageName, 'Flter Payment Wise - ' + vrPaidGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }
    RedirectonPage();
    //GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
    //var searchtext = $("#txtSearch").val();
    //ApplyingFilter(searchtext, 1)
})

//$(".paymenttypes").change(function () {
//})

function GetSubjectsList(classes) {

    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: classes
    };
    $.post("/umbraco/Surface/StructuredProgram/GetFilteredSubjects",
        {
            Input
        },
        function (data, status) {

            $("#filtersubjectscheckbox").html("").append(data);
        });
}



function GetSubjectsListBasedTopic(topics) {

    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedTopics: topics
    };
    $.post("/umbraco/Surface/StructuredProgram/GetFilteredSubjectsBasedTopics",
        {
            Input
        },
        function (data, status) {

            $("#filtersubjectscheckbox").html("").append(data);
        });
}

function GetTopicsList(classes, subjects) {

    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: classes,
        selectedSubject: subjects
    };
    $.post("/umbraco/Surface/StructuredProgram/GetFilteredTopics",
        {
            Input
        },
        function (data, status) {

            $("#filtertopiccheckbox").html("").append(data);
        });
}

function RedirectonPage() {

    var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");
    var vrSubjectGroupe = $(".subjects:checked").map(function () { return this.title }).get().join(",");
    var vrTopicsGroupe = $(".topics:checked").map(function () { return this.title }).get().join(",");
    var vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(",");
    var searchtext = $("#txtSearch").val();

    var Input = {
        CultureInfo: $("#hdnCultureInfo").val(),
        selectedAgeGroup: selectedvrAgrGroupe,
        selectedSubject: vrSubjectGroupe,
        selectedTopics: vrTopicsGroupe,
        selectedPaid: vrPaidGroupe,
        searchText: searchtext
    };
    $.post("/umbraco/Surface/StructuredProgram/GetRedirectUrl",
        {
            Input
        },
        function (data, status) {

            window.location = data;
        });
}
$(function () {
    
    var classpage = $("#class").val();
    if (classpage != undefined && classpage != "" && classpage != null) {
        $(".classes").filter(function (index, obj) {
            if (obj.value === classpage) {
                $(obj).prop('checked', true);
            }
        });
        var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");
        GetSubjectsList(selectedvrAgrGroupe);
        GetTopicsList(selectedvrAgrGroupe, null);
    }
    //setTimeout("", 2000);
    var subjectspage = $("#subject").val();
    if (subjectspage != undefined && subjectspage != "" && subjectspage != null) {
        $(".subjects").filter(function (index, obj) {
            if (obj.value === subjectspage) {
                $(obj).prop('checked', true);
            }
        });
        var selectedvrAgrGroupe = $(".classes:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");
        vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(",");
        GetTopicsList(selectedvrAgrGroupe, vrSubjectGroupe);
    }
    
    var topicspage = $("#topic").val();
    if (topicspage != undefined && topicspage != "" && topicspage != null) {
        $(".topics").filter(function (index, obj) {
            if (obj.value === topicspage) {
                $(obj).prop('checked', true);
            }
        });
        //GetSubjectsListBasedTopic(topicspage);
        //var selectedvrAgrGroupe = $(".topics:checked").map(function () { return this.attributes["alternateclassname"].value }).get().join(",");
        //GetSubjectsList(selectedvrAgrGroupe);
        //GetTopicsList(selectedvrAgrGroupe, null);
    }
    var querystr = getParameterByName("q");
    if (querystr != undefined && querystr != "" && querystr != null) {
        $.post("/umbraco/Surface/StructuredProgram/DecryptQueryString",
            {
                queryString: querystr
            },
            function (data, status) {
                classpage = getParameterUrlByName(data, "classes");
                subjectspage = getParameterUrlByName(data, "subjects");
                topicspage = getParameterUrlByName(data, "topics");
                paymenttypes = getParameterUrlByName(data, "paymenttypes");
                searchtext = getParameterUrlByName(data, "searchtext");
                if (searchtext != undefined && searchtext != "" && searchtext != null) {
                    $("#txtSearch").val(searchtext);
                }
                if (classpage != undefined && classpage != "" && classpage != null) {
                    $(".classes").filter(function (index, obj) {
                        if (classpage.split(',').indexOf($(obj).attr("alternateclassname")) != -1) {
                            $(obj).prop('checked', true);
                        }
                    });
                    GetSubjectsList(classpage);
                    if (subjectspage == undefined || subjectspage == "" || subjectspage == null) {
                        GetTopicsList(classpage, null);
                    }
                }
                setTimeout(function () {
                    if (subjectspage != undefined && subjectspage != "" && subjectspage != null) {
                        $(".subjects").filter(function (index, obj) {
                            if (subjectspage.split(',').indexOf(obj.title) != -1) {
                                $(obj).prop('checked', true);
                            }
                        });
                        GetTopicsList(classpage, subjectspage);
                    }
                }, 1500);
                setTimeout(function () {
                    if (topicspage != undefined && topicspage != "" && topicspage != null) {
                        $(".topics").filter(function (index, obj) {
                            if (topicspage.split(',').indexOf(obj.title) != -1) {
                                $(obj).prop('checked', true);
                            }
                        });
                    }
                }, 2000);
                setTimeout(function () {
                    if (paymenttypes != undefined && paymenttypes != "" && paymenttypes != null) {
                        $(".paymenttypes").filter(function (index, obj) {
                            if (paymenttypes.split(',').indexOf(obj.value) != -1) {
                                $(obj).prop('checked', true);
                            }
                        });
                    }
                });
                $('#worksheetLoading').show();
                setTimeout(function () {
                    var searchtext = $("#txtSearch").val();
                    ApplyingFilter(searchtext, 1)
                }, 2000);

            });
    } else {
        $('#worksheetLoading').show();
        var searchtext = $("#txtSearch").val();
        ApplyingFilter(searchtext, 1);
    }
})

function getParameterByName(name) {
    var regexS = "[\\?&]" + name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(window.location.search);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function getParameterUrlByName(url, name) {
    var regexS = name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(url);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}