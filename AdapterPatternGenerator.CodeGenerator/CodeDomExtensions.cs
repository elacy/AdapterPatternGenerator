using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator
{
    public static class CodeDomExtensions
    {
        public static CodeTypeReference Clone(this CodeTypeReference reference)
        {
            if (reference == null) return null;
            CodeTypeReference r = new CodeTypeReference
            {
                ArrayElementType = reference.ArrayElementType.Clone(),
                ArrayRank = reference.ArrayRank,
                BaseType = reference.BaseType,
                Options = reference.Options
            };
            r.TypeArguments.AddRange(reference.TypeArguments.Clone());
            r.UserData.AddRange(reference.UserData);
            return r;
        }
        public static CodeTypeReferenceCollection Clone(this CodeTypeReferenceCollection collection)
        {
            if (collection == null) return null;
            CodeTypeReferenceCollection c = new CodeTypeReferenceCollection();
            foreach (CodeTypeReference reference in collection)
                c.Add(reference.Clone());
            return c;
        }
        public static void AddRange(this IDictionary toDictionary, IDictionary fromDictionary)
        {
            foreach (var key in fromDictionary.Keys)
                toDictionary[key] = fromDictionary[key];
        }
    }
}
