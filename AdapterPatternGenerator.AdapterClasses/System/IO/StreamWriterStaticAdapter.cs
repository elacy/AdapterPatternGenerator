using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.AdapterClasses.System.IO
{
    public class StreamWriterStaticAdapter : BaseStaticAdapterClass, IStreamWriterStaticAdapter
    {
        public IStreamWriterAdapter NewUp(string path)
        {
            return new StreamWriterAdapter(new StreamWriter(path));
        }

    }
}
