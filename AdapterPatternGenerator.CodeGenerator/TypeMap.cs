using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public class TypeMap:ITypeMap
    {
        readonly Dictionary<Type, CodeTypeDeclaration> _defaultTypes = new Dictionary<Type, CodeTypeDeclaration>();
        readonly Dictionary<Type, List<CodeTypeDeclaration>> _allTypes = new Dictionary<Type, List<CodeTypeDeclaration>>();
        readonly Dictionary<CodeTypeDeclaration, CodeTypeReference> _references = new Dictionary<CodeTypeDeclaration, CodeTypeReference>(); 
        public bool Add(Type type, CodeTypeDeclaration codeTypeDeclaration, bool defaultTypeDeclaration, string baseNameSpace)
        {
            if (defaultTypeDeclaration)
            {
                _defaultTypes.Add(type,codeTypeDeclaration);
            }
            if (_allTypes.ContainsKey(type))
            {
                _allTypes[type].Add(codeTypeDeclaration);
            }
            else
            {
                _allTypes.Add(type, new List<CodeTypeDeclaration>{codeTypeDeclaration});
            }
            _references.Add(codeTypeDeclaration,new CodeTypeReference());

            return true;

        }

        public CodeTypeDeclaration GetDefault(Type type)
        {
            if (_defaultTypes.ContainsKey(type))
            {
                return _defaultTypes[type];
            }
            return null;
        }

        public List<CodeTypeDeclaration> GetAll(Type type)
        {
            if (_allTypes.ContainsKey(type))
            {
                return _allTypes[type];
            }
            return new List<CodeTypeDeclaration>();
        }
    }

    public interface ITypeMap
    {
        bool Add(Type type, CodeTypeDeclaration codeTypeDeclaration, bool defaultTypeDeclaration, string baseNameSpace);
        CodeTypeDeclaration GetDefault(Type type);
        List<CodeTypeDeclaration> GetAll(Type type);
    }
}
