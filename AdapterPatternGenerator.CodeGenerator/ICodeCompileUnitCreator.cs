using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ICodeCompileUnitCreator
    {
        CodeCompileUnit CreateCodeCompileUnit(IEnumerable<Type> types);
    }
}