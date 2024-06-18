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
	/// <summary>Teachers Subscription Benefits Element</summary>
	[PublishedModel("teachersSubscriptionBenefitsElement")]
	public partial class TeachersSubscriptionBenefitsElement : PublishedElementModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "teachersSubscriptionBenefitsElement";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<TeachersSubscriptionBenefitsElement, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public TeachersSubscriptionBenefitsElement(IPublishedElement content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Benefits Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("benefitsTitle")]
		public global::Umbraco.Core.Models.PublishedContent.IPublishedContent BenefitsTitle => this.Value<global::Umbraco.Core.Models.PublishedContent.IPublishedContent>("benefitsTitle");

		///<summary>
		/// Choose Tick
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("chooseTick")]
		public string ChooseTick => this.Value<string>("chooseTick");

		///<summary>
		/// Enter Title any
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("enterTitleAny")]
		public string EnterTitleAny => this.Value<string>("enterTitleAny");

		///<summary>
		/// Is Active
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isActive")]
		public bool IsActive => this.Value<bool>("isActive");

		///<summary>
		/// SCoflag
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("sCoflag")]
		public bool SCoflag => this.Value<bool>("sCoflag");
	}
}
