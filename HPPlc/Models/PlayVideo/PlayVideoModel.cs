using HPPlc.Models.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.PlayVideo
{
    public class PlayVideoModel
    {
        public List<NestedItems> PlayVideoList
        {
            get; set;
        }
        public NestedItems PlayVideo
        {
            get;set;
        }
        public string VideoYouTubeId
        {
            get; set;
        }
        public SeeMore SeeMore
        {
            get; set;
        }
        public string Source
        {
            get; set;
        } = "";
    }
}