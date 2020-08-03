using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CSharp;

namespace Deeva.Helpers
{

    public static class ClassGenerationHelper
    {
        #region Class Generators

        internal static CodeMemberField MakeFieldVariable(string fieldName, string typeName, MemberAttributes accessLevel)
        {
            var field = new CodeMemberField(typeName, fieldName) {Attributes = accessLevel};
            return field;
        }
        internal static CodeMemberField MakeFieldVariable(string fieldName, Type type, MemberAttributes accessLevel)
        {
            var field = new CodeMemberField(type, fieldName) {Attributes = accessLevel};
            return field;
        }
        internal static CodeMemberProperty MakeProperty(string propertyName, string internalName, Type type)
        {
            var myProperty = new CodeMemberProperty
                {
                    Name = propertyName,
                    Attributes = MemberAttributes.Public,
                    Type = new CodeTypeReference(type),
                    HasGet = true,
                    HasSet = true 
                };
            myProperty.Comments.Add(new CodeCommentStatement(String.Format("The {0} property is the returned result", propertyName)));
            myProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName)));
            myProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName),
                    new CodePropertySetValueReferenceExpression()));

            return myProperty;
        }

        /// <summary>
        /// Main driving routines for building a class to evaluate a dynamic expression
        /// </summary>
        public static string BuildClass(string expression, IEnumerable<string> usings, Type outputType = null, params Type[] inputType)
        {
            if (outputType == null)
                outputType = typeof (object);

            if (inputType == null || inputType.Length == 0)
                inputType = new[] {typeof (object)};

            // need a string to put the code into
            var source = new StringBuilder();
            var sw = new StringWriter(source);

            //Declare your provider and generator
            var codeProvider = new CSharpCodeProvider();
            var generator = codeProvider.CreateGenerator(sw);
            var codeOpts = new CodeGeneratorOptions();
            
            var myNamespace = new CodeNamespace(Constants.Namespace);
            foreach (string s in usings)
            {
                myNamespace.Imports.Add(new CodeNamespaceImport(s));
            }
            
            // add to the usings the type's namespaces
            foreach (var type in inputType)
            {
                myNamespace.Imports.Add(new CodeNamespaceImport(type.Namespace));
            }

            //Build the class declaration and member variables			
            var classDeclaration = new CodeTypeDeclaration
                {
                    IsClass = true,
                    Name = Constants.ClassName,
                    Attributes = MemberAttributes.Public
                };
            classDeclaration.Members.Add(MakeFieldVariable(Constants.EvaluatedMember, outputType, MemberAttributes.Private));

            //default constructor
            var defaultConstructor = new CodeConstructor {Attributes = MemberAttributes.Public};
            defaultConstructor.Comments.Add(new CodeCommentStatement("Default Constructor for class", true));
            defaultConstructor.Statements.Add(new CodeSnippetStatement("//TODO: implement default constructor"));
            classDeclaration.Members.Add(defaultConstructor);

            //property
            classDeclaration.Members.Add(MakeProperty(Constants.EvaluatedProperty, Constants.EvaluatedMember, outputType));

            //Our Calculate Method
            var myMethod = new CodeMemberMethod
                {
                    Name = Constants.MethodName,
                    ReturnType = new CodeTypeReference(outputType),
                    Attributes = MemberAttributes.Public
                };
            myMethod.Comments.Add(new CodeCommentStatement("Expression / calculation to be evaluated", true));

            int i = 0;
            foreach (Type type in inputType)
            {
                myMethod.Parameters.Add(new CodeParameterDeclarationExpression(type, string.Format("{0}{1}", Constants.ParameterPrefix, i > 0 ? i.ToString() : string.Empty)));
                i++;
            }

            myMethod.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression(Constants.EvaluatedProperty),
                                                            new CodeSnippetExpression(expression)));
            myMethod.Statements.Add(
                new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
                                                                               Constants.EvaluatedProperty)));
            classDeclaration.Members.Add(myMethod);
            
            //write code
            myNamespace.Types.Add(classDeclaration);
            generator.GenerateCodeFromNamespace(myNamespace, sw, codeOpts);
            sw.Flush();
            sw.Close();

            return source.ToString();
        }

        #endregion
    }
}