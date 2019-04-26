using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    public class HttpCallApi
    {
        public static string Post(string URI, string Payload)
        {
            WebRequest request = WebRequest.Create(URI);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(Payload);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException wex)
            {
                response = wex.Response;
            }
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
        public static string Get(string URI)
        {
            WebRequest request = WebRequest.Create(URI);
            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;
            try
            {
                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (WebException wex)
            {
                responseFromServer = wex.Message;
            }
            return responseFromServer;
        }
    }
}
