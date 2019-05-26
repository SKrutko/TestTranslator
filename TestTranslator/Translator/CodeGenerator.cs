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

        bool addTestClassAttribute = true;

        bool isClassClosed = true;
        bool isMethodClosed = true;

        bool isClassCommented = false;
        bool isMethodCommented = false;

        string lineBeggining = "";

        List<UsingStatement> usingStatements;
        List<Class> classes;
        List<TestMethod> testMethods;
        List<OneLineComment> oneLineComments;
        List<MultipleLineComment> multipleLineComments;
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
            multipleLineComments = document.getListOfMultipleLineComments();
            codeLines = document.getListOfCodeLines();
            testMethods = document.GetTestMethods();
            assertions = document.GetAssertions();

            //TODO: delete tmp
            /*foreach (Class c  in classes)
            {
                foreach(Attribute a in c.getListOfAttributes())
                    AddToResultDocument(a.getKeyWord() + " (" + a.GetArguments() + ")-c");
                AddToResultDocument("------------------------");
            }

            foreach (TestMethod tm in testMethods)
            {
                foreach (Attribute a in tm.getListOfAttributes())
                    AddToResultDocument(a.getKeyWord() + " (" + a.GetArguments() + ")-tm");
                AddToResultDocument("------------------------");
            }*/

                foreach (documentUnit du in document.getDocumentStructure())
            {
                switch (du)
                {
                    case documentUnit.OneLineCommentAfterCode:
                        AddCommentAfterCode("//", oneLineComments.First().getMessage());
                        oneLineComments.RemoveAt(0);
                        break;

                    case documentUnit.OneLineComment:
                        AddToResultDocument("//" + oneLineComments.First().getMessage());
                        oneLineComments.RemoveAt(0);
                        break;

                    case documentUnit.MultipleLineCommentAfterCode:
                        List<string> commentAfterCode = multipleLineComments[0].getMessage();
                        if(commentAfterCode.Count == 1)
                            AddCommentAfterCode("/*", commentAfterCode[0] + "*/");
                        else
                        {
                            AddCommentAfterCode("/*", commentAfterCode[0]);
                            for (int i = 1; i < commentAfterCode.Count - 1; i++)
                                AddToResultDocument(commentAfterCode[i]);
                            AddToResultDocument(commentAfterCode[commentAfterCode.Count - 1] + "*/");
                        }
                        multipleLineComments.RemoveAt(0);
                        break;

                    case documentUnit.MultipleLineComment:
                        List<string> comment = multipleLineComments[0].getMessage();
                        if (comment.Count == 1)
                            AddToResultDocument("/*" + comment[0] + "*/");
                        else
                        {
                            AddToResultDocument("/*" + comment[0]);
                            for (int i = 1; i < comment.Count - 1; i++)
                                AddToResultDocument(comment[i]);
                            AddToResultDocument(comment[comment.Count - 1] + "*/");
                        }
                        multipleLineComments.RemoveAt(0);
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
                        AddRightBraceIfNeededAfterMethod();
                        AddRightBraceIfNeededAfterClass(); //add "}" after previous class if needed
                        AddTestClassAttributeIfNeeded();
                        AddClassAttributeWithoutArgs();
                        break;

                    case documentUnit.ClassAttributeWithArgs:
                        AddRightBraceIfNeededAfterMethod();
                        AddRightBraceIfNeededAfterClass(); //add "}" after previous class if needed
                        AddTestClassAttributeIfNeeded();
                        AddClassAttributeWithArgs();
                        break;

                    case documentUnit.ClassDeclaration:
                        AddRightBraceIfNeededAfterMethod();
                        AddRightBraceIfNeededAfterClass();
                        AddTestClassAttributeIfNeeded();
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
                        AddRightBraceIfNeededAfterMethod();
                        string attrWithArg = CheckIfMethodIsCommented() + "[" + testMethods.ElementAt(0).getListOfAttributes().ElementAt(0).getKeyWord();
                        attrWithArg += "(" + testMethods.ElementAt(0).getListOfAttributes().ElementAt(0).GetArguments() + ")";
                        attrWithArg += "]";
                        testMethods.ElementAt(0).getListOfAttributes().RemoveAt(0);
                        AddToResultDocument(attrWithArg);
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

        private void AddCommentAfterCode(string type, string comment)
        {
            string lineWithComment =  resultDocument.Last() + " " + type + comment;
            resultDocument.RemoveAt(resultDocument.Count() - 1);
            resultDocument.Add(lineWithComment);

        }

        private void AddTestClassAttributeIfNeeded()
        {
            if(addTestClassAttribute)
            {
                AddToResultDocument(CheckIfClassIsCommented() + "[TestClass]");
                addTestClassAttribute = false;
            }
        }

        public void AddClassAttributeWithoutArgs()
        {
            lineBeggining = "    "; //style
           // AddRightBraceIfNeededAfterClass(); //add "}" after previous class if needed

            //form next line:
            string attr = "";
            attr += CheckIfClassIsCommented();
            attr += "[" + classes.ElementAt(0).getListOfAttributes().ElementAt(0).getKeyWord() + "]";
            
            classes.ElementAt(0).getListOfAttributes().RemoveAt(0);
            AddToResultDocument(attr);
        }

        public void AddClassAttributeWithArgs()
        {
            try
            {
                string attr = "";
                attr += CheckIfClassIsCommented();
                attr += "[" + classes.ElementAt(0).getListOfAttributes().ElementAt(0).getKeyWord(); //attribute name
                attr += "(" + classes.ElementAt(0).getListOfAttributes().ElementAt(0).GetArguments() + ")"; //attribute args
                attr += "]";

                classes.ElementAt(0).getListOfAttributes().RemoveAt(0);
                AddToResultDocument(attr);
            }
            catch(Exception e)
            {

            }
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
            lineBeggining = "    ";
            if (!isClassClosed)
            {
                isClassClosed = true;
                addTestClassAttribute = true;
                string line = "";
                if (isClassCommented)
                    line += "//";
                isClassCommented = false;
                AddToResultDocument(line + "}");
            }
        }

        private void AddRightBraceIfNeededAfterMethod()
        {
            lineBeggining = "        ";
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
            formMain.EnableButtonSave();
            formMain.SetResultList(result);

            foreach (string line in result)
                formMain.print(line);
        }

        private void AddToResultDocument(string line)
        {
            resultDocument.Add(lineBeggining + line);
        }

    }
}
