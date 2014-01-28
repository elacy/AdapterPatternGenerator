using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class WhenGeneratingAdapterForExampleClass:BaseGeneratorTests
    {
        private const string DirectoryName = "DirectoryName";
        private const string BaseNameSpace = "BaseNameSpace";

        public WhenGeneratingAdapterForExampleClass()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(new List<Type> { typeof(ExampleClass)}, DirectoryName, BaseNameSpace);
        }



        //public Dictionary<string, int> TestDictionary = new Dictionary<string, int>();

        //public List<List<int>> NestedType { get; set; }

        //public ExampleSealedClass ExampleSealedClass { get; set; }
        private readonly ExpectedProperty[] _expectedInstanceProperties = new[]
            {
                new ExpectedProperty("ExampleProperty",typeof(string), true, true),
                new ExpectedProperty("ExampleReadOnlyProperty",typeof(string), true, false),
                new ExpectedProperty("ExampleWriteOnlyProperty",typeof(string), false, true),
                new ExpectedProperty("Field",typeof(int), true, true),
                new ExpectedProperty("ReadonlyField",typeof(int), true, false),
                new ExpectedProperty("AnotherExampleClass",BaseNameSpace + "." + Constants.InterfacesNamespace + ".AdapterPatternGenerator.Example.IExampleClassAdapter", true, true),
                //new ExpectedProperty("List",typeof(List<int>), true, true),
                //new ExpectedProperty("TestDictionary",typeof(Dictionary<string,int>), true, true),
                //new ExpectedProperty("NestedType",typeof(List<List<int>>), true, true),
            };

        private void AssertAreEquivalent(ExpectedProperty[] expected,
            List<CodeMemberProperty> codeMemberProperties)
        {
            CollectionAssert.AreEquivalent(expected.Select(x=>x.Name),codeMemberProperties.Select(x=>x.Name));
            var query = from expectedProp in expected
                join actualProp in codeMemberProperties
                    on expectedProp.Name equals actualProp.Name
                select new {expectedProp, actualProp};
            foreach (var item in query)
            {
                Assert.AreEqual(item.expectedProp.Type,item.actualProp.Type.BaseType);
                Assert.AreEqual(item.expectedProp.CanRead, item.actualProp.HasGet);
                Assert.AreEqual(item.expectedProp.CanWrite, item.actualProp.HasSet,item.actualProp.Name);
            }
        }

        private List<CodeMemberProperty> GetProperties(string className)
        {
            var adapter = AllCodeTypeDeclarations.First(x => x.Name == className);
            return adapter.Members.OfType<CodeMemberProperty>().ToList();
        }

        [Test]
        public void InstanceInterfaceHasCorrectProperties()
        {
            AssertAreEquivalent(_expectedInstanceProperties, GetProperties("IExampleClassAdapter"));
        }

        [Test]
        public void InstanceClassHasCorrectProperties()
        {
            AssertAreEquivalent(_expectedInstanceProperties, GetProperties("ExampleClassAdapter"));
        }
        private readonly ExpectedProperty[] _expectedStaticProperties = new[]
            {
                new ExpectedProperty("StaticProperty",typeof(string), true, true),
                new ExpectedProperty("AnotherStaticProperty",typeof(int), true, true),
                new ExpectedProperty("StaticField",typeof(int), true, true),
                new ExpectedProperty("ConstTest",typeof(string), true, false),
            };
        [Test]
        public void StaticClassHasCorrectProperties()
        {
            AssertAreEquivalent(_expectedStaticProperties, GetProperties("ExampleClassStaticAdapter"));
        }

        [Test]
        public void StaticInterfaceHasCorrectProperties()
        {
            AssertAreEquivalent(_expectedStaticProperties, GetProperties("IExampleClassStaticAdapter"));
        }

    }
}
