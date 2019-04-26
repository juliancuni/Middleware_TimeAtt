using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class ConfReader
    {
        public static string Read(string key)
        {
            string ConfFile = AppDomain.CurrentDomain.BaseDirectory + "TimeATT.conf";

            //kontrollo nese config file exsiston
            if (File.Exists(ConfFile))
            {
                string[] lines = System.IO.File.ReadAllLines(ConfFile);
                //Kontrollo nese config file ka rreshta te shkruar
                if (lines.Length > 0)
                {
                    foreach (string line in lines)
                    {
                        string[] lineSplit = line.Split('=');
                        if (key == lineSplit[0].Trim())
                        {
                            return lineSplit[1].Trim();
                        }
                    }
                }
                else
                {
                    Logger.WriteLog("ERROR: Config file eshte bosh. S'ka asnje te dhene!");
                    System.Environment.Exit(1);
                }
            }
            else
            {
                Logger.WriteLog("ERROR: Config file nuk egziston");
                System.Environment.Exit(1);
            }
            return "";
        }
    }
}
