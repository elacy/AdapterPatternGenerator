using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile
{
    public interface ISolutionAdapter
    {
         string Version { get; set; }
        ISolutionProjectAdapter AddProject(string projectName);
    }
}
