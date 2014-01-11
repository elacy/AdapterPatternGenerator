using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.AdapterClasses.System.CodeDom.Compiler
{
    public class CodeDomProviderAdapter : BaseInstanceAdapterClass<CodeDomProvider>, ICodeDomProviderAdapter
    {

        public CodeDomProviderAdapter(CodeDomProvider codeDomProvider) : base(codeDomProvider)
        {
        }

        public void GenerateCodeFromCompileUnit(CodeCompileUnit codeCompileUnit, IStreamWriterAdapter streamWriterAdapter, CodeGeneratorOptions codeGeneratorOptions)
        {
            var streamWriter = Convert<StreamWriter>(streamWriterAdapter);
            AdaptedClass.GenerateCodeFromCompileUnit(codeCompileUnit, streamWriter, codeGeneratorOptions);
        }
    }
}