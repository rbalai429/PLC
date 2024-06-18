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
	/// <summary>Worksheet Listing</summary>
	[PublishedModel("worksheet")]
	public partial class Worksheet : PublishedContentModel, ISEO
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new const string ModelTypeAlias = "worksheet";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Worksheet, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public Worksheet(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// All Pages Text( Age Wise): All Pages Text( Age Wise)
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("allPagesTextAgeWise")]
		public string AllPagesTextAgeWise => this.Value<string>("allPagesTextAgeWise");

		///<summary>
		/// All Pages Text( Category Wise): All Pages Text( Age Wise)
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.6")]
		[ImplementPropertyType("allPagesTextCategoryWise")]
		public string AllPagesTextCategoryWise => this.Value<string>("allPagesTextCategoryWise");

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
