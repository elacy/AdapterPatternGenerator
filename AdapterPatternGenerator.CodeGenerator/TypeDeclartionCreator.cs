using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeDeclarationCreator:ITypeDeclarationCreator
    {
        private string GetTypeName(Type type, bool addInterface, bool isStatic)
        {
            return string.Format("{0}{1}{2}", addInterface ? "I" : "", type.Name, isStatic ? "StaticAdapter" : "Adapter");
        }

        private CodeTypeDeclaration CreateType(Type type, ITypeMap typeMap, bool isInterface, bool isStatic)
        {
            var codeTypeDeclaration = new CodeTypeDeclaration(GetTypeName(type, isInterface, isStatic))
            {
                IsInterface = isInterface
            };
            typeMap.Add(type, codeTypeDeclaration, isInterface && !isStatic);
            return codeTypeDeclaration;
        }

        public IEnumerable<CodeTypeDeclaration> CreateTypes(Type type,ITypeMap typeMap )
        {
            yield return CreateType(type, typeMap, false, false);
            yield return CreateType(type, typeMap, false, true);
            yield return CreateType(type, typeMap, true, false);
            yield return CreateType(type, typeMap, true, true);

        }

        public void AddMembers(Type type, ITypeMap typeMap)
        {
            foreach (var typeDeclaration in typeMap.GetAll(type))
            {
                foreach (var property in type.GetProperties())
                {
                    AddProperty(typeDeclaration,property);
                }
            }
        }

        private void AddProperty(CodeTypeDeclaration codeTypeDeclaration, PropertyInfo propertyInfo)
        {
            var property = new CodeMemberProperty
            {
                Name = propertyInfo.Name,
                Type = new CodeTypeReference(propertyInfo.PropertyType),
                HasSet = propertyInfo.CanWrite,
                HasGet = propertyInfo.CanRead
            };
            codeTypeDeclaration.Members.Add(property);
        }
    }
}
