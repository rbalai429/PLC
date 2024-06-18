using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Videos
{
    public class VideosInput
    {
        public string CultureInfo
        {
            get; set;
        }
        public string CurrentNode
        {
            get; set;
        }
       
        public string FilterType
        {
            get; set;
        }
        public string FilterId
        {
            get; set;
        }
        public int DisplayCount
        {
            get; set;
        }
        public string selectedAgeGroup
        {
            get; set;
        }
        public string selectedVolume
        {
            get; set;
        }
        public string selectedCategory
        {
            get; set;
        }
        public string Source
        {
            get; set;
        } = "";
    }
   
}