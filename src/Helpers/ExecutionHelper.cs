using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Deeva.Exceptions;

namespace Deeva.Helpers
{


    public static class ExecutionHelper
    {
        #region Execution Methods

        /// <summary>
        /// Runs the Calculate method in our on-the-fly assembly
        /// </summary>
        /// <param name="results"></param>
        /// <param name="parameters"></param>
        /// <param name="errorHandler"></param>
        public static object RunCode(CompilerResults results, object[] parameters, Action<Exception, object> errorHandler = null)
        {
            Assembly executingAssembly = results.CompiledAssembly;
            try
            {
                //cant call the entry method if the assembly is null
                if (executingAssembly != null)
                {
                    object assemblyInstance = executingAssembly.CreateInstance(string.Format("{0}.{1}", Constants.Namespace, Constants.ClassName));
                    //Use reflection to call the static Main function

                    Module[] modules = executingAssembly.GetModules(false);
                    Type[] types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the entry point method
                    foreach (Type type in types)
                    {
                        MethodInfo[] mis = type.GetMethods();
                        foreach (MethodInfo mi in mis)
                        {
                            if (mi.Name == Constants.MethodName)
                            {
                                object result = mi.Invoke(assemblyInstance, parameters);
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorHandler != null)
                {
                    try
                    {
                        errorHandler(ex, executingAssembly);
                    }
                    catch (Exception)
                    {
                        //todo: eat this and log
                        throw;
                    }
                }
                else
                {
                    throw new UnhandledEvaluationException(ex);
                }
                Console.WriteLine("Error: An exception occurred while executing the script", ex);
            }

            // should have returned by now, otherwise throw exception
            throw new EmptyEvaluationException();
        }

        #endregion
    }
}