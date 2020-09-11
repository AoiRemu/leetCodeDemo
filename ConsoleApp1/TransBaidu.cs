using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    public class TransBaidu
    {
        private const string API_URL = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        private const string API_KEY = "Fkh1csY9jwWsQd9IKkKK";
        private WebClient webClient;
        public class TransResponseData
        {
            public string from { get; set; }
            public string to { get; set; }
            public List<trans_result> trans_result { get; set; }
            public int error_code { get; set; }
        }
        public class trans_result
        {
            public string src { get; set; }
            public string dst { get; set; }
        }
        public class TransRequestData
        {
            public string q { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string appid { get; set; }
            public string salt { get; set; }
            public string sign { get; set; }
        }
        public class ReturnMsg
        {
            public int Code { get; set; }
            public string Msg { get; set; }
        }
        public string GetMd5(string str)
        {
            string result = "";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");
            }
            md5 = null;
            return result;
        }
        public ReturnMsg Translate(TransRequestData model)
        {
            ReturnMsg data = new ReturnMsg();
            string url = $"{API_URL}?q={model.q}&from={model.from}&to={model.to}&appid={model.appid}&salt={model.salt}&sign={model.sign}";
            if (webClient == null)
            {
                webClient = new WebClient();
            }
            string responseStr = string.Empty;
            try
            {
                responseStr = webClient.DownloadString(url);
                var responseData = JsonConvert.DeserializeObject<TransResponseData>(responseStr);
                data.Code = responseData.error_code;
                data.Msg = responseData.trans_result?[0].dst;
            }
            catch (Exception ex)
            {
                data.Msg = $"错误信息：{ex.Message},API报文：{responseStr}";
            }
            
            return data;
        }
        public ReturnMsg Translate_ZhToEn(string q)
        {
            TransRequestData model = new TransRequestData()
            {
                from = "zh",
                to = "en",
                appid = "20200910000563005",
                salt = new Random().Next(1, 10000).ToString()
            };
            model.sign = GetMd5(model.appid + q + model.salt + API_KEY);
            model.q = q;
            return Translate(model);
        }
    }
}
