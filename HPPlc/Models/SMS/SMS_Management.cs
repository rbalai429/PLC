using HPPlc.Models.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace HPPlc.Models
{
	public class SMS_Management
	{
		public async void SendSMS(string mob_number)
		{
			Random generator = new Random();
			int otpint = generator.Next(1, 1000000);
			string otpstr = otpint.ToString().Substring(0, 3).PadLeft(4, '0');
			SessionManagement.StoreInSession(SessionType.OtpValue, otpstr);
			string response = await SmsSendAPI.SendMessage(mob_number, SmsTemplate.SendOTPSMSTmp(otpstr));
		}
		public async Task<string> SendSMSForVerification(string mob_number,string Otp)
		{
			string ms = string.Empty;
			ms = await SmsSendAPI.SendMessage(mob_number, SmsTemplate.SendOTPSMSTmp(Otp));

			return ms;
		}
		public string GenerateOtp()
		{
			//Random generator = new Random();
			//int otpint = generator.Next(1, 1000000);
			//string otpstr = otpint.ToString().Substring(0, 3).PadLeft(4, '0');
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			string vOTP = "";
			int[] results = new int[1];
			var buffer = new byte[4];
			int min = 100000;
			int max = 999999;
			for (int i = 0; i < results.Length; i++)
			{
				while (results[i] < min || results[i] > max)
				{
					provider.GetBytes(buffer);
					results[i] = BitConverter.ToInt32(buffer, 0);
				}
				vOTP = results[i].ToString();
			}

			SessionManagement.StoreInSession(SessionType.OtpValue, vOTP);

			return vOTP;
		}

		public string Generate4Otp()
		{
			//Random generator = new Random();
			//int otpint = generator.Next(1, 1000000);
			//string otpstr = otpint.ToString().Substring(0, 3).PadLeft(4, '0');
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			string vOTP = "";
			int[] results = new int[1];
			var buffer = new byte[4];
			int min = 1000;
			int max = 9999;
			for (int i = 0; i < results.Length; i++)
			{
				while (results[i] < min || results[i] > max)
				{
					provider.GetBytes(buffer);
					results[i] = BitConverter.ToInt32(buffer, 0);
				}
				vOTP = results[i].ToString();
			}

			SessionManagement.StoreInSession(SessionType.OtpValue, vOTP);

			return vOTP;
		}
	}
}