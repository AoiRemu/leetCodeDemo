using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Test
    {
        public int Number { get; set; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="test"></param>
        public static implicit operator int(Test test)
        {
            return test.Number;
        }
        public static implicit operator Test(int x)
        {
            Test test = new Test();
            test.Number = x;
            return test;
        }
        public void TestMethod()
        {
            TransBaidu trans = new TransBaidu();

            const string codePath = "G:\\github项目\\DLPPLUS.Web_wait\\DlpPlus.Web\\clientapp\\src\\components\\Policy";
            const string SAVE_PATH = "C:\\Users\\Administrator\\Desktop\\Test";
            List<FileInfo> fileList = FileHelper.GetAllFiles(codePath);
            FileStream fs;
            foreach (var item in fileList)
            {
                fs = item.OpenRead();
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                string htmlStr = Encoding.UTF8.GetString(bytes);
                var match = Regex.Match(htmlStr, "[>\"'][\u4e00-\u9fa5]+[<\"']");

            }
        }
    }
    public class Hito<T>
    {
        public T Hitori;
        public void Hanashi()
        {
            Console.WriteLine();
        }
    }
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { this.val = x; }
    }
}
