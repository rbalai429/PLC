using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HPPlc.Models
{
    public class Responce
    {
        public HttpStatusCode StatusCode
        {
            get; set;
        }
        public object Result
        {
            get; set;
        }
		public int TotalPage
		{
			get; set;
		}

		public string Message
        {
            get; set;
        }
    }
}