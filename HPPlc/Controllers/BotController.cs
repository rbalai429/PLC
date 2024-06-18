using HPPlc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace HPPlc.Controllers
{
    //[RoutePrefix("api/bot")]
    public class BotController : SurfaceController
    {
        [HttpPost]
        public ActionResult GetBotSubscriptionRequest(string email,string pay)
        {
            if (!String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(pay))
            {
                SessionManagement.DeleteFromSession(SessionType.SubscriptionBotDtls);
                SessionManagement.DeleteFromSession(SessionType.SubscriptionTempDtls);

                SessionManagement.StoreInSession(SessionType.IsBotRequest, "Yes");
                List<BotPaySubscriptionDetails> subscriptionDetails = new List<BotPaySubscriptionDetails>();
                
                string[] Returnparam;

                string emailChk = clsCommon.Decrypt(email);
                pay = clsCommon.Decrypt(pay);

                if (!String.IsNullOrWhiteSpace(pay))
                {
                    string[] paystring = pay.Split('&');
                    if (paystring.Length > 0)
                    {
                        for (int i = 0; i < paystring.Length; i++)
                        {
                            Returnparam = paystring[i].Split(',');
                            if (Returnparam.Length > 0)
                            {
                                var strings = new List<string> { "renew", "upgrade", "new" };
                                string compareString = Returnparam[2].ToString();
                                bool contains = strings.Contains(compareString, StringComparer.OrdinalIgnoreCase);
                                if (contains)
                                {
                                    HomeController home = new HomeController();
                                    var maxSubscription = home.GetMaxSubscriptionRanking();

                                    dbProxy _db = new dbProxy();
                                    BotPaySubscriptionDetails botSubscriptionValidation = new BotPaySubscriptionDetails();
                                    List<SetParameters> sp = new List<SetParameters>()
                                    {
                                        new SetParameters{ ParameterName = "@QType", Value = "2" },
                                        new SetParameters{ ParameterName = "@email", Value = email },
                                        new SetParameters{ ParameterName = "@Culture", Value = "" },
                                        new SetParameters{ ParameterName = "@maxRanking", Value = maxSubscription },
                                        new SetParameters{ ParameterName = "@type", Value = Returnparam[2].ToString() },
                                        new SetParameters{ ParameterName = "@agegroup", Value = Returnparam[1].ToString() },
                                        new SetParameters{ ParameterName = "@rank", Value = Returnparam[0].ToString() }
                                    };
                                    botSubscriptionValidation = _db.GetData<BotPaySubscriptionDetails>("usp_getdata_bot", botSubscriptionValidation, sp);
                                    if (botSubscriptionValidation != null)
                                    {
                                        subscriptionDetails.Add(new BotPaySubscriptionDetails()
                                        {
                                            Ranking = botSubscriptionValidation.Ranking,
                                            ageGroup = botSubscriptionValidation.ageGroup
                                        });
                                    }
                                    else
                                    {
                                        subscriptionDetails.Add(new BotPaySubscriptionDetails()
                                        {
                                            Ranking = "0",
                                            ageGroup = Returnparam[1].ToString()
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                SessionManagement.StoreInSession(SessionType.SubscriptionBotDtls, subscriptionDetails);
                bool IsLoggedIn = HPPlc.Models.SessionExpireAttribute.UserLoggedIn();
                if (IsLoggedIn)
                    return Json(new { status = "Ok", navigation = "subscription/buy-now", message = "buy" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = "Ok", navigation = "my-account/login", message = "login" }, JsonRequestBehavior.AllowGet);
                
            }

            return View();
        }
    }
}