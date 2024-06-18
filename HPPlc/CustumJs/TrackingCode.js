
function Loginlayer(userId) {

	var pagename = $("#pageName").val();

	try {
		dataLayer.push({
			event: 'e_userLogin',
			userID: userId
		});
	}
	catch (ex) {
		//console.log(ex);
	}

	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename,
			linkID: 'Login Successfull'
		});
	}
	catch (ex) {
		//console.log(ex);
	}

	try {
	window.dispatchEvent(new CustomEvent("ANALYTICS.LOGIN", {
		detail: {
			eventLinkType: "e_userLogin",
			userID: userId,
		}
	}))
	}
	catch (ex) {
		//console.log(ex);
	}

	//new Promise((resolve) => {
	//	window.dispatchEvent(new CustomEvent("ANALYTICS.LOGIN", {
	//		detail: {
	//			eventLinkType: "e_userLogin",
	//			userID: userId,
	//		}
	//	}))
	//	setTimeout(resolve, 200)
	//}).then(() => {
	//	window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
	//		detail: {
	//			eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
	//			eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
	//			eventPlacement: pagename, //PageName
	//			eventLinkID: 'Login Successfull'  // CTA / button / Link Name
	//		}
	//	}));
	//})
}

//Registration GA
async function RegistrationLayer(Userid, referCode) {

	try {

		var pagename = $("#pageName").val();

		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename,
			linkID: 'Registration'
		});

		dataLayer.push({
			event: 'e_userRegister',
			linkPlacement: '',
			linkID: referCode,
			userID: Userid
		});

		
	}
	catch (ex) {
		//console.log(ex);
	}
}

//Registration Avtar1
async function RegistrationLayerAvatar() {

	try {
		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: 'Registration'  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

//Registration Avtar2
async function RegistrationLayerAvatarUserRegister() {

	try {
		window.dispatchEvent(new CustomEvent("ANALYTICS.REGISTRATION", {
			detail: {
				eventLinkID: "",
				eventPlacement: "",
				userID: "",
				eventLinkType: "e_userRegister"
			}
		}));

	}
	catch (ex) {
		//console.log(ex);
	}
}

//Open video
function OpenVideo(VideoName, VideoLength) {
	try {
		dataLayer.push({
			event: 'e_videoLoad',
			videoName: VideoName,
			videoDuration: VideoLength
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'open video',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_videoLoad", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: VideoName, //PageName
				eventLinkID: VideoLength  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}
//Start or resume play
function PlayVideo(VideoName, VideoPlayedInSec) {
	try {
		dataLayer.push({
			event: 'e_videoPlay',
			videoName: VideoName,
			videoCuePoint: VideoPlayedInSec
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'video play',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_videoPlay", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: VideoName, //PageName
				eventLinkID: VideoPlayedInSec  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}
//Stop or pause play (user initiated)
function PauseVideo(VideoName) {
	try {
		dataLayer.push({
			event: 'e_videoPause',
			videoName: VideoName
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'video pause',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_videoPause", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: VideoName, //PageName
				eventLinkID: ''  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function addSubscribeDataLayer(detailData, ButtonTitle, navigateUrl, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: detailData + '-' + ButtonTitle // 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'subscription',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: detailData + '-' + ButtonTitle  // CTA / button / Link Name
			}
		}));

		window.location.href = navigateUrl;
	}
	catch (ex) {
		//console.log(ex);
	}
}


function addSubscribeDataLayerWithoutRedirection(detailData, ButtonTitle, navigateUrl, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: detailData + '-' + ButtonTitle // 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: detailData + '-' + ButtonTitle  // CTA / button / Link Name
			}
		}));

		//window.location.href = navigateUrl;
	}
	catch (ex) {
		//console.log(ex);
	}
}

function addDownloadDataLayer(detailData, ButtonTitle, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: detailData + '-' + ButtonTitle // 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'download',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: detailData + '-' + ButtonTitle  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
	return true;
}

function printTracker(data, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: data + '- Print'// 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'print',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: data + '- Print'  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function addVideoSubscribeDataLayer(detailData, buttonTitle, navigateUrl, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Videos
			linkID: detailData + '-' + buttonTitle // 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'play',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: detailData + '-' + buttonTitle  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
	window.location.href = navigateUrl;
}

function addVideoDownloadDataLayer(detailData, buttonTitle, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: detailData + '-' + buttonTitle // 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'download',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: detailData + '-' + buttonTitle  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
	return true;
}

function printVideoTracker(layerDetail, pagename) {
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename, //Worksheet
			linkID: layerDetail + '-Print'// 3-4Years| Week1|Category|Title
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'print',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: layerDetail + '-Print' // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function commonLayer(pagename, cta) {
	if (pagename == '')
		pagename = $("#pageName").val();

	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: pagename,
			linkID: cta
		});

		//Avatar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: pagename, //PageName
				eventLinkID: cta  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function commonShareLayer(linkplacement, linkid) {
	//alert(linkplacement + ',' + linkid);
	try {
		dataLayer.push({
			event: 'e_linkClick',
			linkPlacement: linkplacement,
			linkID: linkid
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'share',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: linkplacement, //PageName
				eventLinkID: linkid  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function commonFilterLayer(filtercategory, filtervalue, sortValue) {
	var clrface = false;
	var ftrChk = true;
	if (filtercategory.toLocaleLowerCase().indexOf("reset") != -1) {
		clrface = true;
		ftrChk = false;
	}
	try {
		dataLayer.push({
			event: 'e_facet',
			filterChecked: ftrChk,
			filterCategories: filtercategory,
			filterValue: filtervalue,
			sortType: sortValue,
			clearFacet: clrface
		});
	}
	catch (ex) {
		//console.log(ex);
	}
}


function CouponNotValid(CouponCode) {
	try {
		//alert('not valid - ' + CouponCode);
		dataLayer.push({
			event: 'e_couponError',
			couponCode: CouponCode
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_couponError", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: CouponCode, //PageName
				eventLinkID: ''  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}


function CouponValid(CouponCode) {
	try {
		//alert('valid - ' + CouponCode);
		dataLayer.push({
			event: 'e_couponApplied',
			couponCode: CouponCode
		});

		//Avtar Script

		window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
			detail: {
				eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
				eventLinkType: "e_couponApplied", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
				eventPlacement: CouponCode, //PageName
				eventLinkID: ''  // CTA / button / Link Name
			}
		}));
	}
	catch (ex) {
		//console.log(ex);
	}
}

function TrackWithUserId(pageName, cta, UserId) {
	dataLayer.push({
		event: 'e_linkClick',
		linkPlacement: pageName,
		linkID: cta,
		userID: UserId
	});

	//Avtar Script

	window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
		detail: {
			eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
			eventLinkType: "e_linkClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
			eventPlacement: pageName, //PageName
			eventLinkID: cta  // CTA / button / Link Name
		}
	}));
}

function TrackSortBy(sortText) {
	dataLayer.push({
		event: 'e_searchSortBy',
		sortType: sortText
	});

	//Avtar Script

	window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
		detail: {
			eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
			eventLinkType: "e_searchSortBy", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
			eventPlacement: '', //PageName
			eventLinkID: sortText  // CTA / button / Link Name
		}
	}));
}

function TrackSearchOnTextChange(seachText, noOfItems) {
	dataLayer.push({
		event: 'e_searchResults',
		searchTerm: seachText,
		numSearchResults: noOfItems
	});

	//Avtar Script

	window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
		detail: {
			eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
			eventLinkType: "e_searchResults", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
			eventPlacement: seachText, //PageName
			eventLinkID: noOfItems  // CTA / button / Link Name
		}
	}));
}

function TrackSearchOnClick(seachText, clickedItem) {
	dataLayer.push({
		event: 'e_searchAutoClick',
		searchTerm: seachText,
		clickedTerm: clickedItem,
		searchCategory: '' //optional
	});

	//Avtar Script

	window.dispatchEvent(new CustomEvent("ANALYTICS.TIER1", {
		detail: {
			eventName: 'click',//Event descriptor - describes event that is being initiated eg:-download/click/pageLoad/exit/videoStart/etc 
			eventLinkType: "e_searchAutoClick", //The link type being clicked; eg: -e_linkClick / e_learnClick Etc
			eventPlacement: seachText, //PageName
			eventLinkID: clickedItem  // CTA / button / Link Name
		}
	}));
}