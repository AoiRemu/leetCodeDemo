using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public class JsonHelper
    {
        /// <summary>
        /// 将json的键值转化为字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string,string> GetDicByJson(JToken json)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DeepJson(json);
            return dic;
            void DeepJson(JToken json)
            {
                var children = json.Children();
                if (children.Count() > 0)
                {
                    foreach(var item in children)
                    {
                        DeepJson(item);
                    }
                }
                else
                {
                    try
                    {
                        if (!dic.ContainsKey(json.Path))
                        {
                            //value 可能取不到值，会报错
                            dic.Add(json.Path, json.Value<string>());
                        }
                    }
                    catch
                    {
                       
                    }
                    
                }
                
            }
        }
        
        /// <summary>
        /// 通过文件全路径返回一个JToken对象
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static JToken GetJsonByFilePath(JToken rootJson,string rootPath, string fileFullName)
        {
            var arr = fileFullName.Substring(fileFullName.IndexOf(rootPath)).Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = arr.Last();
            //去除后缀
            arr[arr.Length - 1] = fileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var tempJson = rootJson;
            foreach (var item in arr)
            {
                if (tempJson[item] == null)
                {
                    ((JObject)tempJson).Add(item,JObject.FromObject(new { }));
                }
                tempJson = tempJson.SelectToken(item);
            }
            return tempJson;
        }
    }
}
