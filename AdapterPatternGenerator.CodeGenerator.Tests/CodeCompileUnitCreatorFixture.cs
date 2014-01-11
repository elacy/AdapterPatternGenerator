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
        public void GeneratesCorrectNameSpaces()
        {
            var codeCompileUnitCreator = NewUp(new Mocks());
            var codeCompileUnits = codeCompileUnitCreator.CreateCodeCompileUnit(new[] { typeof(ExampleClass), typeof(ExampleDifferentNameSpaceClass) });
            var expectedNameSpaces = new[]
            {
                "Interfaces.AdapterPatternGenerator.Example",
                "Classes.AdapterPatternGenerator.Example",
                "Interfaces.AdapterPatternGenerator.Example.DifferentNameSpace",
                "Classes.AdapterPatternGenerator.Example.DifferentNameSpace"
            };
            CollectionAssert.AreEquivalent(expectedNameSpaces, codeCompileUnits.SelectMany(x => x.Namespaces.Cast<CodeNamespace>()).Select(x => x.Name));
        }
        [Test]
        public void GeneratesAnInterfaceCodeUnitAndAClassCodeUnit()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);
            var interfaces = new[] { new CodeTypeDeclaration { IsInterface = true }, new CodeTypeDeclaration { IsInterface = true } };
            var classes = new[] { new CodeTypeDeclaration(), new CodeTypeDeclaration() };
            A.CallTo(() => mocks.TypeDeclarationCreator.CreateTypes(typeof(ExampleClass))).Returns(interfaces.Union(classes));

            var codeCompileUnits = codeCompileUnitCreator.CreateCodeCompileUnit(new[] { typeof(ExampleClass) }).ToList();
            Assert.AreEqual(2,codeCompileUnits.Count());

            var namespaces = codeCompileUnits.SelectMany(x => x.Namespaces.Cast<CodeNamespace>()).ToList();
            Assert.AreEqual(2, namespaces.Count);

            var interfaceNamespace = namespaces.FirstOrDefault(x => x.Name == "Interfaces.AdapterPatternGenerator.Example");
            Assert.IsNotNull(interfaceNamespace);

            CollectionAssert.AreEquivalent(interfaces,interfaceNamespace.Types);

            var classNamespace = namespaces.FirstOrDefault(x => x.Name == "Classes.AdapterPatternGenerator.Example");
            Assert.IsNotNull(classNamespace);

            CollectionAssert.AreEquivalent(classes, classNamespace.Types);
        }
    }
}
