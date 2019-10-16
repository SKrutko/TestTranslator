using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranslator
{
    class AttributeTranslator
{
        private static Dictionary Dictionary = new Dictionary();
     
        public static AttributeListSyntax TranslateClassAttributeList(AttributeListSyntax attributeList)
        {
            for (int i = 0; i < attributeList.Attributes.Count; i++)
            {
                var attribute = attributeList.Attributes.ToList()[i];
                AttributeSyntax translatedAttribute = TranslateAttribute(attribute);
                attributeList = attributeList.ReplaceNode(attribute, translatedAttribute);
            }
            return attributeList;
        }

        public static AttributeListSyntax TranslateMethodAttributeList(AttributeListSyntax attributeList)
        {
            for (int i = 0; i < attributeList.Attributes.Count; i++)
            {
                var attribute = attributeList.Attributes.ToList()[i];
                AttributeSyntax translatedAttribute = TranslateAttribute(attribute);
                attributeList = attributeList.ReplaceNode(attribute, translatedAttribute);
            }
            return attributeList;
        }

        public static AttributeSyntax TranslateAttribute(AttributeSyntax attribute)
        {
            string attributeName = Dictionary.TranslateAttributeName(attribute.Name.ToString());
            return attribute.WithName(SyntaxFactory.IdentifierName(attributeName));
        }
    }
}
