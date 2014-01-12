using System;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeDeclarationHandlerFactory
    {
        ITypeDeclarationHandler Create(Type type, bool isInterface, bool isStatic);
    }
}