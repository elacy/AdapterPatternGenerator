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

        public static string StaticProperty { get; set; }

        public static int AnotherStaticProperty { get; set; }

        public static int StaticField = 4;

        public readonly int ReadonlyField = 4;

        public const string ConstTest = "Test";

        //public Dictionary<string,int> TestDictionary = new Dictionary<string, int>();

        //public List<List<int>> NestedType { get; set; } 

        public ExampleClass AnotherExampleClass { get; set; }

        public int Field = 4;
        //public List<int> List { get; set; } 
    }
}
