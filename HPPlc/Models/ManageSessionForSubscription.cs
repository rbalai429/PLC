using HPPlc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class ManageSessionForSubscription
	{
		public SubscriptionDetails SetSessionForSubs(string targetUrl, string subscriptionId, string ageGroup, string mode = "")
		{
			string subscribeid = clsCommon.Decrypt(subscriptionId);
			SubscriptionDetails subscriptionDetails = new SubscriptionDetails();
			subscriptionDetails.targetUrl = targetUrl;
			subscriptionDetails.subscriptionId = subscribeid;
			subscriptionDetails.ageGroup = ageGroup;

			if (!String.IsNullOrEmpty(SessionManagement.GetCurrentSession<string>(SessionType.UserClickedOnWorksheet)))
			{
				subscriptionDetails.WorksheetId = SessionManagement.GetCurrentSession<string>(SessionType.UserClickedOnWorksheet);
				SessionManagement.DeleteFromSession(SessionType.UserClickedOnWorksheet);
			}

			SessionManagement.StoreInSession(SessionType.SubscriptionDtls, subscriptionDetails);


			try
			{
				SubscriptionController subscriptionCnt = new SubscriptionController();
				if (!String.IsNullOrEmpty(ageGroup))
					ageGroup = clsCommon.Decrypt(ageGroup);

				string status = String.Empty;

				if (!String.IsNullOrEmpty(mode) && mode == "teacher")
					status = subscriptionCnt.SetAgeGroupForAddSubscriptionTeacher(ageGroup);
				else
					status = subscriptionCnt.SetAgeGroupForAddSubscription(ageGroup);

			}
			catch { }

			//try
			//{
			//	// Store subscription data in temp session
			//	//if (!String.IsNullOrWhiteSpace(ageGroup))
			//	//{
			//	SubscriptionController subscriptionCnt = new SubscriptionController();
			//	ageGroup = clsCommon.Decrypt(ageGroup);
			//	string status = subscriptionCnt.SetAgeGroupForAddSubscription(ageGroup);
			//	//}
			//}
			//catch { }

			return subscriptionDetails;
		}

		public string SetSessionForSpecialRedirection(string targetUrl, string target)
		{
			if (!String.IsNullOrWhiteSpace(targetUrl) && !String.IsNullOrWhiteSpace(target))
			{
				SpecialRedirection specialRedirection = new SpecialRedirection();
				specialRedirection.TargetUrl = targetUrl;
				specialRedirection.Target = target;

				SessionManagement.StoreInSession(SessionType.SpecialRedirection, specialRedirection);
			}

			return "Done";
		}
	}
}