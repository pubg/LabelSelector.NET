namespace LabelSelector
{
    public enum TokenType
    {
        Unknown,
        Value, // else
        OpenParentheses, // (
        CloseParentheses, // )
        EqualEqual, // ==
        ExclamationEqual, // !=
        In, // in
        Notin, // notin
        Exclamation, // !
        Comma // ,
    }
}