using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
    public class FilterInput
    {
        public string QType
        {
            get; set;
        }
        public string Search
        {
            get; set;
        } = "";
        public DateTime? StartDate
        {
            get;set;
        }
        public DateTime? EndDate
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
        public int itemsPerPage
        {
            get; set;
        } = 0;
        public int Page
        {
            get; set;
        } = 0;
    }
}