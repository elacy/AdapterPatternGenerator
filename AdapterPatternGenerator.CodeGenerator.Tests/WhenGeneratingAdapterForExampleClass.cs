using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.CodeGenerator.CodeGenerationItems;
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
            yield return CreateProperty("ExampleProperty", typeof(string), true, true, !isInterface, false);
            yield return CreateProperty("ExampleReadOnlyProperty", typeof(string), true, false, !isInterface, false);
            yield return CreateProperty("ExampleWriteOnlyProperty", typeof(string), false, true, !isInterface, false);
            yield return CreateProperty("Field", typeof(int), true, true, !isInterface, true);
            yield return CreateProperty("ReadonlyField", typeof(int), true, false, !isInterface, true);
            yield return CreateProperty("AnotherExampleClass", BaseNameSpace + "." + Constants.InterfacesNamespace + "." + ExampleNameSpace + ".IExampleClassAdapter", true, true, !isInterface, false);
            yield return CreateProperty("List", typeof(List<int>), true, true, !isInterface, false);
            yield return CreateProperty("TestDictionary", typeof(Dictionary<string, int>), true, true, !isInterface, true);
            yield return CreateProperty("NestedType", typeof(List<List<int>>), true, true, !isInterface,false);
            yield return CreateMethod("ToString", typeof(string), !isInterface);
            yield return CreateMethod("GetHashCode", typeof(int), !isInterface);
            yield return CreateMethod("Equals", typeof(bool), !isInterface, CreateParam(typeof(object), "obj"));
            yield return CreateMethod("RefMethod", !isInterface, CreateParam(new CodeTypeReference("System.String&"), "par", FieldDirection.Ref));
            yield return CreateMethod("OutMethod", !isInterface, CreateParam(new CodeTypeReference("System.String&"), "parout", FieldDirection.Out));
        }
        private static CodeParameterDeclarationExpression CreateParam(Type type, string name, FieldDirection direction = FieldDirection.In)
        {
            return CreateParam(new CodeTypeReference(type), name, direction);
        }
        private static CodeParameterDeclarationExpression CreateParam(CodeTypeReference type, string name, FieldDirection direction = FieldDirection.In)
        {
            return new CodeParameterDeclarationExpression()
            {
                Type = type,
                Name = name,
                Direction = direction
            };
        }

        private static IEnumerable<CodeTypeMember> ExpectedStaticMembers(bool isInterface)
        {
            yield return CreateProperty("StaticProperty", typeof(string), true, true, !isInterface,false);
            yield return CreateProperty("AnotherStaticProperty", typeof(int), true, true, !isInterface, false);
            yield return CreateProperty("StaticField", typeof(int), true, true, !isInterface, true);
            yield return CreateProperty("ConstTest", typeof(string), true, false, !isInterface, true);
        }

        private static CodeTypeMember CreateMethod(string name, Type returnType, bool includeWireUp, params CodeParameterDeclarationExpression[] parameters)
        {
            return CreateMethod(name, new CodeTypeReference(returnType), includeWireUp, parameters);
        }
        private static CodeTypeMember CreateMethod(string name,bool includeWireUp, params CodeParameterDeclarationExpression[] parameters)
        {
            return CreateMethod(name, new CodeTypeReference(typeof(void)), includeWireUp, parameters);
        }
        private static CodeTypeMember CreateMethod(string name, CodeTypeReference returnType,bool includeWireUp, params CodeParameterDeclarationExpression[] parameters)
        {

            var passParams = parameters
                .Select(x => new CodeDirectionExpression(x.Direction,new CodeArgumentReferenceExpression(x.Name)))
                .Cast<CodeExpression>().ToArray();
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
                if (returnType.BaseType == typeof(void).FullName)
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
                CustomAttributes = new CodeAttributeDeclarationCollection
                {
                    new CodeAttributeDeclaration(Constants.CodeGenerationAttribute)
                    {
                        Arguments =
                        {
                            new CodeAttributeArgument(new CodePrimitiveExpression(Constants.ProductName)),
                            new CodeAttributeArgument(new CodePrimitiveExpression(Constants.ProductVersion))
                        }
                    }
                },
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
            obj.Members.AddRange(ExpectedStaticMembers(true).ToArray());
            return obj;
        }
        private static CodeTypeDeclaration ExpectedExampleClassStaticAdapter()
        {
            var obj = CreateDeclaration("ExampleClassStaticAdapter");
            obj.IsClass = true;
            obj.Members.AddRange(ExpectedStaticMembers(false).ToArray());
            obj.BaseTypes.Add(GetInterfaceName(obj.Name));
            return obj;
        }

        private static CodeMemberProperty CreateProperty(string name, Type type, bool hasGet, bool hasSet, bool wireUp,bool isField)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet,wireUp,isField);
        }
        private static CodeMemberProperty CreateProperty(string name, string type, bool hasGet, bool hasSet, bool wireUp, bool isField)
        {
            return CreateProperty(name, new CodeTypeReference(type), hasGet, hasSet, wireUp, isField);
        }
        private static CodeMemberProperty CreateProperty(string name, CodeTypeReference type, bool hasGet, bool hasSet,bool wireUp, bool isField)
        {
            var property = new CodeMemberProperty
            {
                Name = name,
                Type = type,
                HasGet = hasGet,
                HasSet = hasSet
            };
            if (wireUp)
            {
                var field = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),  Constants.InternalAdapterFieldName);
                var propertyReference = isField?(CodeExpression)new CodeFieldReferenceExpression(field,name):new CodePropertyReferenceExpression(field, name);
                if (property.HasGet)
                {
                    property.GetStatements.Add(new CodeMethodReturnStatement(propertyReference));
                }
                if (property.HasSet)
                {
                    property.SetStatements.Add(new CodeAssignStatement(propertyReference, new CodeVariableReferenceExpression("value")));
                }
            }
            return property;
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
