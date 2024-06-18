using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
    public class ExpertTalk
    {
        public string Name
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string ContactPrefix
        {
            get; set;
        }public string Contact
        {
            get; set;
        }
        public string MettingName
        {
            get; set;
        }
        
        public DateTime? MettingDate
        {
            get; set;
        }
        public DateTime? JoinDate
        {
            get; set;
        }
    }
}