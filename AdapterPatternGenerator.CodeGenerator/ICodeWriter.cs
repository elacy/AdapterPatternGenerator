using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ICodeWriter
    {
        void WriteCompileUnits(IEnumerable<CodeCompileUnit> codeCompileUnit, string directoryName);
    }
}