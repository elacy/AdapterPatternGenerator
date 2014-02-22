using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using AdapterPatternGenerator.Implementation.Common;
using Autofac;
using FakeItEasy;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class TestIoc
    {
        private readonly IContainer _container;
        public TestIoc()
        {
            var builder = Ioc.CreateBuilder(false);

            DirectoryStaticAdapter = RegisterSingleInstanceMock<IDirectoryStaticAdapter>(builder);
            StreamWriterStaticAdapter = RegisterSingleInstanceMock<IStreamWriterStaticAdapter>(builder);
            CodeDomProviderStaticAdapter = RegisterSingleInstanceMock<ICodeDomProviderStaticAdapter>(builder);
            SolutionWriter = RegisterSingleInstanceMock<ISolutionWriter>(builder);
            _container = builder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private static T RegisterSingleInstanceMock<T>(ContainerBuilder builder) where T : class
        {
            var mock = A.Fake<T>();
            builder.RegisterInstance(mock).As<T>().SingleInstance();
            return mock;
        }
        public ISolutionWriter SolutionWriter { get; set; }
        public IDirectoryStaticAdapter DirectoryStaticAdapter { get; set; }
        public IStreamWriterStaticAdapter StreamWriterStaticAdapter{ get; set; }
        public ICodeDomProviderStaticAdapter CodeDomProviderStaticAdapter { get; set; }
    }
}
