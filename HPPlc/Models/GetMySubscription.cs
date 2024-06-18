using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class GetMySubscription
	{
		public List<GetYourSubscriptionDetails> mySubscriptions(int UserId)
		{
			List<GetYourSubscriptionDetails> subscriptionDetails = new List<GetYourSubscriptionDetails>();
			dbProxy _db = new dbProxy();
			List<SetParameters> sp = new List<SetParameters>()
						{
							new SetParameters{ ParameterName = "@QType", Value = "1" },
							new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
						};

			
			subscriptionDetails = _db.GetDataMultiple<GetYourSubscriptionDetails>("usp_getdata", subscriptionDetails, sp);


			return subscriptionDetails;
		}
	}
}