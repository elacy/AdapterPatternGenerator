using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler
{
    public interface ICodeDomProviderAdapter 
    {
        void GenerateCodeFromCompileUnit(CodeCompileUnit codeCompileUnit, IStreamWriterAdapter streamWriterAdapter,  CodeGeneratorOptions codeGeneratorOptions);
    }
}
