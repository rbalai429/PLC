using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.WhatsApp
{
    public class MessageResponse
    {
        public List<Response> messages { get; set; }
        public Meta meta { get; set; }
    }

    public class Response
    {
        public string id { get; set; }
    }
    public class Meta
    {
        public string api_status { get; set; }
        public string version { get; set; }
    }
}