using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Mailer
{
    public class MailContentModel
    {
        public string Name
        {
            get; set;
        }
        public string Subject
        {
            get; set;
        }
        public string EmailTo
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
		public IHtmlString Body
		{
			get; set;
		}
	}
}