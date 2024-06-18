share()
function share() {

	$(".aFBShare").click(function () {
		var ItemVal = $(this).find('span').html();
		publish(ItemVal);
	});
	$(".aWHTAppSH").click(function () {
		var ItemVal = $(this).find('span').html();
		if (/Mobi/.test(navigator.userAgent)) {
			window.open('whatsapp://send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
		else {
			window.open('https://web.whatsapp.com/send?text=' + encodeURIComponent(ItemVal), 'sharer', 'toolbar=0,status=0,width=550,height=400');
		}
	});
	$(".aMailSh").click(function () {
		var FullString = $(this).find('span').html(); 
		var emailBody = FullString.split('`')[0];
		var email = '';
		var subject = FullString.split('`')[1];
		
		window.location = 'mailto:' + email + '?body=' + encodeURIComponent(emailBody) + '&subject=' + subject;
	});
	$(".aCopyText").click(function () {
		var $temp = $("<input>");
		$("body").append($temp);
		$temp.val($(this).find('span').text()).select();
		document.execCommand("copy");
		$("#copylinkId").text("Copied.");
		$("#copyImageId").hide();
		$temp.remove();
		
	});
	$(".aMSGShare").click(function () {
		var $temp = $("<input>");
		$("body").append($temp);
		$temp.val($(this).find('span').text()).select();
		document.execCommand("copy");
		$temp.remove();

	});
}