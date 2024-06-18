//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v8.12.2
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder.Embedded;

namespace Umbraco.Web.PublishedModels
{
	/// <summary>Registration</summary>
	[PublishedModel("registration")]
	public partial class Registration : PublishedContentModel, ISEO
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "registration";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Registration, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public Registration(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Age Groupe Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("ageGroupeTitle")]
		public string AgeGroupeTitle => this.Value<string>("ageGroupeTitle");

		///<summary>
		/// Age Group Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("ageGroupRequired")]
		public string AgeGroupRequired => this.Value<string>("ageGroupRequired");

		///<summary>
		/// Are you a parent or student
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("areYouAParentOrStudent")]
		public string AreYouAparentOrStudent => this.Value<string>("areYouAParentOrStudent");

		///<summary>
		/// Are You Aparent Or Student Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("areYouAparentOrStudentMessage")]
		public string AreYouAparentOrStudentMessage => this.Value<string>("areYouAparentOrStudentMessage");

		///<summary>
		/// Cancel Button Text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("cancelButtonText")]
		public string CancelButtonText => this.Value<string>("cancelButtonText");

		///<summary>
		/// Communication Email Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationEmailTitle")]
		public string CommunicationEmailTitle => this.Value<string>("communicationEmailTitle");

		///<summary>
		/// Communication on email required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationOnEmailRequired")]
		public string CommunicationOnEmailRequired => this.Value<string>("communicationOnEmailRequired");

		///<summary>
		/// Communication on phone required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationOnPhoneRequired")]
		public string CommunicationOnPhoneRequired => this.Value<string>("communicationOnPhoneRequired");

		///<summary>
		/// Communication on WhatsApp required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationOnWhatsAppRequired")]
		public string CommunicationOnWhatsAppRequired => this.Value<string>("communicationOnWhatsAppRequired");

		///<summary>
		/// Communication Phone Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationPhoneTitle")]
		public string CommunicationPhoneTitle => this.Value<string>("communicationPhoneTitle");

		///<summary>
		/// Communication Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationTitle")]
		public global::System.Web.IHtmlString CommunicationTitle => this.Value<global::System.Web.IHtmlString>("communicationTitle");

		///<summary>
		/// Communication WhatsApp Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("communicationWhatsAppTitle")]
		public string CommunicationWhatsAppTitle => this.Value<string>("communicationWhatsAppTitle");

		///<summary>
		/// Confirm Password
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("confirmPassword")]
		public string ConfirmPassword => this.Value<string>("confirmPassword");

		///<summary>
		/// Email Already Registered
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailAlreadyRegistered")]
		public string EmailAlreadyRegistered => this.Value<string>("emailAlreadyRegistered");

		///<summary>
		/// Email and Mobile Masking Title: Please do not forgot to enter {email} and {mobile} for all language.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailAndMobileMaskingTitle")]
		public string EmailAndMobileMaskingTitle => this.Value<string>("emailAndMobileMaskingTitle");

		///<summary>
		/// Email Format
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailFormat")]
		public string EmailFormat => this.Value<string>("emailFormat");

		///<summary>
		/// Email Masking Title: Please do not forgot to enter {email} for all language.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailMaskingTitle")]
		public string EmailMaskingTitle => this.Value<string>("emailMaskingTitle");

		///<summary>
		/// Email Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailRequired")]
		public string EmailRequired => this.Value<string>("emailRequired");

		///<summary>
		/// Email Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("emailTitle")]
		public string EmailTitle => this.Value<string>("emailTitle");

		///<summary>
		/// Enter OTP
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("enterOTP")]
		public string EnterOtp => this.Value<string>("enterOTP");

		///<summary>
		/// Enter OTP Validation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("enterOTPValidation")]
		public string EnterOtpvalidation => this.Value<string>("enterOTPValidation");

		///<summary>
		/// Invalid Referral Code
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("invalidReferralCode")]
		public string InvalidReferralCode => this.Value<string>("invalidReferralCode");

		///<summary>
		/// Mobile Already Registered
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mobileAlreadyRegistered")]
		public string MobileAlreadyRegistered => this.Value<string>("mobileAlreadyRegistered");

		///<summary>
		/// Mobile no required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mobileNoRequired")]
		public string MobileNoRequired => this.Value<string>("mobileNoRequired");

		///<summary>
		/// Mobile no title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mobileNoTitle")]
		public string MobileNoTitle => this.Value<string>("mobileNoTitle");

		///<summary>
		/// Name Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("nameRequired")]
		public string NameRequired => this.Value<string>("nameRequired");

		///<summary>
		/// Name Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("nameTitle")]
		public string NameTitle => this.Value<string>("nameTitle");

		///<summary>
		/// No Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("noTitle")]
		public string NoTitle => this.Value<string>("noTitle");

		///<summary>
		/// OTP Attempted Maximum Times
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("oTPAttemptedMaximumTimes")]
		public string OTpattemptedMaximumTimes => this.Value<string>("oTPAttemptedMaximumTimes");

		///<summary>
		/// OTP Attempt Maximum Limit
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("oTPAttemptMaximumLimit")]
		public string OTpattemptMaximumLimit => this.Value<string>("oTPAttemptMaximumLimit");

		///<summary>
		/// Otp Submit Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("otpSubmitTitle")]
		public string OtpSubmitTitle => this.Value<string>("otpSubmitTitle");

		///<summary>
		/// Otp Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("otpTitle")]
		public string OtpTitle => this.Value<string>("otpTitle");

		///<summary>
		/// Otp Verification Msg
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("otpVerificationMsg")]
		public string OtpVerificationMsg => this.Value<string>("otpVerificationMsg");

		///<summary>
		/// Password
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("password")]
		public string Password => this.Value<string>("password");

		///<summary>
		/// Password Format
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("passwordFormat")]
		public string PasswordFormat => this.Value<string>("passwordFormat");

		///<summary>
		/// Password Length
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("passwordLength")]
		public string PasswordLength => this.Value<string>("passwordLength");

		///<summary>
		/// Password Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("passwordRequired")]
		public string PasswordRequired => this.Value<string>("passwordRequired");

		///<summary>
		/// Password Same Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("passwordSameRequired")]
		public string PasswordSameRequired => this.Value<string>("passwordSameRequired");

		///<summary>
		/// Referral Code
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("referralCode")]
		public string ReferralCode => this.Value<string>("referralCode");

		///<summary>
		/// Registration Success Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("registrationSuccessMessage")]
		public string RegistrationSuccessMessage => this.Value<string>("registrationSuccessMessage");

		///<summary>
		/// Registration Success Message Button
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("registrationSuccessMessageButton")]
		public string RegistrationSuccessMessageButton => this.Value<string>("registrationSuccessMessageButton");

		///<summary>
		/// Resend Blocking Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("resendBlockingMessage")]
		public string ResendBlockingMessage => this.Value<string>("resendBlockingMessage");

		///<summary>
		/// Resend OTP
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("resendOTP")]
		public string ResendOtp => this.Value<string>("resendOTP");

		///<summary>
		/// Resend OTP Sent
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("resendOTPSent")]
		public string ResendOtpsent => this.Value<string>("resendOTPSent");

		///<summary>
		/// Resend OTP Timer Title: Resend OTP Timer left message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("resendOTPTimerTitle")]
		public string ResendOtptimerTitle => this.Value<string>("resendOTPTimerTitle");

		///<summary>
		/// Submit Button Text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("submitButtonText")]
		public string SubmitButtonText => this.Value<string>("submitButtonText");

		///<summary>
		/// Terms & Condition Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("termsConditionContent")]
		public global::System.Web.IHtmlString TermsConditionContent => this.Value<global::System.Web.IHtmlString>("termsConditionContent");

		///<summary>
		/// Terms Condition Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("termsConditionRequired")]
		public string TermsConditionRequired => this.Value<string>("termsConditionRequired");

		///<summary>
		/// Timer Left
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("timerLeft")]
		public string TimerLeft => this.Value<string>("timerLeft");

		///<summary>
		/// Title of page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("titleOfPage")]
		public string TitleOfPage => this.Value<string>("titleOfPage");

		///<summary>
		/// Validate Attempt Blocking Message: Multi times Validate click Blocking Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("validateAttemptTitle")]
		public string ValidateAttemptTitle => this.Value<string>("validateAttemptTitle");

		///<summary>
		/// WhatsApp no length
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppNoLength")]
		public string WhatsAppNoLength => this.Value<string>("whatsAppNoLength");

		///<summary>
		/// WhatsApp No Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppNoTitle")]
		public string WhatsAppNoTitle => this.Value<string>("whatsAppNoTitle");

		///<summary>
		/// WhatsApp Prefix Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppPrefixTitle")]
		public string WhatsAppPrefixTitle => this.Value<string>("whatsAppPrefixTitle");

		///<summary>
		/// WhatsApp Required
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppRequired")]
		public string WhatsAppRequired => this.Value<string>("whatsAppRequired");

		///<summary>
		/// Wrong OTP Validation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("wrongOTPValidation")]
		public string WrongOtpvalidation => this.Value<string>("wrongOTPValidation");

		///<summary>
		/// Yes Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("yesTitle")]
		public string YesTitle => this.Value<string>("yesTitle");

		///<summary>
		/// HeadSectionScripts: HeadSectionScripts - *Not Mandatory. The Page Specific code inside the head section goes here:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("headSectionScripts")]
		public string HeadSectionScripts => global::Umbraco.Web.PublishedModels.SEO.GetHeadSectionScripts(this);

		///<summary>
		/// Is Enable for Breadcrumb
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isEnableForBreadcrumb")]
		public bool IsEnableForBreadcrumb => global::Umbraco.Web.PublishedModels.SEO.GetIsEnableForBreadcrumb(this);

		///<summary>
		/// Meta Description: SEO Meta Description :
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("metaDescription")]
		public string MetaDescription => global::Umbraco.Web.PublishedModels.SEO.GetMetaDescription(this);

		///<summary>
		/// Meta keywords: Meta Keywords:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("metaKeywords")]
		public global::System.Collections.Generic.IEnumerable<string> MetaKeywords => global::Umbraco.Web.PublishedModels.SEO.GetMetaKeywords(this);

		///<summary>
		/// Meta Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("metaName")]
		public string MetaName => global::Umbraco.Web.PublishedModels.SEO.GetMetaName(this);

		///<summary>
		/// Page Title: Browser Title - title shown in the Browser window / tab, and most important on search-engine listings.:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("pageName")]
		public string PageName => global::Umbraco.Web.PublishedModels.SEO.GetPageName(this);

		///<summary>
		/// SEO Can Index: If checked means SEO can track this page, In case of unchecked SEO can not track.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("sEOCanIndex")]
		public bool SEocanIndex => global::Umbraco.Web.PublishedModels.SEO.GetSEocanIndex(this);

		///<summary>
		/// SEO Follow links: SEO Follow links
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("sEOFollowLinks")]
		public bool SEofollowLinks => global::Umbraco.Web.PublishedModels.SEO.GetSEofollowLinks(this);
	}
}
