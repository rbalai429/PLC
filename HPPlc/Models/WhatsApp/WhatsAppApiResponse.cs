using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WhatsApp
{
	public class WhatsAppApiResponse
	{
        public int StatusCode
        {
            get; set;
        }

        public string StatusMessage
        {
            get; set;
        }
        public object Result
        {
            get; set;
        }
    }
}