using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.HttpClientServices
{
    public class ConstantUrl
    {
        //public static string GetTokenUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.auth.marketingcloudapis.com/v2/token";
        //public static string PostRegistarationUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/data/v1/async/dataextensions/key:74D2F53A-830A-438B-8862-DA9C6C14B52F/rows";
        //public static string GetStatusByRequestId => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/data/v1/async/{requestId}/status";
        //public static string GetResultByRequestId => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/data/v1/async/{requestId}/results";

        //Generate Token
        public static string GetTokenUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.auth.marketingcloudapis.com/v2/token";

        // Add Records
        public static string PostRegistarationUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/interaction/v1/events";

        //public static string PostRegistarationUrlInviteUser => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/v1/events";
        public static string PostRegistarationUrlInviteUser => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/hub/v1/dataevents/key:0417C510-B504-42F5-8A26-8B3EBE5E1A80/rowset";

        // Edit Records Live
        public static string SFMCPostUpdationUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/hub/v1/dataevents/key:B83E9350-7EEB-4015-89EC-C3EC41BDADF3/rowset";


        // Edit Records Live
        public static string SFMCBonusSubscription => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/hub/v1/dataevents/key:4B7770D2-E643-4D56-AF28-46C9D663D0DA/rowset";

        // Edit Records Stag
        //public static string SFMCPostUpdationUrl => "https://mc0k78ns2xzyjk59r010-dfwtbvm.rest.marketingcloudapis.com/hub/v1/dataevents/key:C4477B6C-CC3B-4DC3-BFF9-B7C581D8F26F/rowset";

    }
}