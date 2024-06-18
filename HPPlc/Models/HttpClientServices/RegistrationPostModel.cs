using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HttpClientServices
{

	public class RegistrationPostModel
	{
		public string ContactKey { get; set; } = "ID601";
		public string EventDefinitionKey { get; set; } = "APIEvent-23cf4dfc-9ba3-cb03-dd27-84b6e225a1cf";
		public Item Data
		{
			get; set;
		}
	}

	public class UpdatePostedModel
	{
		//public string ContactKey { get; set; } = "ID601";
		//public string EventDefinitionKey { get; set; } = "APIEvent-23cf4dfc-9ba3-cb03-dd27-84b6e225a1cf";
		public keys keys { get; set; }
		public Item values
		{
			get; set;
		}
	}

	public class BonusSubscriptionData
	{
		//public string ContactKey { get; set; } = "ID601";
		//public string EventDefinitionKey { get; set; } = "APIEvent-23cf4dfc-9ba3-cb03-dd27-84b6e225a1cf";
		public keys keys { get; set; }
		public BonusItem values
		{
			get; set;
		}
	}


	public class UnSubscribePostModel
	{
		public string ContactKey { get; set; } = "ID601";
		public string EventDefinitionKey { get; set; } = "APIEvent-052eb9d4-aad9-dcd8-6983-c2499c230763";
		public UnSubscribeItem Data
		{
			get; set;
		}
	}
	public class Item
	{
		public int? UserId
		{
			get; set;
		}
		public string userUniqueId
		{
			get; set;
		}
		public string DataSource
		{
			get; set;
		}
		public string u_name
		{
			get; set;
		}
		public string u_email
		{
			get; set;
		}

		public string u_whatsappno
		{
			get; set;
		}

		public string EmailConsent
		{
			get; set;
		}
		public string WhatsAppConsent
		{
			get; set;
		}
		public string age_group
		{
			get; set;
		}

		public string Subscriber_Key
		{
			get; set;
		}

		public string Date_of_Subscriber
		{
			get; set;
		}

		public string register_date
		{
			get; set;
		}

		public string update_date
		{
			get; set;
		}

		public string Invoice_Link
		{
			get; set;
		}

		public string UserId_Enc
		{
			get; set;
		}
		public string TransationId
		{
			get; set;
		}

		public string Free_Plan_TRUE_FALSE
		{
			get; set;
		}
		public string INR199_Plan_TRUE_FALSE
		{
			get; set;
		}
		public string INR599_Plan_TRUE_FALSE
		{
			get; set;
		}
		public string INR899_Plan_TRUE_FALSE
		{
			get; set;
		}
		public string Free_Plan_Classes
		{
			get; set;
		}
		public string INR199_Plan_Classes
		{
			get; set;
		}
		public string INR599_Plan_Classes
		{
			get; set;
		}
		public string INR899_Plan_Classes
		{
			get; set;
		}
		public string Free_Plan_Validation_TimePeriod
		{
			get; set;
		}
		public string INR199_Plan_Validation_TimePeriod
		{
			get; set;
		}
		public string INR599_Plan_Validation_TimePeriod
		{
			get; set;
		}
		public string INR899_Plan_Validation_TimePeriod
		{
			get; set;
		}

		public string INR399_Plan_Classes
		{
			get; set;
		} = "";
		public string INR799_Plan_Classes
		{
			get; set;
		} = "";
		public string INR1099_Plan_Classes
		{
			get; set;
		} = "";
		public string Is_subscribe_MD5
		{
			get; set;
		} = "";
		public string UserType
		{
			get; set;
		} = "";
		public string Ex_Field_1
		{
			get; set;
		} = "";
		public string Ex_Field_2
		{
			get; set;
		} = "";
		public string Ex_Field_3
		{
			get; set;
		} = "";
		public string Ex_Field_4
		{
			get; set;
		} = "";
		public string Ex_Field_5
		{
			get; set;
		} = "";

	}

	public class keys
	{
		public string Subscriber_Key
		{
			get; set;
		}
	}
	public class UnSubscribeItem
	{
		public int? UserId
		{
			get; set;
		}
		public string UnSubscribeOption
		{
			get; set;
		}
		public string OtherOption
		{
			get; set;
		}
		public int IsActive
		{
			get; set;
		}
		public string DateOfUnSubscribe
		{
			get; set;
		}
		public string TransactionType
		{
			get; set;
		}
	}
	public class PostResult
	{
		public string requestId
		{
			get; set;
		}
		public string ResultMessage
		{
			get; set;
		}
	}

	public class BonusItem
	{
		public string UserUniqueId
		{
			get; set;
		}
		public string ActualPayment
		{
			get; set;
		}
		public string age_group
		{
			get; set;
		}
		public string CouponCode
		{
			get; set;
		}
		public string CouponDiscountAmt
		{
			get; set;
		}
		public string CouponSource
		{
			get; set;
		}
		public string DateOfCreation
		{
			get; set;
		}
		public string ExistingUserDiscountAmt
		{
			get; set;
		}
		public string InvoiceNo
		{
			get; set;
		}
		public string PartCode
		{
			get; set;
		}
		public string PaymentDate
		{
			get; set;
		}
		public string PaymentId
		{
			get; set;
		}
		public string PaymentStatus
		{
			get; set;
		}
		public string Ranking
		{
			get; set;
		}
		public string SubscriptionDuration
		{
			get; set;
		}
		public string SubscriptionName
		{
			get; set;
		}
		public string SubscriptionPrice
		{
			get; set;
		}
		public string u_email
		{
			get; set;
		}
		public string u_name
		{
			get; set;
		}
		public string UserType
		{
			get; set;
		}
		public string DataSource
		{
			get; set;
		}
		public string Date_of_Subscriber
		{
			get; set;
		}
		public string EmailConsent
		{
			get; set;
		}
		public string Subscriber_Key
		{
			get; set;
		}
		public string UserId_Enc
		{
			get; set;
		}
		public string WhatsAppConsent
		{
			get; set;
		}
		public string u_whatsappno
		{
			get; set;
		}
		public string TransationId
		{
			get; set;
		}
		public string Invoice_Link
		{
			get; set;
		}
		public int userId
		{
			get; set;
		}
		public string ComWithWhatsApp
		{
			get; set;
		}
		public string ComWithPhone
		{
			get; set;
		}
		public string IsActive
		{
			get; set;
		}
		public string Mode
		{
			get; set;
		}
		public string referralCode
		{
			get; set;
		}
		public string DOC
		{
			get; set;
		}
		public string CheckedTAndC
		{
			get; set;
		}
		public string u_whatsappno_prefix
		{
			get; set;
		}
		public string register_date
		{
			get; set;
		}
		public string update_date
		{
			get; set;
		}
		public string INR199_Plan_Classes
		{
			get; set;
		}
		public string INR599_Plan_Classes
		{
			get; set;
		}
		public string INR899_Plan_Classes
		{
			get; set;
		}
		public string WS1_Plan_Classes
		{
			get; set;
		}
		public string WS2_Plan_Classes
		{
			get; set;
		}
		public string WS3_Plan_Classes
		{
			get; set;
		}
		public string WS4_Plan_Classes
		{
			get; set;
		}
		public string WS5_Plan_Classes
		{
			get; set;
		}
		public string INR199_Subscription_Date
		{
			get; set;
		}
		public string INR599_Subscription_Date
		{
			get; set;
		}
		public string INR899_Subscription_Date
		{
			get; set;
		}
		public string WS1_Subscription_Date
		{
			get; set;
		}
		public string WS2_Subscription_Date
		{
			get; set;
		}
		public string WS3_Subscription_Date
		{
			get; set;
		}
		public string WS4_Subscription_Date
		{
			get; set;
		}
		public string WS5_Subscription_Date
		{
			get; set;
		}
		public string INR199_Expiration_Date
		{
			get; set;
		}
		public string INR599_Expiration_Date
		{
			get; set;
		}
		public string INR899_Expiration_Date
		{
			get; set;
		}
		public string WS1_Expiration_Date
		{
			get; set;
		}
		public string WS2_Expiration_Date
		{
			get; set;
		}
		public string WS3_Expiration_Date
		{
			get; set;
		}
		public string WS4_Expiration_Date
		{
			get; set;
		}
		public string WS5_Expiration_Date
		{
			get; set;
		}
		public string First_Enrolled_Class
		{
			get; set;
		}
		public string Final_Expiration_Date
		{
			get; set;
		}
		public bool Multiple_Classes
		{
			get; set;
		} = false;
		public int Total_Allotted_Download_Count
		{
			get; set;
		}
		public int Download_Count
		{
			get; set;
		}
		public int Remaining_Count
		{
			get; set;
		}
		public bool Nursery_Classes_Opt_In
		{
			get; set;
		}
		public bool LKG_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool UKG_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool First_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool Second_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool Third_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool Fourth_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool Fifth_Classes_Opt_In
		{
			get; set;
		} = false;
		public bool Sixth_Classes_Opt_In
		{
			get; set;
		}
		public string Free_Expiration_Date
		{
			get; set;
		}
		public string Free_Plan_Classes
		{
			get; set;
		}
		public string Free_Subscription_Date
		{
			get; set;
		}
		public bool INR199_Plan_TRUE_FALSE
		{
			get; set;
		} = false;
		public bool INR599_Plan_TRUE_FALSE
		{
			get; set;
		} = false;
		public bool INR899_Plan_TRUE_FALSE
		{
			get; set;
		} = false;
		public bool Free_Plan_TRUE_FALSE
		{
			get; set;
		} = false;
		public string Preferred_Class
		{
			get; set;
		}
	}

	//public class RegistrationPostModel
	//{
	//    public string ContactKey { get; set; } = "ID601";
	//    public string EventDefinitionKey { get; set; } = "APIEvent-23cf4dfc-9ba3-cb03-dd27-84b6e225a1cf";
	//    public Item Data
	//    {
	//        get; set;
	//    }
	//}

	//public class UnSubscribePostModel
	//{
	//    public string ContactKey { get; set; } = "ID601";
	//    public string EventDefinitionKey { get; set; } = "APIEvent-052eb9d4-aad9-dcd8-6983-c2499c230763";
	//    public UnSubscribeItem Data
	//    {
	//        get; set;
	//    }
	//}
	//public class Item
	//{
	//    public int? UserId
	//    {
	//        get; set;
	//    }
	//    public string userUniqueId
	//    {
	//        get; set;
	//    }
	//    public string DataSource
	//    {
	//        get; set;
	//    }
	//    public string u_name
	//    {
	//        get; set;
	//    }
	//    public string u_email
	//    {
	//        get; set;
	//    }

	//    public string u_whatsappno
	//    {
	//        get; set;
	//    }

	//    public string EmailConsent
	//    {
	//        get; set;
	//    }
	//    public string WhatsAppConsent
	//    {
	//        get; set;
	//    }
	//    public string age_group
	//    {
	//        get; set;
	//    }

	//    public string Subscriber_Key
	//    {
	//        get; set;
	//    }

	//    public string Date_of_Subscriber
	//    {
	//        get; set;
	//    }

	//    public string register_date
	//    {
	//        get; set;
	//    }

	//    public string update_date
	//    {
	//        get; set;
	//    }

	//    public string Invoice_Link
	//    {
	//        get; set;
	//    }

	//    public string UserId_Enc
	//    {
	//        get; set;
	//    }
	//    public string TransationId
	//    {
	//        get; set;
	//    }

	//    public string Free_Plan_TRUE_FALSE
	//    {
	//        get; set;
	//    }
	//    public string INR199_Plan_TRUE_FALSE
	//    {
	//        get; set;
	//    }
	//    public string INR599_Plan_TRUE_FALSE
	//    {
	//        get; set;
	//    }
	//    public string INR899_Plan_TRUE_FALSE
	//    {
	//        get; set;
	//    }
	//    public string Free_Plan_Classes
	//    {
	//        get; set;
	//    }
	//    public string INR199_Plan_Classes
	//    {
	//        get; set;
	//    }
	//    public string INR599_Plan_Classes
	//    {
	//        get; set;
	//    }
	//    public string INR899_Plan_Classes
	//    {
	//        get; set;
	//    }
	//    public string Free_Plan_Validation_TimePeriod
	//    {
	//        get; set;
	//    }
	//    public string INR199_Plan_Validation_TimePeriod
	//    {
	//        get; set;
	//    }
	//    public string INR599_Plan_Validation_TimePeriod
	//    {
	//        get; set;
	//    }
	//    public string INR899_Plan_Validation_TimePeriod
	//    {
	//        get; set;
	//    }
	//}

	//public class UnSubscribeItem
	//{
	//    public int? UserId
	//    {
	//        get; set;
	//    }
	//    public string UnSubscribeOption
	//    {
	//        get; set;
	//    }
	//    public string OtherOption
	//    {
	//        get; set;
	//    }
	//    public int IsActive
	//    {
	//        get; set;
	//    }
	//    public string DateOfUnSubscribe
	//    {
	//        get; set;
	//    }
	//    public string TransactionType
	//    {
	//        get; set;
	//    }
	//}
	//public class PostResult
	//{
	//    public string requestId
	//    {
	//        get; set;
	//    }
	//    public string ResultMessage
	//    {
	//        get;set;
	//    }
	//}


}