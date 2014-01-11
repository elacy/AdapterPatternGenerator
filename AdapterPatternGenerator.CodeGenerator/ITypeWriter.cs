using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeWriter
    {
        void WriteTypes(CodeCompileUnit codeCompileUnit, string directoryName);
    }
}