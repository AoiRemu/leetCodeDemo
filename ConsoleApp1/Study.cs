using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Study
    {
        
    }
    #region C#图解第六章 类
    /// <summary>
    /// 索引器
    /// </summary>
    public class Employee
    {
        public string LastName;
        public string FirstName;
        public string CityBirth;

        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0: LastName = value; break;
                    case 1: FirstName = value; break;
                    case 2: CityBirth = value; break;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
            get
            {
                switch (index)
                {
                    case 0: return LastName;
                    case 1: return FirstName;
                    case 2: return CityBirth;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }
        /// <summary>
        /// 索引器重载
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int this[string name]
        {
            get
            {
                switch (name)
                {
                    case "LastName": return LastName.Length;
                    case "FirstName": return FirstName.Length;
                    case "CityBirth": return CityBirth.Length;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }
    }

    //分部类和分部方法
    public partial class MyPartClass
    {
        partial void PrintSum(int x, int y);
        public void Add(int x,int y)
        {
            PrintSum(x, y);
        }
    }
    public partial class MyPartClass
    {
        partial void PrintSum(int x, int y)
        {
            Console.WriteLine("Sum is {0}",x+y);
        }
    }
    #endregion
    #region C#图解第七章 类和继承
    public class SomeClass
    {
        public string Field1 = "some class field1";
        public void Method(string value)
        {
            Console.WriteLine("some class method {0}", value);
        }
    }
    public class OtherClass : SomeClass
    {
        new public string Field1 = "other class field2";
        new public void Method(string value)
        {
            Console.WriteLine("other class method {0}", base.Field1);
        }
    }
    #endregion
}
