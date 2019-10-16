using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{

    class AssertionTranslator
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary Dictionary = new Dictionary();

        public static MethodDeclarationSyntax TranslateAssertions(MethodDeclarationSyntax methodDeclaration)
        {
            for (int i = 0; i < methodDeclaration.DescendantNodes().OfType<ExpressionStatementSyntax>().Count(); i++)
            {
                ExpressionStatementSyntax expressionStatement = methodDeclaration.DescendantNodes().OfType<ExpressionStatementSyntax>().ToList()[i];

                if (AssertionExpression.IsAssertion(expressionStatement))
                {
                    AssertionExpression assertionExpression = new AssertionExpression(expressionStatement);

                    if (IsTranslatable(assertionExpression))
                    {
                        AssertionExpression translatedExpression = Dictionary.Translate(assertionExpression);
                        var translatedStatement = SyntaxFactory.ExpressionStatement(SyntaxFactory.ParseExpression(translatedExpression.Class + "." + translatedExpression.Method + "(" + translatedExpression.Arguments + ")")).WithTriviaFrom(expressionStatement);
                        methodDeclaration = methodDeclaration.ReplaceNode(expressionStatement, translatedStatement);
                    }
                    else
                    {
                        var commentedStatement = CommentExpressionStatement(expressionStatement);
                        methodDeclaration = methodDeclaration.ReplaceNode(expressionStatement, commentedStatement);
                    }
                }
                else
                    Logger.Info("is not assertion: " + Environment.NewLine + expressionStatement);
            }
            return methodDeclaration;
        }
        private static bool IsTranslatable(AssertionExpression assertionExpression)
        {
            return Dictionary.IsTranslatable(assertionExpression);
        }

        private static ExpressionStatementSyntax CommentExpressionStatement(ExpressionStatementSyntax expressionStatement)
        {

            string[] triviaList = expressionStatement.GetLeadingTrivia().ToFullString().Split(Environment.NewLine.ToCharArray());
            triviaList[triviaList.Length - 1] = "//" + triviaList[triviaList.Length - 1];

            string resultTrivia = "";
            for (int i = 0; i < triviaList.Length - 1; i++)
                resultTrivia += triviaList[i] + Environment.NewLine;
            resultTrivia += triviaList[triviaList.Length - 1];

            return expressionStatement.WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(resultTrivia));
        }
    }
}
