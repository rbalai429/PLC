using System.Collections.Generic;

namespace HPPlc.Models.WhatsApp
{
    public class MessageBody
    {
        public MessageBody()
        {
            template = new HSMTemplate();
        }
        public string business_id { get; } = "1776";
        public string to { get; set; }
        public string type { get; } = "template";
        public HSMTemplate template { get; set; }
    }
    //public class MessageBody_sp
    //{
    //    public MessageBody_sp()
    //    {
    //        template = new HSMTemplate_sp();
    //    }
    //    public string business_id { get; } = "1776";
    //    public string to { get; set; }
    //    public string type { get; } = "template";
    //    public HSMTemplate_sp template { get; set; }
    //}
    public class HSMTemplate
    {
        public HSMTemplate()
        {
            language = new Language();
            components = new List<components>();
        }

        public string @namespace { get; } = "fb50670c_8a59_46e0_8277_1bfc5ab6871a";
        public string name { get; set; }
        public Language language { get; set; }
        public List<components> components { get; set; }
    }

    //public class HSMTemplate_sp
    //{
    //    public HSMTemplate_sp()
    //    {
    //        language = new Language();
    //        components = new List<components_sp>();
    //    }

    //    public string @namespace { get; } = "fb50670c_8a59_46e0_8277_1bfc5ab6871a";
    //    public string name { get; set; }
    //    public Language language { get; set; }
    //    public List<components_sp> components { get; set; }
    //}
    public class Language
    {
        public string policy { get; } = "deterministic";
        public string code { get; } = "en";
    }
    public class components
    {
        public string type { get; set; }
        public List<parameters> parameters { get; set; }
    }


    
    public class parameters
    {
        public string type { get; set; }
        public string text { get; set; }
        public imageheader image { get; set; }
        public videoheader video { get; set; }
    }
   
    
    public class imgparameters
    {
        public string type { get; set; }
        public string image { get; set; }
    }

    //public class imagebanner
    //{
    //    public string type { get; set; }
    //    public string image { get; set; }
    //}
    public class imageheader
    {
        public string link { get; set; }
    }
    public class videoheader
    {
        public string link { get; set; }
    }
}