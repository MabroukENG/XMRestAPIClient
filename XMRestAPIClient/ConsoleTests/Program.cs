using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XMRestAPIClient;

namespace ConsoleTests
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v3.5");
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerResults results = provider.CompileAssemblyFromSource
            (
                new CompilerParameters(new[] { "System.dll" }),
    @"using System;
using System.Linq.Expressions;

class foo
{
    public static Expression<Func<int, int>> bar()
    {
        return (i => (i + i) * 10 / 5 % 7);
    }
}"
            );

            Expression<Func<int, int>> expression = (Expression<Func<int, int>>)results.CompiledAssembly.GetType("foo").GetMethod("bar").Invoke(null, null);
            Func<int, int> func = expression.Compile();

            Console.WriteLine("func(101) = " + func(101));

           // Console.WriteLine(expBody);
            Console.ReadKey();
        }

        private static Expression<Func<Test, bool>> gettest()
        {
            return x => (x.MyProperty > 2 || x.MyProperty * 5 != 10) && (x.MyProperty <= 17);
        }

        public async static Task<Func<T, bool>> GetPredicate<T>(string filter)
        {
            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);

            Func<T, bool> filterExpression = await CSharpScript.EvaluateAsync<Func<T, bool>>(filter, options);

            return filterExpression;
        }
        private static Expression<Func<T, bool>> FuncToExpression<T>(Func<T, bool> f)
        {
            return x => f(x);
        }


        public static string LamdaExpressionNameToSymbol(string name)
        {
            switch (name)
            {
          
               case "And": return "";
                case "AndAlso": return "";
                case "AndAssign": return "";
                case "IfThen": return "";
                case "IfThenElse": return "";
                case "IsFalse": return "";
                case "IsTrue": return "";
                case "Not": return "";
                case "NotEqual": return "";
                case "Or": return "";
                case "OrAssign": return "";
                case "OrElse": return "";
                default:return "";
            }
        }

    }

    public class Test
    {
        public int MyProperty { get; set; }
    }
}
