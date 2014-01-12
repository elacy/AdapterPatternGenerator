﻿using System;
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
        private readonly ITypeMap _typeMap;

        public CodeCompileUnitCreator(ITypeDeclarationCreator typeDeclarationCreator, ITypeMap typeMap)
        {
            _typeDeclarationCreator = typeDeclarationCreator;
            _typeMap = typeMap;
        }

        public IEnumerable<CodeCompileUnit> CreateCodeCompileUnit(List<Type> types)
        {
            var typesByNamespace = types.GroupBy(x => x.Namespace);
            var codeCompileUnits = new List<CodeCompileUnit>();
            foreach (var nameSpace in typesByNamespace)
            {
                foreach (var type in nameSpace)
                {
                    var typeDeclarations = _typeDeclarationCreator.CreateTypes(type, _typeMap).ToList();
                    var classes = typeDeclarations.Where(x => x.IsClass);
                    codeCompileUnits.Add(CreateCodeUnit(string.Format("Classes.{0}", nameSpace.Key), classes)); ;
                    var interfaces = typeDeclarations.Where(x => x.IsInterface);
                    codeCompileUnits.Add(CreateCodeUnit(string.Format("Interfaces.{0}", nameSpace.Key), interfaces));
                }
            }
            foreach (var type in types)
            {
                _typeDeclarationCreator.AddMembers(type,_typeMap);
            }
            return codeCompileUnits;
        }

        private CodeCompileUnit CreateCodeUnit(string namespaceName,
            IEnumerable<CodeTypeDeclaration> codeTypeDeclarations)
        {
            var nameSpace = new CodeNamespace(namespaceName);
            foreach (var codeTypeDeclaration in codeTypeDeclarations)
            {
                nameSpace.Types.Add(codeTypeDeclaration);
            }
            var codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.Namespaces.Add(nameSpace);
            return codeCompileUnit;
        }

    }
}
