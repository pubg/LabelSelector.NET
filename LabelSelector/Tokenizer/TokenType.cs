namespace LabelSelector
{
    public enum TokenType
    {
        Unknown,
        Value, // else
        OpenParentheses, // (
        CloseParentheses, // )
        Equal, // =
        ExclamationEqual, // !=
        In, // in
        NotIn, // notIn
        Exclamation, // !
        Comma // ,
    }
}