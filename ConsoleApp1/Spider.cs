using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace ConsoleApp1
{
    public class Spider
    {
        string url = "https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&ct=201326592&is=&fp=result&queryWord=%E9%BB%91%E7%99%BD%E5%8A%A8%E6%BC%AB%E5%9B%BE&cl=2&lm=-1&ie=utf-8&oe=utf-8&adpicid=&st=&z=&ic=&hd=&latest=&copyright=&word=%E9%BB%91%E7%99%BD%E5%8A%A8%E6%BC%AB%E5%9B%BE&s=&se=&tab=&width=&height=&face=&istype=&qc=&nc=&fr=&expermode=&force=&pn=90&rn=30&gsm=&1575420928687=";
        string path = "E:\\blackImg\\20191204\\";
        string pixivUrl = "https://www.pixiv.net/ranking.php?mode=male&p=2&format=json";
        string pixivPath = "E:\\pixivImg\\20191204\\";
        #region P站爬虫
        public static void DownloadPixivImg(string path,string url)
        {
            HttpWebRequest request;
            try
            {
                //登录
                LoginPixiv("1060236893@qq.com","1060236893");
                //获取json数据
                WebClient client = new WebClient();
                string jsonStr = client.DownloadString(new Uri(url));
                Newtonsoft.Json.Linq.JObject json = JObject.Parse(jsonStr);
                JArray jsonData = (JArray)json["url"];
                foreach (var item in jsonData)
                {
                    string imgUrl = item.ToString();
                    Console.WriteLine(imgUrl);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
            //request = WebRequest.Create(url) as HttpWebRequest;
            //request.UserAgent = GetUA();
            //request.Proxy = proxyList[new Random().Next(proxyList.Count)];
        }
        private static void LoginPixiv(string account,string pwd)
        {
            string loginUrl = "https://accounts.pixiv.net/login?lang=zh";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.UserAgent = GetUA();
            request.Referer = "https://www.pixiv.net/member_illust.php?mode=medium&illust_id=64503210";
            //获取post_key
            WebClient webClient = new WebClient();
            string loginHtmlStr = webClient.DownloadString(loginUrl);
            var match = new Regex("<input type=\"hidden\" name=\"post_key\" value=\".*?\">").Match(loginHtmlStr);
            string postKey = match.Value;
            postKey = postKey.Substring(postKey.IndexOf("value="));
            postKey = postKey.Substring(postKey.IndexOf("\""), postKey.Length - 1).Replace("\"","");
            request.Headers.Add("pixiv_id", account);
            request.Headers.Add("password", pwd);
            request.Headers.Add("return_to", "https://www.pixiv.net/");
            request.Headers.Add("post_key", postKey);
            request.Method = "POST";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            CookieContainer cok = request.CookieContainer;
            foreach(Cookie item in response.Cookies)
            {
                cok.Add(item);
            }
        }
        #endregion
        #region 百度爬虫
        /// <summary>
        /// 通过acjson返回的文件数据爬取图片
        /// </summary>
        public static async Task DownloadBaiduImgByJson(string word, int beginPage, int endPage, string path, int type,List<WebProxy> proxyList=null)
        {
            HttpClient client = new HttpClient();
            //创建目录
            Directory.CreateDirectory(path);
            int no = 1;
            Stopwatch allWatch = new Stopwatch();
            allWatch.Start();
            Console.WriteLine("开始下载");
            Console.WriteLine("------------------------------------");
            for (int i = beginPage; i < endPage; i++)
            {
                //拼接url
                string tempUrl = GetJsonUrl(word, i*30);
                var str = await client.GetStringAsync(tempUrl);
                Newtonsoft.Json.Linq.JObject json = JObject.Parse(str);
                JArray jsonData = (JArray)json["data"];
                for (int j = 0; j < jsonData.Count; j++)
                {
                    if (jsonData[j].Count() == 0)
                    {
                        break;
                    }
                    JToken relaceJson = jsonData[j]["replaceUrl"];
                    string imgUrl = relaceJson[0]["ObjURL"].ToString();
                    //if (relaceJson.Count() == 2)
                    //{
                    //    imgUrl=relaceJson[1]["ObjURL"].ToString();
                    //}
                    if (type == 1)
                    {
                        if (jsonData[j]["thumbURL"] == null)
                        {
                            Console.WriteLine("下载失败：url不存在");
                            continue;
                        }
                        imgUrl = jsonData[j]["thumbURL"].ToString();
                    }
                    //string imgUrl = jsonData[j]["replaceUrl"][0]["ObjURL"].ToString();
                    string fileName = path + (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1))).TotalMilliseconds + ".jpg";
                    try
                    {
                        HttpWebRequest request = WebRequest.Create(imgUrl) as HttpWebRequest;
                        request.UserAgent = GetUA();
                        request.KeepAlive = false;
                        request.Timeout = 5000;
                        request.Method = "GET";
                        request.Accept = "image/webp,image/apng,image/*,*/*;q=0.8";
                        request.Referer = jsonData[j]["replaceUrl"][0]["FromURL"].ToString();
                        request.Host = imgUrl.Split('/')[2];
                        //request.Host = jsonData[j]["fromURLHost"].ToString();
                        if (proxyList != null)
                        {
                            WebProxy proxy = proxyList[new Random().Next(proxyList.Count)];
                            //WebProxy proxy = new WebProxy("117.95.198.106", 9999);
                            request.Proxy = proxy;
                            Console.WriteLine("使用代理："+proxy.Address);
                        }
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        if (response.ContentType.Contains("image"))
                        {
                            var result = await DownImageAsync(fileName, response);
                            if (result)
                            {
                                no++;
                                Console.WriteLine("下载成功：" + imgUrl + $"耗时：{allWatch.ElapsedMilliseconds}");
                            }
                            else
                            {
                                Console.WriteLine("文件下载失败");
                            }
                            //fs = new FileStream(fileName, FileMode.Create);
                            //response.GetResponseStream().CopyTo(fs);

                            //fs.Close();
                        }
                        else
                        {
                            Console.WriteLine("下载失败：文件类型不正确" + $"耗时：{allWatch.ElapsedMilliseconds}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("下载失败：" + ex.Message + $"耗时：{allWatch.ElapsedMilliseconds}");
                        //imgUrl = jsonData[j]["thumbURL"].ToString();
                        //webClient = new WebClient();
                        //webClient.DownloadFile(imgUrl, fileName);
                        //no++;
                        //Console.WriteLine("重新下载快速图片：" + imgUrl + $"耗时：{allWatch.ElapsedMilliseconds}");
                    }
                    //System.Threading.Thread.Sleep(500);
                }
            }
            allWatch.Stop();
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"下载完成,成功下载{no - 1}个文件，耗时：{allWatch.ElapsedMilliseconds}");
            System.Diagnostics.Process.Start(path);
        }
        private static async Task DownloadBaiduImg(string url, string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            Task<string> str = client.GetStringAsync(url);
            str.Wait();
            //解析数据
            var doc = new HtmlDocument();
            doc.LoadHtml(str.Result);
            //百度图片数据位于  app.setData('imgData',......);中   在app.setData('fcadData'  前面
            string dataStr = str.Result.Substring(str.Result.IndexOf("app.setData('imgData'"), str.Result.IndexOf("app.setData('fcadData'") - str.Result.IndexOf("app.setData('imgData'"));
            Regex regex = new Regex("\"objURL\":.*?,");
            var match = regex.Matches(dataStr);
            //创建目录
            Directory.CreateDirectory(path);
            FileStream fs;
            int count = 1;
            foreach (Match item in match)
            {
                //获取图片url
                string tempUrl = item.Value.Substring(item.Value.IndexOf(":") + 1).Replace("\"", "").Replace(",", "");
                Console.WriteLine(tempUrl);
                try
                {
                    byte[] imgByte = await client.GetByteArrayAsync(tempUrl);
                    string fileName = path + count + ".jpeg";
                    fs = new FileStream(fileName, FileMode.Create);
                    fs.Write(imgByte, 0, imgByte.Length);
                    count++;
                    Console.WriteLine("下载成功：" + fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("下载失败：" + ex.Message);
                }
            }
            Console.WriteLine("------------------------------------");
            Console.WriteLine("下载完成");
        }
        #endregion
        private static string GetUA()//随机获取userAgent
        {
            string[] userAgents =
            {
                 // Opera
             "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36 OPR/26.0.1656.60",
             "Opera/8.0 (Windows NT 5.1; U; en)",
            "Mozilla/5.0 (Windows NT 5.1; U; en; rv:1.8.1) Gecko/20061208 Firefox/2.0.0 Opera 9.50",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; en) Opera 9.50",
            // Firefox
           "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:34.0) Gecko/20100101 Firefox/34.0",
           "Mozilla/5.0 (X11; U; Linux x86_64; zh-CN; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
           //Safari
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.57.2 (KHTML, like Gecko) Version/5.1.7 Safari/534.57.2",
           //chrome
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36",
           "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11",
           "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
           // 360
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36",
           "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
           // 淘宝浏览器
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.11 TaoBrowser/2.0 Safari/536.11",
           // 猎豹浏览器
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.71 Safari/537.1 LBBROWSER",
           "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; LBBROWSER)",
           "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 732; .NET4.0C; .NET4.0E; LBBROWSER)",
           // QQ浏览器
           "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; QQBrowser/7.0.3698.400)",
           "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 732; .NET4.0C; .NET4.0E)",
           // sogou浏览器
           "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.84 Safari/535.11 SE 2.X MetaSr 1.0",
           "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SV1; QQDownload 732; .NET4.0C; .NET4.0E; SE 2.X MetaSr 1.0)",
           // maxthon浏览器
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Maxthon/4.4.3.4000 Chrome/30.0.1599.101 Safari/537.36",
           //UC浏览器
           "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.122 UBrowser/4.0.3214.0 Safari/537.36"
          };
            return userAgents[new Random().Next(0, userAgents.Length)];//随机给予一个代理
        }
        public static string GetUrlString(string str,Encoding encoding)
        {
            return HttpUtility.UrlEncode(str,encoding).ToUpper();
        }
        public static string GetJsonUrl(string word,int page)
        {
            return $"https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&ct=201326592&is=&fp=result&queryWord={GetUrlString(word,Encoding.UTF8)}&cl=2&lm=-1&ie=utf-8&oe=utf-8&adpicid=&st=&z=&ic=&hd=&latest=&copyright=&word={GetUrlString(word, Encoding.UTF8)}&s=&se=&tab=&width=&height=&face=&istype=&qc=&nc=&fr=&expermode=&force=&pn={page}&rn=30&gsm=&1575420928687=";
        }
        public static async Task<bool> DownImageAsync(string fileName,HttpWebResponse response)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        response.GetResponseStream().CopyTo(fs);
                        fs.Close();
                    }
                });
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
    public class SimpleCrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;//启动事件
        public event EventHandler<OnCompletedEventArgs> OnCompleted;//完成事件
        public event EventHandler<Exception> OnError;//出错事件
        public CookieContainer CookieContainer { get; set; }//定义cookie容器

        public async Task<string> Start(Uri uri, WebProxy proxy = null)
        {
            string pageSource = string.Empty;
            return await Task.Run(() =>
            {
                try
                {
                    if (this.OnStart != null)
                    {
                        this.OnStart(this, new OnStartEventArgs(uri) { });
                    }
                    Stopwatch watch = new Stopwatch();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Accept = "*/*";
                    request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
                    request.AllowAutoRedirect = false;//禁止自动跳转
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";//伪装浏览器
                    request.Timeout = 5000;
                    request.KeepAlive = true;//设置长连接
                    request.Method = "GET";//请求方式为get
                    //设置代理服务器  伪装地址
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                    }
                    request.CookieContainer = this.CookieContainer;//附加cookie容器
                    request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取请求响应
                    //将cookie加入容器，保存登录状态
                    foreach(Cookie item in response.Cookies)
                    {
                        this.CookieContainer.Add(item);
                    }
                    Stream stream = response.GetResponseStream();//获取响应流
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);//以utf-8的方式读取流
                    pageSource = reader.ReadToEnd();//获取网页源代码
                    watch.Stop();
                    int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程Id
                    long milliseconds = watch.ElapsedMilliseconds;//获取请求时间
                    //释放资源
                    reader.Close();
                    stream.Close();
                    request.Abort();
                    response.Close();
                    if (this.OnCompleted != null)
                    {
                        this.OnCompleted(this, new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource));
                    }
                }
                catch (Exception ex)
                {
                    if (this.OnError != null)
                    {
                        this.OnError(this, ex);
                    }
                }
                return pageSource;
            });
        }
    }
    public class OnStartEventArgs
    {
        public Uri Uri { get; set; }
        public OnStartEventArgs(Uri uri)
        {
            this.Uri = uri;
        }
    }
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; set; }//url地址
        public int ThreadId { get; set; }//任务线程id
        public string PageSource { get; set; }
        public long Milliseconds { get; set; }
        public OnCompletedEventArgs(Uri uri,int threadId,long milliseconds,string pageSource)
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
        }
    }
}
