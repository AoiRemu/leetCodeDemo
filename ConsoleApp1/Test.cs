using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
