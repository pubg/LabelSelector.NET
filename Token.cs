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
            [typeof(AmpersandAmpersandToken)] = TokenType.AmpersandAmpersand,
            [typeof(VerticalBarVerticalBarToken)] = TokenType.VerticalBarVerticalBar,
            [typeof(EqualEqualToken)] = TokenType.EqualEqual,
            [typeof(ExclamationEqualToken)] = TokenType.ExclamationEqual,
            [typeof(InToken)] = TokenType.In,
            [typeof(NotinToken)] = TokenType.Notin,
            [typeof(OpenSquareBracketToken)] = TokenType.OpenSquareBracket,
            [typeof(CloseSquareBracketToken)] = TokenType.CloseSquareBracket,
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
                case TokenType.AmpersandAmpersand:
                    return new AmpersandAmpersandToken(value);
                case TokenType.VerticalBarVerticalBar:
                    return new VerticalBarVerticalBarToken(value);
                case TokenType.EqualEqual:
                    return new EqualEqualToken(value);
                case TokenType.ExclamationEqual:
                    return new ExclamationEqualToken(value);
                case TokenType.In:
                    return new InToken(value);
                case TokenType.Notin:
                    return new NotinToken(value);
                case TokenType.OpenSquareBracket:
                    return new OpenSquareBracketToken(value);
                case TokenType.CloseSquareBracket:
                    return new CloseSquareBracketToken(value);
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
    public class AmpersandAmpersandToken : Token
    {
        public AmpersandAmpersandToken(ReadOnlyMemory<char> value) : base(TokenType.AmpersandAmpersand, value)
        {
        }
    }
    public class VerticalBarVerticalBarToken : Token
    {
        public VerticalBarVerticalBarToken(ReadOnlyMemory<char> value) : base(TokenType.VerticalBarVerticalBar, value)
        {
        }
    }
    public class EqualEqualToken : Token
    {
        public EqualEqualToken(ReadOnlyMemory<char> value) : base(TokenType.EqualEqual, value)
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
    public class NotinToken : Token
    {
        public NotinToken(ReadOnlyMemory<char> value) : base(TokenType.Notin, value)
        {
        }
    }
    public class OpenSquareBracketToken : Token
    {
        public OpenSquareBracketToken(ReadOnlyMemory<char> value) : base(TokenType.OpenSquareBracket, value)
        {
        }
    }
    public class CloseSquareBracketToken : Token
    {
        public CloseSquareBracketToken(ReadOnlyMemory<char> value) : base(TokenType.CloseSquareBracket, value)
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