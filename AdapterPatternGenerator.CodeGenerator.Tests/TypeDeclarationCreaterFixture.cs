using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using FakeItEasy;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    
    public class TypeDeclarationCreaterFixture
    {
        [Test]
        public void CreateTypesCreates2InterfacesAnd2ClassesForEachtype()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            var declaredTypes = typeDeclarationCreator.CreateTypes(typeof(ExampleClass), typeMap).ToList();
            Assert.AreEqual(2, declaredTypes.Count(x => x.IsInterface));
            Assert.AreEqual(2, declaredTypes.Count(x => x.IsClass));
        }
        [Test]
        public void CreateTypesUsesRightTypeNames()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            var actualNames = typeDeclarationCreator.CreateTypes(typeof(ExampleClass), typeMap).Select(x => x.Name);
            var expectedTypeNames = new[]
            {
                "ExampleClassAdapter",
                "IExampleClassAdapter",
                "ExampleClassStaticAdapter",
                "IExampleClassStaticAdapter",
            };
            CollectionAssert.AreEquivalent(expectedTypeNames,actualNames);
        }
        [Test]
        public void AddsDefaultTypeToTypeMap()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            typeDeclarationCreator.CreateTypes(typeof(ExampleClass), typeMap).ToList();
            A.CallTo(() => typeMap.Add(typeof (ExampleClass),
                        A<CodeTypeDeclaration>.That.Matches(x => x.IsInterface && x.Name == "IExampleClassAdapter"), true));
        }
        [Test]
        public void AddsAllTypesToTypeMap()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            typeDeclarationCreator.CreateTypes(typeof(ExampleClass), typeMap).ToList();
            A.CallTo(() => typeMap.Add(typeof(ExampleClass), A<CodeTypeDeclaration>.Ignored, A<bool>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(4));
        }

        [Test]
        public void AddsMembersAddsStringPropertiesWithRightNames()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            var codeTypeDeclaration = new CodeTypeDeclaration();
            A.CallTo(() => typeMap.GetAll(typeof (ExampleClass)))
                .Returns(new List<CodeTypeDeclaration> {codeTypeDeclaration});
            typeDeclarationCreator.AddMembers(typeof(ExampleClass),typeMap);
            var expected = new[]
            {
                "ExampleProperty",
                "ExampleReadOnlyProperty",
                "ExampleWriteOnlyProperty"
            };
            var actual = codeTypeDeclaration.Members.OfType<CodeMemberProperty>().Select(x => x.Name);
            CollectionAssert.AreEquivalent(expected,actual);
        }

        [Test]
        public void AddMembersAddsPropertiesWithCorrectType()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            var codeTypeDeclaration = new CodeTypeDeclaration();
            A.CallTo(() => typeMap.GetAll(typeof(ExampleClass)))
                .Returns(new List<CodeTypeDeclaration> { codeTypeDeclaration });
            typeDeclarationCreator.AddMembers(typeof(ExampleClass), typeMap);
            Assert.IsTrue(codeTypeDeclaration.Members.OfType<CodeMemberProperty>().All(x => x.Type.BaseType == "System.String"));
        }
        [Test]
        public void AddMembersAddsPropertiesWithCorrectAccess()
        {
            var typeDeclarationCreator = new TypeDeclarationCreator();
            var typeMap = A.Fake<ITypeMap>();
            var codeTypeDeclaration = new CodeTypeDeclaration();
            A.CallTo(() => typeMap.GetAll(typeof(ExampleClass)))
                .Returns(new List<CodeTypeDeclaration> { codeTypeDeclaration });
            typeDeclarationCreator.AddMembers(typeof(ExampleClass), typeMap);
            var properties = codeTypeDeclaration.Members.OfType<CodeMemberProperty>().ToList();
            var readAndWriteProperty = properties.First(x => x.Name == "ExampleProperty");
            Assert.IsTrue(readAndWriteProperty.HasGet && readAndWriteProperty.HasSet);
            var readOnlyProperty = properties.First(x => x.Name == "ExampleReadOnlyProperty");
            Assert.IsTrue(readOnlyProperty.HasGet && !readOnlyProperty.HasSet);
            var writeOnlyProperty = properties.First(x => x.Name == "ExampleWriteOnlyProperty");
            Assert.IsTrue(!writeOnlyProperty.HasGet && writeOnlyProperty.HasSet);

            
        }

    }
}
