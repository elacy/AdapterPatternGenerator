using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using AdapterPatternGenerator.Example.DifferentNameSpace;
using FakeItEasy;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class CodeCompileUnitCreatorFixture
    {
        private class Mocks
        {
            public Mocks()
            {
                
            }
            public readonly ITypeDeclarationCreator TypeDeclarationCreator = A.Fake<ITypeDeclarationCreator>();
        }

        private CodeCompileUnitCreator NewUp(Mocks mocks)
        {
            return new CodeCompileUnitCreator(mocks.TypeDeclarationCreator);
        }

        [Test]
        public void AddsEachNamespace()
        {
            var codeCompileUnitCreator = NewUp(new Mocks());
            var codeCompileUnit = codeCompileUnitCreator.CreateCodeCompileUnit(new[] { typeof(ExampleClass), typeof(ExampleDifferentNameSpaceClass) });
            var expectedNameSpaces = new[] {"AdapterPatternGenerator.Example", "AdapterPatternGenerator.Example.DifferentNameSpace"};
            CollectionAssert.AreEquivalent(expectedNameSpaces,codeCompileUnit.Namespaces.Cast<CodeNamespace>().Select(x=>x.Name));
        }
        [Test]
        public void GetsDeclaredTypesForExampleClassAndAddsThemToNamespace()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);
            var codeTypeDeclarations =new []{new CodeTypeDeclaration(),new CodeTypeDeclaration(),new CodeTypeDeclaration()} ;
            A.CallTo(() => mocks.TypeDeclarationCreator.CreateTypes(typeof (ExampleClass))).Returns(codeTypeDeclarations);

            var codeCompileUnit = codeCompileUnitCreator.CreateCodeCompileUnit(new[] { typeof(ExampleClass) });
            var nameSpace = codeCompileUnit.Namespaces.Cast<CodeNamespace>().Single(x => x.Name == "AdapterPatternGenerator.Example");

            CollectionAssert.AreEquivalent(codeTypeDeclarations,nameSpace.Types);
        }
    }
}
