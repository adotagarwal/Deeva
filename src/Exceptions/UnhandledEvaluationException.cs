using System;

namespace Deeva.Exceptions
{
    public class UnhandledEvaluationException : Exception
    {
        public UnhandledEvaluationException() : base() {}
        public UnhandledEvaluationException(Exception innerException) : base("Unhandled exception raised during expression evaluation", innerException) { }
        public UnhandledEvaluationException(string message, Exception innerException) : base(message, innerException) { }
    }
}