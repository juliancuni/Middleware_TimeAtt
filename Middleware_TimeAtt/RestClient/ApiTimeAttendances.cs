using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class ApiTimeAttendances
    {
        public static string Api = ConfReader.Read("api");
        public static string TimeATTEndpoint = ConfReader.Read("timeattendnpoint");

        public static void RegAttendance(string userAttId, int state)
        {

            AccessToken accessToken = AccessToken.Get();
            string URI = Api + TimeATTEndpoint + "?access_token=" + accessToken.id;
            string payload = "{\"userAttId\": " + userAttId + "}";
            if (!AuthApi.CheckAccessToken(accessToken.userId, accessToken.id))
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
                    string pergjigje = obj["pergjigje"];
                    Logger.WriteLog(pergjigje);
                }

            }
            else
            {
                AuthApi.Login();
                RegAttendance(userAttId, state);
            }
        }
    }
}
