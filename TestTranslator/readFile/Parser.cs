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

        public Parser()
        {
            state = ParserState.ExpectedUsingStatementOrNamespace;
            fillInDictionary();
            document = Program.GetDocument();
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
            if(getTokenType(nextToken) == TokenType.Comment)
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

                case ParserState.ExpectedCWClass:
                    doIfExpectedCWClass(nextToken);
                    break;

                case ParserState.FoundCWClassExpectedClassName:
                    doIfFoundCWClassExpectedClassName(nextToken);
                    break;

            }

            newLine = endl;
        }
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
                    //TODO: attribute
                    break;
                case "public":
                    changeState(ParserState.ExpectedCWClass);
                    break;
                case "class":
                    changeState(ParserState.FoundCWClassExpectedClassName);
                    break;
                default:
                    //TODO : mistake?
                    break;
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
            document.addClass(nextToken);
            changeState(ParserState.FoundClassNameExpectedLeftBrace);
        }
       
        public void end()
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();
            List <UsingStatement> usingStatements = document.getListOfUsingStatements();
            List<OneLineComment> onelineComments = document.getListOfOneLineComments();
            if (getState() != ParserState.Error)
            {
                //for (int i = 0; i < usingStatements.Count; i++)
                //{
                //    formMain.print("using statement: " + usingStatements[i].getStatement());
                //}
                //formMain.print("namespace name:" + document.getNamespaceStatement());
                string nextLine = "document structure size is " + document.getDocumentStructure().Count;
                int i = 0, olci = 0, mlci = 0;
            foreach(documentUnit du in document.getDocumentStructure())
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
                            nextLine += " */";
                            break;
                        case documentUnit.MultipleLineCommentAfterCode:
                            //formMain.print(nextLine);
                            nextLine += " /* " + document.getListOfMultipleLineComments()[mlci++];
                            for (int k = 0; k < document.getListOfMultipleLineComments()[mlci].getMessage().Count; k++)
                            {
                                formMain.print(nextLine);
                                nextLine = document.getListOfMultipleLineComments()[mlci].getMessage()[k];
                            }
                            nextLine += " */";
                            break;
                        default:
                            formMain.print(nextLine);
                            nextLine += "other";
                            break;

                    }
                    
                }
                formMain.print(nextLine);
            }
            else
                formMain.print("error in syntax. Are you sure you have loaded NUnit test?");
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
            keyWords.Add("Test", TokenType.Attribute);
            keyWords.Add("//", TokenType.Comment);
            keyWords.Add("/*", TokenType.Comment);
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
