using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class CodeWriter:ICodeWriter
    {
        private readonly ICodeDomProviderStaticAdapter _codeDomProviderStaticAdapter;
        private readonly IStreamWriterStaticAdapter _streamWriterStaticAdapter;
        private readonly IDirectoryStaticAdapter _directoryStaticAdapter;

        public CodeWriter(ICodeDomProviderStaticAdapter codeDomProviderStaticAdapter, IStreamWriterStaticAdapter streamWriterStaticAdapter, IDirectoryStaticAdapter directoryStaticAdapter)
        {
            _codeDomProviderStaticAdapter = codeDomProviderStaticAdapter;
            _streamWriterStaticAdapter = streamWriterStaticAdapter;
            _directoryStaticAdapter = directoryStaticAdapter;
        }

        public void WriteCompileUnits(IEnumerable<CodeCompileUnit> codeCompileUnits, string directoryName)
        {
            var provider = _codeDomProviderStaticAdapter.CreateProvider(Constants.GeneratedLanguage);
            var options = new CodeGeneratorOptions { BracingStyle = Constants.GeneratedBracingStyle };
            foreach (var codeCompileUnit in codeCompileUnits)
            {
                _directoryStaticAdapter.CreateDirectory(GetPath(directoryName,codeCompileUnit,false));
                using (
                    var sourceWriter = _streamWriterStaticAdapter.NewUp(GetPath(directoryName, codeCompileUnit, true)))
                {
                    provider.GenerateCodeFromCompileUnit( codeCompileUnit, sourceWriter, options);
                }
            }
        }
        private string GetPath(string directoryName, CodeCompileUnit codeCompileUnit,bool includeFileName)
        {
            var nameSpace = codeCompileUnit.Namespaces[0];
            var firstType = nameSpace.Types[0];
            var filePathParts = nameSpace.Name.Split('.').ToList();
            if (includeFileName)
            {
                filePathParts.Add(firstType.Name + ".cs");
            }
            filePathParts.Insert(0,directoryName);
            return Path.Combine(filePathParts.ToArray());
        }
        public void GenerateCSharpCode(CodeCompileUnit codeCompileUnit, string fileName)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions { BracingStyle = "C" };
            using (var sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    codeCompileUnit, sourceWriter, options);
            }
        }
    }
}
