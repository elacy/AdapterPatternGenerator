using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using FakeItEasy;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public abstract class BaseGeneratorTests
    {
        protected BaseGeneratorTests()
        {
            Ioc = new TestIoc();
            Results = new List<Result>();

            A.CallTo(()=>Ioc.CodeDomProviderStaticAdapter.CreateProvider(A<string>.Ignored)).ReturnsLazily((string language)=>CreateCodeDomProviderAdapter(language));

            A.CallTo(() => Ioc.StreamWriterStaticAdapter.NewUp(A<string>.Ignored)).ReturnsLazily((string path) => CreateStreamWriter(path));
        }

        private ICodeDomProviderAdapter CreateCodeDomProviderAdapter(string language)
        {
            var fake = A.Fake<ICodeDomProviderAdapter>();
            A.CallTo(()=>fake.GenerateCodeFromCompileUnit(A<CodeCompileUnit>.Ignored,A<IStreamWriterAdapter>.Ignored,A<CodeGeneratorOptions>.Ignored))
                .Invokes((CodeCompileUnit codeCompileUnit,IStreamWriterAdapter streamWriter, CodeGeneratorOptions codeGeneratorOptions)=>
                    CreateCodeCompileUnit(codeCompileUnit,streamWriter,codeGeneratorOptions,language));
            return fake;
        }

        private void CreateCodeCompileUnit(CodeCompileUnit codeCompileUnit,IStreamWriterAdapter streamWriter,CodeGeneratorOptions codeGeneratorOptions, string language)
        {
            Results.Add(new Result
            {
                CodeCompileUnit = codeCompileUnit,
                FileName = _streamWriters[streamWriter],
                CodeGeneratorOptions = codeGeneratorOptions,
                Language = language
            });
        }
        protected List<Result> Results { get; set; }

        protected IEnumerable<CodeCompileUnit> AllCodeCompileUnits
        {
            get { return Results.Select(x => x.CodeCompileUnit); }
        } 

        protected IEnumerable<CodeNamespace> AllCodeNamespaces
        {
            get { return AllCodeCompileUnits.SelectMany(x => x.Namespaces.OfType<CodeNamespace>()); }
        }
        protected IEnumerable<CodeTypeDeclaration> AllCodeTypeDeclarations
        {
            get { return AllCodeNamespaces.SelectMany(x => x.Types.OfType<CodeTypeDeclaration>()); }
        }

        private readonly Dictionary<IStreamWriterAdapter, string> _streamWriters = new Dictionary<IStreamWriterAdapter, string>();

        protected TestIoc Ioc { get; set; }

        private IStreamWriterAdapter CreateStreamWriter(string path)
        {
            var streamWriter = A.Fake<IStreamWriterAdapter>();
            _streamWriters.Add( streamWriter,path);
            return streamWriter;
        }

        protected class Result
        {
            public string Language { get; set; }
            public CodeGeneratorOptions CodeGeneratorOptions { get; set; }
            public string FileName { get; set; }
            public CodeCompileUnit CodeCompileUnit { get; set; }

        }
        
    }
}