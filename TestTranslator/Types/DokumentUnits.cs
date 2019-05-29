namespace TestTranslator
{
    public enum documentUnit
    {
        Using,
        Namespace,
        Assertion,

        ClassAttributeWithArgs,
        ClassAttributeWithoutArgs,
        TestAttributeWithArgs,
        TestAttributeWithoutArgs,

        ClassDeclaration,
        TestMethodDeclaration,

        OneLineComment,
        OneLineCommentAfterCode,
        MultipleLineComment,
        MultipleLineCommentAfterCode,

        CodeLine        
    }
}
