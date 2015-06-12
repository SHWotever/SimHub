using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ACToolsUtilities
{
    public class EvalResult
    {
        public string GeneratedClass { get; set; }

        public CompilerResults CompilerResults { get; set; }

        public Delegate Method { get; set; }
    }

    public class EvalResult<T, TResult> : EvalResult
    {
        public Func<T, TResult> TypedMethod
        {
            get { return (Func<T, TResult>)Method; }
        }
    }

    public static class EvalProvider
    {
        private static Dictionary<string, object> EvalCache = new Dictionary<string, object>();

        public static object Eval(object value, string code, string[] usingStatements = null, string[] assemblies = null)
        {
            if (value == null)
            {
                value = new object();
            }

            var method = CreateEvalMethod<object, object>("var value=(global::" + value.GetType().FullName + ")arg;\r\n" + code);

            return method.TypedMethod(value);
        }

        public static EvalResult<T, TResult> EvalCode<T, TResult>(T value, string code, string[] usingStatements = null, string[] assemblies = null)
        {
            var method = CreateEvalMethod<T, TResult>("var value=(global::" + typeof(T).FullName + ")arg;\r\n" + code, usingStatements, assemblies);

            return method;
        }

        public static EvalResult<T, TResult> CreateEvalMethod<T, TResult>(string code, string[] usingStatements = null, string[] assemblies = null)
        {
            Type returnType = typeof(TResult);
            Type inputType = typeof(T);

            var includeUsings = new HashSet<string>(new[] { "System", "System.Linq", "System.Collections.Generic", "System.Text" });
            includeUsings.Add(returnType.Namespace);
            includeUsings.Add(inputType.Namespace);

            if (usingStatements != null)
                foreach (var usingStatement in usingStatements)
                    includeUsings.Add(usingStatement);

            var name = "F" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var includeAssemblies = new HashSet<string>(new[] { "system.dll", "system.core.dll", "System.Data.dll" });
            if (assemblies != null)
                foreach (var assembly in assemblies)
                    includeAssemblies.Add(assembly);

            string usings = GetUsing(includeUsings);
            string returnTypeName = "global::" + returnType.FullName;
            string inputTypeName = "global::" + inputType.FullName;

            string cacheKey = string.Concat(usings, "\r\n", returnTypeName, "\r\n", inputTypeName, "\r\n", code);
            if (!EvalCache.ContainsKey(cacheKey))
            {
                lock (EvalCache)
                {
                    if (!EvalCache.ContainsKey(cacheKey))
                    {
                        using (CSharpCodeProvider compiler = new CSharpCodeProvider())
                        {
                            var parameters = new CompilerParameters(includeAssemblies.ToArray())
                            {
                                GenerateInMemory = true
                            };

                            string source = string.Format(@"{0}
namespace {1}
{{
    public static class EvalClass
    {{
        public static {2} Eval({3} arg)
        {{
            {4}
        }}
    }}
}}", usings, name, returnTypeName, inputTypeName, string.Join("\r\n\t\t\t", code.Split(new string[] { "\r\n" }, StringSplitOptions.None)));

                            var compilerResult = compiler.CompileAssemblyFromSource(parameters, source);

                            Type type = null;
                            MethodInfo method = null;
                            Func<T, TResult> function = null;
                            if (compilerResult.Errors.Count == 0)
                            {
                                var compiledAssembly = compilerResult.CompiledAssembly;
                                type = compiledAssembly.GetType(string.Format("{0}.EvalClass", name));
                                method = type.GetMethod("Eval");
                                function = (Func<T, TResult>)Delegate.CreateDelegate(typeof(Func<T, TResult>), method);
                            }

                            var result = new EvalResult<T, TResult>
                            {
                                GeneratedClass = source,
                                CompilerResults = compilerResult,
                                Method = function,
                            };

                            EvalCache.Add(cacheKey, result);
                        }
                    }
                }
            }
            return (EvalResult<T, TResult>)EvalCache[cacheKey];
        }

        private static string GetUsing(HashSet<string> usingStatements)
        {
            StringBuilder result = new StringBuilder();
            foreach (string usingStatement in usingStatements)
            {
                result.AppendLine(string.Format("using {0};", usingStatement));
            }
            return result.ToString();
        }
    }
}