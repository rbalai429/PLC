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
	/// <summary>Bonus Subscription List</summary>
	[PublishedModel("bonusSubscriptionList")]
	public partial class BonusSubscriptionList : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "bonusSubscriptionList";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<BonusSubscriptionList, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public BonusSubscriptionList(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Activated On Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("activatedOnTitle")]
		public string ActivatedOnTitle => this.Value<string>("activatedOnTitle");

		///<summary>
		/// Age Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("ageTitle")]
		public string AgeTitle => this.Value<string>("ageTitle");

		///<summary>
		/// Days Remaining Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("daysRemainingTitle")]
		public string DaysRemainingTitle => this.Value<string>("daysRemainingTitle");

		///<summary>
		/// Days Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("daysTitle")]
		public string DaysTitle => this.Value<string>("daysTitle");

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("description")]
		public global::System.Web.IHtmlString Description => this.Value<global::System.Web.IHtmlString>("description");

		///<summary>
		/// Duration Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("durationTitle")]
		public string DurationTitle => this.Value<string>("durationTitle");

		///<summary>
		/// Ends On Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("endsOnTitle")]
		public string EndsOnTitle => this.Value<string>("endsOnTitle");

		///<summary>
		/// Existing User Banner Desktop
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("existingUserBannerDesktop")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent ExistingUserBannerDesktop => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("existingUserBannerDesktop");

		///<summary>
		/// Existing User Banner Desktop Webp
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("existingUserBannerDesktopWebp")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent ExistingUserBannerDesktopWebp => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("existingUserBannerDesktopWebp");

		///<summary>
		/// Existing User Banner Mobile
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("existingUserBannerMobile")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent ExistingUserBannerMobile => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("existingUserBannerMobile");

		///<summary>
		/// Existing User Banner WebP
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("existingUserBannerWebP")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent ExistingUserBannerWebP => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("existingUserBannerWebP");

		///<summary>
		/// Is Offer Enable
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isOfferEnable")]
		public bool IsOfferEnable => this.Value<bool>("isOfferEnable");

		///<summary>
		/// Max Age Group
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("maxAgeGroup")]
		public int MaxAgeGroup => this.Value<int>("maxAgeGroup");

		///<summary>
		/// No Limit Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("noLimitTitle")]
		public string NoLimitTitle => this.Value<string>("noLimitTitle");

		///<summary>
		/// Offer Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("offerContent")]
		public global::System.Web.IHtmlString OfferContent => this.Value<global::System.Web.IHtmlString>("offerContent");

		///<summary>
		/// Plan Punch Line
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("planPunchLine")]
		public string PlanPunchLine => this.Value<string>("planPunchLine");

		///<summary>
		/// Plan Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("planTitle")]
		public string PlanTitle => this.Value<string>("planTitle");

		///<summary>
		/// Renew Now Button Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("renewNowButtonTitle")]
		public string RenewNowButtonTitle => this.Value<string>("renewNowButtonTitle");

		///<summary>
		/// Select One Plan
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("selectOnePlan")]
		public string SelectOnePlan => this.Value<string>("selectOnePlan");

		///<summary>
		/// Subcription Plan SubTitle
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subcriptionPlanSubTitle")]
		public string SubcriptionPlanSubTitle => this.Value<string>("subcriptionPlanSubTitle");

		///<summary>
		/// Subscription Plan Details
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subscriptionPlanDetails")]
		public global::System.Web.IHtmlString SubscriptionPlanDetails => this.Value<global::System.Web.IHtmlString>("subscriptionPlanDetails");

		///<summary>
		/// Subscription Plan Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subscriptionPlanName")]
		public string SubscriptionPlanName => this.Value<string>("subscriptionPlanName");

		///<summary>
		/// Subscription Plan Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subscriptionPlanTitle")]
		public string SubscriptionPlanTitle => this.Value<string>("subscriptionPlanTitle");

		///<summary>
		/// Subscription Pointers
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subscriptionPointers")]
		public global::System.Collections.Generic.IEnumerable<string> SubscriptionPointers => this.Value<global::System.Collections.Generic.IEnumerable<string>>("subscriptionPointers");

		///<summary>
		/// Subscription Popup Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subscriptionPopupTitle")]
		public string SubscriptionPopupTitle => this.Value<string>("subscriptionPopupTitle");

		///<summary>
		/// Sub Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("subTitle")]
		public string SubTitle => this.Value<string>("subTitle");

		///<summary>
		/// Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("title")]
		public string Title => this.Value<string>("title");

		///<summary>
		/// Upgrade Now Button Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("upgradeNowButtonTitle")]
		public string UpgradeNowButtonTitle => this.Value<string>("upgradeNowButtonTitle");

		///<summary>
		/// Years Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("yearsTitle")]
		public string YearsTitle => this.Value<string>("yearsTitle");
	}
}