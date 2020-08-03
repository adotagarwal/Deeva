using System.Collections.Generic;
using Deeva.CodeRefiner;
using Deeva.Impl;

namespace Deeva
{
    public static class DynamicExpressionEvaluatorFactory
    {
        private static ICodeRefiner[] GetRefiners()
        {
            var refiners = new List<ICodeRefiner>
                {
                    new MathLibraryRefinement(),
                    new UsingsExtractionRefinement()
                };
            return refiners.ToArray();
        }



        /// <summary>
        /// Creates a dynamic expression evaluator that caches compiled expressions and supports
        /// strong typing.
        /// </summary>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator GetDefaultDynamicExpressionEvaluator()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new DynamicExpressionEvaluator(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that caches compiled expressions and supports
        /// strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut,TIn> GetCachingDynamicExpressionEvaluator<TOut, TIn>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new CachingDynamicExpressionEvaluator<TOut, TIn>(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that supports strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut, TIn> GetGenericExpressionEvaluator<TOut, TIn>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new GenericDynamicExpressionEvaluator<TOut, TIn>(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that supports strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut, TIn, TIn2> GetCachingDynamicExpressionEvaluator<TOut, TIn, TIn2>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new CachingDynamicExpressionEvaluator<TOut, TIn, TIn2>(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that caches compiled expressions and supports
        /// strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of the first parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn2">The type of the second parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut, TIn, TIn2> GetGenericExpressionEvaluator<TOut, TIn, TIn2>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new GenericDynamicExpressionEvaluator<TOut, TIn, TIn2>(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that caches compiled expressions and supports
        /// strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of the first parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn2">The type of the second parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn3">The type of the third parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3> GetGenericExpressionEvaluator<TOut, TIn, TIn2, TIn3>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new GenericDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3>(refiners);

            return retVal;
        }

        /// <summary>
        /// Creates a dynamic expression evaluator that caches compiled expressions and supports
        /// strong typing.
        /// </summary>
        /// <typeparam name="TOut">The type of object the evaluated expression returns</typeparam>
        /// <typeparam name="TIn">The type of the first parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn2">The type of the second parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn3">The type of the third parameter the evaluated expression takes in</typeparam>
        /// <typeparam name="TIn4">The type of the fourth parameter the evaluated expression takes in</typeparam>
        /// <returns>Dynamic Expression Evaluator</returns>
        public static IDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3, TIn4> GetGenericExpressionEvaluator<TOut, TIn, TIn2, TIn3, TIn4>()
        {
            // 1. instantiate and add refiners
            var refiners = GetRefiners();

            // 2. instantiate and return expression evaluator
            var retVal = new GenericDynamicExpressionEvaluator<TOut, TIn, TIn2, TIn3, TIn4>(refiners);

            return retVal;
        }
    }
}