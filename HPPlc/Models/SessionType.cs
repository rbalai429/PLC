﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	#region Enum Constant
	public enum SessionType
	{
		UserId,
		UserUniqueId,
		UserType,
		IsLoggedIn,
		SelectedLanguage,
		SelectedLanguageCulture,
		OtpValue,
		whatsAppNo,
		emailid,
		SubscriptionDtls,
		SubscriptionBotDtls,
		IsBotRequest,
		LoggedInDtls,
		SubscriptionInDtls,
		SubscriptionInDtlsTeachers,
		SubscriptionTempDtls,
		SubscriptionTempDtlsBonus,
		CouponTempDtls,
		subscriptionPopup,
		SubscriptionDiscount,
		SubscriptionDiscountAmt,
		//NoOfSubscribed,
		SubscriptionId,
		PaymentId,
		ExpertTalkUrl,
		JoinNowUrl,
		//SubscriptionValidationText,
		//UserAgeGroup,
		UserAgeGroupSelected,
		MettingName,
		MeetingDate,
		ExpertTalkId,
		UserReferralCode,
		DataSource,
		UserClickedOnWorksheet,
		RegistrationMode,
		SubscribedOrNot,
		SubscribedOrNotBonus,
		SubscribedOrNotTeachers,
		AgeGroupExistsOrNot,
		UserSelectVolumeOnWorksSheet,
		UserSelectCategoryOnWorksSheet,
		UserSelectAgeOnWorksSheet,
		CbseContentChecked,
		UserSelectVolumeOnVidoe,
		UserSelectCategoryOnWorksVidoe,
		PayResponseTracker,
		resendCnt,
		HpIdSession,
		HpOfferRedirection,
		HpBundlePopupOpenCount,
		quizWorksheetId,
		quizUniqueCode,
		TempUserDetails,
		InvalidOtpFailedTime,
		OtpId,
		WorksheetDownloadUrl,
		IsCouponCodeOffer,
		CouponCode,
		SpecialRedirection,
		JrnyUserDetails,
		UserRegistrationMode,
		BannerSpecificTag,
		BonusWorkSheetCurrentPage,
		SplRedirection,
		InviteUrlCode,
		OfferCodeCouponValidate,
		LoginMessageDynamic,
		EnsureRedirection,
		AllowuserGroup
	}
	#endregion
}