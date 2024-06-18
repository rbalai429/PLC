using HPPlc.Models;
using HPPlc.Models.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace HPPlc.Controllers
{
    public class SFMCDataPushController : SurfaceController
    {
		dbProxy _db;
		string IsEnableSFMCCode = ConfigurationManager.AppSettings["IsEnableSFMCCode"].ToString();

		public string WorksheetPlan()
		{
			try
			{
				_db = new dbProxy();
				List<Users> users = new List<Users>();
				List<SetParameters> spreg = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@QType", Value = "1" }
								};

				users = _db.GetDataMultiple("usp_getsfmcdataentrydirect", users, spreg);
				foreach (var user in users)
				{
					if (!String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
					{
						SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
						sendDataToSFMC.PostDataSFMCBonus(user.UserId, "", "subscriptionbonus");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "SFMCDataPushController Controller";
				error.MethodName = "WorksheetPlan - Send Data to SFMC for Worksheet";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SFMCDataPushController), ex, message: "WorksheetPlan - Send Data to SFMC");
			}

			return "Done";
		}

		public string LessonPlan()
		{
			try
			{
				_db = new dbProxy();
				List<Users> users = new List<Users>();
				List<SetParameters> spreg = new List<SetParameters>()
								{
									new SetParameters{ ParameterName = "@QType", Value = "2" }
								};

				users = _db.GetDataMultiple("usp_getsfmcdataentrydirect", users, spreg);
				foreach (var user in users)
				{
					if (!String.IsNullOrWhiteSpace(IsEnableSFMCCode) && IsEnableSFMCCode == "Y")
					{
						SendDataToSFMC sendDataToSFMC = new SendDataToSFMC();
						sendDataToSFMC.PostDataSFMC(user.UserId, "", "subscription");
					}
				}
			}
			catch (Exception ex)
			{
				HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
				error.PageName = "SFMCDataPushController Controller";
				error.MethodName = "LessonPlan - Send Data to SFMC for Lesson";
				error.ErrorMessage = ex.Message;

				HPPlc.Models.dbAccessClass.PostApplicationError(error);

				Logger.Error(reporting: typeof(SFMCDataPushController), ex, message: "LessonPlan - Send Data to SFMC");
			}

			return "Done";
		}
	}
}