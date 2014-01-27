﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class ExpectedProperty
    {
        public bool CanRead { get; private set; }
        public bool CanWrite { get; private set; }
        public Type Type { get; private set; }
        public string Name { get; private set; }

        public ExpectedProperty(string name, Type type, bool canRead, bool canWrite)
        {
            CanRead = canRead;
            CanWrite = canWrite;
            Name = name;
            Type = type;
        }


    }
}
