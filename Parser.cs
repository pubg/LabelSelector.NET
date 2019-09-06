using System;
using System.Collections.Generic;

namespace LabelSelector
{
    public static class Parser
    {
        public static Expression Parse(Token[] tokens)
        {
            var tokenConsumer = new TokenConsumer(tokens);
            return Parse(tokenConsumer);
        }
        private static Expression Parse(TokenConsumer tokenConsumer)
        {
            var firstToken = tokenConsumer.PeekToken();

            switch (firstToken.TokenType)
            {
                case TokenType.Value:
                    return ParseInFirstValueState(tokenConsumer);
                //case TokenType.OpenParentheses:
                //    break;
                //case TokenType.AmpersandAmpersand:
                //    break;
                //case TokenType.VerticalBarVerticalBar:
                //    break;
                case TokenType.Exclamation:
                    tokenConsumer.EatToken(TokenType.Exclamation);
                    return new NotExpression(Parse(tokenConsumer));
                default:
                    throw new Exception($"Not expected token {firstToken.TokenType}");
            }
        }

        public static Expression ParseInFirstValueState(TokenConsumer tokenConsumer)
        {
            var valueToken = tokenConsumer.EatToken<ValueToken>();

            if (!tokenConsumer.HasNext)
            {
                return new KeyExpression(valueToken);
            }

            var secondToken = tokenConsumer.PeekToken();

            switch (secondToken.TokenType)
            {
                //case TokenType.Value:
                //    break;
                //case TokenType.OpenParentheses:
                //    break;
                //case TokenType.CloseParentheses:
                //    break;
                //case TokenType.AmpersandAmpersand:
                //    break;
                //case TokenType.VerticalBarVerticalBar:
                //    break;
                //case TokenType.EqualEqual:
                //    break;
                //case TokenType.ExclamationEqual:
                //    break;
                case TokenType.In:
                    return ParseInExpression(tokenConsumer, valueToken);
                case TokenType.Notin:
                    return ParseNotinExpression(tokenConsumer, valueToken);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static InExpression ParseInExpression(TokenConsumer tokenConsumer, ValueToken valueToken)
        {
            tokenConsumer.EatToken(TokenType.In);

            var arraySyntax = ParseArraySyntax(tokenConsumer);

            return new InExpression(valueToken, arraySyntax);
        }
        public static NotinExpression ParseNotinExpression(TokenConsumer tokenConsumer, ValueToken valueToken)
        {
            tokenConsumer.EatToken(TokenType.Notin);

            var arraySyntax = ParseArraySyntax(tokenConsumer);

            return new NotinExpression(valueToken, arraySyntax);
        }

        public static ArraySyntax ParseArraySyntax(TokenConsumer tokenConsumer)
        {
            tokenConsumer.EatToken(TokenType.OpenParentheses);

            var valueTokens = new List<ValueToken>();
            while (true)
            {
                var valueToken = tokenConsumer.EatToken<ValueToken>();
                valueTokens.Add(valueToken);

                if (tokenConsumer.PeekToken().TokenType == TokenType.CloseParentheses)
                {
                    break;
                }

                tokenConsumer.EatToken(TokenType.Comma);
            }

            tokenConsumer.EatToken(TokenType.CloseParentheses);

            return new ArraySyntax(valueTokens);
        }
    }
}