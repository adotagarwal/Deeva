using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Deeva.CodeRefiner
{
    public class MathLibraryRefinement : ICodeRefiner
    {
        private ArrayList _mathMembers;
        private Hashtable _mathMembersMap;

        private MathLibraryRefinement(ArrayList mathMembers, Hashtable mathMembersMap)
        {
            _mathMembers = mathMembers;
            _mathMembersMap = mathMembersMap;
        }

        public MathLibraryRefinement() : this (new ArrayList(), new Hashtable())
        {
            GetMathMemberNames();
        }

        private static readonly string[] MemberExclusions = new string[] { "ToString", "GetType"};

        void GetMathMemberNames()
        {
            // get a reflected assembly of the System assembly
            Assembly systemAssembly = Assembly.GetAssembly(typeof(System.Math));
            try
            {
                //cant call the entry method if the assembly is null
                if (systemAssembly != null)
                {
                    //Use reflection to get a reference to the Math class

                    Module[] modules = systemAssembly.GetModules(false);
                    Type[] types = modules[0].GetTypes();

                    //loop through each class that was defined and look for the first occurrance of the Math class
                    foreach (Type type in types)
                    {
                        if (type.Name == "Math")
                        {
                            // get all of the members of the math class and map them to the same member
                            // name in uppercase
                            MemberInfo[] mis = type.GetMembers();
                            foreach (MemberInfo mi in mis)
                            {
                                if (MemberExclusions.Any(s=>mi.Name.Equals(s)))  //we do not take ToString because every object has it
                                    continue;
                                _mathMembers.Add(mi.Name);
                                _mathMembersMap[mi.Name.ToUpper()] = mi.Name;
                            }
                        }
                        //if the entry point method does return in Int32, then capture it and return it
                    }


                    //if it got here, then there was no entry point method defined.  Tell user about it
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:  An exception occurred while executing the script", ex);
            }
        }

        public string RefineEvaluationString(string eval)
        {
            // look for regular expressions with only letters
            Regex regularExpression = new Regex("[a-zA-Z_]+");

            // track all functions and constants in the evaluation expression we already replaced
            ArrayList replacelist = new ArrayList();

            // find all alpha words inside the evaluation function that are possible functions
            MatchCollection matches = regularExpression.Matches(eval);
            foreach (Match m in matches)
            {
                // if the word is found in the math member map, add a Math prefix to it
                bool isContainedInMathLibrary = _mathMembersMap[m.Value.ToUpper()] != null;
                if (replacelist.Contains(m.Value) == false && isContainedInMathLibrary)
                {
                    eval = eval.Replace(m.Value, "Math." + _mathMembersMap[m.Value.ToUpper()]);
                }

                // we matched it already, so don't allow us to replace it again
                replacelist.Add(m.Value);
            }

            // return the modified evaluation string
            return eval;
        }

        public object Clone()
        {
            var mm = new ArrayList(_mathMembers);
            var mmm = new Hashtable(_mathMembersMap);

            return new MathLibraryRefinement(mm, mmm); // copy the learned memebers
        }
    }
}