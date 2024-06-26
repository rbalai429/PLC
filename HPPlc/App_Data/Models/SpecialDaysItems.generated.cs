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
	/// <summary>Special Days Items</summary>
	[PublishedModel("specialDaysItems")]
	public partial class SpecialDaysItems : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "specialDaysItems";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<SpecialDaysItems, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public SpecialDaysItems(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Age Group
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("ageGroup")]
		public global::Umbraco.Web.Models.Link AgeGroup => this.Value<global::Umbraco.Web.Models.Link>("ageGroup");

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("description")]
		public global::System.Web.IHtmlString Description => this.Value<global::System.Web.IHtmlString>("description");

		///<summary>
		/// Desktop Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("desktopImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent DesktopImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("desktopImage");

		///<summary>
		/// Desktop Next-Gen Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("desktopNextGenImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent DesktopNextGenImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("desktopNextGenImage");

		///<summary>
		/// Facebook Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("facebookContent")]
		public string FacebookContent => this.Value<string>("facebookContent");

		///<summary>
		/// Is Active
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isActive")]
		public bool IsActive => this.Value<bool>("isActive");

		///<summary>
		/// Is Worksheet Locked
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isWorksheetLocked")]
		public bool IsWorksheetLocked => this.Value<bool>("isWorksheetLocked");

		///<summary>
		/// Locked Desktop Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("lockedDesktopImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent LockedDesktopImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("lockedDesktopImage");

		///<summary>
		/// Locked Desktop Next-Gen Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("lockedDesktopNextGenImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent LockedDesktopNextGenImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("lockedDesktopNextGenImage");

		///<summary>
		/// Locked Mobile Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("lockedMobileImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent LockedMobileImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("lockedMobileImage");

		///<summary>
		/// Locked Mobile Next-Gen Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("lockedMobileNextGenImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent LockedMobileNextGenImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("lockedMobileNextGenImage");

		///<summary>
		/// Locked Next-Gen Mobile Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("lockedNextGenMobileImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent LockedNextGenMobileImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("lockedNextGenMobileImage");

		///<summary>
		/// Mail Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mailContent")]
		public string MailContent => this.Value<string>("mailContent");

		///<summary>
		/// Mobile Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mobileImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent MobileImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("mobileImage");

		///<summary>
		/// Mobile Next-Gen Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mobileNextGenImage")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent MobileNextGenImage => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("mobileNextGenImage");

		///<summary>
		/// No of Days
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("noOfDays")]
		public global::Umbraco.Web.Models.Link NoOfDays => this.Value<global::Umbraco.Web.Models.Link>("noOfDays");

		///<summary>
		/// Preview Document
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("previewDocument")]
		public string PreviewDocument => this.Value<string>("previewDocument");

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
		/// Upload Document
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("uploadDocument")]
		public string UploadDocument => this.Value<string>("uploadDocument");

		///<summary>
		/// Use Share Content from here
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("useShareContentFromHere")]
		public bool UseShareContentFromHere => this.Value<bool>("useShareContentFromHere");

		///<summary>
		/// WhatsApp Content
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppContent")]
		public string WhatsAppContent => this.Value<string>("whatsAppContent");

		///<summary>
		/// WhatsApp Share Banner
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("whatsAppShareBanner")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent WhatsAppShareBanner => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("whatsAppShareBanner");
	}
}
