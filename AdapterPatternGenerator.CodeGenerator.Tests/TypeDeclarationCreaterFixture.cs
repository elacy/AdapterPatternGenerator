using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    
    public class TypeDeclarationCreaterFixture
    {
        [Test]
        public void CreateTypesCreates2InterfacesAnd2ClassesForEachtype()
        {
            var types = new[] {typeof (ExampleClass), typeof (ExampleSealedClass)};
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var declaredTypes = typeDeclarationCreator.CreateTypes(types);
            Assert.AreEqual(4, declaredTypes.Count(x => x.IsInterface));
            Assert.AreEqual(4, declaredTypes.Count(x => x.IsClass));
        }
        [Test]
        public void CreateTypesUsesRightTypeNames()
        {
            var types = new[] { typeof(ExampleClass) };
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var actualNames = typeDeclarationCreator.CreateTypes(types).Select(x=>x.Name);
            var expectedTypeNames = new[]
            {
                "ExampleClassAdapter",
                "IExampleClassAdapter",
                "ExampleClassStaticAdapter",
                "IExampleClassStaticAdapter",
            };
            CollectionAssert.AreEquivalent(expectedTypeNames,actualNames);
        }
    }
}
