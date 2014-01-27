using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterClasses.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterClasses.System.IO;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using AdapterPatternGenerator.CodeGenerator;
using Autofac;
using Autofac.Core;

namespace AdapterPatternGenerator.Implementation.Common
{
    public static class Ioc
    {
        public static ContainerBuilder CreateBuilder(bool includeAdapters = true)
        {
            var builder = new ContainerBuilder();
            RegisterInternalClasses(builder);
            if (includeAdapters)
            {
                RegisterAdapters(builder);
            }
            return builder;
        }

        private static void RegisterInternalClasses(ContainerBuilder builder)
        {
            builder.RegisterType<TypeDeclarationHandlerFactory>().As<ITypeDeclarationHandlerFactory>();
            builder.RegisterType<CodeWriter>().As<ICodeWriter>();
            builder.RegisterType<CodeCompileUnitCreator>().As<ICodeCompileUnitCreator>();
            builder.RegisterType<Generator>().As<IGenerator>();
            builder.RegisterType<BaseAdapterCreator>().As<IBaseAdapterCreator>();
            builder.RegisterType<TypeMap>().As<ITypeMap>();
        }

        private static void RegisterAdapters(ContainerBuilder builder)
        {
            builder.RegisterType<DirectoryStaticAdapter>().As<IDirectoryStaticAdapter>();
            builder.RegisterType<StreamWriterStaticAdapter>().As<IStreamWriterStaticAdapter>();
            builder.RegisterType<CodeDomProviderStaticAdapter>().As<ICodeDomProviderStaticAdapter>();
        }
    }
}
