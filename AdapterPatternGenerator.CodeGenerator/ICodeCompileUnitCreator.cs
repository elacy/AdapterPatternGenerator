using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ICodeCompileUnitCreator
    {
        IEnumerable<CodeCompileUnit> CreateCodeCompileUnit(List<Type> types, string baseNameSpace);
    }
}