using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WitBird.XiaoChangHe
{
    class WxApiClass
    {
        private const string rootPath = "https://api.weixin.qq.com/cgi-bin/";
        private static string token = "";
        private static DateTime tokenDate = DateTime.Now;
        public static string appid = "wx532c2c9e17ec3ac5";
        public static string secu = "2f0d77f7dfeefa89926972d9bb470e94";
        public string getToken()
        {
            return token;
        }

        public bool Logon()
        {
            string urlString = string.Format("{0}token?grant_type=client_credential&appid={1}&secret={2}", rootPath, appid, secu);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            try
            {
                string jsonString = this.getRequestObject(urlString, "GET", null, false, null);
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                token = json["access_token"];

                tokenDate = DateTime.Now.AddSeconds((int)json["expires_in"]);
                return true;
            }
            catch { }
            return false;
        }

        public object sendMessage(string message, string useropenid)
        {
            string msg = "{\"touser\": \"" + useropenid + "\",\"msgtype\": \"text\", \"text\": {\"content\": \"" + message + "\"}";
            string urlString = string.Format("{0}message/custom/send?access_token={1}", rootPath, token);
            string poststring = msg;
            string jsonString = this.getRequestObject(urlString, "POST", null, false, poststring);//.Replace("\r","").Replace("\n","").Replace("\"","'").Replace(" ","")
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public object setMenu()
        {
            string urlString = string.Format("{0}menu/create?access_token={1}", rootPath, token);
            string poststring = getPostStringFromFile();
            string jsonString = this.getRequestObject(urlString, "POST", null, false, poststring);//.Replace("\r","").Replace("\n","").Replace("\"","'").Replace(" ","")
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public object getMenu()
        {
            string urlString = string.Format("{0}menu/get?access_token={1}", rootPath, token);
            //string poststring = getPostStringFromFile();
            string jsonString = this.getRequestObject(urlString, "GET", null, false, null);//.Replace("\r","").Replace("\n","").Replace("\"","'").Replace(" ","")
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public dynamic getMember()
        {
            string urlString = string.Format("{0}user/get?access_token={1}", rootPath, token);
            //string poststring = getPostStringFromFile();
            string jsonString = this.getRequestObject(urlString, "GET", null, false, null);//.Replace("\r","").Replace("\n","").Replace("\"","'").Replace(" ","")
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public dynamic getMemberInfo(string openid)
        {
            string urlString = string.Format("{0}user/info?access_token={1}&openid={2}&lang=zh_CN", rootPath, token, openid);
            //string poststring = getPostStringFromFile();
            string jsonString = this.getRequestObject(urlString, "GET", null, false, null);//.Replace("\r","").Replace("\n","").Replace("\"","'").Replace(" ","")
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public string getPostStringFromFile()
        {
            string filename = @"menu.js";
            using (TextReader reader = File.OpenText(filename))
            {
                string text = reader.ReadToEnd();
                return text;
            }

        }

        private string getRequestObject(string urlString, string method, System.Collections.Hashtable queryStrings, bool isIncludeToken, string postBody)
        {
            if (queryStrings != null)
            {
                urlString += "?";
                foreach (string key in queryStrings.Keys)
                {
                    urlString += key + "=" + System.Web.HttpUtility.UrlEncode(queryStrings[key].ToString()) + "&";
                }
            }
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlString);
            //httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = method;
            if (isIncludeToken)
            {
                httpWebRequest.Headers.Add("token", token);
            }
            if (!string.IsNullOrEmpty(postBody))
            {
                byte[] buf = Encoding.UTF8.GetBytes(postBody);
                httpWebRequest.ContentLength = buf.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();
            }
            System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
            Stream receiveStream = httpWebResponse.GetResponseStream();
            System.IO.StreamReader streamReader = new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string jsonstring = streamReader.ReadToEnd();
            return jsonstring;
        }
    }
}
