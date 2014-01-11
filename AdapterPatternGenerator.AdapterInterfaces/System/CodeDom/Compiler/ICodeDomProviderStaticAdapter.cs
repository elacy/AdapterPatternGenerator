using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.AdapterInterfaces.System.CodeDom.Compiler
{
    public interface ICodeDomProviderStaticAdapter
    {
        ICodeDomProviderAdapter CreateProvider(string language);
    }
}
