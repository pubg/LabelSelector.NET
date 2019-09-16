using System;
using System.Collections.Generic;

namespace LabelSelector
{
    public abstract class Token
    {
        public static readonly IReadOnlyDictionary<Type, TokenType> TokenTypeEnumMap = new Dictionary<Type, TokenType>
        {
            [typeof(ValueToken)] = TokenType.Value,
            [typeof(OpenParenthesesToken)] = TokenType.OpenParentheses,
            [typeof(CloseParenthesesToken)] = TokenType.CloseParentheses,
            [typeof(EqualToken)] = TokenType.Equal,
            [typeof(ExclamationEqualToken)] = TokenType.ExclamationEqual,
            [typeof(InToken)] = TokenType.In,
            [typeof(NotInToken)] = TokenType.NotIn,
            [typeof(ExclamationToken)] = TokenType.Exclamation,
            [typeof(CommaToken)] = TokenType.Comma,
        };
        public readonly TokenType TokenType;
        public readonly ReadOnlyMemory<char> Value;

        protected Token(TokenType tokenType, ReadOnlyMemory<char> value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public static Token NewToken(TokenType tokenType, ReadOnlyMemory<char> value)
        {
            switch (tokenType)
            {
                case TokenType.Value:
                    return new ValueToken(value);
                case TokenType.OpenParentheses:
                    return new OpenParenthesesToken(value);
                case TokenType.CloseParentheses:
                    return new CloseParenthesesToken(value);
                case TokenType.Equal:
                    return new EqualToken(value);
                case TokenType.ExclamationEqual:
                    return new ExclamationEqualToken(value);
                case TokenType.In:
                    return new InToken(value);
                case TokenType.NotIn:
                    return new NotInToken(value);
                case TokenType.Exclamation:
                    return new ExclamationToken(value);
                case TokenType.Comma:
                    return new CommaToken(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null);
            }
        }
        public override string ToString()
        {
            return $"[{TokenType}] : {Value}";
        }
    }
    public class ValueToken : Token
    {
        public ValueToken(ReadOnlyMemory<char> value) : base(TokenType.Value, value)
        {
        }
    }
    public class OpenParenthesesToken : Token
    {
        public OpenParenthesesToken(ReadOnlyMemory<char> value) : base(TokenType.OpenParentheses, value)
        {
        }
    }
    public class CloseParenthesesToken : Token
    {
        public CloseParenthesesToken(ReadOnlyMemory<char> value) : base(TokenType.CloseParentheses, value)
        {
        }
    }
    public class EqualToken : Token
    {
        public EqualToken(ReadOnlyMemory<char> value) : base(TokenType.Equal, value)
        {
        }
    }
    public class ExclamationEqualToken : Token
    {
        public ExclamationEqualToken(ReadOnlyMemory<char> value) : base(TokenType.ExclamationEqual, value)
        {
        }
    }
    public class InToken : Token
    {
        public InToken(ReadOnlyMemory<char> value) : base(TokenType.In, value)
        {
        }
    }
    public class NotInToken : Token
    {
        public NotInToken(ReadOnlyMemory<char> value) : base(TokenType.NotIn, value)
        {
        }
    }
    public class ExclamationToken : Token
    {
        public ExclamationToken(ReadOnlyMemory<char> value) : base(TokenType.Exclamation, value)
        {
        }
    }
    public class CommaToken : Token
    {
        public CommaToken(ReadOnlyMemory<char> value) : base(TokenType.Comma, value)
        {
        }
    }
}