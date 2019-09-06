namespace LabelSelector
{
    public enum TokenType
    {
        Unknown,
        Value, // This Special.
        OpenParentheses, // (
        CloseParentheses, // )
        AmpersandAmpersand, // &&
        VerticalBarVerticalBar, // ||
        EqualEqual, // ==
        ExclamationEqual, // !=
        In, // in
        Notin, // notin
        OpenSquareBracket,// [
        CloseSquareBracket,// ]
        Exclamation, // !
        Comma // ,
    }
}