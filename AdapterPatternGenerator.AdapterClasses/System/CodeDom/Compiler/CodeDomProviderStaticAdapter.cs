using System.CodeDom.Compiler;
using AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler;

namespace AdapterPatternGenerator.AdapterClasses.System.CodeDom.Compiler
{
    public class CodeDomProviderStaticAdapter :BaseStaticAdapterClass, ICodeDomProviderStaticAdapter
    {
        public ICodeDomProviderAdapter CreateProvider(string language)
        {
            return new CodeDomProviderAdapter(CodeDomProvider.CreateProvider(language));
        }
    }
}
