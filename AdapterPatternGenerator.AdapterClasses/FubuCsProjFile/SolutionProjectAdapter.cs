using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.FubuCsProjFile;
using FubuCsProjFile;

namespace AdapterPatternGenerator.AdapterClasses.FubuCsProjFile
{
    public class SolutionProjectAdapter : BaseInstanceAdapterClass<SolutionProject>,ISolutionProjectAdapter
    {
        public SolutionProjectAdapter(SolutionProject adaptedClass) : base(adaptedClass)
        {
        }
    }
}
