
var Age = null;
var Volume = null;
var Category = null;
var Page = null;
$(document).ready(function () {
	//GetWorkSheetList("", "", "");
	if (window.location.href.indexOf("/worksheets/") > -1) {
		Page = "worksheetdtls";
	}
	Age = $('#vrAgrGroupe');
	Volume = $('#vrVolumeGroupe');
	Category = $('#vrCategories'); 
	if (Age != null && Age != undefined && Age.length > 0) {
		if (Page == null || Page == undefined || Page.length == 0) {
			GetAgeGroupList();
		}
	//} else {
	//	GetWorkSheetList("", "", "");
	}
	if (Volume != null && Volume != undefined && Volume.length > 0) {
		GetVolumeList("");
		$('.select-week').multipleSelect({
			placeholder: 'Select Week',
			selectAll: false
		});
	}
	if (Category != null && Category != undefined && Category.length > 0) {
		$('.select-category').multipleSelect({
			placeholder: 'Select Category',
			selectAll: false
		});
		GetCategoryList("", "");
	}
});



function GetAgeGroupList() {
	$.ajax({
		type: "GET",
		contentType: "application/json",
		url: "/umbraco/Surface/WorkSheet/GetAgeGroupList",
		success: function (responce) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Age != null && Age != undefined && Age.length > 0) {
						$("#vrAgrGroupe option").remove();
						$.each(responce.Result, function (i, item) {
							$('#vrAgrGroupe').append($('<option>', {
								value: item.ItemValue,
								text: item.ItemName
							}));
						});
						$('.select-age').multipleSelect({
							placeholder: 'Select Age',
							selectAll: false
						});
					}
					//if (Volume == null || Volume == undefined || Volume.length == 0) {
					//	GetWorkSheetList("", "", "");
					//}
				}
			}
		},
		error: function (error) {
			console.log(error)
		}, complete: function () {
			// $("#worksheetLoading").css('display', 'none');
		}
	});
}

function GetVolumeList(SelectedAvge) {
	$.ajax({
		type: "GET",
		contentType: "application/json",
		url: "/umbraco/Surface/WorkSheet/GetVolumeList?AgeItemValue=" + SelectedAvge,
		success: function (responce) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Volume != null && Volume != undefined && Volume.length > 0) {
						$("#vrVolumeGroupe option").remove();
						const sb = document.querySelector('#vrVolumeGroupe');
						$.each(responce.Result, function (i, item) {
							const option = new Option(item.ItemName, item.ItemValue);
							sb.add(option, undefined);
						});
						$('.select-week').multipleSelect("destroy").multipleSelect({
							placeholder: 'Select Week',
							selectAll: false
						});
					}
					//if (Category == null || Category == undefined || Category.length == 0) {
					//	if (Page == null) {
					//		GetWorkSheetList(
					//			$("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", "),
					//			$("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", "),
					//			"");
					//	}
					//	else {
					//		WorksheetTutorialGetById(0);
					//	}
					//}
				}
			}
		},
		error: function (error) {
			//console.log(error)
		}, complete: function () {
			// $("#worksheetLoading").css('display', 'none');
		}
	});
}
function GetCategoryList(SelectedAvge, SelectedVolume) {
	$.ajax({
		type: "GET",
		contentType: "application/json",
		url: "/umbraco/Surface/WorkSheet/GetCategoryList?AgeItemValue=" + SelectedAvge + "&VolumeItemValue=" + SelectedVolume,
		success: function (responce) {
			if (responce != null && responce != undefined && responce.StatusCode == 200) {
				if (responce.Result != null) {
					if (Category != null && Category != undefined && Category.length > 0) {
						$("#vrCategories option").remove();
						const sb = document.querySelector('#vrCategories');
						$.each(responce.Result, function (i, item) {
							const option = new Option(item.ItemName, item.ItemValue);
							sb.add(option, undefined);
						});
						$('.select-category').multipleSelect("destroy").multipleSelect({
							placeholder: 'Select Category',
							selectAll: false
						});
					}
					if (Page == null) {
						GetWorkSheetList(
							$("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", "),
							$("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", "),
							$("#vrCategories option:selected").map(function () { return this.value }).get().join(", "));
					}
					else {
						WorksheetTutorialGetById(0);
					}
				}
			}
		},
		error: function (error) {
			//console.log(error)
		}, complete: function () {
			// $("#worksheetLoading").css('display', 'none');
		}
	});
}




$('.fltr-box #vrAgrGroupe').change(function () {

	var vrAgrGroupe = $("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", ");
	var Volume = $("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", ");
	var Category = $("#vrCategories option:selected").map(function () { return this.value }).get().join(", ");

	if (Volume != null && Volume != undefined && Volume.length > 0) {
		GetVolumeList(vrAgrGroupe);
	}
	if (Category != null && Category != undefined && Category.length > 0) {
		GetCategoryList(vrAgrGroupe, vrVolumeGroupe);
	} else {
		GetWorkSheetList(vrAgrGroupe, "", "");
	}
});

$('.fltr-box #vrVolumeGroupe').change(function () {
	var vrAgrGroupe = $("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", ");
	var vrVolumeGroupe = $("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", ");
	
	if (Page == null) {
		if (Category != null && Category != undefined && Category.length > 0) {
			GetCategoryList(vrAgrGroupe, vrVolumeGroupe);
		} else {
			GetWorkSheetList(vrAgrGroupe, vrVolumeGroupe, "");
		}
	}
	else {
		WorksheetTutorialGetById(0);
		GetCategoryList("worksheetdtls", vrVolumeGroupe);
	}
});

$('.fltr-box #vrCategories').change(function () {
	var vrAgrGroupe = $("#vrAgrGroupe option:selected").map(function () { return this.value }).get().join(", ");
	var vrVolumeGroupe = $("#vrVolumeGroupe option:selected").map(function () { return this.value }).get().join(", ");
	var vrCategoryGroupe = $("#vrCategories option:selected").map(function () { return this.value }).get().join(", ");

	if (Page == null) {
		GetWorkSheetList(
			vrAgrGroupe, vrVolumeGroupe, vrCategoryGroupe);
	}
	else {
		WorksheetTutorialGetById(0);
	}
});