using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace ConsoleApp1
{
    public class WebSocketDemo
    {
        public void Show()
        {
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://127.0.0.1:8080");
            server.Start(soket =>
            {
                //连接开启的委托
                soket.OnOpen = () =>
                {
                    Console.WriteLine("open");
                    allSockets.Add(soket);
                };
                //关闭的委托
                soket.OnClose = () =>
                {
                    Console.WriteLine("close");
                    allSockets.Remove(soket);
                };
                //接收消息的委托
                soket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo:" + message));
                };
            });
            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var soket in allSockets.ToList())
                {
                    soket.Send(input);
                }
                input = Console.ReadLine();
            }
        }
    }
}
