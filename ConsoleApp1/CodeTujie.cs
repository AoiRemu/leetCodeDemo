using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace ConsoleApp1
{
    public class CodeTujie
    {
        Stopwatch sw = new Stopwatch();
        public void DoRun()
        {
            const int LargeNumber = 6000000;
            sw.Start();
            Task<int> t1 = CountCharactersAsync(1, "http://bbs.mmxdm.xyz");
            Task<int> t2 = CountCharactersAsync(2, "http://www.zhihu.com");
            CountToALargeNumber(1, LargeNumber);
            CountToALargeNumber(2, LargeNumber);
            CountToALargeNumber(3, LargeNumber);
            CountToALargeNumber(4, LargeNumber);
            Console.WriteLine("萌漫乡字符数：{0}",t1.Result);
            Console.WriteLine("知乎字符数：{0}",t2.Result);
        }
        private int CountCharacters(int id,string uriString)
        {
            WebClient wc1 = new WebClient();
            Console.WriteLine("开始访问{0}  :   {1} ms",id,sw.Elapsed.TotalMilliseconds);
            string result = wc1.DownloadString(new Uri(uriString));
            Console.WriteLine("访问{0}成功：    {1} ms",id,sw.Elapsed.TotalMilliseconds);
            return result.Length;
        }
        private async Task<int> CountCharactersAsync(int id, string uriString)
        {
            WebClient wc1 = new WebClient();
            Console.WriteLine("开始访问{0}  :   {1} ms", id, sw.Elapsed.TotalMilliseconds);
            string result = await wc1.DownloadStringTaskAsync(new Uri(uriString));
            Console.WriteLine("访问{0}成功：    {1} ms", id, sw.Elapsed.TotalMilliseconds);
            return result.Length;
        }
        private void CountToALargeNumber(int id,int value)
        {
            for (long i = 0; i < value; i++) ;
            Console.WriteLine("结束循环{0}:    {1} ms",id, sw.Elapsed.TotalMilliseconds); 
        }
    }
}
