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

        private void AddProperty(CodeTypeDeclaration codeTypeDeclaration, PropertyInfo propertyInfo,ITypeMap typeMap)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyInfo.Name,
                Type = typeMap.GetInstanceInterface(propertyInfo.PropertyType),
                HasSet = propertyInfo.CanWrite,
                HasGet = propertyInfo.CanRead,
            };
            AddMember(codeTypeDeclaration, property);
        }

        private void AddProperties(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<PropertyInfo> propertyInfos,
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
            foreach (var param in methodInfo.GetParameters())
            {
                var codeParam = new CodeParameterDeclarationExpression
                {
                    Type = typeMap.GetInstanceInterface(param.ParameterType),
                    Name = methodInfo.Name
                };
                method.Parameters.Add(codeParam);
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