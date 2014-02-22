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
        private const string ExampleNameSpace = "AdapterPatternGenerator.Example";
        public WhenGeneratingAdapterForExampleClass()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(new List<Type> { typeof(ExampleClass) }, DirectoryName, BaseNameSpace);
        }

        private static IEnumerable<CodeTypeMember> ExpectedInstanceMembers(bool isInterface)
        {
            yield return CreateProperty("ExampleProperty", typeof (string), true, true,!isInterface);
            yield return CreateProperty("ExampleReadOnlyProperty", typeof(string), true, false, !isInterface);
            yield return CreateProperty("ExampleWriteOnlyProperty", typeof(string), false, true, !isInterface);
            yield return CreateProperty("Field", typeof(int), true, true, !isInterface);
            yield return CreateProperty("ReadonlyField", typeof(int), true, false, !isInterface);
            yield return CreateProperty("AnotherExampleClass", BaseNameSpace + "." + Constants.InterfacesNamespace + "." + ExampleNameSpace + ".IExampleClassAdapter", true, true, !isInterface);
            yield return CreateProperty("List", typeof(List<int>), true, true, !isInterface);
            yield return CreateProperty("TestDictionary", typeof(Dictionary<string, int>), true, true, !isInterface);
            yield return CreateProperty("NestedType", typeof(List<List<int>>), true, true, !isInterface);
            yield return CreateMethod("ToString", typeof(string), !isInterface);
            yield return CreateMethod("GetHashCode", typeof(int), !isInterface);
            yield return CreateMethod("Equals", typeof(bool), !isInterface, new CodeParameterDeclarationExpression(typeof(object), "obj"));
        }

        private static IEnumerable<CodeTypeMember> ExpectedStaticMembers(bool isInterface)
        {
            yield return CreateProperty("StaticProperty", typeof(string), true, true, !isInterface);
            yield return CreateProperty("AnotherStaticProperty", typeof(int), true, true, !isInterface);
            yield return CreateProperty("StaticField", typeof(int), true, true, !isInterface);
            yield return CreateProperty("ConstTest", typeof(string), true, false, !isInterface);
        }
        private static CodeTypeMember CreateMethod(string name, Type returnType,bool includeWireUp, params CodeParameterDeclarationExpression[] parameters)
        {
            return CreateMethod(name, new CodeTypeReference(returnType),includeWireUp,parameters);
        }
        private static CodeTypeMember CreateMethod(string name, CodeTypeReference returnType,bool includeWireUp, params CodeParameterDeclarationExpression[] parameters)
        {
            var passParams = parameters.Select(x => new CodeVariableReferenceExpression(x.Name)).Cast<CodeExpression>().ToArray();
            var cmm = new CodeMemberMethod()
            {
                Name = name,
                Attributes = MemberAttributes.Public,
                ReturnType = returnType,
                
            };
            if (includeWireUp)
            {
                var methodInvokeExpression = new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
                            Constants.InternalAdapterFieldName), name, passParams) ;
                if (returnType == null)
                {
                    cmm.Statements.Add(methodInvokeExpression);
                }
                else
                {
                    cmm.Statements.Add(new CodeMethodReturnStatement(methodInvokeExpression));
                }
            }
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

        private static string GetInterfaceName(string name)
        {
            return BaseNameSpace + "." + Constants.InterfacesNamespace + "." + ExampleNameSpace + ".I" + name;
        }
        private static CodeTypeDeclaration ExpectedIExampleClassAdapter()
        {
            var obj = CreateDeclaration("IExampleClassAdapter");
            obj.IsInterface = true;
            obj.Members.AddRange(ExpectedInstanceMembers(true).ToArray());
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
            obj.BaseTypes.Add(GetInterfaceName(obj.Name));

            obj.Members.AddRange(ExpectedInstanceMembers(false).ToArray());
            return obj;
        }
        private static CodeTypeDeclaration ExpectedIExampleClassStaticAdapter()
        {
            var obj = CreateDeclaration("IExampleClassStaticAdapter");
            obj.IsInterface = true;
            obj.Members.AddRange(ExpectedStaticMembers(false).ToArray());
            return obj;
        }
        private static CodeTypeDeclaration ExpectedExampleClassStaticAdapter()
        {
            var obj = CreateDeclaration("ExampleClassStaticAdapter");
            obj.IsClass = true;
            obj.Members.AddRange(ExpectedStaticMembers(true).ToArray());
            obj.BaseTypes.Add(GetInterfaceName(obj.Name));
            return obj;
        }

        private static CodeMemberProperty CreateProperty(string name, Type type, bool hasGet, bool hasSet, bool wireUp)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet,wireUp);
        }
        private static CodeMemberProperty CreateProperty(string name, string type, bool hasGet, bool hasSet, bool wireUp)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet, wireUp);
        }
        private static CodeMemberProperty CreateProperty(string name, CodeTypeReference type, bool hasGet, bool hasSet,bool wireUp)
        {
            return new CodeMemberProperty
            {
                Name = name,
                Type = type,
                HasGet = hasGet,
                HasSet = hasSet
            };
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
