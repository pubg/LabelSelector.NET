using System;
using System.Collections.Generic;
using System.Linq;

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

    public struct Token
    {
        public readonly TokenType TokenType;
        public readonly ReadOnlyMemory<char> Value;

        public Token(TokenType tokenType, ReadOnlyMemory<char> value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{TokenType}] : {Value}";
        }
    }
    public static class Tokenizer
    {
        private static readonly (TokenType tokenType, string matchString)[] MonopolyTokenDefinitions = new (TokenType tokenType, string matchString)[] {
            (TokenType.In, "in"),
            (TokenType.Notin, "notin"),
        };

        private static readonly (TokenType tokenType, string matchString)[] CoexistTokenDefinitions = new (TokenType tokenType, string matchString)[] {
            (TokenType.OpenParentheses, "("),
            (TokenType.CloseParentheses, ")"),
            (TokenType.Comma, ","),
            (TokenType.AmpersandAmpersand, "&&"),
            (TokenType.VerticalBarVerticalBar, "||"),
            (TokenType.EqualEqual, "=="),
            (TokenType.ExclamationEqual, "!="),
            (TokenType.OpenSquareBracket, "["),
            (TokenType.CloseSquareBracket, "]"),
            (TokenType.Exclamation, "!"),
        };


        public static IEnumerable<ReadOnlyMemory<char>> SplitByWhitespace(ReadOnlyMemory<char> text)
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

                yield return new Token(tokenType, memory);

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

                var monopolyToken = new Token(tokenType, memory);

                nextTokens = ToTokens(chunkedText.Slice(memory.Length)).ToList();
                nextFirstToken = nextTokens.FirstOrDefault();
                if (CoexistTokenDefinitions
                    .All(coexistTokenDefinition =>
                        coexistTokenDefinition.tokenType != nextFirstToken.TokenType))
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
                    var valueToken = new Token(TokenType.Value, mergedValue);

                    yield return valueToken;

                    foreach (var token in nextTokens.Skip(1))
                    {
                        yield return token;
                    }

                    yield break;
                }
                else
                {
                    yield return new Token(TokenType.Value, chunkedText.Slice(1));

                    foreach (var token in nextTokens)
                    {
                        yield return token;
                    }

                    yield break;
                }
            }

            yield return new Token(TokenType.Value, chunkedText);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var testTextes = new[] {
                "environment in (production, qa)",
                "tier notin (frontend, backend)",
                "partition",
                "!partition",
            };

            foreach (var text in testTextes)
            {
                Console.WriteLine($@"""{text}""");
                var result = Tokenizer.Tokenize(text);
                foreach (var token in result)
                {
                    Console.WriteLine(token);
                }

                Console.WriteLine();
            }
        }
    }
}
