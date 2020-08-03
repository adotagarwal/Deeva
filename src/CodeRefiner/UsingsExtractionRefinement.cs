using System.Collections.Generic;
using System.Text;

namespace Deeva.CodeRefiner
{
    public class UsingsExtractionRefinement : ICodeRefiner
    {
        public UsingsExtractionRefinement()
        {
            UsingStatements = new List<string>();
        }

        public List<string> UsingStatements { get; private set; }
        
        public string RefineEvaluationString(string eval)
        {
            StringBuilder sb = new StringBuilder();

            string[] lines = eval.Split(new char[] {'\n'});

            foreach (string line in lines)
            {
                if (line.ToUpper().StartsWith("USING "))
                {
                    var stmt = line.Substring(6).Replace(";","").Trim();
                    UsingStatements.Add(stmt);
                }
                else
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }

        public object Clone()
        {
            return new UsingsExtractionRefinement(); // dont copy the extracted statements
        }
    }
}