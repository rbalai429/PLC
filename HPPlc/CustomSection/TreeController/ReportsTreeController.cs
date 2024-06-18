using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.ModelBinding;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;

namespace HPPlc.CustomSection.Reports.TreeController
{
    [Tree("Reports", "Views", TreeTitle = "Reports", TreeGroup = "ReportsGroup", SortOrder = 5)]
    [PluginController("Reports")]
    public class ReportsTreeController : Umbraco.Web.Trees.TreeController
    {
       

        protected override MenuItemCollection GetMenuForNode(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

                // root actions, perhaps users can create new items in this tree, or perhaps it's not a content tree, it might be a read only tree, or each node item might represent something entirely different...
                // add your menu item actions or custom ActionMenuItems
                menu.Items.Add(new CreateChildEntity(Services.TextService));
                // add refresh menu item (note no dialog)
                menu.Items.Add(new RefreshNode(Services.TextService, true));
                return menu;

        }

        protected override TreeNodeCollection GetTreeNodes(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {

            // create our node collection
            var nodes = new TreeNodeCollection();

            // loop through our favourite things and create a tree item for each one

            // add each node to the tree collection using the base CreateTreeNode method
            // it has several overloads, using here unique Id of tree item, -1 is the Id of the parent node to create, eg the root of this tree is -1 by convention - the querystring collection passed into this route - the name of the tree node -  css class of icon to display for the node - and whether the item has child nodes
            //var node = CreateTreeNode("1", "-1", queryStrings, "Webinar", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "WebinarReports"));

            //    nodes.Add(node);

			//var webinarList = CreateTreeNode("1", "-1", queryStrings, "Webinar List", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "WebinarLists"));
			//nodes.Add(webinarList);
			var registrationReport = CreateTreeNode("1", "-1", queryStrings, "Registration Report", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "RegistrationReport"));
			nodes.Add(registrationReport);

			var subscriptionReport = CreateTreeNode("1", "-1", queryStrings, "Subscriptions Report", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "SubscriptionsReport"));
			nodes.Add(subscriptionReport);

			//var referralDetailReport = CreateTreeNode("1", "-1", queryStrings, "ReferralDetail Report", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ReferralDetailReport"));
			//nodes.Add(referralDetailReport);

			//var referralTransactionsReport = CreateTreeNode("1", "-1", queryStrings, "ReferralTransactions Report", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ReferralTransactionsReport"));
			//nodes.Add(referralTransactionsReport);

			
			nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Import Sftp Excel", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ImportExcelFile")));
			nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Import Local Excel", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ImportLocalExcelFile")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "FAQ Request", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "FAQRequest")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "User Login Log", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "UserLogin")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "WorkSheet Download Log", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "WorkSheetDownload")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Coupon Code", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "CouponCode")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Coupon Code Log", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "CouponCodeLog")));

            
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "User Download data", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "UserDownloadData")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Referral details", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ReferralDetails")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Notification data", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "NotificationData")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "OTP data", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "OTPData")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "URL redirection entry in table", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "URLRedirectionEntryTable")));

            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "-----------", "", false, string.Format("{0}/{1}/{2}", "", "", "")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Images By Node", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "ImagesByNode")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Worksheet Bulk Upload", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "WorksheetBulkUpload")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Worksheet Bulk Update", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "WorksheetBulkUploadUpdate")));

            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Send WhatsApp Notification", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "SendWhatsAppNotification")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "Age Group For Advance Search", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "AgeGroupSynonimsNameForAdvanceSearch")));
            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "No Record Found User Data", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "NoRecordFoundSearch")));

            nodes.Add(CreateTreeNode("2", "-1", queryStrings, "User Transaction Data", "icon-presentation", false, string.Format("{0}/{1}/{2}", "Reports", "Views", "UserTransaction")));
            return nodes;

        }

        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            var root = base.CreateRootNode(queryStrings);
            root.RoutePath = string.Format("{0}/{1}/{2}", "Reports", "Views", "WebinarReports");

            // set the icon
            root.Icon = "icon-hearts";
            // could be set to false for a custom tree with a single node.
            root.HasChildren = true;
            //url for menu
            root.MenuUrl = null;

            return root;
        }
    }
}