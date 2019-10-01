using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    class NewScanner
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        FileStream fs;
        Dictionary Dictionary = new Dictionary();

        public NewScanner()
        {
        }
        public NewScanner(FileStream fs)
        {
            this.fs = fs;
        }

        public void scan()
        {
        
            Logger.Info("Welcome to new scanner!");

            StreamReader sr = new StreamReader(fs);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sr.ReadToEnd());


            var compilation = CSharpCompilation.Create("TestTree")
                                              .AddReferences(
                                                   MetadataReference.CreateFromFile(
                                                       typeof(object).Assembly.Location))
                                              .AddSyntaxTrees(tree);

            Logger.Info("Compilation language: " + compilation.Language + " " + compilation.LanguageVersion);

            Logger.Info("--------------------------");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            Logger.Info("Comments: " + root.DescendantTrivia().OfType<DocumentationCommentTriviaSyntax>().ToList().Count);


            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            CompilationUnitSyntax translatedRoot = root;
            ClassDeclarationSyntax classDeclarationSyntax;
            for(int i = 0; i < translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList().Count; i++)
            {
                classDeclarationSyntax = translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[i];
                //Logger.Info("Before: \n" + classDeclarationSyntax.ToFullString());
                ClassDeclarationSyntax translated = TranslateClass(classDeclarationSyntax);
               // Logger.Info("After: \n" + translated.ToFullString());

                translatedRoot = translatedRoot.ReplaceNode(classDeclarationSyntax,translated);
                Logger.Info("Root after translation: \n" + translatedRoot);

            }
            PrintResult(translatedRoot.ToFullString());
        }

        private ClassDeclarationSyntax TranslateClass(ClassDeclarationSyntax classDeclaration)
        {
            List<MethodDeclarationSyntax> methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            for (int i = 0; i < classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().Count(); i++)
            {
                methods[i] = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[i];
                var methodDeclaration = methods[i];
                Logger.Info("descal " + i + " \n" + methodDeclaration);
                if (IsSimplyTranslatable(methodDeclaration))
                {
                    Logger.Info("translated");

                    MethodDeclarationSyntax translatedMethod = TranslateMethod(methods[i]);
                    classDeclaration = classDeclaration.ReplaceNode(methods[i], translatedMethod);
                } else
                {
                    Logger.Info("translated");

                    classDeclaration = classDeclaration.ReplaceNode(methods[i], CommentMethod(methodDeclaration));
                    classDeclaration = classDeclaration.RemoveNode(classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[i], SyntaxRemoveOptions.KeepExteriorTrivia);
                    i --;
                }
            }
            return classDeclaration;
        }

        private MethodDeclarationSyntax CommentMethod(MethodDeclarationSyntax methodDeclaration)
        {
            List<SyntaxTrivia> commentedSyntaxes = new List<SyntaxTrivia>();
            foreach (string line in methodDeclaration.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var comment = "//" + line + Environment.NewLine;
                var syntaxTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, comment);
                commentedSyntaxes.Add(syntaxTrivia);
            }
            return methodDeclaration.InsertTriviaAfter(methodDeclaration.DescendantTrivia().ToList()[0], commentedSyntaxes);
        }
        

        private MethodDeclarationSyntax TranslateMethod(MethodDeclarationSyntax methodDeclaration)
        {
            foreach (var log in methodDeclaration.DescendantTrivia().ToList())
               if(log.Kind() == SyntaxKind.SingleLineCommentTrivia) Logger.Info("Logs: \n" + log.ToString());
            
                for (int i = 0; i < methodDeclaration.AttributeLists.Count; i++)
                {

                    var attributeList = methodDeclaration.AttributeLists.ToList()[i];
                    AttributeListSyntax newAttributeList = TranslateAttributeList(attributeList);
                    methodDeclaration = methodDeclaration.ReplaceNode(attributeList, newAttributeList);
                }
         
               
            return methodDeclaration;
        }

        private bool IsSimplyTranslatable(MethodDeclarationSyntax methodDeclaration)
        {
            foreach(var attributeList in methodDeclaration.AttributeLists)
            {
                foreach(var attribute in attributeList.Attributes)
                {
                    if (!Dictionary.IsSimplyTranslatableAttribute(attribute.Name.ToString()))
                        return false;
                }
            }
            return true;
        }

        private AttributeListSyntax TranslateAttributeList(AttributeListSyntax attributeList)
        {
            for (int i = 0; i < attributeList.Attributes.Count; i++)
            {
                var attribute = attributeList.Attributes.ToList()[i];
                AttributeSyntax translatedAttribute = TranslateAttribute(attribute);
                attributeList = attributeList.ReplaceNode(attribute, translatedAttribute);
            }
            return attributeList;
        }

        private AttributeSyntax TranslateAttribute(AttributeSyntax attribute)
        {
            string attributeName = Dictionary.TranslateAttributeName(attribute.Name.ToString());          
            return attribute.WithName(SyntaxFactory.IdentifierName(attributeName));
        }

        private void PrintResult(string result)
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();
            formMain.EnableButtonSave();

            formMain.print(result);
        }
    }
}
