using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class Parser
    {
        private ParserState state;
        private ParserState prevState;
        private Dictionary<string, TokenType> keyWords;
        static Document document;

        private string usingDirective = "";
        private string namespaceName = "";

        private string errorMessage = "";
        private string oneLineComment = "";
        private List<string> multipleLineComment = new List<string>();

        private bool newLine = true;
        private bool commentIsAfterCode = false;

        private List<Attribute> listOfAttributes;
        private string lineOfCode = "";

        private TestMethod nextMethod;
        private string methodArgs = "";

       private Assertion newAssertion;
        private string assertionArgs;

        int leftParenthesises = 0;


        bool stringMode = false;

        public Parser()
        {
            state = ParserState.ExpectedUsingStatementOrNamespace;
            fillInDictionary();
            document = Program.GetDocument();

            listOfAttributes = new List<Attribute>();
        }

        public void parse(string token, bool space, bool endl)
        {
            try
            {
                analize(token, space, endl);
            }
            catch (NullReferenceException ex)
            {

            }
            catch (Exception ex)
            {

            }
            
            //end();
        }
        public void analize(string nextToken, bool space, bool endl)
        {
            if(getTokenType(nextToken) == TokenType.Comment && !stringMode)
            {
                prevState = state;
                if (nextToken.Equals("//"))
                {
                    changeState(ParserState.OneLineComment);
                    oneLineComment = "";
                    commentIsAfterCode = !newLine;
                }
                else // "/*"
                {
                    changeState(ParserState.MultipleLineComment);
                    oneLineComment = "";
                    commentIsAfterCode = !newLine;
                }
            }
            else switch (state)
            {
                case ParserState.OneLineComment:
                     doIfOneLineComment(nextToken, space, endl);
                     break;

               case ParserState.MultipleLineComment:
                    doIfMultipleLineComment(nextToken, space, endl);
                    break;

                    case ParserState.ExpectedUsingStatementOrNamespace:
                    doIfExpectedUsingStatementOrNamespace(nextToken);
                    break;

                case ParserState.FoundUsingExpectedDirectoryName:
                    doIfFoundUsingExpectedDirectoryName(nextToken);
                    break;

                case ParserState.FoundNamespaceExpectedName:
                    doIfFoundNamespaceExpectedName(nextToken);
                    break;

                case ParserState.ExpectedClass:
                    doIfExpectedClass(nextToken);
                    break;

                    case ParserState.ExpectedClassAttribute:
                        doIfExpectedClassAttribute(nextToken);
                        break;

                    case ParserState.FoundClassAttrExpectedRightBracketOrComma:
                        doIfFoundClassAttrExpectedRightBracketOrComma(nextToken);
                        break;

                    case ParserState.ExpectedCWClass:
                    doIfExpectedCWClass(nextToken);
                    break;

                case ParserState.FoundCWClassExpectedClassName:
                    doIfFoundCWClassExpectedClassName(nextToken);
                    break;

                    case ParserState.FoundClassNameExpectedLeftBrace:
                        doIfFoundClassNameExpectedLeftBrace(nextToken);
                        break;

                    case ParserState.ExpectedCodeLineOrTestMethod:
                        doIfExpectedCodeLineOrTestMethod(nextToken);
                        break;

                    case ParserState.ExpectedTestAttribute:
                        doIfExpectedTestAttribute(nextToken);
                        break;

                    case ParserState.FoundTestAttrExpectedRightBracketOrComma:
                        doIfFoundTestAttrExpectedRightBracketOrComma(nextToken);
                        break;

                    case ParserState.ExpectedTestMethod:
                        doIfExpectedTestMethod(nextToken);
                        break;

                    case ParserState.FoundCWPublicExpectedReturnType:
                        doIfFoundCWPublicExpectedReturnType(nextToken);
                        break;

                    case ParserState.FoundReturnTypeExpectedMethodName:
                        doIfFoundReturnTypeExpectedMethodName(nextToken);
                        break;

                    case ParserState.FoundMethodNameExpectedLeftParenthesis:
                        doIfFoundMethodNameExpectedLeftParenthesis(nextToken);
                            break;

                    case ParserState.FoundMethodDeclarationExpectedRightParenthesis:
                        doIfFoundMethodDeclarationExpectedRightParenthesis(nextToken);
                            break;

                    case ParserState.FoundMethodDeclarationExpectedLeftBrace:
                        doIfFoundMethodDeclarationExpectedLeftBrace(nextToken);
                        break;

                    case ParserState.ExpectedCodeLineOrAssertion:
                        doIfExpectedCodeLineOrAssertion(nextToken);
                        break;

                    case ParserState.FoundAssertionExpectedDot:
                        doIfFoundAssertionExpectedDot(nextToken);
                        break;

                    case ParserState.FoundAssertionExpectedMethod:
                        doIfFoundAssertionExpectedMethod(nextToken);
                       break;

                    
                    case ParserState.FoundAssertionExpectedLeftParenthesis:
                        doIfFoundAssertionExpectedLeftParenthesis(nextToken);
                        break;
                    case ParserState.FoundAssertionExpectedRightParenthesis:
                        doIfFoundAssertionExpectedRightParenthesis(nextToken, space);
                        break;
                    case ParserState.FoundAssertionExpectedEndl:
                        doIfFoundAssertionExpectedEndl(nextToken);
                        break;

                    case ParserState.ExpectedContinuationOfCode:
                        doIfExpectedContinuationOfCode(nextToken, space, endl);
                        break;

 
            }

            newLine = endl;
        }


        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void doIfOneLineComment(string nextToken, bool space, bool endl)
        {
            if (space)
                oneLineComment += " ";
            oneLineComment += nextToken;
            if(endl)
            {
                changeState(prevState);
                document.addComment(oneLineComment, commentIsAfterCode);
            }
        }

        private void doIfMultipleLineComment(string nextToken, bool space, bool endl)
        {
            if (nextToken.Equals("*/"))
            {
                    changeState(prevState);
                    document.addComment(multipleLineComment, commentIsAfterCode);
            }
            else
            {
                if (space)
                    oneLineComment += " ";
                oneLineComment += nextToken;
                if (endl)
                {
                    multipleLineComment.Add(oneLineComment);
                    oneLineComment = "";
                }
            }
        }
        private void doIfExpectedUsingStatementOrNamespace(string nextToken)
        {
            if (getTokenType(nextToken) == TokenType.Using)
            {
                state = ParserState.FoundUsingExpectedDirectoryName;
                usingDirective = "";
            }
            else if (getTokenType(nextToken) == TokenType.Namespace)
            {
                state = ParserState.FoundNamespaceExpectedName;
            }
        }
        private void doIfFoundUsingExpectedDirectoryName(string nextToken)
        {
            if (nextToken != ";")
                usingDirective += nextToken;
            else
            {
                state = ParserState.ExpectedUsingStatementOrNamespace;
                if (!usingDirective.Equals("NUnit.Framework"))
                    document.addUsingStatement(usingDirective);
            }
        }
        private void doIfFoundNamespaceExpectedName(string nextToken)
        {
            if (nextToken.Equals("{")) //the end of the namespace name
            {
                if (namespaceName.Equals(""))
                {
                    state = ParserState.Error;
                    errorMessage = "No namespace name given";
                }
                else
                {
                    state = ParserState.ExpectedClass;
                    document.addNamespaceStatement(namespaceName);
                }
            }
            else
            {
                namespaceName += nextToken;
            }
        }
        private void doIfExpectedClass(string nextToken) // expected "[" or "private"
        {
            switch (nextToken)
            {
                case "[":
                    changeState(ParserState.ExpectedClassAttribute);
                    break;
                case "public":
                    changeState(ParserState.ExpectedCWClass);
                    break;
                case "class":
                    changeState(ParserState.FoundCWClassExpectedClassName);
                    break;
                case "}":
                    break;
                default:
                    changeState(ParserState.Error);
                    errorMessage = "Unexpected input when expected class. Input: " + nextToken;
                    break;
            }
        }

        private void doIfExpectedClassAttribute(string nextToken) 
        {
            if(getTokenType(nextToken) == TokenType.ClassAttributeWithoutArg)
            {
                changeState(ParserState.FoundClassAttrExpectedRightBracketOrComma);
                listOfAttributes.Add(new Attribute(AttributeType.ClassAttribute, nextToken));
                document.addToStructure(documentUnit.ClassAttributeWithoutArgs);
            }
            else
            {
                changeState(ParserState.FoundClassAttrExpectedRightParenthesis);
                document.addToStructure(documentUnit.ClassAttributeWithArgs);
            }

            
        }

        private void doIfFoundClassAttrExpectedRightBracketOrComma(string nextToken)
        {
            if(nextToken.Equals("]"))
            {
                changeState(ParserState.ExpectedClass);
            }
            else if(nextToken.Equals(","))
            {
                changeState(ParserState.ExpectedClassAttribute);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "unexpected input when expected Right Bracket Or Comma";
            }
        }
        private void doIfExpectedCWClass(string nextToken)
        {
            if (nextToken.Equals("class"))
            {
                changeState(ParserState.FoundCWClassExpectedClassName);
            }
            else
            {
                changeState(ParserState.Error);
            }
        }

        private void doIfFoundCWClassExpectedClassName(string nextToken)
        {
            document.addClass(nextToken, listOfAttributes);
            changeState(ParserState.FoundClassNameExpectedLeftBrace);
        }

        private void doIfFoundClassNameExpectedLeftBrace(string nextToken)
        {
            if (nextToken.Equals("{"))
                changeState(ParserState.ExpectedCodeLineOrTestMethod);
            else
            {
                changeState(ParserState.Error);
                errorMessage = "expected left brace";
            }
        }

        private void doIfExpectedCodeLineOrTestMethod(string nextToken)
        {
            if(nextToken.Equals("["))
            {
                changeState(ParserState.ExpectedTestAttribute);
            }
            else if(!nextToken.Equals("}"))
            {
                lineOfCode = nextToken;
                prevState = ParserState.ExpectedCodeLineOrTestMethod;
                changeState(ParserState.ExpectedContinuationOfCode);
            }
            else
            {
                changeState(ParserState.ExpectedClass);
            }
        }

        private void doIfExpectedTestAttribute(string nextToken)
        {
            if (getTokenType(nextToken) == TokenType.TestAttributeWithoutArg)
            {
                changeState(ParserState.FoundTestAttrExpectedRightBracketOrComma);
                Attribute newAttr = new Attribute(AttributeType.TestAttribute, nextToken);
                listOfAttributes.Add(newAttr);
                document.addToStructure(documentUnit.TestAttributeWithoutArgs);
            }
            else
            {
                changeState(ParserState.FoundTestAttrExpectedRightParenthesis);
                document.addToStructure(documentUnit.TestAttributeWithArgs);
                //TODO?
            }
        }

        private void doIfFoundTestAttrExpectedRightBracketOrComma(string nextToken)
        {
            if (nextToken.Equals("]"))
            {
                changeState(ParserState.ExpectedTestMethod);
            }
            else if (nextToken.Equals(","))
            {
                changeState(ParserState.ExpectedTestAttribute);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "unexpected input when expected Right Bracket Or Comma";
            }
        }
        
        private void doIfExpectedTestMethod(string nextToken)
        {
            if (nextToken.Equals("public"))
            {
                changeState(ParserState.FoundCWPublicExpectedReturnType);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "expected code word public";
            }
        }

        
        private void doIfFoundCWPublicExpectedReturnType(string nextToken)
        {
            if (getTokenType(nextToken) == TokenType.ReturnType)
            {
               
                changeState(ParserState.FoundReturnTypeExpectedMethodName);
                nextMethod = new TestMethod(nextToken, listOfAttributes);
                listOfAttributes.Clear();
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "expected return type";
            }
        }

        private void doIfFoundReturnTypeExpectedMethodName(string nextToken)
        {
            nextMethod.SetName(nextToken);
            changeState(ParserState.FoundMethodNameExpectedLeftParenthesis);
        }

        private void doIfFoundMethodNameExpectedLeftParenthesis(string nextToken)
        {
            if(nextToken.Equals("("))
            {
                changeState(ParserState.FoundMethodDeclarationExpectedRightParenthesis);
            }
        }
        private void doIfFoundMethodDeclarationExpectedRightParenthesis(string nextToken)
        {
            if (nextToken.Equals(")"))
            {

                changeState(ParserState.FoundMethodDeclarationExpectedLeftBrace);
                nextMethod.AddArgs(methodArgs);
                methodArgs = "";
                document.AddTestMethod(nextMethod);
                
            }
            else
            {
                methodArgs += nextToken;
            }
        }
        private void doIfFoundMethodDeclarationExpectedLeftBrace(string nextToken)
        {
            if(nextToken == "{")
            {
                changeState(ParserState.ExpectedCodeLineOrAssertion);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "501";
            }
        }

        private void doIfExpectedCodeLineOrAssertion(string nextToken)
        {
            if(getTokenType(nextToken) == TokenType.Assertion)
            {
                changeState(ParserState.FoundAssertionExpectedDot);
                newAssertion = new Assertion(nextToken);
            }
            else if (!nextToken.Equals("}"))
            {
                prevState = ParserState.ExpectedCodeLineOrAssertion;
                changeState(ParserState.ExpectedContinuationOfCode);
                lineOfCode = nextToken;
            }
            else
            {
                changeState(ParserState.ExpectedCodeLineOrTestMethod);
            }
        }

        private void doIfFoundAssertionExpectedDot(string nextToken) //expected dot
        {
            if (nextToken.Equals("."))
            {
                changeState(ParserState.FoundAssertionExpectedMethod);
                assertionArgs = "";
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "expected . after Assertion, but found: " + nextToken;
            }
            
           
        }

        private void doIfFoundAssertionExpectedMethod(string nextToken)
        {
            newAssertion.SetMethod(nextToken);
            changeState(ParserState.FoundAssertionExpectedLeftParenthesis);
        }

        private void doIfFoundAssertionExpectedLeftParenthesis(string nextToken)
        {
            if (nextToken.Equals("("))
                changeState(ParserState.FoundAssertionExpectedRightParenthesis);
            else
            {
                changeState(ParserState.Error);
                errorMessage = "Expected ( after Assertion, but found: " + nextToken;
            }
        }
        private void doIfFoundAssertionExpectedRightParenthesis(string nextToken, bool space)
        {
            if (nextToken.Equals(")"))
            {
                if (leftParenthesises == 0)
                {
                    changeState(ParserState.FoundAssertionExpectedEndl);
                    newAssertion.SetArgs(assertionArgs);
                    document.AddAssertion(newAssertion);
                }
                else
                    leftParenthesises--;
            }
            else if(nextToken.Equals("("))
            {
                leftParenthesises++;
            }
            else
            {
                if (space)
                    assertionArgs += " ";
                assertionArgs += nextToken;

            }
        }
        private void doIfFoundAssertionExpectedEndl(string nextToken)
        {
            //TODO: stringMode?
            if (nextToken.Equals(";"))
                changeState(ParserState.ExpectedCodeLineOrAssertion);
            else
            {
                changeState(ParserState.Error);
                errorMessage = "Expected ; after Assertion, but found: " + nextToken;
            }
        }


        private void doIfExpectedContinuationOfCode(string nextToken, bool space, bool endl)
        {
            if (nextToken.Equals("\""))
                stringMode = !stringMode;

            if (space)
                lineOfCode += " ";
            lineOfCode += nextToken;
            if((nextToken.Equals(";") && !stringMode) || endl)
            {
                document.addCodeLine(lineOfCode);
                lineOfCode = "";
                changeState(prevState);
            }
        }

        public bool GetStringMode()
        {
            return stringMode;
        }

/// <summary>
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
        public void end()
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();

            /*List <UsingStatement> usingStatements = document.getListOfUsingStatements();
            List<OneLineComment> onelineComments = document.getListOfOneLineComments();
            List<Class> listOfClasses = document.getListOfClasses();
            List<string> listOfCodeLines = document.getListOfCodeLines();
            */
            if (getState() != ParserState.Error)
            {
                /* //for (int i = 0; i < u singStatements.Count; i++)
                 //{
                 //    formMain.print("using statement: " + usingStatements[i].getStatement());
                 //}
                 //formMain.print("namespace name:" + document.getNamespaceStatement());
                 string nextLine = "document structure size is " + document.getDocumentStructure().Count;
                 int i = 0, olci = 0, mlci = 0, codeLinesIndex = 0;

                 foreach (documentUnit du in document.getDocumentStructure())
                 {
                     formMain.print(du.ToString());
                 }
                 formMain.print("---end of stucture---");

                 foreach (documentUnit du in document.getDocumentStructure())
                 {
                     switch (du)
                     {
                         case documentUnit.Using:
                             formMain.print(nextLine);
                             nextLine = "using statement: " + usingStatements[i++].getStatement();
                             break;
                         case documentUnit.Namespace:
                             formMain.print(nextLine);
                             nextLine = "namespace: " + document.getNamespaceStatement();
                             break;
                         case documentUnit.OneLineCommentAfterCode:
                             nextLine += " //" + onelineComments[olci++].getMessage();
                             break;
                         case documentUnit.OneLineComment:
                             formMain.print(nextLine);
                             nextLine = " //" + onelineComments[olci++].getMessage();
                             break;
                         case documentUnit.MultipleLineComment:
                             formMain.print(nextLine);
                             nextLine = "/* " + document.getListOfMultipleLineComments()[mlci++];
                             for (int k = 0; k < document.getListOfMultipleLineComments()[mlci].getMessage().Count; k++)
                             {
                                 formMain.print(nextLine);
                                 nextLine = document.getListOfMultipleLineComments()[mlci].getMessage()[k];
                             }
                            */// nextLine += " */";
                              /* break;
                           case documentUnit.MultipleLineCommentAfterCode:
                               //formMain.print(nextLine);
                               nextLine += " /* " + document.getListOfMultipleLineComments()[mlci++];
                               for (int k = 0; k < document.getListOfMultipleLineComments()[mlci].getMessage().Count; k++)
                               {
                                   formMain.print(nextLine);
                                   nextLine = document.getListOfMultipleLineComments()[mlci].getMessage()[k];
                               }
                            *///   nextLine += " */";
                              /* break;
                           case documentUnit.CodeLine:
                               formMain.print(nextLine);
                               nextLine = listOfCodeLines[codeLinesIndex++];
                               break;
                           default:
                               formMain.print(nextLine);
                               nextLine += "other";
                               break;

                       }

                   }
                   formMain.print(nextLine);*/
                CodeGenerator cg = new CodeGenerator();
                cg.Generate(document);

            }
            else
            {
                formMain.print("error in syntax. Are you sure you have loaded NUnit test?");
                formMain.print("error message: " + errorMessage);
            }
        
        }

        public TokenType getTokenType(string token)
        {
            TokenType tokenType;
            keyWords.TryGetValue(token, out tokenType);
            if (keyWords.ContainsKey(token))
                return tokenType;
            return TokenType.NonToken;
        }

        private void fillInDictionary()
        {
            keyWords = new Dictionary<string, TokenType>();
            keyWords.Add("using", TokenType.Using);
            keyWords.Add("namespace", TokenType.Namespace);
            keyWords.Add("//", TokenType.Comment);
            keyWords.Add("/*", TokenType.Comment);

            keyWords.Add("TestFixture", TokenType.ClassAttributeWithoutArg);
            keyWords.Add("SetUpFixture", TokenType.ClassAttributeWithoutArg);

            keyWords.Add("Parallelizable", TokenType.AttributeWithoutArg);
            keyWords.Add("RequiresThread", TokenType.AttributeWithoutArg);
            keyWords.Add("Culture", TokenType.AttributeWithoutArg);
            keyWords.Add("Explicit", TokenType.AttributeWithoutArg);
            keyWords.Add("NonParallelizable", TokenType.AttributeWithoutArg);
            keyWords.Add("SingleThreaded", TokenType.AttributeWithoutArg);

            keyWords.Add("SetUp", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Test", TokenType.TestAttributeWithoutArg);
            keyWords.Add("TearDown", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Combinatorial", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Pairwise", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Sequential", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Theory", TokenType.TestAttributeWithoutArg);

            keyWords.Add("void", TokenType.ReturnType);
            keyWords.Add("int", TokenType.ReturnType);

            keyWords.Add("Assert", TokenType.Assertion);
            keyWords.Add("CollectionAssert", TokenType.Assertion);


        }

        public void changeState(ParserState newState)
        {
            state = newState;
        }

        public ParserState getState()
        {
            return state;
        }
        /*public void setPrevState(ParserState prev)
        {
            prevState = prev;
        }*/

    }
}
