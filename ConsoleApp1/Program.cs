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

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LeetCodeDemo leetCode = new LeetCodeDemo();
            int[] nums1 = new int[] { 1 };
            int[] nums2 = new int[] { };
            leetCode.Merge(nums1, 1, nums2, 0);
           
            Console.ReadKey();
        }
        static void WriteY()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write("y");
            }
        }
        private static void Demo()
        {
            Console.Write("请输入主体：");
            string main = Console.ReadLine();
            Console.Write("请输入事件：");
            string eventStr = Console.ReadLine();
            Console.Write("请输入另一种说法：");
            string another = Console.ReadLine();
            Console.WriteLine($"{main}{eventStr}是怎么回事呢？{main}相信大家都很熟悉，但{main}{eventStr}是怎么回事呢，下面就让小编带大家一起了解吧。");
            Console.WriteLine($"{main}{eventStr}其实就是{another}，大家可能会惊讶{main}怎么会{eventStr}了呢？但事实就是这样，小编也感到非常惊讶。");
            Console.WriteLine($"这就是关于{main}{eventStr}的事情，大家有什么想法呢，欢迎在评论区告诉小编一起讨论哦！");
        }
        private static void DoSpider()
        {
            Console.WriteLine("---------百度图片爬虫------------");
            Console.WriteLine("请输入存储图片的路径：");
            string path = Console.ReadLine();
            Console.WriteLine("请输入起始爬取页码：");
            int beginPage = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入终止爬取页码：");
            int endPage = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入图片搜索关键字：");
            string word = Console.ReadLine();
            Spider.DownloadBaiduImgByJson(word, beginPage, endPage, path, 1).Wait();
        }

        private static void Spider_test(string url)
        {
            SimpleCrawler crawler = new SimpleCrawler();
            crawler.OnStart += (s, e) =>
            {
                Console.WriteLine("开始抓取:" + e.Uri.ToString());
            };
            crawler.OnError += (s, e) =>
            {
                Console.WriteLine("抓取出现错误:" + e.Message);
            };
            crawler.OnCompleted += (s, e) =>
            {
                Console.WriteLine(e.PageSource);
                Console.WriteLine("===========================");
                Console.WriteLine("抓取完成");
                Console.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                Console.WriteLine("线程：" + e.ThreadId);
                Console.WriteLine("地址：" + e.Uri.ToString());
            };
            crawler.Start(new Uri(url)).Wait();
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
