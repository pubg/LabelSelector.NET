using System;
using System.Collections.Generic;
using System.Linq;

namespace LabelSelector
{
    public static class Parser
    {
        public static IEnumerable<IExpression> Parse(string labelSelector)
        {
            var tokens = Tokenizer.Tokenize(labelSelector).ToArray();
            return Parse(tokens);
        }
        public static IEnumerable<IExpression> Parse(Token[] tokens)
        {
            var tokenConsumer = new TokenConsumer(tokens);
            return Parse(tokenConsumer);
        }
        private static IEnumerable<IExpression> Parse(TokenConsumer tokenConsumer)
        {
            while (tokenConsumer.HasToken)
            {
                var firstToken = tokenConsumer.PeekToken();

                switch (firstToken.TokenType)
                {
                    case TokenType.Value:
                        yield return ParseInFirstValueState(tokenConsumer);
                        break;
                    case TokenType.Exclamation:
                        yield return ParseNotExistsExpression(tokenConsumer);
                        break;
                    case TokenType.Comma:
                        tokenConsumer.EatToken(TokenType.Comma);
                        break;
                    default:
                        throw new Exception($"Not expected token {firstToken.TokenType}");
                }
            }
        }

        private static IExpression ParseNotExistsExpression(TokenConsumer tokenConsumer)
        {
            tokenConsumer.EatToken(TokenType.Exclamation);
            var valueToken = tokenConsumer.EatToken<ValueToken>();
            return new NotExistsExpression(valueToken.Value);
        }

        private static IExpression ParseInFirstValueState(TokenConsumer tokenConsumer)
        {
            var valueToken = tokenConsumer.EatToken<ValueToken>();

            if (!tokenConsumer.HasNextToken)
            {
                return new ExistsExpression(valueToken.Value);
            }

            var secondToken = tokenConsumer.PeekToken();

            switch (secondToken.TokenType)
            {
                case TokenType.In:
                    return ParseInExpression(tokenConsumer, valueToken);
                case TokenType.NotIn:
                    return ParseNotInExpression(tokenConsumer, valueToken);
                case TokenType.Comma:
                    tokenConsumer.EatToken(TokenType.Comma);
                    return new ExistsExpression(valueToken.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static InExpression ParseInExpression(TokenConsumer tokenConsumer, ValueToken valueToken)
        {
            tokenConsumer.EatToken(TokenType.In);

            var arraySyntax = ParseArraySyntax(tokenConsumer);

            return new InExpression(
                valueToken.Value,
                arraySyntax.Values.Select(value => value.Value));
        }
        private static NotInExpression ParseNotInExpression(TokenConsumer tokenConsumer, ValueToken valueToken)
        {
            tokenConsumer.EatToken(TokenType.NotIn);

            var arraySyntax = ParseArraySyntax(tokenConsumer);

            return new NotInExpression(
                valueToken.Value,
                arraySyntax.Values.Select(value => value.Value));
        }

        private static ArraySyntax ParseArraySyntax(TokenConsumer tokenConsumer)
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