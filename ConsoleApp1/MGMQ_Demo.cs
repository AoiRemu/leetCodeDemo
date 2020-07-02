using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace ConsoleApp1
{
    public class MGMQ_Demo
    {
        public MessageQueue queue;
        public void GetQueue(string path)
        {
            if (MessageQueue.Exists(path))
            {
                queue = new MessageQueue(path);
            }
            else
            {
                queue = MessageQueue.Create(path);
            }
        }
        public void Receive(MessageQueue queue)
        {
            //接收字符串消息
            Message msgReceive = queue.Receive();
            msgReceive.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            string str = msgReceive.Body.ToString();
            Console.WriteLine(str);
        }
        public void SendMsg(MessageQueue queue,string msgStr)
        {
            //发送消息
            Message msg = new Message();
            msg.Body = msgStr;
            msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            queue.Send(msg);
        }

    }
}
