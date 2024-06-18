using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HttpClientServices
{
	public class SendDataToSFMC
	{
		public string PostDataSFMC(int UserId,string InvoiceUrl,string DataSource)
		{
			try
			{
				Responce post = new Responce();
				
				Item items = new Item();
				dbProxy _db = new dbProxy();

				//ApiCallServices apiCall = new ApiCallServices();
				RegistrationPostModel postModel = new RegistrationPostModel();
				UpdatePostedModel updatePostModel = new UpdatePostedModel();
				List<UpdatePostedModel> updatePostModelListData = new List<UpdatePostedModel>();

				if (UserId > 0)
				{

					List<SetParameters> spreg = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
								};
					
					items = _db.GetData<Item>("dbo.USP_GetSFMCData", items, spreg);
					if (items != null && items.UserId > 0)
					{

						string name = String.Empty;
						string email = String.Empty;
						string mobile = String.Empty;
						string Subscriber_Key = String.Empty;

						if (!String.IsNullOrWhiteSpace(items.u_name))
							name = clsCommon.Decrypt(items.u_name);
						if (!String.IsNullOrWhiteSpace(items.u_email))
						{
							email = clsCommon.Decrypt(items.u_email);
							Subscriber_Key = MD5HashPassword.CreateMD5Hash(email.ToLower());
						}
						if (!String.IsNullOrWhiteSpace(items.u_whatsappno))
							mobile = clsCommon.Decrypt(items.u_whatsappno);

						items.u_name = name;
						items.u_email = email;
						items.u_whatsappno = mobile;
						//items.Date_of_Subscriber = DateTime.Now.ToString("MM-dd-yyyy");
						items.update_date = DateTime.Now.AddMinutes(330).ToString("MM-dd-yyyy");
						items.Invoice_Link = InvoiceUrl;
						items.Subscriber_Key = Subscriber_Key;
						items.UserId_Enc = clsCommon.Encryptwithbase64Code(UserId.ToString());
						
						if (!String.IsNullOrWhiteSpace(DataSource) && DataSource == "registration")
						{ postModel.Data = items; }
						else
						{
							keys key = new keys();
							key.Subscriber_Key = items.Subscriber_Key;

							updatePostModel.values = items;
							updatePostModel.keys = key;
							updatePostModelListData.Add(updatePostModel);
						}

						if (!String.IsNullOrWhiteSpace(DataSource) && DataSource == "registration")
						{
							ApiCallServices apiCall = new ApiCallServices();
							post = apiCall.PostRegistartionData(postModel, DataSource);
						}
						else if (!String.IsNullOrWhiteSpace(DataSource) && DataSource == "registrationInviteUser")
						{
							ApiCallServices apiCall = new ApiCallServices("registrationInviteUser");
							//post = apiCall.PostRegistartionDataRegistrationUserInvite(postModel, DataSource);
							post = apiCall.PostRegistartionDataRegistrationUserInvite(updatePostModelListData, DataSource);
						}
						else if (!String.IsNullOrWhiteSpace(DataSource) && (DataSource == "updateprofile" || DataSource == "subscription"))
						{
							ApiCallServices apiCall = new ApiCallServices();
							post = apiCall.UpdateRegistartionData(updatePostModelListData, DataSource);
						}

						//DB Log
						string status = dbAccessClass.SFMCResponse(items.TransationId, items.UserId.ToString(), post);

					}

					return post.StatusCode.ToString();
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "SFMC Data";
				error.MethodName = "Save RquestId in Sync Solution";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
			}

			return "fail";
		}

		public string PostDataSFMCBonus(int UserId, string InvoiceUrl, string DataSource)
		{
			try
			{
				Responce post = new Responce();

				BonusItem items = new BonusItem();
				BonusSubscriptionData bonusSubscriptionDataSingle = new BonusSubscriptionData();
				List<BonusSubscriptionData> bonusSubscriptionData = new List<BonusSubscriptionData>();
				dbProxy _db = new dbProxy();


				if (UserId > 0)
				{

					List<SetParameters> spreg = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@QType", Value = "1" },
									new SetParameters{ ParameterName = "@UserId", Value = UserId.ToString() }
								};

					items = _db.GetData<BonusItem>("dbo.USP_GetSFMCData", items, spreg);
					if (items != null && items.userId > 0)
					{

						string name = String.Empty;
						string email = String.Empty;
						string mobile = String.Empty;
						string Subscriber_Key = String.Empty;

						if (!String.IsNullOrWhiteSpace(items.u_name))
							name = clsCommon.Decrypt(items.u_name);
						if (!String.IsNullOrWhiteSpace(items.u_email))
						{
							email = clsCommon.Decrypt(items.u_email);
							Subscriber_Key = MD5HashPassword.CreateMD5Hash(email.ToLower());
						}
						if (!String.IsNullOrWhiteSpace(items.u_whatsappno))
							mobile = clsCommon.Decrypt(items.u_whatsappno);

						items.u_name = name;
						items.u_email = email;
						items.u_whatsappno = mobile;
						items.update_date = DateTime.Now.AddMinutes(330).ToString("MM-dd-yyyy");
						items.Invoice_Link = InvoiceUrl;
						items.Subscriber_Key = Subscriber_Key;
						items.UserId_Enc = clsCommon.Encryptwithbase64Code(UserId.ToString());

						keys key = new keys();
						key.Subscriber_Key = items.Subscriber_Key;


						bonusSubscriptionDataSingle.values = items;
						bonusSubscriptionDataSingle.keys = key;

						bonusSubscriptionData.Add(bonusSubscriptionDataSingle);

						ApiCallServices apiCall = new ApiCallServices();
						post = apiCall.BonusSubscriptionTracker(bonusSubscriptionData, DataSource);


						//DB Log
						string status = dbAccessClass.SFMCResponse(items.TransationId, items.userId.ToString(), post);

					}

					return post.StatusCode.ToString();
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "SFMC Data";
				error.MethodName = "Save RquestId in Sync Solution";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				//Logger.Error(reporting: typeof(HomeController), ex, message: "registration - Save RquestId in Sync Solution");
			}

			return "fail";
		}
	}
}