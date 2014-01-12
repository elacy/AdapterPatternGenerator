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

    public class TypeDeclarationHandlerFixture
    {
        private string[] ExampleInstanceProperties = new[]
            {
                "ExampleProperty",
                "ExampleReadOnlyProperty",
                "ExampleWriteOnlyProperty"
            };
        private string[] ExampleStaticProperties = new[]
            {
                "StaticProperty",
                "AnotherStaticProperty"
            };

        [Test]
        public void AddsMembersAddsInstancePropertiesWithRightNamesToInterface()
        {
            var codeTypeDeclaration = new CodeTypeDeclaration("IExampleClassAdapter") {IsInterface = true};
            var handler = new TypeDeclarationHandler(typeof(ExampleClass), codeTypeDeclaration, false);
            var typeMap = A.Fake<ITypeMap>();
            handler.AddMembers(typeMap);
            var expected = ExampleInstanceProperties;
            var actual = codeTypeDeclaration.Members.OfType<CodeMemberProperty>().Select(x => x.Name);
            CollectionAssert.AreEquivalent(expected, actual);
        }
        [Test]
        public void AddsMembersAddsStaticPropertiesWithRightNamesToStaticInterface()
        {
            var codeTypeDeclaration = new CodeTypeDeclaration("IExampleClassStaticAdapter") { IsInterface = true };
            var handler = new TypeDeclarationHandler(typeof(ExampleClass), codeTypeDeclaration, true);
            var typeMap = A.Fake<ITypeMap>();
            handler.AddMembers(typeMap);
            var expected = ExampleStaticProperties;
            var actual = codeTypeDeclaration.Members.OfType<CodeMemberProperty>().Select(x => x.Name);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void AddsMembersAddsInstancePropertiesToInterfaceWithCorrectType()
        {
            var codeTypeDeclaration = new CodeTypeDeclaration("IExampleClassAdapter") { IsInterface = true };
            var handler = new TypeDeclarationHandler(typeof(ExampleClass), codeTypeDeclaration, false);
            var typeMap = A.Fake<ITypeMap>();
            handler.AddMembers(typeMap);
            Assert.IsTrue(codeTypeDeclaration.Members.OfType<CodeMemberProperty>().All(x => x.Type.BaseType == "System.String"));
        }
        [Test]
        public void AddsMembersAddsInstancePropertiesWithRightAccessToInterface()
        {
            var codeTypeDeclaration = new CodeTypeDeclaration("IExampleClassStaticAdapter") { IsInterface = true };
            var handler = new TypeDeclarationHandler(typeof(ExampleClass), codeTypeDeclaration, false);
            var typeMap = A.Fake<ITypeMap>();
            handler.AddMembers(typeMap);
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
