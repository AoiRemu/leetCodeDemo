using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Shijian
    {
        public delegate void Back();//创建委托
        public event Back TestShijian;//创建这个委托类型的事件
        //触发事件
        public void Say()
        {
            TestShijian();
        }
    }
    public class Fabu
    {
        public void Call()
        {
            Console.WriteLine("发布了");
        }
    }
}
