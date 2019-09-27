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

            if (head < text.Length)
            {
                yield return text.Slice(head, text.Length - head);
            }
        }
        public static IEnumerable<Token> Tokenize(string text)
        {
            var chunks = SplitByWhitespace(text.AsMemory());

            return chunks.SelectMany(ToTokens);
        }

        internal enum TokenCategory
        {
            None,
            Coexist,
            Monopoly,
            Value,
        }

        private static bool TryTokenize(
            ReadOnlyMemory<char> noWhiteSpaceText,
            TokenCategory lastTokenCategory,
            out Token token,
            out TokenCategory tokenCategory,
            out ReadOnlyMemory<char> nextText)
        {
            switch (lastTokenCategory)
            {
                case TokenCategory.None:
                case TokenCategory.Coexist:
                    if (TryTokenizeAsMonopoly(noWhiteSpaceText, out token, out nextText))
                    {
                        tokenCategory = TokenCategory.Monopoly;
                        return true;
                    }
                    if (TryTokenizeAsCoexist(noWhiteSpaceText, out token, out nextText))
                    {
                        tokenCategory = TokenCategory.Coexist;
                        return true;
                    }
                    if (TryTokenizeAsValue(noWhiteSpaceText, out token, out nextText))
                    {
                        tokenCategory = TokenCategory.Value;
                        return true;
                    }
                    break;
                case TokenCategory.Monopoly:
                    if (TryTokenizeAsCoexist(noWhiteSpaceText, out token, out nextText))
                    {
                        tokenCategory = TokenCategory.Coexist;
                        return true;
                    }
                    break;
                case TokenCategory.Value:
                    if (TryTokenizeAsCoexist(noWhiteSpaceText, out token, out nextText))
                    {
                        tokenCategory = TokenCategory.Coexist;
                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lastTokenCategory), lastTokenCategory, null);
            }

            tokenCategory = TokenCategory.None;
            return false;
        }

        private static bool TryTokenizeAsValue(
            in ReadOnlyMemory<char> noWhiteSpaceText,
            out Token token,
            out ReadOnlyMemory<char> nextText)
        {

            if (noWhiteSpaceText.Length == 0)
            {
                token = default;
                nextText = default;
                return false;
            }

            for (var i = 0; i < noWhiteSpaceText.Length; i += 1)
            {
                nextText = noWhiteSpaceText.Slice(i);

                if (TryTokenizeAsCoexist(nextText, out _, out _))
                {
                    token = Token.NewToken(TokenType.Value, noWhiteSpaceText.Slice(0, i));
                    return true;
                }
            }

            token = Token.NewToken(TokenType.Value, noWhiteSpaceText);
            nextText = noWhiteSpaceText.Slice(noWhiteSpaceText.Length);
            return true;
        }

        private static bool TryTokenizeAsCoexist(
            in ReadOnlyMemory<char> noWhiteSpaceText,
            out Token token,
            out ReadOnlyMemory<char> nextText)
        {
            if (noWhiteSpaceText.Length == 0)
            {
                goto fail;
            }

            foreach ((TokenType tokenType, string matchString) in CoexistTokenDefinitions)
            {
                if (noWhiteSpaceText.Length < matchString.Length)
                {
                    continue;
                }

                var memory = noWhiteSpaceText.Slice(0, matchString.Length);

                if (!memory.Span.Equals(matchString, StringComparison.Ordinal))
                {
                    continue;
                }

                token = Token.NewToken(tokenType, memory);
                nextText = noWhiteSpaceText.Slice(matchString.Length);
                return true;
            }

            fail:
            token = default;
            nextText = default;
            return false;
        }

        private static bool TryTokenizeAsMonopoly(
            in ReadOnlyMemory<char> noWhiteSpaceText,
            out Token token,
            out ReadOnlyMemory<char> nextText)
        {
            if (noWhiteSpaceText.Length == 0)
            {
                goto fail;
            }

            foreach ((TokenType tokenType, string matchString) in MonopolyTokenDefinitions)
            {
                if (noWhiteSpaceText.Length < matchString.Length)
                {
                    continue;
                }

                var memory = noWhiteSpaceText.Slice(0, matchString.Length);

                if (!memory.Span.Equals(matchString, StringComparison.Ordinal))
                {
                    continue;
                }

                token = Token.NewToken(tokenType, memory);
                nextText = noWhiteSpaceText.Slice(matchString.Length);
                return true;
            }

            fail:
            token = default;
            nextText = default;
            return false;
        }

        private static IEnumerable<Token> ToTokens(ReadOnlyMemory<char> chunkedText)
        {
            var text = chunkedText;
            var lastTokenCategory = TokenCategory.None;

            while (text.Length > 0
                   && TryTokenize(text,
                       lastTokenCategory,
                       out var token,
                       out lastTokenCategory,
                       out text))
            {
                yield return token;
            }

            if (text.Length > 0)
            {
                throw new Exception($"text({chunkedText}) has not been fully tokenized");
            }
        }
    }
}