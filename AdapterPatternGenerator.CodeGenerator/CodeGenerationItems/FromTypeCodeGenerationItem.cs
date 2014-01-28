using System;
using System.CodeDom;
using System.Collections.Generic;
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

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            codeTypeDeclaration.IsPartial = true;
            if (OriginalType.BaseType != null && OriginalType.BaseType != typeof(object))
            {
                codeTypeDeclaration.BaseTypes.Add(typeMap.GetInstanceInterface(OriginalType.BaseType));
            }
            foreach (var iface in OriginalType.GetInterfaces())
            {
                codeTypeDeclaration.BaseTypes.Add(typeMap.GetInstanceInterface(iface));
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
        private void AddProperty(CodeTypeDeclaration codeTypeDeclaration, PropertyInfo propertyInfo,ITypeMap typeMap)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyInfo.Name,
                Type = typeMap.GetInstanceInterface(propertyInfo.PropertyType),
                HasSet = propertyInfo.CanWrite,
                HasGet = propertyInfo.CanRead,
            };
            codeTypeDeclaration.Members.Add(property);
        }

        protected void AddProperties(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<PropertyInfo> propertyInfos,
            ITypeMap typeMap)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                AddProperty(codeTypeDeclaration,propertyInfo,typeMap);
            }
        }
        private void AddMethod(CodeTypeDeclaration codeTypeDeclaration, MethodInfo methodInfo, ITypeMap typeMap)
        {
            var method = new CodeMemberMethod
            {
                Name = methodInfo.Name,
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
            codeTypeDeclaration.Members.Add(method);
        }
        protected void AddMethods(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<MethodInfo> methodInfos,
            ITypeMap typeMap)
        {
            foreach (var methodInfo in methodInfos)
            {
                AddMethod(codeTypeDeclaration, methodInfo, typeMap);
            }
        }
        private void AddField(CodeTypeDeclaration codeTypeDeclaration, FieldInfo fieldInfo, ITypeMap typeMap)
        {
            var property = new CodeMemberProperty
            {
                Name = fieldInfo.Name,
                Type = typeMap.GetInstanceInterface(fieldInfo.FieldType),
                HasSet = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral,
                HasGet = true,
            };
            codeTypeDeclaration.Members.Add(property);
        }
        protected void AddFields(CodeTypeDeclaration codeTypeDeclaration, IEnumerable<FieldInfo> fieldInfos,
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