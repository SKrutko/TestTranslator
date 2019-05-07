namespace TestTranslator
{
    public enum TokenType
    {
        Using,
        Namespace,

        ClassAttributeWithArg,
        ClassAttributeWithoutArg,

        TestAttributeWithArg,
        TestAttributeWithoutArg,

        AttributeWithArg,
        AttributeWithoutArg,

        ReturnType,
        Assertion,
        NonToken,
        Comment
    }

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

        CodeLineInClass,
        CodeLineInMethod
    }

    public enum ParserState
    {
        ExpectedUsingStatementOrNamespace,
        FoundUsingExpectedDirectoryName,
        FoundNamespaceExpectedName,
        FoundnamespaceNameExpectedLeftBrace, //expected {
        ExpectedClass,

        ExpectedClassAttribute,
        FoundClassAttrExpectedRightBracketOrComma,
        FoundClassAttrExpectedLeftParenthesis,
        FoundClassAttrExpectedRightParenthesis,

        ExpectedCWClass,//CW stands forCode Word
        FoundCWClassExpectedClassName,
        FoundClassNameExpectedLeftBrace,
        ExpectedCodeLineOrTestMethod,
        ExpectedContinuationOfCode,

        ExpectedTestAttribute,
        FoundTestAttrExpectedRightBracketOrComma,
        FoundTestAttrExpectedLeftParenthesis,
        FoundTestAttrExpectedRightParenthesis,

        FoundCWPublicExpectedReturnType,
        FoundReturnTypeExpectedMethodName,
        FoundMethodNameExpectedLeftParenthesis,
        FoundMethodDeclarationExpectedRightParenthesis,
        FoundMethodDeclarationExpectedLeftBrace,
        ExpectedCodeLineOrAssertion,

        FoundAssertionExpectedDot,
        FoundAssertionExpectedMethod, //found cw assert, collectionAssert or other
        FoundAssertionExpectedLeftParenthesis,//found method expected (
        FoundAssertionExpectedRightParenthesis,//found (, expected ), everything in between is seen as arguments
        FoundAssertionExpectedEndl,// expected ;




        OneLineComment,
        MultipleLineComment,

        Error,
        ExpectedTestMethod
    }

    public struct scannerResponse // used in scanner to give information if there is space BEFORE the token
    {
        public string token;
        public bool space;

        public scannerResponse(string token, bool space)
        {
            this.token = token;
            this.space = space;
        }
    }
}
