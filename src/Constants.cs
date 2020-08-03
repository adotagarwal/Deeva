using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deeva
{
    public static class Constants
    {
        public const string Namespace = "Deeva.RunTimeExpression";
        public const string ClassName = "DeevaExpression";
        public const string MethodName = "Evaluate";

        // these two should be kept in sync... the evaluated member needs to be the first line of a multiline expression
        public const string EvaluatedProperty = "Result";
        public const string EvaluatedMember = "result";

        public const string ParameterPrefix = "input";

        public static readonly string[] DefaultUsings = new [] { "System", "System.Collections.Generic", "System.Text", "System.Linq", "Deeva" };
        public static readonly string[] DefaultLibraryReferences = new[] { "mscorlib.dll", "System.dll", "System.Core.dll" };
    }
}
