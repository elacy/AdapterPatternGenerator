using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<CodeTypeDeclaration> CreateTypes(Type type)
        {
            yield return new CodeTypeDeclaration(GetTypeName(type,false,false))
            {
            };
            yield return new CodeTypeDeclaration(GetTypeName(type, true, false))
            {
                IsInterface = true
            };
            yield return new CodeTypeDeclaration(GetTypeName(type, false, true))
            {
            };
            yield return new CodeTypeDeclaration(GetTypeName(type, true, true))
            {
                IsInterface = true
            };

        }
    }
}
