using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;

namespace Deeva.Helpers
{
    /// <summary>
    /// Main driving routines for generating a C# compiler 
    /// which will compile dynamic expression classes
    /// </summary>
    public static class CompilationHelper
    {
        #region Compiler Generators

        /// <summary>
        /// Creates a compiler
        /// </summary>
        /// <returns></returns>
        private static ICodeCompiler CreateCompiler()
        {
            //Create an instance of the C# compiler   
            CodeDomProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler compiler = codeProvider.CreateCompiler();
            return compiler;
        }

        /// <summary>
        /// Creawte parameters for compiling
        /// </summary>
        /// <returns></returns>
        private static CompilerParameters CreateCompilerParameters()
        {
            //add compiler parameters and assembly references
            var compilerParams = new CompilerParameters
                {
                    CompilerOptions = "/target:library /optimize",
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = false
                };

            // key is file name, value is path
            var assembliesToReference = new Dictionary<string, string>();

            // add library references (system.dll, etc...)
            foreach (string lib in Constants.DefaultLibraryReferences)
            {
                //compilerParams.ReferencedAssemblies.Add(lib);
                assembliesToReference.Add(lib, lib);
            }

            // try getting loaded assemblies

            try
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
             
                loadedAssemblies.ForEach(
                    a =>
                        {
                            try
                            {
                                var fn = Path.GetFileName(a.Location);
                                if (!assembliesToReference.ContainsKey(fn))
                                {
                                    assembliesToReference.Add(fn, a.Location);
                                }

                                //if (!compilerParams.ReferencedAssemblies.Any())
                                //    compilerParams.ReferencedAssemblies.Add(fn);
                            }
                            catch (Exception ex)
                            {
                            }
                        });

            }
            catch (Exception ex)
            {
            }


            foreach (var kvp in assembliesToReference)
            {
                compilerParams.ReferencedAssemblies.Add(kvp.Value);
            }

            //try
            //{
            //    foreach (AssemblyName asmName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            //    {
            //        var fileName = Path.GetFileName(Assembly.ReflectionOnlyLoad(asmName.FullName).Location);

            //        if (!compilerParams.ReferencedAssemblies.Contains(fileName))
            //            compilerParams.ReferencedAssemblies.Add(fileName);
            //    }
            //}
            //catch (Exception)
            //{
            //}


            //try
            //{
            //    foreach (AssemblyName asmName in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            //    {
            //        var fileName = Path.GetFileName(Assembly.ReflectionOnlyLoad(asmName.FullName).Location);

            //        if (!compilerParams.ReferencedAssemblies.Contains(fileName))
            //            compilerParams.ReferencedAssemblies.Add(fileName);
            //    }
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    foreach (AssemblyName asmName in Assembly.GetCallingAssembly().GetReferencedAssemblies())
            //    {
            //        var fileName = Path.GetFileName(Assembly.ReflectionOnlyLoad(asmName.FullName).Location);

            //        if (!compilerParams.ReferencedAssemblies.Contains(fileName))
            //            compilerParams.ReferencedAssemblies.Add(fileName);
            //    }
            //}
            //catch (Exception)
            //{
            //}


            //try
            //{
            //    var execAssemblyName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            //    if (!compilerParams.ReferencedAssemblies.Contains(execAssemblyName))
            //        compilerParams.ReferencedAssemblies.Add(execAssemblyName);
            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}

            //try
            //{
            //    var execAssemblyName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            //    if (!compilerParams.ReferencedAssemblies.Contains(execAssemblyName))
            //        compilerParams.ReferencedAssemblies.Add(execAssemblyName);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    var execAssemblyName = Path.GetFileName(Assembly.GetCallingAssembly().Location);
            //    if (!compilerParams.ReferencedAssemblies.Contains(execAssemblyName))
            //        compilerParams.ReferencedAssemblies.Add(execAssemblyName);
            //}
            //catch (Exception)
            //{
                
            //}


            return compilerParams;
        }


        /// <summary>
        /// Compiles the code from the code string
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="parms"></param>
        /// <param name="source"></param>
        /// <param name="compilerErrorHandler"></param>
        /// <param name="compilerErrorsHandler"></param>
        /// <returns></returns>
        private static CompilerResults CompileCode(ICodeCompiler compiler, CompilerParameters parms, string source, Action<CompilerError> compilerErrorHandler = null, Action<CompilerErrorCollection> compilerErrorsHandler = null)
        {
            //actually compile the code
            CompilerResults results = compiler.CompileAssemblyFromSource(
                parms, source);

            //Do we have any compiler errors?
            if (results.Errors.Count > 0)
            {

                if (compilerErrorsHandler != null)
                {
                    try
                    {
                        compilerErrorsHandler(results.Errors);
                    }
                    catch (Exception)
                    {
                        //todo: log this
                        throw;
                    }
                }
                else if (compilerErrorHandler != null)
                    foreach (CompilerError error in results.Errors)
                    {
                        //WriteLine("Compile Error:" + error.ErrorText);
                        try
                        {
                            compilerErrorHandler(error);
                        }
                        catch (Exception)
                        {
                            //todo: log this
                            throw;
                        }
                    }

                return null;
            }

            return results;
        }

        #endregion

        /// <summary>
        /// Compiles the c# into an assembly if there are no syntax errors
        /// </summary>
        /// <returns></returns>
        public static CompilerResults CompileAssembly(string source, Action<CompilerError> compilerErrorHandler = null, Action<CompilerErrorCollection> compilerErrorsHandler = null)
        {
            // create a compiler
            ICodeCompiler compiler = CreateCompiler();
            // get all the compiler parameters
            CompilerParameters parms = CreateCompilerParameters();
            // compile the code into an assembly
            CompilerResults results = CompileCode(compiler, parms, source, compilerErrorHandler, compilerErrorsHandler);
            return results;
        }
    }
}