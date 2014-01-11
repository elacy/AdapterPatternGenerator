using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class CodeCompileUnitCreator : ICodeCompileUnitCreator
    {
        private readonly ITypeDeclarationCreator _typeDeclarationCreator;

        public CodeCompileUnitCreator(ITypeDeclarationCreator typeDeclarationCreator)
        {
            _typeDeclarationCreator = typeDeclarationCreator;
        }

        public CodeCompileUnit CreateCodeCompileUnit(IEnumerable<Type> types)
        {
            var codeCompileUnit = new CodeCompileUnit();
            var typesByNamespace = types.GroupBy(x => x.Namespace);
            foreach (var nameSpace in typesByNamespace)
            {
                var codeNamespace = new CodeNamespace(nameSpace.Key);
                foreach (var type in nameSpace)
                {
                    foreach (var typeDeclaration in _typeDeclarationCreator.CreateTypes(type))
                    {
                        codeNamespace.Types.Add(typeDeclaration);
                    }
                }
                codeCompileUnit.Namespaces.Add(codeNamespace);
            }
            return codeCompileUnit;
        }
    }
}
