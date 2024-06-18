using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WhatsApp
{
    public class WhatsAppHelper
    {
        public IRestResponse SendMessage(MessageBody messageBody)
        {
            var client = new RestClient("https://whatsapp-notifications.haptikapi.com/whatsapp/notification/v2/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("client-id", "4e31ad12d08c7c93b75e67cf3b8325a0940465a7");
            request.AddHeader("Authorization", "Bearer fdb4ca223f7fb36ff99e8511b0f5c620");
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(messageBody), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response;
            //Console.WriteLine(response.Content);
        }

        public IRestResponse SendMessage_sp(MessageBody messageBody)
        {
            var client = new RestClient("https://whatsapp-notifications.haptikapi.com/whatsapp/notification/v2/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("client-id", "4e31ad12d08c7c93b75e67cf3b8325a0940465a7");
            request.AddHeader("Authorization", "Bearer fdb4ca223f7fb36ff99e8511b0f5c620");
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(messageBody), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            //HPPlc.Models.ApplicationError error = new HPPlc.Models.ApplicationError();
            //error.PageName = "Pay Response Value Input";
            //error.MethodName = "PayResponse";
            //error.ErrorMessage = request.Body.Value.ToString();

            //HPPlc.Models.dbAccessClass.PostApplicationError(error);

            return response;
            //Console.WriteLine(response.Content);
        }
        public IRestResponse CreateMessage(string to, string elmentname, WhatsAppDynamicValue content)
        {
            MessageBody messageBody = new MessageBody();
            messageBody.to = to;
            HSMTemplate hSMTemplate = new HSMTemplate();

            List<components> component = new List<components>();

            if (content != null)
            {
                //Header Template
                
                List<parameters> parameters = new List<parameters>();
                if (!String.IsNullOrWhiteSpace(content.BannerUrl))
                {
                    //List<imagebanner> image = new List<imagebanner>();
                    imageheader imageheader = new imageheader();

                    imageheader.link = content.BannerUrl;
                    //image.Add(new imagebanner { type = "image", image = imgbanner }) = content.BannerUrl;
                    parameters.Add(new parameters { type = "image", image = imageheader });

                    component.Add(new components { type = "header", parameters = parameters });
                }

                //Body Template
                List<parameters> bodyparameter = new List<parameters>();
				if (!String.IsNullOrWhiteSpace(content.Name))
					bodyparameter.Add(new parameters { type = "text", text = content.Name });
				else
					bodyparameter.Add(new parameters { type = "text", text = "Parent" });

				if (!String.IsNullOrWhiteSpace(content.Subjects))
                    bodyparameter.Add(new parameters { type = "text", text = content.Subjects });

				if (!String.IsNullOrWhiteSpace(content.Domain))
					bodyparameter.Add(new parameters { type = "text", text = content.Domain });

				if (!String.IsNullOrWhiteSpace(content.PdfUrl))
                    bodyparameter.Add(new parameters { type = "text", text = content.PdfUrl });

                component.Add(new components { type = "body", parameters = bodyparameter });
            }

           
            hSMTemplate.components.AddRange(component);

            hSMTemplate.name = elmentname;
            messageBody.template = hSMTemplate;
            IRestResponse response = SendMessage(messageBody);

            return response;
        }

        public IRestResponse CreateMessageForSpecialPlan(string to, string elmentname, WhatsAppDynamicValue content)
        {
            MessageBody messageBody = new MessageBody();
            messageBody.to = to;
            HSMTemplate hSMTemplate = new HSMTemplate();

            List<components> component = new List<components>();

            string domain = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            if (content != null)
            {
                //Header Template
                List<parameters> headparameters = new List<parameters>();
                List<parameters> bodyparameters = new List<parameters>();
                if (!String.IsNullOrWhiteSpace(content.BannerUrl))
				{
                    imageheader imageheader = new imageheader();
                    imageheader.link = domain + content.BannerUrl;

                    headparameters.Add(new parameters { type = "image", image = imageheader });

					component.Add(new components { type = "header", parameters = headparameters });
				}

				//Body Template
                if (!String.IsNullOrWhiteSpace(content.PdfUrl))
                    bodyparameters.Add(new parameters { type = "text", text = content.PdfUrl });

                component.Add(new components { type = "body", parameters = bodyparameters });
            }


            hSMTemplate.components.AddRange(component);

            hSMTemplate.name = elmentname.Trim();
            messageBody.template = hSMTemplate;
            IRestResponse response = SendMessage_sp(messageBody);

            return response;
        }

        public IRestResponse CreateMessageForBulk(string to, string elmentname, WhatsAppDynamicValue content)
        {
            MessageBody messageBody = new MessageBody();
            messageBody.to = to;
            HSMTemplate hSMTemplate = new HSMTemplate();

            List<components> component = new List<components>();

            if (content != null)
            {
                //Header Template

                List<parameters> parameters = new List<parameters>();
                if (!String.IsNullOrEmpty(content.HeaderIsBannerOrVideo))
                {
                    if (!String.IsNullOrWhiteSpace(content.BannerUrl) && content.HeaderIsBannerOrVideo == "Banner")
                    {
                        //List<imagebanner> image = new List<imagebanner>();
                        imageheader imageheader = new imageheader();

                        imageheader.link = content.BannerUrl;
                        //image.Add(new imagebanner { type = "image", image = imgbanner }) = content.BannerUrl;
                        parameters.Add(new parameters { type = "image", image = imageheader });

                        component.Add(new components { type = "header", parameters = parameters });
                    }
                    else if (!String.IsNullOrWhiteSpace(content.VideoUrl) && content.HeaderIsBannerOrVideo == "Video")
                    {
                        //List<imagebanner> image = new List<imagebanner>();
                        videoheader videoheader = new videoheader();

                        videoheader.link = content.VideoUrl;
                        //image.Add(new imagebanner { type = "image", image = imgbanner }) = content.BannerUrl;
                        parameters.Add(new parameters { type = "video", video = videoheader });

                        component.Add(new components { type = "header", parameters = parameters });
                    }
                }

                //Body Template
                List<parameters> bodyparameter = new List<parameters>();
                if (!String.IsNullOrWhiteSpace(content.Name))
                    bodyparameter.Add(new parameters { type = "text", text = content.Name });

                if (!String.IsNullOrWhiteSpace(content.Subjects))
                    bodyparameter.Add(new parameters { type = "text", text = content.Subjects });

                if (!String.IsNullOrWhiteSpace(content.Domain))
                    bodyparameter.Add(new parameters { type = "text", text = content.Domain });

                if (!String.IsNullOrWhiteSpace(content.PdfUrl))
                    bodyparameter.Add(new parameters { type = "text", text = content.PdfUrl });

                if(bodyparameter != null && bodyparameter.Count > 0)
                    component.Add(new components { type = "body", parameters = bodyparameter });
            }


            hSMTemplate.components.AddRange(component);

            hSMTemplate.name = elmentname;
            messageBody.template = hSMTemplate;
            IRestResponse response = SendMessage(messageBody);

            return response;
        }
    }
}