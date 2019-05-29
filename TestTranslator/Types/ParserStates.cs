namespace TestTranslator
{
    public enum ParserState
    {
        ExpectedUsingStatementOrNamespace,
        FoundUsingExpectedDirectoryName,
        FoundNamespaceExpectedName,
        ExpectedClass,

        ExpectedClassAttribute,
        FoundClassAttrExpectedRightBracketOrComma,
        FoundClassAttrExpectedLeftParenthesis,
        FoundClassAttrExpectedRightParenthesis,

        ExpectedCWClass,//CW stands for Code Word
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

}
