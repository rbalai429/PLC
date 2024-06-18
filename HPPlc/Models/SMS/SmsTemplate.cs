using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPPlc.Models.SMS
{
    public static class SmsTemplate
    {
        static string template = string.Empty;

        public static string SendOTPSMSTmp(string otp)
        {
            template = string.Format(otp + @" is your One Time Password (OTP) to login into HP Print Learn Center. Please use this OTP within 15 minutes only.");
            return template;
        }
    }
}
