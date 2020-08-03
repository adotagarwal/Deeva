using System;

namespace Deeva.CodeRefiner
{
    public interface ICodeRefiner : ICloneable
    {
        string RefineEvaluationString(string eval);
    }
}