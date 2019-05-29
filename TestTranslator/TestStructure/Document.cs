using System.Collections.Generic;

namespace TestTranslator
{
    public class Document
    {
        private List<UsingStatement> usingStatements;

        private NamespaseStatement namespaseStatement;

        private List<Class> listOfClasses;
        private List<TestMethod> testMethods;

        private List<documentUnit> documentStructure;

        private List<OneLineComment> oneLineComments;
        private List<MultipleLineComment> multipleLineComments;

        private List<string> listOfCodeLines;
        private List<Assertion> assertions;


        public Document()
        {
            usingStatements = new List<UsingStatement>();
            namespaseStatement = new NamespaseStatement("");
            listOfClasses = new List<Class>();
            documentStructure = new List<documentUnit>();
            listOfCodeLines = new List<string>();
            testMethods = new List<TestMethod>();
            assertions = new List<Assertion>();

            oneLineComments = new List<OneLineComment>();
            multipleLineComments = new List<MultipleLineComment>();
        }

        public void addUsingStatement(string statement)
        {
            UsingStatement usingStatement = new UsingStatement(statement);
            usingStatements.Add(usingStatement);
            documentStructure.Add(documentUnit.Using);
        }

        public List<UsingStatement> getListOfUsingStatements()
        {
            return usingStatements;
        }

        public void addNamespaceStatement(string name)
        {
            namespaseStatement = new NamespaseStatement(name);
            documentStructure.Add(documentUnit.Namespace);
        }
        public string getNamespaceStatement()
        {
            return namespaseStatement.getName();
        }

        public List<Class> getListOfClasses()
        {
            return listOfClasses;
        }

        public void addClass(string className, List<Attribute> listOfAttributes)
        {
            Class newClass = new Class(className, listOfAttributes);
            listOfClasses.Add(newClass);
            documentStructure.Add(documentUnit.ClassDeclaration);
        }

        public void addComment(string comment, bool isAfterCode)
        {
            OneLineComment newComment = new OneLineComment(comment);
            oneLineComments.Add(newComment);
            if (isAfterCode)
                documentStructure.Add(documentUnit.OneLineCommentAfterCode);
            else
                documentStructure.Add(documentUnit.OneLineComment);
        }
        public void addComment(List<string> comment, bool isAfterCode)
        {
            MultipleLineComment newComment = new MultipleLineComment(comment);
            multipleLineComments.Add(newComment);
            if (isAfterCode)
                documentStructure.Add(documentUnit.MultipleLineCommentAfterCode);
            else
                documentStructure.Add(documentUnit.MultipleLineComment);
        }

        public List<documentUnit> getDocumentStructure()
        {
            return documentStructure;
        }

        public List<OneLineComment> getListOfOneLineComments()
        {
            return oneLineComments;
        }
        public List<MultipleLineComment> getListOfMultipleLineComments()
        {
            return multipleLineComments;
        }

        public void addCodeLine(string line)
        {
            listOfCodeLines.Add(line);
            documentStructure.Add(documentUnit.CodeLine);
        }

        public List<string> getListOfCodeLines()
        {
            return listOfCodeLines;
        }

        public void addToStructure(documentUnit unit)
        {
            documentStructure.Add(unit);
        }

        public void AddTestMethod(TestMethod tm)
        {
            testMethods.Add(tm);
            addToStructure(documentUnit.TestMethodDeclaration);
        }

        public List<TestMethod> GetTestMethods()
        {
            return testMethods;
        }

        public void AddAssertion(Assertion assertion)
        {
            addToStructure(documentUnit.Assertion);
            assertions.Add(assertion);
            if (!assertion.IsTranslatable())
                testMethods[testMethods.Count - 1].ToComment();
        }
        public List<Assertion> GetAssertions()
        {
            return assertions;
        }

    }


}