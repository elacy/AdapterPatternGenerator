using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ICodeWriter
    {
        void WriteCompileUnits(IEnumerable<CodeCompileUnit> codeCompileUnit, string directoryName);
        string GetPath(string directoryName, CodeCompileUnit codeCompileUnit, bool includeFileName);
    }
}