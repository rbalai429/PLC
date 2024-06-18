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
	/// <summary>Notification for Paid User</summary>
	[PublishedModel("notificationForPaidUser")]
	public partial class NotificationForPaidUser : Notifications
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const string ModelTypeAlias = "notificationForPaidUser";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<NotificationForPaidUser, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public NotificationForPaidUser(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// End Time
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
		/// Notification Mapping
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("notificationMapping")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationItems1> NotificationMapping => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationItems1>>("notificationMapping");

		///<summary>
		/// Notification Mapping for Upgradation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("notificationMappingForUpgradation")]
		public global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationUpgradeMessageForPaidUser> NotificationMappingForUpgradation => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Web.PublishedModels.NotificationUpgradeMessageForPaidUser>>("notificationMappingForUpgradation");

		///<summary>
		/// Start Time
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "8.12.2")]
		[ImplementPropertyType("startTime")]
		public global::System.DateTime StartTime => this.Value<global::System.DateTime>("startTime");
	}
}