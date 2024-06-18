
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Umbraco.Web.PublishedModels;


namespace HPPlc.Models.Mailer
{
    public class SenderMailer
    {
        public async Task SendOTPEmailerContent(string type, string subject, string To, IEnumerable<string> CC, IEnumerable<string> BCC, string cust_name, string OTP)
        {
            try
            {
                //IEnumerable<string> BCC = null;
                if (String.IsNullOrWhiteSpace(subject))
                    subject = "HP Print Learn Center";

                MailerContent objmailer = new MailerContent();
                string mailbody = objmailer.RegistrationOTP(type, cust_name, OTP);
                await SendMail(To, subject, mailbody, CC, BCC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    //    public void SendOTPEmailerContent(string type, string subject, string To, IEnumerable<string> CC, string cust_name, string OTP)
    //    {
    //        try
    //        {
				//IEnumerable<string> BCC = null;

				//MailerContent objmailer = new MailerContent();
    //            string mailbody = objmailer.RegistrationOTP(type, cust_name, OTP);
    //            SendMail(To, subject, mailbody,CC, BCC);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

		public async Task SendUserValidateEmailerContent(string subject, string To, IEnumerable<string> CC, string mailbody)
		{
			try
			{
				IEnumerable<string> BCC = null;

				MailerContent objmailer = new MailerContent();
				//string mailbody = objmailer.RegistrationVerifyEmail(type, cust_name, verifyurl);
				await SendMail(To, subject, mailbody, CC, BCC);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//public void SendUserValidateEmailerContent(string type, string subject, string To, string CC, string cust_name, string verifyurl)
		//{
		//    try
		//    {
		//        MailerContent objmailer = new MailerContent();
		//        string mailbody = objmailer.RegistrationVerifyEmail(type, cust_name, verifyurl);
		//        SendMail(To, CC, subject, mailbody);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }
		//}

		public bool SendPaymentEmailerContent(string subject, string To, string body, IEnumerable<string> CC, IEnumerable<string> BCC)
        {
            bool vResponse = false;
            try
            {
                MailerContent objmailer = new MailerContent();
                //string mailbody = objmailer.RegistrationOTP(type, cust_name, body);
                vResponse = SendSubscriptionMail(To, subject, body, CC,BCC);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vResponse;
        }

        public bool SendEPrintEmailContent(string subject, string To, string CC, string cust_name, string File)
        {
            try
            {
                MailerContent objmailer = new MailerContent();
                string mailbody = objmailer.EPrintEmail(cust_name);
                bool status = SendMailWithAttachment(To, CC, subject, mailbody, File);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendMail(string toemail, string Subject, string mailBody, IEnumerable<string> ccemail = null, IEnumerable<string> bccemail = null)
        {
            MailMessage mail = new MailMessage();
            bool vResponse = false;
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

            try
            {
                //List<string> bccMail = new List<string>();
				//bccMail.Add("mohitsinghdharam@gmail.com");
				//bccMail.Add("naamwar.khan@digitas.com");
				//bccMail.Add("zaferuddin.ali@digitas.com");

				mail.To.Add(toemail);

                if (ccemail != null)
                {
					foreach (var cc in ccemail)
					{
						mail.CC.Add(cc);
					}
                }
				if (bccemail != null)
				{
					foreach (string bcc in bccemail)
					{
						mail.Bcc.Add(bcc);
					}
				}
				

				mail.From = new MailAddress(fromEmail, "HP Print Learn Center");
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                mail.Body = mailBody;
                SmtpClient smtp = new SmtpClient();

                if (ConfigurationManager.AppSettings["SendEmailFromlocal"].ToString().ToLower() == "true")
                {
                    smtp.Host = "180.235.155.220"; // only for local digitas network
                }
                else
                {
					smtp.EnableSsl = true;
                    smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                    smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PORT"]);  // 25;
                    var basicCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["HostUserId"].ToString(), ConfigurationManager.AppSettings["HostPassword"].ToString());
                    smtp.Credentials = basicCredential;

                    vResponse = true;
                }
                //await smtp.SendMailAsync(mail);
                await Task.Run(() =>
                {
                    smtp.SendAsync(mail, null);
                });
            }
            catch (Exception ex)
            {
                vResponse = false;
                throw ex;
            }
            return vResponse;
        }

        public bool SendSubscriptionMail(string toemail, string Subject, string mailBody, IEnumerable<string> ccemail = null, IEnumerable<string> bccemail = null)
        {
            MailMessage mail = new MailMessage();
            bool vResponse = false;
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

            try
            {
                //List<string> bccMail = new List<string>();
                //bccMail.Add("mohitsinghdharam@gmail.com");
                //bccMail.Add("naamwar.khan@digitas.com");
                //bccMail.Add("zaferuddin.ali@digitas.com");

                mail.To.Add(toemail);

                if (ccemail != null)
                {
                    foreach (var cc in ccemail)
                    {
                        mail.CC.Add(cc);
                    }
                }
                if (bccemail != null)
                {
                    foreach (string bcc in bccemail)
                    {
                        mail.Bcc.Add(bcc);
                    }
                }


                mail.From = new MailAddress(fromEmail, "HP Print Learn Center");
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                mail.Body = mailBody;
                SmtpClient smtp = new SmtpClient();

                if (ConfigurationManager.AppSettings["SendEmailFromlocal"].ToString().ToLower() == "true")
                {
                    smtp.Host = "180.235.155.220"; // only for local digitas network
                }
                else
                {
                    smtp.EnableSsl = true;
                    smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                    smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PORT"]);  // 25;
                    var basicCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["HostUserId"].ToString(), ConfigurationManager.AppSettings["HostPassword"].ToString());
                    smtp.Credentials = basicCredential;

                    vResponse = true;
                }

                smtp.Send(mail);
                
            }
            catch (Exception ex)
            {
                vResponse = false;
                throw ex;
            }
            return vResponse;
        }

        public bool SendMailWithAttachment(string toemail, string ccemail, string Subject, string mailBody, string attachmentFile)
        {
            MailMessage mail = new MailMessage();
            bool vResponse = false;
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

            try
            {
                List<string> bccMail = new List<string>();
                //bccMail.Add("mohitsinghdharam@gmail.com");
                //bccMail.Add("naamwar.khan@digitas.com");
                //bccMail.Add("zaferuddin.ali@digitas.com");

                mail.To.Add(toemail);

                if (ccemail != "")
                {
                    mail.CC.Add(ccemail);
                }
                if (bccMail != null)
                {
                    foreach (string item in bccMail)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                mail.From = new MailAddress(fromEmail, "HP Print Learn Center");
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                mail.Body = mailBody;

                string[] temp = attachmentFile.Split('/');
                string fileName = temp[temp.Length - 1];
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(attachmentFile);
                    using (HttpWebResponse HttpWResp = (HttpWebResponse)req.GetResponse())
                    using (Stream responseStream = HttpWResp.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        Attachment attachment = new Attachment(ms, fileName, "application/pdf");
                        mail.Attachments.Add(attachment);

                        SmtpClient smtp = new SmtpClient();

                        if (ConfigurationManager.AppSettings["SendEmailFromlocal"].ToString().ToLower() == "true")
                        {
                            smtp.Host = "180.235.155.220"; // only for local digitas network
                        }
                        else
                        {
                            smtp.EnableSsl = true;
                            smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                            smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PORT"]);  // 25;
                            var basicCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["HostUserId"].ToString(), ConfigurationManager.AppSettings["HostPassword"].ToString());
                            smtp.Credentials = basicCredential;

                            vResponse = true;
                        }
                        smtp.Send(mail);
                    }
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                vResponse = false;
                throw ex;

            }
            return vResponse;
        }

        public bool SendeMailWithAttchment(MemoryStream AttchmentsStream, string FileName, string mediaType, MailContentModel mailContent)
        {
            bool vResponse = false;
            try
            {
                MailMessage mail = new MailMessage();
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

                try
                {
                    List<string> bccMail = new List<string>();
                    if (mailContent != null && mailContent.EmailBcc != null)
                    {
                        var EmailBcc = mailContent.EmailBcc as IEnumerable<string>;
                        if (EmailBcc != null)
                        {
                            foreach (var item in EmailBcc)
                            {
                                mail.Bcc.Add(item);
                            }
                        }

                    }
                    if (mailContent != null && mailContent.EmailCC != null)
                    {
                        var EmailCC = mailContent.EmailCC as IEnumerable<string>;
                        if (EmailCC != null)
                        {
                            foreach (var item in EmailCC)
                            {
                                mail.CC.Add(item);
                            }
                        }

                    }
                    mail.To.Add(mailContent.EmailTo);
                    mail.From = new MailAddress(fromEmail, "HP Print Learn Center");
                    mail.Subject = mailContent.Subject;
                    mail.IsBodyHtml = true;
                    mail.Body = mailContent?.Body?.ToString();
                    SmtpClient smtp = new SmtpClient();

                    if (ConfigurationManager.AppSettings["SendEmailFromlocal"].ToString().ToLower() == "true")
                    {
                        smtp.Host = "180.235.155.220"; // only for local digitas network
                    }
                    else
                    {
                        smtp.EnableSsl = true;
                        smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                        smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PORT"]);  // 25;
                        var basicCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["HostUserId"].ToString(), ConfigurationManager.AppSettings["HostPassword"].ToString());
                        smtp.Credentials = basicCredential;

                        vResponse = true;
                    }

                    Attachment attachment = new Attachment(AttchmentsStream, FileName, mediaType);
                    mail.Attachments.Add(attachment);
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    vResponse = false;
                    throw ex;

                }
                return vResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vResponse;
        }
        public bool SendeMailUserAndAdmin(string mailBody, MailContentModel mailContent)
        {
            bool vResponse = false;
            try
            {
                MailMessage mail = new MailMessage();
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

                try
                {
                    List<string> bccMail = new List<string>();
                    if (mailContent != null && mailContent.EmailBcc != null)
                    {
                        var EmailBcc = mailContent.EmailBcc as IEnumerable<string>;
                        if (EmailBcc != null)
                        {
                            foreach (var item in EmailBcc)
                            {
                                mail.Bcc.Add(item);
                            }
                        }

                    }
                    if (mailContent != null && mailContent.EmailCC != null)
                    {
                        var EmailCC = mailContent.EmailCC as IEnumerable<string>;
                        if (EmailCC != null)
                        {
                            foreach (var item in EmailCC)
                            {
                                mail.CC.Add(item);
                            }
                        }

                    }
                    mail.To.Add(mailContent.EmailTo);
                    mail.From = new MailAddress(fromEmail, "HP Print Learn Center");
                    mail.Subject = mailContent.Subject;
                    mail.IsBodyHtml = true;
                    mail.Body = mailBody;
                    SmtpClient smtp = new SmtpClient();

                    if (ConfigurationManager.AppSettings["SendEmailFromlocal"].ToString().ToLower() == "true")
                    {
                        smtp.Host = "180.235.155.220"; // only for local digitas network
                    }
                    else
                    {
                        smtp.EnableSsl = true;
                        smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                        smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PORT"]);  // 25;
                        var basicCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["HostUserId"].ToString(), ConfigurationManager.AppSettings["HostPassword"].ToString());
                        smtp.Credentials = basicCredential;

                        vResponse = true;
                    }
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    vResponse = false;
                    throw ex;

                }
                return vResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vResponse;
        }

    }
}