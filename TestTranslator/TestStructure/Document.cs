using System.Collections.Generic;

namespace TestTranslator
{
    public class Document
    {
        private List<UsingStatement> usingStatements;

        private NamespaseStatement namespaseStatement;

        private List<Class> listOfClasses;

        private List<documentUnit> documentStructure;

        private List<OneLineComment> oneLineComments;
        private List<MultipleLineComment> multipleLineComments;


        public Document()
        {
            usingStatements = new List<UsingStatement>();
            //namespaseStatement = new NamespaseStatement("");
            listOfClasses = new List<Class>();
            documentStructure = new List<documentUnit>();

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

        public void addClass(string className)
        {
            Class newClass = new Class(className);
            listOfClasses.Add(newClass);
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
    }
}