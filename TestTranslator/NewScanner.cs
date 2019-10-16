using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestTranslator
{
    class NewScanner
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static AttributeSyntax TestClassAttributeSyntax = SyntaxFactory.Attribute(SyntaxFactory.ParseName("TestClass"));
        private static Regex commentRegex = new Regex(@"//.*");

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
            StreamReader sr = new StreamReader(fs);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sr.ReadToEnd());

            PrintBefore(tree.ToString());


            var compilation = CSharpCompilation.Create("TestTree")
                                              .AddReferences(
                                                   MetadataReference.CreateFromFile(
                                                       typeof(object).Assembly.Location))
                                              .AddSyntaxTrees(tree);

            Logger.Info("Compilation language: " + compilation.Language + " " + compilation.LanguageVersion);

            Logger.Info("--------------------------");

            var root = (CompilationUnitSyntax)tree.GetRoot();
            root = TranslateUsingDirective(root);

            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            CompilationUnitSyntax translatedRoot = root;
            ClassDeclarationSyntax classDeclarationSyntax;
            for(int i = 0; i < translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList().Count; i++)
            {
                classDeclarationSyntax = translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[i];

                if (IsTranslatable(classDeclarationSyntax))
                {
                    ClassDeclarationSyntax translated = TranslateClass(classDeclarationSyntax);

                    translatedRoot = translatedRoot.ReplaceNode(classDeclarationSyntax, translated);
                    Logger.Info("Root after translation: \n" + translatedRoot);
                }
                else
                {
                    //  var node = classDeclarationSyntax.WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia("*/" + classDeclarationSyntax.GetTrailingTrivia()));

                    //   node = node.WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(node.GetLeadingTrivia() + "/*"));
                    //   translatedRoot = translatedRoot.ReplaceNode(classDeclarationSyntax, node);
                    //translatedRoot = translatedRoot.InsertTriviaAfter(classDeclarationSyntax.GetTrailingTrivia()[classDeclarationSyntax.GetTrailingTrivia().Count - 1], SyntaxFactory.ParseTrailingTrivia("*/"));

                    var commented = Comment(classDeclarationSyntax);
                    translatedRoot = translatedRoot.ReplaceNode(translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[i], commented);
                    translatedRoot =  translatedRoot.RemoveNode(translatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[i], SyntaxRemoveOptions.KeepLeadingTrivia);
                    i--;
                }


            }
            Print(translatedRoot.ToFullString());
        }

        private CompilationUnitSyntax TranslateUsingDirective(CompilationUnitSyntax root)
        {
            foreach (var usingDirective in root.DescendantNodes().OfType<UsingDirectiveSyntax>().ToList())
                if (usingDirective.ToString().Equals("using NUnit.Framework;"))
                    return root.ReplaceNode(usingDirective, SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(" Microsoft.VisualStudio.TestTools.UnitTesting")));
            return root;
        }

        private ClassDeclarationSyntax TranslateClass(ClassDeclarationSyntax classDeclaration)
        {

                for (int i = 0; i < classDeclaration.AttributeLists.Count; i++)
                {
                    var attributeList = classDeclaration.AttributeLists.ToList()[i];
                    AttributeListSyntax newAttributeList = AttributeTranslator.TranslateClassAttributeList(attributeList);
                    classDeclaration = classDeclaration.ReplaceNode(attributeList, newAttributeList);
                }

            classDeclaration = AddClassAnnotationIfNotPresent(classDeclaration);
            classDeclaration = AddPublicModifierIfNotPresent(classDeclaration);
            
                //methods
            List<MethodDeclarationSyntax> methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

                for (int i = 0; i < classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().Count(); i++)
                {
                    methods[i] = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[i];
                    var methodDeclaration = methods[i];
                    if (IsTranslatable(methodDeclaration))
                    {
                        MethodDeclarationSyntax translatedMethod = TranslateMethod(methods[i]);
                        classDeclaration = classDeclaration.ReplaceNode(methods[i], translatedMethod);
                    }
                    else
                    {
                        classDeclaration = classDeclaration.ReplaceNode(methods[i], Comment(methodDeclaration));
                        classDeclaration = classDeclaration.RemoveNode(classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[i], SyntaxRemoveOptions.KeepExteriorTrivia);
                        i--;
                    }
                }
            return classDeclaration;
        }

        private ClassDeclarationSyntax AddClassAnnotationIfNotPresent(ClassDeclarationSyntax classDeclaration)
        {
            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString().Equals("TestClass"))
                        return classDeclaration;
                }

            }
            SeparatedSyntaxList<AttributeSyntax> list = new SeparatedSyntaxList<AttributeSyntax>();
            list = list.Add(TestClassAttributeSyntax);
            return classDeclaration.WithAttributeLists(classDeclaration.AttributeLists.Add(SyntaxFactory.AttributeList(list)));
        }

        private ClassDeclarationSyntax AddPublicModifierIfNotPresent(ClassDeclarationSyntax classDeclaration)
        {
            foreach(var modifier in classDeclaration.Modifiers.ToList())
            {
                if (modifier.ToString().Equals("public"))
                    return classDeclaration;
            }
            return classDeclaration.WithModifiers(classDeclaration.Modifiers.Add(SyntaxFactory.ParseToken("public")));
        }

            private SyntaxNode Comment(SyntaxNode node)
        {
            List<SyntaxTrivia> commentedSyntaxes = new List<SyntaxTrivia>();
            foreach (string line in node.ToFullString().Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                SyntaxTrivia syntaxTrivia;
                if (!IsCommented(line)) {
                    var comment = "//" + line + Environment.NewLine;
                    syntaxTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, comment);
                }
                else
                {
                    syntaxTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, line + Environment.NewLine);
                }
                commentedSyntaxes.Add(syntaxTrivia);
            }
            return node.WithLeadingTrivia(commentedSyntaxes);
        }

        private bool IsCommented(string line)
        {
            return line.Trim().StartsWith("//");
        }
        

        private MethodDeclarationSyntax TranslateMethod(MethodDeclarationSyntax methodDeclaration)
        {
            foreach (var log in methodDeclaration.DescendantTrivia().ToList())
               if(log.Kind() == SyntaxKind.SingleLineCommentTrivia) Logger.Info("Logs: \n" + log.ToString());
            
                for (int i = 0; i < methodDeclaration.AttributeLists.Count; i++)
                {
                    var attributeList = methodDeclaration.AttributeLists.ToList()[i];
                    AttributeListSyntax newAttributeList = AttributeTranslator.TranslateMethodAttributeList(attributeList);
                    methodDeclaration = methodDeclaration.ReplaceNode(attributeList, newAttributeList);
                }
               
            return AssertionTranslator.TranslateAssertions(methodDeclaration);
        }



        private bool IsTranslatable(MethodDeclarationSyntax methodDeclaration)
        {
            foreach(var attributeList in methodDeclaration.AttributeLists)
            {
                foreach(var attribute in attributeList.Attributes)
                {
                    if (!Dictionary.IsTranslatableMethodAttribute(attribute.Name.ToString()))
                        return false;
                }
            }
            return true;
        }

        private bool IsTranslatable(ClassDeclarationSyntax classDeclaration)
        {
            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (!Dictionary.IsTranslatableClassAttribute(attribute.Name.ToString()))
                        return false;
                }
            }
            return true;
        }

        private void Print(string result)
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();
            formMain.EnableButtonSave();

            formMain.print(result);
        }
        private void PrintBefore(string text)
        {
            MainFormController mfc = Program.getMainFormController();
            FormMain formMain = mfc.getForm();
            //formMain.EnableButtonSave();

            formMain.printBefore(text);
        }
    }
}
