using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.PublishedModels;

namespace HPPlc.Models.Mailer
{
    public class RegistrationMailerModel
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Link
        {
            get; set;
        }
        public string HeaderBanner
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public string HelloText
        {
            get; set;
        }
        public IHtmlString BodyContent
        {
            get; set;
        }
        public string VerifyButton
        {
            get; set;
        }
        public IHtmlString FooterText
        {
            get; set;
        }
        public string UnsubscribeButton
        {
            get; set;
        }
        public string ViewName
        {
            get; set;
        }
		public string SubscriptionUrl
		{
			get; set;
		}
	}
}