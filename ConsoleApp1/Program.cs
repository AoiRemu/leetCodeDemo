using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing;
using System.Xml;
using System.Reflection;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LeetCodeDemo leetCode = new LeetCodeDemo();
            Stopwatch stopwatch = new Stopwatch();
            List<int> list = new List<int>();
            stopwatch.Start();
            
            stopwatch.Stop();
            Console.WriteLine($"耗时{stopwatch.ElapsedMilliseconds}ms");
            Console.ReadKey();
        }
        public static void TranslateRun()
        {
            const string SAVE_PATH = "C:\\Users\\Administrator\\Desktop\\Test\\";
            const string FILE_PATH = @"G:\github项目\DLPPLUS.Web_wait\DlpPlus.Web\clientapp\src\components\Policy";

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
                    var matchs = Regex.Matches(htmlStr, "[>\"'][\u4e00-\u9fa5]+[<\"']");
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

        //p站spider
        public static void PixivSpider()
        {
            string url = " https://i.pximg.net/c/600x600/img-master/img/2017/07/06/19/07/01/63732644_p0_master1200.jpg";
            WebClient myWebClient = new WebClient();
            myWebClient.Headers.Add("Accept: */*");
            myWebClient.Headers.Add("User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; .NET4.0E; .NET4.0C; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; SE 2.X MetaSr 1.0)");
            myWebClient.Headers.Add("Accept-Language: zh-cn");
            myWebClient.Headers.Add("Content-Type: multipart/form-data");
            myWebClient.Headers.Add("Accept-Encoding: gzip, deflate");
            myWebClient.Headers.Add("Cache-Control: no-cache");
            myWebClient.Headers.Add("Referer: https://www.pixiv.net/member_illust.php?mode=medium&illust_id=63845072");
            // myWebClient.ResponseHeaders
            myWebClient.DownloadFile(url, "E:\\feature-bt.png");
        }
        
        public static bool search()
        {
            Queue<string> que = new Queue<string>();
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("kamiko", new string[] { "kakura", "kamui" });
            dic.Add("sagata", new string[] { "gintoki", "gintoki" });
            dic.Add("okita", new string[] { "sougo", "dos" });
            dic.Add("kakura", new string[] { "sagata", "okita" });
            dic.Add("kamui", new string[] {});
            dic.Add("gintoki", new string[] {});
            dic.Add("sougo", new string[] {});
            dic.Add("dos", new string[] {});
            que.Enqueue("kamiko");
            List<string> list = new List<string>();
            while (que.Count != 0)
            {
                string temp = que.Dequeue();
                if (list.Contains(temp))
                {
                    continue;
                }
                if (temp == "gintoki")
                {
                    Console.WriteLine("mitsugata gintoki!");
                    return true;
                }
                else
                {
                    foreach(string item in dic[temp])
                    {
                        que.Enqueue(item);
                    }
                }
                list.Add(temp);
            }
            Console.WriteLine("mitsuganai ano gintokiwa");
            return false;
        }
        static void deepSearch()
        {
            //节点关系
            Dictionary<string, Dictionary<string, int>> point = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> start = new Dictionary<string, int>();
            start.Add("A", 5);
            start.Add("B", 2);
            point.Add("start", start);
            Dictionary<string, int> A = new Dictionary<string, int>();
            A.Add("C", 4);
            A.Add("D", 2);
            point.Add("A", A);
            Dictionary<string, int> B = new Dictionary<string, int>();
            B.Add("A", 8);
            B.Add("D", 7);
            point.Add("B", B);
            Dictionary<string, int> C = new Dictionary<string, int>();
            C.Add("D", 6);
            C.Add("end", 3);
            point.Add("C", C);
            Dictionary<string, int> D = new Dictionary<string, int>();
            D.Add("end", 1);
            point.Add("D", D);
            point.Add("end", new Dictionary<string, int>());

            //节点图权重
            Dictionary<string, int> map = new Dictionary<string, int>();
            map.Add("A", 5);
            map.Add("B", 2);
            map.Add("C", int.MaxValue);
            map.Add("D", int.MaxValue);
            map.Add("end", int.MaxValue);

            //路径
            Dictionary<string, string> parent = new Dictionary<string, string>();
            parent.Add("A", "start");
            parent.Add("B", "start");
            parent.Add("C", "");
            parent.Add("D", "");
            parent.Add("end", "");

            //已处理名单
            List<string> list = new List<string>() { "start"};

            //找到当前权重图中最低开销的节点
            string lower = getLower(map);
            //有未检查过的节点 则循环
            while (!string.IsNullOrEmpty(lower))
            {
                //当前权重最低开销
                int num = map[lower];
                //当前节点的下一级节点集合
                Dictionary<string, int> nei = point[lower];
                //遍历下级节点集合
                foreach (string key in nei.Keys)
                {
                    //获取下级节点新开销
                    int newNum = num + nei[key];
                    //如果开销总和小于下级节点开销
                    if (map[key] > newNum)
                    {
                        //更新开销
                        map[key] = newNum;
                        //更新父子关系
                        parent[key] = lower;
                    }
                }
                //加入已检查名单
                list.Add(lower);
                //重新获取最低开销
                lower = getLower(map);
            }
            string str = "=>end";
            string pa = "end";
            for(int i = 0; i < parent.Keys.Count; i++)
            {
                str = parent[pa]+ str;
                if (parent[pa] == "start")
                {
                    break;
                }
                str = "=>"+str;
                pa = parent[pa];
            }
            Console.WriteLine($"路径为：{str}");

            string getLower(Dictionary<string,int> dic)
            {
                string item = "";
                int num = int.MaxValue;
                foreach(string key in dic.Keys)
                {
                    //如果未检查过
                    if (!list.Contains(key))
                    {
                        if (dic[key] < num)
                        {
                            item = key;
                            num = dic[key];
                        }
                    }
                }
                return item;
            }
            
        }
    }
}
