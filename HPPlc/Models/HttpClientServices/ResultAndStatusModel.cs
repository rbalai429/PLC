using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HttpClientServices
{
    public class ResultAndStatusModel
    {
        public class Item
        {
            public string message
            {
                get; set;
            }
            public string status
            {
                get; set;
            }
        }

        public class Result
        {
            public int page
            {
                get; set;
            }
            public int pageSize
            {
                get; set;
            }
            public int count
            {
                get; set;
            }
            public List<Item> items
            {
                get; set;
            }
            public string requestId
            {
                get; set;
            }
            public List<object> resultMessages
            {
                get; set;
            }
        }
        public class Status
        {
            public DateTime callDateTime
            {
                get; set;
            }
            public DateTime completionDateTime
            {
                get; set;
            }
            public bool hasErrors
            {
                get; set;
            }
            public DateTime pickupDateTime
            {
                get; set;
            }
            public string requestStatus
            {
                get; set;
            }
            public string resultStatus
            {
                get; set;
            }
            public string requestId
            {
                get; set;
            }
        }

        public class RequestStatus
        {
            public Status status
            {
                get; set;
            }
            public string requestId
            {
                get; set;
            }
            public List<object> resultMessages
            {
                get; set;
            }
        }
    }
}