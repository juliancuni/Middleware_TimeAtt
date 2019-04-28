using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class ZKTeco_TCP_Client
    {
        ZKTechoSDK sdk = new ZKTechoSDK();
        int failZKTecoCounter = 0;

        public void ConnectZKTecoDevice()
        {
            string ip = ConfReader.Read("zktecoip");
            string port = ConfReader.Read("zktecoport");
            string commkey = ConfReader.Read("zktecocommkey");

            if (sdk.sta_ConnectTCP(ip, port, commkey) != 1)
            {
                if (failZKTecoCounter >= 5)
                {
                    SendMail.Send("ZKTeco TCP Connection Failure", "Pas 6 tentativash per tu lidhur me ZKTeco Server, TimeATT service deshtoi te lidhet.\nJu lutem njoftoni Administratorin/Developerin\nKy email eshte derguar automatikisht nga TimeATT Service.");
                }
                else
                {
                    ++failZKTecoCounter;
                    ConnectZKTecoDevice();
                }
            }
            //HapFingerSkan();
        }

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
