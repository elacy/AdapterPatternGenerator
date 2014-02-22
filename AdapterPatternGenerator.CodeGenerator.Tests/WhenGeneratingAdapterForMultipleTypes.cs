using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using AdapterPatternGenerator.Example.DifferentNameSpace;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class WhenGeneratingAdapterForMultipleTypes : BaseGeneratorTests
    {
        private const string BaseNameSpace = "BaseNameSpace";
        private const string DirectoryName = "DirectoryName";

        private const string ClassesNameSpace = BaseNameSpace + "." + Constants.ClassesNamespace;
        private const string InterfacesNameSpace = BaseNameSpace + "." + Constants.InterfacesNamespace;

        private const string TestTypesNameSpace = @"AdapterPatternGenerator.Example";
        private const string DifferentNameSpace = TestTypesNameSpace + ".DifferentNameSpace";

        private const string TestTypesInterfaceNamespace = InterfacesNameSpace + "." + TestTypesNameSpace;
        private const string TestTypesClassesNamespace = ClassesNameSpace + "." + TestTypesNameSpace;

        private const string DifferentNsInterfaceNamespace = InterfacesNameSpace + "." + DifferentNameSpace;
        private const string DifferentNsClassesNamespace = ClassesNameSpace + "." + DifferentNameSpace;

        private const string TestTypesDirectory = @"AdapterPatternGenerator\Example";
        private const string DifferentNameSpaceDirectory = TestTypesDirectory + @"\DifferentNameSpace";

        private static readonly string InterfacesDirectory = 
            Path.Combine(DirectoryName, BaseNameSpace, Constants.InterfacesNamespace, TestTypesDirectory);

        private static readonly string ClassesDirectory = 
            Path.Combine(DirectoryName, BaseNameSpace, Constants.ClassesNamespace, TestTypesDirectory);


        private static readonly string DifferentNamespaceInterfacesDirectory =
            Path.Combine(DirectoryName, BaseNameSpace, Constants.InterfacesNamespace, DifferentNameSpaceDirectory);

        private static readonly string DifferentNameSpaceClassesDirectory = 
            Path.Combine(DirectoryName, BaseNameSpace, Constants.ClassesNamespace, DifferentNameSpaceDirectory);

        public WhenGeneratingAdapterForMultipleTypes()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(_types, DirectoryName, BaseNameSpace);
        }
        private readonly List<Type> _types = new List<Type> { typeof(ExampleClass), typeof(ExampleSealedClass), typeof(ExampleDifferentNameSpaceClass) };


        [Test]
        public void ItShouldCreateADirectoryForExampleClass()
        {
            A.CallTo(() => Ioc.DirectoryStaticAdapter.CreateDirectory(ClassesDirectory)).MustHaveHappened();
        }
        [Test]
        public void ItShouldCreateADirectoryForExampleClassInterface()
        {
            A.CallTo(() => Ioc.DirectoryStaticAdapter.CreateDirectory(InterfacesDirectory)).MustHaveHappened();
        }

        [Test]
        public void ItShouldCreateADirectoryForExampleDifferentNameSpaceClass()
        {
            A.CallTo(() => Ioc.DirectoryStaticAdapter.CreateDirectory(DifferentNameSpaceClassesDirectory)).MustHaveHappened();
        }
        [Test]
        public void ItShouldCreateADirectoryForExampleDifferentNameSpaceClassInterface()
        {
            A.CallTo(() => Ioc.DirectoryStaticAdapter.CreateDirectory(DifferentNamespaceInterfacesDirectory)).MustHaveHappened();
        }

        [Test]
        public void ItShouldUseLanguageSetInConstantsToGenerateCode()
        {
            Assert.IsTrue(Results.All(x => x.Language == Constants.GeneratedLanguage));
        }
        [Test]
        public void ItShouldUseBracingStyleSetInConstantsToGenerateCode()
        {
            Assert.IsTrue(Results.All(x => x.CodeGeneratorOptions.BracingStyle == Constants.GeneratedBracingStyle));
        }
        [Test]
        public void AllClassesShouldBePartial()
        {
            Assert.IsTrue(AllCodeTypeDeclarations.All(x => x.IsPartial));
        }
        [Test]
        public void AllClassesShouldHaveGeneratedCodeAttribute()
        {
            Assert.IsTrue(AllCodeTypeDeclarations.All(x => x.CustomAttributes.AsEnumerable().Any(y => y.AttributeType.BaseType == Constants.CodeGenerationAttribute)));
        }
        [Test]
        public void CorrectAdapterClassesAreCreated()
        {
            var expected = new[]
            {
                ClassesNameSpace + "." + Constants.BaseInstanceAdapterName,
                TestTypesClassesNamespace + ".ExampleClassAdapter",
                TestTypesInterfaceNamespace + ".IExampleClassAdapter",
                TestTypesClassesNamespace + ".ExampleClassStaticAdapter",
                TestTypesInterfaceNamespace + ".IExampleClassStaticAdapter",
                TestTypesClassesNamespace + ".ExampleSealedClassAdapter",
                TestTypesInterfaceNamespace + ".IExampleSealedClassAdapter",
                TestTypesClassesNamespace + ".ExampleSealedClassStaticAdapter",
                TestTypesInterfaceNamespace + ".IExampleSealedClassStaticAdapter",
                DifferentNsInterfaceNamespace + ".IExampleDifferentNameSpaceClassAdapter",
                DifferentNsClassesNamespace + ".ExampleDifferentNameSpaceClassAdapter",
                DifferentNsInterfaceNamespace + ".IExampleDifferentNameSpaceClassStaticAdapter",
                DifferentNsClassesNamespace + ".ExampleDifferentNameSpaceClassStaticAdapter",
                
            };

            var actual = AllCodeNamespaces.SelectMany(ns => ns.Types.AsEnumerable().Select(ctd => ns.Name + "." + ctd.Name)).ToArray();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void InstanceAdaptersInheritFromTheBaseInstanceAdapter()
        {
            const string adapter = "Adapter";
            var query = from type in _types
                        join typeDeclaration in AllCodeTypeDeclarations
                            on type.Name + adapter equals typeDeclaration.Name
                        select new { type, TypeDeclaration = typeDeclaration };
            foreach (var item in query)
            {
                Assert.AreEqual(ClassesNameSpace + "." + Constants.BaseInstanceAdapterName + "`1", item.TypeDeclaration.BaseTypes[0].BaseType);
                Assert.AreEqual(item.type.FullName, item.TypeDeclaration.BaseTypes[0].TypeArguments[0].BaseType);
            }
        }

        [Test]
        public void CreatesSingleSolution()
        {
            var solution = Solutions.Single();
            var path = Path.Combine(DirectoryName, BaseNameSpace);
            Assert.AreEqual(path, solution.ParentDirectory);
            Assert.AreEqual(BaseNameSpace, solution.Name);
        }
        [Test]
        public void AddedProjectForClasses()
        {
            var solution = Solutions.Single();
            Assert.AreEqual(1, solution.Projects.Count(x => x.ProjectName == Constants.ClassesNamespace));
        }
        [Test]
        public void AddedProjectForInterfaces()
        {
            var solution = Solutions.Single();
            Assert.AreEqual(1, solution.Projects.Count(x => x.ProjectName == Constants.InterfacesNamespace));
        }


    }
}
