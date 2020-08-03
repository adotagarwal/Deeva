using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Deeva.Exceptions;
using Deeva.CodeRefiner;
using Deeva.Helpers;

namespace Deeva.Impl
{
    public class DynamicExpressionEvaluator : IDynamicExpressionEvaluator, IDynamicExpressionEvaluator<object, object> // for posterity
    {
        public event Action<CompilerError> OnCompilationError;
        public event Action<CompilerErrorCollection> OnCompilationErrors;
        public event Action<Exception, object> OnRuntimeError; 

        public List<ICodeRefiner> Refinements { get; set; }

        public List<string> UsedLibraries { get; set; } 

        public DynamicExpressionEvaluator(params ICodeRefiner[] refinements)
        {
            Refinements = new List<ICodeRefiner>(refinements);
            UsedLibraries = new List<string>(Constants.DefaultUsings);
        }

        protected virtual object EvaluateExpression(string expression, object[] parameters = null,
                                                    bool skipRefinements = false)
        {
            var refinedExpression = expression;
            var classDefinition = string.Empty;

            //0. initialize global usings
            var usings = new List<string>(UsedLibraries);

            //1. run refinements on input expression
            if (!skipRefinements)
            {
                foreach (ICodeRefiner refinement in Refinements)
                {
                    // use the original refinement if the author didnt implement clone correctly
                    var cloneRefinement = refinement.Clone() as ICodeRefiner ?? refinement;
                    refinedExpression = (cloneRefinement).RefineEvaluationString(refinedExpression);

                    if (cloneRefinement is UsingsExtractionRefinement)
                    {
                        var usingRefiner = cloneRefinement as UsingsExtractionRefinement;
                        usings.AddRange(usingRefiner.UsingStatements);
                    }
                }
            }

            //2. Build a class
            classDefinition = BuildClass(refinedExpression, usings);

            //3. Compile the class
            var assembly = CompileClass(classDefinition);

            //4. Run the code
            object retVal = null;
            if (assembly != null && assembly.CompiledAssembly != null)
                retVal = ExecutionHelper.RunCode(assembly, parameters, OnRuntimeError);

            return retVal;
        }

        public virtual object EvaluateExpression(string expression, object parameter = null, bool skipRefinements = false)
        {
            return EvaluateExpression(expression, new[] {parameter}, skipRefinements);
        }
        
        internal virtual string BuildClass(string expression, IEnumerable<string> usedLibraries)
        {
            return ClassGenerationHelper.BuildClass(expression, usedLibraries);
        }

        internal virtual CompilerResults CompileClass(string classDefinition)
        {
            return CompilationHelper.CompileAssembly(classDefinition, OnCompilationError, OnCompilationErrors);
        }

    }
}
