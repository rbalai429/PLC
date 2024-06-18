using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Mailer
{
    public class SubscriptionMailerModel
    {

        public string HeaderLogoUrl
        {
            get; set;
        }
        public string HeaderUrl
        {
            get; set;
        }
        public IHtmlString Body
        {
            get; set;
        }
        public string VisitPrintCenterLogoUrl
        {
            get; set;

        }
        public string VisitPrintCenterUrl
        {
            get; set;
        }
        public IHtmlString SocialShare
        {
            get; set;
        }
        public IHtmlString Footer
        {
            get; set;
        }
        public string Ranking
        {
            get; set;
        }
        public string ViewName
        {
            get; set;
        }
        public string PDFUrl
        {
            get; set;
        }

        public string TransactionId
        {
            get; set;
        }

		public string Subject
		{
			get; set;
		}
		public IEnumerable<string> EmailBcc
		{
			get; set;
		}
		public IEnumerable<string> EmailCC
		{
			get; set;
		}

	}
}