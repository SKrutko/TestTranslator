using System.Collections.Generic;

namespace TestTranslator
{
    public class Document
    {
        private bool hasUsingNUnitStatement;
        private List<UsingStatement> usingStatements;

        private NamespaseStatement namespaseStatement;


        public Document()
        {
            usingStatements = new List<UsingStatement>();
            hasUsingNUnitStatement = false;
            //namespaseStatement = new NamespaseStatement("");
        }

        public void UsingNUnitStatement()
        {
            hasUsingNUnitStatement = true;
        }

        public bool foundUsingNUnitStatement()
        {
            return hasUsingNUnitStatement;
        }


        public void addUsingStatement(string statement)
        {
            UsingStatement usingStatement = new UsingStatement(statement);
            usingStatements.Add(usingStatement);
        }

        public List<UsingStatement> getListOfUsingStatements()
        {
            return usingStatements;
        }

        public void addNamespaceStatement(string name)
        {
            namespaseStatement = new NamespaseStatement(name);
        }
        public string getNamespaceStatement()
        {
            return namespaseStatement.getName();
        }
    }
}