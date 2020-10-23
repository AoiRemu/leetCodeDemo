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
using System.Linq.Expressions;
using System.ComponentModel;

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


            {

                //LabelTarget labelBreak = Expression.Label();//中断目标  一个标识，标志某个循环或者switch之类的，用于中断或者跳过
                //ParameterExpression loopIndex = Expression.Parameter(typeof(int), "index");//for循环的 i 

                //BlockExpression block = Expression.Block(//方法块
                //    new[] { loopIndex },//局部变量参数
                //    Expression.Loop(//for
                //        Expression.IfThenElse(Expression.LessThanOrEqual(loopIndex, Expression.Constant(10)),//if(i<=10)
                //        Expression.Block(
                //            Expression.Call(null, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),Expression.Constant("Hello")),//Console.WriteLine("Hello");
                //            Expression.PostIncrementAssign(loopIndex)),//i++
                //        Expression.Break(labelBreak)//else { break;}
                //        ), labelBreak));

                //Expression<Action> expre = Expression.Lambda<Action>(block);
                //expre.Compile().Invoke();
                //JObject applyData = JObject.Parse("{\"contents\":[{\"control\":\"Textarea\",\"id\":\"item-item-1493800414708\",\"title\":[{\"text\":\"加班事由\",\"lang\":\"zh_CN\"}],\"value\":{\"text\":\"加班\",\"tips\":[],\"members\":[],\"departments\":[],\"files\":[],\"children\":[],\"stat_field\":[],\"sum_field\":[],\"related_approval\":[],\"students\":[],\"classes\":[]}},{\"control\":\"Attendance\",\"id\":\"smart-time\",\"title\":[{\"text\":\"加班\",\"lang\":\"zh_CN\"}],\"value\":{\"tips\":[],\"members\":[],\"departments\":[],\"files\":[],\"children\":[],\"stat_field\":[],\"attendance\":{\"date_range\":{\"type\":\"hour\",\"new_begin\":1596243600,\"new_end\":1596276000,\"new_duration\":32400},\"type\":5},\"sum_field\":[],\"related_approval\":[],\"students\":[],\"classes\":[]}}]}");
                //var contents = applyData.SelectToken("contents")?.Children().ToList();
                //int type = 0;
                //double begin = 0;
                //double end = 0;
                //double new_duration = 0;
                //foreach (JObject control in contents)
                //{
                //    if (control.SelectToken("control")?.Value<string>() == "Attendance")
                //    {
                //        var value = (JObject)control.SelectToken("value");
                //        var attendance = (JObject)value.SelectToken("attendance");
                //        type = attendance.SelectToken("type").Value<int>();
                //        var date_range = (JObject)attendance.SelectToken("date_range");
                //        //begin = date_range.SelectToken("new_begin").Value<double>();
                //        begin=control.SelectToken("value").SelectToken("attendance").SelectToken("date_range").SelectToken("new_begin").Value<double>();
                //        end = date_range.SelectToken("new_end").Value<double>();
                //        new_duration = date_range.SelectToken("new_duration").Value<double>();
                //    }
                //}
                //Console.WriteLine($"{type}\n{begin}\n{end}\n{new_duration}");


                string logLine = "EC12-0001EC12 2020/04/22 17:00:12 157	04/80010000	FileAudit.cpp       (  881)	ExeSvc.exe          (  0/  1276/  3424)	[FileAudit] 执行文件上传.";
                var logLineBlockArr = logLine.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                var LogTime = Convert.ToDateTime(logLineBlockArr[0].Substring(logLineBlockArr[0].IndexOf(" ") + 1, 19) + "." + logLineBlockArr[0].Substring(logLineBlockArr[0].IndexOf(" ") + 21, 3));
                var lvAndType = logLineBlockArr[1].Split('/');
                var LogLevel = Convert.ToInt32(lvAndType[0]);
                var LogType = lvAndType[1];
                var  FileNameLine = logLineBlockArr[2].Replace(" ", "");
                var ProcessName = logLineBlockArr[3].Substring(0, 39).Substring(0, 20).Trim();
                var iDs = logLineBlockArr[3].Substring(21, 17).Replace(" ", "").Split('/');
                var SessionId = Convert.ToInt32(iDs[0]);
                var ProcessId = Convert.ToInt32(iDs[1]);
                var ThreadId = Convert.ToInt32(iDs[2]);

                var message = logLineBlockArr[4];
                var Message = message;
                Console.ReadKey();
            }


            stopwatch.Stop();
            Console.WriteLine($"耗时{stopwatch.ElapsedMilliseconds}ms");
            Console.ReadKey();
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
