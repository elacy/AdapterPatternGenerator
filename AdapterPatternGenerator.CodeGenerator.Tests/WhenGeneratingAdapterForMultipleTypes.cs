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
using FubuCsProjFile;
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
            foreach (var codeTypeDeclaration in AllCodeTypeDeclarations)
            {
                var customAttribute =
                    codeTypeDeclaration.CustomAttributes.AsEnumerable()
                        .First(x => x.AttributeType.BaseType == Constants.CodeGenerationAttribute);
                Assert.AreEqual(Constants.ProductName, GetValue(customAttribute.Arguments[0]));
                Assert.AreEqual(Constants.ProductVersion, GetValue(customAttribute.Arguments[1]));
            }
        }

        private string GetValue(CodeAttributeArgument argument)
        {
            var primitive = argument.Value as CodePrimitiveExpression;
            if (primitive != null)
            {
                return primitive.Value as string;
            }
            return null;
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

        private Solution GetSingleSolution()
        {
            if (Solutions.Count != 1)
            {
                Assert.Inconclusive();
            }
            return Solutions.Single();
        }

        private SolutionProject GetSingleProject(string name)
        {
            var solution = GetSingleSolution();
            if (solution.Projects.Count(x => x.ProjectName == name) != 1)
            {
                Assert.Inconclusive();
            }
            return solution.Projects.Single(x => x.ProjectName == name);
        }

        private SolutionProject GetClassesProject()
        {
            return GetSingleProject(Constants.ClassesNamespace);
        }
        private SolutionProject GetInterfacesProject()
        {
            return GetSingleProject(Constants.InterfacesNamespace);
        }
        [Test]
        public void AddedProjectForClasses()
        {
            var solution = GetSingleSolution();
            Assert.AreEqual(1, solution.Projects.Count(x => x.ProjectName == Constants.ClassesNamespace));
        }
        [Test]
        public void AddedProjectForInterfaces()
        {
            var solution = GetSingleSolution();
            Assert.AreEqual(1, solution.Projects.Count(x => x.ProjectName == Constants.InterfacesNamespace));
        }

        [Test]
        public void ClassesProjectHasRightFiles()
        {
            var project = GetClassesProject();
            var codeFiles = project.Project.All<CodeFile>();
            var actual = codeFiles.Select(x => x.Include);

            var expected = Results.Where(IsClass).Select(x => GetPath(x.CodeCompileUnit,ClassesNameSpace));
            CollectionAssert.AreEquivalent(expected,actual);
        }

        [Test]
        public void InterfacesProjectHasRightFiles()
        {
            var project = GetInterfacesProject();
            var codeFiles = project.Project.All<CodeFile>();
            var actual = codeFiles.Select(x => x.Include);

            var expected = Results.Where(IsInterface).Select(x => GetPath(x.CodeCompileUnit, InterfacesNameSpace));
            CollectionAssert.AreEquivalent(expected, actual);
        }
        private static string GetPath(CodeCompileUnit codeCompileUnit, string baseNameSpace)
        {
            var nameSpace = codeCompileUnit.Namespaces[0];
            var type = nameSpace.Types[0];
            var fullName = nameSpace.Name + "." + type.Name;
            fullName = fullName.Substring(baseNameSpace.Length + 1);
            return Path.Combine(fullName.Split('.')) + ".cs";
        }
        private static bool IsInterface(Result result)
        {
            return result.CodeCompileUnit.Namespaces[0].Types[0].IsInterface;
        }
        private static bool IsClass(Result result)
        {
            return result.CodeCompileUnit.Namespaces[0].Types[0].IsClass;
        }
    }
}
