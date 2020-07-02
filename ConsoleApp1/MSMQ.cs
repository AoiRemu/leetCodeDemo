using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace ConsoleApp1
{
    public class MSMQ
    {
        public void Show()
        {
            using(var queue = MessageQueue.Create(@"./MyNewPublicQueue"))
            {
                queue.Label = "Demo Queue";
                Console.WriteLine("创建消息队列");
                Console.WriteLine($"");
            }
        }
    }
}
