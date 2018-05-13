using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVersionFeatures
{
    /// <summary>
    /// 1、Out variables
    /// 2、Tuples and deconstruction（元组、元组结构）
    /// 3、Pattern matching（模式匹配）
    /// 4、Local functions（局部函数）
    /// 5、Expanded expression bodied members（）
    /// 6、Ref locals and returns（局部引用和引用返回）
    /// 7、Throw expressions（throw表达式）
    /// 8、Generalized async return types （ 扩展异步返回类型）
    /// </summary>
    public sealed class CSharp7
    {
        public CSharp7()
        {
            //1、
            this.OutVariables();

            //2、
            this.TuplesAndDeconstruction();

            //3、
            this.PatternMatching();

            //4、
            this.LocalFunctions();

            //5、7、
            var animal = new Animal();
            var indexStr = animal["index"];

            //6、
            this.RefLocalsAndReturns();

            //8、
            this.GeneralizedAsyncReturnTypes();
        }

        private void OutVariables()
        {
            // 以前我们使用out变量必须在使用前进行声明，C# 7.0 给我们提供了一种更简洁的语法 “使用时进行内联声明” 
            var inputStr = "1";

            if (int.TryParse(inputStr, out var result))
            {
                Console.WriteLine($"inputStr parse to int({result})");
            }
        }

        private void TuplesAndDeconstruction()
        {
            //元组（Tuple）在.Net 4.0 的时候就有了，但元组也有些缺点，如：
            //1）Tuple 会影响代码的可读性，因为它的属性名都是：Item1，Item2.. 。
            //2）Tuple 还不够轻量级，因为它是引用类型（Class）。

            //创建元组
            var tuple1 = (1, 2);                           // 使用语法糖创建元组
            var tuple2 = ValueTuple.Create(1, 2);         // 使用静态方法【Create】创建元组
            var tuple3 = new ValueTuple<int, int>(1, 2);  // 使用 new 运算符创建元组
            Console.WriteLine($"first：{tuple1.Item1}, second：{tuple1.Item2}, 上面三种方式都是等价的。");

            //创建给字段命名的元组
            //左边指定法
            (int one, int two) tuple4 = (1, 2);
            Console.WriteLine($"first：{tuple4.one}, second：{tuple4.two}");
            //右边指定法
            var tuple5 = (one: 1, two: 2);
            Console.WriteLine($"first：{tuple5.one}, second：{tuple5.two}");
            //左右两边同时指定字段名称 /* 此处会有警告，右边会被忽略掉 */
            (int one, int two) tuple6 = (first: 1, second: 2);
            Console.WriteLine($"first：{tuple6.one}, second：{tuple6.two}");

            //元组解构
            var (one, two) = (1, 2);
            Console.WriteLine($"first：{one}, second：{two}");
            var (three, four) = this.GetTuple();
            var (Name, Age) = new Person("Xiao Ming", 18);
            //等价于=>var (Name, Age) = new Person() { Name = "Xiao Ming", Age = 18 };
            Console.WriteLine($"name：{Name}, age：{Age}");
        }

        private void PatternMatching()
        {
            var lst = new List<object>();
            var sum = 0;

            foreach (var item in lst)
            {
                if (item is short) //C# 7 之前的 is expressions
                {
                    sum += (short)item;
                    continue;
                }

                if (item is int val) //C# 7 的 is expressions，判断为true的同时赋值变量val
                {
                    sum += val;
                    continue;
                }

                switch (item)
                {
                    case 0: break; //常量模式匹配
                    case int val2: // 类型模式匹配
                        sum += val2;
                        break;
                    case string val3 when int.TryParse(val3, out var result): // 类型模式匹配 + 条件表达式
                        sum += result;
                        break;

                    default: break;
                }
            }
        }

        private IEnumerable<int> LocalFunctions()
        {
            return GetList();

            //方法里的局部方法
            IEnumerable<int> GetList()
            {
                for (int i = 0; i < 10; i++)
                {
                    yield return i;
                }
            }
        }

        ref int GetLocalRef(int[,] arr, Func<int, bool> func)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (func(arr[i, j]))
                    {
                        return ref arr[i, j];
                    }
                }
            }

            throw new InvalidOperationException("Not found");
        }

        private void RefLocalsAndReturns()
        {
            //我们知道 C# 的 ref 和 out 关键字是对值传递的一个补充，是为了防止值类型大对象在Copy过程中损失更多的性能。
            //现在在C# 7中 ref 关键字得到了加强，它不仅可以获取值类型的引用而且还可以获取某个变量（引用类型）的局部引用。

            //寻找数组中值为20的地址，并修改对应的值为600；指针的味道
            int[,] arr = { { 10, 15 }, { 20, 25 } };
            ref var num = ref this.GetLocalRef(arr, c => c == 20);
            num = 600;
            Console.WriteLine(arr[1, 0]);
        }

        private void GeneralizedAsyncReturnTypes()
        {
            //主要是Task与ValueTask的选择问题，特定场景特定使用
            //这里不详细展开，具体可参考：https://www.cnblogs.com/chenug/p/6803649.html?utm_source=itdadao&utm_medium=referral
            async ValueTask<int> Func() {
                await Task.Delay(3000);
                return 100;
            }
        }

        private (int, int) GetTuple()
        {
            return (3, 4);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public Person()
            {

            }

            public Person(string name, int age)
            {
                this.Name = name;
                this.Age = age;
            }

            public void Deconstruct(out string name, out int age)
            {
                name = this.Name;
                age = this.Age;
            }
        }

        public class Animal
        {
            //C# 6 的时候就支持表达式体成员，但当时只支持“函数成员”和“只读属性”，
            //这一特性在C# 7中得到了扩展，它能支持更多的成员：构造函数、析构函数、带 get，set 访问器的属性、以及索引器

            public Animal() => Console.WriteLine("ctor target");

            public Animal(string name)
            {
                this._name = name ?? throw new ArgumentNullException(nameof(name));
            }

            ~Animal() => Console.WriteLine("finalized");

            //get set 属性
            private string _name;
            public string Name
            {
                get => _name ;
                set => _name = value ?? "Hehe";
            }

            //索引器
            public string this[string name] => Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
        }
    }
}
