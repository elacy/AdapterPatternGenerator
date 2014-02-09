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

        public static IEnumerable<CodeTypeReference> AsEnumerable(this CodeTypeReferenceCollection collection)
        {
            return collection.Cast<CodeTypeReference>();
        }
        public static IEnumerable<CodeTypeMember> AsEnumerable(this CodeTypeMemberCollection collection)
        {
            return collection.Cast<CodeTypeMember>();
        }

        public static IEnumerable<CodeParameterDeclarationExpression> AsEnumerable(this CodeParameterDeclarationExpressionCollection collection)
        {
            return collection.Cast<CodeParameterDeclarationExpression>();
        }
        public static IEnumerable<CodeAttributeDeclaration> AsEnumerable(this CodeAttributeDeclarationCollection collection)
        {
            return collection.Cast<CodeAttributeDeclaration>();
        }
        public static IEnumerable<CodeTypeDeclaration> AsEnumerable(this CodeTypeDeclarationCollection collection)
        {
            return collection.Cast<CodeTypeDeclaration>();
        }
        public static IEnumerable<CodeNamespace> AsEnumerable(this CodeNamespaceCollection collection)
        {
            return collection.Cast<CodeNamespace>();
        }
        public static IEnumerable<CodeCommentStatement> AsEnumerable(this CodeCommentStatementCollection collection)
        {
            return collection.Cast<CodeCommentStatement>();
        }
        public static IEnumerable<CodeDirective> AsEnumerable(this CodeDirectiveCollection collection)
        {
            return collection.Cast<CodeDirective>();
        }
        public static IEnumerable<CodeAttributeArgument> AsEnumerable(this CodeAttributeArgumentCollection collection)
        {
            return collection.Cast<CodeAttributeArgument>();
        }
        public static IEnumerable<CodeTypeParameter> AsEnumerable(this CodeTypeParameterCollection collection)
        {
            return collection.Cast<CodeTypeParameter>();
        }
        public static IEnumerable<CodeStatement> AsEnumerable(this CodeStatementCollection collection)
        {
            return collection.Cast<CodeStatement>();
        }
        public static IEnumerable<CodeExpression> AsEnumerable(this CodeExpressionCollection collection)
        {
            return collection.Cast<CodeExpression>();
        }
    }
}
