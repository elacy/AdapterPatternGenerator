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
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var declaredTypes = typeDeclarationCreator.CreateTypes(typeof(ExampleClass));
            Assert.AreEqual(2, declaredTypes.Count(x => x.IsInterface));
            Assert.AreEqual(2, declaredTypes.Count(x => x.IsClass));
        }
        [Test]
        public void CreateTypesUsesRightTypeNames()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var actualNames = typeDeclarationCreator.CreateTypes(typeof(ExampleClass)).Select(x => x.Name);
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
