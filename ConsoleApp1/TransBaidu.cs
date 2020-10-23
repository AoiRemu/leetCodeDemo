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
        //const string SAVE_PATH = "C:\\Users\\Administrator\\Desktop\\Test\\";
        //const string FILE_PATH = @"G:\github项目\DLPPLUS.Web_wait\DlpPlus.Web\clientapp\src\components\EndpointPolicyLite";
        //TransBaidu.TranslateRun(SAVE_PATH, FILE_PATH);
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
        public static void TranslateRun(string SAVE_PATH, string FILE_PATH)
        {
            //获取已有json语言库
            string jsonStr = File.ReadAllText(@"G:\github项目\DLPPLUS.Web_wait\DlpPlus.Web\clientapp\public\lang\lang.json");
            JObject resultJson = JObject.Parse(jsonStr);
            JToken cnJson = resultJson.SelectToken("CN");
            JToken enJson = resultJson.SelectToken("EN");
            //将已有语言库加入字典
            var dicZH = JsonHelper.GetDicByJson(cnJson);
            var dicEN = JsonHelper.GetDicByJson(enJson);
            //获取目录下的所有文件
            var fileList = FileHelper.GetAllFiles(FILE_PATH);
            string rootPath = FILE_PATH.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
            TransBaidu trans = new TransBaidu();
            FileStream fs;
            //遍历文件进行处理
            foreach (var item in fileList)
            {
                try
                {
                    //读取文件字符串
                    fs = item.OpenRead();
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    string htmlStr = Encoding.UTF8.GetString(bytes);
                    fs.Close();
                    fs = null;
                    //匹配大多数汉字出现的位置
                    var matchs = Regex.Matches(htmlStr, "[>\"'][\u4e00-\u9fa5]+(\\(([\u4e00-\u9fa5]+)\\))*[<\"']");
                    int num = 0;//同名键的后缀
                    foreach (Match match in matchs)
                    {
                        num++;
                        string start = match.Value.Substring(0, 1);
                        string end = match.Value.Substring(match.Value.Length - 1, 1);
                        //去除前后缀的字符，翻译
                        string value = match.Value.Substring(1, match.Value.Length - 2);
                        //如果字典中有这个值，则直接替换
                        if (dicZH.ContainsValue(value))
                        {
                            //替换时，去掉CN或者EN的前缀
                            //htmlStr = htmlStr.Replace(match.Value, $"{start}$t('{dicZH.FirstOrDefault(a => a.Value == value).Key.Substring(3)}'){end}");
                            htmlStr = FileHelper.ReplaceHtml(htmlStr, start, value, end, dicZH.FirstOrDefault(a => a.Value == value).Key.Substring(3));
                        }
                        else
                        {
                            var transData = trans.Translate_ZhToEn(value);
                            //api调用限制为1秒
                            Thread.Sleep(1000);
                            //调用成功
                            if (transData.Code == 0 || transData.Code == 52000)
                            {
                                if (!string.IsNullOrEmpty(transData.Msg))
                                {
                                    //写入jobject语言库，将文件中的汉字替换为$t("xxxx.xxx")格式
                                    var CN_TempJson = (JObject)JsonHelper.GetJsonByFilePath(cnJson, rootPath, item.FullName);
                                    var EN_TempJson = (JObject)JsonHelper.GetJsonByFilePath(enJson, rootPath, item.FullName);
                                    //处理得到json键
                                    string msgTrim = transData.Msg.Replace(" ", "").Replace("'", "");
                                    string transKey = msgTrim.Length > 8 ? msgTrim.Substring(0, 8) : msgTrim;
                                    transKey = CN_TempJson.ContainsKey(transKey) ? transKey + $"_{num}" : transKey;
                                    //写入json值
                                    CN_TempJson.Add(transKey, value);
                                    EN_TempJson.Add(transKey, transData.Msg);
                                    //写入字典
                                    var transJson = CN_TempJson.Property(transKey);
                                    dicZH.Add(transJson.Path, value);
                                    dicEN.Add(transJson.Path, transData.Msg);
                                    //替换文本
                                    //htmlStr = htmlStr.Replace(match.Value, $"{start}$t('{transJson.Path.Substring(3)}'){end}");
                                    htmlStr = FileHelper.ReplaceHtml(htmlStr, start, value, end, transJson.Path.Substring(3));
                                }
                            }
                            else
                            {
                                Console.WriteLine($"关键字[{value}]调用API失败，位置：{item.FullName}，错误信息:{transData.Msg}");
                            }
                        }

                    }
                    //此文件翻译完成，文件写入指定保存位置
                    string rootFilePath = item.FullName.Substring(item.FullName.IndexOf(rootPath));
                    //检查目录是否存在
                    string checkPath = SAVE_PATH;
                    var checkArr = rootFilePath.Split('\\');
                    for (var i = 0; i < checkArr.Length - 1; i++)
                    {
                        checkPath += ("\\" + checkArr[i]);
                        if (!Directory.Exists(checkPath))
                        {
                            Directory.CreateDirectory(checkPath);
                        }
                    }
                    File.WriteAllText(SAVE_PATH + rootFilePath, htmlStr, Encoding.UTF8);
                    Console.WriteLine($"文件翻译成功！位置:{SAVE_PATH + rootFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"程序错误！文件:{item.FullName},错误信息：{ex.Message}");
                }

            }
            //将json转化为文件
            File.WriteAllText(SAVE_PATH + @"result.json", JsonConvert.SerializeObject(resultJson));
            Console.WriteLine("所有文件翻译完成");
        }
    }
}
