using System;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeDeclarationHandlerFactory
    {
        ITypeDeclarationHandler Create(Type type, string baseNamespace, bool isInterface, bool isStatic);
    }
}