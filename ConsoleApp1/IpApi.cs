using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// ip-api 获取ip地理位置
    /// </summary>
    public class IpApi
    {
        public class IpApiParameter
        {
            public string query { get; set; }
            public string fields { get; set; }
            public string lang { get; set; }
            public IpApiParameter()
            {
                this.fields = "city,country,countryCode,query";
                this.lang = "zh-CN";
            }
        }
        /// <summary>
        /// 批量获取ip地理位置
        /// </summary>
        public void Run()
        {
            HttpClient httpClien = new HttpClient();
            List<IpApiParameter> listPara = new List<IpApiParameter>()
                {
                    new IpApiParameter()
                    {
                        query="222.93.168.248"
                    },
                    new IpApiParameter()
                    {
                        query="222.93.168.248"
                    },
                };
            for (int i = 0; i < 10; i++)
            {
                var result = httpClien.PostAsync("http://ip-api.com/batch?lang=zh-CN", new StringContent(JsonConvert.SerializeObject(listPara, Newtonsoft.Json.Formatting.Indented), Encoding.UTF8, "text/plain")).Result;
                string ipJson = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(ipJson);
            }
        }
    }
}
