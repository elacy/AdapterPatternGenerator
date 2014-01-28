using System;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public abstract class CopyCodeGenerationItem : FromTypeCodeGenerationItem
    {
        private static string GetNameSpace(string baseNameSpace,Type originalType)
        {
            return CreateNameSpace(baseNameSpace, Constants.ClassesNamespace, originalType.Namespace);
        }
        private static string GetName(Type originalType)
        {
            return String.Format("{0}Copy", originalType.Name);
        }
        protected CopyCodeGenerationItem(Type originalType, string baseNameSpace)
            : base(originalType, GetNameSpace(baseNameSpace, originalType), GetName(originalType))
        {
        }

        
    }
}