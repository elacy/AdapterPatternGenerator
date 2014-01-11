using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeWriter
    {
        void WriteTypes(List<CodeTypeDeclaration> declaredTypes, string directoryName);
    }
}