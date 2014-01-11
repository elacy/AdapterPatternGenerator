using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeWriter:ITypeWriter
    {
        private const string FileName = "file.cs";
        public void WriteTypes(CodeCompileUnit codeCompileUnit, string directoryName)
        {
            GenerateCSharpCode(codeCompileUnit,Path.Combine(directoryName,FileName));
        }
        public void GenerateCSharpCode(CodeCompileUnit codeCompileUnit, string fileName)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions {BracingStyle = "C"};
            using (var sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    codeCompileUnit, sourceWriter, options);
            }
        }
    }
}
