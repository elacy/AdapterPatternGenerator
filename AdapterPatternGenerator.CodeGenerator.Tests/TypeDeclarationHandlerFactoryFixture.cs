using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class TypeDeclarationHandlerFactoryFixture
    {
        private const string BaseNamespace = "BaseSpace";
        [Test]
        public void CreatesInterface()
        {
            var factory = new TypeDeclarationHandlerFactory();
            var handler = factory.Create(typeof (ExampleClass), BaseNamespace, true, false);
            Assert.IsTrue(handler.IsInterface);
            Assert.IsFalse(handler.IsStatic);
        }
        [Test]
        public void CreatesClass()
        {
            var factory = new TypeDeclarationHandlerFactory();
            var handler = factory.Create(typeof(ExampleClass), BaseNamespace, false, false);
            Assert.IsFalse(handler.IsInterface);
            Assert.IsFalse(handler.IsStatic);
        }
        [Test]
        public void CreatesInstanceClassWithCorrectBaseClass()
        {
            var factory = new TypeDeclarationHandlerFactory();
            var handler = factory.Create(typeof(ExampleClass), BaseNamespace, false, false);
            var topBaseType = handler.Declaration.BaseTypes[0];
            const string expected =  BaseNamespace + "." + Constants.ClassesNamespace + "." + Constants.BaseInstanceAdapterName +"`1";
            Assert.AreEqual(expected, topBaseType.BaseType);
            Assert.AreEqual(typeof(ExampleClass).FullName,topBaseType.TypeArguments[0].BaseType);
        }
        [Test]
        public void CreatesOtherTypesWithoutBaseClass()
        {
            var factory = new TypeDeclarationHandlerFactory();
            Assert.AreEqual(0, factory.Create(typeof(ExampleClass), BaseNamespace, false, true).Declaration.BaseTypes.Count);
            Assert.AreEqual(0, factory.Create(typeof(ExampleClass), BaseNamespace, true, false).Declaration.BaseTypes.Count);
            Assert.AreEqual(0, factory.Create(typeof(ExampleClass), BaseNamespace, true, true).Declaration.BaseTypes.Count);
        }
        [Test]
        public void CreatesStaticInterface()
        {
            var factory = new TypeDeclarationHandlerFactory();
            var handler = factory.Create(typeof(ExampleClass), BaseNamespace, true, true);
            Assert.IsTrue(handler.IsInterface);
            Assert.IsTrue(handler.IsStatic);
        }
        [Test]
        public void CreatesStaticClass()
        {
            var factory = new TypeDeclarationHandlerFactory();
            var handler = factory.Create(typeof(ExampleClass), BaseNamespace, false, true);
            Assert.IsFalse(handler.IsInterface);
            Assert.IsTrue(handler.IsStatic);
        }
    }
}
