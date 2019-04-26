using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class Logger
    {
        static readonly string LogFile = AppDomain.CurrentDomain.BaseDirectory + "TimeATT.log";

        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(LogFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                sw = new StreamWriter(LogFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + e.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }

        }

        public static void WriteLog(string LogTxt)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(LogFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + LogTxt);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                sw = new StreamWriter(LogFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + e.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }

        }
    }
}
