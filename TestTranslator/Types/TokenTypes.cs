namespace TestTranslator
{
    public enum TokenType
    {
        Using,
        Namespace,
        ClassAttribute,
        Attribute,
        Assertion,
        NonToken,
        Comment
    }

    public enum documentUnit
    {
        Using,
        Namespace,
        ClassAttribute,
        Attribute,
        Assertion,
        NonToken,
        OneLineComment,
        OneLineCommentAfterCode,
        MultipleLineComment,
        MultipleLineCommentAfterCode,
    }

    public enum ParserState
    {
        ExpectedUsingStatementOrNamespace,
        FoundUsingExpectedDirectoryName,
        FoundNamespaceExpectedName,
        FoundnamespaceNameExpectedLeftBrace, //expected {
        ExpectedClass,
        ExpectedCWClass,//Code Word
        FoundCWClassExpectedClassName,
        FoundClassNameExpectedLeftBrace,

        OneLineComment,
        MultipleLineComment,

        Error
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
