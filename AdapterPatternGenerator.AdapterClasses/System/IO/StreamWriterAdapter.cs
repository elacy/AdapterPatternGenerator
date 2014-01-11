using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.AdapterInterfaces.System.IO;

namespace AdapterPatternGenerator.AdapterClasses.System.IO
{
    public class StreamWriterAdapter : BaseInstanceAdapterClass<StreamWriter>, IStreamWriterAdapter
    {
        public StreamWriterAdapter(StreamWriter streamWriter) : base(streamWriter)
        {
            
        }
    }
}
