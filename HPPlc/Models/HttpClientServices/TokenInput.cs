using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HttpClientServices
{
    public class TokenInput
    {
        public string grant_type
        {
            get; set;
        } = "client_credentials";
        public string client_id
        {
            get; set;
        }
        public string client_secret
        {
            get; set;
        }
        public string account_id
        {
            get; set;
        } = "1363516";
    }
    public class TokenData
    {
        public string access_token
        {
            get; set;
        }
        public string token_type
        {
            get; set;
        }
        public int expires_in
        {
            get; set;
        }
        public string scope
        {
            get; set;
        }
        public string soap_instance_url
        {
            get; set;
        }
        public string rest_instance_url
        {
            get; set;
        }

    }
}