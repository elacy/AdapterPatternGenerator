﻿using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdapterPatternGenerator.CodeGenerator
{
    public interface ITypeDeclarationCreator
    {
        IEnumerable<CodeTypeDeclaration> CreateTypes(Type type);
    }
}