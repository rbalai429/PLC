using HP_PLC_Doc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class SubscriptionDetails
	{
		public string targetUrl { get; set; }
		public string subscriptionId { get; set; }
		public string ageGroup { get; set; }
		public string WorksheetId { get; set; }
		public string DiscountMode { get; set; } = "";
	}

	public class BotPaySubscriptionDetails
	{
		public string Ranking { get; set; }
		public string ageGroup { get; set; }
	}

	public class GetUserCurrentSubscription
	{
		public string Ranking { get; set; }
		public string AgeGroup { get; set; }
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string SubscriptionDuration { get; set; }
		public string SubscriptionStartDate { get; set; }
		public string SubscriptionEndDate { get; set; }
		public int RewardReferralMonth { get; set; }
		public int DaysRemaining { get; set; }
		public string NextSubscriptionTAT { get; set; }
		public int ReferralRewardMonth { get; set; }
		public int ReferralRewardDays { get; set; }
		public int IsActive { get; set; }
	}


	public class TempSubscriptionDiscount
	{
		public string UniqueCode { get; set; }
		public string DiscountMode { get; set; }
		public decimal DiscountAmt { get; set; }
	}
	public class TempSubscriptionBind
	{
		public int Ranking { get; set; }
	}
	public class TempSubscriptionCreatedByUser
	{
		public int SrNo { get; set; }
		public int SubscriptionId { get; set; }
		public string Ranking { get; set; } = "";
		public string AgeGroup { get; set; }
		public string SubscriptionName { get; set; } = "";
		public string SubscriptionPrice { get; set; } = "";
		public int ValidMonths { get; set; }
		public string ValidMonthsText { get; set; }
		public string PartCode { get; set; }
		public int DiscountAmount { get; set; } = 0;
	}

	public class RecommendedVideos
	{
		public int NodeId { get; set; }
		public int VideoCount { get; set; }
	}

	public class SelectedAgeGroup
	{
		public string AgeGroup { get; set; }
	}
	public class WatchedVideos
	{
		public int UniqueId { get; set; }
		public int UserId { get; set; }
		public string VideoId { get; set; }
		public string VideoExecutionTimeInMin { get; set; }
		public string VideosTotalTimeInMin { get; set; }
		public string VideosFinished { get; set; }
	}

	public class SubscriptionModel
	{
		public List<GetYourSubscriptionDetails> getYourSubscriptionDetails { get; set; }
		public SubscriptionDisplayContent subscriptionDisplayContent { get; set; }
	}
	public class GetYourSubscriptionDetails
	{
		public int UserId { get; set; }
		public string Ranking { get; set; }
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string SubscriptionDuration { get; set; }
		public string SubscriptionStartDate { get; set; }
		public string SubscriptionEndDate { get; set; }
		public string AgeGroup { get; set; }
		public int DaysRemaining { get; set; }
		public int SubscriptionCMSId { get; set; }
		public string NextSubscriptionTAT { get; set; }
		public int WorksheetDwnPrnCount { get; set; }
		public int VideoWatchedCount { get; set; }
		public string ListOfWorksheetDownloaded { get; set; }
		public string ListOfVideoWatched { get; set; }
		public SubscriptionDisplayContent subscriptionDisplayContent { get; set; }
	}

	public class GetYourReferralDetails
	{
		public int TotalReferralSent { get; set; }
		public int TotalReferralConverted { get; set; }
		public int ReferralReward { get; set; }
		public string ReferralCode { get; set; }
	}

	public class SubscriptionDisplayContent
	{
		public string subscriptionPlanName { get; set; }
		public string planTitle { get; set; }
		public string planPunchLine { get; set; }
		public string durationTitle { get; set; }
		public string daysRemainingTitle { get; set; }
		public string activatedOnTitle { get; set; }
		public string endsOnTitle { get; set; }
		public string upgradeNowButtonTitle { get; set; }
		public string renewNowButtonTitle { get; set; }
		public string subscriptionPlanNameIcon { get; set; }
		public string YearsTitle { get; set; }
		public string DaysTitle { get; set; }
		public string NoLimitTitle { get; set; }
		public string Culture { get; set; }
	}
	public class MyProfile
	{
		public string UserUniqueId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string WhatsAppNoPrefix { get; set; }
		public string WhatsAppNo { get; set; }

		public string Mobileno { get; set; }
		public string MobileAlternateNo { get; set; }
		public string EncPassword { get; set; }
		public string SelectedAgeGroup { get; set; }
		//public string LastPaymentDate { get; set; }
		//public string NextPaymentDate { get; set; }
		//public string SubscriptionStatus { get; set; }
		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string ComWithPhone { get; set; }
		//public string SubscriptionName { get; set; }
		public string ReferralCode { get; set; }
		public string IsAutheticated { get; set; }
		public string TempPassword { get; set; }
		public string AreYouStudentOrParent { get; set; }
		public string Mode
		{
			get; set;
		}
	}

	public class MyProfileWithSubscription
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string WhatsAppNoPrefix { get; set; }
		public string WhatsAppNo { get; set; }

		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string ComWithPhone { get; set; }

		public string Ranking { get; set; }
		public string Amount { get; set; }
		public string Duration { get; set; }
	}

	public class SubscriptionSuccessParam
	{
		public string Amount { get; set; }
		public string Duration { get; set; }
		public InvoiceData InvoiceData { get; set; }
		public List<Product> ProductList { get; set; }
		public decimal? TotalAmount { get; set; }
	}
	public class Product
	{
		public string name
		{
			get; set;
		}
		public string id
		{
			get; set;
		}
		public string price
		{
			get; set;
		}
		public string MaxPrice
		{
			get; set;
		} = "0";
		public string DiscountPrice
		{
			get; set;
		} = "0";
		public string brand
		{
			get; set;
		}
		public string category
		{
			get; set;
		}
		public string variant
		{
			get; set;
		}
		public string quantity
		{
			get; set;
		}
		public string coupon
		{
			get; set;
		}
		public string discountAmount
		{
			get; set;
		}

	}
	public class RegisterTemp
	{
		public int Id
		{
			get; set;
		}
		public int? UserId
		{
			get; set;
		}
		public string Name
		{
			get; set;
		}
		public string Email
		{
			get; set;
		}
		public string WhatsAppNoPrefix
		{
			get; set;
		}
		public string WhatsAppNo
		{
			get; set;
		}
		public string SelectedAgeGroup
		{
			get; set;
		}
		public string LastPaymentDate
		{
			get; set;
		}
		public string NextPaymentDate
		{
			get; set;
		}
		public string SubscriptionStatus
		{
			get; set;
		}
		public string ComWithEmail
		{
			get; set;
		}
		public string ComWithWhatsApp
		{
			get; set;
		}
		public string ComWithPhone
		{
			get; set;
		}
		public string SubscriptionName
		{
			get; set;
		}
		public string ReferralCode
		{
			get; set;
		}
		public string termsChecked
		{
			get; set;
		}
		public int IsActive
		{
			get; set;
		} = 1;
		public DateTime? DOC
		{
			get; set;
		}
		public DateTime? DOM
		{
			get; set;
		}
		public string EncPassword
		{
			get; set;
		}
		public string Reason
		{
			get; set;
		} = "";
		public string UploadStatus
		{
			get; set;
		} = "";

		public string Mode
		{
			get; set;
		} = "";
		public string DataSource
		{
			get; set;
		} = "";
	}
	public class RegisterBackup
	{
		public int Id
		{
			get; set;
		}
		public int? UserId
		{
			get; set;
		}
		public string Name
		{
			get; set;
		}
		public string Email
		{
			get; set;
		}
		public string WhatsAppNoPrefix
		{
			get; set;
		}
		public string WhatsAppNo
		{
			get; set;
		}
		public string SelectedAgeGroup
		{
			get; set;
		}
		public string LastPaymentDate
		{
			get; set;
		}
		public string NextPaymentDate
		{
			get; set;
		}
		public string SubscriptionStatus
		{
			get; set;
		}
		public string ComWithEmail
		{
			get; set;
		}
		public string ComWithWhatsApp
		{
			get; set;
		}
		public string ComWithPhone
		{
			get; set;
		}
		public string SubscriptionName
		{
			get; set;
		}
		public string ReferralCode
		{
			get; set;
		}
		public string termsChecked
		{
			get; set;
		}
		public DateTime? DOC
		{
			get; set;
		}
		public DateTime? DOM
		{
			get; set;
		}
		public string EncPassword
		{
			get; set;
		}
		public string Reason
		{
			get; set;
		}
		public int FromTemp
		{
			get; set;
		} = 0;
	}
	public class EncryptionDecryption
	{
		public int UserId { get; set; }
		public string EncEmail { get; set; }
		public string EncName { get; set; }
		public string EncMobile { get; set; }
	}

	public class UnSubscribe
	{
		public string UnsubscribeOpt { get; set; }
		public string OtherContent { get; set; }

		public string CurrentUrl { get; set; }
	}
	public class ApplicationError
	{
		public string PageName { get; set; }
		public string MethodName { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class MyProfile_Temp
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Mobileno { get; set; }

		public string IsPasswordSetPostHPIdRemoval { get; set; }
		public int StepsCompletted { get; set; }
	}
	public class CouponCodeofferWindow
	{
		public int IsvalidForWindowAppering { get; set; }
	}

	public class SpecialRedirection
	{
		public string TargetUrl { get; set; }
		public string Target { get; set; }
	}

	public class SpecialRedirectionCheck
	{
		public int UserId { get; set; }
	}

	public class MyDownloadsData
	{
		public string UserUniqueId { get; set; }
		public int WorkSheetId { get; set; }
	}

	public class GetSpecialPlanData
	{
		public int UserId { get; set; }
		public int RowNoInDays { get; set; }
		public string UserUniqueCode { get; set; }
		public int NoOfDay { get; set; }
		public string SentDate { get; set; }
		public string SentStatus { get; set; }
	}

	public class SpecialProgramGetDataForWhatsApp
	{
		public int UserId { get; set; }
		public string UserUniqueCode { get; set; }
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string u_mobileno { get; set; }
		public string ComwithwhatsApp { get; set; }
	}
	public class GetSpecialPlanSubscriptionRportData
	{
		public string SubscriptionName { get; set; }
		public string LearningSetDownloads { get; set; }
		public string SubscriptionStartDate { get; set; }
		public string SubscriptionEndDate { get; set; }
	}

	public class GetBonusSubscriptionDetails
	{
		
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string SubscriptionDuration { get; set; }
		public string SubscriptionStartDate { get; set; }
		public string SubscriptionEndDate { get; set; }
		
	}

	public class GetBonusSubscriptionDownloadsDetails
	{
		public int TotalEligibleForDownloads { get; set; }
		public int RemainingEligibleForDownloads { get; set; }
		public int TotalWorksheetDownloaded { get; set; }
		public int TotalVideoWkstEligibleForDownloads { get; set; }
		public int TotalVideoWkstRemainingtoDownloads { get; set; }
		public int TotalVideoWorksheetDownloaded { get; set; }
	}


	public class GetTeachersSubscriptionDetails
	{
		public string ClassName { get; set; }
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		public string SubscriptionDuration { get; set; }
		public string SubscriptionStartDate { get; set; }
		public string SubscriptionEndDate { get; set; }
		public int DaysRemaining { get; set; }
		public int TotalWorksheetDownloaded { get; set; }
	}

	public class pdfdownloaddata
	{
		public string pdffilename { get; set; }
	}
	public class Users
	{
		public int UserId { get; set; }
	}
}