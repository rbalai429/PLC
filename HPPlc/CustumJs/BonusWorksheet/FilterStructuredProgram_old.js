var pageName = $("#page").val();

$(document).ready(function () {
    $('.doneBtn').click(function () {
        $(this).parents(".chchlist").removeClass("listFltr");
    });
});
$(".classes").change(function () {
    var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(", ");

    try {
        var fiterCategory = 'Class';
        var sortByVal = $("#dropsortBy").val();
        var sorteBy = "";
        if (sortByVal != null && sortByVal != "" && sortByVal != undefined && sortByVal != "0")
            sorteBy = $("#dropsortBy :selected").text()

        commonFilterLayer(fiterCategory, vrAgrGroupe, sorteBy);
    }
    catch (ex) {
        //console.log(ex);
    }


    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", -1);
})

$(".subjects").change(function () {
    var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(", ");

    try {
        var fiterCategory = 'Subject';
        var sortByVal = $("#dropsortBy").val();
        var sorteBy = "";
        if (sortByVal != null && sortByVal != "" && sortByVal != undefined && sortByVal != "0")
            sorteBy = $("#dropsortBy :selected").text()

        commonFilterLayer(fiterCategory, vrAgrGroupe, sorteBy);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", -1);

})

$(".topics").change(function () {
    var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(", ");


    try {
        var fiterCategory = 'Topics';
        var sortByVal = $("#dropsortBy").val();
        var sorteBy = "";
        if (sortByVal != null && sortByVal != "" && sortByVal != undefined && sortByVal != "0")
            sorteBy = $("#dropsortBy :selected").text()

        commonFilterLayer(fiterCategory, vrAgrGroupe, sorteBy);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", -1);
})

$(".paymenttypes").change(function () {
    var vrAgrGroupe = '';
    var vrSubjectGroupe = '';
    var vrTopicsGroupe = '';
    var vrPaidGroupe = '';
    vrAgrGroupe = $(".classes:checked").map(function () { return this.value }).get().join(", ");
    vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
    vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");
    vrPaidGroupe = $(".paymenttypes:checked").map(function () { return this.value }).get().join(", ");


    try {
        var fiterCategory = 'PaymentType';
        var sortByVal = $("#dropsortBy").val();
        var sorteBy = "";
        if (sortByVal != null && sortByVal != "" && sortByVal != undefined && sortByVal != "0")
         sorteBy = $("#dropsortBy :selected").text()

        commonFilterLayer(fiterCategory, vrAgrGroupe, sorteBy);
    }
    catch (ex) {
        //console.log(ex);
    }

    GetStructuredProgramListFilter(vrAgrGroupe, vrSubjectGroupe, vrTopicsGroupe, vrPaidGroupe, 1, "", -1);

})
