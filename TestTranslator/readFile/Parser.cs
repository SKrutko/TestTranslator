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
        private Dictionary<string, TokenType> keyWords;
        static Document document;

        private string usingDirective = "";
        private string namespaceName = "";

        private string errorMessage = "";

        public Parser()
        {
            state = ParserState.ExpectedUsingStatementOrNamespace;
            fillInDictionary();
            document = Program.GetDocument();
        }

        public void parse(string token)
        {
            try
            {
                
                analize(token);
            }
            catch (NullReferenceException ex)
            {

            }
            catch (Exception ex)
            {

            }
            
            //end();
        }
        public void analize(string nextToken)
        {
            switch (state)
            {
                case ParserState.ExpectedUsingStatementOrNamespace:
                    if(getTokenType(nextToken) == TokenType.Using)
                    {
                        state = ParserState.FoundUsingExpectedDirectoryName;
                        usingDirective = "";
                    }
                    else if (getTokenType(nextToken) == TokenType.Namespace)
                    {
                        state = ParserState.FoundNamespaceExpectedName;
                    }
                    break;

                case ParserState.FoundUsingExpectedDirectoryName:
                    if (nextToken != ";")
                        usingDirective += nextToken;
                    else
                    {
                        state = ParserState.ExpectedUsingStatementOrNamespace;
                        if (!usingDirective.Equals("NUnit.Framework"))
                            document.addUsingStatement(usingDirective);
                    }
                    break;

                case ParserState.FoundNamespaceExpectedName:
                    doIfFoundNamespaceExpectedName(nextToken);
                    break;
                case ParserState.ExpectedClass:
                    break;

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
        public void analize(List<string> listOfTokens)
        {
            
            if (state == ParserState.ExpectedUsingStatementOrNamespace)
            {
                if (getTokenType(listOfTokens.ElementAt(0)) == TokenType.Using)
                {
                    string directive = "";
                    for(int i = 1; i < listOfTokens.Count; i++)
                    {
                        string nextToken = listOfTokens.ElementAt(i);
                        if (nextToken != ";")
                            directive += nextToken;
                    }
                    if (!directive.Equals("NUnit.Framework"))
                        document.addUsingStatement(directive);
                }
                else  if(getTokenType(listOfTokens.ElementAt(0)) == TokenType.Namespace)
                {
                    state = ParserState.FoundNamespaceExpectedName;
                    if(listOfTokens.Count > 1)
                    {
                        string name = "";
                        for (int i = 1; i < listOfTokens.Count; i++)
                        {
                            string nextToken = listOfTokens.ElementAt(i);
                            if (nextToken != "{")
                                name += nextToken;
                            else
                                state = ParserState.ExpectedClass;
                        }
                        if (state != ParserState.ExpectedClass)
                            state = ParserState.FoundnamespaceNameExpectedLeftBrace;
                        document.addNamespaceStatement(name);
                    }
                   

                }
            }
        }
        /*private void changeStateToPhaseTwo()
        {
            //if in input file hasn't been found "using NUnit.Framework" then them input file is not NUnit test
            if (!document.foundUsingNUnitStatement())
                state = ParserState.Error;
            else
                state = ParserState.PhaseTwo;
        }*/


        public void end()
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();
            List <UsingStatement> usingStatements = document.getListOfUsingStatements();
            if (getState() != ParserState.Error)
            {
                for (int i = 0; i < usingStatements.Count; i++)
                {
                    formMain.print("using statement: " + usingStatements[i].getStatement());
                }
                formMain.print("namespace name:" + document.getNamespaceStatement());
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

        }

        private void changeState(ParserState newState)
        {
            state = newState;
        }

        public ParserState getState()
        {
            return state;
        }

    }
}
