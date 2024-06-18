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
	/// <summary>Bonus Pay Response</summary>
	[PublishedModel("bonusPayResponse")]
	public partial class BonusPayResponse : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "bonusPayResponse";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<BonusPayResponse, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public BonusPayResponse(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Aborted Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("abortedMessage")]
		public string AbortedMessage => this.Value<string>("abortedMessage");

		///<summary>
		/// Aborted Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("abortedTitle")]
		public string AbortedTitle => this.Value<string>("abortedTitle");

		///<summary>
		/// Awaited Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("awaitedMessage")]
		public string AwaitedMessage => this.Value<string>("awaitedMessage");

		///<summary>
		/// Awaited Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("awaitedTitle")]
		public string AwaitedTitle => this.Value<string>("awaitedTitle");

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("description")]
		public global::System.Web.IHtmlString Description => this.Value<global::System.Web.IHtmlString>("description");

		///<summary>
		/// Error Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("errorMessage")]
		public string ErrorMessage => this.Value<string>("errorMessage");

		///<summary>
		/// Error Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("errorTitle")]
		public string ErrorTitle => this.Value<string>("errorTitle");

		///<summary>
		/// Failed Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("failedMessage")]
		public string FailedMessage => this.Value<string>("failedMessage");

		///<summary>
		/// Failed Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("failedTitle")]
		public string FailedTitle => this.Value<string>("failedTitle");

		///<summary>
		/// Footer Items
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("footerItems")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.TitleDescriptionAndNested> FooterItems => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.TitleDescriptionAndNested>>("footerItems");

		///<summary>
		/// Pending Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("pendingMessage")]
		public string PendingMessage => this.Value<string>("pendingMessage");

		///<summary>
		/// Pending Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("pendingTitle")]
		public string PendingTitle => this.Value<string>("pendingTitle");

		///<summary>
		/// Popup Home Text
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("popupHomeText")]
		public string PopupHomeText => this.Value<string>("popupHomeText");

		///<summary>
		/// Success Message
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("successMessage")]
		public string SuccessMessage => this.Value<string>("successMessage");

		///<summary>
		/// Success Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("successTitle")]
		public string SuccessTitle => this.Value<string>("successTitle");

		///<summary>
		/// Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("title")]
		public string Title => this.Value<string>("title");
	}
}
