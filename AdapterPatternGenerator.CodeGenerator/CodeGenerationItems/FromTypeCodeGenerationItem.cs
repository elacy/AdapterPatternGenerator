using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public abstract class FromTypeCodeGenerationItem : CodeGenerationItem
    {
        protected readonly Type OriginalType;

        protected FromTypeCodeGenerationItem(Type originalType,string nameSpace, string name) : base(nameSpace, name)
        {
            OriginalType = originalType;
        }


        private IEnumerable<CodeTypeReference> GetBaseTypes(Type type, ITypeMap typeMap)
        {
            if (type.BaseType == typeof (Enum))
            {
                yield break;
            }
            if (type.BaseType != typeof (object))
            {
                yield return typeMap.GetInstanceInterface(OriginalType.BaseType);
            }
            foreach (var item in type.GetInterfaces())
            {
                yield return typeMap.GetInstanceInterface(item);
            }
        }

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            codeTypeDeclaration.IsPartial = true;
            foreach (var item in GetBaseTypes(OriginalType, typeMap))
            {
                codeTypeDeclaration.BaseTypes.Add(item);
            }

            for(int i = 0; i < OriginalType.GenericTypeArguments.Length;i++)
            {
                var current = OriginalType.GenericTypeArguments[i];
                var codeTypeParameter = new CodeTypeParameter(current.Name);

                foreach (var constaint in current.GetGenericParameterConstraints())
                {
                    codeTypeParameter.Constraints.Add(typeMap.GetInstanceInterface(constaint));
                }
                codeTypeDeclaration.TypeParameters.Add(codeTypeParameter);
            }
            return codeTypeDeclaration;
        }

        private readonly static string[] MembersNotIncluded = new[] {"GetType"};
        protected void AddMembers(CodeTypeDeclaration codeTypeDeclaration, BindingFlags bindingFlags, ITypeMap typeMap)
        {
            AddProperties(codeTypeDeclaration, OriginalType.GetProperties(bindingFlags), typeMap);
            var methodInfos = OriginalType.GetMethods(bindingFlags)
                .Where(mi => !(mi.IsSpecialName && (mi.Name.StartsWith("set_") || mi.Name.StartsWith("get_"))));
            AddMethods(codeTypeDeclaration, methodInfos, typeMap);
            AddFields(codeTypeDeclaration, OriginalType.GetFields(bindingFlags), typeMap);
        }

        private static void AddProperty(CodeTypeDeclaration codeTypeDeclaration, PropertyInfo propertyInfo,ITypeMap typeMap)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyInfo.Name,
                Type = typeMap.GetInstanceInterface(propertyInfo.PropertyType),
                HasSet = propertyInfo.CanWrite,
                HasGet = propertyInfo.CanRead,
            };

            if (!codeTypeDeclaration.IsInterface)
            {
                var field = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), Constants.InternalAdapterFieldName);
                var propertyReference = new CodePropertyReferenceExpression(field, property.Name);

                if (property.HasGet)
                {
                    property.GetStatements.Add(new CodeMethodReturnStatement(propertyReference));
                }
                if (property.HasSet)
                {
                    property.SetStatements.Add(new CodeAssignStatement(propertyReference, new CodeVariableReferenceExpression("value")));
                }
            }
            AddMember(codeTypeDeclaration, property);
        }

        private static void AddProperties(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<PropertyInfo> propertyInfos,
            ITypeMap typeMap)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                AddProperty(codeTypeDeclaration,propertyInfo,typeMap);
            }
        }
        private static void AddMethod(CodeTypeDeclaration codeTypeDeclaration, MethodInfo methodInfo, ITypeMap typeMap)
        {
            var method = new CodeMemberMethod
            {
                Name = methodInfo.Name,
                Attributes = MemberAttributes.Public,
                ReturnType = typeMap.GetInstanceInterface(methodInfo.ReturnType)
            };
            var arguments = new List<CodeExpression>();
            foreach (var param in methodInfo.GetParameters())
            {
                
                var codeParam = new CodeParameterDeclarationExpression
                {
                    Type = typeMap.GetInstanceInterface(param.ParameterType),
                    Name = param.Name
                };
                if(param.IsOut)
                {
                    codeParam.Direction = FieldDirection.Out;
                }
                else if (param.ParameterType.IsByRef)
                {
                    codeParam.Direction = FieldDirection.Ref;
                }
                arguments.Add(new CodeDirectionExpression(codeParam.Direction, new CodeArgumentReferenceExpression(codeParam.Name)));
                
                method.Parameters.Add(codeParam);
            }
            
            if (!codeTypeDeclaration.IsInterface)
            {
                var methodInvokeExpression = new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
                            Constants.InternalAdapterFieldName), method.Name, arguments.ToArray());
                if (method.ReturnType.BaseType == new CodeTypeReference(typeof(void)).BaseType)
                {
                    method.Statements.Add(methodInvokeExpression);
                }
                else
                {
                    method.Statements.Add(new CodeMethodReturnStatement(methodInvokeExpression));
                }
            }
            AddMember(codeTypeDeclaration, method);
        }
        private static void AddMethods(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<MethodInfo> methodInfos,
            ITypeMap typeMap)
        {

            foreach (var methodInfo in methodInfos)
            {
                AddMethod(codeTypeDeclaration, methodInfo, typeMap);
            }
        }

        private static void AddMember(CodeTypeDeclaration codeTypeDeclaration, CodeTypeMember member)
        {
            if (MembersNotIncluded.Contains(member.Name))
                return;
            codeTypeDeclaration.Members.Add(member);
        }
        private static void AddField(CodeTypeDeclaration codeTypeDeclaration, FieldInfo fieldInfo, ITypeMap typeMap)
        {
            var property = new CodeMemberProperty
            {
                Name = fieldInfo.Name,
                Type = typeMap.GetInstanceInterface(fieldInfo.FieldType),
                HasSet = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral,
                HasGet = true,
            };
            if (!codeTypeDeclaration.IsInterface)
            {
                var field = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), Constants.InternalAdapterFieldName);
                var propertyReference = new CodeFieldReferenceExpression(field, property.Name);

                if (property.HasGet)
                {
                    property.GetStatements.Add(new CodeMethodReturnStatement(propertyReference));
                }
                if (property.HasSet)
                {
                    property.SetStatements.Add(new CodeAssignStatement(propertyReference, new CodeVariableReferenceExpression("value")));    
                }
                
            }
            AddMember(codeTypeDeclaration, property);
        }
        private static void AddFields(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<FieldInfo> fieldInfos,
            ITypeMap typeMap)
        {
            foreach (var fieldInfo in fieldInfos)
            {
                AddField(codeTypeDeclaration, fieldInfo, typeMap);
            }
        }
        protected static string GetNamespace(Type originalType,string baseNameSpace, bool isInterface)
        {
            return CreateNameSpace(GetNamespace(baseNameSpace, isInterface), originalType.Namespace);
        }
    }
}