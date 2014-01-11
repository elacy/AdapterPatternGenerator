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
        public List<CodeTypeDeclaration> CreateTypes(IEnumerable<Type> exportedTypes)
        {
            return exportedTypes.SelectMany(CreateDeclarations).ToList();
        }

        private IEnumerable<CodeTypeDeclaration> CreateDeclarations(Type type)
        {
            yield return new CodeTypeDeclaration(string.Format("{0}Adapter", type.Name))
            {
            };
            yield return new CodeTypeDeclaration(string.Format("I{0}Adapter", type.Name))
            {
                IsInterface = true
            };
            yield return new CodeTypeDeclaration(string.Format("{0}StaticAdapter", type.Name))
            {
            };
            yield return new CodeTypeDeclaration(string.Format("I{0}StaticAdapter", type.Name))
            {
                IsInterface = true
            };

        }
    }
}
