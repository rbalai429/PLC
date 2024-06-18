using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.PlayVideo
{
    public class PlayVideoInput
    {
        public string CultureInfo
        {
            get; set;
        }
        public string FilterType
        {
            get; set;
        }
        public string Source
        {
            get; set;
        } = "";
    }
}