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
	/// <summary>Teacher Program Items</summary>
	[PublishedModel("teacherProgramItems")]
	public partial class TeacherProgramItems : PublishedContentModel, IFooterContent, IFooterFaqs, ISEO
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "teacherProgramItems";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<TeacherProgramItems, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public TeacherProgramItems(IPublishedContent content)
			: base(content)
		{ }

		// properties

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
		/// IsEnableForDetailsPage
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isEnableForDetailsPage")]
		public bool IsEnableForDetailsPage => this.Value<bool>("isEnableForDetailsPage");

		///<summary>
		/// Is Guest User Sheet
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isGuestUserSheet")]
		public bool IsGuestUserSheet => this.Value<bool>("isGuestUserSheet");

		///<summary>
		/// Paid
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isPaid")]
		public bool IsPaid => this.Value<bool>("isPaid");

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
		/// Ranking Index
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("rankingIndex")]
		public int RankingIndex => this.Value<int>("rankingIndex");

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
		/// umbracoUrlAlias: Please must be enter here for worksheet details page URL.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("umbracoUrlAlias")]
		public string UmbracoUrlAlias => this.Value<string>("umbracoUrlAlias");

		///<summary>
		/// Upload PDF
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("uploadPDF")]
		public string UploadPdf => this.Value<string>("uploadPDF");

		///<summary>
		/// Upload Preview PDF
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("uploadPreviewPDF")]
		public string UploadPreviewPdf => this.Value<string>("uploadPreviewPDF");

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
		/// Active On Page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("activeOnPage")]
		public bool ActiveOnPage => global::Umbraco.Web.PublishedModels.FooterContent.GetActiveOnPage(this);

		///<summary>
		/// Page Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("pageDesc")]
		public global::System.Web.IHtmlString PageDesc => global::Umbraco.Web.PublishedModels.FooterContent.GetPageDesc(this);

		///<summary>
		/// Page Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("pageTitle")]
		public string PageTitle => global::Umbraco.Web.PublishedModels.FooterContent.GetPageTitle(this);

		///<summary>
		/// Footer FAQs
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("sEOFAQs")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.FAqitems> SEofaqs => global::Umbraco.Web.PublishedModels.FooterFaqs.GetSEofaqs(this);

		///<summary>
		/// Show On Page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("showOnPage")]
		public bool ShowOnPage => global::Umbraco.Web.PublishedModels.FooterFaqs.GetShowOnPage(this);

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