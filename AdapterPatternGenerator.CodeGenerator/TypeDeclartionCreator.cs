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
            return null;
        }
    }
}
