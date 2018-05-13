using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVersionFeatures
{
    /// <summary>
    /// 1、协变和逆变，C#4.0开始支持针对泛型接口的协变和逆变(仅针对引用类型)
    /// 2、动态绑定（Dynamic 动态类型对DuckType 的支持）
    /// 3、可选参数、命名参数
    /// </summary>
    public sealed class CSharp4
    {
        public CSharp4()
        {
            Console.WriteLine("===================C#4.0===================");

            //1、
            //注意：IList<T>不能对 T 进行协变
            IEnumerable<string> strLst = new List<string>();
            IEnumerable<object> objLst = strLst;
            IEnumerable<Manager> managerLst = new List<Manager>();
            IEnumerable<Employee> employeeLst = managerLst;

            //2、
            Console.WriteLine(Calculator.Add(1, 2));
            Console.WriteLine(Calculator.Add("a", "b"));

            dynamic calculator = new Calculator();
            calculator.Print(1);

            dynamic person = new ExpandoObject();
            person.Name = "Xiao Ming";
            person.Age = 20;
            Console.WriteLine(person.Name + ":" + person.Age);

            //3、
            Calculator.Test(1, 2);
            Calculator.Test(optionalArg: "3", arg2: 2, arg1: 1);

            Console.WriteLine("===================C#4.0===================");
        }

        public class Employee { }
        public class Manager : Employee { }

        public class Calculator
        {
            public void Print(int arg)
            {
                Console.WriteLine(string.Format("Haha -> {0}", arg));
            }

            /// <summary>
            /// duck type
            /// </summary>
            public static T Add<T>(T t1, T t2)
            {
                dynamic d1 = t1;
                dynamic d2 = t2;

                return (T)(d1 + d2);
            }

            public static void Test(int arg1, int arg2, string optionalArg = null)
            {
                Console.WriteLine(string.Format("arg1:{0} arg2:{1} optionalArg：{2}", arg1, arg2, optionalArg ?? "null"));
            }
        }
    }
}
