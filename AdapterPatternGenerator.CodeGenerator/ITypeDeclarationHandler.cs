using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeDeclarationHandler
    {
        void AddMembers(ITypeMap typeMap);

        Type Type { get;  }
        string Name { get; }
        bool IsInterface { get;  }
        bool IsStatic { get; }
        CodeTypeDeclaration Declaration { get; }
    }
}