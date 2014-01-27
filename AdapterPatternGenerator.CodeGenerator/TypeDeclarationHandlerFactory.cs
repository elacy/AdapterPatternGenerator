using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeDeclarationHandlerFactory : ITypeDeclarationHandlerFactory
    {
        public ITypeDeclarationHandler Create(Type type, string baseNamespace, bool isInterface, bool isStatic)
        {
            var codeTypeDeclaration = new CodeTypeDeclaration(GetTypeName(type, isInterface, isStatic))
            {
                IsInterface = isInterface,
                IsPartial = true
            };
            if (!isInterface && !isStatic)
            {
                var baseType = baseNamespace + "." + Constants.ClassesNamespace + "." + Constants.BaseInstanceAdapterName;
                var reference = new CodeTypeReference(baseType);
                reference.TypeArguments.Add(type);
                codeTypeDeclaration.BaseTypes.Add(reference);
            }

            return new TypeDeclarationHandler(type, baseNamespace, codeTypeDeclaration, isStatic);
        }
        private string GetTypeName(Type type, bool addInterface, bool isStatic)
        {
            return string.Format("{0}{1}{2}", addInterface ? "I" : "", type.Name, isStatic ? "StaticAdapter" : "Adapter");
        }
    }
}
