using System;

namespace AdapterPatternGenerator.CodeGenerator.CodeGenerationItems
{
    public abstract class AdapterItem : FromTypeCodeGenerationItem
    {

        protected AdapterItem(Type originalType, string nameSpace,  string name) : 
            base(originalType,nameSpace,name)
        {
        }
    }
}