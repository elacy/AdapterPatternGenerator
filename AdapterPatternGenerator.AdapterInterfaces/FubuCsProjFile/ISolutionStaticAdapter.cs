using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile
{
    public interface ISolutionStaticAdapter
    {
        ISolutionAdapter CreateNew(string directory, string name);

        string VS2010 { get; }
        string VS2012 { get; }
        string VS2013 { get; }
    }
}
