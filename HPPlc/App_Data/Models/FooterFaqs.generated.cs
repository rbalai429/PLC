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
	// Mixin Content Type with alias "footerFAQs"
	/// <summary>Footer FAQs</summary>
	public partial interface IFooterFaqs : IPublishedContent
	{
		/// <summary>Footer FAQs</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.FAqitems> SEofaqs { get; }

		/// <summary>Show On Page</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		bool ShowOnPage { get; }
	}

	/// <summary>Footer FAQs</summary>
	[PublishedModel("footerFAQs")]
	public partial class FooterFaqs : PublishedContentModel, IFooterFaqs
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "footerFAQs";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<FooterFaqs, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public FooterFaqs(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Footer FAQs
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("sEOFAQs")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.FAqitems> SEofaqs => GetSEofaqs(this);

		/// <summary>Static getter for Footer FAQs</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.FAqitems> GetSEofaqs(IFooterFaqs that) => that.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.FAqitems>>("sEOFAQs");

		///<summary>
		/// Show On Page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("showOnPage")]
		public bool ShowOnPage => GetShowOnPage(this);

		/// <summary>Static getter for Show On Page</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static bool GetShowOnPage(IFooterFaqs that) => that.Value<bool>("showOnPage");
	}
}