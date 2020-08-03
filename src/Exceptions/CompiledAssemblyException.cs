using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deeva.Exceptions
{
    public class CompiledAssemblyException  : Exception
    {
        public string ClassDefinition { get; private set; }

        public CompiledAssemblyException(string classDefinition)
            : base("Could not compile, load or execute class definition.")
        {
            ClassDefinition = classDefinition;
        }
    }
}
