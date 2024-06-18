//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v8.1.6
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
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace Umbraco.Web.PublishedModels
{
	/// <summary>Worksheet Category</summary>
	[PublishedModel("worksheetCategory")]
	public partial class WorksheetCategory : PublishedContentModel, IFooterContent, IFooterFaqs, ISEO
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new const string ModelTypeAlias = "worksheetCategory";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<WorksheetCategory, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public WorksheetCategory(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Subject Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("categoryName")]
		public Umbraco.Web.Models.Link CategoryName => this.Value<Umbraco.Web.Models.Link>("categoryName");

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("description")]
		public IHtmlString Description => this.Value<IHtmlString>("description");

		///<summary>
		/// Is Active
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("isActive")]
		public bool IsActive => this.Value<bool>("isActive");

		///<summary>
		/// Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("title")]
		public string Title => this.Value<string>("title");

		///<summary>
		/// Active On Page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("activeOnPage")]
		public bool ActiveOnPage => FooterContent.GetActiveOnPage(this);

		///<summary>
		/// Page Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("pageDesc")]
		public IHtmlString PageDesc => FooterContent.GetPageDesc(this);

		///<summary>
		/// Page Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("pageTitle")]
		public string PageTitle => FooterContent.GetPageTitle(this);

		///<summary>
		/// Footer FAQs
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("sEOFAQs")]
		public IEnumerable<FAqitems> SEofaqs => FooterFaqs.GetSEofaqs(this);

		///<summary>
		/// Show On Page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("showOnPage")]
		public bool ShowOnPage => FooterFaqs.GetShowOnPage(this);

		///<summary>
		/// HeadSectionScripts: HeadSectionScripts - *Not Mandatory. The Page Specific code inside the head section goes here:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("headSectionScripts")]
		public string HeadSectionScripts => SEO.GetHeadSectionScripts(this);

		///<summary>
		/// Meta Description: SEO Meta Description :
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("metaDescription")]
		public string MetaDescription => SEO.GetMetaDescription(this);

		///<summary>
		/// Meta keywords: Meta Keywords:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("metaKeywords")]
		public IEnumerable<string> MetaKeywords => SEO.GetMetaKeywords(this);

		///<summary>
		/// Meta Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("metaName")]
		public string MetaName => SEO.GetMetaName(this);

		///<summary>
		/// Page Title: Browser Title - title shown in the Browser window / tab, and most important on search-engine listings.:
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("pageName")]
		public string PageName => SEO.GetPageName(this);

		///<summary>
		/// SEO Can Index: If checked means SEO can track this page, In case of unchecked SEO can not track.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("sEOCanIndex")]
		public bool SEocanIndex => SEO.GetSEocanIndex(this);

		///<summary>
		/// SEO Follow links: SEO Follow links
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("sEOFollowLinks")]
		public bool SEofollowLinks => SEO.GetSEofollowLinks(this);
	}
}