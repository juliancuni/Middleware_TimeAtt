using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Middleware_TimeAtt
{
    class ZKTeco_TCP_Client
    {
        ZKTechoSDK sdk = new ZKTechoSDK();
        Timer _timer = new Timer();
        //Local Variables
        public string ip { get; set; } = ConfReader.Read("zktecoip");
        public string port { get; set; } = ConfReader.Read("zktecoport");
        public string commkey { get; set; } = ConfReader.Read("zktecocommkey");
        public bool isConnected { get; set; } = false;
        public string mailSubject { get; set; }
        public string mailBody { get; set; }


        public void ConnectZKTecoDevice()
        {
            _timer.Interval = 60000; // Pas 1 minute

            //Nese pajisja i pergjigjet ping request
            if (ZKTecoAlive(ip))
            {

                //Nese nuk mund te lidhesh me pajisjen. Ose kemi nje problem ne pajise, ose ip/port nuk jane ne rregull.
                if (isConnected == false && sdk.sta_ConnectTCP(ip, port, commkey) != 1)
                {
                    isConnected = false;
                    mailSubject = "ZKTeco TCP Failure";
                    mailBody = "TimeATT Middleware service deshtoi te lidhet. Kontrollo ip address/port nese jane ne rregull.\nJu lutem njoftoni Administratorin/Developerin\nKy email eshte derguar automatikisht nga TimeATT Service.";
                    Logger.WriteLog("ERROR: ZKTeco TCP Connection Failure - TimeATT Middleware service deshtoi te lidhet. Kontrollo ip address/port nese jane ne rregull.");
                    _timer.Elapsed += new ElapsedEventHandler(RestartThisService);
                    _timer.Start();
                }
            }
            //Nese pajisja nuk i pergjigjet ping request
            else
            {
                isConnected = false;
                mailSubject = "ZKTeco Device Down";
                mailBody = "ZKTeco nuk pergjigjet ping request. Kontrolloni nese pajisja eshte e ndezur dhe e lidhur ne rrjet.\nJu lutem njoftoni Administratorin/Developerin\nKy email eshte derguar automatikisht nga TimeATT Service.";
                Logger.WriteLog("ERROR: ZKTeco Device Down - ZKTeco nuk pergjigjet ping request. Kontrolloni nese pajisja eshte e ndezur dhe e lidhur ne rrjet.");
                _timer.Elapsed += new ElapsedEventHandler(RestartThisService);
                _timer.Start();
            }
            isConnected = true;
            _timer.Elapsed += new ElapsedEventHandler(CheckEveryMinute);
            _timer.Start();
        }


        #region Ping ZKTeco

        public void CheckEveryMinute(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Checked");
            ConnectZKTecoDevice();
        }

        public void RestartThisService(object source, ElapsedEventArgs e)
        {
            Logger.WriteLog("INFO: Restarting Middlware Service...");
            SendMail.Send(mailSubject, mailBody);
            Environment.Exit(1);
        }

        public static bool ZKTecoAlive(string host)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "01001010010101010010101010010101";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(host, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                return true;
            } else
            {
                return false;
            }
        }
        #endregion


        public void HapFingerSkan(string attID, string fingerIndex, string cbFlag)
        {
            int ret = sdk.sta_OnlineEnroll(attID, fingerIndex, cbFlag);
        }

        public void RegNewUser(string attID, string emerIPlote, string privilegj, string password)
        {
            sdk.sta_SetUserInfo(attID, emerIPlote, privilegj, password);
        }

        public void DelUser(int machineId, string attId)
        {
            bool ret = sdk.axCZKEM1.SSR_DeleteEnrollData(machineId, attId, 12);
        }

        public void DisconnectZKTecoDevice()
        {
            sdk.sta_DisConnect();
        }


        public bool DergoKomandeNeZKT(string komanda)
        {
            return true;
        }
    }
}
