﻿using System;
using System.CodeDom;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public class EnumItem : CopyCodeGenerationItem
    {

        public EnumItem(string baseNameSpace, Type originalType)
            : base(originalType, baseNameSpace)
        {
        }

        public override CodeTypeDeclaration Generate(ITypeMap typeMap)
        {
            var codeTypeDeclaration = base.Generate(typeMap);
            codeTypeDeclaration.IsEnum = true;
            var names = Enum.GetNames(OriginalType);
            var values = Enum.GetValues(OriginalType);

            for (int i = 0; i < names.Length; i++)
            {
                codeTypeDeclaration.Members.Add(new CodeMemberField(Name, names[i])
                {
                    InitExpression = new CodePrimitiveExpression(values.GetValue(i))
                });
            }
            return codeTypeDeclaration;
        }
    }
}