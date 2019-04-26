using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class AuthApi
    {
        public static string Api = ConfReader.Read("api");
        public static string LoginEndpoint = ConfReader.Read("loginendpoint");
        public static string PerdoruesEndpoint = "api/Perdoruesit/";
        public static string Username = ConfReader.Read("apiuser");
        public static string Password = ConfReader.Read("apipass");

        public static void Login()
        {
            string URI = Api + LoginEndpoint;
            Perdoruesi perdoruesi = new Perdoruesi()
            {
                username = Username,
                password = Password
            };

            string payload = JsonConvert.SerializeObject(perdoruesi);

            AccessToken accessToken = AccessToken.Get();
            if(!String.IsNullOrEmpty(accessToken.id))
            {
                if (CheckAccessToken(accessToken.userId, accessToken.id))
                {
                    
                    string ResponseLogin = HttpCallApi.Post(URI, payload);

                    var obj = JsonConvert.DeserializeObject<dynamic>(ResponseLogin);

                    if (obj["error"] != null)
                    {
                        string errCode = obj["error"]["statusCode"];
                        string errMsg = obj["error"]["message"];
                        Logger.WriteLog("ERROR: " + errCode + ". " + errMsg);
                    }
                    else
                    {
                        Logger.WriteLog("Logged in. AccessToken =  " + obj["id"]);
                        AccessToken.Set(ResponseLogin);
                    }
                }
                else
                {
                    Logger.WriteLog("Logged in. Ska nevoje per login");
                }
            }
            else
            {
                string ResponseLogin = HttpCallApi.Post(URI, payload);

                var obj = JsonConvert.DeserializeObject<dynamic>(ResponseLogin);

                if (obj["error"] != null)
                {
                    string errCode = obj["error"]["statusCode"];
                    string errMsg = obj["error"]["message"];
                    Logger.WriteLog("ERROR: " + errCode + ". " + errMsg);
                }
                else
                {
                    Logger.WriteLog("Logged in. AccessToken =  " + obj["id"]);
                    AccessToken.Set(ResponseLogin);
                }
            }
            
        }

        public static bool CheckAccessToken(string userId, string accessToken)
        {
            string URI = Api + PerdoruesEndpoint + userId + "?access_token=" + accessToken;
            string ResponseLogin = HttpCallApi.Get(URI);
            try
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(ResponseLogin);
                if (obj["error"] != null)
                {
                    string errCode = obj["error"]["statusCode"];
                    string errMsg = obj["error"]["message"];
                    Logger.WriteLog("ERROR: " + errCode + ". AccessToken Expired");
                    return true;
                }
            }
            catch (JsonReaderException jrex)
            {
                Logger.WriteLog("ERROR: Response deserialization wrong format" + jrex.Message);
            }
            return false;            
        }

        public void Logout()
        {
            string Api = ConfReader.Read("api");
            string LogoutEndpoint = ConfReader.Read("logoutendpoint");
            string accessToken = AccessToken.Get().id;
            string URI = Api + LogoutEndpoint + "?access_token=" + accessToken;

            string ResponseLogout = HttpCallApi.Post(URI, "");
            var obj = JsonConvert.DeserializeObject<dynamic>(ResponseLogout);

            if (obj != null)
            {
                AccessToken.Delete();
            }
            else
            {
                string errCode = obj["error"]["statusCode"];
                string errMsg = obj["error"]["message"];
                Logger.WriteLog("ERROR: " + errCode + ". " + errMsg);
            }
        }
    }
}
