using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class FileHelper
    {
        /// <summary>
        /// 获得目录下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileInfo> GetAllFiles(string path)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DeepDirFile(rootDir);
            return fileList;


            void DeepDirFile(DirectoryInfo dirInfo)
            {
                var list = dirInfo.GetFiles();
                var dirInfoArr = dirInfo.GetDirectories();
                if (list.Length > 0)
                {
                    fileList.AddRange(list);
                }
                if (dirInfoArr.Length > 0)
                {
                    foreach(DirectoryInfo item in dirInfoArr)
                    {
                        DeepDirFile(item);
                    }
                }
            }
        }
        public static string ReplaceHtml(string htmlStr,string start,string value,string end, string result)
        {
            if (start == "\"")
            {
                var matchs = Regex.Matches(htmlStr, $"\\s[a-zA-Z]+=\"{value}\"");
                foreach (Match item in matchs)
                {
                    string temp = $"{item.Value.Substring(0, item.Value.IndexOf(value)-1).Insert(1,":")}\"$t('{result}')\"";
                    htmlStr = htmlStr.Replace(item.Value, temp);
                }
                htmlStr = htmlStr.Replace(start + value + end, $"$t('{result}')");
            }
            else if (start == ">")
            {
                htmlStr = htmlStr.Replace(start + value + end, $"{start}{{{{$t(\"{result}\")}}}}{end}");
            }
            else
            {
                htmlStr = htmlStr.Replace(start + value + end, $"$t('{result}')");
            }
            return htmlStr;
        }
    }
}
