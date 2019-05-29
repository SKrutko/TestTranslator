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
}
