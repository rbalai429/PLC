$(document).ready(function () {
	share();


	$("#BlogLoadmore").click(function () {
		LoadMore();
	});
});


function LoadMore() {
	var blogBindedOnPage = $("#blogBindedOnPage").val();
	var BlogsToBeDisplay = $("#HowManyBlogsToBeDisplay").val();
	var TotalCountOfBlogs = $("#TotalCountOfBlogs").val();
	var OlderPostsTitle = $("#OlderPostsTitle").val();
	$("#blogListingLoading").css('display', 'block');


	var Input = {
		BindedOnPage: blogBindedOnPage,
		HowManyBlogsToBeDisplay: BlogsToBeDisplay,
		TotalCountOfBlogs: TotalCountOfBlogs,
		OlderPostTitle: OlderPostsTitle
	};
	$.ajax({
		type: 'POST',
		url: "/umbraco/Surface/Blog/GetBlogsListing",
		data: JSON.stringify(Input),
		contentType: "application/json",
		success: function (data) {
			$("#blogListing").html("");
			$("#blogListing").html(data);

			var totalBind = (parseInt(blogBindedOnPage) + parseInt(BlogsToBeDisplay));
			$("#blogBindedOnPage").val(totalBind);
			
			$("#BlogLoadmore").click(function () {
				LoadMore();
			});
		}, error: function (error) {
			$("#blogListingLoading").css('display', 'none');
		}, complete: function () {
			$("#blogListingLoading").css('display', 'none');
		}
	});
}