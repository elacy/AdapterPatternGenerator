using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.Example
{
    public class ExampleClass
    {
        public string ExampleProperty { get; set; }

        public string ExampleReadOnlyProperty
        {
            get { return string.Empty; }
        }
        internal string InternalProperty { get; set; }

        public string ExampleWriteOnlyProperty {  set{} }
    }
}
