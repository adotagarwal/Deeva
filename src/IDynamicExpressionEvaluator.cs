using System;
using System.CodeDom.Compiler;

namespace Deeva
{
    public interface IDynamicExpressionEvaluator
    {
        object EvaluateExpression(string expression, object parameter, bool skipRefinements = false);
        event Action<CompilerError> OnCompilationError;
        event Action<CompilerErrorCollection> OnCompilationErrors;
        event Action<Exception, object> OnRuntimeError;
    }

    public interface IDynamicExpressionEvaluator<out TOut, in TIn> : IDynamicExpressionEvaluator
    {
        TOut EvaluateExpression(string expression, TIn parameter, bool skipRefinements = false);
    }

    public interface IDynamicExpressionEvaluator<out TOut, in TIn, in TIn2> : IDynamicExpressionEvaluator
    {
        TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, bool skipRefinements = false);
    }

    public interface IDynamicExpressionEvaluator<out TOut, in TIn, in TIn2, in TIn3> : IDynamicExpressionEvaluator
    {
        TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, TIn3 parameter3, bool skipRefinements = false);
    }

    public interface IDynamicExpressionEvaluator<out TOut, in TIn, in TIn2, in TIn3, in TIn4> : IDynamicExpressionEvaluator
    {
        TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, TIn3 parameter3, TIn4 parameter4, bool skipRefinements = false);
    }
}