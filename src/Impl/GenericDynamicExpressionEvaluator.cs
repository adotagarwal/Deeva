using System.Collections.Generic;
using Deeva.CodeRefiner;
using Deeva.Helpers;

namespace Deeva.Impl
{
    public class GenericDynamicExpressionEvaluator<TOut,TIn> : DynamicExpressionEvaluator, IDynamicExpressionEvaluator<TOut,TIn>
    {
        public GenericDynamicExpressionEvaluator(params ICodeRefiner[] refinements) : base(refinements)
        {
        
        }

        internal override string BuildClass(string expression, IEnumerable<string> usedLibraries)
        {
            return ClassGenerationHelper.BuildClass(expression, usedLibraries, typeof(TOut), typeof(TIn));
        }

        public virtual TOut EvaluateExpression(string expression, TIn parameter = default(TIn),
                                               bool skipRefinements = false)
        {
            var obj = base.EvaluateExpression(expression, parameter, skipRefinements);
            if (obj is TOut)
            {
                return ((TOut)obj);
            }

            return default(TOut);
        }
    }

    public class GenericDynamicExpressionEvaluator<TOut, TIn, TIn2> : DynamicExpressionEvaluator, IDynamicExpressionEvaluator<TOut, TIn, TIn2>
    {
        public GenericDynamicExpressionEvaluator(params ICodeRefiner[] refinements) : base(refinements)
        {
            
        }

        internal override string BuildClass(string expression, IEnumerable<string> usedLibraries)
        {
            return ClassGenerationHelper.BuildClass(expression, usedLibraries, typeof (TOut), typeof (TIn),
                                                    typeof (TIn2));
        }

        public TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, bool skipRefinements = false)
        {
            var obj = base.EvaluateExpression(expression, new object[] {parameter, parameter2}, skipRefinements);

            if (obj is TOut)
            {
                return ((TOut) obj);
            }

            return default(TOut);
        }
    }

    public class GenericDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3> : DynamicExpressionEvaluator, IDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3>
    {
        public GenericDynamicExpressionEvaluator(params ICodeRefiner[] refinements)
            : base(refinements)
        {

        }

        internal override string BuildClass(string expression, IEnumerable<string> usedLibraries)
        {
            return ClassGenerationHelper.BuildClass(expression, usedLibraries, typeof(TOut), typeof(TIn),
                                                    typeof(TIn2), typeof(TIn3));
        }

        public TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, TIn3 parameter3, bool skipRefinements = false)
        {
            var obj = base.EvaluateExpression(expression, new object[] { parameter, parameter2, parameter3 }, skipRefinements);

            if (obj is TOut)
            {
                return ((TOut)obj);
            }

            return default(TOut);
        }
    }

    public class GenericDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3, TIn4> : DynamicExpressionEvaluator, IDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3, TIn4>
    {
        public GenericDynamicExpressionEvaluator(params ICodeRefiner[] refinements)
            : base(refinements)
        {

        }

        internal override string BuildClass(string expression, IEnumerable<string> usedLibraries)
        {
            return ClassGenerationHelper.BuildClass(expression, usedLibraries, typeof(TOut), typeof(TIn),
                                                    typeof(TIn2), typeof(TIn3), typeof(TIn4));
        }

        public TOut EvaluateExpression(string expression, TIn parameter, TIn2 parameter2, TIn3 parameter3, TIn4 parameter4, bool skipRefinements = false)
        {
            var obj = base.EvaluateExpression(expression, new object[] { parameter, parameter2, parameter3 }, skipRefinements);

            if (obj is TOut)
            {
                return ((TOut)obj);
            }

            return default(TOut);
        }
    }
}