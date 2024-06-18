using HPPlc.Models.FAQ;
using HPPlc.Models.Mailer;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Umbraco.Web;
using Timer = System.Timers.Timer;
namespace HPPlc.Models.ImportExcelFiles
{
    public class ScheduleViaStartUpFile
    {
        private IUmbracoContextFactory _context;
        public ScheduleViaStartUpFile(IUmbracoContextFactory context)
        {
            _context = context;
        }
        public void Every10MintusCall()
        {
            var curr = DateTime.Now;
            Timer scheduleTimer = new Timer();
            //scheduleTimer.Interval = 24 * 60 * 60 * 1000;//24 hours
            //scheduleTimer.Interval = 3 * 60 * 1000;//3 min
            //scheduleTimer.Interval = 5000;//5 seconds
            scheduleTimer.Interval = 10 * 60 * 1000;//10 min
            Timer t = new Timer(scheduleTimer.Interval); // 1 sec = 1000, 60 sec = 60000
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(CallFuncationEveryOne10Mintes);
            t.Start();
         

        }
        public void CallFuncationEveryOne10Mintes(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan start = new TimeSpan(19, 0, 0); //0 o'clock like 6 PM to 6:10 PM
            TimeSpan end = new TimeSpan(19, 9, 0);
            var today = DateTime.Today; //0:59:59 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {

                Responce responce = new Responce();
                try
                {

                    FAQHelper.SendEmailToFAQRequestHandler(GetSMTPForAdmin());

                }
                catch (Exception ex)
                {
                    responce.StatusCode = HttpStatusCode.InternalServerError;
                    responce.Message = ex.ToString();
                }

            }

        }
        private MailContentModel GetSMTPForAdmin()
        {
            MailContentModel mail = new MailContentModel();
            using (var cref = _context.EnsureUmbracoContext())
            {
                var cache = cref.UmbracoContext.Content;
                var node = cache.GetById(Constant.Constant.SMTFForFAQRequestHandler);
                if (node != null)
                {
                    mail.Name = node.Name;
                    mail.Subject = node.Value("subject").ToString();
                    mail.EmailTo = node.Value("emailTo").ToString();
                    mail.EmailBcc = node.Value("emailBcc") as IEnumerable<string>;
                    mail.EmailCC = node.Value("emailCC") as IEnumerable<string>;
                }
                return mail;
            }

        }

    }
}