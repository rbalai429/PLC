
function LoginForTeachers(mode, downloadUrl, Ispaid, trackingdata, source, nodeId, msgIdforLoginwndw, AgeGroup) {

	try {
		
		CaptureClickedEvent(mode, downloadUrl, Ispaid, trackingdata, source, nodeId, "teacher", AgeGroup);

		FetchLoginMessageAjax(msgIdforLoginwndw);

		var pageName = 'teacher';
		$("#page").val('teacher');
		if (mode == "print") {
			commonLayer(pageName, "Clicked on Print");
		}
		else if (mode == "dwnld") {
			commonLayer(pageName, "Clicked on Download");
		}
	}
	catch (ex) {
		//console.log('error');
	}

	$(".overlayLogin").show();
}

function downloadDocmntTeachers(mode, downloadUrl, Ispaid, trackdata, source, nodeId, msgIdforLoginwndw, AgeGroup) {

	//try {
	//	if (mode == "print") {
	//		commonLayer(pageName, "Clicked on Print Icon");
	//	}
	//	else if (mode == "dwnld") {
	//		commonLayer(pageName, "Clicked on Download Icon");
	//	}
	//}
	//catch (ex) {
	//	//console.log('error');
	//}
	
	CaptureClickedEvent(mode, downloadUrl, Ispaid, trackdata, source, nodeId, "teacher", AgeGroup);

	PrintDownloadForWorksheetsTeachers(mode, downloadUrl, Ispaid, trackdata, '', source, nodeId, AgeGroup);

	try {
		FetchLoginMessageAjax(msgIdforLoginwndw);
	}
	catch {
		//console.log('');
	}
}



function PrintDownloadForWorksheetsTeachers(mode, downloadUrl, Ispaid, trackdata, evnt, source, nodeId, AgeGroup) {
	
	if (mode != '' && mode != "" && mode != null && mode != undefined) {

		try {
			if (mode == "dwnld") {
				addDownloadDataLayer("Teachers", trackdata, pageName);
			}
			if (mode == "print") {
				printTracker("Teachers", trackdata, pageName);
			}
		}
		catch (ex) {
			//console.log('error');
		}

			var Input = {
				AgeGroup: AgeGroup,
				Source: source,
				NodeId: nodeId
			};

			$.ajax({
				type: "POST",
				url: "/umbraco/Surface/Teachers/DownloadEligibility",
				data: Input,
				success: function (e) {
					
					if (e.Result == "0" && mode == "print") {
						if (evnt == "justlogin") {
							location.href = window.location.href;
						}
						else {
							if (e.CurrentWorksheetDownloaded > 0) {
								PrintWorkSheet(downloadUrl);
							}
							else if (e.DownloadedWorksheet >= e.LimitOfDownloads) {//User already 2 worksheets downloaded
								var downloadLimitTextPrint = "You’ve reached a download/print limit!";

								$('.cntOfDownload').html(downloadLimitTextPrint);
								$("#PayRequirement").show();
								
							}
							else {
								PrintWorkSheet(downloadUrl);
							}
						}
					}
					else if (e.Result == "0" && mode == "dwnld") {
						if (e.CurrentWorksheetDownloaded > 0) { //User already downloaded
							var existingdownloadStrs = "Worksheet downloaded successfully! ";

							$(".cntOfDownload").html(existingdownloadStrs);
							
							if (evnt == "justlogin") {
								$("#PayRequirementJustLogin").show();
							}
							else {
								$("#PayRequirement").show();
							}

							location.href = downloadUrl;
						}
						else if (e.DownloadedWorksheet >= e.LimitOfDownloads) {//User already 2 worksheets downloaded
							var downloadLimitText = "You’ve reached a download/print limit!";

							$('.cntOfDownload').html(downloadLimitText);
							if (evnt == "justlogin") {
								$("#PayRequirementJustLogin").show();
							}
							else {
								$("#PayRequirement").show();
							}
							$(".viewpricing").show();
						}
						else {
							var downloadStrs = "";
							var CntOfDownloaded = 0;
							if (e.CurrentWorksheetDownloaded > 0)
								CntOfDownloaded = e.DownloadedWorksheet
							else
								CntOfDownloaded = (e.DownloadedWorksheet + 1)

							if (e.DownloadedWorksheet == 0) {
								downloadStrs = "Lesson plan downloaded successfully! " + (e.LimitOfDownloads - CntOfDownloaded).toString() + " download left.";
							}
							else {
								downloadStrs = "Worksheet downloaded successfully! " + (e.LimitOfDownloads - CntOfDownloaded).toString() + " downloads left.";

							}

							$('.cntOfDownload').html(downloadStrs);
							$(".viewpricing").show();

							if (evnt == "justlogin") {
								$("#freetrialJustLogin").show();
							}
							else {
								$("#freetrialLoggrdIn").show();
							}

							location.href = downloadUrl;
						}
					}
					else if (e.Result == "1" && mode == "print") {
						//User Subscribed
						if (evnt == "justlogin") {
							location.href = window.location.href;
						}
						else {
							if (e.CurrentWorksheetDownloaded > 0 || e.IsEligibleForDwnldWorksheet == 1) {
								PrintWorkSheet(downloadUrl);
							}
							else {
								$(".cntOfDownload").html("You’ve reached a download/print limit!");
								$("#paidDwnld").show();
								
							}
						}
					}
					else if (e.Result == "1" && mode == "dwnld") {
						var popMessage = "";
						var DaysLeft = "";
						var viewpricingactive = 0;

						DaysLeft = "Your plan is about to expire! Renew today to enjoy uninterrupted access.";

						if (e.CurrentWorksheetDownloaded > 0) {
							var existingdownloadStrspaid = "Worksheet downloaded successfully! ";

							$(".cntOfDownload").html(existingdownloadStrspaid);
							$("#PayRequirement").show();

							location.href = downloadUrl;
						}
						else if (e.IsEligibleForDwnldWorksheet == 1) {

							downloadStrs = "Worksheet downloaded successfully! ";

							if (e.NoOfDaysRemainingForSubscription > 7) {
								popMessage = downloadStrs;
							}
							else if (e.NoOfDaysRemainingForSubscription != "" || e.NoOfDaysRemainingForSubscription <= "7") {
								viewpricingactive = 1;
								popMessage = downloadStrs + " <br/><br/>" + DaysLeft;
							}
							else {
								popMessage = downloadStrs;
							}


							$(".cntOfDownload").html(popMessage);

							location.href = downloadUrl;

						}
						else {
							$(".cntOfDownload").html("You’ve reached a download/print limit!");
							$(".viewpricing").show();
							
						}

						if (evnt == "justlogin") {
							$("#paidDwnldJustLgn").show();
							if (viewpricingactive == "1") {
								$(".viewpricing").show();
							}
						}
						else {
							$("#paidDwnld").show();
							if (viewpricingactive == "1") {
								$(".viewpricing").show();
							}
						}
					}

					//if (e.IsEligibleForDwnldWorksheet <= 0) {//plan expired
					//	var ValidityExp = "You've exhausted your download/print limit under the current plan. Renew today to enjoy uninterrupted access.";

					//	$('.cntOfDownload').html(ValidityExp);
					//	$("#PayRequirement").show();
					//	$(".closebtn").hide();
					//}
					//else {
					//	var popMessage = "";
					//	var SevenDaysLeft = "";
					//	var TwentyDownloadsLeft = "";
					//	var viewpricingactive = 0;

					//	if (e.RemainingValidityInDays > 0 && e.RemainingValidityInDays <= 7) {
					//		viewpricingactive = "1";
					//		SevenDaysLeft = "Your plan is about to expire! Renew today to enjoy uninterrupted access.";
					//	}
					//	if (e.RemainingWorksheetForDwnld > 0 && e.RemainingWorksheetForDwnld <= 20) {
					//		viewpricingactive = "1";
					//		TwentyDownloadsLeft = "You’re about to exhaust the number of downloads in your plan! Renew today to enjoy uninterrupted access.";
					//	}

					//	if (e.CurrentWorksheetDownloaded > 0) {

					//		if (mode == "print") {
					//			if (evnt == "justlogin") {
					//				location.href = window.location.href;
					//			}
					//			else {
					//				PrintWorkSheet(downloadUrl);
					//			}
					//		}
					//		else {

					//			downloadStrs = "Worksheet downloaded successfully! ";

					//			if (SevenDaysLeft == "" && TwentyDownloadsLeft == "") {
					//				popMessage = downloadStrs;
					//			}
					//			else if (SevenDaysLeft != "" && TwentyDownloadsLeft != "") {
					//				popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
					//			}
					//			else if (SevenDaysLeft == "") {
					//				popMessage = downloadStrs + " <br/><br/>" + TwentyDownloadsLeft;
					//			}
					//			else if (TwentyDownloadsLeft == "") {
					//				popMessage = downloadStrs + " <br/><br/>" + SevenDaysLeft + " <br/><br/>" + TwentyDownloadsLeft;
					//			}
					//			else {
					//				popMessage = downloadStrs;
					//			}

					//			$(".cntOfDownload").html(popMessage);


					//			if (evnt == "justlogin") {
					//				$("#paidDwnldJustLgn").show();
					//				if (viewpricingactive == "1") {
					//					$(".pricing").show();
					//				}
					//			}
					//			else {
					//				$("#paidDwnld").show();
					//				if (viewpricingactive == "1") {
					//					$(".pricing").show();
					//				}
					//			}

					//			location.href = downloadUrl;
					//		}
					//	}
						
					//}
				},
				error: function (error) {

				}
			});
		
	}
}
