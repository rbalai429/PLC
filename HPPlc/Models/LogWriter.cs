using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPPlc
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage, string msg)
        {
            LogWrite(logMessage, msg);
        }
        public void LogWrite(string logMessage, string msg)
        {
            m_exePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Configuration/Log");
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "Log.txt"))
                {
                    Log(logMessage, msg, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, string msg, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("  :{0}", msg);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
