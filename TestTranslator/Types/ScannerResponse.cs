namespace TestTranslator
{
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
