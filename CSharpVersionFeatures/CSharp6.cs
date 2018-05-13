using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace CSharpVersionFeatures
{
    /// <summary>
    /// 1、Static imports（引用静态类，在Using中可以指定一个静态类，然后可以在随后的代码中直接使用静态的成员）
    /// 2、Exception filters（异常过滤器）
    /// 3、Property initializers（属性初始化增强）
    /// 4、Expression bodied members（用Lambda作为函数体）
    /// 5、Null propagator（空值判断）
    /// 6、String interpolation（字符串插入）
    /// 7、nameof operator
    /// 8、Dictionary initializer（带索引的对象初始化器）
    /// </summary>
    public sealed class CSharp6
    {
        public CSharp6()
        {
            Console.WriteLine("===================C#6.0===================");

            //1、
            this.StaticImports();

            //2、
            this.ExceptionFilters();

            //3、4、
            var person = new Person();

            //5、
            this.NullPropagator();

            //6、
            this.StringInterpolation();

            //7、
            this.NameofOperator(null);

            //8、
            this.DictionaryInitializer();


        }

        private void StaticImports()
        {
            Console.WriteLine(PI);
            Console.WriteLine(Sqrt(3 * 3));
        }

        private void ExceptionFilters()
        {
            try
            {
                throw new ArgumentException("A");
            }
            catch (ArgumentException ex) when (ex.Message == "A")
            {
                Console.WriteLine("target A exception message");
            }

            //等价以前写法
            try
            {
                throw new ArgumentException("A");
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "A")
                {
                    Console.WriteLine("target A exception message");
                }
                else
                { throw; }
            }
        }

        private void NullPropagator()
        {
            List<Person> personLst = null;
            Console.WriteLine(personLst?.Count);
            Console.WriteLine(personLst?[0]);
        }

        private void StringInterpolation()
        {
            var name = "Xiao Ming";
            Console.WriteLine(string.Format("Hello, {0}", name));
            Console.WriteLine($"Hello, {name}");
        }

        private void NameofOperator(Person person)
        {
            try
            {
                if (person == null)
                {
                    throw new ArgumentNullException(nameof(person));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DictionaryInitializer()
        {
            //老写法
            var dict1 = new Dictionary<int, Person>()
            {
                { 1,new Person()},
                { 2,new Person()}
            };

            //新写法
            var dict2 = new Dictionary<int, Person>()
            {
                [1] = new Person(),
                [2] = new Person()
            };
        }

        public class Person
        {
            public string Name_New { get; private set; } = "Xiao Ming";

            public string Name_Old { get; }

            public Person()
            {
                this.Name_Old = "Xiao Ming";
            }

            public void Work() => Console.WriteLine("I am working.");

            public string Say() => "Hello";
        }
    }
}
