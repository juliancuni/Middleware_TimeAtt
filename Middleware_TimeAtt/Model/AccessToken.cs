using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class AccessToken
    {
        public string id { get; set; }
        public string userId { get; set; }

        public static AccessToken Get()
        {
            string AccessTokenFile = ConfReader.Read("accesstokenfile");
            AccessToken accessToken = new AccessToken();
            try
            {
                string accessTokenStr = File.ReadAllText(AccessTokenFile);
                accessToken = JsonConvert.DeserializeObject<AccessToken>(accessTokenStr);
                return accessToken;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ERROR: AccessToken file nuk mund te lexohet: " + ex.Message.ToString().Trim());
            }

            return accessToken;
        }

        public static void Set(string Token)
        {

            string AccessTokenFile = ConfReader.Read("accesstokenfile");

            File.Delete(AccessTokenFile);
            try
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + AccessTokenFile, Token);
                Logger.WriteLog("INFO: AccessToken file u shkrua.");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ERROR: AccessToken file nuk mund te shkruhet: " + ex.Message.ToString().Trim());
            }

        }

        public static void Delete()
        {
            string AccessTokenFile = ConfReader.Read("accesstokenfile");
            File.Delete(AccessTokenFile);
        }
    }
}
