$(document).ready(function () {
    $('.doneBtn').click(function () {
        $(this).parents(".chchlist").removeClass("listFltr");
    });

    var pageName = $("#page").val();
});
$(".classes").change(function () {
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

    try {
        commonLayer(pageName, 'Flter Class Wise - ' + vrAgrGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }


    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
})

$(".subjects").change(function () {
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

    try {
        commonLayer(pageName, 'Flter Subject Wise - ' + vrSubjectGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);

})

$(".topics").change(function () {
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
    try {
        commonLayer(pageName, 'Flter Topics Wise - ' + vrTopicsGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);
})

$(".paymenttypes").change(function () {
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

    try {
        commonLayer(pageName, 'Flter Payment Wise - ' + vrPaidGroupe);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", SortBy);

})
