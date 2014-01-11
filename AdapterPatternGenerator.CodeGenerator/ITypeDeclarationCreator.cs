using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeDeclarationCreator
    {
        List<CodeTypeDeclaration> CreateTypes(IEnumerable<Type> exportedTypes);
    }
}