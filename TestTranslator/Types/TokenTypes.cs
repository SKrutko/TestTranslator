namespace TestTranslator
{
    public enum TokenType
    {
        Using,
        Namespace,
        ClassAttribute,
        Attribute,
        Assertion,
        NonToken
    }

    public enum ParserState
    {
        ExpectedUsingStatementOrNamespace,
        FoundUsingExpectedDirectoryName,
        FoundNamespaceExpectedName,
        FoundnamespaceNameExpectedLeftBrace, //expected {
        ExpectedClass,

        Error
    }
}
