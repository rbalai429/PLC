using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIS.Models
{
	public class Response
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