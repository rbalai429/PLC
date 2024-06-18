using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class Registration
	{
		public string name { get; set; } = "";
		public string mobileno { get; set; }
		public string alternatemobileno { get; set; }	
		public string email { get; set; }
		public string HPiD { get; set; } = "";
		public string whatsupprefix { get; set; } = "";
		public string whatsupnumber { get; set; } = "";
		public string[] ageGroup { get; set; } = null;
		public string supportOnEmailFromHP { get; set; }
		public string supportOnWhatsupFromHP { get; set; }
		public string supportOnPhoneFromHP { get; set; }
		public string termsChecked { get; set; }
		public string referralCode { get; set; } = "";
		public string regpassword { get; set; } = "";
		public string subject { get; set; } = "";
		public string otptype { get; set; } = "";
		public string Otp { get; set; } = "";
		public string page { get; set; } = "";
		public int BotPlanSelection { get; set; } = 0;
		public int PlanMode { get; set; } = 0;
		public string RuParentOrStudent { get; set; } = "";
		public string ReferedBy { get; set; } = "";
	}
	public class BotUserRegistration
	{
		public string name { get; set; }
		public string email { get; set; }
		public string mobile { get; set; }
		public string age { get; set; }
		public int PlanSelection { get; set; }
	}

	public class BotSubscriptions
	{
		public int Status { get; set; }
		public string Message { get; set; }

		public List<BotSubscriptionDetails> Subscriptions { get; set; }
	}

	public class BotSubscriptionDetails
	{
		public string AgeGroup { get; set; }
		//public string Ranking { get; set; }
		public string SubscriptionName { get; set; }
		public string SubscriptionPrice { get; set; }
		//public string SubscriptionStartDate { get; set; }
		//public string SubscriptionEndDate { get; set; }
		public string Type { get; set; }
		public string Is_PLC1_Customer { get; set; }
		public string PlanValidity { get; set; }
		//public int DaysRemaining { get; set; }
	}
	public class BotSubscriptionPaymentUrl
	{
		public string Email { get; set; }
		public List<BotSubscriptionSetPaymentUrl> botSubscriptionSetPaymentUrl { get; set; }
	}

	public class BotSubscriptionSetPaymentUrl
	{
		public string Agegroup { get; set; }
		public string Type { get; set; }
		public int Rank { get; set; }
	}

	public class PaymentLinkResponse
	{
		public int Status { get; set; }
		public string Message { get; set; }

		public string PaymentLink { get; set; }
	}


	public class GetStatus
	{
		public int returnValue { get; set; }
		public string returnStatus { get; set; }
		public string returnMessage { get; set; }
	}

	public class ResponseUserVerification
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }

		public string ResponseText { get; set; }
		public string ResponseMessage { get; set; }
	}
	public class ReturnMessage
	{
		public string status { get; set; }
		public string navigation { get; set; }
		public string message { get; set; }
	}

	public class PostPayUser
	{
		public int UserId { get; set; }
		public string Email { get; set; }
		public string Status { get; set; }
	}
	public class SetParameters
	{
		public string ParameterName { get; set; }
		public string Value { get; set; }
	}

	public class LoggedIn
	{
		public int UserId { get; set; }
		public string UserUniqueId { get; set; }
		public string IsPasswordSetPostHPIdRemoval { get; set; }
		public string IsUserRegistered { get; set; }
		public int StepsCompletted { get; set; }
		public int ProfileStatus { get; set; }
		public int UserType { get; set; }
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string u_whatsappno { get; set; }
		public string UserTransactionType { get; set; }
		public string ReferralCode { get; set; }
		public string ResponseText { get; set; }
		public string RegistrationMode { get; set; }
		public string SubscribedOrNot { get; set; } // Yes/No
		public string AgeGroupExistsOrNot { get; set; }

		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string ComWithPhone { get; set; }

		public string RegSource { get; set; }
		public string LoginType { get; set; }

		public string UserRegistrationMode { get; set; }
		public string SubscribedOrNotBonus { get; set; }

		public string ReferralBenefitPlan { get; set; }
		// Yes/No
		//public int NoOfSubscribed { get; set; }
		//public int RegisteredSubscriptionId { get; set; }
		//public string RegisteredSubscriptionName { get; set; }
		//public string SubscriptionValidationText { get; set; }
		//public int Ranking { get; set; }
		//public string AgeGroup { get; set; }
	}


	public class LoggedIn_SpecialPlan
	{
		public int UserId { get; set; }
		public string UserUniqueId { get; set; }
		public string IsPasswordSetPostHPIdRemoval { get; set; }
		public string IsUserRegistered { get; set; }
		public int StepsCompletted { get; set; }
		public int ProfileStatus { get; set; }
		public int UserType { get; set; }
		public string u_name { get; set; }
		public string u_email { get; set; }
		public string u_whatsappno { get; set; }
		public string UserTransactionType { get; set; }
		public string ReferralCode { get; set; }
		public string ResponseText { get; set; }
		public string RegistrationMode { get; set; }
		public string SubscribedOrNot { get; set; } // Yes/No
		public string AgeGroupExistsOrNot { get; set; }

		public string ComWithEmail { get; set; }
		public string ComWithWhatsApp { get; set; }
		public string ComWithPhone { get; set; }

		public string RegSource { get; set; }
		public string LoginType { get; set; }

		public string UserRegistrationMode { get; set; }

	}
	public class PostPaymentLogin
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class TempUserJourneyDtls
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserMobile { get; set; }
		public string UserEmail { get; set; }
		public string UserPassword { get; set; }
		public string Source { get; set; }
		public int StepsCompletted { get; set; }
		public int AlreadyRegCommMode { get; set; }
		public string Rememberme { get; set; }
	}
	public class RegistrationOtp
	{
		public string name { get; set; }
		public string UserName { get; set; }
		public string email { get; set; }
		public string mobileno { get; set; }
		public string type { get; set; }

		public string referralCode { get; set; } = "";
		public string supportOnEmailFromHP { get; set; } = "";
		public string supportOnWhatsupFromHP { get; set; } = "";
		public string[] ageGroup { get; set; }
	}
	public class LoginParam
	{
		public string UserName { get; set; }
		public string PwdText { get; set; }
		public string PageId { get; set; }
	}
	public class ForgotPasswordParam
	{
		public string UserName { get; set; }
	}
	public class OtpResendParam
	{
		public string UserName { get; set; }
	}
	public class OtpVerification
	{
		public string OneTimePwd { get; set; }
		public string RememberMe { get; set; }
	}

	public class UserSetPassword
	{
		public string regpassword { get; set; }
		public string regpasswordconfirm { get; set; }
	}

	public class ChangePasswordParam
	{
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}
	public class JourneyOfAccount
	{
		public string Status { get; set; }
		public string UserRegistered { get; set; }
		public string StatusOfJrny { get; set; }
		public string Navigation { get; set; }
		public int StepsCompletted { get; set; }
		public int ProfileStatus { get; set; }

		public string RegSource { get; set; }
		public string mobmasking { get; set; }
		public string mailmasking { get; set; }
		public string LoginType { get; set; }
		public int IsotpVerified { get; set; }
		public string UserRegisteredMode { get; set; }
		public string UserEmail { get; set; }
		public string UserMobile { get; set; }	
		public string AlternateUserMobile { get; set; }
		public string UserName { get; set; }
		public int UserId { get; set; }
		public string ValidateMessage { get; set; }
		public string Page { get; set; }
		public int AlreadyRegCommMode { get; set; }
		public string UserUniqueCode { get; set; } = "";
	}
	public class ValidateUser
	{
		public int emailExists { get; set; }
		public int mobilenoExists { get; set; }
	}

	public class SendOTPMessage
	{
		public string ValidateCode { get; set; }
		public string ValidateMessage { get; set; }
	}
}