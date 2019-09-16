using System;
using System.Collections.Generic;
using System.Linq;

namespace LabelSelector
{
    public static class Tokenizer
    {
        private static readonly (TokenType tokenType, string matchString)[] MonopolyTokenDefinitions = {
            (TokenType.In, "in"),
            (TokenType.NotIn, "notin"),
        };

        private static readonly (TokenType tokenType, string matchString)[] CoexistTokenDefinitions = {
            (TokenType.OpenParentheses, "("),
            (TokenType.CloseParentheses, ")"),
            (TokenType.Comma, ","),
            (TokenType.Equal, "="),
            (TokenType.ExclamationEqual, "!="),
            (TokenType.Exclamation, "!"),
        };


        private static IEnumerable<ReadOnlyMemory<char>> SplitByWhitespace(ReadOnlyMemory<char> text)
        {
            int head = 0;
            for (var i = 0; i < text.Length; i += 1)
            {
                if (!text.Slice(i, 1).Span.IsWhiteSpace())
                {
                    continue;
                }
                if (head < i)
                {
                    yield return text.Slice(head, i - head);
                }
                head = i + 1;
            }

            if (head < text.Length - 1)
            {
                yield return text.Slice(head, text.Length - head);
            }
        }
        public static IEnumerable<Token> Tokenize(string text)
        {
            var chunks = SplitByWhitespace(text.AsMemory());

            return chunks.SelectMany(ToTokens);
        }

        private static IEnumerable<Token> ToTokens(ReadOnlyMemory<char> chunkedText)
        {
            if (chunkedText.Length == 0)
            {
                yield break;
            }

            foreach ((TokenType tokenType, string matchString) in CoexistTokenDefinitions)
            {
                if (chunkedText.Length < matchString.Length)
                {
                    continue;
                }

                var memory = chunkedText.Slice(0, matchString.Length);

                if (!memory.Span.Equals(matchString, StringComparison.Ordinal))
                {
                    continue;
                }

                yield return Token.NewToken(tokenType, memory);

                foreach (var token in ToTokens(chunkedText.Slice(memory.Length)))
                {
                    yield return token;
                }

                yield break;
            }

            List<Token> nextTokens;
            Token nextFirstToken;

            foreach ((TokenType tokenType, string matchString) in MonopolyTokenDefinitions)
            {
                if (chunkedText.Length < matchString.Length)
                {
                    continue;
                }

                var memory = chunkedText.Slice(0, matchString.Length);

                if (!memory.Span.Equals(matchString, StringComparison.Ordinal))
                {
                    continue;
                }

                var monopolyToken = Token.NewToken(tokenType, memory);

                nextTokens = ToTokens(chunkedText.Slice(memory.Length)).ToList();
                if (nextTokens.Count != 0
                    && CoexistTokenDefinitions
                        .All(coexistTokenDefinition =>
                            coexistTokenDefinition.tokenType != nextTokens[0].TokenType))
                {
                    continue;
                }

                yield return monopolyToken;

                foreach (var token in nextTokens)
                {
                    yield return token;
                }

                yield break;
            }

            nextTokens = ToTokens(chunkedText.Slice(1)).ToList();
            if (nextTokens.Any())
            {
                nextFirstToken = nextTokens[0];
                if (nextFirstToken.TokenType == TokenType.Value)
                {
                    var mergedValue = chunkedText.Slice(0, nextFirstToken.Value.Length + 1);
                    var valueToken = Token.NewToken(TokenType.Value, mergedValue);

                    yield return valueToken;

                    foreach (var token in nextTokens.Skip(1))
                    {
                        yield return token;
                    }

                    yield break;
                }
                else
                {
                    yield return Token.NewToken(TokenType.Value, chunkedText.Slice(0, 1));

                    foreach (var token in nextTokens)
                    {
                        yield return token;
                    }

                    yield break;
                }
            }

            yield return Token.NewToken(TokenType.Value, chunkedText);
        }
    }
}