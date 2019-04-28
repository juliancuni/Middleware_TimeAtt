using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace Middleware_TimeAtt
{
    public class TCPServer
    {

        TcpListener server = new TcpListener(IPAddress.Parse(ConfReader.Read("tcpserverip")), int.Parse(ConfReader.Read("tcpserverport")));
        ZKTeco_TCP_Client zktTcpClient = new ZKTeco_TCP_Client();

        public void StartServer()
        {
            Logger.WriteLog("INFO: Starting Middlware Service...");
            server.Start();
            Logger.WriteLog("INFO: TCP server started");
            AuthApi.Login();
            zktTcpClient.ConnectZKTecoDevice();
            Accept_connection();

        }

        private void Accept_connection()
        {
            server.BeginAcceptTcpClient(Handle_connection, server);
        }

        private void Handle_connection(IAsyncResult result)  //the parameter is a delegate, used to communicate between threads
        {
            Accept_connection();  //once again, checking for any other incoming connections
            TcpClient client = server.EndAcceptTcpClient(result);  //creates the TcpClient
            Byte[] bytes = new Byte[1024];
            String data = null;
            NetworkStream stream = client.GetStream();

            int i = stream.Read(bytes, 0, bytes.Length);

            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            KomandaModel komanda = JsonConvert.DeserializeObject<KomandaModel>(data);

            byte[] msg;
            if (komanda.Komanda == "Skan_New_Finger")
            {

                zktTcpClient.HapFingerSkan(komanda.AttId, komanda.GishtId, "0");
                msg = System.Text.Encoding.ASCII.GetBytes("sukses");
            }
            else if (komanda.Komanda == "Reg_New_User")
            {
                zktTcpClient.RegNewUser(komanda.AttId, komanda.EmerIplote, komanda.Privilegji, komanda.Password);
                msg = System.Text.Encoding.ASCII.GetBytes("sukses");
            }
            else if (komanda.Komanda == "Del_User")
            {
                zktTcpClient.DelUser(1, komanda.AttId);
                msg = System.Text.Encoding.ASCII.GetBytes("sukses");
            }
            else
            {
                msg = System.Text.Encoding.ASCII.GetBytes("deshtim");
                stream.Write(msg, 0, msg.Length);
            }
            stream.Write(msg, 0, msg.Length);
            client.Close();

        }

        public void StopServer()
        {
            Logger.WriteLog("INFO: Middlware Service Stopped");
            //server.Stop();
        }
    }
}