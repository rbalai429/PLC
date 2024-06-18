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
	/// <summary>Notification Template 1</summary>
	[PublishedModel("notificationItem")]
	public partial class NotificationItem : Notifications
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "notificationItem";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<NotificationItem, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public NotificationItem(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// End time
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("endTime")]
		public global::System.DateTime EndTime => this.Value<global::System.DateTime>("endTime");

		///<summary>
		/// IsActive
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("isActive")]
		public bool IsActive => this.Value<bool>("isActive");

		///<summary>
		/// Subscription Type
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("mode")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.Models.Link> Mode => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.Models.Link>>("mode");

		///<summary>
		/// Notification Mapping
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("notificationMapping")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationItems> NotificationMapping => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationItems>>("notificationMapping");

		///<summary>
		/// Start time
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("startTime")]
		public global::System.DateTime StartTime => this.Value<global::System.DateTime>("startTime");

		///<summary>
		/// Types of Notification
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("typesOfNotification")]
		public string TypesOfNotification => this.Value<string>("typesOfNotification");

		///<summary>
		/// User Type
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("userType")]
		public string UserType => this.Value<string>("userType");
	}
}