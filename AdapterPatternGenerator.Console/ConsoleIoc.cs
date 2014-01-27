using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using AdapterPatternGenerator.Implementation.Common;
using Autofac;

namespace AdapterPatternGenerator.Console
{
    public class ConsoleIoc
    {
        private readonly IContainer _container;
        public ConsoleIoc()
        {
            var builder = Ioc.CreateBuilder();

            _container = builder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

    }
}
