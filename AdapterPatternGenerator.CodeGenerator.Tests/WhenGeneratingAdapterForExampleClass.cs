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
    public class WhenGeneratingAdapterForExampleClass : BaseGeneratorTests
    {
        private const string DirectoryName = "DirectoryName";
        private const string BaseNameSpace = "BaseNameSpace";

        public WhenGeneratingAdapterForExampleClass()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(new List<Type> { typeof(ExampleClass) }, DirectoryName, BaseNameSpace);
        }

        private static readonly CodeTypeMember[] _expectedInstanceMembers =
            {
                CreateProperty("ExampleProperty",typeof(string), true, true),
                CreateProperty("ExampleReadOnlyProperty",typeof(string), true, false),
                CreateProperty("ExampleWriteOnlyProperty",typeof(string), false, true),
                CreateProperty("Field",typeof(int), true, true),
                CreateProperty("ReadonlyField",typeof(int), true, false),
                CreateProperty("AnotherExampleClass",BaseNameSpace + "." + Constants.InterfacesNamespace + ".AdapterPatternGenerator.Example.IExampleClassAdapter", true, true),
                CreateProperty("List",typeof(List<int>), true, true),
                CreateProperty("TestDictionary",typeof(Dictionary<string,int>), true, true),
                CreateProperty("NestedType",typeof(List<List<int>>), true, true),
                CreateMethod("ToString", typeof(string)),
                CreateMethod("GetHashCode", typeof(int)),
                CreateMethod("Equals", typeof(bool),new CodeParameterDeclarationExpression(typeof(object),"obj")),
            };
        private static readonly CodeTypeMember[] _expectedStaticMembers =
            {
                CreateProperty("StaticProperty",typeof(string), true, true),
                CreateProperty("AnotherStaticProperty",typeof(int), true, true),
                CreateProperty("StaticField",typeof(int), true, true),
                CreateProperty("ConstTest",typeof(string), true, false),
            };
        private static CodeTypeMember CreateMethod(string name, Type returnType, params CodeParameterDeclarationExpression[] parameters)
        {
            return CreateMethod(name, new CodeTypeReference(returnType),parameters);
        }
        private static CodeTypeMember CreateMethod(string name, CodeTypeReference returnType, params CodeParameterDeclarationExpression[] parameters)
        {
            var cmm = new CodeMemberMethod()
            {
                Name = name,
                ReturnType = returnType
            };
            foreach (var item in parameters)
            {
                cmm.Parameters.Add(item);
            }
            return cmm;
        }

        private static CodeTypeDeclaration CreateDeclaration(string name)
        {
            return new CodeTypeDeclaration(name)
            {
                CustomAttributes = new CodeAttributeDeclarationCollection { new CodeAttributeDeclaration(Constants.CodeGenerationAttribute)},
                IsPartial = true
            };
        }
        private static CodeTypeDeclaration ExpectedIExampleClassAdapter()
        {
            var obj = CreateDeclaration("IExampleClassAdapter");
            obj.IsInterface = true;
            obj.Members.AddRange(_expectedInstanceMembers);
            return obj;
        }
        private static CodeTypeDeclaration ExpectedExampleClassAdapter()
        {
            var obj = CreateDeclaration("ExampleClassAdapter");
            obj.IsClass = true;

            var baseTypeReference =
                new CodeTypeReference(BaseNameSpace + "." + Constants.ClassesNamespace + "." +
                                      Constants.BaseInstanceAdapterName);
            baseTypeReference.TypeArguments.Add(typeof (ExampleClass));
            obj.BaseTypes.Add(baseTypeReference);

            obj.Members.AddRange(_expectedInstanceMembers);
            return obj;
        }
        private static CodeTypeDeclaration ExpectedIExampleClassStaticAdapter()
        {
            var obj = CreateDeclaration("IExampleClassStaticAdapter");
            obj.IsInterface = true;
            obj.Members.AddRange(_expectedStaticMembers);
            return obj;
        }
        private static CodeTypeDeclaration ExpectedExampleClassStaticAdapter()
        {
            var obj = CreateDeclaration("ExampleClassStaticAdapter");
            obj.IsClass = true;
            obj.Members.AddRange(_expectedStaticMembers);
            return obj;
        }

        private static CodeMemberProperty CreateProperty(string name, Type type, bool hasGet, bool hasSet)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet);
        }
        private static CodeMemberProperty CreateProperty(string name, string type, bool hasGet, bool hasSet)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet);
        }
        private static CodeMemberProperty CreateProperty(string name, CodeTypeReference type, bool hasGet, bool hasSet)
        {
            return new CodeMemberProperty
            {
                Name = name,
                Type = type,
                HasGet = hasGet,
                HasSet = hasSet
            };
        }


        private void Verify(CodeTypeMember expected )
        {
            var actual = AllCodeTypeDeclarations.First(x => x.Name == expected.Name);
            CodeDomAssert.AreEqual(expected,actual);
        }

        [Test]
        public void VerifyIExampleClassAdapterProperties()
        {
            Verify(ExpectedIExampleClassAdapter());
        }
        [Test]
        public void VerifyExampleClassAdapterProperties()
        {
            Verify(ExpectedExampleClassAdapter());
        }
        [Test]
        public void VerifyIExampleClassStaticAdapterProperties()
        {
            Verify(ExpectedIExampleClassStaticAdapter());
        }
        [Test]
        public void VerifyExampleClassStaticAdapterProperties()
        {
            Verify(ExpectedExampleClassStaticAdapter());
        }

    }
}
