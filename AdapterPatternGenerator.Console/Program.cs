using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.CodeGenerator;
using AdapterPatternGenerator.Example;
using Autofac;
using Autofac.Core;

namespace AdapterPatternGenerator.Console
{
    class Program
    {
        private const string CodeGenDirectory = @"c:\AdapterPatternGeneratorCodeGenDir";
        static void Main(string[] args)
        {
            Directory.CreateDirectory(CodeGenDirectory);
            var container = CreateContainer();
            var generator = container.Resolve<IGenerator>();
            var types = Assembly.GetAssembly(typeof (ExampleClass)).ExportedTypes;
            generator.GenerateCode(types, CodeGenDirectory);
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TypeDeclarationCreator>().As<ITypeDeclarationCreator>();
            builder.RegisterType<TypeWriter>().As<ITypeWriter>();
            builder.RegisterType<Generator>().As<IGenerator>();
            return builder.Build();
        }
    }
}
