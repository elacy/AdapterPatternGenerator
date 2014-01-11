using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;
using FakeItEasy;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class CodeWriterFixture
    {
        private class Mocks
        {
            public Mocks()
            {
                A.CallTo(() => CodeDomProviderStaticAdapter.CreateProvider("CSharp")).Returns(CodeDomProviderAdapter);
            }
            public readonly IDirectoryStaticAdapter DirectoryStaticAdapter = A.Fake<IDirectoryStaticAdapter>();
            public readonly IStreamWriterStaticAdapter StreamWriterStaticAdapter = A.Fake<IStreamWriterStaticAdapter>();
            public readonly ICodeDomProviderStaticAdapter CodeDomProviderStaticAdapter = A.Fake<ICodeDomProviderStaticAdapter>();
            public readonly ICodeDomProviderAdapter CodeDomProviderAdapter = A.Fake<ICodeDomProviderAdapter>();
        }

        private CodeWriter NewUp(Mocks mocks)
        {
            return new CodeWriter(mocks.CodeDomProviderStaticAdapter, mocks.StreamWriterStaticAdapter, mocks.DirectoryStaticAdapter);
        }

        private CodeCompileUnit CreateCodeCompileUnit(string nameSpace, string typeName)
        {
            var codeCompileUnit = new CodeCompileUnit();
            var codeNameSpace = new CodeNamespace(nameSpace);
            codeCompileUnit.Namespaces.Add(codeNameSpace);
            var declaredType = new CodeTypeDeclaration(typeName);
            codeNameSpace.Types.Add(declaredType);
            var secondType = new CodeTypeDeclaration(typeName +"Static");
            codeNameSpace.Types.Add(secondType);
            return codeCompileUnit;
        }

        [Test]
        public void WritesCSharpToTheRightFile()
        {
            var codeCompileUnit = CreateCodeCompileUnit("Name.Space.Test", "TestType");
            var codeCompileUnits = new[] { codeCompileUnit };

            var mocks = new Mocks();

            var streamWriterAdapter = A.Fake<IStreamWriterAdapter>();
            A.CallTo(() => mocks.StreamWriterStaticAdapter.NewUp("c:\\directory\\name\\Name\\Space\\Test\\TestType.cs")).Returns(streamWriterAdapter);

            var codeWriter = NewUp(mocks);

            codeWriter.WriteCompileUnits(codeCompileUnits,"c:\\directory\\name");

            A.CallTo(
                () =>
                    mocks.CodeDomProviderAdapter.GenerateCodeFromCompileUnit(codeCompileUnit,streamWriterAdapter,
                        A<CodeGeneratorOptions>.That.Matches(x => x.BracingStyle == "C"))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void CreatesADirectoryForFile()
        {
            var codeCompileUnit = CreateCodeCompileUnit("Name.Space.Test", "TestType");
            var codeCompileUnits = new[] { codeCompileUnit };

            var mocks = new Mocks();
            var stream = A.Fake<Stream>();

            var streamWriterAdapter = A.Fake<IStreamWriterAdapter>();
            A.CallTo(() => mocks.StreamWriterStaticAdapter.NewUp("c:\\directory\\name\\Name\\Space\\Test\\TestType.cs")).Returns(streamWriterAdapter);

            var codeWriter = NewUp(mocks);
            codeWriter.WriteCompileUnits(codeCompileUnits, "c:\\directory\\name");

            A.CallTo(() => mocks.DirectoryStaticAdapter.CreateDirectory("c:\\directory\\name\\Name\\Space\\Test")).MustHaveHappened(Repeated.Exactly.Once);
            

        }

    }
}
