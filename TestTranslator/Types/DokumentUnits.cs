namespace TestTranslator
{
    public enum documentUnit
    {
        Using,
        Namespace,
        Assertion,
        NonToken,

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

        CodeLineInClass        
    }
}
