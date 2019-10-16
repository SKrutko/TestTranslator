using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestTranslator
{
    class AssertionExpression
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
       // private static Regex regex = new Regex(@"(\S+)\.(\S+)\((.)\);$");
        private static Regex regex = new Regex(@"(\S+)\.(\S+)\((.+)\);$");
        public String Class;
        public String Method;
        public String Arguments;

        public static bool IsAssertion(ExpressionStatementSyntax expressionStatement)
        {
            //TODO
            return expressionStatement.ToString().Contains("Assert");
        }

        public AssertionExpression(ExpressionStatementSyntax expressionStatement)
        {
          
            Match match = regex.Match(expressionStatement.ToString());
            if (match.Success)
            {               
                Class = match.Groups[1].Value;
                Method = match.Groups[2].Value;
                Arguments = match.Groups[3].Value;
            }
            else
            {
                Logger.Info(" no mach ");
                throw new Exception();
            }
        }


    }
}
