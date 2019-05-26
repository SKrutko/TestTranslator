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
        //TODO: delete tmp
        //temporary
        List<string> tokens;
        List<bool> spaces;
        List<bool> endlines;
        //temporary


        private ParserState state;
        private ParserState prevState;
        private Dictionary<string, TokenType> keyWords;
        static Document document;

        private bool foundNUnit = false;

        private string usingDirective = "";
        private string namespaceName = "";

        private string errorMessage = "";
        private string oneLineComment = "";
        private List<string> multipleLineComment = new List<string>();

        private bool newLine = true;
        private bool commentIsAfterCode = false;

        private List<Attribute> listOfAttributes;
        private string attributeArgs = "";
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

            tokens = new List<string>();
            spaces = new List<bool>();
            endlines = new List<bool>();
        }

        public void parse(string token, bool space, bool endl)
        {
            tokens.Add(token);
            spaces.Add(space);
            endlines.Add(endl);
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
                    multipleLineComment = new List<string>();
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

                    case ParserState.FoundClassAttrExpectedLeftParenthesis:
                        doIfFoundClassAttrExpectedLeftParenthesis(nextToken);
                        break;

                    case ParserState.FoundClassAttrExpectedRightParenthesis:
                        doIfFoundAttrWithArgsExpectedRightParenthesis(nextToken, space);
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

                    case ParserState.FoundTestAttrExpectedLeftParenthesis:
                        doIfFoundTestAttrExpectedLeftParenthesis(nextToken);
                        break;

                    case ParserState.FoundTestAttrExpectedRightParenthesis:
                        doIfFoundAttrWithArgsExpectedRightParenthesis(nextToken, space);
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
                multipleLineComment.Add(oneLineComment);
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
                if(!foundNUnit)
                {
                    changeState(ParserState.Error);
                    errorMessage = "Statement \"using NUnit.Framework;\" was not found.";
                }
                else
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
                else
                    foundNUnit = true;
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
            if(getTokenType(nextToken) == TokenType.ClassAttributeWithoutArg || getTokenType(nextToken) == TokenType.AttributeWithoutArg)
            {
                changeState(ParserState.FoundClassAttrExpectedRightBracketOrComma);
                if (!nextToken.Equals("TestFixture"))
                {
                    listOfAttributes.Add(new Attribute(AttributeType.ClassAttribute, nextToken));
                    document.addToStructure(documentUnit.ClassAttributeWithoutArgs);
                }
            }
            else if (getTokenType(nextToken) == TokenType.ClassAttributeWithArg || getTokenType(nextToken) == TokenType.AttributeWithArg)
            {
                changeState(ParserState.FoundClassAttrExpectedLeftParenthesis);
                listOfAttributes.Add(new Attribute(AttributeType.ClassAttribute, nextToken));
                document.addToStructure(documentUnit.ClassAttributeWithArgs);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "Unknown class attribute: " + nextToken;
            }
        }

        private void doIfFoundClassAttrExpectedLeftParenthesis(string nextToken)
        {
            if(nextToken.Equals("("))
            {
                prevState = ParserState.FoundClassAttrExpectedRightBracketOrComma;
                changeState(ParserState.FoundClassAttrExpectedRightParenthesis);
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "Expected left parenthesis after class attribute but found " + nextToken;
            }
        }

        private void doIfFoundAttrWithArgsExpectedRightParenthesis(string nextToken, bool space)
        {
            if(nextToken.Equals(")"))
            {
                if (leftParenthesises == 0)
                {
                    changeState(prevState);
                    listOfAttributes[listOfAttributes.Count - 1].SetArguments(attributeArgs);
                    attributeArgs = "";
                }
                else {
                    leftParenthesises--;
                    if (space)
                        attributeArgs += " ";
                    attributeArgs += nextToken;
                }
            }
           else
            {
                if (nextToken.Equals("("))
                {
                    leftParenthesises++;
                }
                if (space)
                    attributeArgs += " ";
                attributeArgs += nextToken;
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
            listOfAttributes = new List<Attribute>();
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
            if (getTokenType(nextToken) == TokenType.TestAttributeWithoutArg || getTokenType(nextToken) == TokenType.AttributeWithoutArg)
            {
                changeState(ParserState.FoundTestAttrExpectedRightBracketOrComma);
                Attribute newAttr = new Attribute(AttributeType.TestAttribute, nextToken);
                listOfAttributes.Add(newAttr);
                document.addToStructure(documentUnit.TestAttributeWithoutArgs);
            }
            else if (getTokenType(nextToken) == TokenType.TestAttributeWithArg || getTokenType(nextToken) == TokenType.AttributeWithArg)
            {
                changeState(ParserState.FoundTestAttrExpectedLeftParenthesis);
                document.addToStructure(documentUnit.TestAttributeWithArgs);
                listOfAttributes.Add(new Attribute(AttributeType.TestAttribute, nextToken));
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = " Unknown test attribute: " + nextToken;
            }
        }

        private void doIfFoundTestAttrExpectedLeftParenthesis(string nextToken)
        {
            if (nextToken.Equals("("))
            {
                prevState = ParserState.FoundTestAttrExpectedRightBracketOrComma;
                changeState(ParserState.FoundTestAttrExpectedRightParenthesis);
                leftParenthesises = 0;
            }
            else
            {
                changeState(ParserState.Error);
                errorMessage = "Expected left parenthesis after test attribute but found: " + nextToken;
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
            else if(nextToken.Equals("["))
            {
                changeState(ParserState.ExpectedTestAttribute);
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
                listOfAttributes = new List<Attribute>();
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
                {
                    leftParenthesises--;
                    assertionArgs += ")";
                }
            }
            else if(nextToken.Equals("("))
            {
                leftParenthesises++;
                assertionArgs += "(";
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
               /* for(int i = 0; i < tokens.Count; i++)
                {
                    formMain.print(tokens[i] + ", " + spaces[i] + ", " + endlines[i]);
                }*/
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

            //keyWords.Add("", TokenType.ClassAttributeWithArg);

            keyWords.Add("Parallelizable", TokenType.AttributeWithoutArg);
            keyWords.Add("RequiresThread", TokenType.AttributeWithoutArg);
            keyWords.Add("Culture", TokenType.AttributeWithoutArg);
            keyWords.Add("Explicit", TokenType.AttributeWithoutArg);
            keyWords.Add("NonParallelizable", TokenType.AttributeWithoutArg);
            keyWords.Add("SingleThreaded", TokenType.AttributeWithoutArg);

            keyWords.Add("Author", TokenType.AttributeWithArg);
            keyWords.Add("Description", TokenType.AttributeWithArg);


            keyWords.Add("SetUp", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Test", TokenType.TestAttributeWithoutArg);
            keyWords.Add("TearDown", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Combinatorial", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Pairwise", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Sequential", TokenType.TestAttributeWithoutArg);
            keyWords.Add("Theory", TokenType.TestAttributeWithoutArg);

            keyWords.Add("Retry", TokenType.TestAttributeWithArg);

            keyWords.Add("void", TokenType.ReturnType);
            keyWords.Add("int", TokenType.ReturnType);

            keyWords.Add("Assert", TokenType.Assertion);
            keyWords.Add("CollectionAssert", TokenType.Assertion);
            keyWords.Add("StringAssert", TokenType.Assertion);
            keyWords.Add("FileAssert", TokenType.Assertion);
            keyWords.Add("DirectoryAssert", TokenType.Assertion);


        }

        public void changeState(ParserState newState)
        {
            state = newState;
        }

        public ParserState getState()
        {
            return state;
        }
        
        public void FoundNUnit()
        {
            foundNUnit = true;
        }

    }
}
