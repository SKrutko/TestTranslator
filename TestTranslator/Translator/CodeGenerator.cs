using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    public class CodeGenerator
    {
        List<string> resultDocument;
        List<string> endOfDocument;

        bool isClassClosed = true;
        bool isMethodClosed = true;

        bool isClassCommented = false;
        bool isMethodCommented = false;

        string lineBeggining = "";

        List<UsingStatement> usingStatements;
        List<Class> classes;
        List<TestMethod> testMethods;
        List<OneLineComment> oneLineComments;
        List<string> codeLines;
        List<Assertion> assertions;


        public List<string> TranslateDocument(Document document)
        {
            resultDocument = new List<string>();
            endOfDocument = new List<string>();
            resultDocument.Add("using Microsoft.VisualStudio.TestTools.UnitTesting;");

            usingStatements = document.getListOfUsingStatements();
            classes = document.getListOfClasses();
            oneLineComments = document.getListOfOneLineComments();
            codeLines = document.getListOfCodeLines();
            testMethods = document.GetTestMethods();
            assertions = document.GetAssertions();


            foreach(documentUnit du in document.getDocumentStructure())
            {
                switch (du)
                {
                    case documentUnit.OneLineCommentAfterCode:
                        AddCommentAfterCode(oneLineComments.First().getMessage());
                        oneLineComments.RemoveAt(0);
                        break;

                    case documentUnit.OneLineComment:
                        //TODO
                        break;

                    case documentUnit.Using:
                        addUsingStatement();
                        break;

                    case documentUnit.Namespace:
                        AddToResultDocument("namespace " + document.getNamespaceStatement());
                        resultDocument.Add("{");
                        endOfDocument.Add("}");
                        break;

                    case documentUnit.ClassAttributeWithoutArgs:
                        AddClassAttributeWithoutArgs();
                        break;

                    case documentUnit.ClassAttributeWithArgs:
                        //TODO
                        break;

                    case documentUnit.ClassDeclaration:
                        lineBeggining = "    ";
                        AddRightBraceIfNeededAfterClass();
                        AddToResultDocument(CheckIfClassIsCommented() +"public class " + classes.ElementAt(0).getName());
                        AddToResultDocument(AddCommentIfNeeded(isClassCommented) + "{");
                        isClassClosed = false;
                        classes.RemoveAt(0);
                        lineBeggining = "        ";
                        break;

                    case documentUnit.CodeLineInClass:
                        
                        AddCodeLine();
                        break;

                    case documentUnit.TestAttributeWithoutArgs:
                        lineBeggining = "        ";
                        AddRightBraceIfNeededAfterMethod();
                        string tAttr = CheckIfMethodIsCommented() + "[" + testMethods.ElementAt(0).getListOfAttributes().ElementAt(0).getKeyWord() + "]";
                        testMethods.ElementAt(0).getListOfAttributes().RemoveAt(0);
                        AddToResultDocument(tAttr);
                        break;

                    case documentUnit.TestAttributeWithArgs:
                        //TODO
                        break;

                    case documentUnit.TestMethodDeclaration:
                        lineBeggining = "        ";
                        AddRightBraceIfNeededAfterMethod();
                        string declaration = CheckIfMethodIsCommented() +"public " + testMethods.ElementAt(0).GetReturnType() + " " +
                            testMethods.ElementAt(0).GetName() + "(" + testMethods.ElementAt(0).GetArgs() + ")";
                        AddToResultDocument(declaration);
                        AddToResultDocument(AddCommentIfNeeded(isMethodCommented) + "{");
                        isMethodClosed = false;
                        testMethods.RemoveAt(0);
                        lineBeggining = "            ";
                        break;

                    case documentUnit.Assertion:
                        AddToResultDocument(AddCommentIfNeeded(isMethodCommented) + assertions[0].GetLineToPrint());
                        assertions.RemoveAt(0);
                        break;

                    default:
                        break;
                }
            }
            lineBeggining = "        ";
            AddRightBraceIfNeededAfterMethod();
            lineBeggining = "    ";
            AddRightBraceIfNeededAfterClass();
            resultDocument.AddRange(endOfDocument);
            return resultDocument;
        }

        private void addUsingStatement()
        {
            AddToResultDocument("using " + usingStatements.First().getStatement() + ";");
            usingStatements.RemoveAt(0);
        }

        private void AddCommentAfterCode(string comment)
        {
            string lineWithComment =  resultDocument.Last() + " //" + comment;
            resultDocument.RemoveAt(resultDocument.Count() - 1);
            resultDocument.Add(lineWithComment);

        }

        public void AddClassAttributeWithoutArgs()
        {
            lineBeggining = "    "; //style
            AddRightBraceIfNeededAfterClass(); //add "}" after previous class if needed

            //form next line:
            string attr = "";
            attr += CheckIfClassIsCommented();
            attr += "[" + classes.ElementAt(0).getListOfAttributes().ElementAt(0).getKeyWord() + "]";
            
            classes.ElementAt(0).getListOfAttributes().RemoveAt(0);
            AddToResultDocument(attr);
        }
        private string CheckIfClassIsCommented()
        {
            string res = "";
            if (classes.ElementAt(0).IsCommented())
            {
                res += "//";
                isClassCommented = true;
            }
            else
                isClassCommented = false;
            return res;
        }
        private string CheckIfMethodIsCommented()
        {
            string res = "";
            if (isClassCommented || testMethods[0].isCommented())
            {
                res += "//";
                isMethodCommented = true;
            }
            else
                isMethodCommented = false;
            return res;
        }

        private string AddCommentIfNeeded(bool isCommented)
        {
            if (isCommented)
                return "//";
            return "";
        }

        private void AddRightBraceIfNeededAfterClass()
        {
            if(!isClassClosed)
            {
                isClassClosed = true;
                string line = "";
                if (isClassCommented)
                    line += "//";
                isClassCommented = false;
                AddToResultDocument(line + "}");
            }
        }

        private void AddRightBraceIfNeededAfterMethod()
        {
            if (!isMethodClosed)
            {
                string line = "";
                if (isClassCommented || isMethodCommented)
                    line += "//";
                isMethodClosed = true;
                AddToResultDocument(line + "}");
            }
        }

        private void AddCodeLine()
        {
            string line = "";
            if (isClassCommented || (!isMethodClosed && isMethodCommented))
                line = "//";
            AddToResultDocument(line + codeLines[0]);
            codeLines.RemoveAt(0);
        }

        public void Generate(Document document)
        {
            TranslateDocument(document);
            PrintResult(resultDocument);
        }

        private void PrintResult(List<string> result)
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();

            foreach (string line in result)
                formMain.print(line);
        }

        private void AddToResultDocument(string line)
        {
            resultDocument.Add(lineBeggining + line);
        }

    }
}
