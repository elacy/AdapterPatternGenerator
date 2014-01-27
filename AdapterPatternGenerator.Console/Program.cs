using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterClasses;
using AdapterPatternGenerator.AdapterClasses.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterClasses.System.IO;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using AdapterPatternGenerator.CodeGenerator;
using AdapterPatternGenerator.Example;
using Autofac;
using Autofac.Core;

namespace AdapterPatternGenerator.Console
{
    class Program
    {
        private const string CodeGenDirectory = @"c:\AdapterPatternGeneratorCodeGenDir";
        private const string BaseNameSpace = @"AdapterPatternGenerator";
        static void Main(string[] args)
        {
            if (Directory.Exists(CodeGenDirectory))
            {
                Directory.Delete(CodeGenDirectory, true);
            }
            Directory.CreateDirectory(CodeGenDirectory);
            var ioc = new ConsoleIoc();
            var generator = ioc.Resolve<IGenerator>();
            var types = Assembly.GetAssembly(typeof (ExampleClass)).ExportedTypes.ToList();
            generator.GenerateCode(types, CodeGenDirectory, BaseNameSpace);
        }
    }
}
