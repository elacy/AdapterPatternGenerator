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

            public readonly IBaseAdapterCreator BaseAdapterCreator = A.Fake<IBaseAdapterCreator>();
            public readonly ITypeMap TypeMap = A.Fake<ITypeMap>();
            public readonly ITypeDeclarationHandlerFactory TypeDeclarationHandlerFactory = A.Fake<ITypeDeclarationHandlerFactory>();
        }

        private CodeCompileUnitCreator NewUp(Mocks mocks)
        {
            return new CodeCompileUnitCreator(mocks.TypeDeclarationHandlerFactory, mocks.TypeMap, mocks.BaseAdapterCreator);
        }

        private const string BaseNameSpace = "BaseNameSpace";
        private const string BaseClassNameSpace = BaseNameSpace + "." + Constants.ClassesNamespace;
        private const string BaseInterfaceNameSpace = BaseNameSpace +"." + Constants.InterfacesNamespace;

        [Test]
        public void AddsBaseAdapter()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);
            var types = new List<Type> { typeof(ExampleClass), typeof(ExampleDifferentNameSpaceClass) };
            var codeCompileUnit = new CodeCompileUnit();
            A.CallTo(() => mocks.BaseAdapterCreator.CreateBaseClass(BaseNameSpace)).Returns(codeCompileUnit);
            var codeCompileUnits = codeCompileUnitCreator.CreateCodeCompileUnit(types, BaseNameSpace);
            Assert.AreSame(codeCompileUnit, codeCompileUnits.First());
        }

        [Test]
        public void GeneratesCorrectNameSpaces()
        {
            var codeCompileUnitCreator = NewUp(new Mocks());
            var types = new List<Type> { typeof(ExampleClass), typeof(ExampleDifferentNameSpaceClass) };
            var codeCompileUnits = codeCompileUnitCreator.CreateCodeCompileUnit(types, BaseNameSpace);
            var expectedNameSpaces = new[]
            {
                BaseInterfaceNameSpace+".AdapterPatternGenerator.Example",
                BaseClassNameSpace+".AdapterPatternGenerator.Example",
                BaseInterfaceNameSpace+".AdapterPatternGenerator.Example.DifferentNameSpace",
                BaseClassNameSpace+".AdapterPatternGenerator.Example.DifferentNameSpace"
            };
            CollectionAssert.AreEquivalent(expectedNameSpaces, codeCompileUnits.SelectMany(x => x.Namespaces.Cast<CodeNamespace>()).Select(x => x.Name));
        }
        [Test]
        public void GeneratesAnInterfaceCodeUnitAndAClassCodeUnit()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);

            var interFaceTypeHandler = A.Fake<ITypeDeclarationHandler>();
            A.CallTo(() => interFaceTypeHandler.IsInterface).Returns(true);
            var classTypeHandler = A.Fake<ITypeDeclarationHandler>();
            var type = typeof (ExampleClass);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, true, A<bool>.Ignored)).Returns(interFaceTypeHandler);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, false, A<bool>.Ignored)).Returns(classTypeHandler);

            var types = new List<Type> { type };
            var codeCompileUnits = codeCompileUnitCreator.CreateCodeCompileUnit(types, BaseNameSpace).ToList();
            Assert.AreEqual(3,codeCompileUnits.Count());

            var namespaces = codeCompileUnits.SelectMany(x => x.Namespaces.Cast<CodeNamespace>()).ToList();
            Assert.AreEqual(2, namespaces.Count);

            var interfaceNamespace = namespaces.FirstOrDefault(x => x.Name == BaseInterfaceNameSpace + ".AdapterPatternGenerator.Example");
            Assert.IsNotNull(interfaceNamespace);

            Assert.AreSame(interFaceTypeHandler.Declaration, interfaceNamespace.Types[0]);
            Assert.AreSame(interFaceTypeHandler.Declaration, interfaceNamespace.Types[1]);

            var classNamespace = namespaces.FirstOrDefault(x => x.Name == BaseClassNameSpace + ".AdapterPatternGenerator.Example");
            Assert.IsNotNull(classNamespace);

            Assert.AreSame(classTypeHandler.Declaration, classNamespace.Types[0]);
            Assert.AreSame(classTypeHandler.Declaration, classNamespace.Types[1]);
        }

        [Test]
        public void CallsAddMembers()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);
            var interfaces = new[] {new CodeTypeDeclaration {IsInterface = true}};

            var classTypeHandler = A.Fake<ITypeDeclarationHandler>();
            var type = typeof(ExampleClass);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, A<bool>.Ignored, A<bool>.Ignored)).Returns(classTypeHandler);
            

            var types = new List<Type> { typeof(ExampleClass) };
            codeCompileUnitCreator.CreateCodeCompileUnit(types, BaseNameSpace).ToList();

            A.CallTo(()=>classTypeHandler.AddMembers(mocks.TypeMap)).MustHaveHappened(Repeated.Exactly.Times(4));
        }

        [Test]
        public void AddsInterfaceToTypeMap()
        {
            var mocks = new Mocks();
            var codeCompileUnitCreator = NewUp(mocks);
            var interfaces = new[] { new CodeTypeDeclaration { IsInterface = true } };

            var interFaceTypeHandler = A.Fake<ITypeDeclarationHandler>();
            A.CallTo(() => interFaceTypeHandler.IsInterface).Returns(true);
            var otherTypeHandler = A.Fake<ITypeDeclarationHandler>();
            var type = typeof(ExampleClass);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, true, true)).Returns(otherTypeHandler);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, false, true)).Returns(otherTypeHandler);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, false, false)).Returns(otherTypeHandler);
            A.CallTo(() => mocks.TypeDeclarationHandlerFactory.Create(type, true, false)).Returns(interFaceTypeHandler);

            var types = new List<Type> { typeof(ExampleClass) };
            codeCompileUnitCreator.CreateCodeCompileUnit(types, BaseNameSpace).ToList();

            A.CallTo(() => mocks.TypeMap.Add(type, otherTypeHandler.Declaration, false)).MustHaveHappened(Repeated.Exactly.Times(3));
            A.CallTo(() => mocks.TypeMap.Add(type, interFaceTypeHandler.Declaration, true)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
