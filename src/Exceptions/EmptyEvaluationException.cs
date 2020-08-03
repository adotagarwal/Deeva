using System;

namespace Deeva.Exceptions
{
    public class EmptyEvaluationException : Exception
    {
        public EmptyEvaluationException() : base("The expression was evaluated but did not yield a result. Please check for any additional exceptions...") { }
    }
}