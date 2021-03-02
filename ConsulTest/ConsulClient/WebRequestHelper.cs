using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsulClients
{
    public static class WebRequestHelper
    {
        /// <summary>
        /// POST json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="JSONData"></param>
        /// <returns></returns>
        public static string PostJson(string url, string JSONData)
        {
            string result = string.Empty;
            //byte[] bs = Encoding.UTF8.GetBytes(body);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(myRequest.GetRequestStream()))
            {
                streamWriter.Write(JSONData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)myRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            httpResponse.Close();
            myRequest.Abort();
            return result;
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url">请求url(不含参数)</param>
        /// <param name="postDataStr">参数部分：roleId=1&uid=2</param>
        /// <param name="timeout">等待时长(毫秒)</param>
        /// <returns></returns>
        public static string GetHttp(string url, string postDataStr, int timeout = 2000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Timeout = timeout;//等待

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            response.Close();
            request.Abort();
            return retString;
        }
    }
}
